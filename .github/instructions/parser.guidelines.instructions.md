# Parser Predicate Recognition Bug Fix Guide

This guide documents the specific pattern for fixing bugs where identifier-based predicates (like `REGEXP_LIKE`) are not properly recognized when wrapped in parentheses in boolean expressions.

## Problem Description

**Symptom**: Parentheses around identifier-based boolean predicates cause syntax errors.
- Example: `SELECT 1 WHERE (REGEXP_LIKE('a', 'pattern'))` fails to parse
- Works: `SELECT 1 WHERE REGEXP_LIKE('a', 'pattern')` (without parentheses)

**Root Cause**: The `IsNextRuleBooleanParenthesis()` function in `TSql80ParserBaseInternal.cs` only recognizes:
- Keyword-based predicates (tokens): `LIKE`, `BETWEEN`, `CONTAINS`, `EXISTS`, etc.
- One identifier-based predicate: `IIF`
- But doesn't recognize newer identifier-based predicates like `REGEXP_LIKE`

## Understanding the Fix

### The `IsNextRuleBooleanParenthesis()` Function

This function determines whether parentheses contain a boolean expression vs. a scalar expression. It scans forward from a `LeftParenthesis` token looking for boolean operators or predicates.

**Location**: `SqlScriptDom/Parser/TSql/TSql80ParserBaseInternal.cs`

**Key Logic**:
```csharp
case TSql80ParserInternal.Identifier:
    // if identifier is IIF
    if(NextTokenMatches(CodeGenerationSupporter.IIf))
    {
        ++insideIIf;
    }
    // ADD NEW IDENTIFIER-BASED PREDICATES HERE
    break;
```

### The Solution Pattern

For identifier-based boolean predicates, add detection logic in the `Identifier` case:

```csharp
case TSql80ParserInternal.Identifier:
    // if identifier is IIF
    if(NextTokenMatches(CodeGenerationSupporter.IIf))
    {
        ++insideIIf;
    }
    // if identifier is REGEXP_LIKE
    else if(NextTokenMatches(CodeGenerationSupporter.RegexpLike))
    {
        if (caseDepth == 0 && topmostSelect == 0 && insideIIf == 0)
        {
            matches = true;
            loop = false;
        }
    }
    break;
```

## Step-by-Step Fix Process

### 1. Reproduce the Issue
Create a test case to confirm the bug:
```sql
SELECT 1 WHERE (REGEXP_LIKE('a', 'pattern'));  -- Should fail without fix
```

### 2. Identify the Predicate Constant
Find the predicate identifier in `CodeGenerationSupporter`:
```csharp
// In CodeGenerationSupporter.cs
public const string RegexpLike = "REGEXP_LIKE";
```

### 3. Apply the Fix
Modify `TSql80ParserBaseInternal.cs` in the `IsNextRuleBooleanParenthesis()` method:

**File**: `SqlScriptDom/Parser/TSql/TSql80ParserBaseInternal.cs`
**Method**: `IsNextRuleBooleanParenthesis()`
**Location**: Around line 808, in the `case TSql80ParserInternal.Identifier:` block

Add the predicate detection logic following the pattern shown above.

### 4. Update Test Cases
Add test cases covering the parentheses scenario:

**Test Script**: `Test/SqlDom/TestScripts/RegexpLikeTests170.sql`
```sql
SELECT 1 WHERE (REGEXP_LIKE('a', '%pattern%'));
```

**Baseline**: `Test/SqlDom/Baselines170/RegexpLikeTests170.sql`
```sql
SELECT 1
WHERE (REGEXP_LIKE ('a', '%pattern%'));
```

**Test Configuration**: Update error counts in `Only170SyntaxTests.cs` if the new test cases affect older parser versions.

### 5. Build and Verify
```bash
# Build the parser
dotnet build SqlScriptDom/Microsoft.SqlServer.TransactSql.ScriptDom.csproj -c Debug

# Run the specific test
dotnet test Test/SqlDom/UTSqlScriptDom.csproj --filter "FullyQualifiedName~SqlStudio.Tests.UTSqlScriptDom.SqlDomTests.TSql170SyntaxIn170ParserTest" -c Debug
```

## When to Apply This Pattern

This fix pattern applies when:

1. **Identifier-based predicates**: The predicate is defined as an identifier (not a keyword token)
2. **Boolean context**: The predicate returns a boolean value for use in WHERE clauses, CHECK constraints, etc.
3. **Parentheses fail**: The predicate works without parentheses but fails with parentheses
4. **Already implemented**: The predicate grammar and AST are already correctly implemented

## Common Predicates That May Need This Fix

- `REGEXP_LIKE` (✅ Fixed)
- Future identifier-based boolean functions
- Custom function predicates that return boolean values

## Related Files Modified

This type of fix typically involves:

1. **Core Parser Logic**:
   - `SqlScriptDom/Parser/TSql/TSql80ParserBaseInternal.cs` - Main fix

2. **Test Infrastructure**:
   - `Test/SqlDom/TestScripts/[TestName].sql` - Input test cases
   - `Test/SqlDom/Baselines[Version]/[TestName].sql` - Expected output
   - `Test/SqlDom/Only[Version]SyntaxTests.cs` - Test configuration

3. **Potentially Affected**:
   - `Test/SqlDom/TestScripts/BooleanExpressionTests.sql` - May need additional test cases
   - `Test/SqlDom/BaselinesCommon/BooleanExpressionTests.sql` - Corresponding baselines

## Verification Checklist

- [ ] Parentheses syntax parses without errors
- [ ] Non-parentheses syntax still works
- [ ] Test suite passes for target SQL version
- [ ] Older SQL versions have appropriate error counts
- [ ] Related boolean expression tests still pass

## Notes and Gotchas

- **IIF Special Handling**: `IIF` has special logic (`++insideIIf`) because it's not a simple boolean predicate
- **Context Conditions**: The fix includes conditions (`caseDepth == 0 && topmostSelect == 0 && insideIIf == 0`) to ensure proper parsing context
- **Token vs Identifier**: Keyword predicates are handled as tokens, identifier predicates need special detection
- **Cross-Version Impact**: Adding test cases may increase error counts for older SQL Server parsers

This pattern ensures that identifier-based boolean predicates work consistently with parentheses, maintaining parser compatibility across different syntactic contexts.