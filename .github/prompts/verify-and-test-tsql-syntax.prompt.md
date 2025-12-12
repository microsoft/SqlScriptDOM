---
title: How to Verify T-SQL Syntax Support and Add Tests
description: Step-by-step guide to check if a T-SQL syntax is already supported and how to add comprehensive test coverage
tags: [testing, verification, tsql, syntax, parser, baseline]
---

# How to Verify T-SQL Syntax Support and Add Tests

This guide helps you determine if a T-SQL syntax is already supported by ScriptDOM and shows you how to add proper test coverage.

## Step 0: Verify the Exact Script First

**CRITICAL**: Before doing anything else, test the exact T-SQL script provided to confirm whether it works or fails.

**IMPORTANT**: For initial verification, you MUST add a debug unit test method directly to an existing test file (like Only170SyntaxTests.cs). This is only for initial verification. Once you confirm the syntax status, you'll follow the proper testing workflow to add comprehensive test coverage.

### Step 1: Add Debug Unit Test Method

Add this debug test method to the appropriate test file (e.g., `Test/SqlDom/Only170SyntaxTests.cs`):

```csharp
[TestMethod]
public void DebugExactScriptTest()
{
    // PUT THE EXACT T-SQL SCRIPT HERE - DO NOT CREATE SEPARATE FILES
    string script = @"SELECT      Id,
            DATEADD(DAY, 1, GETDATE())
FROM        Table1";
    
    Console.WriteLine($"Testing exact script: {script}");
    
    // Test with the target parser version first (e.g., TSql170)
    TSql170Parser parser170 = new TSql170Parser(true);
    IList<ParseError> errors170;
    
    using (StringReader reader = new StringReader(script))
    {
        TSqlFragment fragment = parser170.Parse(reader, out errors170);
        
        Console.WriteLine($"\n=== TSql170 Parser Results ===");
        if (errors170.Count == 0)
        {
            Console.WriteLine("✅ SUCCESS: Parsed without errors");
            
            // Test script generation (round-trip)
            Sql170ScriptGenerator generator = new Sql170ScriptGenerator();
            string generatedScript;
            generator.GenerateScript(fragment, out generatedScript);
            Console.WriteLine($"Generated: {generatedScript}");
        }
        else
        {
            Console.WriteLine($"❌ FAILED: {errors170.Count} parse errors:");
            foreach (var error in errors170)
            {
                Console.WriteLine($"  Line {error.Line}, Col {error.Column}: {error.Message}");
            }
        }
    }
    
    // Test with older parser version for comparison (e.g., TSql160)
    TSql160Parser parser160 = new TSql160Parser(true);
    IList<ParseError> errors160;
    
    using (StringReader reader = new StringReader(script))
    {
        TSqlFragment fragment = parser160.Parse(reader, out errors160);
        
        Console.WriteLine($"\n=== TSql160 Parser Results ===");
        if (errors160.Count == 0)
        {
            Console.WriteLine("✅ SUCCESS: Parsed without errors");
        }
        else
        {
            Console.WriteLine($"❌ FAILED: {errors160.Count} parse errors:");
            foreach (var error in errors160)
            {
                Console.WriteLine($"  Line {error.Line}, Col {error.Column}: {error.Message}");
            }
        }
    }
    
    // Use Assert.Inconclusive to document current status without failing the test
    if (errors170.Count > 0)
    {
        Assert.Inconclusive($"Script currently fails with {errors170.Count} errors. Needs implementation.");
    }
    else
    {
        Assert.Inconclusive("Script already works! Can proceed to add comprehensive test coverage.");
    }
}
```

### Step 2: Build and Run the Debug Test
### Step 2: Build and Run the Debug Test

```bash
# 1. Build the parser to ensure it's up to date
dotnet build SqlScriptDom/Microsoft.SqlServer.TransactSql.ScriptDom.csproj -c Debug

# 2. Run the debug test to see current status
dotnet test --filter "DebugExactScriptTest" Test/SqlDom/UTSqlScriptDom.csproj -c Debug

# 3. Check the test output for detailed results
# Look for the console output showing parsing results
```

### Interpret Results

- **✅ SUCCESS**: Script works! You can skip to Step 6 to add comprehensive tests
- **❌ FAILURE**: Script fails. Continue with Steps 1-5 to implement the missing functionality

**Important**: Always test the **exact script provided** character-for-character, including:
- Exact table/column names (e.g., `t.c1`, `t.c2`)
- Exact function syntax (e.g., `JSON_OBJECTAGG( t.c1 : t.c2 )`)
- Complete query context (FROM clause, subqueries, etc.)
- Exact whitespace and formatting as provided

**Remember**: Only add unit test methods to existing test files. Do not create separate SQL files, program files, or any other external files.

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
Create a unit test method to verify parsing:

```csharp
// Add to appropriate test file (e.g., Test/SqlDom/Only170SyntaxTests.cs)
[TestMethod]
public void QuickTestExactSyntax()
{
    string script = @"ALTER TABLE t ADD CONSTRAINT pk PRIMARY KEY (id) WITH (RESUMABLE = ON);";
    
    TSql170Parser parser = new TSql170Parser(true);
    IList<ParseError> errors;
    
    using (StringReader reader = new StringReader(script))
    {
        TSqlFragment fragment = parser.Parse(reader, out errors);
        Console.WriteLine($"Errors: {errors.Count}");
        
        // This will show you exactly which parser versions support the syntax
        // and what error messages are generated if it fails
    }
    
    Assert.Inconclusive($"Test completed with {errors.Count} errors");
}
```

### Method 5: Test in Existing Test Framework
Add a temporary test method to verify quickly:

```csharp
// Add to appropriate test file (e.g., Test/SqlDom/Only170SyntaxTests.cs)
[TestMethod]
public void TempTestExactScript()
{
    // Put ONLY your exact script here - do not create external files
    string script = @"YOUR_EXACT_SCRIPT_HERE";
    
    TSql170Parser parser = new TSql170Parser(true);
    IList<ParseError> errors;
    
    using (StringReader reader = new StringReader(script))
    {
        TSqlFragment fragment = parser.Parse(reader, out errors);
        Console.WriteLine($"Parse result: {errors.Count} errors");
        foreach (var error in errors)
        {
            Console.WriteLine($"Error: {error.Message}");
        }
    }
    
    Assert.Inconclusive("Temporary test - remove after verification");
}
```

Then run the test:
```bash
dotnet test --filter "TempTestExactScript" -c Debug
```

Remember to remove this temporary test method after verification.

## Step 3: Create a Test Script

**CRITICAL**: Your test script MUST include the exact T-SQL statement provided. Don't modify, simplify, or generalize the syntax - test the precise statement given.

### Test File Naming Convention
Follow the pattern from testing.guidelines.instructions.md:
- Format: `<FeatureName>Tests<SQLVersion>.sql`
- Examples: `JsonFunctionTests160.sql`, `AlterTableResumableTests160.sql`
- Location: `Test/SqlDom/TestScripts/`
- Use version number corresponding to SQL Server version where feature was introduced

### Test Script Requirements

1. **Start with the exact script provided** - copy it exactly as given
2. **Add comprehensive coverage** as described in testing guidelines:
   - Basic syntax variations
   - Function in different contexts (SELECT, WHERE, RETURN statements)
   - Edge cases (empty parameters, NULL handling, subqueries)
   - Integration contexts (variables, parameters, computed expressions)
3. **Include context** - ensure the exact context (table aliases, subqueries) is tested
4. **Test RETURN statements** - Critical for functions, always test in ALTER FUNCTION RETURN statements

### Test Script Template

Follow the comprehensive coverage pattern from testing guidelines:

```sql
-- Test 1: EXACT SCRIPT PROVIDED (REQUIRED - COPY EXACTLY)
-- PUT THE EXACT T-SQL STATEMENT HERE WITHOUT ANY MODIFICATIONS
-- Example: SELECT JSON_OBJECTAGG( t.c1 : t.c2 ) FROM (VALUES('key1', 'c'), ('key2', 'b'), ('key3','a')) AS t(c1, c2);

-- Test 2: Basic function call (if applicable)
SELECT YOUR_FUNCTION('param1', 'param2');

-- Test 3: Function in different contexts
SELECT col1, YOUR_FUNCTION('param') AS computed FROM table1;
WHERE YOUR_FUNCTION('param') > 0;

-- Test 4: CRITICAL - Function in RETURN statements (for functions)
ALTER FUNCTION TestYourFunction()
RETURNS NVARCHAR(MAX)
AS
BEGIN
    RETURN (YOUR_FUNCTION('test_value'));
END;
GO

-- Test 5: With variables/parameters
SELECT YOUR_FUNCTION(@variable);
SELECT YOUR_FUNCTION(column_name);

-- Test 6: Edge cases
SELECT YOUR_FUNCTION();  -- Empty parameters (if valid)
SELECT YOUR_FUNCTION(NULL, 'test', 123);  -- NULL handling
SELECT YOUR_FUNCTION((SELECT nested FROM table));  -- Subqueries
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

## Step 4: Configure Test Entry

Add test configuration to the appropriate `Only<version>SyntaxTests.cs` file as described in testing guidelines.

### Test Configuration File Location
- Format: `Only{Version}SyntaxTests.cs`
- Example: `Only160SyntaxTests.cs` (for SQL Server 2022)
- Location: `Test/SqlDom/`
- Add to the `Only{Version}TestInfos` array

### Test Configuration Template

Use the simplified approach from testing guidelines:

```csharp
// Option 1: Simplified - only specify error counts you care about
new ParserTest{Version}("YourFeatureTests{Version}.sql"),  // All previous versions default to null (ignored), current version expects 0 errors

// Option 2: Specify only some previous version error counts
new ParserTest{Version}("YourFeatureTests{Version}.sql", nErrors80: 1, nErrors90: 1),  // Only SQL 2000/2005 expect errors

// Option 3: Full specification (legacy compatibility)
new ParserTest{Version}("YourFeatureTests{Version}.sql", 
    nErrors80: 1,   // SQL Server 2000 - expect error for new syntax
    nErrors90: 1,   // SQL Server 2005 - expect error for new syntax  
    nErrors100: 1,  // SQL Server 2008 - expect error for new syntax
    nErrors110: 1,  // SQL Server 2012 - expect error for new syntax
    nErrors120: 1,  // SQL Server 2014 - expect error for new syntax
    nErrors130: 1,  // SQL Server 2016 - expect error for new syntax
    nErrors140: 1,  // SQL Server 2017 - expect error for new syntax
    nErrors150: 1   // SQL Server 2019 - expect error for new syntax
    // nErrors{Version}: 0 is implicit for current version - expect success
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

## Step 5: Create Baseline File

Baseline files contain the expected formatted output after parsing and script generation.

### Baseline File Location
- Format: `Baselines{Version}/YourTestFile{Version}.sql`
- Example: `Baselines160/AlterTableResumableTests160.sql`
- Location: `Test/SqlDom/`
- **Critical**: Baseline filename MUST exactly match the test script filename

### Baseline Generation Process

Follow the testing guidelines process:

#### Initial Creation:
1. **Create empty or placeholder baseline file first**
2. **Run the test** (it will fail) 
3. **Copy "Actual" output** from test failure message
4. **Paste into baseline file** with proper formatting

```bash
# 1. Create placeholder baseline file
New-Item "Test/SqlDom/Baselines160/YourFeatureTests160.sql" -ItemType File

# 2. Run the test (will fail initially)
dotnet test --filter "YourFeatureTests160" -c Debug

# 3. Copy the "Actual" output from test failure into baseline file
# Look for the test failure message showing:
# Expected: <empty or old content>
# Actual: <This is what you need to copy>

# 4. Re-run the test (should pass now)
dotnet test --filter "YourFeatureTests160" -c Debug
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

## Step 6: Run and Validate Test

Follow the testing guidelines validation process.

### Build the Parser
```bash
# Build ScriptDOM library
dotnet build SqlScriptDom/Microsoft.SqlServer.TransactSql.ScriptDom.csproj -c Debug

# Build test project  
dotnet build Test/SqlDom/UTSqlScriptDom.csproj -c Debug
```

### Run Your Specific Test
```bash
# Run specific test method
dotnet test Test/SqlDom/UTSqlScriptDom.csproj --filter "FullyQualifiedName~TSql160SyntaxIn160ParserTest" -c Debug

# Run tests for specific version
dotnet test Test/SqlDom/UTSqlScriptDom.csproj --filter "TestCategory=TSql160" -c Debug

# Run by test script name filter
dotnet test --filter "YourFeatureTests160" -c Debug

# Run with verbose output to see details
dotnet test --filter "YourFeatureTests160" -c Debug -v detailed
```

### Run Full Test Suite (CRITICAL!)
```bash
# Always run ALL tests before committing
dotnet test Test/SqlDom/UTSqlScriptDom.csproj -c Debug

# Expected output:
# Test summary: total: 1116, failed: 0, succeeded: 1116, skipped: 0
```

### Interpret Results

Follow the testing guidelines interpretation:

- ✅ **Success**: Generated output matches baseline, error counts match expectations
- ❌ **Failure**: Review actual vs expected output, adjust baseline or fix grammar  
- ⚠️ **Baseline Mismatch**: Copy correct "Actual" output to baseline file
- ⚠️ **Error Count Mismatch**: Adjust error expectations in test configuration

### Common Test Results

✅ **Success**: All tests pass, including your new test
```
Test summary: total: 1116, failed: 0, succeeded: 1116, skipped: 0
```

❌ **Baseline Mismatch**: Generated output doesn't match baseline
```
Expected: <baseline content>
Actual: <generated content>
```
**Solution**: Copy the "Actual" output to your baseline file (note spacing differences)

❌ **Error Count Mismatch**: Parse error count differs from expected  
```
TestYourFeature.sql: number of errors after parsing is different from expected.
Expected: 1, Actual: 0
```
**Solutions**:
- **If Actual < Expected**: Grammar now supports syntax in older versions → Update error counts
- **If Actual > Expected**: Grammar has issues → Fix grammar or adjust test

❌ **Parse Errors**: Syntax not recognized
```
SQL46010: Incorrect syntax near 'YOUR_TOKEN'. at offset 45, line 2, column 15
```
**Solutions**: Check grammar rules, verify syntactic predicates, see function guidelines for RETURN statement issues

## Complete Example Workflow

### Example: Testing ALTER TABLE RESUMABLE for SQL Server 2022

```bash
# Step 0: Test exact script first
echo "ALTER TABLE MyTable ADD CONSTRAINT pk PRIMARY KEY (id) WITH (RESUMABLE = ON);" > temp_test_script.sql
# Add debug test method to Only160SyntaxTests.cs and run to confirm current status

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
- ✅ **TEST EXACT SCRIPT PROVIDED** (most critical)
- ✅ Test basic syntax variations
- ✅ Test with multiple options
- ✅ Test different statement variations
- ✅ Test with parameters/variables (if applicable)
- ✅ Test edge cases
- ✅ Test error conditions (if relevant)
- ✅ Test complete context (subqueries, table aliases, etc.)

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

### ❌ Not Testing Exact Script
**Problem**: Testing simplified/modified versions instead of the exact script provided
**Solution**: Always include the exact T-SQL statement as provided, character-for-character

## Troubleshooting

### Test Fails: "Syntax error near..."
**Diagnosis**: Parser doesn't recognize the syntax
**Solution**: Grammar needs to be updated (see [Bug Fixing Guide](../instructions/bug_fixing.guidelines.instructions.md))

### Test Fails: "Option 'X' is not valid..."
**Diagnosis**: Validation logic rejects the syntax
**Solution**: See [Validation Fix Guide](../instructions/validation_fix.guidelines.instructions.md)

### Test Fails: Baseline mismatch
**Diagnosis**: Generated output differs from baseline
**Solution**: Update baseline with actual output or fix generator

### Full Suite Fails: Other tests break
**Diagnosis**: Your changes affected shared code
**Solution**: Review your changes, create context-specific rules

## Quick Reference Commands

```bash
# Step 0: Add debug unit test method first (NO external files)
# Add DebugExactScriptTest method to appropriate test file with exact script embedded

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

# Create test files (only for comprehensive testing, not initial verification)
New-Item "Test/SqlDom/TestScripts/MyTest160.sql"
New-Item "Test/SqlDom/Baselines160/MyTest160.sql"
```

## Related Guides

- [debugging_workflow.guidelines.instructions.md](../instructions/debugging_workflow.guidelines.instructions.md) - How to diagnose issues
- [Validation_fix.guidelines.instructions.md](../instructions/validation_fix.guidelines.instructions.md) - Fix validation errors
- [Bug Fixing Guide](../instructions/bug_fixing.guidelines.instructions.md) - Add new grammar rules
- [copilot-instructions.md](../copilot-instructions.md) - Main project documentation
