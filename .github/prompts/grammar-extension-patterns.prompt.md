---
title: Grammar Extension Patterns for SqlScriptDOM
description: Common patterns for extending the SqlScriptDOM parser grammar to support new syntax or enhance existing functionality
tags: [grammar, ast, parser, extension, patterns]
---

# Grammar Extension Patterns for SqlScriptDOM

This guide documents common patterns for extending the SqlScriptDOM parser grammar to support new syntax or enhance existing functionality.

## Pattern 1: Extending Literals to Expressions

### When to Use
When existing grammar rules only accept literal values but need to support dynamic expressions like parameters, variables, or computed values.

### Example Problem
Functions or constructs that currently accept only:
- `IntegerLiteral` (e.g., `TOP_N = 10`)
- `StringLiteral` (e.g., `VALUE = 'literal'`)

But need to support:
- Parameters: `@parameter`
- Variables: `@variable`
- Column references: `table.column`
- Outer references: `outerref.column`
- Function calls: `FUNCTION(args)`
- Computed expressions: `value + 1`

### ⚠️ Critical Warning: Avoid Modifying Shared Grammar Rules

**DO NOT** modify existing shared grammar rules like `identifierColumnReferenceExpression` that are used throughout the codebase. This can cause unintended side effects and break other functionality.

**Instead**, create specialized rules for your specific context.

### Solution Template

#### Step 1: Update AST Definition (`Ast.xml`)
```xml
<!-- Before: -->
<Member Name="PropertyName" Type="IntegerLiteral" Summary="Description" />

<!-- After: -->
<Member Name="PropertyName" Type="ScalarExpression" Summary="Description" />
```

#### Step 2: Create Context-Specific Grammar Rule (`TSql*.g`)
```antlr
// Create a specialized rule for your context
yourContextColumnReferenceExpression returns [ColumnReferenceExpression vResult = this.FragmentFactory.CreateFragment<ColumnReferenceExpression>()]
{
    MultiPartIdentifier vMultiPartIdentifier;
}
    :
        vMultiPartIdentifier=multiPartIdentifier[2]  // Allows table.column syntax
        {
            vResult.ColumnType = ColumnType.Regular;
            vResult.MultiPartIdentifier = vMultiPartIdentifier;
        }
    ;

// Use the specialized rule in your custom grammar
yourContextParameterRule returns [ScalarExpression vResult]
    : vResult=signedInteger
    | vResult=variable
    | vResult=yourContextColumnReferenceExpression  // Context-specific rule
    | vResult=expression  // Allows computed expressions
    ;
```

#### Step 3: Verify Script Generator
Most script generators using `GenerateNameEqualsValue()` or similar methods work automatically with `ScalarExpression`. No changes typically needed.

#### Step 4: Add Test Coverage
```sql
-- Test parameter
FUNCTION_NAME(PARAM = @parameter)

-- Test outer reference
FUNCTION_NAME(PARAM = outerref.column)

-- Test computed expression
FUNCTION_NAME(PARAM = value + 1)
```

### Real-World Example: VECTOR_SEARCH TOP_N

**Problem**: `VECTOR_SEARCH` TOP_N parameter only accepted integer literals.

**❌ Wrong Approach**: Modify `identifierColumnReferenceExpression` to use `multiPartIdentifier[2]`
- **Result**: Broke `CreateIndexStatementErrorTest` because other grammar rules started accepting invalid syntax

**✅ Correct Approach**: Create `vectorSearchColumnReferenceExpression` specialized for VECTOR_SEARCH
- **Result**: VECTOR_SEARCH supports multi-part identifiers without affecting other functionality

**Final Implementation**:
```antlr
signedIntegerOrVariableOrColumnReference returns [ScalarExpression vResult]
    : vResult=signedInteger
    | vResult=variable
    | vResult=vectorSearchColumnReferenceExpression  // VECTOR_SEARCH-specific rule
    ;

vectorSearchColumnReferenceExpression returns [ColumnReferenceExpression vResult = ...]
    :
        vMultiPartIdentifier=multiPartIdentifier[2]  // Allows table.column syntax
        {
            vResult.ColumnType = ColumnType.Regular;
            vResult.MultiPartIdentifier = vMultiPartIdentifier;
        }
    ;
```

**Result**: Now supports dynamic TOP_N values:
```sql
-- Parameters
VECTOR_SEARCH(..., TOP_N = @k) AS ann

-- Outer references  
VECTOR_SEARCH(..., TOP_N = outerref.max_results) AS ann
```

## Pattern 2: Adding New Enum Members

### When to Use
When adding new operators, keywords, or options to existing constructs.

### Solution Template

#### Step 1: Update Enum in AST (`Ast.xml`)
```xml
<Enum Name="ExistingEnumType">
  <Member Name="ExistingValue1" />
  <Member Name="ExistingValue2" />
  <Member Name="NewValue" />  <!-- Add this -->
</Enum>
```

#### Step 2: Update Grammar Rule (`TSql*.g`)
```antlr
// Add new token matching
| tNewValue:Identifier
{
    Match(tNewValue, CodeGenerationSupporter.NewValue);
    vResult.EnumProperty = ExistingEnumType.NewValue;
}
```

#### Step 3: Update Script Generator
```csharp
// Add mapping in appropriate generator file
private static readonly Dictionary<EnumType, string> _enumGenerators = 
    new Dictionary<EnumType, string>()
{
    { EnumType.ExistingValue1, CodeGenerationSupporter.ExistingValue1 },
    { EnumType.ExistingValue2, CodeGenerationSupporter.ExistingValue2 },
    { EnumType.NewValue, CodeGenerationSupporter.NewValue }, // Add this
};
```

## Pattern 3: Adding New Function or Statement

### When to Use
When adding completely new T-SQL functions or statements.

### Solution Template

#### Step 1: Define AST Node (`Ast.xml`)
```xml
<Class Name="NewFunctionCall" Base="PrimaryExpression">
  <Member Name="Parameter1" Type="ScalarExpression" />
  <Member Name="Parameter2" Type="StringLiteral" />
</Class>
```

#### Step 2: Add Grammar Rule (`TSql*.g`)
```antlr
newFunctionCall returns [NewFunctionCall vResult = FragmentFactory.CreateFragment<NewFunctionCall>()]
{
    ScalarExpression vParam1;
    StringLiteral vParam2;
}
    :
    tFunction:Identifier LeftParenthesis
    {
        Match(tFunction, CodeGenerationSupporter.NewFunction);
        UpdateTokenInfo(vResult, tFunction);
    }
    vParam1 = expression
    {
        vResult.Parameter1 = vParam1;
    }
    Comma vParam2 = stringLiteral
    {
        vResult.Parameter2 = vParam2;
    }
    RightParenthesis
    ;
```

#### Step 3: Integrate with Existing Rules
Add the new rule to appropriate places in the grammar (e.g., `functionCall`, `primaryExpression`, etc.).

#### Step 4: Create Script Generator
```csharp
public override void ExplicitVisit(NewFunctionCall node)
{
    GenerateIdentifier(CodeGenerationSupporter.NewFunction);
    GenerateSymbol(TSqlTokenType.LeftParenthesis);
    GenerateFragmentIfNotNull(node.Parameter1);
    GenerateSymbol(TSqlTokenType.Comma);
    GenerateFragmentIfNotNull(node.Parameter2);
    GenerateSymbol(TSqlTokenType.RightParenthesis);
}
```

## Best Practices

### 1. Backward Compatibility
- Always ensure existing syntax continues to work
- Extend rather than replace existing rules
- Test both old and new syntax

### 2. Testing Strategy
- Add comprehensive test cases in `TestScripts/`
- Update baseline files with expected output
- Test edge cases and error conditions

### 3. Documentation
- Update grammar comments with new syntax
- Add examples in code comments
- Document any limitations or requirements

### 4. Version Targeting
- Add new features to the appropriate SQL Server version grammar
- Consider whether feature should be backported to earlier versions
- Update all relevant grammar files if syntax is version-independent

## Common Pitfalls

### 1. Forgetting Script Generator Updates
- Grammar changes often require corresponding script generator changes
- Test the round-trip: parse → generate → parse again

### 2. Incomplete Test Coverage
- Test all supported expression types when extending to `ScalarExpression`
- Include error cases and boundary conditions

### 3. Missing Version Updates
- New syntax should be added to all relevant grammar versions
- Consider SQL Server version compatibility

### 4. AST Design Issues
- Choose appropriate base classes for new AST nodes
- Consider reusing existing AST patterns where possible
- Ensure proper inheritance hierarchy

## Reference Examples

- **VECTOR_SEARCH TOP_N Extension**: Literal to expression pattern
- **REGEXP_LIKE Predicate**: Boolean parentheses recognition pattern
- **EVENT SESSION Predicates**: Function-style vs operator-style predicates

For detailed step-by-step examples, see [BUG_FIXING_GUIDE.md](../BUG_FIXING_GUIDE.md).
