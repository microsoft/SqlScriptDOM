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
        private static readonly ParserTest[] Only170TestInfos =
        {
            new ParserTest170("RegexpTVFTests170.sql", nErrors80: 1, nErrors90: 1, nErrors100: 0, nErrors110: 0, nErrors120: 0, nErrors130: 0, nErrors140: 0, nErrors150: 0, nErrors160: 0),
            new ParserTest170("JsonIndexTests170.sql", nErrors80: 2, nErrors90: 10, nErrors100: 10, nErrors110: 10, nErrors120: 10, nErrors130: 10, nErrors140: 10, nErrors150: 10, nErrors160: 10),
            new ParserTest170("VectorIndexTests170.sql", nErrors80: 2, nErrors90: 12, nErrors100: 12, nErrors110: 12, nErrors120: 12, nErrors130: 12, nErrors140: 12, nErrors150: 12, nErrors160: 12),
            new ParserTest170("AlterDatabaseManualCutoverTests170.sql", nErrors80: 4, nErrors90: 4, nErrors100: 4, nErrors110: 4, nErrors120: 4, nErrors130: 4, nErrors140: 4, nErrors150: 4, nErrors160: 4),
            new ParserTest170("CreateColumnStoreIndexTests170.sql", nErrors80: 3, nErrors90: 3, nErrors100: 3, nErrors110: 3, nErrors120: 3, nErrors130: 0, nErrors140: 0, nErrors150: 0, nErrors160: 0),
            new ParserTest170("RegexpTests170.sql", nErrors80: 0, nErrors90: 0, nErrors100: 0, nErrors110: 0, nErrors120: 0, nErrors130: 0, nErrors140: 0, nErrors150: 0, nErrors160: 0),
            new ParserTest170("AiGenerateChunksTests170.sql", nErrors80: 19, nErrors90: 16, nErrors100: 15, nErrors110: 15, nErrors120: 15, nErrors130: 15, nErrors140: 15, nErrors150: 15, nErrors160: 15),
            new ParserTest170("JsonFunctionTests170.sql", nErrors80: 13, nErrors90: 8, nErrors100: 38, nErrors110: 38, nErrors120: 38, nErrors130: 38, nErrors140: 38, nErrors150: 38, nErrors160: 38),
            new ParserTest170("AiGenerateEmbeddingsTests170.sql", nErrors80: 14, nErrors90: 11, nErrors100: 11, nErrors110: 11, nErrors120: 11, nErrors130: 11, nErrors140: 11, nErrors150: 11, nErrors160: 11),
            new ParserTest170("CreateExternalModelStatementTests170.sql", nErrors80: 2, nErrors90: 2, nErrors100: 2, nErrors110: 2, nErrors120: 2, nErrors130: 4, nErrors140: 4, nErrors150: 4, nErrors160: 4),
            new ParserTest170("AlterExternalModelStatementTests170.sql", nErrors80: 2, nErrors90: 2, nErrors100: 2, nErrors110: 2, nErrors120: 2, nErrors130: 5, nErrors140: 5, nErrors150: 5, nErrors160: 5),
            new ParserTest170("DropExternalModelStatementTests170.sql", nErrors80: 1, nErrors90: 1, nErrors100: 1, nErrors110: 1, nErrors120: 1, nErrors130: 1, nErrors140: 1, nErrors150: 1, nErrors160: 1),
            new ParserTest170("VectorFunctionTests170.sql", nErrors80: 1, nErrors90: 1, nErrors100: 3, nErrors110: 3, nErrors120: 3, nErrors130: 3, nErrors140: 3, nErrors150: 3, nErrors160: 3),
            new ParserTest170("SecurityStatementExternalModelTests170.sql", nErrors80: 2, nErrors90: 17, nErrors100: 17, nErrors110: 17, nErrors120: 17, nErrors130: 17, nErrors140: 17, nErrors150: 17, nErrors160: 17),
            new ParserTest170("CreateEventSessionNotLikePredicate.sql", nErrors80: 2, nErrors90: 1,  nErrors100: 1, nErrors110: 1, nErrors120: 1, nErrors130: 0, nErrors140: 0, nErrors150: 0, nErrors160: 0),
            new ParserTest170("VectorTypeSecondParameterTests170.sql", nErrors80: 2, nErrors90: 2, nErrors100: 2, nErrors110: 2, nErrors120: 2, nErrors130: 2, nErrors140: 2, nErrors150: 2, nErrors160: 2),
            new ParserTest170("RegexpLikeTests170.sql", nErrors80: 15, nErrors90: 15, nErrors100: 15, nErrors110: 18, nErrors120: 18, nErrors130: 18, nErrors140: 18, nErrors150: 18, nErrors160: 18)
            new ParserTest170("OptimizedLockingTests170.sql", nErrors80: 2, nErrors90: 2, nErrors100: 2, nErrors110: 2, nErrors120: 2, nErrors130: 2, nErrors140: 2, nErrors150: 2, nErrors160: 2),
        };

        private static readonly ParserTest[] SqlAzure170_TestInfos =
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
        public void GraphTSql170SyntaxParserTest()
        {
            TSql170Parser parser = new TSql170Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql170);
            foreach (ParserTest ti in GraphTestsOnly)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result170);
            }

            // Parsing both for Azure and standalone flavors - explicitly set
            // enum value to 'All'
            //
            parser = new TSql170Parser(true, SqlEngineType.All);
            scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql170);
            scriptGen.Options.SqlEngineType = SqlEngineType.All;
            foreach (ParserTest ti in GraphTestsOnly)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result170);
            }

            // Explicitly ask for 'standalone' and parse
            //
            parser = new TSql170Parser(true, SqlEngineType.Standalone);
            scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql170);
            scriptGen.Options.SqlEngineType = SqlEngineType.Standalone;
            foreach (ParserTest ti in GraphTestsOnly)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result170);
            }
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql170SyntaxIn170ParserTest()
        {
            // Parsing both for Azure and standalone flavors
            //
            TSql170Parser parser = new TSql170Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql170);
            foreach (ParserTest ti in Only170TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result170);
            }

            // Parsing both for Azure and standalone flavors - explicitly set
            // enum value to 'All'
            //
            parser = new TSql170Parser(true, SqlEngineType.All);
            scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql170);
            scriptGen.Options.SqlEngineType = SqlEngineType.All;
            foreach (ParserTest ti in Only170TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result170);
            }

            // Explicitly ask for 'standalone' and parse
            //
            parser = new TSql170Parser(true, SqlEngineType.Standalone);
            scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql170);
            scriptGen.Options.SqlEngineType = SqlEngineType.Standalone;
            foreach (ParserTest ti in Only170TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result170);
            }
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql170Azure_SyntaxIn170ParserTest()
        {
            // This test checks that Azure-specific syntax constructs are successfully parsed
            // with Sql170 parser pointed to Sql Azure.
            //
            TSql170Parser parser = new TSql170Parser(true, SqlEngineType.SqlAzure);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql170);
            scriptGen.Options.SqlEngineType = SqlEngineType.SqlAzure;
            foreach (ParserTest ti in SqlAzure170_TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result170);
            }
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql170SyntaxIn160ParserTest()
        {
            TSql150Parser parser = new TSql150Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql160);
            System.Collections.Generic.List<ParserTest> all170ParserTests = new System.Collections.Generic.List<ParserTest>();
            all170ParserTests.AddRange(Only170TestInfos);
            all170ParserTests.AddRange(SqlAzure170_TestInfos);

            foreach (ParserTest ti in all170ParserTests)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result160);
            }
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql170SyntaxIn150ParserTest()
        {
            TSql150Parser parser = new TSql150Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql150);
            System.Collections.Generic.List<ParserTest> all170ParserTests = new System.Collections.Generic.List<ParserTest>();
            all170ParserTests.AddRange(Only170TestInfos);
            all170ParserTests.AddRange(SqlAzure170_TestInfos);

            foreach (ParserTest ti in all170ParserTests)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result150);
            }
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql170SyntaxIn140ParserTest()
        {
            TSql140Parser parser = new TSql140Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql140);
            System.Collections.Generic.List<ParserTest> all170ParserTests = new System.Collections.Generic.List<ParserTest>();
            all170ParserTests.AddRange(Only170TestInfos);
            all170ParserTests.AddRange(SqlAzure170_TestInfos);

            foreach (ParserTest ti in all170ParserTests)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result140);
            }
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql170SyntaxIn130ParserTest()
        {
            TSql130Parser parser = new TSql130Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql130);
            System.Collections.Generic.List<ParserTest> all170ParserTests = new System.Collections.Generic.List<ParserTest>();
            all170ParserTests.AddRange(Only170TestInfos);
            all170ParserTests.AddRange(SqlAzure170_TestInfos);

            foreach (ParserTest ti in all170ParserTests)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result130);
            }
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql170SyntaxIn120ParserTest()
        {
            TSql120Parser parser = new TSql120Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql120);
            System.Collections.Generic.List<ParserTest> all170ParserTests = new System.Collections.Generic.List<ParserTest>();
            all170ParserTests.AddRange(Only170TestInfos);
            all170ParserTests.AddRange(SqlAzure170_TestInfos);

            foreach (ParserTest ti in all170ParserTests)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result120);
            }
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql170SyntaxIn110ParserTest()
        {
            TSql110Parser parser = new TSql110Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql110);
            System.Collections.Generic.List<ParserTest> all170ParserTests = new System.Collections.Generic.List<ParserTest>();
            all170ParserTests.AddRange(Only170TestInfos);
            all170ParserTests.AddRange(SqlAzure170_TestInfos);

            foreach (ParserTest ti in all170ParserTests)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result110);
            }
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql170SyntaxIn100ParserTest()
        {
            TSql100Parser parser = new TSql100Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql100);
            System.Collections.Generic.List<ParserTest> all170ParserTests = new System.Collections.Generic.List<ParserTest>();
            all170ParserTests.AddRange(Only170TestInfos);
            all170ParserTests.AddRange(SqlAzure170_TestInfos);

            foreach (ParserTest ti in all170ParserTests)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result100);
            }
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql170SyntaxIn90ParserTest()
        {
            TSql90Parser parser = new TSql90Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql90);
            System.Collections.Generic.List<ParserTest> all170ParserTests = new System.Collections.Generic.List<ParserTest>();
            all170ParserTests.AddRange(Only170TestInfos);
            all170ParserTests.AddRange(SqlAzure170_TestInfos);

            foreach (ParserTest ti in all170ParserTests)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result90);
            }
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql170SyntaxIn80ParserTest()
        {
            TSql80Parser parser = new TSql80Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql80);
            System.Collections.Generic.List<ParserTest> all170ParserTests = new System.Collections.Generic.List<ParserTest>();
            all170ParserTests.AddRange(Only170TestInfos);
            all170ParserTests.AddRange(SqlAzure170_TestInfos);

            foreach (ParserTest ti in all170ParserTests)
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
        public void TSql160SyntaxIn170ParserTest()
        {
            TSql170Parser parser = new TSql170Parser(true, SqlEngineType.Standalone);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql160);
            foreach (ParserTest ti in Only140TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result160);
            }
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql150SyntaxIn170ParserTest()
        {
            TSql170Parser parser = new TSql170Parser(true, SqlEngineType.Standalone);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql150);
            foreach (ParserTest ti in Only140TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result150);
            }
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql140SyntaxIn170ParserTest()
        {
            TSql170Parser parser = new TSql170Parser(true, SqlEngineType.Standalone);
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
        public void TSql130SyntaxIn170ParserTest()
        {
            TSql170Parser parser = new TSql170Parser(true, SqlEngineType.Standalone);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql130);
            foreach (ParserTest ti in Only130TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result130);
            }
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql120SyntaxIn170ParserTest()
        {
            TSql170Parser parser = new TSql170Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql120);
            foreach (ParserTest ti in Only120TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result120);
            }
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql110SyntaxIn170ParserTest()
        {
            TSql170Parser parser = new TSql170Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql110);
            foreach (ParserTest ti in Only110TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result110);
            }
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql100SyntaxIn170ParserTest()
        {
            TSql170Parser parser = new TSql170Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql100);
            foreach (ParserTest ti in Only100TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result100);
            }
        }
    }
}