# Contributing

Welcome and thank you for your interest in contributing to **Sql Script DOM**! Before contributing to this
project, please review this document for policies and procedures which
will ease the contribution and review process for everyone.

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).
For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or
contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

## Issues and Feature Requests

Before filing an issue, please make sure that the issue has not been submitted before in our public repository [Sql Script DOM](https://github.com/microsoft/SqlScriptDOM/issues). 

To report a new issue please add the **bug** tag and use the **enhancement** tag for new feature proposals.

### Bugs

Please create an issue per bug and include the version of Sql Script DOM where you are experiencing the issue, the steps to repro the bug, include error messages if applicable, and add a description of the expected result versus the actual result.

### Feature Requests

Please create an issue per feature request and describe the current and desired behavior. If you have a proposed solution, please include it in the issue as well as whether you'd like to contribute the change.

## Contributing Code

### Contributor License Agreement

This project welcomes contributions and suggestions.  Most contributions require you to agree to a
Contributor License Agreement (CLA) declaring that you have the right to, and actually do, grant us
the rights to use your contribution. For details, visit https://cla.opensource.microsoft.com.

When you submit a pull request, a CLA bot will automatically determine whether you need to provide
a CLA and decorate the PR appropriately (e.g. status check, comment). Simply follow the instructions
provided by the bot. You will only need to do this once across all repos using our CLA.


### Getting Started

#### Windows

1. Download .NET SDKS from https://dotnet.microsoft.com/download/visual-studio-sdks 
    - .NET Framework SDK (4.6.2 or higher)
    - .NET 6 SDK (see [global.json](./global.json) for latest version)
2. Install [Visual Studio 2022 or newer](https://visualstudio.microsoft.com/vs/community/). (Optional)

3.  Install the [SlnGen](https://microsoft.github.io/slngen/) tool. (Optional Visual Studio generator)
    ```cmd 
    dotnet tool install --global Microsoft.VisualStudio.SlnGen.Tool
    ```
4. Run DisableStrongName script as administrator: [disableStrongName.ps1](./disableStrongName.ps1) This step only needs to be done once.

5. Clone the repository
    ```
        git clone https://github.com/microsoft/SqlScriptDOM
    ```
    
#### macOS/Linux

1. Download .NET SDKS from https://dotnet.microsoft.com/download/visual-studio-sdks
    - .NET 6 SDK (see [global.json](./global.json) for latest version)

2. Install the Java runtime (JRE)
    - macOS: `brew install openjdk`
    - Linux: see https://openjdk.org/install/

3. Install PowerShell Core
    - macOS: `brew install powershell/tap/powershell`
    - Linux: see https://learn.microsoft.com/powershell/scripting/install/installing-powershell-on-linux

4. Clone the repository
    ```
        git clone https://github.com/microsoft/SqlScriptDOM
    ```

### Building

Navigate to the root of the source code:
```cmd
cd SqlScriptDOM
```

Optional, Windows-only - Generate and open Visual Studio solution. This step is not required to build the project.
```cmd
slngen
```

Restore project dependencies:
```cmd
dotnet restore
```

To build:
```cmd
dotnet build
```



### Running the tests

You can run tests directly in Visual Studio Text Explorer or by using the ```dotnet test``` command.

Example: To run all priority 0 tests
```cmd
dotnet test --filter Priority=0
```

#### ⚠️ CRITICAL: Full Test Suite for Parser Changes

**If you make ANY changes to grammar files (`.g` files) or AST definitions (`Ast.xml`), you MUST run the complete test suite** to ensure no regressions:

```cmd
dotnet test Test/SqlDom/UTSqlScriptDom.csproj -c Debug
```

**Why this is critical for parser changes:**
- Grammar changes can have far-reaching effects on seemingly unrelated functionality
- Shared grammar rules are used in multiple contexts throughout the parser
- AST modifications can affect script generation and visitor patterns across the entire codebase
- Token recognition changes can impact parsing of statements that don't even use the modified feature

**Example of unexpected failures:**
- Modifying a shared rule like `identifierColumnReferenceExpression` can cause other tests to fail because the rule now accepts syntax that should be rejected in different contexts
- Changes to operator precedence can affect unrelated expressions
- Adding new AST members without proper script generation support can break round-trip parsing

Always verify that all ~557 tests pass before submitting your changes.

### Pull Request Process

Before sending a Pull Request, please do the following:

1. **For parser changes (grammar/AST modifications): Run the complete test suite** (`dotnet test Test/SqlDom/UTSqlScriptDom.csproj -c Debug`) and ensure all ~557 tests pass. Grammar changes can have unexpected side effects.
2. Ensure builds are still successful and tests, including any added or updated tests, pass prior to submitting the pull request.
3. Update any documentation, user and contributor, that is impacted by your changes.
4. Include your change description in `CHANGELOG.md` file as part of pull request.
5. You may merge the pull request in once you have the sign-off of two other developers, or if you do not have permission to do that, you may request the second reviewer to merge it for you.

### Helpful notes for SQLDOM extensions

1. For changing the DOM classes, modify the XML file (the C# code is generated based on this during the build process) `SqlScriptDom\Parser\TSql\Ast.xml`. Change Ast.xml to put the class pieces on their appropriate statements.
    1. The build process is defined in `SqlScriptDom\GenerateFiles.props` (Target Name="CreateAST")
    2. The generated files are dropped in `obj\SqlScriptDom\AnyCPU\<Debug|Release>\<TargetPlatform>\Microsoft.SqlServer.TransactSql.ScriptDom.csproj\`
    
    Regenerating generated sources (what to run and when)
    ---------------------------------------------------
    When you modify `Source\SqlDom\SqlScriptDom\Parser\TSql\Ast.xml` or any `TSql<#>.g` grammar file, the C# parser and DOM sources are produced by MSBuild generation targets (for example `CreateAST`). These targets are invoked automatically during a normal build, so in most cases you can simply run:

    ```powershell
    dotnet build Source\SqlDom\SqlScriptDom\Microsoft.SqlServer.TransactSql.ScriptDom.csproj -c Debug
    ```

    If you only want to run generation targets (no compile) or need more detailed generation logs, invoke the MSBuild targets directly:

    ```powershell
    dotnet msbuild Source\SqlDom\SqlScriptDom\Microsoft.SqlServer.TransactSql.ScriptDom.csproj -t:GLexerParserCompile;GSqlTokenTypesCompile;CreateAST -p:Configuration=Debug
    ```

    Generated files are written into the `obj` folder for that project (for example `obj\SqlScriptDom\AnyCPU\<Debug|Release>\<TargetPlatform>\Microsoft.SqlServer.TransactSql.ScriptDom.csproj\`). If antlr or related tools are missing, see `Directory.Build.props` for `AntlrLocation` and follow the repo guidance to supply the binaries.
   
2. For changing the parser, modify the .g file here:
`SqlScriptDom\Parser\TSql\TSql<#>.g` where # is the version (ie - 100, 120, 130). This will usually be the latest number if adding new grammar. Change the Tsql(xxx).g file to parse the new syntax.
    1. The build process is defined in `SqlScriptDom\GenerateFiles.props` (Target Name="CreateAST")
    2. The generated files are dropped in `obj\SqlScriptDom\AnyCPU\<Debug|Release>\<TargetPlatform>\Microsoft.SqlServer.TransactSql.ScriptDom.csproj\`

3. For changing the ScriptGenerator, modify the appropriate file (i.e. Visitor that accepts the modified DOM class) in here: `SqlScriptDom\ScriptDom\SqlServer\ScriptGenerator`.
    1. Change The visitors SqlScriptGenerator.X to use the new piece from AST.XML
    1. If you're adding syntax that's Azure-only or Standalone-only, implement appropriate Versioning Visitor for your constructs.


4. To extend the tests do the following:
    1. Baselines# needs to be updated or added with the appropriate .sql file as expected results.
    1. The Only#SyntaxTests.cs needs to be extended or added to specify the appropriate TestScripts script.
    1. Positive tests go in Only#SyntaxTests.cs if adding new grammar.
    1. Create a \TestScripts script that is the input for the test. (This is what is run and is checked against the .sql files in Baselines#)
    1. Negative tests go in ParserErrorsTests.cs.