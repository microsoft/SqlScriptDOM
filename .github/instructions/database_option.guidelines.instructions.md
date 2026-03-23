# Adding Database Options to ScriptDOM

This guide covers how to add new database options to `ALTER DATABASE` and `CREATE DATABASE` statements in ScriptDOM.

## Decision Tree: Choose Your Implementation Pattern

```
What values does your database option accept?
│
├─ Simple ON/OFF only
│   └─ Pattern A: Use Generic OnOffDatabaseOption (easiest)
│       Examples: AutoClose, AutoShrink, DataRetention
│
├─ ON/OFF with sub-options (e.g., CHANGE_TRACKING (AUTO_CLEANUP = ON))
│   └─ Pattern B: Custom AST Class + Complex Sub-options
│       Examples: ChangeTrackingDatabaseOption, QueryStoreDatabaseOption
│
├─ Specific enum values (e.g., FULL, BULK_LOGGED, SIMPLE)
│   └─ Pattern C: Custom AST Class + Enum Helper
│       Examples: RecoveryDatabaseOption, PageVerifyDatabaseOption
│
└─ Special behavior (only output when ON, custom formatting)
    └─ Pattern D: Custom AST Class + Custom Script Generator
        Examples: LedgerOption, OptimizedLockingDatabaseOption
```

---

## Pattern A: Generic ON/OFF Database Option (Recommended for Simple Cases)

Use this pattern when your option only accepts `ON` or `OFF` values.

### Step 1: Add to DatabaseOptionKind Enum

**File**: `SqlScriptDom/Parser/TSql/DatabaseOptionKind.cs`

```csharp
public enum DatabaseOptionKind
{
    // ... existing options ...
    OptimizedLocking            = 71,
    YourNewOption               = 72   // ← Add your option
}
```

### Step 2: Add Keyword Constant

**File**: `SqlScriptDom/Parser/TSql/CodeGenerationSupporter.cs`

Add in alphabetical order within the Auto* section:

```csharp
internal const string YourOption = "YOUR_OPTION";
internal const string AutomaticTuning = "AUTOMATIC_TUNING";
internal const string AutoShrink = "AUTO_SHRINK";
```

### Step 3: Register in OnOffSimpleDbOptionsHelper

**File**: `SqlScriptDom/Parser/TSql/OnOffSimpleDbOptionsHelper.cs`

Add to the constructor under the appropriate version:

```csharp
// 170 options
AddOptionMapping(DatabaseOptionKind.YourNewOption, 
    CodeGenerationSupporter.YourOption, 
    SqlVersionFlags.TSql170AndAbove);
```

### Step 4: Add to RequiresEqualsSign Method (If Needed)

Some options require `=` between option name and value (e.g., `LEDGER = ON`). If your option needs this:

**File**: `SqlScriptDom/Parser/TSql/OnOffSimpleDbOptionsHelper.cs`

```csharp
internal bool RequiresEqualsSign(DatabaseOptionKind optionKind)
{
    switch (optionKind)
    {
        case DatabaseOptionKind.MemoryOptimizedElevateToSnapshot:
        case DatabaseOptionKind.NestedTriggers:
        case DatabaseOptionKind.TransformNoiseWords:
        case DatabaseOptionKind.Ledger:
        case DatabaseOptionKind.YourNewOption:  // ← Add here if needed
            return true;
        default:
            return false;
    }
}
```

### Step 5: Create Test Files

**Test Script**: `Test/SqlDom/TestScripts/AlterDatabaseYourOption170.sql`

```sql
-- YourOption for VNext and Azure
ALTER DATABASE db
    SET YOUR_OPTION = ON;

ALTER DATABASE db
    SET YOUR_OPTION = OFF;
```

**Baseline**: `Test/SqlDom/Baselines170/AlterDatabaseYourOption170.sql`

```sql
ALTER DATABASE db
    SET YOUR_OPTION = ON;

ALTER DATABASE db
    SET YOUR_OPTION = OFF;
```

**Test Configuration**: `Test/SqlDom/Only170SyntaxTests.cs`

```csharp
new ParserTest170("AlterDatabaseYourOption170.sql", 
    nErrors80: 2, nErrors90: 2, nErrors100: 2, 
    nErrors110: 2, nErrors120: 2, nErrors130: 2, 
    nErrors140: 2, nErrors150: 2, nErrors160: 2),
```

### Pattern A Complete - No Ast.xml or Script Generator Changes Needed!

---

## Pattern B: Custom AST Class with Sub-Options

Use when your option has complex sub-options (e.g., `CHANGE_TRACKING (AUTO_CLEANUP = ON)`).

### Example: ChangeTrackingDatabaseOption

#### Step 1-3: Same as Pattern A

Add to enum, CodeGenerationSupporter, and helpers as in Pattern A.

#### Step 4: Define Custom AST Class

**File**: `SqlScriptDom/Parser/TSql/Ast.xml`

```xml
<Class Name="YourOptionDatabaseOption" Base="DatabaseOption" 
       Summary="YOUR_OPTION option in ALTER DATABASE statement, SET case">
  <InheritedClass Name="DatabaseOption" />
  <Member Name="OptionState" Type="OptionState" 
          GenerateUpdatePositionInfoCall="false" 
          Summary="Option state."/>
  <Member Name="Details" Type="YourOptionDetail" Collection="true" 
          Summary="Optional sub-option details."/>
</Class>

<Class Name="YourOptionDetail" Abstract="true" 
       Summary="One detail for YourOptionDatabaseOption"/>

<Class Name="SubOption1Detail" Base="YourOptionDetail" 
       Summary="SUB_OPTION_1 part of YOUR_OPTION">
  <InheritedClass Name="YourOptionDetail"/>
  <Member Name="IsOn" Type="bool" 
          Summary="True if set to ON, false otherwise."/>
</Class>

<Class Name="SubOption2Detail" Base="YourOptionDetail" 
       Summary="SUB_OPTION_2 part of YOUR_OPTION">
  <InheritedClass Name="YourOptionDetail"/>
  <Member Name="Value" Type="Literal" 
          Summary="The value for this sub-option."/>
</Class>
```

#### Step 5: Create Custom Script Generator

**File**: `SqlScriptDom/ScriptDom/SqlServer/ScriptGenerator/SqlScriptGeneratorVisitor.YourOptionDatabaseOption.cs`

```csharp
namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(YourOptionDatabaseOption node)
        {
            GenerateIdentifier(CodeGenerationSupporter.YourOption);
            GenerateSpaceAndKeyword(TSqlTokenType.EqualsSign);
            GenerateSpace();
            GenerateOptionStateOnOff(node.OptionState);
            
            if (node.Details != null && node.Details.Count > 0)
            {
                GenerateSpace();
                GenerateParenthesisedCommaSeparatedList(node.Details);
            }
        }
        
        public override void ExplicitVisit(SubOption1Detail node)
        {
            GenerateNameEqualsValue(
                CodeGenerationSupporter.SubOption1,
                node.IsOn ? TSqlTokenType.On : TSqlTokenType.Off);
        }
        
        public override void ExplicitVisit(SubOption2Detail node)
        {
            GenerateNameEqualsValue(
                CodeGenerationSupporter.SubOption2, 
                node.Value);
        }
    }
}
```

#### Step 6: Update Grammar File

**File**: `SqlScriptDom/Parser/TSql/TSql170.g`

Add grammar rules to parse your option and sub-options.

---

## Pattern C: Custom AST Class with Enum Values

Use when your option accepts specific enum values (not just ON/OFF).

### Example: RecoveryDatabaseOption

#### Step 1-2: Add Enum and Keyword

Same as Pattern A, plus define value enum:

**File**: `SqlScriptDom/Parser/TSql/RecoveryDatabaseOptionKind.cs`

```csharp
public enum RecoveryDatabaseOptionKind
{
    Full = 0,
    BulkLogged = 1,
    Simple = 2
}
```

#### Step 3: Define Custom AST Class

**File**: `SqlScriptDom/Parser/TSql/Ast.xml`

```xml
<Class Name="RecoveryDatabaseOption" Base="DatabaseOption" 
       Summary="RECOVERY option in ALTER DATABASE statement, SET case">
  <InheritedClass Name="DatabaseOption" />
  <Member Name="Value" Type="RecoveryDatabaseOptionKind" 
          GenerateUpdatePositionInfoCall="false" 
          Summary="Actual option value"/>
</Class>
```

#### Step 4: Create Helper for Values

**File**: `SqlScriptDom/Parser/TSql/RecoveryDbOptionsHelper.cs`

```csharp
internal class RecoveryDbOptionsHelper : OptionsHelper<RecoveryDatabaseOptionKind>
{
    private RecoveryDbOptionsHelper()
    {
        AddOptionMapping(RecoveryDatabaseOptionKind.Full, 
            CodeGenerationSupporter.Full);
        AddOptionMapping(RecoveryDatabaseOptionKind.BulkLogged, 
            CodeGenerationSupporter.BulkLogged);
        AddOptionMapping(RecoveryDatabaseOptionKind.Simple, 
            CodeGenerationSupporter.Simple);
    }
    
    internal static readonly RecoveryDbOptionsHelper Instance = 
        new RecoveryDbOptionsHelper();
}
```

#### Step 5: Create Script Generator

**File**: `SqlScriptDom/ScriptDom/SqlServer/ScriptGenerator/SqlScriptGeneratorVisitor.RecoveryDatabaseOption.cs`

```csharp
namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(RecoveryDatabaseOption node)
        {
            GenerateIdentifier(CodeGenerationSupporter.Recovery);
            GenerateSpace();
            RecoveryDbOptionsHelper.Instance.GenerateSourceForOption(
                _writer, node.Value);
        }
    }
}
```

---

## Pattern D: Custom AST Class with Special Script Generation

Use when the option has special formatting or conditional output.

### Example: LedgerOption (Only outputs when ON)

#### Step 1-2: Same as Pattern A

#### Step 3: Define Custom AST Class

**File**: `SqlScriptDom/Parser/TSql/Ast.xml`

```xml
<Class Name="LedgerOption" Base="DatabaseOption" 
       Summary="Sets the database's Ledger Option">
  <InheritedClass Name="DatabaseOption"/>
  <Member Name="OptionState" Type="OptionState" 
          GenerateUpdatePositionInfoCall="false" 
          Summary="The option state is ON or OFF value for Ledger."/>
</Class>
```

#### Step 4: Create Custom Script Generator

**File**: `SqlScriptDom/ScriptDom/SqlServer/ScriptGenerator/SqlScriptGeneratorVisitor.LedgerOption.cs`

```csharp
namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(LedgerOption node)
        {
            System.Diagnostics.Debug.Assert(
                node.OptionKind == DatabaseOptionKind.Ledger);

            // Special behavior: Only output when ON
            if (node.OptionState == OptionState.On)
            {
                GenerateNameEqualsValue(
                    CodeGenerationSupporter.Ledger, 
                    node.OptionState.ToString());
            }
        }
    }
}
```

---

## Testing Checklist

For all patterns:

- [ ] Database option enum added to `DatabaseOptionKind.cs`
- [ ] Keyword constant added to `CodeGenerationSupporter.cs`
- [ ] Option registered in appropriate helper class
- [ ] Test script created in `TestScripts/`
- [ ] Baseline created in `Baselines<version>/`
- [ ] Test configured in `Only<version>SyntaxTests.cs`
- [ ] Parser builds successfully
- [ ] Specific test passes
- [ ] **Full test suite passes** (593/593 tests)

For Patterns B, C, D (Custom AST):

- [ ] AST class defined in `Ast.xml`
- [ ] Custom script generator created (if needed)
- [ ] Helper class created (for enum values)
- [ ] Grammar rules updated (if new syntax)

---

## Common Pitfalls

### 1. Forgetting to Rebuild After Ast.xml Changes

```bash
# Always rebuild after modifying Ast.xml
dotnet build SqlScriptDom/Microsoft.SqlServer.TransactSql.ScriptDom.csproj -c Debug
```

### 2. Incorrect Error Counts in Tests

Error count = number of statements in test script that use the new option.

```csharp
// WRONG: Count doesn't match statements
new ParserTest170("TwoStatements.sql", nErrors160: 1)  // ❌

// RIGHT: Match statement count
new ParserTest170("TwoStatements.sql", nErrors160: 2)  // ✓
```

### 3. Missing RequiresEqualsSign Registration

Some options need `=` between name and value:
- With `=`: `LEDGER = ON`, `AUTOMATIC_INDEX_COMPACTION = ON`
- Without `=`: `AUTO_CLOSE ON`, `AUTO_SHRINK ON`

Check existing similar options to determine which pattern to follow.

### 4. Not Running Full Test Suite

Grammar changes can break unrelated tests. Always run:

```bash
dotnet test Test/SqlDom/UTSqlScriptDom.csproj -c Debug
```

---

## Examples Reference

### Pattern A Examples (Generic ON/OFF)
- `AutoClose`
- `AutoShrink`
- `DataRetention`

**Files to check**:
- `DatabaseOptionKind.cs` (enum value)
- `OnOffSimpleDbOptionsHelper.cs` (registration)
- No Ast.xml changes needed
- No custom script generator needed

### Pattern B Examples (Sub-Options)
- `ChangeTrackingDatabaseOption`
- `QueryStoreDatabaseOption`
- `AutomaticTuningDatabaseOption`

**Files to check**:
- All Pattern A files, plus:
- `Ast.xml` (custom class with Details collection)
- Custom script generator

### Pattern C Examples (Enum Values)
- `RecoveryDatabaseOption`
- `PageVerifyDatabaseOption`
- `CursorDefaultDatabaseOption`

**Files to check**:
- All Pattern A files, plus:
- `Ast.xml` (custom class with Value member)
- Helper class for enum mapping
- Custom script generator

### Pattern D Examples (Special Behavior)
- `LedgerOption` (only outputs when ON)
- `OptimizedLockingDatabaseOption` (custom formatting)

**Files to check**:
- All Pattern A files, plus:
- `Ast.xml` (custom class)
- Custom script generator with special logic

---

## Quick Reference: File Locations

| Component | Path |
|-----------|------|
| Database option enum | `SqlScriptDom/Parser/TSql/DatabaseOptionKind.cs` |
| Keyword constants | `SqlScriptDom/Parser/TSql/CodeGenerationSupporter.cs` |
| ON/OFF helper | `SqlScriptDom/Parser/TSql/OnOffSimpleDbOptionsHelper.cs` |
| Other option helpers | `SqlScriptDom/Parser/TSql/DatabaseOptionKindHelper.cs` |
| AST definitions | `SqlScriptDom/Parser/TSql/Ast.xml` |
| Script generators | `SqlScriptDom/ScriptDom/SqlServer/ScriptGenerator/` |
| Test scripts | `Test/SqlDom/TestScripts/` |
| Baselines | `Test/SqlDom/Baselines<version>/` |
| Test classes | `Test/SqlDom/Only<version>SyntaxTests.cs` |

---

## Version Flags Reference

```csharp
SqlVersionFlags.TSql80      // SQL Server 2000
SqlVersionFlags.TSql90      // SQL Server 2005
SqlVersionFlags.TSql100     // SQL Server 2008
SqlVersionFlags.TSql110     // SQL Server 2012
SqlVersionFlags.TSql120     // SQL Server 2014
SqlVersionFlags.TSql130     // SQL Server 2016
SqlVersionFlags.TSql140     // SQL Server 2017
SqlVersionFlags.TSql150     // SQL Server 2019
SqlVersionFlags.TSql160     // SQL Server 2022
SqlVersionFlags.TSql170     // SQL Server 2025
SqlVersionFlags.TSqlFabricDW // Fabric DW

// Combined flags
SqlVersionFlags.TSql170AndAbove  // SQL 2025+
SqlVersionFlags.TSql160AndAbove  // SQL 2022+
```

For Azure SQL Database, Fabric, VNext: Use the latest version (currently `TSql170AndAbove`).
