# Guidelines for Adding New Parser Versions to SqlScriptDOM

This guide provides step-by-step instructions for adding support for a new SQL Server version parser to the SqlScriptDOM library. This pattern was established from the TSql180 parser implementation for SQL Server Yellowstone (vnext).

## When to Use This Guide

Use this pattern when:
- ✅ Adding support for a **new SQL Server version** (e.g., SQL Server 2025, vnext releases)
- ✅ Creating a **new parser for a specific compatibility level** (e.g., 180, 190, etc.)
- ✅ Adding a **new SQL engine variant** (e.g., Azure SQL, Fabric DW)

**Prerequisites:**
- Understand ANTLR v2 grammar syntax
- Familiarity with the ScriptDom parser architecture
- Knowledge of the new SQL Server version's feature set

## Version Numbering Convention

SqlScriptDOM uses a version numbering scheme that corresponds to SQL Server versions:
- TSql80 = SQL Server 2000
- TSql90 = SQL Server 2005
- TSql100 = SQL Server 2008
- TSql110 = SQL Server 2012
- TSql120 = SQL Server 2014
- TSql130 = SQL Server 2016
- TSql140 = SQL Server 2017
- TSql150 = SQL Server 2019
- TSql160 = SQL Server 2022
- TSql170 = SQL Server 2025
- TSql180 = SQL Server Yellowstone (vnext)

**Naming Pattern**: `TSql{CompatibilityLevel}` where CompatibilityLevel is typically `(MajorVersion - 1900) * 10`

## Step-by-Step Implementation Guide

### Step 1: Add SqlVersion Enum Value

Update the `SqlVersion` enumeration to include the new version.

**File**: `SqlScriptDom/ScriptDom/SqlServer/SqlVersion.cs`

```csharp
namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    public enum SqlVersion
    {
        // ... existing versions ...
        
        /// <summary>
        /// Sql 18.0 mode (SQL Server Yellowstone/vnext).
        /// </summary>
        Sql180 = 11,
    }
}
```

**Key Points**:
- Use the next sequential enum value
- Add XML documentation comment describing the SQL Server version
- Maintain consistent naming: `Sql{CompatibilityLevel}`

### Step 2: Create Grammar File

Create a new ANTLR v2 grammar file that inherits from the previous version.

**File**: `SqlScriptDom/Parser/TSql/TSql180.g`

```antlr
//------------------------------------------------------------------------------
// <copyright file="TSql180.g" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

options {
    language = "CSharp";
    namespace = "Microsoft.SqlServer.TransactSql.ScriptDom";
}

{
    using System.Diagnostics;
    using System.Globalization;
    using System.Collections.Generic;
}

class TSql180ParserInternal extends Parser("TSql180ParserBaseInternal");
options {
    k = 2;
    defaultErrorHandler=false;
    classHeaderPrefix = "internal partial";
    importVocab = TSql;
}

{
    public TSql180ParserInternal(bool initialQuotedIdentifiersOn)
        : base(initialQuotedIdentifiersOn)
    {
        initialize();
    }
}

// Entry point rules (required for parser functionality)
entryPointChildObjectName returns [ChildObjectName vResult = null]
    :
        vResult=childObjectNameWithThreePrefixes
        EOF
    ;

entryPointSchemaObjectName returns [SchemaObjectName vResult = null]
    :
        vResult=schemaObjectThreePartName
        EOF
    ;

entryPointScalarDataType returns [DataTypeReference vResult = null]
    :
        vResult=scalarDataType
        EOF
    ;

entryPointExpression returns [ScalarExpression vResult = null]
    :
        vResult=expression
        EOF
    ;

entryPointBooleanExpression returns [BooleanExpression vResult = null]
    :
        vResult=booleanExpression
        EOF
    ;

entryPointStatementList returns [StatementList vResult = null]
    :
        vResult=statementList
        EOF
    ;

// Main script entry point
script returns [TSqlScript vResult = this.FragmentFactory.CreateFragment<TSqlScript>()]
    :
        vResult.Batches = batches
        EOF
    ;

// Add new version-specific grammar rules below
// For initial version bump, typically inherit all rules from previous version
```

**Key Points**:
- Grammar file initially inherits all rules from the previous version via the base class
- Add version-specific rules only when new syntax is needed
- Entry point rules are required for various parsing scenarios
- The `importVocab = TSql` directive imports token definitions

### Step 3: Add Grammar to Build Configuration

Register the grammar file in the build system.

**File**: `SqlScriptDom/GenerateFiles.props`

```xml
<ItemGroup>
  <GLexerParserCompile Include="$(SQLSCRIPTDOM)\Parser\TSql\TSql160.g" />
  <GLexerParserCompile Include="$(SQLSCRIPTDOM)\Parser\TSql\TSqlFabricDW.g" />
  <GLexerParserCompile Include="$(SQLSCRIPTDOM)\Parser\TSql\TSql170.g" />
  <GLexerParserCompile Include="$(SQLSCRIPTDOM)\Parser\TSql\TSql180.g" />
</ItemGroup>
```

**Key Points**:
- ANTLR will generate parser code during build from this grammar file
- Generated files go to `$(CsGenIntermediateOutputPath)` (under `obj/`)
- Never manually edit generated parser files

### Step 4: Create Parser Base Internal Class

Create the internal parser base class that inherits from the previous version.

**File**: `SqlScriptDom/Parser/TSql/TSql180ParserBaseInternal.cs`

```csharp
//------------------------------------------------------------------------------
// <copyright file="TSql180ParserBaseInternals.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using antlr;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    internal abstract class TSql180ParserBaseInternal : TSql170ParserBaseInternal
    {
        #region Constructors

        // Required by ANTLR-generated code
        protected TSql180ParserBaseInternal(TokenBuffer tokenBuf, int k)
            : base(tokenBuf, k)
        {
        }

        protected TSql180ParserBaseInternal(ParserSharedInputState state, int k)
            : base(state, k)
        {
        }

        protected TSql180ParserBaseInternal(TokenStream lexer, int k)
            : base(lexer, k)
        {
        }

        /// <summary>
        /// Primary constructor
        /// </summary>
        /// <param name="initialQuotedIdentifiersOn">if set to <c>true</c> initial quoted identifiers will be set to on.</param>
        public TSql180ParserBaseInternal(bool initialQuotedIdentifiersOn)
            : base(initialQuotedIdentifiersOn)
        {
        }

        #endregion

        /// <summary>
        /// Gets the SQL version for this parser
        /// </summary>
        protected override SqlVersion SqlVersionCurrent
        {
            get { return SqlVersion.Sql180; }
        }

        // Add version-specific helper methods and overrides here as needed
    }
}
```

**Key Points**:
- Inherits from previous version's base internal class (e.g., `TSql170ParserBaseInternal`)
- Overrides `SqlVersionCurrent` property to return the new version
- Contains version-specific validation and helper methods
- The multiple constructors are required by ANTLR's generated code

### Step 5: Create Public Parser Class

Create the public parser class that users will instantiate.

**File**: `SqlScriptDom/Parser/TSql/TSql180Parser.cs`

```csharp
//------------------------------------------------------------------------------
// <copyright file="TSql180Parser.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Microsoft.SqlServer.TransactSql.ScriptDom.Versioning;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// The TSql Parser for 18.0 (SQL Server Yellowstone/vnext).
    /// </summary>
    [Serializable]
    public class TSql180Parser : TSqlParser
    {
        /// <summary>
        /// Parser flavor (standalone/azure/all)
        /// </summary>
        protected SqlEngineType engineType = SqlEngineType.All;

        /// <summary>
        /// Initializes a new instance of the <see cref="TSql180Parser"/> class.
        /// </summary>
        /// <param name="initialQuotedIdentifiers">if set to <c>true</c> [initial quoted identifiers].</param>
        public TSql180Parser(bool initialQuotedIdentifiers)
            : base(initialQuotedIdentifiers)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TSql180Parser"/> class.
        /// </summary>
        /// <param name="initialQuotedIdentifiers">if set to <c>true</c> [initial quoted identifiers].</param>
        /// <param name="engineType">Parser engine type</param>
        public TSql180Parser(bool initialQuotedIdentifiers, SqlEngineType engineType)
            : base(initialQuotedIdentifiers)
        {
            this.engineType = engineType;
        }

        internal override TSqlLexerBaseInternal GetNewInternalLexer()
        {
            return new TSql180LexerInternal();
        }

        #region Helper Methods
        TSql180ParserInternal GetNewInternalParser()
        {
            return new TSql180ParserInternal(QuotedIdentifier);
        }

        TSql180ParserInternal GetNewInternalParserForInput(TextReader input, out IList<ParseError> errors, 
            int startOffset, int startLine, int startColumn)
        {
            TSql180ParserInternal parser = GetNewInternalParser();
            InitializeInternalParserInput(parser, input, out errors, startOffset, startLine, startColumn);
            return parser;
        }
        #endregion

        /// <summary>
        /// The main parse method.
        /// </summary>
        /// <param name="tokens">The list of tokens to parse.</param>
        /// <param name="errors">The parse errors.</param>
        /// <returns>The parsed TSqlFragment.</returns>        
        public override TSqlFragment Parse(IList<TSqlParserToken> tokens, out IList<ParseError> errors)
        {
            errors = new List<ParseError>();
            TSql180ParserInternal parser = GetNewInternalParser();
            parser.InitializeForNewInput(tokens, errors, false);

            TSqlFragment result = parser.ParseRuleWithStandardExceptionHandling<TSqlScript>(parser.script, ScriptEntryMethod);

            // Run versioning visitor for engine-specific validation
            if (result != null)
            {
                VersioningVisitor versioningVisitor = new VersioningVisitor(engineType, SqlVersion.Sql180);
                result.Accept(versioningVisitor);

                foreach (ParseError p in versioningVisitor.GetErrors())
                {
                    errors.Add(p);
                }
            }

            return result;
        }

        /// <summary>
        /// Parses an input string to get a ChildObjectName.
        /// </summary>
        public override ChildObjectName ParseChildObjectName(TextReader input, out IList<ParseError> errors, 
            int startOffset, int startLine, int startColumn)
        {
            TSql180ParserInternal parser = GetNewInternalParserForInput(input, out errors, startOffset, startLine, startColumn);
            return parser.ParseRuleWithStandardExceptionHandling<ChildObjectName>(parser.entryPointChildObjectName, "entryPointChildObjectName");
        }

        /// <summary>
        /// Parses an input string to get a SchemaObjectName.
        /// </summary>
        public override SchemaObjectName ParseSchemaObjectName(TextReader input, out IList<ParseError> errors, 
            int startOffset, int startLine, int startColumn)
        {
            TSql180ParserInternal parser = GetNewInternalParserForInput(input, out errors, startOffset, startLine, startColumn);
            return parser.ParseRuleWithStandardExceptionHandling<SchemaObjectName>(parser.entryPointSchemaObjectName, "entryPointSchemaObjectName");
        }

        /// <summary>
        /// Parses an input string to get a data type.
        /// </summary>
        public override DataTypeReference ParseScalarDataType(TextReader input, out IList<ParseError> errors, 
            int startOffset, int startLine, int startColumn)
        {
            TSql180ParserInternal parser = GetNewInternalParserForInput(input, out errors, startOffset, startLine, startColumn);
            return parser.ParseRuleWithStandardExceptionHandling<DataTypeReference>(parser.entryPointScalarDataType, "entryPointScalarDataType");
        }

        /// <summary>
        /// Parses an input string to get an expression.
        /// </summary>
        public override ScalarExpression ParseExpression(TextReader input, out IList<ParseError> errors, 
            int startOffset, int startLine, int startColumn)
        {
            TSql180ParserInternal parser = GetNewInternalParserForInput(input, out errors, startOffset, startLine, startColumn);
            return parser.ParseRuleWithStandardExceptionHandling<ScalarExpression>(parser.entryPointExpression, "entryPointExpression");
        }

        /// <summary>
        /// Parses an input string to get a boolean expression.
        /// </summary>
        public override BooleanExpression ParseBooleanExpression(TextReader input, out IList<ParseError> errors, 
            int startOffset, int startLine, int startColumn)
        {
            TSql180ParserInternal parser = GetNewInternalParserForInput(input, out errors, startOffset, startLine, startColumn);
            return parser.ParseRuleWithStandardExceptionHandling<BooleanExpression>(parser.entryPointBooleanExpression, "entryPointBooleanExpression");
        }

        /// <summary>
        /// Parses an input string to get a statement list.
        /// </summary>
        public override StatementList ParseStatementList(TextReader input, out IList<ParseError> errors, 
            int startOffset, int startLine, int startColumn)
        {
            TSql180ParserInternal parser = GetNewInternalParserForInput(input, out errors, startOffset, startLine, startColumn);
            return parser.ParseRuleWithStandardExceptionHandling<StatementList>(parser.entryPointStatementList, "entryPointStatementList");
        }

        /// <summary>
        /// Parses an input string to get a subquery expression with optional common table expressions.
        /// </summary>
        public override SelectStatement ParseSubQueryExpressionWithOptionalCTE(TextReader input, out IList<ParseError> errors, 
            int startOffset, int startLine, int startColumn)
        {
            TSql180ParserInternal parser = GetNewInternalParserForInput(input, out errors, startOffset, startLine, startColumn);
            return parser.ParseRuleWithStandardExceptionHandling<SelectStatement>(parser.subQueryExpressionWithOptionalCTE, "subQueryExpressionWithOptionalCTE");
        }
    }
}
```

**Key Points**:
- Public API that developers use to parse T-SQL
- Supports engine type filtering (Standalone, Azure, All)
- Implements all entry point methods for parsing specific constructs
- Runs versioning visitor for version-specific validation

### Step 6: Create Script Generator

Create the script generator class for generating T-SQL from the AST.

**File**: `SqlScriptDom/ScriptDom/SqlServer/Sql180ScriptGenerator.cs`

```csharp
//------------------------------------------------------------------------------
// <copyright file="Sql180ScriptGenerator.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// Script generator for T-SQL 180 (SQL Server Yellowstone/vnext)
    /// </summary>
    public sealed class Sql180ScriptGenerator : SqlScriptGenerator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Sql180ScriptGenerator"/> class.
        /// </summary>
        public Sql180ScriptGenerator()
            : this(new SqlScriptGeneratorOptions())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Sql180ScriptGenerator"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public Sql180ScriptGenerator(SqlScriptGeneratorOptions options)
            : base(options)
        {
            options.SqlVersion = SqlVersion.Sql180;
        }

        internal override SqlScriptGeneratorVisitor CreateSqlScriptGeneratorVisitor(
            SqlScriptGeneratorOptions options, ScriptWriter scriptWriter)
        {
            ScriptGeneratorSupporter.CheckForNullReference(options, "options");
            ScriptGeneratorSupporter.CheckForNullReference(scriptWriter, "scriptWriter");

            return new Sql180ScriptGeneratorVisitor(options, scriptWriter);
        }
    }
}
```

**Key Points**:
- Sealed class that cannot be inherited
- Sets `SqlVersion.Sql180` in options
- Creates version-specific visitor for AST traversal

### Step 7: Create Script Generator Visitor

Create the visitor class that generates T-SQL from the AST.

**File**: `SqlScriptDom/ScriptDom/SqlServer/ScriptGenerator/Sql180ScriptGeneratorVisitor.cs`

```csharp
//------------------------------------------------------------------------------
// <copyright file="Sql180ScriptGeneratorVisitor.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    internal partial class Sql180ScriptGeneratorVisitor : Sql170ScriptGeneratorVisitor
    {
        public Sql180ScriptGeneratorVisitor(SqlScriptGeneratorOptions options, ScriptWriter writer)
            : base(options, writer)
        {
        }

        // Add version-specific visitor overrides here as needed
        // Override visit methods for AST nodes that have new syntax in this version
    }
}
```

**Key Points**:
- Inherits from previous version's visitor (e.g., `Sql170ScriptGeneratorVisitor`)
- Override visit methods only for nodes with new version-specific syntax
- Uses visitor pattern to traverse and generate T-SQL from AST

### Step 8: Update TSqlParser Factory

Add the new version to the parser factory method.

**File**: `SqlScriptDom/Parser/TSql/TSqlParser.cs`

Find the `CreateParser` method and add a case:

```csharp
public static TSqlParser CreateParser(SqlVersion tsqlParserVersion, bool initialQuotedIdentifiers = false)
{
    switch (tsqlParserVersion)
    {
        // ... existing cases ...
        case SqlVersion.Sql170:
            return new TSql170Parser(initialQuotedIdentifiers);
        case SqlVersion.Sql180:
            return new TSql180Parser(initialQuotedIdentifiers);
        default:
            throw new ArgumentException(
                String.Format(CultureInfo.CurrentCulture, 
                    SqlScriptGeneratorResource.UnknownEnumValue, 
                    tsqlParserVersion, 
                    "TSqlParserVersion"), 
                "tsqlParserVersion");
    }
}
```

### Step 9: Add SqlVersionFlags Support

Add version flags for the new version if needed for validation logic.

**File**: `SqlScriptDom/Parser/TSql/SqlVersionFlags.cs`

```csharp
[Flags]
internal enum SqlVersionFlags
{
    // ... existing flags ...
    TSql170AndAbove = TSql170 | TSql180,
    TSql180 = 0x00000800,
    TSql180AndAbove = TSql180,
}
```

**Key Points**:
- Add individual version flag as power of 2
- Add `AndAbove` flag combining all versions >= this version
- Used for validation logic that checks syntax availability

### Step 10: Update OptionsHelper (if needed)

If the new version introduces new options or changes option availability, update the options helper.

**File**: `SqlScriptDom/Parser/TSql/OptionsHelper.cs`

Add version checks where options become available:

```csharp
// Example: if a new option becomes available in 180
internal static bool IsOptionValidForVersion(IndexOptionKind option, SqlVersion version)
{
    switch (option)
    {
        // ... existing options ...
        case IndexOptionKind.NewOption:
            return version >= SqlVersion.Sql180;
        default:
            return true;
    }
}
```

### Step 11: Create Test Infrastructure

Create test class for version-specific syntax tests.

**File**: `Test/SqlDom/Only180SyntaxTests.cs`

```csharp
using Microsoft.SqlServer.TransactSql.ScriptDom;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlStudio.Tests.AssemblyTools.TestCategory;
using System;
using System.Collections.Generic;
using System.IO;

namespace SqlStudio.Tests.UTSqlScriptDom
{
    public partial class SqlDomTests
    {
        // Note: These filenames are case sensitive, match checked-in files exactly
        private static readonly ParserTest[] Only180TestInfos =
        {
            // Add new 180-specific tests here as needed
            // Example:
            // new ParserTest180("NewFeatureTest180.sql"),
        };

        private static readonly ParserTest[] SqlAzure180_TestInfos =
        {
            // Add Azure-specific syntax tests here
        };

        /// <summary>
        /// Test method for TSql180 version-specific syntax
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql180SyntaxTests()
        {
            TSql180Parser parser = new TSql180Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql180);

            foreach (ParserTest t in Only180TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, t);
            }
        }

        /// <summary>
        /// Test method for Azure-specific TSql180 syntax
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql180AzureSyntaxTests()
        {
            TSql180Parser parser = new TSql180Parser(true, SqlEngineType.SqlAzure);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql180);

            foreach (ParserTest t in SqlAzure180_TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, t);
            }
        }
    }
}
```

### Step 12: Create Baselines Folder

Create the baselines folder for test output files.

**Directory**: `Test/SqlDom/Baselines180/`

- Create this folder even if initially empty
- Test baseline files (expected T-SQL output) will be placed here
- Baselines are generated from test runs and verified manually

### Step 13: Update Test Project Configuration

Add the baselines folder to embedded resources.

**File**: `Test/SqlDom/UTSqlScriptDom.csproj`

```xml
<ItemGroup>
  <EmbeddedResource Include="Baselines150\*.sql" />
  <EmbeddedResource Include="Baselines160\*.sql" />
  <EmbeddedResource Include="Baselines170\*.sql" />
  <EmbeddedResource Include="Baselines180\*.sql" />
  <EmbeddedResource Include="BaselinesFabricDW\*.sql" />
  <!-- ... other resources ... -->
</ItemGroup>
```

### Step 14: Add Parser Test Helper Methods

Add test utility methods for the new version.

**File**: `Test/SqlDom/ParserTest.cs`

Add constructor helper method:

```csharp
/// <summary>
/// Creates a parser test for TSql180 with specified error expectations
/// </summary>
public static ParserTest ParserTest180(
    string fileName,
    int nErrors80 = 0, int nErrors90 = 0, int nErrors100 = 0,
    int nErrors110 = 0, int nErrors120 = 0, int nErrors130 = 0,
    int nErrors140 = 0, int nErrors150 = 0, int nErrors160 = 0,
    int nErrors170 = 0, int nErrors180 = 0, int nErrorsFabricDW = int.MaxValue)
{
    return new ParserTest(
        fileName,
        new int[]
        {
            nErrors80, nErrors90, nErrors100, nErrors110,
            nErrors120, nErrors130, nErrors140, nErrors150,
            nErrors160, nErrors170, nErrors180, nErrorsFabricDW
        });
}
```

**File**: `Test/SqlDom/TestUtilities.cs`

Update the error count array handling to support the new version index.

### Step 15: Add Version Mapping Tests

Add tests to verify version enum and parser mapping.

**File**: `Test/SqlDom/TSqlParserTest.cs`

```csharp
[TestMethod]
public void TSql180ParserVersionTest()
{
    TSqlParser parser = TSqlParser.CreateParser(SqlVersion.Sql180);
    Assert.IsNotNull(parser);
    Assert.IsInstanceOfType(parser, typeof(TSql180Parser));
}
```

### Step 16: Build and Verify

Build the project to generate parser code and verify everything compiles.

```bash
# Build the ScriptDom project
dotnet build SqlScriptDom/Microsoft.SqlServer.TransactSql.ScriptDom.csproj -c Debug

# Run tests to verify
dotnet test Test/SqlDom/UTSqlScriptDom.csproj -c Debug
```

**What Happens During Build**:
1. ANTLR generates lexer and parser C# files from `.g` grammar
2. Post-processing scripts clean up generated code
3. Generated files are compiled along with hand-written sources
4. Test baselines are embedded as resources

## Summary Checklist

When adding a new parser version, ensure you complete:

- [ ] **Step 1**: Add `SqlVersion` enum value
- [ ] **Step 2**: Create `.g` grammar file
- [ ] **Step 3**: Add grammar to `GenerateFiles.props`
- [ ] **Step 4**: Create `TSqlXXXParserBaseInternal.cs`
- [ ] **Step 5**: Create `TSqlXXXParser.cs`
- [ ] **Step 6**: Create `SqlXXXScriptGenerator.cs`
- [ ] **Step 7**: Create `SqlXXXScriptGeneratorVisitor.cs`
- [ ] **Step 8**: Update `TSqlParser.cs` factory method
- [ ] **Step 9**: Add `SqlVersionFlags` support
- [ ] **Step 10**: Update `OptionsHelper.cs` if needed
- [ ] **Step 11**: Create `OnlyXXXSyntaxTests.cs`
- [ ] **Step 12**: Create `BaselinesXXX/` folder
- [ ] **Step 13**: Update `UTSqlScriptDom.csproj`
- [ ] **Step 14**: Add parser test helper methods
- [ ] **Step 15**: Add version mapping tests
- [ ] **Step 16**: Build and verify everything compiles

## Common Pitfalls

1. **Forgetting to add grammar to GenerateFiles.props**: Parser won't be generated
2. **Incorrect inheritance chain**: Must inherit from previous version's base class
3. **Missing entry point rules in grammar**: Certain parsing scenarios will fail
4. **Not creating Baselines folder**: Test project won't build due to missing embedded resources
5. **Wrong enum value**: Can conflict with existing versions or break version comparison logic
6. **Missing SqlVersionFlags**: Validation logic may not work correctly
7. **Not updating factory method**: Parser can't be instantiated via factory

## Related Documentation

- [Testing Guidelines](testing.guidelines.instructions.md) - How to add tests for new syntax
- [Bug Fixing Guidelines](bug_fixing.guidelines.instructions.md) - How to add new grammar rules
- [Grammar Guidelines](grammer.guidelines.instructions.md) - ANTLR v2 grammar patterns
- [Validation Fix Guidelines](Validation_fix.guidelines.instructions.md) - Version-gated validation

## References

- Example Implementation: TSql180 parser (commits 0b84f8f through f4f4c88)
- ANTLR v2 Documentation: See `tools/antlr-2.7.5` documentation
- Build System: See `SqlScriptDom/GenerateFiles.props` for generation targets
