# New SQL Server Feature Implementation Guide

This prompt will identify the type of SQL Server feature you want to add to SqlScriptDOM and **automatically implement it** using the appropriate guideline. After feature type identification, it will execute the complete implementation workflow.

## Feature Type Identification

Please answer the following questions to determine the best implementation approach:

### 1. What type of SQL Server feature are you implementing?

**A) Data Type** - A new SQL Server data type (e.g., VECTOR, GEOMETRY, GEOGRAPHY)
- Example: `DECLARE @embedding AS VECTOR(1536, FLOAT32)`
- Example: `CREATE TABLE tbl (geo_data GEOGRAPHY)`
- **Key indicators**: New type with custom parameters, specialized syntax for type definitions

**B) Index Type** - A specialized index type with unique syntax (e.g., JSON INDEX, VECTOR INDEX)
- Example: `CREATE JSON INDEX IX_JSON ON table (column) FOR ('$.path1', '$.path2')`
- Example: `CREATE VECTOR INDEX IX_VECTOR ON table (column) WITH (METRIC = 'cosine')`
- **Key indicators**: CREATE [TYPE] INDEX syntax, type-specific clauses or options

**C) System Function** - A new T-SQL built-in function (e.g., JSON_OBJECT, JSON_ARRAY)
- Example: `SELECT JSON_OBJECT('key1': 'value1', 'key2': 'value2')`
- Example: `RETURN JSON_ARRAY('item1', 'item2', 'item3')`
- **Key indicators**: Function calls in expressions, may need RETURN statement support

**D) Grammar/Syntax Enhancement** - New operators, statements, or syntax modifications
- Example: Adding new WHERE clause predicates like `REGEXP_LIKE`
- Example: New statement types or operators
- **Key indicators**: Parser doesn't recognize syntax, needs AST updates

**E) Validation Fix** - Existing syntax fails validation but should work per SQL Server docs
- Example: ALTER TABLE RESUMABLE option works in ALTER INDEX but not ALTER TABLE
- **Key indicators**: "Option 'X' is not valid..." errors, similar syntax works elsewhere

**F) Parser Predicate Issue** - Identifier-based predicates fail with parentheses
- Example: `WHERE REGEXP_LIKE(...)` works but `WHERE (REGEXP_LIKE(...))` fails
- **Key indicators**: Syntax errors near closing parenthesis with identifier predicates

### 2. SQL Server Feature Details

**Feature Name**: _______________
**SQL Server Version**: _______________
**Example Syntax**:
```sql
-- Provide 2-3 examples of the syntax you want to support
```

**Current Behavior**: _______________
**Expected Behavior**: _______________

### 3. Feature Characteristics

Check all that apply to your feature:

- [ ] Requires completely new AST node classes
- [ ] Extends existing AST nodes with new members
- [ ] Needs new grammar rules in .g files
- [ ] Requires new keywords/constants
- [ ] Needs specialized script generation logic
- [ ] Has version-specific behavior (SQL Server 2014+, 2022+, etc.)
- [ ] Includes optional syntax elements or clauses
- [ ] Supports collections/lists of parameters
- [ ] Requires new validation logic
- [ ] Needs new index options or statement options

## AUTO-IMPLEMENTATION TRIGGER

**To begin automatic implementation, provide your feature details in this exact format:**

```
Feature Name: [Your feature name]
SQL Server Version: [SQL Server version]
Exact T-SQL Syntax:
```sql
[Copy the EXACT T-SQL syntax from the user's request here]
```
Feature Type: [Will be determined from analysis below]
```

**The system will then automatically identify the feature type and begin implementation.**

## Implementation Guidance

Based on your feature type identification below, the system will automatically execute the appropriate implementation workflow:

### → Data Type (Answer A)
**Auto-Executes**: [New Data Types Guidelines](../instructions/new_data_types.guidelines.instructions.md)

**Automatic implementation includes**:
- Creating new `DataTypeReference` AST classes
- Adding specialized parsing rules for custom type syntax
- Implementing parameter handling (dimensions, base types, etc.)
- Script generation for type definitions
- Version-specific type support
- Comprehensive testing across all SQL contexts

**Best for**: VECTOR, custom CLR types, spatial types, hierarchical types

### → Index Type (Answer B) 
**Auto-Executes**: [New Index Types Guidelines](../instructions/new_index_types.guidelines.instructions.md)

**Automatic implementation includes**:
- Creating new `IndexStatement` AST classes
- Implementing type-specific index syntax parsing
- Adding specialized clauses (FOR, WITH type-specific options)
- Index option registration and validation
- Script generation for index statements
- Integration with existing index framework

**Best for**: JSON INDEX, VECTOR INDEX, SPATIAL INDEX, custom index types

### → System Function (Answer C)
**Auto-Executes**: [Function Guidelines](../instructions/function.guidelines.instructions.md)

**Automatic implementation includes**:
- Function AST design for new T-SQL functions
- Grammar rules with syntactic predicates for RETURN statement support
- ANTLR v2 lookahead limitations and solutions
- Script generation for function calls
- Comprehensive testing in all expression contexts

**Best for**: JSON_OBJECT, JSON_ARRAY, AI functions, mathematical functions

### → Grammar/Syntax Enhancement (Answer D)
**Auto-Executes**: [Bug Fixing Guidelines](../instructions/bug_fixing.guidelines.instructions.md)

**Automatic implementation includes**:
- Grammar rule modifications and AST updates
- Script generation implementation
- Testing framework integration
- Extending literals to expressions pattern
- Version compatibility considerations

**Best for**: New operators, statement types, expression enhancements

### → Validation Fix (Answer E)
**Auto-Executes**: [Validation Fix Guidelines](../instructions/validation_fix.guidelines.instructions.md)

**Automatic implementation includes**:
- Version-gated validation fixes
- SQL Server version compatibility checks
- Context-specific validation rules
- Testing validation behavior across versions
- No grammar changes needed

**Best for**: Feature works in one context but not another, version support issues

### → Parser Predicate Issue (Answer F)
**Auto-Executes**: [Parser Guidelines](../instructions/parser.guidelines.instructions.md)

**Automatic implementation includes**:
- Identifier-based predicate recognition fixes
- `IsNextRuleBooleanParenthesis()` function updates
- Syntactic vs semantic predicate handling
- Parentheses support in boolean contexts

**Best for**: Functions work without parentheses but fail with them

## Grammar Extension Patterns

For users implementing Grammar/Syntax Enhancement (Option D), here are common patterns:

### Pattern 1: Extending Literals to Expressions

#### When to Use
When existing grammar rules only accept literal values but need to support dynamic expressions like parameters, variables, or computed values.

#### Example Problem
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

#### ⚠️ Critical Warning: Avoid Modifying Shared Grammar Rules

**DO NOT** modify existing shared grammar rules like `identifierColumnReferenceExpression` that are used throughout the codebase. This can cause unintended side effects and break other functionality.

**Instead**, create specialized rules for your specific context.

#### Solution Template

**Step 1: Update AST Definition (`Ast.xml`)**
```xml
<!-- Before: -->
<Member Name="PropertyName" Type="IntegerLiteral" Summary="Description" />

<!-- After: -->
<Member Name="PropertyName" Type="ScalarExpression" Summary="Description" />
```

**Step 2: Create Context-Specific Grammar Rule (`TSql*.g`)**
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

**Step 3: Verify Script Generator**
Most script generators using `GenerateNameEqualsValue()` or similar methods work automatically with `ScalarExpression`. No changes typically needed.

#### Real-World Example: VECTOR_SEARCH TOP_N

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

### Pattern 2: Adding New Enum Members

#### When to Use
When adding new operators, keywords, or options to existing constructs.

#### Solution Template

**Step 1: Update Enum in AST (`Ast.xml`)**
```xml
<Enum Name="ExistingEnumType">
  <Member Name="ExistingValue1" />
  <Member Name="ExistingValue2" />
  <Member Name="NewValue" />  <!-- Add this -->
</Enum>
```

**Step 2: Update Grammar Rule (`TSql*.g`)**
```antlr
// Add new token matching
| tNewValue:Identifier
{
    Match(tNewValue, CodeGenerationSupporter.NewValue);
    vResult.EnumProperty = ExistingEnumType.NewValue;
}
```

**Step 3: Update Script Generator**
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

### Pattern 3: Adding New Function or Statement

#### When to Use
When adding completely new T-SQL functions or statements.

#### Solution Template

**Step 1: Define AST Node (`Ast.xml`)**
```xml
<Class Name="NewFunctionCall" Base="PrimaryExpression">
  <Member Name="Parameter1" Type="ScalarExpression" />
  <Member Name="Parameter2" Type="StringLiteral" />
</Class>
```

**Step 2: Add Grammar Rule (`TSql*.g`)**
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

**Step 3: Integrate with Existing Rules**
Add the new rule to appropriate places in the grammar (e.g., `functionCall`, `primaryExpression`, etc.).

**Step 4: Create Script Generator**
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

## Quick Decision Tree

**Start here** → Does the syntax exist in SQL Server documentation?

**No** → Use Grammar/Syntax Enhancement (D)

**Yes** → Does SqlScriptDOM recognize the syntax?

**No** → What type of syntax?
- Data type declaration → Data Type (A)
- CREATE [TYPE] INDEX → Index Type (B)  
- Function call → System Function (C)
- Other syntax → Grammar/Syntax Enhancement (D)

**Yes** → Does it parse without errors?

**No** → Parser Predicate Issue (F)

**Yes** → Does validation reject it incorrectly?

**Yes** → Validation Fix (E)

**No** → Review existing implementation or check for edge cases

## Pre-Implementation Checklist

Before starting implementation:

- [ ] Verified feature exists in SQL Server documentation
- [ ] Identified target SQL Server version for the feature
- [ ] Confirmed feature doesn't already exist in SqlScriptDOM
- [ ] Collected comprehensive syntax examples from SQL Server docs
- [ ] Reviewed similar existing implementations in the codebase
- [ ] Selected appropriate guideline based on feature type

## Testing Strategy

Regardless of feature type, ensure you:

- [ ] Create comprehensive test scripts covering all syntax variations
- [ ] Generate proper baseline files with expected formatted output
- [ ] Configure error counts for all SQL Server versions
- [ ] Run full test suite to prevent regressions
- [ ] Test edge cases, quoted identifiers, and schema qualification
- [ ] Verify round-trip parsing (parse → generate → parse)

## Additional Resources

- **Main Copilot Instructions**: [copilot-instructions.md](../copilot-instructions.md)
- **Testing Framework Guide**: [Testing Guidelines](../instructions/testing.guidelines.instructions.md)
- **Grammar Extension Patterns**: See Grammar Extension Patterns section above
- **Detailed Grammar Guidelines**: [Grammar Guidelines](../instructions/grammer.guidelines.instructions.md)

---

**Ready to implement?** Follow the guideline that matches your feature type above. Each guide provides step-by-step instructions, real-world examples, and comprehensive testing strategies.

## Implementation Workflow

**IMPORTANT**: After identifying your feature type above, this prompt will automatically begin implementation. Provide the following information to start:

### Required Information
1. **Feature Name**: _______________
2. **SQL Server Version**: _______________  
3. **Exact T-SQL Syntax Examples**:
```sql
-- Provide the EXACT syntax you want to support (copy-paste from user request)
-- Example: SELECT JSON_OBJECTAGG( t.c1 : t.c2 ) FROM (VALUES('key1', 'c'), ('key2', 'b'), ('key3','a')) AS t(c1, c2);
```
4. **Feature Type** (from analysis above): A, B, C, D, E, or F

### Automatic Implementation Process

Once you provide the information above, this prompt will:

#### Phase 1: Analysis and Verification (Always Done First)
1. **Verify current status** using the exact syntax provided
2. **Search existing codebase** for similar implementations  
3. **Identify SQL Server version** and parser target
4. **Create implementation plan** with specific steps
5. **Show the plan** and get confirmation before proceeding

#### Phase 2: Implementation (Executed Automatically)
Based on feature type identification:

**For Grammar/Syntax Enhancement (Type D)**:
1. **Update AST definition** (`Ast.xml`) if new nodes needed
2. **Add grammar rules** in appropriate `TSql*.g` files
3. **Create script generators** for new AST nodes
4. **Build and validate** parser compilation
5. **Create comprehensive tests** with exact syntax provided
6. **Generate baseline files** from parser output
7. **Run full test suite** to ensure no regressions

**For Validation Fix (Type E)**:
1. **Locate validation function** throwing the error
2. **Verify Microsoft documentation** for version support
3. **Apply version-gated validation** (not unconditional rejection)
4. **Create test cases** covering all scenarios
5. **Build and validate** the fix
6. **Run full test suite** to ensure correctness

**For System Function (Type C)**:
1. **Define AST node structure** for the function
2. **Add grammar rules** with syntactic predicates for RETURN statement support
3. **Create script generator** for the function
4. **Build and test** grammar changes
5. **Create comprehensive test scripts** including RETURN statement usage
6. **Validate full test suite** for regressions

**For Data Type (Type A)**:
1. **Define AST node** inheriting from `DataTypeReference`
2. **Create specialized parsing rule** for the data type
3. **Integrate with scalar data type rule**
4. **Add string constants** for keywords
5. **Create script generator**
6. **Build and comprehensive test** across all SQL contexts

**For Index Type (Type B)**:
1. **Define AST node** inheriting from `IndexStatement`
2. **Create specialized parsing rule** for the index type
3. **Integrate with main index grammar**
4. **Add index options** if needed
5. **Create script generator**
6. **Build and comprehensive test** all syntax variations

**For Parser Predicate Issue (Type F)**:
1. **Locate `IsNextRuleBooleanParenthesis()`** function
2. **Add identifier-based predicate detection**
3. **Build and test** the fix
4. **Create tests** covering parentheses scenarios
5. **Validate** existing functionality

#### Phase 3: Validation and Documentation
1. **Run complete test suite** (`dotnet test Test/SqlDom/UTSqlScriptDom.csproj -c Debug`)
2. **Verify all tests pass** (expect 1,100+ tests to succeed)
3. **Document changes made** with before/after examples
4. **Provide usage examples** showing the new functionality

### Starting Implementation

To begin implementation, provide your feature details using this format:

```
Feature Name: [FUNCTION_NAME or FEATURE_NAME]
SQL Server Version: [SQL Server 20XX / TSqlXXX]
Exact T-SQL Syntax:
```sql
[EXACT_COPY_OF_SYNTAX_FROM_USER_REQUEST]
```
Feature Type: [A/B/C/D/E/F based on analysis above]
```

**The prompt will then automatically execute the appropriate implementation workflow and start making the necessary code changes.**

### Implementation Principles

1. **Always test exact syntax first**: Use the exact T-SQL provided, not simplified versions
2. **Follow established patterns**: Reuse existing patterns from similar implementations
3. **Maintain backward compatibility**: Ensure existing functionality continues to work
4. **Comprehensive testing**: Test all syntax variations, edge cases, and error conditions
5. **Version compatibility**: Consider which SQL Server versions should support the feature
6. **Full regression testing**: Always run the complete test suite before completion