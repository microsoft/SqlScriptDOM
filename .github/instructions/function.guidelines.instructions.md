# Guidelines for Adding New System Functions to SqlScriptDOM Parser

This guide provides comprehensive instructions for adding new T-SQL system functions to the SqlScriptDOM parser, incorporating lessons learned from fixing JSON function parsing in RETURN statements.

## Overview

Adding a new system function involves three main components:
1. **AST Definition** (`Ast.xml`) - Define the abstract syntax tree node structure
2. **Grammar Rules** (`.g` files) - Define parsing logic for the function syntax
3. **Script Generator** - Handle conversion from AST back to T-SQL text
4. **Testing** - Ensure functionality works correctly across all contexts

## Key Principle: Support Functions in RETURN Statements

**Critical Requirement**: New system functions must be parseable in `ALTER FUNCTION` RETURN statements. This requires special handling due to ANTLR v2's limitation with semantic predicates during syntactic predicate lookahead.

### The RETURN Statement Challenge

The `returnStatement` grammar rule uses a syntactic predicate for lookahead:
```antlr
returnStatement: Return ((expression) => expression)? semicolonOpt
```

During lookahead, ANTLR cannot evaluate semantic predicates (which check runtime values like `vResult.FunctionName.Value`). This causes new functions to fail parsing in RETURN contexts even if they work elsewhere.

## Why SELECT Works but RETURN Fails (The Core Problem)

This section explains the fundamental issue we encountered with JSON functions and why it affects any new system function.

### SELECT Statement Context (Always Works)
```sql
SELECT JSON_ARRAY('name');  -- ✅ Always worked
```

**Grammar Path**: `selectStatement` → `selectElementsList` → `selectElement` → `expression` → `expressionPrimary` → `builtInFunctionCall`

**Why it works**: No syntactic predicates in the path - parser can evaluate semantic predicates normally during parsing.

### RETURN Statement Context (Previously Failed)
```sql
RETURN JSON_ARRAY('name');  -- ❌ Failed before our fix
```

**Grammar Path**: `returnStatement` uses syntactic predicate `((expression) =>` for lookahead

**Why it failed**:
1. Parser encounters `RETURN JSON_ARRAY(...)`
2. Syntactic predicate triggers lookahead to check if `JSON_ARRAY(...)` is a valid expression
3. During lookahead: `expression` → `expressionPrimary` → `builtInFunctionCall`
4. `builtInFunctionCall` has semantic predicate: `{(vResult.FunctionName.Value == "JSON_ARRAY")}?`
5. **ANTLR v2 limitation**: Cannot evaluate `vResult.FunctionName.Value` during lookahead (object doesn't exist yet)
6. Lookahead fails → parser assumes not an expression → syntax error

**The Solution**: Add token-based syntactic predicates in `expressionPrimary` that work during lookahead:
```antlr
{NextTokenMatches(CodeGenerationSupporter.JsonArray) && (LA(2) == LeftParenthesis)}?
vResult=jsonArrayCall
```

This is why **every new system function** must include the syntactic predicate pattern to work in RETURN statements.

## Step-by-Step Implementation Guide

### 1. Update AST Definition (`SqlScriptDom/Parser/TSql/Ast.xml`)

Define the function's AST node structure:

```xml
<!-- Add to appropriate location in Ast.xml -->
<Class Name="YourNewFunctionCall" Base="PrimaryExpression">
    <Member Name="Parameter1" Type="ScalarExpression" Summary="First parameter of the function" />
    <Member Name="Parameter2" Type="StringLiteral" Summary="Second parameter (if string literal only)" />
    <!-- Use ScalarExpression for dynamic values, specific types for literals only -->
</Class>
```

**Best Practice**: Use `ScalarExpression` for parameters that should support:
- Literals (`'value'`, `123`)
- Parameters (`@param`)
- Variables (`@variable`)
- Column references (`table.column`)
- Computed expressions (`value + 1`)

Use specific literal types only when the SQL syntax strictly requires literals.

### 2. Add Grammar Rules (`.g` files)

#### 2a. Define the Function Rule

Add to the appropriate grammar files (typically `TSql160.g`, `TSql170.g`, `TSqlFabricDW.g`):

```antlr
yourNewFunctionCall returns [YourNewFunctionCall vResult = FragmentFactory.CreateFragment<YourNewFunctionCall>()]
{
    ScalarExpression vParam1;
    StringLiteral vParam2;
}
    :
    tFunction:Identifier LeftParenthesis
    {
        Match(tFunction, CodeGenerationSupporter.YourFunctionName);
        UpdateTokenInfo(vResult, tFunction);
    }
    vParam1 = expression
    {
        vResult.Parameter1 = vParam1;
    }
    (Comma vParam2 = stringLiteral
    {
        vResult.Parameter2 = vParam2;
    })?
    RightParenthesis
    ;
```

#### 2b. **CRITICAL**: Add Syntactic Predicate for RETURN Statement Support

Add to `expressionPrimary` rule **before** the generic `(Identifier LeftParenthesis)` predicate:

```antlr
expressionPrimary returns [PrimaryExpression vResult]
    // ... existing rules ...
    
    // Add BEFORE the generic identifier predicate
    | {NextTokenMatches(CodeGenerationSupporter.YourFunctionName) && (LA(2) == LeftParenthesis)}?
      vResult=yourNewFunctionCall
    
    // ... rest of existing rules including the generic identifier case ...
    | (Identifier LeftParenthesis) => vResult=builtInFunctionCall
```

**Why This is Required**: 
- The syntactic predicate uses `NextTokenMatches()` which works during lookahead
- It must come **before** the generic `builtInFunctionCall` predicate
- This enables the function to be recognized in RETURN statements

#### 2c. Add to Built-in Function Call (if needed)

If your function should also be recognized through the general built-in function mechanism, add it to `builtInFunctionCall`:

```antlr
builtInFunctionCall returns [FunctionCall vResult = FragmentFactory.CreateFragment<FunctionCall>()]
    // ... existing cases ...
    
    | {(vResult.FunctionName.Value == "YOUR_FUNCTION_NAME")}?
      vResult=yourNewFunctionCall
```

### 3. Update CodeGenerationSupporter Constants

Add the function name constant to `CodeGenerationSupporter.cs`:

```csharp
public const string YourFunctionName = "YOUR_FUNCTION_NAME";
```

### 4. Create Script Generator

Add visitor method to handle AST-to-script conversion in the appropriate script generator file:

```csharp
public override void ExplicitVisit(YourNewFunctionCall node)
{
    GenerateIdentifier(CodeGenerationSupporter.YourFunctionName);
    GenerateSymbol(TSqlTokenType.LeftParenthesis);
    
    if (node.Parameter1 != null)
    {
        GenerateFragmentIfNotNull(node.Parameter1);
        
        if (node.Parameter2 != null)
        {
            GenerateSymbol(TSqlTokenType.Comma);
            GenerateSpace();
            GenerateFragmentIfNotNull(node.Parameter2);
        }
    }
    
    GenerateSymbol(TSqlTokenType.RightParenthesis);
}
```

### 5. Integrate with Grammar Hierarchy

Add your function to the appropriate place in the grammar hierarchy:

```antlr
// Add to function call expressions
functionCall returns [FunctionCall vResult]
    : vResult=yourNewFunctionCall
    | // ... other function types
    ;

// Or add to primary expressions if it's a primary expression type
primaryExpression returns [PrimaryExpression vResult]
    : vResult=yourNewFunctionCall
    | // ... other primary expressions
    ;
```

### 6. Build and Test

#### 6a. Build the Project

```bash
dotnet build SqlScriptDom/Microsoft.SqlServer.TransactSql.ScriptDom.csproj -c Debug
```

This will regenerate parser files from the grammar.

#### 6b. Create Test Files

Create test script in `Test/SqlDom/TestScripts/YourFunctionTests160.sql`:

```sql
-- Test basic function call
SELECT YOUR_FUNCTION_NAME('param1', 'param2');

-- CRITICAL: Test in ALTER FUNCTION RETURN statement
ALTER FUNCTION TestYourFunction()
RETURNS NVARCHAR(MAX)
AS
BEGIN
    RETURN (YOUR_FUNCTION_NAME('value1', 'value2'));
END;
GO
```

#### 6c. Generate Baselines

1. Create placeholder baseline file: `Test/SqlDom/Baselines160/YourFunctionTests160.sql`
2. Run the test (it will fail)
3. Copy the "Actual" output from the test failure
4. Update the baseline file with the correctly formatted output

#### 6d. Configure Test

Add test entry to `Test/SqlDom/Only160SyntaxTests.cs`:

```csharp
new ParserTest160("YourFunctionTests160.sql", nErrors80: 1, nErrors90: 1, nErrors100: 1, nErrors110: 1, nErrors120: 1, nErrors130: 1, nErrors140: 1, nErrors150: 1),
```

Adjust error counts based on which SQL versions should support your function.

#### 6e. Run Full Test Suite

```bash
dotnet test Test/SqlDom/UTSqlScriptDom.csproj -c Debug
```

Ensure all tests pass, including existing ones (no regressions).

## Real-World Example: JSON Functions Fix

This guide incorporates lessons learned from fixing `JSON_OBJECT` and `JSON_ARRAY` parsing in RETURN statements:

### Problem Encountered
```sql
-- This worked fine:
SELECT JSON_ARRAY('name');  -- ✅ Always worked

-- This failed before the fix:
ALTER FUNCTION GetAuth() RETURNS NVARCHAR(MAX) AS BEGIN
    RETURN (JSON_OBJECT('key': 'value'));  -- ❌ Parse error here
END;
```

### Why SELECT Worked but RETURN Didn't

**SELECT Statement Context** (Always Worked):
```sql
SELECT JSON_ARRAY('name');
```
In a SELECT statement, the parser follows this path:
1. `selectStatement` → `queryExpression` → `querySpecification` 
2. `selectElementsList` → `selectElement` → `expression`
3. `expression` → `expressionPrimary` → `builtInFunctionCall`
4. ✅ No syntactic predicate blocking the path

**RETURN Statement Context** (Previously Failed):
```sql
RETURN JSON_ARRAY('name');
```
In a RETURN statement, the parser follows this path:
1. `returnStatement` uses a **syntactic predicate**: `((expression) =>`
2. During lookahead, parser tries: `expression` → `expressionPrimary` → `builtInFunctionCall`
3. `builtInFunctionCall` has a **semantic predicate**: `{(vResult.FunctionName.Value == "JSON_ARRAY")}?`
4. ❌ **ANTLR v2 limitation**: Semantic predicates cannot be evaluated during syntactic predicate lookahead
5. ❌ Lookahead fails → parser doesn't recognize `JSON_ARRAY` as valid expression

### Root Cause
The semantic predicate `{(vResult.FunctionName.Value == "JSON_OBJECT")}?` in `builtInFunctionCall` could not be evaluated during the syntactic predicate lookahead in `returnStatement`.

### Solution Applied
Added syntactic predicates in `expressionPrimary`:

```antlr
// Added before generic identifier predicate
| {NextTokenMatches(CodeGenerationSupporter.JsonObject) && (LA(2) == LeftParenthesis)}?
  vResult=jsonObjectCall
| {NextTokenMatches(CodeGenerationSupporter.JsonArray) && (LA(2) == LeftParenthesis)}?
  vResult=jsonArrayCall
```

This uses token-based checking (`NextTokenMatches`) which works during lookahead, unlike semantic predicates.

## Grammar Files to Modify

For SQL Server 2022+ functions, typically modify:
- `SqlScriptDom/Parser/TSql/TSql160.g` (SQL Server 2022)
- `SqlScriptDom/Parser/TSql/TSql170.g` (SQL Server 2025)
- `SqlScriptDom/Parser/TSql/TSqlFabricDW.g` (Azure Synapse)

For earlier versions, add to appropriate grammar files (`TSql150.g`, `TSql140.g`, etc.).

## Common Pitfalls

1. **Forgetting RETURN Statement Support**: Always add syntactic predicates to `expressionPrimary`
2. **Wrong Predicate Order**: Syntactic predicates must come **before** generic predicates
3. **Semantic Predicates in Lookahead**: Don't rely on semantic predicates in contexts with syntactic predicate lookahead
4. **Missing Script Generator**: Every AST node needs a corresponding script generation visitor
5. **Incomplete Testing**: Test both standalone function calls and RETURN statement usage
6. **Version Compatibility**: Consider which SQL versions should support your function

## Testing Checklist

- [ ] Function parses in SELECT statements
- [ ] Function parses in WHERE clauses
- [ ] **Function parses in ALTER FUNCTION RETURN statements**
- [ ] Function parses with literal parameters
- [ ] Function parses with variable parameters
- [ ] Function parses with computed expressions as parameters
- [ ] Script generation produces correct T-SQL output
- [ ] Round-trip parsing (parse → generate → parse) works
- [ ] No regressions in existing tests
- [ ] Appropriate error handling for invalid syntax

## Architecture Notes

### Why Syntactic vs Semantic Predicates Matter

- **Syntactic Predicates**: Can check token types during lookahead (`LA()`, `NextTokenMatches()`)
- **Semantic Predicates**: Check runtime values, but fail during lookahead in syntactic predicates
- **RETURN Statement Context**: Uses syntactic predicate `((expression) =>` which triggers lookahead

### Grammar Rule Hierarchy

```
returnStatement
  └── expression
      └── expressionPrimary
          ├── yourNewFunctionCall (syntactic predicate)
          └── builtInFunctionCall (semantic predicate) 
```

By adding syntactic predicates to `expressionPrimary`, we catch function calls before they reach the problematic semantic predicate in `builtInFunctionCall`.

## Summary

Following this guide ensures new system functions work correctly in all T-SQL contexts, especially the challenging RETURN statement scenario. The key insight is that ANTLR v2's limitations require careful predicate ordering and the use of token-based syntactic predicates for functions that need to work in lookahead contexts.