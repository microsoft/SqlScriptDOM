using System;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlStudio.Tests.AssemblyTools.TestCategory;

namespace SqlStudio.Tests.UTSqlScriptDom
{
    /// <summary>
    /// Tests the Set statement fragments
    /// </summary>
    public partial class SqlDomTests
    {
        private const string Statistics = "STATISTICS";
        private static readonly string QuotedIdentifierStatement = EstablishTestStatement(CodeGenerationSupporter.QuotedIdentifier, true);
        private static readonly string ConcatNullYieldsNullStatement = EstablishTestStatement(CodeGenerationSupporter.ConcatNullYieldsNull, false);
        private static readonly string CursorCloseOnCommitStatement = EstablishTestStatement(CodeGenerationSupporter.CursorCloseOnCommit, false);
        private static readonly string ArithAbortStatement = EstablishTestStatement(CodeGenerationSupporter.ArithAbort, true);
        private static readonly string ArithIgnoreStatement = EstablishTestStatement(CodeGenerationSupporter.ArithIgnore, false);
        private static readonly string FmtOnlyStatement = EstablishTestStatement(CodeGenerationSupporter.FmtOnly, true);
        private static readonly string NoCountStatement = EstablishTestStatement(CodeGenerationSupporter.NoCount, false);
        private static readonly string NoExecStatement = EstablishTestStatement(CodeGenerationSupporter.NoExec, true);
        private static readonly string NumericRoundAbortStatement = EstablishTestStatement(CodeGenerationSupporter.NumericRoundAbort, false);
        private static readonly string ParseOnlyStatement = EstablishTestStatement(CodeGenerationSupporter.ParseOnly, true);
        private static readonly string AnsiDefaultsStatement = EstablishTestStatement(CodeGenerationSupporter.AnsiDefaults, true);
        private static readonly string AnsiNullDfltOffStatement = EstablishTestStatement(CodeGenerationSupporter.AnsiNullDfltOff, false);
        private static readonly string AnsiNullDfltOnStatement = EstablishTestStatement(CodeGenerationSupporter.AnsiNullDfltOn, true);
        private static readonly string AnsiNullsStatement = EstablishTestStatement(CodeGenerationSupporter.AnsiNulls, false);
        private static readonly string AnsiPaddingStatement = EstablishTestStatement(CodeGenerationSupporter.AnsiPadding, true);
        private static readonly string AnsiWarningsStatement = EstablishTestStatement(CodeGenerationSupporter.AnsiWarnings, true);
        private static readonly string ForcePlanStatement = EstablishTestStatement(CodeGenerationSupporter.ForcePlan, false);
        private static readonly string ShowPlanAllStatement = EstablishTestStatement(CodeGenerationSupporter.ShowPlanAll, true);
        private static readonly string ShowPlanTextStatement = EstablishTestStatement(CodeGenerationSupporter.ShowPlanText, false);
        private static readonly string ShowPlanXmlStatement = EstablishTestStatement(CodeGenerationSupporter.ShowPlanXml, false);
        private static readonly string StatisticsIOStatement = EstablishTestStatement(Statistics + " " + CodeGenerationSupporter.IO, true);
        private static readonly string StatisticsProfileStatement = EstablishTestStatement(Statistics + " " + CodeGenerationSupporter.Profile, false);
        private static readonly string StatisticsTimeStatement = EstablishTestStatement(Statistics + " " + CodeGenerationSupporter.Time, true);
        private static readonly string ImplicitTransactionsStatement = EstablishTestStatement(CodeGenerationSupporter.ImplicitTransactions, false);
        private static readonly string RemoteProcTransactionsStatement = EstablishTestStatement(CodeGenerationSupporter.RemoteProcTransactions, true);
        private static readonly string XactAbortStatement = EstablishTestStatement(CodeGenerationSupporter.XactAbort, false);

        private static string EstablishTestStatement(string option, bool isOn)
        {
            return String.Format("{0} {1} {2}", "SET", option, isOn ? "ON" : "OFF");
        }

        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]
        public void TestPredicateStates()
        {
            ParserTestUtils.ExecuteTestForAllParsers(delegate(TSqlParser parser)
            {
                AssertPredicateState(parser, QuotedIdentifierStatement, SetOptions.QuotedIdentifier, true);

                AssertPredicateState(parser, ConcatNullYieldsNullStatement, SetOptions.ConcatNullYieldsNull, false);

                AssertPredicateState(parser, CursorCloseOnCommitStatement, SetOptions.CursorCloseOnCommit, false);

                AssertPredicateState(parser, ArithAbortStatement, SetOptions.ArithAbort, true);

                AssertPredicateState(parser, ArithIgnoreStatement, SetOptions.ArithIgnore, false);

                AssertPredicateState(parser, FmtOnlyStatement, SetOptions.FmtOnly, true);

                AssertPredicateState(parser, NoCountStatement, SetOptions.NoCount, false);

                AssertPredicateState(parser, NoExecStatement, SetOptions.NoExec, true);

                AssertPredicateState(parser, NumericRoundAbortStatement, SetOptions.NumericRoundAbort, false);

                AssertPredicateState(parser, ParseOnlyStatement, SetOptions.ParseOnly, true);

                AssertPredicateState(parser, AnsiDefaultsStatement, SetOptions.AnsiDefaults, true);

                AssertPredicateState(parser, AnsiNullDfltOffStatement, SetOptions.AnsiNullDfltOff, false);

                AssertPredicateState(parser, AnsiNullDfltOnStatement, SetOptions.AnsiNullDfltOn, true);

                AssertPredicateState(parser, AnsiNullsStatement, SetOptions.AnsiNulls, false);

                AssertPredicateState(parser, AnsiPaddingStatement, SetOptions.AnsiPadding, true);

                AssertPredicateState(parser, AnsiWarningsStatement, SetOptions.AnsiWarnings, true);

                AssertPredicateState(parser, ForcePlanStatement, SetOptions.ForcePlan, false);

                AssertPredicateState(parser, ShowPlanAllStatement, SetOptions.ShowPlanAll, true);

                AssertPredicateState(parser, ShowPlanTextStatement, SetOptions.ShowPlanText, false);

                AssertPredicateState(parser, ShowPlanXmlStatement, SetOptions.ShowPlanXml, false);

                AssertSetStatementState(parser, StatisticsIOStatement, SetStatisticsOptions.IO, true);

                AssertSetStatementState(parser, StatisticsProfileStatement, SetStatisticsOptions.Profile, false);

                AssertSetStatementState(parser, StatisticsTimeStatement, SetStatisticsOptions.Time, true);

                AssertPredicateState(parser, ImplicitTransactionsStatement, SetOptions.ImplicitTransactions, false);

                AssertPredicateState(parser, XactAbortStatement, SetOptions.XactAbort, false);
            }, true);
        }

        private static PredicateSetStatement AssertPredicateState(TSqlParser parser, string strStatement, SetOptions option, bool isOn)
        {
            TSqlScript script = (TSqlScript)ParserTestUtils.ParseStringNoErrors(parser, strStatement);

            TSqlBatch batch = script.Batches[0];
            Assert.IsTrue(batch.Statements.Count == 1);
            Assert.IsTrue(batch.Statements[0] is PredicateSetStatement);

            PredicateSetStatement statement = batch.Statements[0] as PredicateSetStatement;
            Assert.IsTrue(statement.Options == option);
            Assert.IsTrue(isOn == true ? statement.IsOn : statement.IsOn == false);
            return statement;
        }

        private static SetStatisticsStatement AssertSetStatementState(TSqlParser parser, string strStatement, SetStatisticsOptions option, bool isOn)
        {
            TSqlScript script = (TSqlScript)ParserTestUtils.ParseStringNoErrors(parser, strStatement);

            TSqlBatch batch = script.Batches[0];
            Assert.IsTrue(batch.Statements.Count == 1);
            Assert.IsTrue(batch.Statements[0] is SetStatisticsStatement);

            SetStatisticsStatement statement = batch.Statements[0] as SetStatisticsStatement;
            Assert.IsTrue(statement.Options == option);
            Assert.IsTrue(isOn == true ? statement.IsOn : statement.IsOn == false);
            return statement;
        }
    }
}
