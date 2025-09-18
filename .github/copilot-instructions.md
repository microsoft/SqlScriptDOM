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

## Developer workflow & conventions (typical change cycle)
1. Add/modify grammar rule(s) in the correct `TSql*.g` (pick the _version_ the syntax belongs to).
2. If tokens or token ordering change, update `TSqlTokenTypes.g` (and the sed/ps1 post-processors if necessary).
3. Rebuild the ScriptDom project to regenerate parser and AST (`dotnet build` will run generation). Use the targeted msbuild targets if you only want generation.
4. Add tests:
   - Put the input SQL in `Test/SqlDom/TestScripts/` (filename is case sensitive and used as an embedded resource).
   - Add/confirm baseline output in `Test/SqlDom/Baselines<version>/` (the UT project embeds these baselines as resources).
   - Update the appropriate `Only<version>SyntaxTests.cs` (e.g., `Only170SyntaxTests.cs`) by adding a `ParserTest170("MyNewTest.sql", ...)` entry. See `ParserTest.cs` and `ParserTestOutput.cs` for helper constructors and verification semantics.
5. Run `dotnet test Test/SqlDom/UTSqlScriptDom.csproj -c Debug` and iterate until tests pass.

## Testing details and how tests assert correctness
- Tests run a full parse -> script generator -> reparse round-trip. Baseline comparison verifies pretty-printed generated scripts exactly match the stored baseline.
- Expected parse errors (where applicable) are verified by number and exact error messages; test helpers live in `ParserTest.cs`, `ParserTestOutput.cs`, and `ParserTestUtils.cs`.
- If a test fails due to mismatch in generated script, compare the generated output (the test harness logs it) against the baseline to spot formatting/structure differences.

## Bug Fixing and Baseline Generation
For a practical guide on fixing bugs, including the detailed workflow for generating test baselines, see the [Bug Fixing Guide](BUG_FIXING_GUIDE.md).

For specific parser predicate recognition issues (when identifier-based predicates like `REGEXP_LIKE` don't work with parentheses), see the [Parser Predicate Recognition Fix Guide](PARSER_PREDICATE_RECOGNITION_FIX.md).

## Editing generated outputs, debugging generation
- Never edit generated files permanently (they live under `obj/...`/CsGenIntermediateOutputPath). Instead change:
  - `.g` grammar files
  - post-processing scripts (`*.ps1`/`*.sed`)
  - AST XML in `SqlScriptDom/Parser/TSql/Ast.xml` if AST node shapes need to change (used by `tools/AstGen`).
- To see antlr output/errors, force verbose generation by setting MSBuild property `OutputErrorInLexerParserCompile=true` on the command line (e.g. `dotnet msbuild -t:GLexerParserCompile -p:OutputErrorInLexerParserCompile=true`).
- If the antlr download fails during build, manually download `antlr-2.7.5.jar` (for non-Windows) or `.exe` (for Windows) and place it at the location defined in `Directory.Build.props` or override `AntlrLocation` when invoking msbuild.


## Patterns & code style to follow (examples you will see)
- Grammar rule pattern: `ruleName returns [Type vResult = this.FragmentFactory.CreateFragment<Type>()] { ... } : ( alternatives ) ;` — this pattern initializes an AST fragment via FragmentFactory.
- Parser-generated code frequently uses `Match(<token>, CodeGenerationSupporter.<Symbol>)` and `ThrowParseErrorException("SQLxxxx", ...)` for diagnostics.
- The codebase prefers using the factory and fragment visitors for AST creation and script generation. Look at `ScriptDom/SqlServer/ScriptGenerator` for script generation patterns.

## Grammar Gotchas & Common Pitfalls
- **Operator vs. Function-Style Predicates:** Be careful to distinguish between standard T-SQL operators (like `NOT LIKE`, `>`, `=`) and the function-style predicates used in some contexts (like `package.equals(...)` in `CREATE EVENT SESSION`). For example, `NOT LIKE` in an event session's `WHERE` clause is a standard comparison operator, not a function call. Always verify the exact T-SQL syntax before modifying the grammar.
- **Logical `NOT` vs. Compound Operators:** The grammar handles the logical `NOT` operator (e.g., `WHERE NOT (condition)`) in a general way, often in a `booleanExpressionUnary` rule. This is distinct from compound operators like `NOT LIKE` or `NOT IN`, which are typically parsed as a single unit within a comparison rule. Don't assume that because `NOT` is supported, `NOT LIKE` will be automatically supported in all predicate contexts.

