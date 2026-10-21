//------------------------------------------------------------------------------
// <copyright file="HavingPredicatesListFormattingTests.cs" company="Microsoft">
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
    public class HavingPredicatesListFormattingTests
    {
        private static SqlScriptGeneratorOptions MakeOptions(bool multiline)
        {
            return new SqlScriptGeneratorOptions
            {
                AlignClauseBodies = false,
                MultilineHavingPredicatesList = multiline,
                MultilineSelectElementsList = false,
                MultilineWherePredicatesList = false,
            };
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestDefaultWritesHavingPredicatesOnMultipleLines()
        {
            const string input = "SELECT a FROM t GROUP BY a HAVING COUNT(*) > 1 AND SUM(a) > 2;";
            var options = new SqlScriptGeneratorOptions { MultilineSelectElementsList = false };
            const string expected = @"
SELECT   a
FROM     t
GROUP BY a
HAVING   COUNT(*) > 1
         AND SUM(a) > 2;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestHavingPredicatesStayOnOneLineWhenDisabled()
        {
            const string input = "SELECT a FROM t GROUP BY a HAVING COUNT(*) > 1 AND SUM(a) > 2;";
            SqlScriptGeneratorOptions options = MakeOptions(false);
            const string expected = @"
SELECT a
FROM t
GROUP BY a
HAVING COUNT(*) > 1 AND SUM(a) > 2;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestHavingPredicatesAreMultilineWhenEnabled()
        {
            const string input = "SELECT a FROM t WHERE a > 0 AND a < 10 GROUP BY a HAVING COUNT(*) > 1 AND SUM(a) > 2;";
            SqlScriptGeneratorOptions options = MakeOptions(true);
            const string expected = @"
SELECT a
FROM t
WHERE a > 0 AND a < 10
GROUP BY a
HAVING COUNT(*) > 1
       AND SUM(a) > 2;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineHavingPredicatesWithIndentedClauseBodies()
        {
            const string input = "SELECT a FROM t GROUP BY a HAVING COUNT(*) > 1 AND SUM(a) > 2;";
            SqlScriptGeneratorOptions options = MakeOptions(true);
            options.ClauseBodyAlignment = ClauseBodyAlignment.Indented;
            const string expected = @"
SELECT
    a
FROM
    t
GROUP BY
    a
HAVING
    COUNT(*) > 1
    AND SUM(a) > 2;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineHavingOrPredicates()
        {
            const string input = "SELECT a FROM t GROUP BY a HAVING COUNT(*) > 1 OR SUM(a) > 2;";
            SqlScriptGeneratorOptions options = MakeOptions(true);
            const string expected = @"
SELECT a
FROM t
GROUP BY a
HAVING COUNT(*) > 1
       OR SUM(a) > 2;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineHavingPreservesParenthesizedMixedPredicates()
        {
            const string input = "SELECT a FROM t GROUP BY a HAVING (COUNT(*) > 1 OR SUM(a) > 2) AND MAX(a) < 10;";
            SqlScriptGeneratorOptions options = MakeOptions(true);
            const string expected = @"
SELECT a
FROM t
GROUP BY a
HAVING (COUNT(*) > 1
        OR SUM(a) > 2)
       AND MAX(a) < 10;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestNestedWhereUsesItsOwnPredicateSetting()
        {
            const string input = "SELECT a FROM t GROUP BY a HAVING EXISTS (SELECT 1 FROM u WHERE u.a = t.a AND u.b > 0) AND COUNT(*) > 1;";
            SqlScriptGeneratorOptions options = MakeOptions(false);
            options.MultilineWherePredicatesList = true;
            const string expected = @"
SELECT a
FROM t
GROUP BY a
HAVING EXISTS (SELECT 1
               FROM u
               WHERE u.a = t.a
                     AND u.b > 0) AND COUNT(*) > 1;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIndentedWhereAndHavingUseIndependentPredicateSettings()
        {
            const string input = "SELECT a FROM t WHERE a > 0 AND a < 10 GROUP BY a HAVING COUNT(*) > 1 AND SUM(a) > 2;";
            SqlScriptGeneratorOptions options = MakeOptions(true);
            options.ClauseBodyAlignment = ClauseBodyAlignment.Indented;
            const string expected = @"
SELECT
    a
FROM
    t
WHERE
    a > 0 AND a < 10
GROUP BY
    a
HAVING
    COUNT(*) > 1
    AND SUM(a) > 2;";

            AssertGenerated(input, options, expected);
        }
    }
}