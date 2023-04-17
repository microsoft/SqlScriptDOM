//------------------------------------------------------------------------------
// <copyright file="Only150SyntaxTests.cs" company="Microsoft">
//		 Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using Microsoft.SqlServer.TransactSql.ScriptDom;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlStudio.Tests.AssemblyTools.TestCategory;

namespace SqlStudio.Tests.UTSqlScriptDom
{
    public partial class SqlDomTests
    {
        // Note: These filenames are case sensitive, make sure they match the checked-in file exactly
        private static readonly ParserTest[] Only150TestInfos = 
        {
            new ParserTest150("AlterServerConfigurationExternalAuthenticationTests150.sql", 2, 3, 3, 3, 3, 3, 3),
            new ParserTest150("AcceleratedDatabaseRecoveryTests150.sql",  2, 2, 2, 2, 2, 2, 2),
            new ParserTest150("AlwaysEncryptedTests150.sql", 1, 2, 2, 2, 2, 1, 1),
            new ParserTest150("AlterCreateDatabaseFilePath150.sql",  4, 4, 4, 4, 4, 4, 4),
            new ParserTest150("AlterIndexStatementTests150.sql",  2, 4, 4, 4, 4, 4, 4),
            new ParserTest150("CreateIndexStatementTests150.sql",  9, 9, 9, 9, 9, 9, 9),
            new ParserTest150("CreateTableTests150.sql", 5, 5, 5, 5, 5, 5, 5),
            new ParserTest150("ScalarUDFInlineTests150.sql", 4, 5, 5, 5, 5, 4, 4),
            new ParserTest150("GraphDbSyntaxTests150.sql", 5, 5, 5, 5, 5, 5, 5),
            new ParserTest150("BulkInsertStatementTests150.sql", 6, 6, 6, 6, 6, 6, 6),
            new ParserTest150("OpenRowsetBulkStatementTests150.sql", 10, 10, 10, 10, 10, 10, 10),
            /* When a batch is being parsed via a rule and the rule consumes all the tokens it can then it expects
             the batch to end with a GO. If there is not a 'GO' then the parser assumes that we have reached EOF.
             This is the current parser behavior. So any new addition to the end of the rule will result in parsing errors 
             in previous versions (in this case 130 and 140). So the parser terminates givin error as - expecting EOF but found 'newToken'.
             The parser terminates on that batch itself without parsing the rest of the batches in the file. 
             So for this new syntax, spliting the two scenarios in two seperate files to cover the test cases. */
            new ParserTest150("AlterDatabaseScopedConfigClearProcCacheTests150.sql", 2, 2, 2, 2, 2, 1, 1),
            new ParserTest150("AlterDatabaseScopedConfigForSecondaryClearProcCacheTests150.sql", 2, 2, 2, 2, 2, 1, 1),
            new ParserTest150("GraphDbShortestPathSyntaxTests150.sql", 40, 40, 40, 40, 40, 40, 32),
            new ParserTest150("LoginStatementTests150.sql", 2, 10, 10, 10, 10, 10, 10),
            new ParserTest150("DataClassificationTests150.sql", 1, 4, 4, 4, 4, 0, 0),
            new ParserTest150("ServerAuditSpecificationStatementTests150.sql", 5, 8, 1, 1, 1, 1, 1),
            new ParserTest150("DatabaseAuditSpecificationStatementTests150.sql", 8, 8, 1, 1, 1, 1, 1),
            new ParserTest150("ServerAuditStatementTests150.sql", 5, 15, 6, 6, 6, 6, 6),
            new ParserTest150("OptimizerHintsTests150.sql", 1, 1, 1, 1, 1, 0, 0),
            new ParserTest150("GroupByClauseTests150.sql", 4, 4, 2, 2, 2, 0, 0),
            new ParserTest150("UniqueConstraintTests150.sql", 6, 6, 6, 6, 6, 6, 6),
            new ParserTest150("CreateExternalLibrary150.sql", 2, 2, 2, 2, 2, 4, 2),
            new ParserTest150("AlterExternalLibrary150.sql", 2, 2, 2, 2, 2, 4, 2),
            new ParserTest150("CreateExternalLanguage150.sql", 2, 2, 2, 2, 2, 4, 4),
            new ParserTest150("AlterExternalLanguage150.sql", 2, 2, 2, 2, 2, 6, 6),
            new ParserTest150("DropExternalLanguage150.sql", 2, 2, 2, 2, 2, 2, 2),
            new ParserTest150("FromClauseTests150.sql", 2, 2, 0, 0, 0, 0, 0),
            new ParserTest150("DeclareTableVariableTests150.sql", 1, 1, 1, 1, 1, 1, 1)
        };

        /// <summary>
        /// Only the graph tests.
        /// </summary>
        private static readonly ParserTest150[] GraphTestsOnly = 
        {
            new ParserTest150("GraphDbSyntaxTests150.sql", 5, 5, 5, 5, 5, 5, 5),
            new ParserTest150("GraphDbShortestPathSyntaxTests150.sql", 38, 38, 38, 38, 38, 38, 30),
            new ParserTest150("GraphDbEdgeConstraintsSyntaxTests150.sql", 5, 5, 5, 5, 5, 5, 5),
        };

        private static readonly ParserTest[] SqlAzure150_TestInfos = 
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
        public void GraphTSql150SyntaxParserTest()
        {
            TSql150Parser parser = new TSql150Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql150);
            foreach (ParserTest ti in GraphTestsOnly)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result150);
            }

            // Parsing both for Azure and standalone flavors - explicitly set
            // enum value to 'All'
            //
            parser = new TSql150Parser(true, SqlEngineType.All);
            scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql150);
            scriptGen.Options.SqlEngineType = SqlEngineType.All;
            foreach (ParserTest ti in GraphTestsOnly)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result150);
            }

            // Explicitly ask for 'standalone' and parse
            //
            parser = new TSql150Parser(true, SqlEngineType.Standalone);
            scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql150);
            scriptGen.Options.SqlEngineType = SqlEngineType.Standalone;
            foreach (ParserTest ti in GraphTestsOnly)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result150);
            }
        }

        [TestMethod] 
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql150SyntaxIn150ParserTest()
        {
            // Parsing both for Azure and standalone flavors
            //
            TSql150Parser parser = new TSql150Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql150);
            foreach (ParserTest ti in Only150TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result150);
            }

            // Parsing both for Azure and standalone flavors - explicitly set
            // enum value to 'All'
            //
            parser = new TSql150Parser(true, SqlEngineType.All);
            scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql150);
            scriptGen.Options.SqlEngineType = SqlEngineType.All;
            foreach (ParserTest ti in Only150TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result150);
            }

            // Explicitly ask for 'standalone' and parse
            //
            parser = new TSql150Parser(true, SqlEngineType.Standalone);
            scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql150);
            scriptGen.Options.SqlEngineType = SqlEngineType.Standalone;
            foreach (ParserTest ti in Only150TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result150);
            }
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql150Azure_SyntaxIn150ParserTest()
        {
            // This test checks that Azure-specific syntax constructs are successfully parsed
            // with Sql150 parser pointed to Sql Azure.
            //
            TSql150Parser parser = new TSql150Parser(true, SqlEngineType.SqlAzure);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql150);
            scriptGen.Options.SqlEngineType = SqlEngineType.SqlAzure;
            foreach (ParserTest ti in SqlAzure150_TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result150);
            }
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql150SyntaxIn140ParserTest()
        {
            TSql140Parser parser = new TSql140Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql140);
            System.Collections.Generic.List<ParserTest> all150ParserTests = new System.Collections.Generic.List<ParserTest>();
            all150ParserTests.AddRange(Only150TestInfos);
            all150ParserTests.AddRange(SqlAzure150_TestInfos);

            foreach (ParserTest ti in all150ParserTests)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result140);
            }
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql150SyntaxIn130ParserTest()
        {
            TSql130Parser parser = new TSql130Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql130);
            System.Collections.Generic.List<ParserTest> all150ParserTests = new System.Collections.Generic.List<ParserTest>();
            all150ParserTests.AddRange(Only150TestInfos);
            all150ParserTests.AddRange(SqlAzure150_TestInfos);

            foreach (ParserTest ti in all150ParserTests)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result130);
            }
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql150SyntaxIn120ParserTest()
        {
            TSql120Parser parser = new TSql120Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql120);
            System.Collections.Generic.List<ParserTest> all150ParserTests = new System.Collections.Generic.List<ParserTest>();
            all150ParserTests.AddRange(Only150TestInfos);
            all150ParserTests.AddRange(SqlAzure150_TestInfos);

            foreach (ParserTest ti in all150ParserTests)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result120);
            }
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql150SyntaxIn110ParserTest()
        {
            TSql110Parser parser = new TSql110Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql110);
            System.Collections.Generic.List<ParserTest> all150ParserTests = new System.Collections.Generic.List<ParserTest>();
            all150ParserTests.AddRange(Only150TestInfos);
            all150ParserTests.AddRange(SqlAzure150_TestInfos);

            foreach (ParserTest ti in all150ParserTests)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result110);
            }
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql150SyntaxIn100ParserTest()
        {
            TSql100Parser parser = new TSql100Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql100);
            System.Collections.Generic.List<ParserTest> all150ParserTests = new System.Collections.Generic.List<ParserTest>();
            all150ParserTests.AddRange(Only150TestInfos);
            all150ParserTests.AddRange(SqlAzure150_TestInfos);

            foreach (ParserTest ti in all150ParserTests)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result100);
            }
        }

        [TestMethod] 
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql150SyntaxIn90ParserTest()
        {
            TSql90Parser parser = new TSql90Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql90);
            System.Collections.Generic.List<ParserTest> all150ParserTests = new System.Collections.Generic.List<ParserTest>();
            all150ParserTests.AddRange(Only150TestInfos);
            all150ParserTests.AddRange(SqlAzure150_TestInfos);

            foreach (ParserTest ti in all150ParserTests)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result90);
            }
        }

        [TestMethod] 
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql150SyntaxIn80ParserTest()
        {
            TSql80Parser parser = new TSql80Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql80);
            System.Collections.Generic.List<ParserTest> all150ParserTests = new System.Collections.Generic.List<ParserTest>();
            all150ParserTests.AddRange(Only150TestInfos);
            all150ParserTests.AddRange(SqlAzure150_TestInfos);

            foreach (ParserTest ti in all150ParserTests)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result80);
            }
        }

        /// <summary>
        /// Let's make sure the older syntax tests still pass in the new parser
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql140SyntaxIn150ParserTest()
        {
            TSql150Parser parser = new TSql150Parser(true, SqlEngineType.Standalone);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql140);
            foreach (ParserTest ti in Only140TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result140);
            }
        }

        /// <summary>
        /// Let's make sure the older syntax tests still pass in the new parser
        /// </summary>
        [TestMethod] 
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql130SyntaxIn150ParserTest()
        {
            TSql150Parser parser = new TSql150Parser(true, SqlEngineType.Standalone);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql150);
            foreach (ParserTest ti in Only130TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result130);
            }
        }

        [TestMethod] 
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql120SyntaxIn150ParserTest()
        {
            TSql150Parser parser = new TSql150Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql150);
            foreach (ParserTest ti in Only120TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result120);
            }
        }

        [TestMethod] 
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql110SyntaxIn150ParserTest()
        {
            TSql150Parser parser = new TSql150Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql150);
            foreach (ParserTest ti in Only110TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result110);
            }
        }

        [TestMethod] 
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql100SyntaxIn150ParserTest()
        {
            TSql150Parser parser = new TSql150Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql150);
            foreach (ParserTest ti in Only100TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result100);
            }
        }
    }
}
