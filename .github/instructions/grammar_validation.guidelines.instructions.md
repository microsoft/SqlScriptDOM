# Validation-Based Bug Fix Guide for SqlScriptDOM

This guide covers bugs where the **grammar already supports the syntax**, but the parser incorrectly rejects it due to validation logic. This is different from grammar-level fixes where you need to add new parsing rules.

## When to Use This Guide

Use this pattern when:
- ✅ The syntax **should** parse based on SQL Server documentation
- ✅ The error message is a **validation error** (e.g., "SQL46057: Option 'X' is not valid...")
- ✅ Similar syntax works in **other contexts** (e.g., ALTER INDEX works but ALTER TABLE fails)
- ✅ The feature was **added in a newer SQL Server version** but is rejected even in the correct parser

**Do NOT use this guide when:**
- ❌ Grammar rules need to be added/modified (use [bug_fixing.guidelines.instructions.md](bug_fixing.guidelines.instructions.md) instead)
- ❌ AST nodes need to be created (use [grammer.guidelines.instructions.md](grammer.guidelines.instructions.md))
- ❌ The syntax never existed in SQL Server

## Real-World Example: ALTER TABLE RESUMABLE Option

### The Problem

User reported this SQL failed to parse:
```sql
ALTER TABLE table1 
ADD CONSTRAINT PK_Constraint PRIMARY KEY CLUSTERED (a) 
WITH (ONLINE = ON, MAXDOP = 2, RESUMABLE = ON, MAX_DURATION = 240);
```

**Error**: `SQL46057: Option 'RESUMABLE' is not a valid index option in 'ALTER TABLE' statement.`

**But**: The same options worked fine in `ALTER INDEX` statements.

### Investigation Steps

#### 1. Search for the Error Message
```bash
# Search for the error code or message text
grep -r "SQL46057" SqlScriptDom/
grep -r "is not a valid index option" SqlScriptDom/
```

**Result**: Found in `TSql80ParserBaseInternal.cs` in the `VerifyAllowedIndexOption()` method.

#### 2. Examine the Validation Logic
```csharp
// Location: SqlScriptDom/Parser/TSql/TSql80ParserBaseInternal.cs
protected void VerifyAllowedIndexOption(IndexAffectingStatement statement, 
                                        IndexOption option, 
                                        SqlVersionFlags versionFlags)
{
    switch (statement)
    {
        case IndexAffectingStatement.AlterTableAddElement:
            // BEFORE: Unconditionally blocked RESUMABLE and MAX_DURATION
            if (option.OptionKind == IndexOptionKind.Resumable || 
                option.OptionKind == IndexOptionKind.MaxDuration)
            {
                ThrowParseErrorException("SQL46057", /* ... */);
            }
            break;
        // ... other cases ...
    }
}
```

**Key Finding**: The validation was **hardcoded** to reject these options for ALTER TABLE, regardless of SQL Server version.

#### 3. Check Microsoft Documentation
Always verify the **exact SQL Server version** support:
- **RESUMABLE**: Introduced in SQL Server 2022 (version 160)
- **MAX_DURATION**: Introduced in SQL Server 2014 (version 120) for low-priority locks, extended for resumable operations

**Important**: Different options can have different version requirements even within the same feature set!

### The Fix

#### Step 1: Identify Version Flags

The codebase uses `SqlVersionFlags` for version checking:
- `TSql80AndAbove` = SQL Server 2000+
- `TSql90AndAbove` = SQL Server 2005+
- `TSql100AndAbove` = SQL Server 2008+
- `TSql110AndAbove` = SQL Server 2012+
- `TSql120AndAbove` = SQL Server 2014+
- `TSql130AndAbove` = SQL Server 2016+
- `TSql140AndAbove` = SQL Server 2017+
- `TSql150AndAbove` = SQL Server 2019+
- `TSql160AndAbove` = SQL Server 2022+
- `TSql170AndAbove` = SQL Server 2025+

#### Step 2: Apply Version-Gated Validation

Replace unconditional rejection with version checking:

```csharp
case IndexAffectingStatement.AlterTableAddElement:
    // Invalidate RESUMABLE for versions before SQL Server 2022 (160)
    // Invalidate MAX_DURATION for versions before SQL Server 2014 (120)
    if (((versionFlags & SqlVersionFlags.TSql160AndAbove) == 0 && 
         option.OptionKind == IndexOptionKind.Resumable) ||
        ((versionFlags & SqlVersionFlags.TSql120AndAbove) == 0 && 
         option.OptionKind == IndexOptionKind.MaxDuration))
    {
        // Throw an error indicating the option is not supported in the current SQL Server version
        ThrowParseErrorException("SQL46057", "Option not supported in this SQL Server version.");
    }
    break;
```

**Pattern Explanation**:
- `(versionFlags & SqlVersionFlags.TSql160AndAbove) == 0` → Returns true if parser version < 160
- If true AND option is RESUMABLE → Throw error (option not supported yet)
- Same pattern for MAX_DURATION with TSql120AndAbove

#### Step 3: Create Comprehensive Tests

**Test Script**: `Test/SqlDom/TestScripts/AlterTableResumableTests160.sql`
```sql
-- Test 1: RESUMABLE with MAX_DURATION (minutes)
ALTER TABLE dbo.MyTable 
ADD CONSTRAINT pk_test PRIMARY KEY CLUSTERED (id)
WITH (RESUMABLE = ON, MAX_DURATION = 240 MINUTES);

-- Test 2: RESUMABLE = ON
ALTER TABLE dbo.MyTable
ADD CONSTRAINT pk_test PRIMARY KEY CLUSTERED (id)
WITH (RESUMABLE = ON);

-- Test 3: RESUMABLE = OFF
ALTER TABLE dbo.MyTable
ADD CONSTRAINT pk_test PRIMARY KEY CLUSTERED (id)
WITH (RESUMABLE = OFF);

-- Test 4: UNIQUE constraint with RESUMABLE
ALTER TABLE dbo.MyTable
ADD CONSTRAINT uq_test UNIQUE NONCLUSTERED (name)
WITH (RESUMABLE = ON);
```

#### Step 4: Configure Test Expectations

**Test Configuration**: `Test/SqlDom/Only160SyntaxTests.cs`
```csharp
new ParserTest160("AlterTableResumableTests160.sql"),
```

#### Step 5: Create Baseline Files

**Baseline**: `Test/SqlDom/Baselines160/AlterTableResumableTests160.sql`
```sql
ALTER TABLE dbo.MyTable
    ADD CONSTRAINT pk_test PRIMARY KEY CLUSTERED (id) WITH (RESUMABLE = ON, MAX_DURATION = 240 MINUTES);

ALTER TABLE dbo.MyTable
    ADD CONSTRAINT pk_test PRIMARY KEY CLUSTERED (id) WITH (RESUMABLE = ON);

ALTER TABLE dbo.MyTable
    ADD CONSTRAINT pk_test PRIMARY KEY CLUSTERED (id) WITH (RESUMABLE = OFF);

ALTER TABLE dbo.MyTable
    ADD CONSTRAINT uq_test UNIQUE NONCLUSTERED (name) WITH (RESUMABLE = ON);
```

#### Step 6: Validate the Fix

```bash
# Build to ensure code compiles
dotnet build SqlScriptDom/Microsoft.SqlServer.TransactSql.ScriptDom.csproj -c Debug

# Run specific test
dotnet test --filter "FullyQualifiedName~AlterTableResumableTests" -c Debug

# Run FULL test suite to catch regressions
dotnet test Test/SqlDom/UTSqlScriptDom.csproj -c Debug
```

**Expected Results**:
- ✅ TSql160Parser: 0 errors (all tests pass)
- ✅ TSql80-150Parsers: 4 errors each (RESUMABLE correctly rejected)
- ✅ Full suite: 1,116 tests passed, 0 failed

## Common Validation Patterns

### Pattern 1: Version-Gated Validation
```csharp
// Allow feature only in specific SQL Server versions
if ((versionFlags & SqlVersionFlags.TSqlXXXAndAbove) == 0 && 
    condition)
{
    ThrowParseErrorException(...);
}
```

### Pattern 2: Multiple Version Requirements
```csharp
// Different features with different version requirements
if (((versionFlags & SqlVersionFlags.TSql160AndAbove) == 0 && feature1) ||
    ((versionFlags & SqlVersionFlags.TSql120AndAbove) == 0 && feature2))
{
    ThrowParseErrorException(...);
}
```

### Pattern 3: Context-Specific Validation
```csharp
// Same option, different rules for different statements
switch (statement)
{
    case IndexAffectingStatement.AlterTableAddElement:
        // Stricter rules for ALTER TABLE
        break;
    case IndexAffectingStatement.CreateIndex:
        // More permissive for CREATE INDEX
        break;
}
```

## Key Files for Validation Fixes

### 1. Validation Logic
- **`SqlScriptDom/Parser/TSql/TSql80ParserBaseInternal.cs`**
  - Base validation shared by all parser versions
  - Contains `VerifyAllowedIndexOption()`, `VerifyAllowedIndexType()`, etc.
  - Most validation fixes happen here

### 2. Version-Specific Overrides
- **`SqlScriptDom/Parser/TSql/TSql160ParserBaseInternal.cs`**
  - Can override base validation for specific versions
  - Example: `VerifyAllowedIndexOption160()` calls base then adds version-specific logic

### 3. Option Registration
- **`SqlScriptDom/ScriptDom/SqlServer/IndexOptionHelper.cs`**
  - Maps option keywords to `IndexOptionKind` enum values
  - Defines version support: `AddOptionMapping(kind, keyword, versionFlags)`
  - **Note**: Registration here controls grammar acceptance, validation happens separately

### 4. Enums and Constants
- **`SqlScriptDom/ScriptDom/SqlServer/IndexAffectingStatement.cs`**
  - Defines statement types: `CreateIndex`, `AlterIndex`, `AlterTableAddElement`, etc.
  
- **`SqlScriptDom/ScriptDom/SqlServer/IndexOptionKind.cs`**
  - Defines option types: `Resumable`, `MaxDuration`, `Online`, etc.

## Debugging Workflow

### Step 1: Reproduce the Error
```bash
# Create a minimal test file
echo "ALTER TABLE t ADD CONSTRAINT pk PRIMARY KEY (id) WITH (RESUMABLE = ON);" > test.sql

# Try parsing it (will fail)
# Use your test harness or create a simple parser test
```

### Step 2: Find the Error Source
```bash
# Search for error code
grep -r "SQL46057" SqlScriptDom/

# Search for error message text
grep -r "is not a valid" SqlScriptDom/
```

### Step 3: Locate Validation Function
Common validation functions to check:
- `VerifyAllowedIndexOption()` - Most common
- `VerifyAllowedIndexType()`
- `VerifyFeatureSupport()`
- `CheckFeatureAvailability()`

### Step 4: Examine the Logic
Look for:
- Hardcoded rejections (unconditional throws)
- Version checks that are too strict
- Missing version flag checks
- Incorrect version constants

### Step 5: Check Similar Working Cases
If ALTER INDEX works but ALTER TABLE doesn't:
- Compare their validation paths
- Check for different `switch` cases
- Look for statement-type specific logic

## Testing Strategy

### Test Coverage Checklist
- [ ] Test with option enabled (`OPTION = ON`)
- [ ] Test with option disabled (`OPTION = OFF`)
- [ ] Test with option + other options (`OPTION = ON, OTHER_OPTION = value`)
- [ ] Test different statement types (PRIMARY KEY, UNIQUE, etc.)
- [ ] Test across all SQL Server versions (verify error counts)

### Version-Specific Error Expectations
```csharp
// Pattern for test configuration
new ParserTestXXX("TestFile.sql",
    nErrors80: X,   // Count errors for SQL 2000
    nErrors90: X,   // Count errors for SQL 2005
    nErrors100: X,  // Count errors for SQL 2008
    nErrors110: X,  // Count errors for SQL 2012
    nErrors120: Y,  // May differ if feature added in 2014
    nErrors130: Y,  // Same as above
    nErrors140: Y,  // Same as above
    nErrors150: Y,  // Same as above
    // nErrors160: 0 (implicit) - Feature supported in 2022+
)
```

## Common Pitfalls

### 1. Assuming Same Version for Related Features
❌ **Wrong**: "RESUMABLE and MAX_DURATION are both resumable features, so both need TSql160+"
✅ **Correct**: Check documentation - MAX_DURATION existed before RESUMABLE (TSql120 vs TSql160)

### 2. Not Running Full Test Suite
❌ **Wrong**: Only run the new test, assume it's fine
✅ **Correct**: Run ALL tests - validation changes can affect unexpected areas

### 3. Incorrect Version Flag Logic
❌ **Wrong**: `if (versionFlags & SqlVersionFlags.TSql160AndAbove)` (missing == 0)
✅ **Correct**: `if ((versionFlags & SqlVersionFlags.TSql160AndAbove) == 0)` (check if NOT set)

### 4. Forgetting Statement Context
❌ **Wrong**: Apply same validation to all statement types
✅ **Correct**: Different statements may have different option support

## Summary Checklist

- [ ] **Identify** the validation function throwing the error
- [ ] **Verify** Microsoft documentation for exact version support
- [ ] **Apply** version-gated validation (not unconditional rejection)
- [ ] **Create** comprehensive test cases covering all scenarios
- [ ] **Configure** test expectations for all SQL Server versions
- [ ] **Generate** baseline files from actual parser output
- [ ] **Build** the ScriptDOM project successfully
- [ ] **Run** full test suite (ALL 1,100+ tests must pass)
- [ ] **Document** the fix with clear before/after examples

## Related Guides

- [bug_fixing.guidelines.instructions.md](bug_fixing.guidelines.instructions.md) - For grammar-level fixes
- [grammer.guidelines.instructions.md](grammer.guidelines.instructions.md) - For extending existing grammar
- [parser.guidelines.instructions.md](parser.guidelines.instructions.md) - For parentheses recognition issues

## Real-World Examples

### Example 1: ALTER TABLE RESUMABLE (SQL Server 2022)
- **File**: `TSql80ParserBaseInternal.cs`
- **Function**: `VerifyAllowedIndexOption()`
- **Fix**: Added `TSql160AndAbove` check for RESUMABLE
- **Tests**: `AlterTableResumableTests160.sql`

### Example 2: MAX_DURATION (SQL Server 2014)
- **File**: Same as above
- **Function**: Same as above  
- **Fix**: Added `TSql120AndAbove` check for MAX_DURATION
- **Tests**: Same file, different version expectations

These examples demonstrate how validation fixes are often simpler than grammar changes - the parser already knows how to parse the syntax, it just needs permission to accept it in specific contexts and versions.
