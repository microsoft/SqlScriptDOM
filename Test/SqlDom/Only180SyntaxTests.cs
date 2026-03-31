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
        // Note: These filenames are case sensitive, make sure they match the checked-in file exactly
        private static readonly ParserTest[] Only180TestInfos =
        {
            // Add new 180-specific tests here as needed
        };

        private static readonly ParserTest[] SqlAzure180_TestInfos =
        {
            // Add parser tests that have Azure-specific syntax constructs
            // (those that have versioning visitor implemented)
            //
        };

        /// <summary>
        /// This test runs only the graph syntax tests.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void GraphTSql180SyntaxParserTest()
        {
            TSql180Parser parser = new TSql180Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql180);
            foreach (ParserTest ti in GraphTestsOnly)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result180);
            }

            // Parsing both for Azure and standalone flavors - explicitly set
            // enum value to 'All'
            //
            parser = new TSql180Parser(true, SqlEngineType.All);
            scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql180);
            scriptGen.Options.SqlEngineType = SqlEngineType.All;
            foreach (ParserTest ti in GraphTestsOnly)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result180);
            }

            // Explicitly ask for 'standalone' and parse
            //
            parser = new TSql180Parser(true, SqlEngineType.Standalone);
            scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql180);
            scriptGen.Options.SqlEngineType = SqlEngineType.Standalone;
            foreach (ParserTest ti in GraphTestsOnly)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result180);
            }
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql180SyntaxTests()
        {
            TSql180Parser parser = new TSql180Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql180);
            foreach (ParserTest ti in Only180TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result180);
            }
        }

        /// <summary>
        /// This test runs SQL Azure syntax tests - explicit check for Azure
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql180AzureOnlySyntaxTests()
        {
            TSql180Parser parser = new TSql180Parser(true, SqlEngineType.SqlAzure);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql180);
            scriptGen.Options.SqlEngineType = SqlEngineType.SqlAzure;
            foreach (ParserTest ti in SqlAzure180_TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result180);
            }
        }
    }
}
