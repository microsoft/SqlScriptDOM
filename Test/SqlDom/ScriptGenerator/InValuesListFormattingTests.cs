//------------------------------------------------------------------------------
// <copyright file="InValuesListFormattingTests.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using Microsoft.SqlServer.TransactSql.ScriptDom;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlStudio.Tests.AssemblyTools.TestCategory;
using static SqlStudio.Tests.UTSqlScriptDom.ScriptGeneratorTestHelper;

namespace SqlStudio.Tests.UTSqlScriptDom
{
    // Tests for the MultilineInValuesList script-generation option, which controls whether the
    // values in an IN (values) predicate are written on a single line (default) or on multiple
    // lines. Kept in a dedicated file to avoid churn in ScriptGeneratorTests.cs.
    //
    // Work item: Formatter option: IN (values) list width
    [TestClass]
    public class InValuesListFormattingTests
    {
        // Builds options that isolate the IN-list layout: clause bodies are not aligned and clauses
        // are not broken onto their own lines, so the surrounding statement stays on one line and the
        // expectations focus on the IN (values) list itself.
        private static SqlScriptGeneratorOptions MakeOptions(bool multilineInValuesList)
        {
            return new SqlScriptGeneratorOptions
            {
                MultilineInValuesList = multilineInValuesList,
                AlignClauseBodies = false,
                NewLineBeforeFromClause = false,
                NewLineBeforeWhereClause = false,
                MultilineSelectElementsList = false,
                MultilineWherePredicatesList = false,
            };
        }

        // -----------------------------------------------------------------------------------------
        // Default
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineInValuesListDefaultIsFalse()
        {
            Assert.IsFalse(new SqlScriptGeneratorOptions().MultilineInValuesList);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestDefaultKeepsInListOnSingleLine()
        {
            // With the option at its default (false) the IN list stays on a single line.
            const string input = "SELECT * FROM t WHERE x IN (1, 2, 3);";
            var options = MakeOptions(false);
            const string expected = @"SELECT * FROM t WHERE x IN (1, 2, 3);";

            AssertGenerated(input, options, expected);
        }

        // -----------------------------------------------------------------------------------------
        // Multi-line layout
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineTrailingCommaPutsEachValueOnItsOwnLine()
        {
            // With the option enabled and the default (trailing) comma placement, each value is
            // written on its own line with a trailing comma. The list is aligned under the WHERE
            // clause body, with each value indented one level (4) past the closing parenthesis.
            const string input = "SELECT * FROM t WHERE x IN (1, 2, 3);";
            var options = MakeOptions(true);
            const string expected = @"
SELECT * FROM t WHERE x IN (
                    1,
                    2,
                    3
                );";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineLeadingCommaPlacement()
        {
            // CommaPlacement = Leading applies within the IN list: values sit at the list
            // indentation level and each comma is indented two characters fewer.
            const string input = "SELECT * FROM t WHERE x IN (1, 2, 3);";
            var options = MakeOptions(true);
            options.CommaPlacement = CommaPlacement.Leading;
            const string expected = @"
SELECT * FROM t WHERE x IN (
                    1
                  , 2
                  , 3
                );";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineNewLineBeforeOpenParenthesis()
        {
            // NewLineBeforeOpenParenthesisInMultilineList moves the open parenthesis onto its own line.
            const string input = "SELECT * FROM t WHERE x IN (1, 2, 3);";
            var options = MakeOptions(true);
            options.NewLineBeforeOpenParenthesisInMultilineList = true;
            const string expected = @"
SELECT * FROM t WHERE x IN
                (
                    1,
                    2,
                    3
                );";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineNoNewLineBeforeCloseParenthesis()
        {
            // NewLineBeforeCloseParenthesisInMultilineList = false keeps the close parenthesis on the
            // same line as the last value.
            const string input = "SELECT * FROM t WHERE x IN (1, 2, 3);";
            var options = MakeOptions(true);
            options.NewLineBeforeCloseParenthesisInMultilineList = false;
            const string expected = @"
SELECT * FROM t WHERE x IN (
                    1,
                    2,
                    3);";

            AssertGenerated(input, options, expected);
        }

        // -----------------------------------------------------------------------------------------
        // Variants and edge cases
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineAppliesToNotIn()
        {
            // The option also governs a negated (NOT IN) list.
            const string input = "SELECT * FROM t WHERE x NOT IN (1, 2, 3);";
            var options = MakeOptions(true);
            const string expected = @"
SELECT * FROM t WHERE x NOT IN (
                    1,
                    2,
                    3
                );";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineSingleValueStillWraps()
        {
            // A single-value IN list wraps too (consistent with the parenthesized-list behavior used
            // for column lists).
            const string input = "SELECT * FROM t WHERE x IN (1);";
            var options = MakeOptions(true);
            const string expected = @"
SELECT * FROM t WHERE x IN (
                    1
                );";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineDoesNotAffectSubqueryInPredicate()
        {
            // An IN (subquery) predicate has no value list, so the option must not change it.
            const string input = "SELECT * FROM t WHERE x IN (SELECT id FROM u);";
            var options = MakeOptions(true);
            const string expected = @"SELECT * FROM t WHERE x IN (SELECT id FROM u);";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineWrapsStringAndExpressionValues()
        {
            // Values can be arbitrary scalar expressions (string literals, columns), not just integers.
            const string input = "SELECT * FROM t WHERE x IN ('a', 'b', c);";
            var options = MakeOptions(true);
            const string expected = @"
SELECT * FROM t WHERE x IN (
                    'a',
                    'b',
                    c
                );";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineAppliesInDeleteWhereClause()
        {
            // The option applies wherever an IN (values) predicate appears, not only in SELECT. The
            // list aligns under this statement's (shorter) WHERE clause body.
            const string input = "DELETE FROM t WHERE x IN (1, 2, 3);";
            var options = MakeOptions(true);
            const string expected = @"
DELETE t WHERE x IN (
             1,
             2,
             3
         );";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineOnlyAffectsInListWhenOtherListsAreDefault()
        {
            // Isolation: enabling MultilineInValuesList on top of otherwise-default options wraps the
            // IN list, while every other list keeps its own default behavior. Here the SELECT column
            // list still wraps because MultilineSelectElementsList defaults to true, independently of
            // this option.
            const string input = "SELECT a, b FROM t WHERE x IN (1, 2, 3);";
            var options = new SqlScriptGeneratorOptions { MultilineInValuesList = true };
            const string expected = @"
SELECT a,
       b
FROM   t
WHERE  x IN (
    1,
    2,
    3
);";

            AssertGenerated(input, options, expected);
        }

        // -----------------------------------------------------------------------------------------
        // Indentation and multiple contexts
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineHonorsCustomIndentationSize()
        {
            // Teams that use a 2-space indent should see the wrapped IN values indented by that
            // amount (IndentationSize) past the list's alignment level, not the default 4.
            const string input = "SELECT * FROM t WHERE x IN (1, 2, 3);";
            var options = MakeOptions(true);
            options.IndentationSize = 2;
            const string expected = @"
SELECT * FROM t WHERE x IN (
                  1,
                  2,
                  3
                );";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineHonorsTabIndentation()
        {
            // With IndentationMode = Tabs the wrapped IN values are indented with tab characters.
            const string input = "SELECT * FROM t WHERE x IN (1, 2, 3);";
            var options = MakeOptions(true);
            options.IndentationMode = IndentationMode.Tabs;
            // The wrapped values are indented with tab characters (the literal tabs in the lines below).
            const string expected = @"
SELECT * FROM t WHERE x IN (
					1,
					2,
					3
				);";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineAppliesInCheckConstraint()
        {
            // The option applies to DDL too: a CHECK constraint whose predicate is an IN (values)
            // list wraps the values just like a DML WHERE clause.
            const string input = "ALTER TABLE t ADD CONSTRAINT ck CHECK (x IN (1, 2, 3));";
            var options = MakeOptions(true);
            const string expected = @"
ALTER TABLE t
    ADD CONSTRAINT ck CHECK (x IN (
            1,
            2,
            3
        ));";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineWrapsMultipleInListsIndependently()
        {
            // A realistic query filtering on two IN lists joined by AND: each list wraps on its own,
            // and the surrounding predicate layout is preserved.
            const string input = "SELECT * FROM t WHERE x IN (1, 2, 3) AND y IN (4, 5, 6);";
            var options = MakeOptions(true);
            const string expected = @"
SELECT * FROM t WHERE x IN (
                          1,
                          2,
                          3
                      ) AND y IN (
                          4,
                          5,
                          6
                      );";

            AssertGenerated(input, options, expected);
        }
    }
}
