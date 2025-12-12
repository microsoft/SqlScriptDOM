# Guidelines for Adding New Data Types to SqlScriptDOM

This guide provides step-by-step instructions for adding support for completely new SQL Server data types to the SqlScriptDOM parser. This pattern was established from the Vector data type implementation (commits 38a0971 and cd69b78).

## When to Use This Guide

Use this pattern when:
- ✅ Adding a **completely new SQL Server data type** (e.g., VECTOR, GEOMETRY, GEOGRAPHY)
- ✅ The data type has **custom parameters** not handled by standard SQL data types
- ✅ The data type requires **specialized parsing logic** beyond simple name/size parameters
- ✅ The data type is **introduced in a specific SQL Server version**

**Do NOT use this guide for:**
- ❌ Modifying existing data types (use [validation_fix.guidelines.instructions.md](validation_fix.guidelines.instructions.md))
- ❌ Adding function syntax (use [function.guidelines.instructions.md](function.guidelines.instructions.md))
- ❌ Simple keyword additions (use [bug_fixing.guidelines.instructions.md](bug_fixing.guidelines.instructions.md))

## Real-World Example: Vector Data Type

The Vector data type implementation demonstrates this pattern:

### SQL Server Syntax Supported
```sql
-- Basic vector with dimension only
DECLARE @embedding AS VECTOR(1536);
CREATE TABLE tbl (embedding VECTOR(1536));

-- Vector with dimension and base type
DECLARE @embedding AS VECTOR(1536, FLOAT32);
CREATE TABLE tbl (embedding VECTOR(1536, FLOAT16));
```

### Key Challenge Solved
The Vector type requires custom parsing because:
- **Standard data types** use size parameters: `VARCHAR(50)`, `DECIMAL(10,2)`
- **Vector type** uses dimension + optional base type: `VECTOR(1536, FLOAT32)`
- **Base type parameter** is an identifier (FLOAT16/FLOAT32), not a size literal

## Step-by-Step Implementation Guide

### 1. Define AST Node Structure (`Ast.xml`)

Add a new class inheriting from `DataTypeReference`:

```xml
<!-- Location: SqlScriptDom/Parser/TSql/Ast.xml -->
<!-- Add near other DataTypeReference classes (~line 270) -->

<Class Name="YourDataTypeReference" Base="DataTypeReference" Summary="Represents your new data types">
    <InheritedClass Name="DataTypeReference" />
    <Member Name="Parameter1" Type="IntegerLiteral" Summary="First parameter description"/>
    <Member Name="Parameter2" Type="Identifier" Summary="Optional second parameter" />
    <!-- Use appropriate types based on your syntax requirements -->
</Class>
```

**Design Principles**:
- **Inherit from `DataTypeReference`**: All SQL data types inherit from this base class
- **Choose appropriate member types**:
  - `IntegerLiteral`: For numeric parameters (dimensions, sizes)
  - `Identifier`: For type names or keywords
  - `StringLiteral`: For string parameters
  - `ScalarExpression`: For complex expressions (use sparingly)
- **Optional parameters**: Members can be null for optional syntax

### 2. Add Grammar Rule (`TSql*.g`)

Create a specialized parsing rule for your data type:

```antlr
// Location: SqlScriptDom/Parser/TSql/TSql170.g (or appropriate version)
// Add after xmlDataType rule (~line 30672)

yourDataType [SchemaObjectName vName] returns [YourDataTypeReference vResult = FragmentFactory.CreateFragment<YourDataTypeReference>()]
{
    vResult.Name = vName;
    vResult.UpdateTokenInfo(vName);
    
    IntegerLiteral vParameter1 = null;
    Identifier vParameter2 = null;
}
    :
        (   LeftParenthesis vParameter1=integer
            {
                vResult.Parameter1 = vParameter1;
            }
            (
                Comma vParameter2=identifier
                {
                    vResult.Parameter2 = vParameter2;
                }
            )?
            tRParen:RightParenthesis
            {
                UpdateTokenInfo(vResult,tRParen);
            }
        )
    ;
```

**Grammar Pattern Explanation**:
- **Function signature**: Takes `SchemaObjectName vName` parameter and returns your AST type
- **Variable declarations**: Declare variables for each parameter using appropriate types
- **Parameter parsing**: Use `integer`, `identifier`, `stringLiteral` based on syntax needs
- **Optional parameters**: Wrap in `( ... )?` syntax for optional elements
- **Token info updates**: Always call `UpdateTokenInfo()` for proper source location tracking

### 3. Integrate with Scalar Data Type Rule

Connect your new grammar rule to the main data type parsing logic:

```antlr
// Location: SqlScriptDom/Parser/TSql/TSql170.g
// Find scalarDataType rule (~line 30694) and add your type check

scalarDataType returns [DataTypeReference vResult = null]
{
    SchemaObjectName vName;
    SqlDataTypeOption typeOption = SqlDataTypeOption.None;
    // ... existing variables ...
}
    :   vName = schemaObjectFourPartName
        {
            typeOption = GetSqlDataTypeOption(vName);
            // ... existing logic ...
        }
        (
            (
                {isXmlDataType}?
                vResult = xmlDataType[vName]
            |
                {typeOption == SqlDataTypeOption.YourType}?  // Add this condition
                vResult = yourDataType[vName]
            |
                {typeOption != SqlDataTypeOption.None}?
                vResult = sqlDataTypeWithoutNational[vName, typeOption]
            // ... rest of existing alternatives
```

**Integration Requirements**:
- **Add type option check**: Use `{typeOption == SqlDataTypeOption.YourType}?` semantic predicate
- **Maintain order**: Place before the generic `sqlDataTypeWithoutNational` fallback
- **Update SqlDataTypeOption enum**: Add your type to the enum (implementation dependent)

### 4. Add String Constants

Add necessary string constants for keywords:

```csharp
// Location: SqlScriptDom/Parser/TSql/CodeGenerationSupporter.cs
// Add alphabetically in the constants section (~line 427 for Float constants)

internal const string YourType = "YOURTYPE";
internal const string YourTypeParam1 = "PARAM1_KEYWORD";
internal const string YourTypeParam2 = "PARAM2_KEYWORD";
```

**Naming Convention**:
- **Use exact SQL keyword casing**: `VECTOR`, `FLOAT16`, `FLOAT32`
- **Group related constants**: Keep data type constants together
- **Alphabetical ordering**: Maintain alphabetical order within sections

### 5. Create Script Generator

Implement the visitor method to convert AST back to T-SQL:

```csharp
// Location: SqlScriptDom/ScriptDom/SqlServer/ScriptGenerator/SqlScriptGeneratorVisitor.YourDataType.cs
// Create new file following naming convention

//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.YourDataType.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(YourDataTypeReference node)
        {
            GenerateIdentifier(CodeGenerationSupporter.YourType);
            GenerateSymbol(TSqlTokenType.LeftParenthesis);
            GenerateFragmentIfNotNull(node.Parameter1);
            if (node.Parameter2 != null)
            {
                GenerateSymbol(TSqlTokenType.Comma);
                GenerateSpaceAndFragmentIfNotNull(node.Parameter2);
            }
            GenerateSymbol(TSqlTokenType.RightParenthesis);
        }
    }
}
```

**Script Generation Patterns**:
- **Use `GenerateIdentifier()`**: For type names and keywords
- **Use `GenerateSymbol()`**: For punctuation (`LeftParenthesis`, `Comma`, etc.)
- **Use `GenerateFragmentIfNotNull()`**: For required parameters
- **Use `GenerateSpaceAndFragmentIfNotNull()`**: For optional parameters with preceding space
- **Handle optional parameters**: Always check for null before generating

### 6. Build and Test Grammar Changes

Build the project to regenerate parser files:

```bash
# Build the ScriptDOM project to regenerate parser from grammar
dotnet build SqlScriptDom/Microsoft.SqlServer.TransactSql.ScriptDom.csproj -c Debug
```

**Common Build Issues**:
- **Grammar syntax errors**: Check ANTLR syntax in `.g` files
- **Missing constants**: Ensure all referenced constants exist in `CodeGenerationSupporter.cs`
- **AST node mismatches**: Verify AST class names match grammar return types

### 7. Create Comprehensive Test Scripts

Create test script covering all syntax variations:

```sql
-- File: Test/SqlDom/TestScripts/YourDataTypeTests170.sql

-- Basic syntax with single parameter
CREATE TABLE tbl (col1 YOURTYPE(100));
DECLARE @var1 AS YOURTYPE(100);

-- Extended syntax with optional parameters  
CREATE TABLE tbl (col1 YOURTYPE(100, PARAM1));
DECLARE @var2 AS YOURTYPE(100, PARAM2);

-- Case insensitivity testing
CREATE TABLE tbl (col1 yourtype(100, param1));
DECLARE @var3 AS YOURTYPE(100, param2);

-- Integration with other SQL constructs
CREATE TABLE tbl (
    id INT PRIMARY KEY,
    data YOURTYPE(100, PARAM1) NOT NULL
);

-- Variables and parameters
CREATE FUNCTION TestFunction(@input YOURTYPE(100))
RETURNS YOURTYPE(200, PARAM2)
AS
BEGIN
    DECLARE @result YOURTYPE(200, PARAM2);
    RETURN @result;
END;
```

**Test Coverage Requirements**:
- **All syntax variations**: Test with and without optional parameters
- **Case sensitivity**: Test different case combinations
- **Integration contexts**: Variables, table columns, function parameters/returns
- **Edge cases**: Minimum/maximum parameter values if applicable

### 8. Generate Baseline Files

Create the expected output baseline:

1. **Create placeholder baseline**: `Test/SqlDom/Baselines170/YourDataTypeTests170.sql`
2. **Run the test** (will fail initially):
   ```bash
   dotnet test --filter "YourDataTypeTests170" Test/SqlDom/UTSqlScriptDom.csproj -c Debug
   ```
3. **Copy "Actual" output** from test failure into baseline file
4. **Verify formatting** matches parser's standard formatting

**Baseline Example** (Vector data type):
```sql
CREATE TABLE tbl (
    embedding VECTOR(1)
);

CREATE TABLE tbl (
    embedding VECTOR(1, float32)
);

DECLARE @embedding AS VECTOR(2);

DECLARE @embedding AS VECTOR(2, FLOAT32);
```

### 9. Configure Test Expectations

Add test configuration to version-specific test class:

```csharp
// Location: Test/SqlDom/Only170SyntaxTests.cs (or appropriate version)
// Add to the ParserTest170 array

new ParserTest170("YourDataTypeTests170.sql"),
```

**Error Count Guidelines**:
- **Count all syntax instances**: Each usage of the new type in test script
- **Consider SQL version support**: When was the feature actually introduced?
- **Consistent across versions**: Usually same error count until supported version
- **Test validation**: Run tests to verify error counts are accurate

### 10. Full Test Suite Validation

Run complete test suite to ensure no regressions:

```bash
# Run all ScriptDOM tests
dotnet test Test/SqlDom/UTSqlScriptDom.csproj -c Debug

# Expected result: All tests pass, including new data type tests
# Total tests: 1,100+ (number increases with new features)
```

**Regression Prevention**:
- **Grammar changes can break existing functionality**: Shared rules affect multiple contexts
- **AST changes can break script generation**: Ensure all visitors are updated
- **Version compatibility**: New syntax shouldn't break older version parsers

## Advanced Considerations

### Version-Specific Implementation

For data types introduced in specific SQL Server versions:

```antlr
// Different grammar files for different SQL versions
// TSql160.g - SQL Server 2022 features
// TSql170.g - SQL Server 2025 features  
// TSqlFabricDW.g - Azure Synapse features
```

**Guidelines**:
- **Target appropriate version**: Add to the SQL version where feature was introduced
- **Cascade to later versions**: Copy rules to all subsequent version grammar files
- **Version-specific testing**: Test error behavior in older parsers

### Complex Parameter Types

For data types requiring complex parameter parsing:

```xml
<!-- Advanced AST member types -->
<Member Name="AdvancedParam" Type="ScalarExpression" Summary="Supports variables and expressions"/>
<Member Name="OptionList" Type="IList&lt;DataTypeOption&gt;" Summary="List of options"/>
```

**When to use complex types**:
- **ScalarExpression**: When parameters can be variables, function calls, or computed values
- **Collections**: When syntax supports multiple values or options
- **Custom classes**: When parameters have their own sub-syntax

### Script Generator Considerations

For complex formatting requirements:

```csharp
public override void ExplicitVisit(ComplexDataTypeReference node)
{
    GenerateIdentifier(CodeGenerationSupporter.ComplexType);
    GenerateSymbol(TSqlTokenType.LeftParenthesis);
    
    // Complex formatting with line breaks
    if (node.HasMultipleParameters)
    {
        Indent();
        GenerateNewLine();
    }
    
    GenerateCommaSeparatedList(node.Parameters);
    
    if (node.HasMultipleParameters)
    {
        Outdent();
        GenerateNewLine();
    }
    
    GenerateSymbol(TSqlTokenType.RightParenthesis);
}
```

## Common Pitfalls and Solutions

### 1. Forgetting Script Generator Implementation
**Problem**: AST node created but no script generation visitor
**Solution**: Always implement `ExplicitVisit()` method for new AST nodes

### 2. Incorrect Grammar Integration  
**Problem**: Data type not recognized in all contexts
**Solution**: Ensure integration with `scalarDataType` rule and proper semantic predicates

### 3. Missing Version Compatibility
**Problem**: New type breaks older version parsers unexpectedly
**Solution**: Add proper version checks and test all SQL Server versions

### 4. Incomplete Test Coverage
**Problem**: Edge cases not covered in testing
**Solution**: Test all syntax variations, case sensitivity, and integration contexts

### 5. AST Design Issues
**Problem**: AST doesn't properly represent the SQL syntax
**Solution**: Design AST members to match SQL parameter structure and optionality

## Validation Checklist

- [ ] **AST Definition**: New class inherits from correct base class with appropriate members
- [ ] **Grammar Rules**: Specialized parsing rule handles all syntax variations  
- [ ] **Grammar Integration**: Connected to `scalarDataType` with proper semantic predicate
- [ ] **String Constants**: All keywords added to `CodeGenerationSupporter.cs`
- [ ] **Script Generator**: `ExplicitVisit()` method generates correct T-SQL output
- [ ] **Test Scripts**: Comprehensive test coverage including edge cases
- [ ] **Baseline Files**: Generated output matches expected formatted T-SQL
- [ ] **Test Configuration**: Error counts configured for all SQL Server versions
- [ ] **Build Success**: Project builds without errors and regenerates parser
- [ ] **Full Test Suite**: All existing tests continue to pass (no regressions)

## Related Guides

- [bug_fixing.guidelines.instructions.md](bug_fixing.guidelines.instructions.md) - For general grammar modifications
- [function.guidelines.instructions.md](function.guidelines.instructions.md) - For adding system functions
- [validation_fix.guidelines.instructions.md](validation_fix.guidelines.instructions.md) - For validation-only issues
- [testing.guidelines.instructions.md](testing.guidelines.instructions.md) - For comprehensive testing strategies

## Real-World Examples

### Vector Data Type (SQL Server 2025)
- **AST Class**: `VectorDataTypeReference` with `Dimension` and `BaseType` members
- **Syntax**: `VECTOR(1536)`, `VECTOR(1536, FLOAT32)`
- **Commits**: cd69b78, 38a0971
- **Challenge**: Optional second parameter with identifier type

### Future Examples  
This pattern can be applied to other SQL Server data types like:
- **GEOMETRY**: Spatial data with complex parameters
- **GEOGRAPHY**: Geographic data with coordinate systems  
- **HIERARCHYID**: Hierarchical data with custom syntax
- **Custom CLR Types**: User-defined types with specialized parameters

The Vector implementation serves as the canonical example for this pattern and should be referenced for similar future data type additions.