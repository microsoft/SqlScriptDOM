//------------------------------------------------------------------------------
// <copyright file="CaseExpressionFormattingTests.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using Microsoft.SqlServer.TransactSql.ScriptDom;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlStudio.Tests.AssemblyTools.TestCategory;
using static SqlStudio.Tests.UTSqlScriptDom.ScriptGeneratorTestHelper;

namespace SqlStudio.Tests.UTSqlScriptDom
{
    // Tests for the MultilineCaseExpression script-generation option, which controls whether the
    // WHEN clauses and ELSE of a CASE expression are written on a single line (default) or each on
    // its own line, with END aligned with CASE. Kept in a dedicated file to avoid churn in
    // ScriptGeneratorTests.cs.
    [TestClass]
    public class CaseExpressionFormattingTests
    {
        // Builds options that isolate the CASE layout: clause bodies are not aligned and clauses are
        // not broken onto their own lines, so the surrounding statement stays on one line and the
        // expectations focus on the CASE expression itself.
        private static SqlScriptGeneratorOptions MakeOptions(bool multilineCaseExpression)
        {
            return new SqlScriptGeneratorOptions
            {
                MultilineCaseExpression = multilineCaseExpression,
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
        public void TestMultilineCaseExpressionDefaultIsFalse()
        {
            Assert.IsFalse(new SqlScriptGeneratorOptions().MultilineCaseExpression);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestDefaultKeepsSearchedCaseOnSingleLine()
        {
            const string input = "SELECT CASE WHEN a = 1 THEN 'x' WHEN a = 2 THEN 'y' ELSE 'z' END AS c FROM t;";
            var options = MakeOptions(false);
            const string expected = "SELECT CASE WHEN a = 1 THEN 'x' WHEN a = 2 THEN 'y' ELSE 'z' END AS c FROM t;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestDefaultKeepsSimpleCaseOnSingleLine()
        {
            const string input = "SET @v = CASE @w WHEN 1 THEN 'a' WHEN 2 THEN 'b' ELSE 'c' END;";
            var options = MakeOptions(false);
            const string expected = "SET @v = CASE @w WHEN 1 THEN 'a' WHEN 2 THEN 'b' ELSE 'c' END;";

            AssertGenerated(input, options, expected);
        }

        // -----------------------------------------------------------------------------------------
        // Multi-line layout
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineSearchedCasePutsEachBranchOnItsOwnLine()
        {
            // Each WHEN and the ELSE are indented one level (4) from CASE, and END is aligned with it.
            const string input = "SELECT CASE WHEN a = 1 THEN 'x' WHEN a = 2 THEN 'y' ELSE 'z' END AS c FROM t;";
            var options = MakeOptions(true);
            const string expected = @"
SELECT CASE
           WHEN a = 1 THEN 'x'
           WHEN a = 2 THEN 'y'
           ELSE 'z'
       END AS c FROM t;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineSimpleCaseKeepsInputExpressionOnCaseLine()
        {
            // The input expression of a simple CASE stays on the CASE line.
            const string input = "SET @v = CASE @w WHEN 1 THEN 'a' WHEN 2 THEN 'b' ELSE 'c' END;";
            var options = MakeOptions(true);
            const string expected = @"
SET @v = CASE @w
             WHEN 1 THEN 'a'
             WHEN 2 THEN 'b'
             ELSE 'c'
         END;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineWithoutElse()
        {
            const string input = "SELECT CASE WHEN a = 1 THEN 'x' WHEN a = 2 THEN 'y' END FROM t;";
            var options = MakeOptions(true);
            const string expected = @"
SELECT CASE
           WHEN a = 1 THEN 'x'
           WHEN a = 2 THEN 'y'
       END FROM t;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineSingleBranchCase()
        {
            // A CASE with a single WHEN is laid out the same way as one with several.
            const string input = "SELECT CASE WHEN a = 1 THEN 1 ELSE 0 END FROM t;";
            var options = MakeOptions(true);
            const string expected = @"
SELECT CASE
           WHEN a = 1 THEN 1
           ELSE 0
       END FROM t;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineNestedCaseAlignsWithItsOwnCase()
        {
            // A nested CASE aligns its branches and END with its own CASE keyword.
            const string input = "SELECT CASE WHEN a = 1 THEN CASE WHEN b = 1 THEN 'x' ELSE 'y' END ELSE 'z' END FROM t;";
            var options = MakeOptions(true);
            const string expected = @"
SELECT CASE
           WHEN a = 1 THEN CASE
                               WHEN b = 1 THEN 'x'
                               ELSE 'y'
                           END
           ELSE 'z'
       END FROM t;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineCollationFollowsEnd()
        {
            const string input = "SELECT CASE WHEN a = 1 THEN 'x' ELSE 'y' END COLLATE Latin1_General_CI_AS FROM t;";
            var options = MakeOptions(true);
            const string expected = @"
SELECT CASE
           WHEN a = 1 THEN 'x'
           ELSE 'y'
       END COLLATE Latin1_General_CI_AS FROM t;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineHonorsIndentationSize()
        {
            const string input = "SELECT CASE WHEN a = 1 THEN 'x' ELSE 'y' END FROM t;";
            var options = MakeOptions(true);
            options.IndentationSize = 2;
            const string expected = @"
SELECT CASE
         WHEN a = 1 THEN 'x'
         ELSE 'y'
       END FROM t;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineKeepsTrailingCommentOnItsBranch()
        {
            // A trailing comment on a branch stays on that branch's line rather than moving to the end
            // of the statement.
            const string input =
@"SET @v = CASE @w
             WHEN 1 THEN 'a'
             ELSE 'b' -- anything else
         END;";
            var options = MakeOptions(true);
            options.PreserveComments = true;
            const string expected = @"
SET @v = CASE @w
             WHEN 1 THEN 'a'
             ELSE 'b' -- anything else
         END;";

            AssertGenerated(input, options, expected);
        }

        // -----------------------------------------------------------------------------------------
        // Interaction with other options
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineWithDefaultOptionsHasNoStaircase()
        {
            // With otherwise-default options, a WHEN predicate joined by AND still breaks at the AND
            // (MultilineWherePredicatesList defaults to true) and the continuation aligns with the start
            // of the predicate. Each following WHEN starts its own line, so the continuations do not
            // compound.
            const string input =
@"SELECT CASE
           WHEN o.status = 'P' AND o.paid_amount >= o.total_amount THEN 'settled'
           WHEN o.status = 'P' AND o.paid_amount > 0 THEN 'part-paid'
           WHEN o.status = 'C' THEN 'cancelled'
           ELSE 'open'
       END AS settlement_state,
       CASE WHEN o.due_date < SYSUTCDATETIME() THEN 1 ELSE 0 END AS is_overdue
FROM orders AS o
WHERE o.tenant_id = 42;";
            var options = new SqlScriptGeneratorOptions { MultilineCaseExpression = true };
            const string expected = @"
SELECT CASE
           WHEN o.status = 'P'
                AND o.paid_amount >= o.total_amount THEN 'settled'
           WHEN o.status = 'P'
                AND o.paid_amount > 0 THEN 'part-paid'
           WHEN o.status = 'C' THEN 'cancelled'
           ELSE 'open'
       END AS settlement_state,
       CASE
           WHEN o.due_date < SYSUTCDATETIME() THEN 1
           ELSE 0
       END AS is_overdue
FROM   orders AS o
WHERE  o.tenant_id = 42;";

            AssertGenerated(input, options, expected);
        }
    }
}
