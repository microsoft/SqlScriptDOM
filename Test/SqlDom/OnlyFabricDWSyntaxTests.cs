using Microsoft.SqlServer.TransactSql.ScriptDom;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlStudio.Tests.AssemblyTools.TestCategory;

namespace SqlStudio.Tests.UTSqlScriptDom
{
    public partial class SqlDomTests
    {
        // Note: These filenames are case sensitive, make sure they match the checked-in file exactly
        private static readonly ParserTest[] OnlyFabricDWTestInfos =
        {
            new ParserTestFabricDW("CloneTableTestsFabricDW.sql", nErrors80: 4, nErrors90: 4, nErrors100: 4, nErrors110: 4, nErrors120: 4, nErrors130: 4, nErrors140: 4, nErrors150: 4, nErrors160: 4, nErrors170: 4),
            new ParserTestFabricDW("CreateAlterTableClusterByTestsFabricDW.sql", nErrors80: 6, nErrors90: 6, nErrors100: 6, nErrors110: 6, nErrors120: 6, nErrors130: 6, nErrors140: 6, nErrors150: 6, nErrors160: 6, nErrors170: 6),
            new ParserTestFabricDW("CreateExternalTableStatementTestsFabricDW.sql", nErrors80: 2, nErrors90: 2, nErrors100: 2, nErrors110: 2, nErrors120: 2, nErrors130: 2, nErrors140: 2, nErrors150: 2, nErrors160: 0, nErrors170: 0),
            new ParserTestFabricDW("CreateProcedureCloneTableTestsFabricDW.sql", nErrors80: 4, nErrors90: 4, nErrors100: 4, nErrors110: 4, nErrors120: 4, nErrors130: 4, nErrors140: 4, nErrors150: 4, nErrors160: 4, nErrors170: 4),
            new ParserTestFabricDW("IdentityColumnTestsFabricDW.sql", nErrors80: 0, nErrors90: 0, nErrors100: 0, nErrors110: 0, nErrors120: 0, nErrors130: 0, nErrors140: 0, nErrors150: 0, nErrors160: 0, nErrors170: 0),
            new ParserTestFabricDW("NestedCTETestsFabricDW.sql", nErrors80: 1, nErrors90: 2, nErrors100: 2, nErrors110: 2, nErrors120: 2, nErrors130: 2, nErrors140: 2, nErrors150: 2, nErrors160: 2, nErrors170: 2),
            new ParserTestFabricDW("ScalarFunctionTestsFabricDW.sql", nErrors80: 3, nErrors90: 2, nErrors100: 2, nErrors110: 2, nErrors120: 2, nErrors130: 0, nErrors140: 0, nErrors150: 0, nErrors160: 0, nErrors170: 0)
        };

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSqlFabricDWSyntaxParserTest()
        {
            TSqlFabricDWParser parser = new TSqlFabricDWParser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.SqlFabricDW);
            foreach (ParserTest ti in OnlyFabricDWTestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._resultFabricDW);
            }
            
            scriptGen.Options.SqlEngineType = SqlEngineType.All;
            foreach (ParserTest ti in OnlyFabricDWTestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._resultFabricDW);
            }
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSqlFabricDWSyntaxIn80ParserTest()
        {
            TSql80Parser parser = new TSql80Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql80);
            foreach (ParserTest ti in OnlyFabricDWTestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result80);
            }
            
            scriptGen.Options.SqlEngineType = SqlEngineType.All;
            foreach (ParserTest ti in OnlyFabricDWTestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result80);
            }
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSqlFabricDWSyntaxIn90ParserTest()
        {
            TSql90Parser parser = new TSql90Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql90);
            foreach (ParserTest ti in OnlyFabricDWTestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result90);
            }

            scriptGen.Options.SqlEngineType = SqlEngineType.All;
            foreach (ParserTest ti in OnlyFabricDWTestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result90);
            }
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSqlFabricDWSyntaxIn100ParserTest()
        {
            TSql100Parser parser = new TSql100Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql100);
            foreach (ParserTest ti in OnlyFabricDWTestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result100);
            }

            scriptGen.Options.SqlEngineType = SqlEngineType.All;
            foreach (ParserTest ti in OnlyFabricDWTestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result100);
            }
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSqlFabricDWSyntaxIn110ParserTest()
        {
            TSql110Parser parser = new TSql110Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql110);
            foreach (ParserTest ti in OnlyFabricDWTestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result110);
            }

            scriptGen.Options.SqlEngineType = SqlEngineType.All;
            foreach (ParserTest ti in OnlyFabricDWTestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result110);
            }
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSqlFabricDWSyntaxIn120ParserTest()
        {
            TSql120Parser parser = new TSql120Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql120);
            foreach (ParserTest ti in OnlyFabricDWTestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result120);
            }

            scriptGen.Options.SqlEngineType = SqlEngineType.All;
            foreach (ParserTest ti in OnlyFabricDWTestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result120);
            }
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSqlFabricDWSyntaxIn130ParserTest()
        {
            TSql130Parser parser = new TSql130Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql130);
            foreach (ParserTest ti in OnlyFabricDWTestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result130);
            }

            scriptGen.Options.SqlEngineType = SqlEngineType.All;
            foreach (ParserTest ti in OnlyFabricDWTestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result130);
            }
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSqlFabricDWSyntaxIn140ParserTest()
        {
            TSql140Parser parser = new TSql140Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql140);
            foreach (ParserTest ti in OnlyFabricDWTestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result140);
            }

            scriptGen.Options.SqlEngineType = SqlEngineType.All;
            foreach (ParserTest ti in OnlyFabricDWTestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result140);
            }
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSqlFabricDWSyntaxIn150ParserTest()
        {
            TSql150Parser parser = new TSql150Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql150);
            foreach (ParserTest ti in OnlyFabricDWTestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result150);
            }

            scriptGen.Options.SqlEngineType = SqlEngineType.All;
            foreach (ParserTest ti in OnlyFabricDWTestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result150);
            }
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSqlFabricDWSyntaxIn160ParserTest()
        {
            TSql160Parser parser = new TSql160Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql160);
            foreach (ParserTest ti in OnlyFabricDWTestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result160);
            }

            scriptGen.Options.SqlEngineType = SqlEngineType.All;
            foreach (ParserTest ti in OnlyFabricDWTestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result160);
            }
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSqlFabricDWSyntaxIn170ParserTest()
        {
            TSql170Parser parser = new TSql170Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql170);
            foreach (ParserTest ti in OnlyFabricDWTestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result170);
            }

            scriptGen.Options.SqlEngineType = SqlEngineType.All;
            foreach (ParserTest ti in OnlyFabricDWTestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result170);
            }
        }
    }
}