//------------------------------------------------------------------------------
// <copyright file="OrderByElementsListFormattingTests.cs" company="Microsoft">
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
    public class OrderByElementsListFormattingTests
    {
        private static SqlScriptGeneratorOptions MakeOptions(bool multiline)
        {
            return new SqlScriptGeneratorOptions
            {
                AlignClauseBodies = false,
                MultilineOrderByElementsList = multiline,
                MultilineSelectElementsList = false,
            };
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestDefaultKeepsOrderByElementsOnOneLine()
        {
            const string input = "SELECT a, b FROM t ORDER BY a, b DESC;";
            var options = new SqlScriptGeneratorOptions { MultilineSelectElementsList = false };
            const string expected = @"
SELECT   a, b
FROM     t
ORDER BY a, b DESC;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestOrderByElementsStayOnOneLineWhenDisabled()
        {
            const string input = "SELECT a, b FROM t ORDER BY a, b DESC;";
            SqlScriptGeneratorOptions options = MakeOptions(false);
            const string expected = @"
SELECT a, b
FROM t
ORDER BY a, b DESC;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestOrderByElementsAreMultilineWhenEnabled()
        {
            const string input = "SELECT a, b FROM t ORDER BY a, b DESC;";
            SqlScriptGeneratorOptions options = MakeOptions(true);
            const string expected = @"
SELECT a, b
FROM t
ORDER BY a,
         b DESC;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineOrderByElementsWithIndentedClauseBodies()
        {
            const string input = "SELECT a, b FROM t ORDER BY a, b DESC;";
            SqlScriptGeneratorOptions options = MakeOptions(true);
            options.ClauseBodyAlignment = ClauseBodyAlignment.Indented;
            const string expected = @"
SELECT
    a, b
FROM
    t
ORDER BY
    a,
    b DESC;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineOrderByElementsHonorLeadingCommaPlacement()
        {
            const string input = "SELECT a, b FROM t ORDER BY a, b DESC;";
            SqlScriptGeneratorOptions options = MakeOptions(true);
            options.CommaPlacement = CommaPlacement.Leading;
            const string expected = @"
SELECT a, b
FROM t
ORDER BY a
       , b DESC;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIndentedMultilineOrderByElementsHonorLeadingCommaPlacement()
        {
            const string input = "SELECT a, b FROM t ORDER BY a, b DESC;";
            SqlScriptGeneratorOptions options = MakeOptions(true);
            options.ClauseBodyAlignment = ClauseBodyAlignment.Indented;
            options.CommaPlacement = CommaPlacement.Leading;
            const string expected = @"
SELECT
    a, b
FROM
    t
ORDER BY
    a
  , b DESC;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestLeadingCommasWithMultilineSelectAndIndentedOrderByElements()
        {
            const string input = "SELECT a, b FROM t ORDER BY a, b DESC;";
            SqlScriptGeneratorOptions options = MakeOptions(true);
            options.MultilineSelectElementsList = true;
            options.ClauseBodyAlignment = ClauseBodyAlignment.Indented;
            options.CommaPlacement = CommaPlacement.Leading;
            const string expected = @"
SELECT
    a
  , b
FROM
    t
ORDER BY
    a
  , b DESC;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestTrailingCommasWithMultilineSelectAndAlignedOrderByElements()
        {
            const string input = "SELECT a, b FROM t ORDER BY a, b DESC;";
            SqlScriptGeneratorOptions options = MakeOptions(true);
            options.MultilineSelectElementsList = true;
            options.ClauseBodyAlignment = ClauseBodyAlignment.Aligned;
            options.AlignClauseBodies = true;
            options.CommaPlacement = CommaPlacement.Trailing;
            const string expected = @"
SELECT   a,
         b
FROM     t
ORDER BY a,
         b DESC;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestThreeMultilineOrderByElementsHonorLeadingCommaSpacing()
        {
            const string input = "SELECT a, b, c FROM t ORDER BY a, b DESC, c;";
            SqlScriptGeneratorOptions options = MakeOptions(true);
            options.CommaPlacement = CommaPlacement.Leading;
            options.LeadingCommaSpaceCount = 2;
            const string expected = @"
SELECT a, b, c
FROM t
ORDER BY a
      ,  b DESC
      ,  c;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineOrderByElementsInOverClause()
        {
            const string input = "SELECT ROW_NUMBER() OVER (ORDER BY a, b DESC) FROM t;";
            SqlScriptGeneratorOptions options = MakeOptions(true);
            const string expected = @"
SELECT ROW_NUMBER() OVER (ORDER BY a,
                                   b DESC)
FROM t;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestOrderByAllIgnoresMultilineElementsSetting()
        {
            const string input = "SELECT a, b FROM t ORDER BY ALL DESC;";
            SqlScriptGeneratorOptions options = MakeOptions(true);
            const string expected = @"
SELECT a, b
FROM t
ORDER BY ALL DESC;";

            AssertGeneratedFabric(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestOrderByAllWithoutSortOrderIgnoresMultilineElementsSetting()
        {
            const string input = "SELECT a, b FROM t ORDER BY ALL;";
            SqlScriptGeneratorOptions options = MakeOptions(true);
            const string expected = @"
SELECT a, b
FROM t
ORDER BY ALL;";

            AssertGeneratedFabric(input, options, expected);
        }
    }
}