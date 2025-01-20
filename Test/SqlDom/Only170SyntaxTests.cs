using Microsoft.SqlServer.TransactSql.ScriptDom;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlStudio.Tests.AssemblyTools.TestCategory;

namespace SqlStudio.Tests.UTSqlScriptDom
{
    public partial class SqlDomTests
    {
        // Note: These filenames are case sensitive, make sure they match the checked-in file exactly
        private static readonly ParserTest[] Only170TestInfos =
        {
            new ParserTest170("RegexpTVFTests170.sql", nErrors80: 0, nErrors90: 0, nErrors100: 0, nErrors110: 0, nErrors120: 0, nErrors130: 0, nErrors140: 0, nErrors150: 0, nErrors160: 0)
        };

        private static readonly ParserTest[] SqlAzure170_TestInfos =
        {
            // Add parser tests that have Azure-specific syntax constructs
            // (those that have versioning visitor implemented)
            //
        };

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

    }
}