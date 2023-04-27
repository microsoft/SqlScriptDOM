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

1. Download .NET SDKS from https://dotnet.microsoft.com/download/visual-studio-sdks 
    - .NET Framework SDK (4.6.2 or higher)
    - .NET 6 SDK (see [global.json](./global.json) for latest version)
2. Install [Visual Studio 2022 or newer](https://visualstudio.microsoft.com/vs/community/). (Optional)

3.  Install the [SlnGen](https://microsoft.github.io/slngen/) tool. (Optional Visual Studio generator)
    ```cmd 
    dotnet tool install --global Microsoft.VisualStudio.SlnGen.Tool
    ```
4. Download DisableStrongName script from Sql Management Objects repository: https://github.com/microsoft/sqlmanagementobjects/blob/main/SmoBuild/DisableStrongName.ps1 and run as administrator. This step only needs to be done once.

5. Clone the repository
    ```
        git clone https://github.com/microsoft/SqlScriptDOM
    ```
    
### Building

Navigate to the root of the source code:
```cmd
cd C:\SqlScriptDOM\
```
Generate and open Visual Studio solution. This is an optional step for developers using Visual Studio
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

### Pull Request Process

Before sending a Pull Request, please do the following:

1. Ensure builds are still successful and tests, including any added or updated tests, pass prior to submitting the pull request.
2. Update any documentation, user and contributor, that is impacted by your changes.
3. Include your change description in `CHANGELOG.md` file as part of pull request.
4. You may merge the pull request in once you have the sign-off of two other developers, or if you do not have permission to do that, you may request the second reviewer to merge it for you.

### Helpful notes for SQLDOM extensions

1. For changing the DOM classes, modify the XML file (the C# code is generated based on this during the build process) `$(EnlistmentRoot)\Source\SqlDom\SqlScriptDom\Parser\TSql\Ast.xml`. Change Ast.xml to put the class pieces on their appropriate statements.
    1. The build process is defined in `$(EnlistmentRoot)\Source\SqlDom\SqlScriptDom\SqlScriptDom.props` (Target Name="CreateAST")
    2. The generated files are dropped in `$(EnlistmentRoot)\obj\<x64|x86>\<Debug|Release>\SqlScriptDom.csproj\`
   
2. For changing the parser, modify the .g file here:
`$(EnlistmentRoot)\Source\SqlDom\SqlScriptDom\Parser\TSql\TSql<#>.g` where # is the version (ie - 100, 120, 130). This will usually be the latest number if adding new grammar. Change the Tsql(xxx).g file to parse the new syntax.
    1. The build process is defined in `$(EnlistmentRoot)\Source\SqlDom\SqlScriptDom\SqlScriptDom.props` (Target Name="CreateAST")
    2. The generated files are dropped in `$(EnlistmentRoot)\obj\x86|x64\Debug|Release\sqlscriptdom.csproj\`

3. For changing the ScriptGenerator, modify the appropriate file (i.e. Visitor that accepts the modified DOM class) in here: `$(EnlistmentRoot)\Source\SqlDom\SqlScriptDom\ScriptDom\SqlServer\ScriptGenerator`.
    1. To add a new ScriptGenerator, you need to add the file to `$(EnlistmentRoot)\Source\SqlDom\SqlScriptDom\SqlScriptDom.props`
    1. Change The visitors SqlScriptGenerator.X to use the new piece from AST.XML
    1. If you're adding syntax that's Azure-only or Standalone-only, implement appropriate Versioning Visitor for your constructs.

4. When adding/removing new files please add/remove an entry to/from `$(EnlistmentRoot)\Source\SqlDom\SqlScriptDom\SqlScriptDom.csproj` 

5. To extend the tests do the following:
    1. Baselines# needs to be updated or added with the appropriate .sql file as expected results.
    1. The Only#SyntaxTests.cs needs to be extended or added to specify the appropriate TestScripts script.
    1. Positive tests go in Only#SyntaxTests.cs if adding new grammar.
    1. Create a \TestScripts script that is the input for the test. (This is what is run and is checked against the .sql files in Baselines#)
    1. Negative tests go in ParserErrorsTests.cs.