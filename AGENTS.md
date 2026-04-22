# AGENTS.md

This file is the canonical repository guidance for SqlScriptDOM.

- Treat `TSql180` as the current vNext/latest parser target in agent workflows unless a more specific repo instruction overrides it.

## Project Overview

ScriptDom is a library for parsing and generating T-SQL scripts. It is primarily used by DacFx to build database projects, perform schema comparisons, and generate scripts for deployment.

## Key Points
- Grammar files live in: `SqlScriptDom/Parser/TSql/` — each file corresponds to a SQL Server version (e.g. `TSql170.g` for 170 / SQL Server 2025).
- Grammar format: ANTLR v2. Generated C# lexer/parser code is produced during the build (see `GenerateFiles.props`).
- Build and tests: use the .NET SDK pinned in `global.json`. Typical commands from repo root:
	- `dotnet build -c Debug`
	- `dotnet test Test/SqlDom/UTSqlScriptDom.csproj -c Debug`
- To regenerate parser/token/AST sources explicitly, build the main project (generation targets are hooked into its build):
	- `dotnet build SqlScriptDom/Microsoft.SqlServer.TransactSql.ScriptDom.csproj -c Debug`
	- `dotnet msbuild SqlScriptDom/Microsoft.SqlServer.TransactSql.ScriptDom.csproj -t:GLexerParserCompile;GSqlTokenTypesCompile;CreateAST -p:Configuration=Debug`

## Why Files Are Generated And Where
- `SqlScriptDom/GenerateFiles.props` contains the MSBuild targets invoked during the library build:
	- `GSqlTokenTypesCompile` / `GLexerParserCompile` -> run ANTLR and post-process outputs (powershell/sed scripts)
	- `CreateAST` -> runs AstGen tool (from `tools/AstGen`) to generate AST visitor/fragment classes
	- `GenerateEverything` -> runs ScriptGenSettingsGenerator and TokenListGenerator
- The Antlr binary is downloaded to the path defined in `Directory.Build.props` (`AntlrLocation`) when the build runs (via the `InstallAntlr` target).
- Generated C# files are written to `$(CsGenIntermediateOutputPath)` (under `obj/...` by default). Do not hand-edit generated files — change the `.g` grammar or post-processing scripts instead.

## Important Files And Folders
- `SqlScriptDom/Parser/TSql/*.g` — ANTLR v2 grammar files (TSql80..TSql170 etc.). Example: `TSql170.g` defines new-170 syntax.
- `SqlScriptDom/GenerateFiles.props` and `Directory.Build.props` — define code generation targets and antlr location.
- `SqlScriptDom/ParserPostProcessing.sed`, `LexerPostProcessing.sed`, `TSqlTokenTypes.ps1` — post-processing for generated C# sources and tokens.
- `tools/` — contains code generators used during build: `AstGen`, `ScriptGenSettingsGenerator`, `TokenListGenerator`.
- `Test/SqlDom/` — unit tests, baselines and test scripts. See `Only170SyntaxTests.cs`, `TestScripts/`, and `Baselines170/`.

## Developer Workflow And Conventions
1. Add or modify grammar rule(s) in the correct `TSql*.g` file for the SQL version that owns the syntax.
2. If tokens or token ordering change, update `TSqlTokenTypes.g` and the sed/ps1 post-processors if necessary.
3. Rebuild the ScriptDom project to regenerate parser and AST sources. `dotnet build` will run generation automatically.
4. Add tests:
	 - **YOU MUST ADD UNIT TESTS** - Use the existing test framework in `Test/SqlDom/`
	 - **DO NOT CREATE STANDALONE PROGRAMS TO TEST** - Avoid separate console applications or debug programs
	 - Put the input SQL in `Test/SqlDom/TestScripts/` (filename is case sensitive and used as an embedded resource).
	 - Add or confirm baseline output in `Test/SqlDom/Baselines<version>/` (the UT project embeds these baselines as resources).
	 - Update the appropriate `Only<version>SyntaxTests.cs` (e.g., `Only170SyntaxTests.cs`) by adding a `ParserTest170("MyNewTest.sql", ...)` entry. See `ParserTest.cs` and `ParserTestOutput.cs` for helper constructors and verification semantics.
	 - For comprehensive testing guidance, see [Testing Guidelines](.agents/skills/testing/SKILL.md).
5. Run the full test suite to ensure no regressions:
	 ```bash
	 dotnet test Test/SqlDom/UTSqlScriptDom.csproj -c Debug
	 ```
	 - Grammar changes can break unrelated functionality when shared rules are modified.
	 - If tests fail unexpectedly, create context-specific grammar rules instead of modifying shared ones.
6. Iterate until all tests pass, including both new functionality and existing regression tests.

## Testing Details
- Tests run a full parse -> script generator -> reparse round-trip. Baseline comparison verifies pretty-printed generated scripts exactly match the stored baseline.
- Expected parse errors (where applicable) are verified by number and exact error messages; test helpers live in `ParserTest.cs`, `ParserTestOutput.cs`, and `ParserTestUtils.cs`.
- If a test fails due to mismatch in generated script, compare the generated output against the baseline to spot formatting or structure differences.

## Bug Fixing And Baseline Generation

Different types of bugs require different fix approaches. Start by diagnosing which type of issue you are dealing with.

### 1. Validation-Based Issues
If you see an error like "Option 'X' is not valid..." or "Feature 'Y' not supported..." but the syntax should work according to SQL Server docs:
- Guide: [Validation Fix Guide](.agents/skills/grammar_validation/SKILL.md)
- Example: ALTER TABLE RESUMABLE option (SQL Server 2022+)
- Key signal: Similar syntax works in other contexts (e.g., ALTER INDEX works but ALTER TABLE doesn't)

### 2. Grammar-Based Issues
If the parser does not recognize the syntax at all, or you need to add new T-SQL features:
- Guide: [Bug Fixing Guide](.agents/skills/bug_fixing/SKILL.md)
- Example: Adding new operators, statements, or function types
- Key signal: Syntax error like "Incorrect syntax near..." or "Unexpected token..."

### 3. Parser Predicate Recognition Issues
If identifier-based predicates (like `REGEXP_LIKE`) work without parentheses but fail with them:
- Guide: [Parser Predicate Recognition Fix Guide](.agents/skills/parser/SKILL.md)
- Example: `WHERE REGEXP_LIKE('a', 'pattern')` works, but `WHERE (REGEXP_LIKE('a', 'pattern'))` fails
- Key signal: Syntax error near closing parenthesis or semicolon

Quick diagnostic: search for the error message in the codebase to determine which type of fix is needed.

### 4. Adding New System Functions
For adding new T-SQL system functions to the parser, including handling RETURN statement contexts and ANTLR v2 syntactic predicate limitations:
- Guide: [Function Guidelines](.agents/skills/function/SKILL.md)
- Example: JSON_OBJECT, JSON_ARRAY functions with RETURN statement support
- Key requirements: Syntactic predicates for lookahead, proper AST design, comprehensive testing

### 5. Adding New Data Types
For adding completely new SQL Server data types that require custom parsing logic and specialized AST nodes:
- Guide: [New Data Types Guidelines](.agents/skills/new_data_types/SKILL.md)
- Example: VECTOR data type with dimension and optional base type parameters
- Key signal: New SQL Server data type with custom parameter syntax different from standard data types

### 6. Adding New Index Types
For adding completely new SQL Server index types that require specialized syntax and custom parsing logic:
- Guide: [New Index Types Guidelines](.agents/skills/new_index_types/SKILL.md)
- Example: JSON INDEX with FOR clause, VECTOR INDEX with METRIC/TYPE options
- Key signal: New SQL Server index type with custom syntax different from standard CREATE INDEX

## Editing Generated Outputs
- Never edit generated files permanently (they live under `obj/...` / `CsGenIntermediateOutputPath`). Instead change:
	- `.g` grammar files
	- post-processing scripts (`*.ps1` / `*.sed`)
	- AST XML in `SqlScriptDom/Parser/TSql/Ast.xml` if AST node shapes need to change
- To see ANTLR output or errors, force verbose generation with `-p:OutputErrorInLexerParserCompile=true`.
- If the ANTLR download fails during build, manually download `antlr-2.7.5.jar` (non-Windows) or `.exe` (Windows) and place it at the location defined in `Directory.Build.props`, or override `AntlrLocation` when invoking msbuild.

## Debugging Tips And Investigation Workflow

### Step 1: Identify The Bug Type
Start by searching for the error message to understand what type of fix is needed:
```bash
grep -r "SQL46057" SqlScriptDom/
grep -r "is not a valid" SqlScriptDom/
```

Common error patterns:
- `"Option 'X' is not valid..."` -> Validation issue (see [grammar_validation/SKILL.md](.agents/skills/grammar_validation/SKILL.md))
- `"Incorrect syntax near..."` -> Grammar issue (see [bug_fixing/SKILL.md](.agents/skills/bug_fixing/SKILL.md))
- `"Syntax error near ')'"` with parentheses -> Predicate recognition (see [parser/SKILL.md](.agents/skills/parser/SKILL.md))

### Step 2: Find Where Similar Syntax Works
If the syntax works in one context but not another:
```bash
grep -r "RESUMABLE" Test/SqlDom/TestScripts/
grep -r "OptionName" SqlScriptDom/Parser/TSql/
```

Example: ALTER INDEX with RESUMABLE works, but ALTER TABLE does not -> likely validation issue.

### Step 3: Locate The Relevant Code
Common files to check:
- Validation: `SqlScriptDom/Parser/TSql/TSql80ParserBaseInternal.cs`
- Grammar: `SqlScriptDom/Parser/TSql/TSql*.g`
- Options: `SqlScriptDom/ScriptDom/SqlServer/IndexOptionHelper.cs`
- AST: `SqlScriptDom/Parser/TSql/Ast.xml`

### Step 4: Check SQL Server Version Support
Always verify Microsoft documentation:
- Search for `Applies to: SQL Server 20XX (XX.x)` in Microsoft docs.
- Note that different features within the same option set can have different version requirements.
- Example: MAX_DURATION (SQL 2014+) vs RESUMABLE (SQL 2022+)

### Step 5: Verify With Tests
Before and after making changes:
```bash
dotnet build SqlScriptDom/Microsoft.SqlServer.TransactSql.ScriptDom.csproj -c Debug
dotnet test --filter "FullyQualifiedName~YourTest" -c Debug
dotnet test Test/SqlDom/UTSqlScriptDom.csproj -c Debug
```

### Common Investigation Patterns

#### Pattern 1: Option Not Recognized
```bash
grep -r "YourOptionName" SqlScriptDom/ScriptDom/SqlServer/IndexOptionHelper.cs
grep -r "enum IndexOptionKind" SqlScriptDom/
```

#### Pattern 2: Version-Specific Behavior
```bash
grep -r "TSql160AndAbove" SqlScriptDom/Parser/TSql/
```

Check which parser version you are testing:
- `TSql80` = SQL Server 2000
- `TSql90` = SQL Server 2005
- `TSql100` = SQL Server 2008
- `TSql110` = SQL Server 2012
- `TSql120` = SQL Server 2014
- `TSql130` = SQL Server 2016
- `TSql140` = SQL Server 2017
- `TSql150` = SQL Server 2019
- `TSql160` = SQL Server 2022
- `TSql170` = SQL Server 2025

#### Pattern 3: Statement-Specific Restrictions
```bash
grep -r "IndexAffectingStatement" SqlScriptDom/Parser/TSql/TSql80ParserBaseInternal.cs
```

Common statement types: `CreateIndex`, `AlterIndex`, `AlterTableAddElement`.

## Patterns And Code Style To Follow
- Grammar rule pattern: `ruleName returns [Type vResult = this.FragmentFactory.CreateFragment<Type>()] { ... } : ( alternatives ) ;`
- Parser-generated code frequently uses `Match(<token>, CodeGenerationSupporter.<Symbol>)` and `ThrowParseErrorException("SQLxxxx", ...)` for diagnostics.
- The codebase prefers using the factory and fragment visitors for AST creation and script generation. Look at `ScriptDom/SqlServer/ScriptGenerator` for script generation patterns.

## Grammar Gotchas And Common Pitfalls
- Be careful to distinguish between standard T-SQL operators and function-style predicates used in specific contexts.
- Do not assume logical `NOT` support automatically implies support for compound operators like `NOT LIKE` in all predicate contexts.
- **Never modify shared grammar rules** like `identifierColumnReferenceExpression` unless you are certain of the impact. Prefer specialized rules for specific contexts.
- After any grammar changes, always run the complete test suite.
- When functions or constructs need to accept dynamic values instead of only literals, change both the AST definition and grammar rules to use `ScalarExpression` instead. See [bug_fixing/SKILL.md](.agents/skills/bug_fixing/SKILL.md#special-case-extending-grammar-rules-from-literals-to-expressions) and [grammer/SKILL.md](.agents/skills/grammer/SKILL.md).x