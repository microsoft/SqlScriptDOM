//------------------------------------------------------------------------------
// <copyright file="PartitionByElementsListFormattingTests.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using Microsoft.SqlServer.TransactSql.ScriptDom;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlStudio.Tests.AssemblyTools.TestCategory;
using static SqlStudio.Tests.UTSqlScriptDom.ScriptGeneratorTestHelper;

namespace SqlStudio.Tests.UTSqlScriptDom
{
    [TestClass]
    public class PartitionByElementsListFormattingTests
    {
        private static SqlScriptGeneratorOptions MakeOptions(bool multiline)
        {
            return new SqlScriptGeneratorOptions
            {
                AlignClauseBodies = false,
                MultilinePartitionByElementsList = multiline,
                MultilineSelectElementsList = false,
            };
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestDefaultKeepsPartitionByElementsOnOneLine()
        {
            const string input = "SELECT SUM(c) OVER (PARTITION BY a, b) FROM t;";
            var options = new SqlScriptGeneratorOptions { MultilineSelectElementsList = false };
            const string expected = @"
SELECT SUM(c) OVER (PARTITION BY a, b)
FROM   t;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestPartitionByElementsStayOnOneLineWhenDisabled()
        {
            const string input = "SELECT SUM(c) OVER (PARTITION BY a, b) FROM t;";
            SqlScriptGeneratorOptions options = MakeOptions(false);
            const string expected = @"
SELECT SUM(c) OVER (PARTITION BY a, b)
FROM t;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestPartitionByElementsAreMultilineInOverClause()
        {
            const string input = "SELECT SUM(c) OVER (PARTITION BY a, b) FROM t;";
            SqlScriptGeneratorOptions options = MakeOptions(true);
            const string expected = @"
SELECT SUM(c) OVER (PARTITION BY a,
                                 b)
FROM t;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestPartitionByElementsAreMultilineInNamedWindow()
        {
            const string input = "SELECT SUM(c) OVER win FROM t WINDOW win AS (PARTITION BY a, b);";
            SqlScriptGeneratorOptions options = MakeOptions(true);
            const string expected = @"
SELECT SUM(c) OVER win
FROM t
WINDOW win AS (PARTITION BY a,
                            b);";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilinePartitionByElementsWithIndentedClauseBodies()
        {
            const string input = "SELECT SUM(c) OVER (PARTITION BY a, b) FROM t;";
            SqlScriptGeneratorOptions options = MakeOptions(true);
            options.ClauseBodyAlignment = ClauseBodyAlignment.Indented;
            const string expected = @"
SELECT
    SUM(c) OVER (PARTITION BY a,
                              b)
FROM
    t;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilinePartitionByElementsBeforeOrderByAndFrame()
        {
            const string input = "SELECT SUM(c) OVER (PARTITION BY a, b ORDER BY c ROWS UNBOUNDED PRECEDING) FROM t;";
            SqlScriptGeneratorOptions options = MakeOptions(true);
            const string expected = @"
SELECT SUM(c) OVER (PARTITION BY a,
                                 b ORDER BY c ROWS UNBOUNDED PRECEDING)
FROM t;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilinePartitionByElementsBeforeOrderByInNamedWindow()
        {
            const string input = "SELECT SUM(c) OVER win FROM t WINDOW win AS (PARTITION BY a, b ORDER BY c);";
            SqlScriptGeneratorOptions options = MakeOptions(true);
            const string expected = @"
SELECT SUM(c) OVER win
FROM t
WINDOW win AS (PARTITION BY a,
                            b ORDER BY c);";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilinePartitionByElementsHonorLeadingCommaPlacement()
        {
            const string input = "SELECT SUM(c) OVER (PARTITION BY a, b) FROM t;";
            SqlScriptGeneratorOptions options = MakeOptions(true);
            options.CommaPlacement = CommaPlacement.Leading;
            const string expected = @"
SELECT SUM(c) OVER (PARTITION BY a
                               , b)
FROM t;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIndentedMultilinePartitionByElementsHonorLeadingCommaPlacement()
        {
            const string input = "SELECT SUM(c) OVER (PARTITION BY a, b) FROM t;";
            SqlScriptGeneratorOptions options = MakeOptions(true);
            options.ClauseBodyAlignment = ClauseBodyAlignment.Indented;
            options.CommaPlacement = CommaPlacement.Leading;
            const string expected = @"
SELECT
    SUM(c) OVER (PARTITION BY a
                            , b)
FROM
    t;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestLeadingCommasWithMultilineSelectAndIndentedPartitionByElements()
        {
            const string input = "SELECT a, SUM(c) OVER (PARTITION BY a, b) FROM t;";
            SqlScriptGeneratorOptions options = MakeOptions(true);
            options.MultilineSelectElementsList = true;
            options.ClauseBodyAlignment = ClauseBodyAlignment.Indented;
            options.CommaPlacement = CommaPlacement.Leading;
            const string expected = @"
SELECT
    a
  , SUM(c) OVER (PARTITION BY a
                            , b)
FROM
    t;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestTrailingCommasWithMultilineSelectAndAlignedPartitionByElements()
        {
            const string input = "SELECT a, SUM(c) OVER (PARTITION BY a, b) FROM t;";
            SqlScriptGeneratorOptions options = MakeOptions(true);
            options.MultilineSelectElementsList = true;
            options.ClauseBodyAlignment = ClauseBodyAlignment.Aligned;
            options.AlignClauseBodies = true;
            options.CommaPlacement = CommaPlacement.Trailing;
            const string expected = @"
SELECT a,
       SUM(c) OVER (PARTITION BY a,
                                 b)
FROM   t;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilinePartitionByOnlySplitsTopLevelElements()
        {
            const string input = "SELECT SUM(c) OVER (PARTITION BY COALESCE(a, b), c, d) FROM t;";
            SqlScriptGeneratorOptions options = MakeOptions(true);
            const string expected = @"
SELECT SUM(c) OVER (PARTITION BY COALESCE (a, b),
                                 c,
                                 d)
FROM t;";

            AssertGenerated(input, options, expected);
        }
    }
}