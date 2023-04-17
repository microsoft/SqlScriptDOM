---
ArtifactType: nupkg
Documentation: URL
Language: csharp, powershell
Platform: dotnet
Stackoverflow: URL
Tags: ScriptDOM, SqlDOM
---

# SQL ScriptDOM

SQL Script DOM is a .NET library that provides formatting and parsing capabilities to analyze SQL Scripts. It can be used by Powershell and C#.

Script DOM is used by DacFX and as an standalone library for client applications. 

## Getting Started

1. Install Visual Studio 2022 or newer.

2. Download .NET SDKS from https://dotnet.microsoft.com/download/visual-studio-sdks 
    - .NET Framework SDK (4.6.2 or higher)
    - .NET 6 SDK (see [global.json](./global.json) for latest version)
3.  Install the [SlnGen](https://microsoft.github.io/slngen/) tool.
    ```cmd 
    dotnet tool install --global Microsoft.VisualStudio.SlnGen.Tool
    ```
4. Install antlr-2.7.5.exe from https://www.antlr2.org/download.html

5. Create environment variable named Antlr2Exe pointing to the installation path.

6. Clone the repository
    ```
        git clone https://github.com/microsoft/SqlScriptDOM
    ```
    
### Building

Navigate to the root of the source code:
```cmd
cd C:\SqlScriptDOM\
```
Open Visual Studio solution
```cmd
slngen
```

Restore project dependencies:
```cmd
msbuild /t:Restore
```

To build:
```cmd
msbuild
```
## Contributing
Notes for SQLDOM extensions:

1. Make the changes in
     For changing the DOM classes, modify the XML file (the C# code is generated based on this during the build process)
     $(EnlistmentRoot)\Source\SqlDom\SqlScriptDom\Parser\TSql\Ast.xml
	 Change Ast.xml to put the class pieces on their appropriate statements
	 
	 Note for above:
		 1. The build process is defined in ```$(EnlistmentRoot)\Source\SqlDom\SqlScriptDom\SqlScriptDom.props``` (Target Name="CreateAST")
		 2. The generated files are dropped in ```$(EnlistmentRoot)\obj\<x64|x86>\<Debug|Release>\SqlScriptDom.csproj\```
   
     For changing the parser, modify the .g file here:
```$(EnlistmentRoot)\Source\SqlDom\SqlScriptDom\Parser\TSql\TSql<#>.g``` where # is the version (ie - 100, 120, 130). This will usually be the latest number if adding new grammar.
	 Change the Tsql(xxx).g file to parse the new syntax.
	 
	 Note for above:
		 1. The build process is defined in ```$(EnlistmentRoot)\Source\SqlDom\SqlScriptDom\SqlScriptDom.props``` (Target Name="CreateAST")
		 2. The generated files are dropped in ```$(EnlistmentRoot)\obj\x86|x64\Debug|Release\sqlscriptdom.csproj\```
   
     For changing the ScriptGenerator, modify the appropriate file 
     (i.e. Visitor that accepts the modified DOM class) in here
     ```$(EnlistmentRoot)\Source\SqlDom\SqlScriptDom\ScriptDom\SqlServer\ScriptGenerator```
	 To add a new ScriptGenerator, you need to add the file to ```$(EnlistmentRoot)\Source\SqlDom\SqlScriptDom\SqlScriptDom.props```
	 Change The visitors SqlScriptGenerator.X to use the new piece from AST.XML
	 If you're adding syntax that's Azure-only or Standalone-only, implement appropriate Versioning Visitor for your constructs.

2. When adding/removing new files please add/remove an entry to/from ```$(EnlistmentRoot)\Source\SqlDom\SqlScriptDom\SqlScriptDom.csproj``` 

3. To extend the tests do the following:
     a. Baselines# needs to be updated or added with the appropriate .sql file as expected results.
     b. The Only#SyntaxTests.cs needs to be extended or added to specify the appropriate TestScripts script.
	 c. Positive tests go in Only#SyntaxTests.cs if adding new grammar.
     d. Create a \TestScripts script that is the input for the test. (This is what is run and is checked against the .sql files in Baselines#)
	 e. Negative tests go in ParserErrorsTests.cs.

## Running the tests

You can run tests directly in Visual Studio Text Explorer or by using the ```dotnet test``` command.

Example: To run all priority 0 tests
```cmd
dotnet test --filter Priority=0
```

## Built With
* https://www.antlr.org/
* https://www.gnu.org/software/sed/manual/sed.html

## Contributing

Please read our [CONTRIBUTING.md](CONTRIBUTING.md) which outlines all of our policies, procedures, and requirements for contributing to this project.

## License

Copyright (c) Microsoft Corporation. All rights reserved.

Licensed under the [Source MIT](LICENSE).
