//------------------------------------------------------------------------------
// <copyright file="JoinClauseFormattingTests.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using Microsoft.SqlServer.TransactSql.ScriptDom;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlStudio.Tests.AssemblyTools.TestCategory;
using static SqlStudio.Tests.UTSqlScriptDom.ScriptGeneratorTestHelper;

namespace SqlStudio.Tests.UTSqlScriptDom
{
    // Tests for the NewLineAfterJoinKeyword and NewLineBeforeOnClause script-generation options that
    // control how a QualifiedJoin (INNER/LEFT/RIGHT/FULL OUTER JOIN ... ON ...) is formatted.
    [TestClass]
    public class JoinClauseFormattingTests
    {
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestNewLineAfterJoinKeywordDefaultIsTrue()
        {
            Assert.AreEqual(true, new SqlScriptGeneratorOptions().NewLineAfterJoinKeyword);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestNewLineBeforeOnClauseDefaultIsTrue()
        {
            Assert.AreEqual(true, new SqlScriptGeneratorOptions().NewLineBeforeOnClause);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestDefaultsReproduceOriginalJoinLayout()
        {
            // Defaults (NewLineAfterJoinKeyword = true, NewLineBeforeOnClause = true) must reproduce
            // the original formatter output exactly: table source on its own line after the JOIN
            // keyword, and ON on a new line aligned with the JOIN keyword (no extra indentation).
            const string input = "SELECT * FROM a INNER JOIN b ON a.x = b.x;";
            var options = new SqlScriptGeneratorOptions();
            const string expected =
@"
SELECT *
FROM   a
       INNER JOIN
       b
       ON a.x = b.x;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMostRequestedStyleTableSourceOnJoinLineOnNewLine()
        {
            // Most-requested style: table source kept on the JOIN line, ON on its own line indented
            // one level from the JOIN keyword.
            const string input = "SELECT * FROM a INNER JOIN b ON a.x = b.x;";
            var options = new SqlScriptGeneratorOptions
            {
                NewLineAfterJoinKeyword = false,
                NewLineBeforeOnClause = true
            };
            const string expected =
@"
SELECT *
FROM   a
       INNER JOIN b
           ON a.x = b.x;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestBothFalseSingleLineJoin()
        {
            // Both options false: JOIN, table source and ON all stay on a single line.
            const string input = "SELECT * FROM a INNER JOIN b ON a.x = b.x;";
            var options = new SqlScriptGeneratorOptions
            {
                NewLineAfterJoinKeyword = false,
                NewLineBeforeOnClause = false
            };
            const string expected =
@"
SELECT *
FROM   a
       INNER JOIN b ON a.x = b.x;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestTableSourceOnOwnLineOnKeptOnSameLine()
        {
            // Newline after JOIN keyword but ON kept on the same line as the table source.
            const string input = "SELECT * FROM a INNER JOIN b ON a.x = b.x;";
            var options = new SqlScriptGeneratorOptions
            {
                NewLineAfterJoinKeyword = true,
                NewLineBeforeOnClause = false
            };
            const string expected =
@"
SELECT *
FROM   a
       INNER JOIN
       b ON a.x = b.x;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultiPredicateOnWrapsWithMultilineWherePredicatesList()
        {
            // A multi-predicate ON wraps the AND predicate onto its own line, aligned under the
            // first predicate, when MultilineWherePredicatesList is enabled (the same wrapping used
            // for WHERE predicates).
            const string input = "SELECT * FROM a INNER JOIN b ON a.x = b.x AND a.y = b.y;";
            var options = new SqlScriptGeneratorOptions
            {
                NewLineAfterJoinKeyword = false,
                NewLineBeforeOnClause = true,
                MultilineWherePredicatesList = true
            };
            const string expected =
@"
SELECT *
FROM   a
       INNER JOIN b
           ON a.x = b.x
              AND a.y = b.y;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultiPredicateOnStaysInlineWhenMultilineDisabled()
        {
            // With MultilineWherePredicatesList disabled, a multi-predicate ON stays on a single line.
            const string input = "SELECT * FROM a INNER JOIN b ON a.x = b.x AND a.y = b.y;";
            var options = new SqlScriptGeneratorOptions
            {
                NewLineAfterJoinKeyword = false,
                NewLineBeforeOnClause = true,
                MultilineWherePredicatesList = false
            };
            const string expected =
@"
SELECT *
FROM   a
       INNER JOIN b
           ON a.x = b.x AND a.y = b.y;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestChainedJoinsEachOnIndentedOneLevel()
        {
            // Chained joins: each ON is indented one level from its JOIN, independently.
            const string input = "SELECT * FROM a INNER JOIN b ON a.x = b.x INNER JOIN c ON b.y = c.y;";
            var options = new SqlScriptGeneratorOptions
            {
                NewLineAfterJoinKeyword = false,
                NewLineBeforeOnClause = true
            };
            const string expected =
@"
SELECT *
FROM   a
       INNER JOIN b
           ON a.x = b.x
       INNER JOIN c
           ON b.y = c.y;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCrossJoinUnaffectedByOnClauseOptions()
        {
            // CROSS JOIN has no ON clause (it is an UnqualifiedJoin), so the ON-related options
            // (NewLineAfterJoinKeyword, NewLineBeforeOnClause) do not change its formatting. The
            // leading newline comes from NewLineBeforeJoinClause, which defaults to true.
            const string input = "SELECT * FROM a CROSS JOIN b;";
            var options = new SqlScriptGeneratorOptions
            {
                NewLineAfterJoinKeyword = false,
                NewLineBeforeOnClause = true
            };
            const string expected =
@"
SELECT *
FROM   a
       CROSS JOIN b;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCrossApplyUnaffectedByOnClauseOptions()
        {
            // CROSS APPLY has no ON clause either, so the ON-related options leave it unchanged. The
            // leading newline comes from NewLineBeforeJoinClause, which defaults to true.
            const string input = "SELECT * FROM a CROSS APPLY dbo.f(a.id) AS b;";
            var options = new SqlScriptGeneratorOptions
            {
                NewLineAfterJoinKeyword = false,
                NewLineBeforeOnClause = true
            };
            const string expected =
@"
SELECT *
FROM   a
       CROSS APPLY dbo.f(a.id) AS b;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCrossApplyNewLineBeforeJoinClauseDefaultTrue()
        {
            // NewLineBeforeJoinClause (default true) applies to CROSS APPLY just like a qualified
            // JOIN: the operator goes on its own line under the first table source.
            const string input = "SELECT * FROM a CROSS APPLY dbo.f(a.id) AS b;";
            var options = new SqlScriptGeneratorOptions();
            const string expected =
@"
SELECT *
FROM   a
       CROSS APPLY dbo.f(a.id) AS b;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCrossApplyInlineWhenNewLineBeforeJoinClauseFalse()
        {
            // When NewLineBeforeJoinClause is false, CROSS APPLY stays on the same line as the first
            // table source.
            const string input = "SELECT * FROM a CROSS APPLY dbo.f(a.id) AS b;";
            var options = new SqlScriptGeneratorOptions
            {
                NewLineBeforeJoinClause = false
            };
            const string expected =
@"
SELECT *
FROM   a CROSS APPLY dbo.f(a.id) AS b;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestOuterApplyInlineWhenNewLineBeforeJoinClauseFalse()
        {
            // When NewLineBeforeJoinClause is false, OUTER APPLY stays on the same line as the first
            // table source.
            const string input = "SELECT * FROM a OUTER APPLY dbo.f(a.id) AS b;";
            var options = new SqlScriptGeneratorOptions
            {
                NewLineBeforeJoinClause = false
            };
            const string expected =
@"
SELECT *
FROM   a OUTER APPLY dbo.f(a.id) AS b;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestOuterApplyNewLineBeforeJoinClauseDefaultTrue()
        {
            // NewLineBeforeJoinClause (default true) applies to OUTER APPLY just like a qualified
            // JOIN: the operator goes on its own line under the first table source.
            const string input = "SELECT * FROM a OUTER APPLY dbo.f(a.id) AS b;";
            var options = new SqlScriptGeneratorOptions();
            const string expected =
@"
SELECT *
FROM   a
       OUTER APPLY dbo.f(a.id) AS b;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCrossJoinInlineWhenNewLineBeforeJoinClauseFalse()
        {
            // When NewLineBeforeJoinClause is false, CROSS JOIN stays on the same line as the first
            // table source.
            const string input = "SELECT * FROM a CROSS JOIN b;";
            var options = new SqlScriptGeneratorOptions
            {
                NewLineBeforeJoinClause = false
            };
            const string expected =
@"
SELECT *
FROM   a CROSS JOIN b;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCrossJoinNewLineBeforeJoinClauseDefaultTrue()
        {
            // NewLineBeforeJoinClause (default true) applies to CROSS JOIN just like a qualified
            // JOIN: the operator goes on its own line under the first table source.
            const string input = "SELECT * FROM a CROSS JOIN b;";
            var options = new SqlScriptGeneratorOptions();
            const string expected =
@"
SELECT *
FROM   a
       CROSS JOIN b;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestJoinInlineWithFirstTableSourceWhenNewLineBeforeJoinClauseFalse()
        {
            // When NewLineBeforeJoinClause is false, the JOIN keyword stays on the same line as the
            // first table source; the new options still control the table source and ON placement.
            const string input = "SELECT * FROM a INNER JOIN b ON a.x = b.x;";
            var options = new SqlScriptGeneratorOptions
            {
                NewLineBeforeJoinClause = false,
                NewLineAfterJoinKeyword = false,
                NewLineBeforeOnClause = true
            };
            const string expected =
@"
SELECT *
FROM   a INNER JOIN b
           ON a.x = b.x;";

            AssertGenerated(input, options, expected);
        }
    }
}
