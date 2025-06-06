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
            new ParserTestFabricDW("ScalarFunctionIntTestsFabricDW.sql", nErrors80: 0, nErrors90: 0, nErrors100: 0, nErrors110: 0, nErrors120: 0, nErrors130: 0, nErrors140: 0, nErrors150: 0, nErrors160: 0, nErrors170: 0),
            new ParserTestFabricDW("ScalarFunctionVarCharTestsFabricDW.sql", nErrors80: 0, nErrors90: 0, nErrors100: 0, nErrors110: 0, nErrors120: 0, nErrors130: 0, nErrors140: 0, nErrors150: 0, nErrors160: 0, nErrors170: 0)
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
            
            parser = new TSqlFabricDWParser(true, SqlEngineType.All);
            scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.SqlFabricDW);
            scriptGen.Options.SqlEngineType = SqlEngineType.All;
            foreach (ParserTest ti in OnlyFabricDWTestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._resultFabricDW);
            }
        }
    }
}