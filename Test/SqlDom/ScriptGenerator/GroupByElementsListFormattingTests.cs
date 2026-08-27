//------------------------------------------------------------------------------
// <copyright file="GroupByElementsListFormattingTests.cs" company="Microsoft">
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
    public class GroupByElementsListFormattingTests
    {
        private static SqlScriptGeneratorOptions MakeOptions(bool multiline)
        {
            return new SqlScriptGeneratorOptions
            {
                AlignClauseBodies = false,
                MultilineGroupByElementsList = multiline,
                MultilineSelectElementsList = false,
            };
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestDefaultKeepsGroupByElementsOnOneLine()
        {
            const string input = "SELECT a, b, COUNT(*) FROM t GROUP BY a, b;";
            var options = new SqlScriptGeneratorOptions { MultilineSelectElementsList = false };
            const string expected = @"
SELECT   a, b, COUNT(*)
FROM     t
GROUP BY a, b;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestGroupByElementsStayOnOneLineWhenDisabled()
        {
            const string input = "SELECT a, b, COUNT(*) FROM t GROUP BY a, b;";
            SqlScriptGeneratorOptions options = MakeOptions(false);
            const string expected = @"
SELECT a, b, COUNT(*)
FROM t
GROUP BY a, b;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestGroupByElementsAreMultilineWhenEnabled()
        {
            const string input = "SELECT a, b, COUNT(*) FROM t GROUP BY a, b;";
            SqlScriptGeneratorOptions options = MakeOptions(true);
            const string expected = @"
SELECT a, b, COUNT(*)
FROM t
GROUP BY a,
         b;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineGroupByElementsWithIndentedClauseBodies()
        {
            const string input = "SELECT a, b, COUNT(*) FROM t GROUP BY a, b;";
            SqlScriptGeneratorOptions options = MakeOptions(true);
            options.ClauseBodyAlignment = ClauseBodyAlignment.Indented;
            const string expected = @"
SELECT
    a, b, COUNT(*)
FROM
    t
GROUP BY
    a,
    b;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineLegacyGroupByAllElements()
        {
            const string input = "SELECT a, b, COUNT(*) FROM t GROUP BY ALL a, b;";
            SqlScriptGeneratorOptions options = MakeOptions(true);
            const string expected = @"
SELECT a, b, COUNT(*)
FROM t
GROUP BY ALL a,
             b;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineGroupByElementsBeforeWithCube()
        {
            const string input = "SELECT a, b, COUNT(*) FROM t GROUP BY a, b WITH CUBE;";
            SqlScriptGeneratorOptions options = MakeOptions(true);
            const string expected = @"
SELECT a, b, COUNT(*)
FROM t
GROUP BY a,
         b WITH CUBE;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineGroupByElementsHonorLeadingCommaPlacement()
        {
            const string input = "SELECT a, b, COUNT(*) FROM t GROUP BY a, b;";
            SqlScriptGeneratorOptions options = MakeOptions(true);
            options.CommaPlacement = CommaPlacement.Leading;
            const string expected = @"
SELECT a, b, COUNT(*)
FROM t
GROUP BY a
       , b;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIndentedMultilineGroupByElementsHonorLeadingCommaPlacement()
        {
            const string input = "SELECT a, b, COUNT(*) FROM t GROUP BY a, b;";
            SqlScriptGeneratorOptions options = MakeOptions(true);
            options.ClauseBodyAlignment = ClauseBodyAlignment.Indented;
            options.CommaPlacement = CommaPlacement.Leading;
            const string expected = @"
SELECT
    a, b, COUNT(*)
FROM
    t
GROUP BY
    a
  , b;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestLeadingCommasWithMultilineSelectAndIndentedGroupByElements()
        {
            const string input = "SELECT a, b, COUNT(*) FROM t GROUP BY a, b;";
            SqlScriptGeneratorOptions options = MakeOptions(true);
            options.MultilineSelectElementsList = true;
            options.ClauseBodyAlignment = ClauseBodyAlignment.Indented;
            options.CommaPlacement = CommaPlacement.Leading;
            const string expected = @"
SELECT
    a
  , b
  , COUNT(*)
FROM
    t
GROUP BY
    a
  , b;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestTrailingCommasWithMultilineSelectAndAlignedGroupByElements()
        {
            const string input = "SELECT a, b, COUNT(*) FROM t GROUP BY a, b;";
            SqlScriptGeneratorOptions options = MakeOptions(true);
            options.MultilineSelectElementsList = true;
            options.ClauseBodyAlignment = ClauseBodyAlignment.Aligned;
            options.AlignClauseBodies = true;
            options.CommaPlacement = CommaPlacement.Trailing;
            const string expected = @"
SELECT   a,
         b,
         COUNT(*)
FROM     t
GROUP BY a,
         b;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineGroupByOnlySplitsTopLevelElements()
        {
            const string input = "SELECT a, b, c, COUNT(*) FROM t GROUP BY ROLLUP(a, b), c;";
            SqlScriptGeneratorOptions options = MakeOptions(true);
            const string expected = @"
SELECT a, b, c, COUNT(*)
FROM t
GROUP BY ROLLUP(a, b),
         c;";

            AssertGenerated(input, options, expected);
        }
    }
}