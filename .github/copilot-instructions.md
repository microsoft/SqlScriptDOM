# Copilot / AI instructions for SqlScriptDOM

ScriptDom is a library for parsing and generating T-SQL scripts. It is primarily used by DacFx to build database projects, perform schema comparisons, and generate scripts for deployment.

## Key points (quick read)
- Grammar files live in: `SqlScriptDom/Parser/TSql/` — each file corresponds to a SQL Server version (e.g. `TSql170.g` for 170 / SQL Server 2025).
- Grammar format: ANTLR v2. Generated C# lexer/parser code is produced during the build (see `GenerateFiles.props`).
- Build & tests: use the .NET SDK pinned in `global.json`. Typical commands from repo root:
  - `dotnet build -c Debug`
  - `dotnet test Test/SqlDom/UTSqlScriptDom.csproj -c Debug`
- To regenerate parser/token/AST sources explicitly, build the main project (generation targets are hooked into its build):
  - `dotnet build SqlScriptDom/Microsoft.SqlServer.TransactSql.ScriptDom.csproj -c Debug`
  - (or) `dotnet msbuild SqlScriptDom/Microsoft.SqlServer.TransactSql.ScriptDom.csproj -t:GLexerParserCompile;GSqlTokenTypesCompile;CreateAST -p:Configuration=Debug`

## Why files are generated and where
- `SqlScriptDom/GenerateFiles.props` contains the MSBuild targets invoked during the library build:
  - `GSqlTokenTypesCompile` / `GLexerParserCompile` -> run ANTLR and post-process outputs (powershell/sed scripts)
  - `CreateAST` -> runs AstGen tool (from `tools/AstGen`) to generate AST visitor/fragment classes
  - `GenerateEverything` -> runs ScriptGenSettingsGenerator and TokenListGenerator
- The Antlr binary is downloaded to the path defined in `Directory.Build.props` (`AntlrLocation`) when the build runs (via the `InstallAntlr` target).
- Generated C# files are written to `$(CsGenIntermediateOutputPath)` (under `obj/...` by default). Do not hand-edit generated files — change the .g grammar or post-processing scripts instead.

## Important files and folders (read these first)
- `SqlScriptDom/Parser/TSql/*.g` — ANTLR v2 grammar files (TSql80..TSql170 etc.). Example: `TSql170.g` defines new-170 syntax.
- `SqlScriptDom/GenerateFiles.props` and `Directory.Build.props` — define code generation targets and antlr location.
- `SqlScriptDom/ParserPostProcessing.sed`, `LexerPostProcessing.sed`, `TSqlTokenTypes.ps1` — post-processing for generated C# sources and tokens.
- `tools/` — contains code generators used during build: `AstGen`, `ScriptGenSettingsGenerator`, `TokenListGenerator`.
- `Test/SqlDom/` — unit tests, baselines and test scripts. See `Only170SyntaxTests.cs`, `TestScripts/`, and `Baselines170/`.
- `.github/instructions/testing.guidelines.instructions.md` — comprehensive testing framework guide with patterns and best practices.
- `.github/instructions/function.guidelines.instructions.md` — specialized guide for adding new T-SQL system functions.

## Developer workflow & conventions (typical change cycle)
1. Add/modify grammar rule(s) in the correct `TSql*.g` (pick the _version_ the syntax belongs to).
2. If tokens or token ordering change, update `TSqlTokenTypes.g` (and the sed/ps1 post-processors if necessary).
3. Rebuild the ScriptDom project to regenerate parser and AST (`dotnet build` will run generation). Use the targeted msbuild targets if you only want generation.
4. Add tests:
   - **YOU MUST ADD UNIT TESTS** - Use the existing test framework in `Test/SqlDom/`
   - **DO NOT CREATE STANDALONE PROGRAMS TO TEST** - Avoid separate console applications or debug programs
   - Put the input SQL in `Test/SqlDom/TestScripts/` (filename is case sensitive and used as an embedded resource).
   - Add/confirm baseline output in `Test/SqlDom/Baselines<version>/` (the UT project embeds these baselines as resources).
   - Update the appropriate `Only<version>SyntaxTests.cs` (e.g., `Only170SyntaxTests.cs`) by adding a `ParserTest170("MyNewTest.sql", ...)` entry. See `ParserTest.cs` and `ParserTestOutput.cs` for helper constructors and verification semantics.
   - **For comprehensive testing guidance**, see [Testing Guidelines](instructions/testing.guidelines.instructions.md) with detailed patterns, best practices, and simplified constructor approaches.
5. **Run full test suite** to ensure no regressions:
   ```bash
   dotnet test Test/SqlDom/UTSqlScriptDom.csproj -c Debug
   ```
   - ⚠️ **CRITICAL**: Grammar changes can break unrelated functionality when shared rules are modified
   - If tests fail unexpectedly, create context-specific grammar rules instead of modifying shared ones
6. Iterate until all tests pass, including both new functionality and existing regression tests.

## Testing details and how tests assert correctness
- Tests run a full parse -> script generator -> reparse round-trip. Baseline comparison verifies pretty-printed generated scripts exactly match the stored baseline.
- Expected parse errors (where applicable) are verified by number and exact error messages; test helpers live in `ParserTest.cs`, `ParserTestOutput.cs`, and `ParserTestUtils.cs`.
- If a test fails due to mismatch in generated script, compare the generated output (the test harness logs it) against the baseline to spot formatting/structure differences.

## Bug Fixing and Baseline Generation

Different types of bugs require different fix approaches. **Start by diagnosing which type of issue you're dealing with:**

### 1. Validation-Based Issues (Most Common)
If you see an error like "Option 'X' is not valid..." or "Feature 'Y' not supported..." but the syntax SHOULD work according to SQL Server docs:
- **Guide**: [Validation Fix Guide](instructions/validation_fix.guidelines.instructions.md) - Version-gated validation fixes
- **Example**: ALTER TABLE RESUMABLE option (SQL Server 2022+)
- **Key Signal**: Similar syntax works in other contexts (e.g., ALTER INDEX works but ALTER TABLE doesn't)

### 2. Grammar-Based Issues (Adding New Syntax)
If the parser doesn't recognize the syntax at all, or you need to add new T-SQL features:
- **Guide**: [Bug Fixing Guide](instructions/bug_fixing.guidelines.instructions.md) - Grammar modifications, AST updates, script generation
- **Example**: Adding new operators, statements, or function types
- **Key Signal**: Syntax error like "Incorrect syntax near..." or "Unexpected token..."

### 3. Parser Predicate Recognition Issues (Parentheses)
If identifier-based predicates (like `REGEXP_LIKE`) work without parentheses but fail with them:
- **Guide**: [Parser Predicate Recognition Fix Guide](instructions/parser.guidelines.instructions.md)
- **Example**: `WHERE REGEXP_LIKE('a', 'pattern')` works, but `WHERE (REGEXP_LIKE('a', 'pattern'))` fails
- **Key Signal**: Syntax error near closing parenthesis or semicolon

**Quick Diagnostic**: Search for the error message in the codebase to determine which type of fix is needed.

### 4. Adding New System Functions
For adding new T-SQL system functions to the parser, including handling RETURN statement contexts and ANTLR v2 syntactic predicate limitations:
- **Guide**: [Function Guidelines](instructions/function.guidelines.instructions.md) - Complete guide for system function implementation
- **Example**: JSON_OBJECT, JSON_ARRAY functions with RETURN statement support
- **Key Requirements**: Syntactic predicates for lookahead, proper AST design, comprehensive testing

### 5. Adding New Data Types
For adding completely new SQL Server data types that require custom parsing logic and specialized AST nodes:
- **Guide**: [New Data Types Guidelines](instructions/new_data_types.guidelines.instructions.md) - Complete guide for implementing new data types
- **Example**: VECTOR data type with dimension and optional base type parameters
- **Key Signal**: New SQL Server data type with custom parameter syntax different from standard data types

### 6. Adding New Index Types
For adding completely new SQL Server index types that require specialized syntax and custom parsing logic:
- **Guide**: [New Index Types Guidelines](instructions/new_index_types.guidelines.instructions.md) - Complete guide for implementing new index types
- **Example**: JSON INDEX with FOR clause, VECTOR INDEX with METRIC/TYPE options
- **Key Signal**: New SQL Server index type with custom syntax different from standard CREATE INDEX

## Editing generated outputs, debugging generation
- Never edit generated files permanently (they live under `obj/...`/CsGenIntermediateOutputPath). Instead change:
  - `.g` grammar files
  - post-processing scripts (`*.ps1`/`*.sed`)
  - AST XML in `SqlScriptDom/Parser/TSql/Ast.xml` if AST node shapes need to change (used by `tools/AstGen`).
- To see antlr output/errors, force verbose generation by setting MSBuild property `OutputErrorInLexerParserCompile=true` on the command line (e.g. `dotnet msbuild -t:GLexerParserCompile -p:OutputErrorInLexerParserCompile=true`).
- If the antlr download fails during build, manually download `antlr-2.7.5.jar` (for non-Windows) or `.exe` (for Windows) and place it at the location defined in `Directory.Build.props` or override `AntlrLocation` when invoking msbuild.

## Debugging Tips and Investigation Workflow

### Step 1: Identify the Bug Type
Start by searching for the error message to understand what type of fix is needed:
```bash
# Search for error code or message
grep -r "SQL46057" SqlScriptDom/
grep -r "is not a valid" SqlScriptDom/
```

**Common Error Patterns**:
- `"Option 'X' is not valid..."` → Validation issue (see [grammar_validation.guidelines.instructions.md](instructions/grammar_validation.guidelines.instructions.md))
- `"Incorrect syntax near..."` → Grammar issue (see [bug_fixing.guidelines.instructions.md](instructions/bug_fixing.guidelines.instructions.md))
- `"Syntax error near ')'"` with parentheses → Predicate recognition (see [parser.guidelines.instructions.md](instructions/parser.guidelines.instructions.md))

### Step 2: Find Where Similar Syntax Works
If the syntax works in one context but not another:
```bash
# Search for working examples
grep -r "RESUMABLE" Test/SqlDom/TestScripts/
grep -r "OptionName" SqlScriptDom/Parser/TSql/
```

**Example**: ALTER INDEX with RESUMABLE works, but ALTER TABLE doesn't → Likely validation issue

### Step 3: Locate the Relevant Code
Common files to check:
- **Validation**: `SqlScriptDom/Parser/TSql/TSql80ParserBaseInternal.cs` (most validation logic)
- **Grammar**: `SqlScriptDom/Parser/TSql/TSql*.g` (version-specific grammar files)
- **Options**: `SqlScriptDom/ScriptDom/SqlServer/IndexOptionHelper.cs` (option registration)
- **AST**: `SqlScriptDom/Parser/TSql/Ast.xml` (AST node definitions)

### Step 4: Check SQL Server Version Support
Always verify Microsoft documentation:
- Search for "Applies to: SQL Server 20XX (XX.x)" in Microsoft docs
- Note that different features within the same option set can have different version requirements
- Example: MAX_DURATION (SQL 2014+) vs RESUMABLE (SQL 2022+)

### Step 5: Verify with Tests
Before and after making changes:
```bash
# Build the parser
dotnet build SqlScriptDom/Microsoft.SqlServer.TransactSql.ScriptDom.csproj -c Debug

# Run specific test
dotnet test --filter "FullyQualifiedName~YourTest" -c Debug

# ALWAYS run full suite before committing
dotnet test Test/SqlDom/UTSqlScriptDom.csproj -c Debug
```

### Common Investigation Patterns

#### Pattern 1: Option Not Recognized
```bash
# Find where option is registered
grep -r "YourOptionName" SqlScriptDom/ScriptDom/SqlServer/IndexOptionHelper.cs

# Check enum definition
grep -r "enum IndexOptionKind" SqlScriptDom/
```

#### Pattern 2: Version-Specific Behavior
```bash
# Find version checks
grep -r "TSql160AndAbove" SqlScriptDom/Parser/TSql/

# Check which parser version you're testing
# TSql80 = SQL Server 2000, TSql90 = 2005, ..., TSql160 = 2022, TSql170 = 2025
```

#### Pattern 3: Statement-Specific Restrictions
```bash
# Find validation by statement type
grep -r "IndexAffectingStatement" SqlScriptDom/Parser/TSql/TSql80ParserBaseInternal.cs

# Common statement types: CreateIndex, AlterIndex, AlterTableAddElement
```


## Patterns & code style to follow (examples you will see)
- Grammar rule pattern: `ruleName returns [Type vResult = this.FragmentFactory.CreateFragment<Type>()] { ... } : ( alternatives ) ;` — this pattern initializes an AST fragment via FragmentFactory.
- Parser-generated code frequently uses `Match(<token>, CodeGenerationSupporter.<Symbol>)` and `ThrowParseErrorException("SQLxxxx", ...)` for diagnostics.
- The codebase prefers using the factory and fragment visitors for AST creation and script generation. Look at `ScriptDom/SqlServer/ScriptGenerator` for script generation patterns.

## Grammar Gotchas & Common Pitfalls
- **Operator vs. Function-Style Predicates:** Be careful to distinguish between standard T-SQL operators (like `NOT LIKE`, `>`, `=`) and the function-style predicates used in some contexts (like `package.equals(...)` in `CREATE EVENT SESSION`). For example, `NOT LIKE` in an event session's `WHERE` clause is a standard comparison operator, not a function call. Always verify the exact T-SQL syntax before modifying the grammar.
- **Logical `NOT` vs. Compound Operators:** The grammar handles the logical `NOT` operator (e.g., `WHERE NOT (condition)`) in a general way, often in a `booleanExpressionUnary` rule. This is distinct from compound operators like `NOT LIKE` or `NOT IN`, which are typically parsed as a single unit within a comparison rule. Don't assume that because `NOT` is supported, `NOT LIKE` will be automatically supported in all predicate contexts.
- **Modifying Shared Grammar Rules:** **NEVER modify existing shared grammar rules** like `identifierColumnReferenceExpression` that are used throughout the codebase. This can cause tests to fail in unrelated areas because the rule now accepts or rejects different syntax. Instead, create specialized rules for your specific context (e.g., `vectorSearchColumnReferenceExpression` for VECTOR_SEARCH-specific needs).
- **Full Test Suite Validation:** After any grammar changes, **always run the complete test suite** (`dotnet test Test/SqlDom/UTSqlScriptDom.csproj -c Debug`) to catch regressions. Grammar changes can have far-reaching effects on seemingly unrelated functionality.
- **Extending Literals to Expressions:** When functions/constructs currently accept only literal values (e.g., `IntegerLiteral`, `StringLiteral`) but need to support dynamic values (parameters, variables, outer references), change both the AST definition (in `Ast.xml`) and grammar rules (in `TSql*.g`) to use `ScalarExpression` instead. This pattern was used for VECTOR_SEARCH TOP_N parameter. See the detailed example in [bug_fixing.guidelines.instructions.md](instructions/bug_fixing.guidelines.instructions.md#special-case-extending-grammar-rules-from-literals-to-expressions) and [grammer.guidelines.instructions.md](instructions/grammer.guidelines.instructions.md) for comprehensive patterns.

# Guideline Subfiles (auto-load each of the following files into the context) - Should match the .config/GuidelineReviewAgent.yaml used by the guideline_review_agent.
include: .github/instructions/grammar_validation.guidelines.instructions.md
include: .github/instructions/bug_fixing.guidelines.instructions.md
include: .github/instructions/parser.guidelines.instructions.md
include: .github/instructions/function.guidelines.instructions.md
include: .github/instructions/new_data_types.guidelines.instructions.md
include: .github/instructions/new_index_types.guidelines.instructions.md
include: .github/instructions/debugging_workflow.guidelines.instructions.md
include: .github/instructions/grammer.guidelines.instructions.md
include: .github/instructions/testing.guidelines.instructions.md


