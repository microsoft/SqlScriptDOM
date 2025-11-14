---
title: How to Verify T-SQL Syntax Support and Add Tests
description: Step-by-step guide to check if a T-SQL syntax is already supported and how to add comprehensive test coverage
tags: [testing, verification, tsql, syntax, parser, baseline]
---

# How to Verify T-SQL Syntax Support and Add Tests

This guide helps you determine if a T-SQL syntax is already supported by ScriptDOM and shows you how to add proper test coverage.

## Step 1: Determine the SQL Server Version

First, identify which SQL Server version introduced the syntax you want to test.

### SQL Server Version Mapping

| SQL Server Version | Parser Version | Year | Common Name |
|-------------------|----------------|------|-------------|
| SQL Server 2000 | TSql80 | 2000 | SQL Server 2000 |
| SQL Server 2005 | TSql90 | 2005 | SQL Server 2005 |
| SQL Server 2008 | TSql100 | 2008 | SQL Server 2008 |
| SQL Server 2012 | TSql110 | 2012 | SQL Server 2012 |
| SQL Server 2014 | TSql120 | 2014 | SQL Server 2014 |
| SQL Server 2016 | TSql130 | 2016 | SQL Server 2016 |
| SQL Server 2017 | TSql140 | 2017 | SQL Server 2017 |
| SQL Server 2019 | TSql150 | 2019 | SQL Server 2019 |
| SQL Server 2022 | TSql160 | 2022 | SQL Server 2022 |
| SQL Server 2025 | TSql170 | 2025 | SQL Server 2025 |

### How to Find the Version

1. **Check Microsoft Documentation**: Look for "Applies to: SQL Server 20XX (XX.x)"
2. **Search Online**: Look for the feature announcement or blog posts
3. **Test in SSMS**: Connect to different SQL Server versions and try the syntax

**Example**: 
- `RESUMABLE = ON` for ALTER TABLE → SQL Server 2022 → **TSql160**
- `MAX_DURATION` for indexes → SQL Server 2014 → **TSql120**
- `VECTOR_SEARCH` function → SQL Server 2025 → **TSql170**

## Step 2: Check if Syntax is Already Supported

### Method 1: Search Test Scripts (Fastest)
```bash
# Search for the keyword in test scripts
grep -r "YOUR_KEYWORD" Test/SqlDom/TestScripts/

# Example: Check if RESUMABLE is tested for ALTER TABLE
grep -r "RESUMABLE" Test/SqlDom/TestScripts/*.sql

# Search in specific version test files
grep -r "RESUMABLE" Test/SqlDom/TestScripts/*160.sql
```

### Method 2: Search Grammar Files
```bash
# Search in grammar files
grep -r "YOUR_KEYWORD" SqlScriptDom/Parser/TSql/*.g

# Example: Check if RESUMABLE is in grammar
grep -r "Resumable" SqlScriptDom/Parser/TSql/TSql160.g
```

### Method 3: Search AST Definitions
```bash
# Search in AST XML
grep -r "YourFeatureName" SqlScriptDom/Parser/TSql/Ast.xml

# Example: Check for VECTOR_SEARCH node
grep -r "VectorSearch" SqlScriptDom/Parser/TSql/Ast.xml
```

### Method 4: Try Parsing with Test Script
Create a minimal test file and try parsing:

```bash
# Create test SQL file
echo "ALTER TABLE t ADD CONSTRAINT pk PRIMARY KEY (id) WITH (RESUMABLE = ON);" > test_syntax.sql

# Build the parser
dotnet build SqlScriptDom/Microsoft.SqlServer.TransactSql.ScriptDom.csproj -c Debug

# Run your test (you'll need to create a simple parser test or use existing test framework)
```

## Step 3: Create a Test Script

### Test File Naming Convention
- Format: `DescriptiveFeatureName{Version}.sql`
- Example: `AlterTableResumableTests160.sql` (for SQL Server 2022/TSql160)
- Location: `Test/SqlDom/TestScripts/`

### Test Script Template

```sql
-- Test 1: Basic syntax
ALTER TABLE dbo.MyTable 
ADD CONSTRAINT pk_test PRIMARY KEY CLUSTERED (id)
WITH (YOUR_OPTION = value);

-- Test 2: With multiple options
ALTER TABLE dbo.MyTable
ADD CONSTRAINT pk_test PRIMARY KEY CLUSTERED (id)
WITH (ONLINE = ON, YOUR_OPTION = value);

-- Test 3: Different statement variations
ALTER TABLE dbo.MyTable
ADD CONSTRAINT uq_test UNIQUE NONCLUSTERED (name)
WITH (YOUR_OPTION = value);

-- Test 4: With parameters (if applicable)
ALTER TABLE dbo.MyTable
ADD CONSTRAINT pk_test PRIMARY KEY CLUSTERED (id)
WITH (YOUR_OPTION = @parameter);

-- Test 5: Complex scenario
ALTER TABLE dbo.MyTable
ADD CONSTRAINT pk_test PRIMARY KEY CLUSTERED (id, name)
WITH (YOUR_OPTION = value, OTHER_OPTION = 100 MINUTES);
```

### Real-World Example: ALTER TABLE RESUMABLE

**File**: `Test/SqlDom/TestScripts/AlterTableResumableTests160.sql`

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

## Step 4: Create Test Configuration

### Test Configuration File Location
- Format: `Only{Version}SyntaxTests.cs`
- Example: `Only160SyntaxTests.cs` (for SQL Server 2022)
- Location: `Test/SqlDom/`

### Test Configuration Template

```csharp
new ParserTest{Version}("YourTestFile{Version}.sql",
    nErrors80: X,   // SQL Server 2000 - usually errors if new feature
    nErrors90: X,   // SQL Server 2005
    nErrors100: X,  // SQL Server 2008
    nErrors110: X,  // SQL Server 2012
    nErrors120: Y,  // SQL Server 2014 - may differ if partially supported
    nErrors130: Y,  // SQL Server 2016
    nErrors140: Y,  // SQL Server 2017
    nErrors150: Y,  // SQL Server 2019
    // nErrors{Version}: 0 - The version where feature is supported (default 0)
),
```

### How to Determine Error Counts

**Rule**: Count the number of SQL statements that will fail in each version.

**Example**: If your test file has 4 statements with the new feature:
- Versions that DON'T support it: `nErrors = 4` (all 4 statements fail)
- Version that DOES support it: `nErrors = 0` (all 4 statements pass, implicit default)

### Real-World Example: ALTER TABLE RESUMABLE

**File**: `Test/SqlDom/Only160SyntaxTests.cs`

```csharp
new ParserTest160("AlterTableResumableTests160.sql",
    nErrors80: 4,   // SQL Server 2000: RESUMABLE not supported (4 errors)
    nErrors90: 4,   // SQL Server 2005: RESUMABLE not supported (4 errors)
    nErrors100: 4,  // SQL Server 2008: RESUMABLE not supported (4 errors)
    nErrors110: 4,  // SQL Server 2012: RESUMABLE not supported (4 errors)
    nErrors120: 4,  // SQL Server 2014: RESUMABLE not supported (4 errors)
    nErrors130: 4,  // SQL Server 2016: RESUMABLE not supported (4 errors)
    nErrors140: 4,  // SQL Server 2017: RESUMABLE not supported (4 errors)
    nErrors150: 4   // SQL Server 2019: RESUMABLE not supported (4 errors)
    // nErrors160: 0 (implicit) - SQL Server 2022: RESUMABLE supported! (0 errors)
),
```

## Step 5: Generate Baseline Files

Baseline files contain the expected formatted output after parsing and script generation.

### Baseline File Location
- Format: `Baselines{Version}/YourTestFile{Version}.sql`
- Example: `Baselines160/AlterTableResumableTests160.sql`
- Location: `Test/SqlDom/`

### Baseline Generation Process

#### Option A: Automatic Generation (Recommended)

```bash
# 1. Create empty baseline file first
New-Item "Test/SqlDom/Baselines160/AlterTableResumableTests160.sql" -ItemType File

# 2. Run the test (it will fail, showing the generated output)
dotnet test --filter "FullyQualifiedName~AlterTableResumableTests160" -c Debug

# 3. Copy the "Actual" output from test failure into the baseline file
# Look for the section that says:
# Expected: <empty or old content>
# Actual: <This is what you need to copy>

# 4. Re-run the test (should pass now)
dotnet test --filter "FullyQualifiedName~AlterTableResumableTests160" -c Debug
```

#### Option B: Manual Creation

Create the baseline file with properly formatted SQL:

```sql
-- Baseline follows ScriptDOM formatting rules:
-- - Keywords in UPPERCASE
-- - Proper indentation
-- - Line breaks at appropriate places

ALTER TABLE dbo.MyTable
    ADD CONSTRAINT pk_test PRIMARY KEY CLUSTERED (id) WITH (RESUMABLE = ON, MAX_DURATION = 240 MINUTES);

ALTER TABLE dbo.MyTable
    ADD CONSTRAINT pk_test PRIMARY KEY CLUSTERED (id) WITH (RESUMABLE = ON);

ALTER TABLE dbo.MyTable
    ADD CONSTRAINT pk_test PRIMARY KEY CLUSTERED (id) WITH (RESUMABLE = OFF);

ALTER TABLE dbo.MyTable
    ADD CONSTRAINT uq_test UNIQUE NONCLUSTERED (name) WITH (RESUMABLE = ON);
```

### Real-World Example: ALTER TABLE RESUMABLE Baseline

**File**: `Test/SqlDom/Baselines160/AlterTableResumableTests160.sql`

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

## Step 6: Run and Validate Tests

### Build the Parser
```bash
# Build ScriptDOM library
dotnet build SqlScriptDom/Microsoft.SqlServer.TransactSql.ScriptDom.csproj -c Debug

# Build test project
dotnet build Test/SqlDom/UTSqlScriptDom.csproj -c Debug
```

### Run Your Specific Test
```bash
# Run by test name filter
dotnet test --filter "FullyQualifiedName~YourTestName" -c Debug

# Example: Run ALTER TABLE RESUMABLE tests
dotnet test --filter "FullyQualifiedName~AlterTableResumableTests" -c Debug

# Run with verbose output to see details
dotnet test --filter "FullyQualifiedName~YourTestName" -c Debug -v detailed
```

### Run Full Test Suite (CRITICAL!)
```bash
# Always run ALL tests before committing
dotnet test Test/SqlDom/UTSqlScriptDom.csproj -c Debug

# Expected output:
# Test summary: total: 1116, failed: 0, succeeded: 1116, skipped: 0
```

### Understanding Test Results

✅ **Success**: All tests pass, including your new test
```
Test summary: total: 1120, failed: 0, succeeded: 1120, skipped: 0
```

❌ **Baseline Mismatch**: Generated output doesn't match baseline
```
Expected: <baseline content>
Actual: <generated content>
```
**Fix**: Update baseline file with the "Actual" content

❌ **Parsing Error**: Syntax not recognized or validation failed
```
Error: SQL46057: Option 'X' is not a valid option...
```
**Fix**: Grammar or validation needs to be updated (see other guides)

❌ **Regression**: Existing tests now fail
```
Test summary: total: 1120, failed: 5, succeeded: 1115, skipped: 0
```
**Fix**: Your change broke existing functionality - review and fix

## Complete Example Workflow

### Example: Testing ALTER TABLE RESUMABLE for SQL Server 2022

```bash
# Step 1: Determine version
# Research shows: RESUMABLE for ALTER TABLE added in SQL Server 2022 → TSql160

# Step 2: Check if already supported
grep -r "RESUMABLE" Test/SqlDom/TestScripts/*.sql
# Result: Found in ALTER INDEX tests, but not ALTER TABLE tests

# Step 3: Create test script
New-Item "Test/SqlDom/TestScripts/AlterTableResumableTests160.sql"
# Add 4 test cases covering different scenarios

# Step 4: Add test configuration
# Edit Test/SqlDom/Only160SyntaxTests.cs
# Add: new ParserTest160("AlterTableResumableTests160.sql", nErrors80: 4, ...)

# Step 5: Create empty baseline
New-Item "Test/SqlDom/Baselines160/AlterTableResumableTests160.sql"

# Step 6: Build and run test
dotnet build SqlScriptDom/Microsoft.SqlServer.TransactSql.ScriptDom.csproj -c Debug
dotnet test --filter "AlterTableResumableTests160" -c Debug
# Test fails - copy "Actual" output into baseline file

# Step 7: Re-run test
dotnet test --filter "AlterTableResumableTests160" -c Debug
# Test passes!

# Step 8: Run full suite
dotnet test Test/SqlDom/UTSqlScriptDom.csproj -c Debug
# All 1120 tests pass!

# Step 9: Commit changes
git add Test/SqlDom/TestScripts/AlterTableResumableTests160.sql
git add Test/SqlDom/Baselines160/AlterTableResumableTests160.sql
git add Test/SqlDom/Only160SyntaxTests.cs
git commit -m "Add tests for ALTER TABLE RESUMABLE option (SQL Server 2022)"
```

## Testing Best Practices

### 1. Comprehensive Coverage
- ✅ Test basic syntax
- ✅ Test with multiple options
- ✅ Test different statement variations
- ✅ Test with parameters/variables (if applicable)
- ✅ Test edge cases
- ✅ Test error conditions (if relevant)

### 2. Baseline Accuracy
- ✅ Generate baseline from actual parser output
- ✅ Don't hand-edit baseline formatting
- ✅ Verify baseline matches ScriptDOM formatting conventions
- ✅ Check for proper indentation and line breaks

### 3. Version-Specific Testing
- ✅ Test only in the version where feature was introduced
- ✅ Verify older versions properly reject the syntax
- ✅ Document version dependencies clearly

### 4. Regression Prevention
- ✅ Always run full test suite before committing
- ✅ Investigate any unexpected test failures
- ✅ Don't assume your change is isolated

## Common Pitfalls

### ❌ Wrong Version Number
**Problem**: Testing in TSql150 when feature is TSql160-only
**Solution**: Verify SQL Server version in Microsoft docs

### ❌ Incorrect Error Counts
**Problem**: `nErrors80: 2` but test has 4 failing statements
**Solution**: Count all statements that use the new feature

### ❌ Hand-Edited Baselines
**Problem**: Baseline formatting doesn't match ScriptDOM output
**Solution**: Always copy from actual parser output

### ❌ Skipping Full Test Suite
**Problem**: Your change breaks existing tests
**Solution**: Run `dotnet test Test/SqlDom/UTSqlScriptDom.csproj -c Debug`

### ❌ Missing Test Cases
**Problem**: Feature works for basic case but fails with parameters
**Solution**: Add comprehensive test coverage

## Troubleshooting

### Test Fails: "Syntax error near..."
**Diagnosis**: Parser doesn't recognize the syntax
**Solution**: Grammar needs to be updated (see [BUG_FIXING_GUIDE.md](../BUG_FIXING_GUIDE.md))

### Test Fails: "Option 'X' is not valid..."
**Diagnosis**: Validation logic rejects the syntax
**Solution**: See [VALIDATION_FIX_GUIDE.md](../VALIDATION_FIX_GUIDE.md)

### Test Fails: Baseline mismatch
**Diagnosis**: Generated output differs from baseline
**Solution**: Update baseline with actual output or fix generator

### Full Suite Fails: Other tests break
**Diagnosis**: Your changes affected shared code
**Solution**: Review your changes, create context-specific rules

## Quick Reference Commands

```bash
# Search for syntax in tests
grep -r "KEYWORD" Test/SqlDom/TestScripts/

# Search in grammar
grep -r "KEYWORD" SqlScriptDom/Parser/TSql/*.g

# Build parser
dotnet build SqlScriptDom/Microsoft.SqlServer.TransactSql.ScriptDom.csproj -c Debug

# Run specific test
dotnet test --filter "TestName" -c Debug

# Run full suite
dotnet test Test/SqlDom/UTSqlScriptDom.csproj -c Debug

# Create test files
New-Item "Test/SqlDom/TestScripts/MyTest160.sql"
New-Item "Test/SqlDom/Baselines160/MyTest160.sql"
```

## Related Guides

- [DEBUGGING_WORKFLOW.md](../DEBUGGING_WORKFLOW.md) - How to diagnose issues
- [VALIDATION_FIX_GUIDE.md](../VALIDATION_FIX_GUIDE.md) - Fix validation errors
- [BUG_FIXING_GUIDE.md](../BUG_FIXING_GUIDE.md) - Add new grammar rules
- [copilot-instructions.md](../copilot-instructions.md) - Main project documentation
