//------------------------------------------------------------------------------
// <copyright file="DeepNestingTests.cs" company="Microsoft">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlStudio.Tests.AssemblyTools.TestCategory;

namespace SqlStudio.Tests.UTSqlScriptDom
{
    /// <summary>
    /// A deeply nested statement must stop the parser, the script generator and the fragment visitors
    /// with InsufficientExecutionStackException. Without the stack checks it overflows the stack, and a
    /// stack overflow ends the process. Every case runs on a thread with a fixed 1 MB stack, and every
    /// deep case below overflows that stack when the checks are missing.
    /// </summary>
    public partial class SqlDomTests
    {
        private const int SmallStackSize = 1024 * 1024;

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void DeeplyNestedParenthesesThrowInsufficientExecutionStack()
        {
            string script = "SELECT " + NestedText("(", "1", ")", 10000);

            ParserTestUtils.ExecuteTestForAllParsers(parser =>
            {
                AssertThrowsInsufficientExecutionStack(() => ParseWithoutErrors(parser, script));
            }, true);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void DeeplyNestedScalarSubqueriesThrowInsufficientExecutionStack()
        {
            string script = "SELECT " + NestedText("(SELECT ", "1", ")", 5000);

            AssertThrowsInsufficientExecutionStack(() => ParseWithoutErrors(new TSql170Parser(true), script));
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void DeeplyNestedFunctionCallsThrowInsufficientExecutionStack()
        {
            string script = "SELECT " + NestedText("ABS(", "1", ")", 10000);

            AssertThrowsInsufficientExecutionStack(() => ParseWithoutErrors(new TSql170Parser(true), script));
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void LongBinaryExpressionChainThrowsInsufficientExecutionStack()
        {
            string script = "SELECT 1" + RepeatedText(" + 1", 100000);

            AssertThrowsInsufficientExecutionStack(() => ParseWithoutErrors(new TSql170Parser(true), script));
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void LongUnionAllChainThrowsInsufficientExecutionStackInScriptGenerator()
        {
            SelectStatement statement = BuildUnionAllChain(100000);

            AssertThrowsInsufficientExecutionStack(() =>
            {
                string script;
                new Sql170ScriptGenerator().GenerateScript(statement, out script);
            });
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void LongUnionAllChainThrowsInsufficientExecutionStackInVisitor()
        {
            SelectStatement statement = BuildUnionAllChain(100000);

            AssertThrowsInsufficientExecutionStack(() => statement.Accept(new GenericFragmentVisitor<QuerySpecification>(query => true)));
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void OrdinaryAndModeratelyNestedStatementsParseOnSmallStack()
        {
            string[] scripts =
            {
                "SELECT a.id, b.name FROM dbo.a AS a INNER JOIN dbo.b AS b ON a.id = b.id WHERE a.x > 1 ORDER BY b.name;",
                "WITH c AS (SELECT id FROM dbo.t) SELECT id FROM c WHERE id IN (SELECT id FROM dbo.u) GROUP BY id HAVING COUNT(*) > 1;",
                "CREATE PROCEDURE dbo.p @x INT AS BEGIN IF @x > 0 BEGIN UPDATE dbo.t SET y = y + 1 WHERE id = @x; END ELSE SELECT 0; END",
                "SELECT " + NestedText("(", "1", ")", 64),
                // SQL Server documents 32 as the nesting limit for subqueries.
                "SELECT " + NestedText("(SELECT ", "1", ")", 32),
                "SELECT " + NestedText("ABS(", "1", ")", 40),
                "SELECT 1" + RepeatedText(" + 1", 500),
                "SELECT 1" + RepeatedText(" UNION ALL SELECT 1", 400),
            };

            foreach (string script in scripts)
            {
                Exception exception = RunOnThreadWithStack(SmallStackSize, () =>
                {
                    TSqlFragment fragment = ParseWithoutErrors(new TSql170Parser(true), script);

                    string generated;
                    new Sql170ScriptGenerator().GenerateScript(fragment, out generated);
                    ParseWithoutErrors(new TSql170Parser(true), generated);

                    fragment.Accept(new GenericFragmentVisitor<TSqlFragment>(node => true));
                });

                Assert.IsNull(exception, "Failed on a statement of " + script.Length + " characters: " + exception);
            }
        }

        private static string NestedText(string open, string inner, string close, int depth)
        {
            return RepeatedText(open, depth) + inner + RepeatedText(close, depth);
        }

        private static string RepeatedText(string text, int count)
        {
            StringBuilder builder = new StringBuilder(text.Length * count);
            for (int i = 0; i < count; i++)
            {
                builder.Append(text);
            }
            return builder.ToString();
        }

        private static TSqlFragment ParseWithoutErrors(TSqlParser parser, string script)
        {
            IList<ParseError> errors;
            TSqlFragment fragment = parser.Parse(new StringReader(script), out errors);
            Assert.AreEqual(0, errors.Count, errors.Count == 0 ? string.Empty : errors[0].Message);
            return fragment;
        }

        // A left-deep chain of UNION ALL query expressions, the shape the parser builds for
        // "SELECT 1 UNION ALL SELECT 1 ...". Building it directly keeps the parse out of these cases.
        private static SelectStatement BuildUnionAllChain(int count)
        {
            QueryExpression query = BuildSelectOne();
            for (int i = 0; i < count; i++)
            {
                query = new BinaryQueryExpression
                {
                    BinaryQueryExpressionType = BinaryQueryExpressionType.Union,
                    All = true,
                    FirstQueryExpression = query,
                    SecondQueryExpression = BuildSelectOne()
                };
            }
            return new SelectStatement { QueryExpression = query };
        }

        private static QuerySpecification BuildSelectOne()
        {
            QuerySpecification query = new QuerySpecification();
            query.SelectElements.Add(new SelectScalarExpression { Expression = new IntegerLiteral { Value = "1" } });
            return query;
        }

        // Runs the action on a new thread with the given stack size and returns what it threw, so that
        // the assertion runs on the test thread.
        private static Exception RunOnThreadWithStack(int maxStackSize, Action action)
        {
            Exception caught = null;
            Thread thread = new Thread(() =>
            {
                try
                {
                    action();
                }
                catch (Exception exception)
                {
                    caught = exception;
                }
            }, maxStackSize);
            thread.Start();
            thread.Join();
            return caught;
        }

        private static void AssertThrowsInsufficientExecutionStack(Action action)
        {
            Exception exception = RunOnThreadWithStack(SmallStackSize, action);
            Assert.IsNotNull(exception, "Expected InsufficientExecutionStackException, but the call completed.");
            Assert.IsInstanceOfType(exception, typeof(InsufficientExecutionStackException), exception.GetType().FullName + ": " + exception.Message);
        }
    }
}
