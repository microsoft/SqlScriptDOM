# Testing Guidelines for SqlScriptDOM

This guide provides comprehensive instructions for adding and running tests in the SqlScriptDOM parser, based on the testing framework patterns and best practices.

## Overview

The SqlScriptDOM testing framework validates parser functionality through:
1. **Parse → Generate → Parse Round-trip Testing** - Ensures syntax is correctly parsed and regenerated
2. **Baseline Comparison** - Verifies generated T-SQL matches expected formatted output
3. **Error Count Validation** - Confirms expected parse errors for invalid syntax across SQL versions
4. **Version-Specific Testing** - Tests syntax across multiple SQL Server versions (SQL 2000-2025)

## Test Framework Architecture

### Core Components

- **Test Scripts** (`Test/SqlDom/TestScripts/`) - Input T-SQL files containing syntax to test
- **Baselines** (`Test/SqlDom/Baselines<version>/`) - Expected formatted output for each test script
- **Test Configuration** (`Test/SqlDom/Only<version>SyntaxTests.cs`) - Test definitions with error expectations
- **Test Runners** - MSTest framework running parse/generate/validate cycles

### How Tests Work

1. **Parse Phase**: Test script is parsed using specified SQL Server version parser
2. **Generate Phase**: Parsed AST is converted back to T-SQL using script generator  
3. **Validate Phase**: Generated output is compared against baseline file
4. **Error Validation**: Parse error count is compared against expected error count for each SQL version

## Adding New Tests

### 1. Create Test Script

Create a new `.sql` file in `Test/SqlDom/TestScripts/` with descriptive name:

**File**: `Test/SqlDom/TestScripts/YourFeatureTests160.sql`
```sql
-- Test basic syntax
SELECT JSON_ARRAY('value1', 'value2');

-- Test in complex context
ALTER FUNCTION TestFunction()
RETURNS NVARCHAR(MAX)
AS
BEGIN
    RETURN (JSON_ARRAY('name', 'value'));
END;
GO

-- Test edge cases
SELECT JSON_ARRAY();
SELECT JSON_ARRAY(NULL, 'test', 123);
```

**Naming Convention**:
- `<FeatureName>Tests<SQLVersion>.sql` (e.g., `JsonFunctionTests160.sql`)
- `<StatementType>Tests<SQLVersion>.sql` (e.g., `CreateTableTests170.sql`)
- Use version number corresponding to SQL Server version where feature was introduced

### 2. Create Baseline File

Create corresponding baseline file in version-specific baseline directory:

**File**: `Test/SqlDom/Baselines160/YourFeatureTests160.sql`

**Initial Creation**:
1. Create empty or placeholder baseline file first
2. Run the test (it will fail) 
3. Copy "Actual" output from test failure message
4. Paste into baseline file with proper formatting

**Example Baseline**:
```sql
SELECT JSON_ARRAY ('value1', 'value2');

ALTER FUNCTION TestFunction
( )
RETURNS NVARCHAR (MAX)
AS
BEGIN
    RETURN (JSON_ARRAY ('name', 'value'));
END

GO

SELECT JSON_ARRAY ();
SELECT JSON_ARRAY (NULL, 'test', 123);
```

**Formatting Notes**:
- Parser adds consistent spacing around parentheses and operators
- GO statements are preserved
- Indentation follows parser's formatting rules

### 3. Configure Test Entry

Add test configuration to appropriate `Only<version>SyntaxTests.cs` file:

**File**: `Test/SqlDom/Only160SyntaxTests.cs`
```csharp
// Around line where other ParserTest160 entries are defined
new ParserTest160("YourFeatureTests160.sql", 
    nErrors80: 1,   // SQL Server 2000 - expect error for new syntax
    nErrors90: 1,   // SQL Server 2005 - expect error for new syntax  
    nErrors100: 1,  // SQL Server 2008 - expect error for new syntax
    nErrors110: 1,  // SQL Server 2012 - expect error for new syntax
    nErrors120: 1,  // SQL Server 2014 - expect error for new syntax
    nErrors130: 1,  // SQL Server 2016 - expect error for new syntax
    nErrors140: 1,  // SQL Server 2017 - expect error for new syntax
    nErrors150: 1   // SQL Server 2019 - expect error for new syntax
    // nErrors160: 0 is implicit for SQL Server 2022 - expect success
),
```

**Error Count Guidelines**:
- **0 errors**: Syntax should parse successfully in this SQL version
- **1+ errors**: Syntax should fail with specified number of parse errors
- **Consider SQL version compatibility**: When was the feature introduced?

### 4. Run and Validate Test

#### Run Specific Test
```bash
# Run specific test method
dotnet test Test/SqlDom/UTSqlScriptDom.csproj --filter "FullyQualifiedName~TSql160SyntaxIn160ParserTest" -c Debug

# Run tests for specific version
dotnet test Test/SqlDom/UTSqlScriptDom.csproj --filter "TestCategory=TSql160" -c Debug
```

#### Run Full Test Suite
```bash
# Run complete test suite (recommended for final validation)
dotnet test Test/SqlDom/UTSqlScriptDom.csproj -c Debug
```

#### Interpret Results
- ✅ **Success**: Generated output matches baseline, error counts match expectations
- ❌ **Failure**: Review actual vs expected output, adjust baseline or fix grammar
- ⚠️ **Baseline Mismatch**: Copy correct "Actual" output to baseline file
- ⚠️ **Error Count Mismatch**: Adjust error expectations in test configuration

## Test Categories and Patterns

### Version-Specific Tests

Each SQL version has its own test class:
- `TSql80SyntaxTests` - SQL Server 2000
- `TSql90SyntaxTests` - SQL Server 2005  
- `TSql100SyntaxTests` - SQL Server 2008
- `TSql110SyntaxTests` - SQL Server 2012
- `TSql120SyntaxTests` - SQL Server 2014
- `TSql130SyntaxTests` - SQL Server 2016
- `TSql140SyntaxTests` - SQL Server 2017
- `TSql150SyntaxTests` - SQL Server 2019
- `TSql160SyntaxTests` - SQL Server 2022
- `TSql170SyntaxTests` - SQL Server 2025

### Cross-Version Testing

When you add a test to `Only160SyntaxTests.cs`, the framework automatically runs it against all SQL parsers:
- `TSql160SyntaxIn160ParserTest` - Parse with SQL 2022 parser (should succeed)
- `TSql160SyntaxIn150ParserTest` - Parse with SQL 2019 parser (may fail for new syntax)
- `TSql160SyntaxIn140ParserTest` - Parse with SQL 2017 parser (may fail for new syntax)
- ... and so on for all versions

### Common Test Patterns

#### Function Tests
```sql
-- Basic function call
SELECT YOUR_FUNCTION('param');

-- Function in different contexts
SELECT col1, YOUR_FUNCTION('param') AS computed FROM table1;
WHERE YOUR_FUNCTION('param') > 0;

-- Function in RETURN statements (critical test)
ALTER FUNCTION Test() RETURNS NVARCHAR(MAX) AS BEGIN
    RETURN (YOUR_FUNCTION('value'));
END;
```

#### Statement Tests  
```sql
-- Basic statement
YOUR_STATEMENT option1, option2;

-- With expressions
YOUR_STATEMENT @variable, 'literal', column_name;

-- Complex nested scenarios
YOUR_STATEMENT (
    SELECT nested FROM table 
    WHERE condition = YOUR_FUNCTION('test')
);
```

#### Error Condition Tests
```sql
-- Invalid syntax that should produce parse errors
YOUR_STATEMENT INVALID SYNTAX HERE;

-- Incomplete statements
YOUR_STATEMENT MISSING;
```

## Test Debugging and Troubleshooting

### Common Issues

#### 1. Baseline Mismatch
```
Assert.AreEqual failed. Expected output does not match actual output.
Actual: 'SELECT JSON_ARRAY ('value1', 'value2');'
Expected: 'SELECT JSON_ARRAY('value1', 'value2');'
```

**Solution**: Copy the "Actual" output to your baseline file (note spacing differences).

#### 2. Error Count Mismatch  
```
TestYourFeature.sql: number of errors after parsing is different from expected.
Expected: 1, Actual: 0
```

**Solutions**:
- **If Actual < Expected**: Grammar now supports syntax in older versions → Update error counts
- **If Actual > Expected**: Grammar has issues → Fix grammar or adjust test

#### 3. Parse Errors
```
SQL46010: Incorrect syntax near 'YOUR_TOKEN'. at offset 45, line 2, column 15
```

**Solutions**:
- Check grammar rules for your syntax
- Verify syntactic predicates are in correct order
- For RETURN statement issues, see [Function Guidelines](function.guidelines.instructions.md)

#### 4. Missing Baseline Files
```
System.IO.FileNotFoundException: Could not find file 'Baselines160\YourTest.sql'
```

**Solution**: Create the baseline file in correct directory with exact same name as test script.

### Debugging Steps

1. **Check File Names**: Ensure test script and baseline have identical names
2. **Verify File Location**: Scripts in `TestScripts/`, baselines in `Baselines<version>/`
3. **Run Single Test**: Isolate issue by running specific test method
4. **Check Grammar**: Ensure grammar rules support your syntax
5. **Validate AST**: Verify AST nodes are properly generated
6. **Test Round-trip**: Parse → Generate → Parse should succeed

## Best Practices

### Test Design

#### Comprehensive Coverage
```sql
-- ✅ Good: Covers multiple scenarios
SELECT JSON_ARRAY('simple');
SELECT JSON_ARRAY('multiple', 'values', 123);
SELECT JSON_ARRAY(NULL);
SELECT JSON_ARRAY();
SELECT JSON_ARRAY(@variable);
SELECT JSON_ARRAY(column_name);
ALTER FUNCTION Test() RETURNS NVARCHAR(MAX) AS BEGIN
    RETURN (JSON_ARRAY('in_return'));
END;
```

#### Focused Testing
```sql
-- ❌ Avoid: Mixing unrelated syntax in single test
SELECT JSON_ARRAY('test');
CREATE TABLE test_table (id INT);  -- Unrelated to JSON
INSERT INTO test_table VALUES (1);  -- Unrelated to JSON
```

#### Edge Cases
```sql
-- ✅ Include edge cases
SELECT JSON_ARRAY();  -- Empty parameters
SELECT JSON_ARRAY(NULL, NULL);  -- NULL handling  
SELECT JSON_ARRAY('very_long_string_value_that_tests_parser_limits');
SELECT JSON_ARRAY((SELECT nested FROM table));  -- Subqueries
```

### Error Expectations

#### Version Compatibility
```csharp
// ✅ Good: Realistic version expectations
new ParserTest160("JsonTests160.sql", 
    nErrors80: 1,   // JSON not in SQL 2000
    nErrors90: 1,   // JSON not in SQL 2005
    // ... 
    nErrors150: 1,  // JSON not in SQL 2019
    // nErrors160: 0 - JSON supported in SQL 2022
),
```

#### Grammar Reality
```csharp
// ⚠️ Consider: Grammar changes may affect all versions
new ParserTest160("TestFunction160.sql",
    nErrors80: 1,   // If function syntax fails in SQL 2000 due to function structure
    nErrors90: 0,   // If shared grammar makes it work in all versions
    // ...
),
```

### File Organization

#### Logical Grouping
```
TestScripts/
├── JsonFunctionTests160.sql      # JSON-specific functions
├── StringFunctionTests160.sql    # String manipulation  
├── CreateTableTests170.sql       # DDL statements
├── SelectStatementTests170.sql   # DML statements
└── AlterFunctionTests160.sql     # Function-specific syntax
```

#### Version Alignment
```
TestScripts/JsonTests160.sql ↔ Baselines160/JsonTests160.sql
TestScripts/JsonTests170.sql ↔ Baselines170/JsonTests170.sql  
```

## Performance Considerations

### Test Execution Time

#### Minimal Test Sets
```bash
# Run specific version tests only
dotnet test --filter "TestCategory=TSql160" -c Debug

# Run specific feature tests  
dotnet test --filter "FullyQualifiedName~Json" -c Debug
```

#### Parallel Execution
```bash
# Use parallel test execution for faster runs
dotnet test --parallel -c Debug
```

#### Focused Development
```bash
# During development, run only your new tests
dotnet test --filter "FullyQualifiedName~YourTestMethod" -c Debug
```

### Build Performance

#### Incremental Testing
1. Add test script and baseline
2. Run specific test to validate
3. Run full test suite only before commit

#### Cached Builds
- Parser regeneration only occurs when grammar files change
- Test compilation is incremental
- Use `-c Debug` for faster iteration

## Integration with Development Workflow

### 1. Grammar Development
```bash
# After grammar changes
dotnet build SqlScriptDom/Microsoft.SqlServer.TransactSql.ScriptDom.csproj -c Debug

# Test specific functionality  
dotnet test --filter "FullyQualifiedName~YourFeature" -c Debug
```

### 2. Test-Driven Development
```bash
# 1. Create failing test
dotnet test --filter "FullyQualifiedName~YourNewTest" -c Debug  # Should fail

# 2. Implement grammar changes
dotnet build -c Debug

# 3. Update baseline and validate
dotnet test --filter "FullyQualifiedName~YourNewTest" -c Debug  # Should pass

# 4. Run regression tests
dotnet test Test/SqlDom/UTSqlScriptDom.csproj -c Debug  # Should all pass
```

### 3. Continuous Integration
```bash
# Full validation before commit
dotnet test Test/SqlDom/UTSqlScriptDom.csproj -c Debug
# Ensure: Total tests: 1,116, Failed: 0, Succeeded: 1,116
```

## Common Test Scenarios

### Adding New Function

```sql
-- Test/SqlDom/TestScripts/NewFunctionTests160.sql
SELECT NEW_FUNCTION('param1', 'param2');
SELECT NEW_FUNCTION(@variable);
SELECT NEW_FUNCTION(column_name);

-- Critical: Test in RETURN statement
ALTER FUNCTION TestNewFunction()
RETURNS NVARCHAR(MAX) 
AS
BEGIN
    RETURN (NEW_FUNCTION('test_value'));
END;
GO
```

### Adding New Statement

```sql  
-- Test/SqlDom/TestScripts/NewStatementTests170.sql
NEW_STATEMENT option1 = 'value1', option2 = 'value2';

NEW_STATEMENT 
    option1 = 'value1',
    option2 = @parameter,
    option3 = (SELECT nested FROM table);

-- Test with expressions
NEW_STATEMENT computed_option = (value1 + value2);
```

### Testing Error Conditions

```sql
-- Test/SqlDom/TestScripts/ErrorConditionTests160.sql  
-- These should generate parse errors

NEW_FUNCTION();  -- Invalid: missing required parameters
NEW_FUNCTION('param1',);  -- Invalid: trailing comma
NEW_FUNCTION('param1' 'param2');  -- Invalid: missing comma
```

## Advanced Testing Patterns

### Multi-File Tests

For complex scenarios requiring multiple related test files:
```
TestScripts/
├── ComplexScenarioTests160_Part1.sql
├── ComplexScenarioTests160_Part2.sql  
└── ComplexScenarioTests160_Integration.sql

Baselines160/
├── ComplexScenarioTests160_Part1.sql
├── ComplexScenarioTests160_Part2.sql
└── ComplexScenarioTests160_Integration.sql
```

### Version Migration Tests

Testing syntax evolution across versions:
```sql
-- Test/SqlDom/TestScripts/FeatureEvolutionTests170.sql
-- Tests new syntax in 170 that extends 160 functionality
SELECT JSON_ARRAY('basic');  -- Supported in 160
SELECT JSON_ARRAY('new', 'syntax', 'in', 'version', '170');  -- New in 170
```

### Regression Tests

When fixing bugs, add specific regression tests:
```sql
-- Test/SqlDom/TestScripts/RegressionBugFix12345Tests160.sql
-- Specific test case that reproduced bug #12345
ALTER FUNCTION TestRegression()
RETURNS NVARCHAR(MAX)
AS  
BEGIN
    RETURN (JSON_OBJECT('key': (SELECT value FROM table)));
END;
```

## Summary

The SqlScriptDOM testing framework provides comprehensive validation of parser functionality through:
- **Round-trip testing** (Parse → Generate → Parse)
- **Baseline comparison** (Generated output vs expected)
- **Cross-version validation** (Test syntax across SQL Server versions)
- **Error condition testing** (Invalid syntax produces expected errors)

Following these guidelines ensures robust test coverage for parser functionality and prevents regressions when adding new features or fixing bugs.