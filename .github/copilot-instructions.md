ScriptDom is a library for parsing and generating T-SQL scripts. It is primarily used by DacFx to build database projects, perform schema comparisons, and generate scripts for deployment.

T-SQL syntax definitions are defined in the .g files in SqlScriptDom/Parser/TSql/. The file names map to SQL Server versions, e.g. TSql170.g corresponds to the syntax definitions for SQL Server 2025, TSql160.g to SQL Server 2022, etc. Syntax for Azure SQL Database should always be based on the latest SQL Server version.

The grammar files are in ANTLR v2 format. C# code is generated from these grammar files as part of the build process.

For each new syntax definition, ScriptDom needs to be able to parse it successfully, and roundtrip back to the original script via the script generator.

Changes need to have accompanying tests in Only170SyntaxTests.cs or the one for its respective version. The test framework should already verify the parser and script generator; you just need to add the test scripts to TestScripts and corresponding Baselines folder. Older syntaxes should be supported unless explicitly stated otherwise.