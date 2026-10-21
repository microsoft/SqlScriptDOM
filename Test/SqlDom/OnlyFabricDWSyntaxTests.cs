using System;
using System.IO;
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
            new ParserTestFabricDW("CloneTableTestsFabricDW.sql", nErrors80: 4, nErrors90: 4, nErrors100: 4, nErrors110: 4, nErrors120: 4, nErrors130: 4, nErrors140: 4, nErrors150: 4, nErrors160: 4, nErrors170: 4, nErrors180: 4),
            new ParserTestFabricDW("CreateAlterExternalDataSourceTestsFabricDW.sql", nErrors80: 2, nErrors90: 2, nErrors100: 2, nErrors110: 2, nErrors120: 2, nErrors130: 0, nErrors140: 0, nErrors150: 0, nErrors160: 0, nErrors170: 0, nErrors180: 0),
            new ParserTestFabricDW("CreateAlterTableClusterByTestsFabricDW.sql", nErrors80: 7, nErrors90: 7, nErrors100: 7, nErrors110: 7, nErrors120: 7, nErrors130: 7, nErrors140: 7, nErrors150: 7, nErrors160: 7, nErrors170: 7, nErrors180: 7),
            new ParserTestFabricDW("CreateExternalTableStatementTestsFabricDW.sql", nErrors80: 2, nErrors90: 2, nErrors100: 2, nErrors110: 2, nErrors120: 2, nErrors130: 2, nErrors140: 2, nErrors150: 2, nErrors160: 0, nErrors170: 0, nErrors180: 0),
            new ParserTestFabricDW("CreateProcedureCloneTableTestsFabricDW.sql", nErrors80: 4, nErrors90: 4, nErrors100: 4, nErrors110: 4, nErrors120: 4, nErrors130: 4, nErrors140: 4, nErrors150: 4, nErrors160: 4, nErrors170: 4, nErrors180: 4),
            new ParserTestFabricDW("IdentityColumnTestsFabricDW.sql", nErrors80: 0, nErrors90: 0, nErrors100: 0, nErrors110: 0, nErrors120: 0, nErrors130: 0, nErrors140: 0, nErrors150: 0, nErrors160: 0, nErrors170: 0, nErrors180: 0),
            new ParserTestFabricDW("NestedCTETestsFabricDW.sql", nErrors80: 1, nErrors90: 2, nErrors100: 2, nErrors110: 2, nErrors120: 2, nErrors130: 2, nErrors140: 2, nErrors150: 2, nErrors160: 2, nErrors170: 2, nErrors180: 2),
            new ParserTestFabricDW("ScalarFunctionTestsFabricDW.sql", nErrors80: 3, nErrors90: 2, nErrors100: 2, nErrors110: 2, nErrors120: 2, nErrors130: 0, nErrors140: 0, nErrors150: 0, nErrors160: 0, nErrors170: 0, nErrors180: 0),
            new ParserTestFabricDW("CreateEventSessionNotLikePredicate.sql", nErrors80: 2, nErrors90: 1,  nErrors100: 1, nErrors110: 1, nErrors120: 1, nErrors130: 0, nErrors140: 0, nErrors150: 0, nErrors160: 0, nErrors170: 0, nErrors180: 0),
            new ParserTestFabricDW("AiAnalyzeSentimentTestsFabricDW.sql", nErrors80: 5, nErrors90: 1, nErrors100: 5, nErrors110: 5, nErrors120: 5, nErrors130: 5, nErrors140: 5, nErrors150: 5, nErrors160: 5, nErrors170: 5, nErrors180: 5),
            new ParserTestFabricDW("AiClassifyTestsFabricDW.sql", nErrors80: 5, nErrors90: 1, nErrors100: 5, nErrors110: 5, nErrors120: 5, nErrors130: 5, nErrors140: 5, nErrors150: 5, nErrors160: 5, nErrors170: 5, nErrors180: 5),
            new ParserTestFabricDW("AiExtractTestsFabricDW.sql", nErrors80: 5, nErrors90: 1, nErrors100: 5, nErrors110: 5, nErrors120: 5, nErrors130: 5, nErrors140: 5, nErrors150: 5, nErrors160: 5, nErrors170: 5, nErrors180: 5),
            new ParserTestFabricDW("AiFixGrammarTestsFabricDW.sql", nErrors80: 5, nErrors90: 1, nErrors100: 5, nErrors110: 5, nErrors120: 5, nErrors130: 5, nErrors140: 5, nErrors150: 5, nErrors160: 5, nErrors170: 5, nErrors180: 5),
            new ParserTestFabricDW("AiGenerateResponseTestsFabricDW.sql", nErrors80: 6, nErrors90: 1, nErrors100: 5, nErrors110: 5, nErrors120: 5, nErrors130: 5, nErrors140: 5, nErrors150: 5, nErrors160: 5, nErrors170: 5, nErrors180: 5),
            new ParserTestFabricDW("AiSummarizeTestsFabricDW.sql", nErrors80: 5, nErrors90: 1, nErrors100: 5, nErrors110: 5, nErrors120: 5, nErrors130: 5, nErrors140: 5, nErrors150: 5, nErrors160: 5, nErrors170: 5, nErrors180: 5),
            new ParserTestFabricDW("AiTranslateTestsFabricDW.sql", nErrors80: 3, nErrors90: 1, nErrors100: 6, nErrors110: 6, nErrors120: 6, nErrors130: 6, nErrors140: 6, nErrors150: 6, nErrors160: 6, nErrors170: 6, nErrors180: 6),
            new ParserTestFabricDW("InvokeExternalApiTestsFabricDW.sql", nErrors80: 2, nErrors90: 1, nErrors100: 0, nErrors110: 0, nErrors120: 0, nErrors130: 0, nErrors140: 0, nErrors150: 0, nErrors160: 0, nErrors170: 0, nErrors180: 0),
            new ParserTestFabricDW("ExternalFunctionTestsFabricDW.sql", nErrors80: 12, nErrors90: 4, nErrors100: 4, nErrors110: 4, nErrors120: 4, nErrors130: 11, nErrors140: 11, nErrors150: 11, nErrors160: 11, nErrors170: 11, nErrors180: 0),
            new ParserTestFabricDW("CreateDatabaseCollateBeforeEditionTestsFabricDW.sql", nErrors80: 3, nErrors90: 3, nErrors100: 3, nErrors110: 3, nErrors120: 0, nErrors130: 0, nErrors140: 0, nErrors150: 0, nErrors160: 0, nErrors170: 0, nErrors180: 0),
            new ParserTestFabricDW("ModernGroupByAllTestsFabricDW.sql", nErrors80: 9, nErrors90: 9, nErrors100: 9, nErrors110: 9, nErrors120: 9, nErrors130: 9, nErrors140: 9, nErrors150: 9, nErrors160: 9, nErrors170: 9, nErrors180: 9),
            new ParserTestFabricDW("OrderByAllTestsFabricDW.sql", nErrors80: 13, nErrors90: 13, nErrors100: 13, nErrors110: 13, nErrors120: 13, nErrors130: 13, nErrors140: 13, nErrors150: 13, nErrors160: 13, nErrors170: 13, nErrors180: 13),
            new ParserTestFabricDW("QueryHintsTestsFabricDW.sql", nErrors80: 6, nErrors90: 6, nErrors100: 6, nErrors110: 6, nErrors120: 6, nErrors130: 6, nErrors140: 6, nErrors150: 6, nErrors160: 6, nErrors170: 3, nErrors180: 3),
            new ParserTestFabricDW("WindowClauseQuotedNameTests160.sql", nErrors80: 7, nErrors90: 7, nErrors100: 7, nErrors110: 7, nErrors120: 7, nErrors130: 7, nErrors140: 7, nErrors150: 7, nErrors160: 0, nErrors170: 0, nErrors180: 0),
            new ParserTestFabricDW("AlterAvailabilityGroupBackupPriorityTests.sql", nErrors80: 6, nErrors90: 3, nErrors100: 3, nErrors110: 0, nErrors120: 0, nErrors130: 0, nErrors140: 0, nErrors150: 0, nErrors160: 0, nErrors170: 0, nErrors180: 0),
            new ParserTestFabricDW("AlterAvailabilityGroupCreateAnyDatabaseTests130.sql", nErrors80: 4, nErrors90: 3, nErrors100: 3, nErrors110: 3, nErrors120: 3, nErrors130: 0, nErrors140: 0, nErrors150: 0, nErrors160: 0, nErrors170: 0, nErrors180: 0),
        };

        [DataTestMethod]
        [DataRow("GROUP BY ALL")]
        [DataRow("GROUP BY ALL City")]
        [DataRow("GROUP BY City")]
        [DataRow("ORDER BY ALL")]
        [DataRow("ORDER BY ALL ASC")]
        [DataRow("ORDER BY ALL DESC")]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void FabricDWGroupingAndOrderingTokenRanges(string clauseText)
        {
            string selectText = "SELECT TOP 100 City, COUNT(*) AS Cnt FROM dbo.Employees " + clauseText;
            string sql = "CREATE VIEW dbo.TestView AS " + selectText + ";";
            var parser = new TSqlFabricDWParser(true);
            using (var reader = new StringReader(sql))
            {
                var script = (TSqlScript)parser.Parse(reader, out var errors);
                Assert.AreEqual(0, errors.Count);
                var view = (CreateViewStatement)script.Batches[0].Statements[0];
                var query = (QuerySpecification)view.SelectStatement.QueryExpression;
                TSqlFragment clause = query.GroupByClause != null
                    ? (TSqlFragment)query.GroupByClause
                    : query.OrderByClause;

                Assert.AreEqual(clauseText, sql.Substring(clause.StartOffset, clause.FragmentLength));
                Assert.AreEqual(selectText, sql.Substring(query.StartOffset, query.FragmentLength));
                Assert.AreEqual(selectText, sql.Substring(view.SelectStatement.StartOffset, view.SelectStatement.FragmentLength));
                Assert.AreEqual(clause.LastTokenIndex, query.LastTokenIndex);
                Assert.AreEqual(clause.LastTokenIndex, view.SelectStatement.LastTokenIndex);
            }
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void AiTranslateOnErrorClauseAstShape()
        {
            const string sql =
                "SELECT AI_TRANSLATE('text', 'es' ERROR ON ERROR);" +
                "SELECT AI_TRANSLATE('text', 'es' NULL ON ERROR);" +
                "SELECT AI_TRANSLATE('text', 'es' DEFAULT 'fallback' ON ERROR);";

            var parser = new TSqlFabricDWParser(true);
            var script = (TSqlScript)parser.Parse(new StringReader(sql), out var errors);

            Assert.AreEqual(0, errors.Count);

            var errorFunction = GetAiTranslateFunction(script, 0);
            Assert.IsInstanceOfType(errorFunction.OnErrorClause, typeof(AIErrorOnErrorClause));

            var nullFunction = GetAiTranslateFunction(script, 1);
            Assert.IsInstanceOfType(nullFunction.OnErrorClause, typeof(AINullOnErrorClause));

            var defaultFunction = GetAiTranslateFunction(script, 2);
            Assert.IsInstanceOfType(defaultFunction.OnErrorClause, typeof(AIDefaultOnErrorClause));
            Assert.AreEqual(
                "fallback",
                ((AIDefaultOnErrorClause)defaultFunction.OnErrorClause).Value.Value);
        }

        private static AITranslateFunctionCall GetAiTranslateFunction(TSqlScript script, int statementIndex)
        {
            var selectStatement = (SelectStatement)script.Batches[0].Statements[statementIndex];
            var query = (QuerySpecification)selectStatement.QueryExpression;
            return (AITranslateFunctionCall)((SelectScalarExpression)query.SelectElements[0]).Expression;
        }

        [DataTestMethod]
        [DataRow("AI_ANALYZE_SENTIMENT('text')")]
        [DataRow("AI_FIX_GRAMMAR('text' ERROR ON ERROR)")]
        [DataRow("AI_GENERATE_RESPONSE('prompt', 'data' NULL ON ERROR)")]
        [DataRow("AI_SUMMARIZE('text' DEFAULT 'fallback' ON ERROR)")]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void FabricDWAiFunctionTokenRanges(string functionText)
        {
            string sql = "SELECT " + functionText;
            var parser = new TSqlFabricDWParser(true);
            using (var reader = new StringReader(sql))
            {
                var script = (TSqlScript)parser.Parse(reader, out var errors);
                Assert.AreEqual(0, errors.Count);
                var select = (SelectStatement)script.Batches[0].Statements[0];
                var function = ((SelectScalarExpression)((QuerySpecification)select.QueryExpression).SelectElements[0]).Expression;

                Assert.AreEqual(functionText, sql.Substring(function.StartOffset, function.FragmentLength));
                Assert.AreEqual(sql, sql.Substring(select.StartOffset, select.FragmentLength));
                Assert.AreEqual(function.LastTokenIndex, select.LastTokenIndex);
            }
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void AiDefaultOnErrorClauseRequiresValue()
        {
            var function = new AISummarizeFunctionCall
            {
                Input = new StringLiteral { Value = "text" },
                OnErrorClause = new AIDefaultOnErrorClause(),
            };

            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.SqlFabricDW);
            Assert.ThrowsException<InvalidOperationException>(() => scriptGen.GenerateScript(function, out string _));
        }

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