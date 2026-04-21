---
name: add-feature
description: Add new T-SQL features to ScriptDOM parser. Asks questions about SQL Server version and feature type, then guides through grammar changes, AST updates, script generation, and testing using existing instruction files. Handles syntax additions, new functions, data types, index types, and validation rules.
argument-hint: "Feature description (e.g., 'Add VECTOR data type', 'Add JSON_OBJECT function')"
user-invocable: true
platforms: "SQL Server, Azure SQL DB, Fabric, Fabric DW, VNext"
---

## Overview

This skill helps add new T-SQL features to the ScriptDOM parser by:
1. **Interviewing** you about the feature and target platform (SQL Server, Azure SQL DB, Fabric, Fabric DW, VNext)
2. **Determining the latest parser version** from `SqlVersionFlags.cs`
3. **Routing** you to the appropriate parser:
   - SQL Server → version-specific parser (TSql120-180)
   - Azure SQL DB/Fabric/VNext → latest parser version
   - Fabric DW → separate TSqlFabricDW parser
4. **Classifying** the change type (grammar, validation, function, data type, etc.)
5. **Guiding** through implementation and testing without duplicating existing documentation

---

## Step 1: Feature Discovery Interview

Ask the user these questions to understand the feature:

### Required Questions
1. **What T-SQL feature are you adding?**
   - Example: "VECTOR data type", "JSON_OBJECT function", "RESUMABLE option for ALTER TABLE"

2. **Which platform is this feature for?**
   - **SQL Server** (on-premises / standalone)
   - **Azure SQL Database** (uses latest parser version)
   - **Fabric** (uses latest parser version)
   - **Fabric DW** (uses separate TSqlFabricDW parser)
   - **VNext** (future version, uses latest parser version unless repo instructions say otherwise)

3. **If SQL Server, which version introduced this feature?**
   - SQL Server 2014 (TSql120)
   - SQL Server 2016 (TSql130)
   - SQL Server 2017 (TSql140)
   - SQL Server 2019 (TSql150)
   - SQL Server 2022 (TSql160)
   - SQL Server 2025 (TSql170)
   - SQL Server vNext (TSql180)
   - *(Skip if Azure SQL DB, Fabric, or VNext - these use TSql180 by default)*

4. **Do you have example T-SQL syntax or Microsoft documentation links?**
   - This helps verify the exact syntax requirements

### Optional Context Questions
4. **Is this a new syntax element or enabling existing syntax in a new context?**
   - New syntax: Requires grammar rules, AST nodes
   - Validation fix: May only need version checks in validation code

5. **Does similar syntax already work in other contexts?**
   - Example: "RESUMABLE works for ALTER INDEX but not ALTER TABLE"
   - This indicates a validation issue rather than missing grammar

---

## Step 2: Determine Feature Type

Based on the answers, classify the feature into one of these types:

### Type A: Validation-Only Fix
**Indicators:**
- Similar syntax already works elsewhere in SQL
- Error message says "Option 'X' is not valid..." or "Feature 'Y' not supported..."
- The grammar may already parse it, but validation blocks it

**→ Route to:** [grammar_validation.guidelines.instructions.md](../../instructions/grammar_validation.guidelines.instructions.md)

### Type B: New Grammar/Syntax
**Indicators:**
- Completely new statement, clause, or operator
- Error says "Incorrect syntax near..." or "Unexpected token..."
- Parser doesn't recognize the syntax at all

**→ Route to:** [bug_fixing.guidelines.instructions.md](../../instructions/bug_fixing.guidelines.instructions.md)

### Type C: New System Function
**Indicators:**
- Adding a new built-in T-SQL function (e.g., JSON_OBJECT, STRING_AGG)
- May need special handling for RETURN statement contexts

**→ Route to:** [function.guidelines.instructions.md](../../instructions/function.guidelines.instructions.md)

### Type D: New Data Type
**Indicators:**
- Adding a completely new SQL Server data type (e.g., VECTOR, GEOGRAPHY)
- Requires custom parameter syntax (e.g., `VECTOR(1536, FLOAT32)`)

**→ Route to:** [new_data_types.guidelines.instructions.md](../../instructions/new_data_types.guidelines.instructions.md)

### Type E: New Index Type
**Indicators:**
- Adding a new CREATE INDEX variant (e.g., VECTOR INDEX, JSON INDEX)
- Requires specialized syntax different from standard indexes

**→ Route to:** [new_index_types.guidelines.instructions.md](../../instructions/new_index_types.guidelines.instructions.md)

### Type F: Parser Predicate Recognition (Parentheses)
**Indicators:**
- Identifier-based predicate works without parentheses but fails with them
- Error near closing parenthesis: `WHERE (REGEXP_LIKE(...))` fails

**→ Route to:** [parser.guidelines.instructions.md](../../instructions/parser.guidelines.instructions.md)

### Type G: Database Option (ALTER/CREATE DATABASE SET)
**Indicators:**
- Adding a database option for ALTER DATABASE or CREATE DATABASE statements
- Simple ON/OFF option (e.g., `SET AUTOMATIC_INDEX_COMPACTION = ON`)
- Complex option with sub-options (e.g., `SET CHANGE_TRACKING (AUTO_CLEANUP = ON)`)
- Enum-based option (e.g., `SET RECOVERY FULL`)

**→ Route to:** [database_option.guidelines.instructions.md](../../instructions/database_option.guidelines.instructions.md)

---

## Step 3: Grammar Rule Development

For grammar changes (Types B, C, D, E, G), follow this workflow:

### 3.1 Identify Target Grammar File

**First, determine the latest parser version:**
Check `SqlScriptDom/Parser/TSql/SqlVersionFlags.cs` for the highest TSql version enum value.

**Then select the appropriate grammar file:**

#### For SQL Server (on-premises):
- SQL 2014+: TSql120.g
- SQL 2016+: TSql130.g
- SQL 2017+: TSql140.g
- SQL 2019+: TSql150.g
- SQL 2022+: TSql160.g
- SQL 2025+: TSql170.g
- SQL Server vNext: TSql180.g

#### For Azure SQL Database, Fabric, VNext:
- Use the **latest parser version** (currently TSql180.g)
- Check `SqlVersionFlags.cs` to confirm the highest version

#### For Fabric DW:
- Use **TSqlFabricDW.g** (separate parser with Fabric-specific syntax)

### 3.2 Grammar Development Patterns
**Reference:** [grammar.guidelines.instructions.md](../../instructions/grammar.guidelines.instructions.md)

Key patterns:
- Use `this.FragmentFactory.CreateFragment<Type>()` for AST nodes
- Use syntactic predicates `(LA(1) == Token)` for lookahead
- Avoid modifying shared grammar rules (create specialized versions instead)
- Include proper script generation in `ScriptGenerator`

### 3.3 AST Updates
If adding new AST nodes:
1. Edit `SqlScriptDom/Parser/TSql/Ast.xml`
2. Rebuild to regenerate visitor classes
3. Implement script generation in `SqlScriptDom/ScriptDom/SqlServer/ScriptGenerator`

---

## Step 4: Testing

**Reference:** [testing.guidelines.instructions.md](../../instructions/testing.guidelines.instructions.md)

### 4.1 Create Test Files
1. **Input Script**: `Test/SqlDom/TestScripts/YourFeature.sql`
   - Include all syntax variations
   - Test edge cases, parameters, expressions

2. **Baseline**: `Test/SqlDom/Baselines<version>/YourFeature.sql`
   - Expected pretty-printed output after parse → script gen → reparse

### 4.2 Add Test Method

#### For SQL Server versions (TSql120-180):
In `Test/SqlDom/Only<version>SyntaxTests.cs`:

```csharp
[TestMethod]
public void YourFeatureTest()
{
    ParserTest<version>("YourFeature.sql");
}
```

#### For Fabric DW:
Add to the `OnlyFabricDWTestInfos` array in `Test/SqlDom/OnlyFabricDWSyntaxTests.cs`:

```csharp
new ParserTestFabricDW("YourFeatureFabricDW.sql", 
    nErrors80: X, nErrors90: Y, nErrors100: Z, 
   nErrors110: A, nErrors120: B, nErrors130: C, 
   nErrors140: D, nErrors150: E, nErrors160: F, nErrors170: G, nErrors180: H),
```
*(Set expected error counts for each parser version)*

**Simplified patterns available** - see testing guidelines for:
- `ParserTestAllowingErrors` - for scripts with expected parse errors
- `ParserTestOutput` constructors - for custom output verification
- Error message verification - exact match or fragment-based

### 4.3 Run Tests
```bash
# Build to regenerate parser
dotnet build SqlScriptDom/Microsoft.SqlServer.TransactSql.ScriptDom.csproj -c Debug

# Run specific test
dotnet test --filter "FullyQualifiedName~YourFeatureTest" -c Debug

# ALWAYS run full suite before committing
dotnet test Test/SqlDom/UTSqlScriptDom.csproj -c Debug
```

---

## Step 5: Debugging and Troubleshooting

**Reference:** [debugging_workflow.guidelines.instructions.md](../../instructions/debugging_workflow.guidelines.instructions.md)

Common issues:
- **Tests fail after grammar changes**: Likely modified a shared rule - create specialized version
- **Script generation mismatch**: Update `ScriptGenerator` for your AST nodes
- **Version errors**: Check validation code in `TSql80ParserBaseInternal.cs`

---

## Step 6: Implementation Checklist

Before marking the feature complete:

- [ ] Grammar rules added/modified in correct `TSql<version>.g` or `TSqlFabricDW.g` file
- [ ] AST nodes defined in `Ast.xml` (if new nodes needed)
- [ ] Script generation implemented for new AST nodes
- [ ] Validation rules updated (if version-gated)
- [ ] Test script created in `TestScripts/` (use appropriate suffix: `.sql` or `FabricDW.sql`)
- [ ] Baseline created in `Baselines<version>/` or `BaselinesFabricDW/`
- [ ] Test method added to `Only<version>SyntaxTests.cs` or `OnlyFabricDWSyntaxTests.cs`
- [ ] All tests pass (including full test suite)
- [ ] No regressions in unrelated tests

---

## Quick Reference: File Locations

| Component | Path |
|-----------|------|
| Grammar files (SQL Server) | `SqlScriptDom/Parser/TSql/TSql*.g` |
| Grammar file (Fabric DW) | `SqlScriptDom/Parser/TSql/TSqlFabricDW.g` |
| Version detection | `SqlScriptDom/Parser/TSql/SqlVersionFlags.cs` |
| AST definition | `SqlScriptDom/Parser/TSql/Ast.xml` |
| Script generator | `SqlScriptDom/ScriptDom/SqlServer/ScriptGenerator` |
| Validation code | `SqlScriptDom/Parser/TSql/TSql80ParserBaseInternal.cs` |
| Test scripts | `Test/SqlDom/TestScripts/` |
| Baselines | `Test/SqlDom/Baselines<version>/` or `BaselinesFabricDW/` |
| Test classes | `Test/SqlDom/Only<version>SyntaxTests.cs` or `FabricDWSyntaxTests.cs` |

---

## Agent Workflow

When invoked, the agent should:

1. **Determine the latest parser version** by reading `SqlScriptDom/Parser/TSql/SqlVersionFlags.cs`
2. **Interview the user** using the questions in Step 1
   - Ask about platform (SQL Server, Azure SQL DB, Fabric, Fabric DW, VNext)
   - For Azure/Fabric/VNext → use latest parser version
   - For Fabric DW → use TSqlFabricDW.g
   - For SQL Server → ask for specific version
3. **Classify the feature type** using Step 2 criteria
4. **Load the appropriate instruction file** using `read_file` tool
5. **Guide through implementation** by referencing specific sections of the loaded instruction file
6. **Execute the development workflow**:
   - Make grammar/validation changes
   - Regenerate parser (build)
   - Create test files
   - Run tests
   - Debug failures
7. **Verify completeness** using Step 6 checklist

**Never duplicate content from instruction files** - always reference and quote specific sections.
