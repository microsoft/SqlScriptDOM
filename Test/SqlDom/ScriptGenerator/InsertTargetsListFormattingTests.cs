//------------------------------------------------------------------------------
// <copyright file="InsertTargetsListFormattingTests.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using Microsoft.SqlServer.TransactSql.ScriptDom;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlStudio.Tests.AssemblyTools.TestCategory;
using static SqlStudio.Tests.UTSqlScriptDom.ScriptGeneratorTestHelper;

namespace SqlStudio.Tests.UTSqlScriptDom
{
    // Tests for the MultilineInsertTargetsList script-generation option, which controls whether the
    // INSERT column target list (the parenthesized list of columns after the target table) is
    // written one column per line (true) as a multi-line parenthesized list - like the
    // CREATE TABLE / VIEW column lists - or collapsed onto a single line (false, the default). The
    // option only affects the INSERT target list; the INSERT source (VALUES / SELECT / EXECUTE) is
    // unaffected.
    [TestClass]
    public class InsertTargetsListFormattingTests
    {
        // -----------------------------------------------------------------------------------------
        // Default
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineInsertTargetsListDefaultIsFalse()
        {
            // The default preserves the historical single-line INSERT target list output.
            Assert.AreEqual(false, new SqlScriptGeneratorOptions().MultilineInsertTargetsList);
        }

        // -----------------------------------------------------------------------------------------
        // VALUES source
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestSingleLineTargetsIsDefault()
        {
            // With default options the target list stays on a single line (unchanged behavior).
            const string input = "INSERT INTO t (a, b, c) VALUES (1, 2, 3);";
            var options = new SqlScriptGeneratorOptions();
            const string expected =
@"
INSERT  INTO t (a, b, c)
VALUES        (1, 2, 3);";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineTargetsWhenEnabled()
        {
            // When the option is enabled each target column is placed on its own line inside the
            // parentheses.
            const string input = "INSERT INTO t (a, b, c) VALUES (1, 2, 3);";
            var options = new SqlScriptGeneratorOptions { MultilineInsertTargetsList = true };
            const string expected =
@"
INSERT  INTO t (
    a,
    b,
    c
)
VALUES        (1, 2, 3);";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestSingleLineTargetsWhenDisabled()
        {
            // With the option off the target list collapses onto a single line.
            const string input = "INSERT INTO t (a, b, c) VALUES (1, 2, 3);";
            var options = new SqlScriptGeneratorOptions { MultilineInsertTargetsList = false };
            const string expected =
@"
INSERT  INTO t (a, b, c)
VALUES        (1, 2, 3);";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestSingleTargetMultiline()
        {
            // A single-column target list is still spread onto its own line when the option is on.
            const string input = "INSERT INTO t (a) VALUES (1);";
            var options = new SqlScriptGeneratorOptions { MultilineInsertTargetsList = true };
            const string expected =
@"
INSERT  INTO t (
    a
)
VALUES        (1);";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestNoTargetsUnaffected()
        {
            // An INSERT with no column target list is unaffected by the option.
            const string input = "INSERT INTO t VALUES (1, 2, 3);";
            var options = new SqlScriptGeneratorOptions();
            const string expected =
@"
INSERT  INTO t
VALUES (1, 2, 3);";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestNoTargetsUnaffectedWhenDisabled()
        {
            // The same INSERT with no target list is identical whether the option is on or off.
            const string input = "INSERT INTO t VALUES (1, 2, 3);";
            var on = new SqlScriptGeneratorOptions { MultilineInsertTargetsList = true };
            var off = new SqlScriptGeneratorOptions { MultilineInsertTargetsList = false };

            Assert.AreEqual(Normalize(Generate(input, on)), Normalize(Generate(input, off)));
        }

        // -----------------------------------------------------------------------------------------
        // SELECT source
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineTargetsWithSelectSource()
        {
            // The multi-line target list works the same way when the source is a SELECT.
            const string input = "INSERT INTO t (a, b) SELECT x, y FROM s;";
            var options = new SqlScriptGeneratorOptions { MultilineInsertTargetsList = true };
            const string expected =
@"
INSERT INTO t (
    a,
    b
)
SELECT x,
       y
FROM   s;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestSingleLineTargetsWithSelectSourceWhenDisabled()
        {
            const string input = "INSERT INTO t (a, b) SELECT x, y FROM s;";
            var options = new SqlScriptGeneratorOptions { MultilineInsertTargetsList = false };
            const string expected =
@"
INSERT INTO t (a, b)
SELECT x,
       y
FROM   s;";

            AssertGenerated(input, options, expected);
        }

        // -----------------------------------------------------------------------------------------
        // Comma placement
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestLeadingCommaMultilineTargets()
        {
            // With CommaPlacement = Leading each column's comma is emitted at the start of its line,
            // indented two characters fewer than the column.
            const string input = "INSERT INTO t (a, b, c) VALUES (1, 2, 3);";
            var options = new SqlScriptGeneratorOptions { MultilineInsertTargetsList = true, CommaPlacement = CommaPlacement.Leading };
            const string expected =
@"
INSERT  INTO t (
    a
  , b
  , c
)
VALUES        (1, 2, 3);";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestTrailingCommaMultilineTargets()
        {
            // With CommaPlacement = Trailing (default) each column's comma follows it.
            const string input = "INSERT INTO t (a, b, c) VALUES (1, 2, 3);";
            var options = new SqlScriptGeneratorOptions { MultilineInsertTargetsList = true, CommaPlacement = CommaPlacement.Trailing };
            const string expected =
@"
INSERT  INTO t (
    a,
    b,
    c
)
VALUES        (1, 2, 3);";

            AssertGenerated(input, options, expected);
        }

        // -----------------------------------------------------------------------------------------
        // Parenthesis placement options
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineTargetsNewLineBeforeOpenParenthesis()
        {
            // NewLineBeforeOpenParenthesisInMultilineList moves the opening parenthesis to its own
            // line for the multi-line target list.
            const string input = "INSERT INTO t (a, b, c) VALUES (1, 2, 3);";
            var options = new SqlScriptGeneratorOptions
            {
                MultilineInsertTargetsList = true,
                NewLineBeforeOpenParenthesisInMultilineList = true,
            };
            const string expected =
@"
INSERT  INTO t
(
    a,
    b,
    c
)
VALUES        (1, 2, 3);";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineTargetsNewLineBeforeCloseParenthesisDisabled()
        {
            // With NewLineBeforeCloseParenthesisInMultilineList off the closing parenthesis stays on
            // the same line as the last column.
            const string input = "INSERT INTO t (a, b, c) VALUES (1, 2, 3);";
            var options = new SqlScriptGeneratorOptions
            {
                MultilineInsertTargetsList = true,
                NewLineBeforeCloseParenthesisInMultilineList = false,
            };
            const string expected =
@"
INSERT  INTO t (
    a,
    b,
    c)
VALUES        (1, 2, 3);";

            AssertGenerated(input, options, expected);
        }

        // -----------------------------------------------------------------------------------------
        // Interaction with other INSERT clauses / sources
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineTargetsWithOutputClause()
        {
            // The multi-line target list renders correctly when an OUTPUT clause follows it.
            const string input = "INSERT INTO t (a, b) OUTPUT inserted.a, inserted.b VALUES (1, 2);";
            var options = new SqlScriptGeneratorOptions { MultilineInsertTargetsList = true };
            const string expected =
@"
INSERT  INTO t (
    a,
    b
)
OUTPUT  inserted.a, inserted.b
VALUES        (1, 2);";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineTargetsWithOutputIntoClause()
        {
            // The multi-line target list renders correctly when an OUTPUT ... INTO clause follows it.
            const string input = "INSERT INTO t (a, b) OUTPUT inserted.a INTO @log VALUES (1, 2);";
            var options = new SqlScriptGeneratorOptions { MultilineInsertTargetsList = true };
            const string expected =
@"
INSERT  INTO t (
    a,
    b
)
OUTPUT  inserted.a INTO @log
VALUES        (1, 2);";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineTargetsWithExecuteSource()
        {
            // The multi-line target list works when the source is an EXECUTE statement.
            const string input = "INSERT INTO t (a, b) EXEC('select 1, 2');";
            var options = new SqlScriptGeneratorOptions { MultilineInsertTargetsList = true };
            const string expected =
@"
INSERT INTO t (
    a,
    b
)
EXECUTE ('select 1, 2');";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineTargetsWithTopRowFilter()
        {
            // A TOP row filter before the target table does not interfere with the multi-line list.
            const string input = "INSERT TOP (5) INTO t (a, b) SELECT x, y FROM s;";
            var options = new SqlScriptGeneratorOptions { MultilineInsertTargetsList = true };
            const string expected =
@"
INSERT TOP (5) INTO t (
    a,
    b
)
SELECT x,
       y
FROM   s;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineTargetsWithCommonTableExpression()
        {
            // A leading WITH common table expression does not interfere with the multi-line list.
            const string input = "WITH c AS (SELECT 1 x, 2 y) INSERT INTO t (a, b) SELECT x, y FROM c;";
            var options = new SqlScriptGeneratorOptions { MultilineInsertTargetsList = true };
            const string expected =
@"
WITH c
AS   (SELECT 1 AS x,
             2 AS y)
INSERT INTO t (
    a,
    b
)
SELECT x,
       y
FROM   c;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineTargetsWithMultiRowValues()
        {
            // Only the target list is affected; a multi-row VALUES source is left as-is.
            const string input = "INSERT INTO t (a, b) VALUES (1, 2), (3, 4);";
            var options = new SqlScriptGeneratorOptions { MultilineInsertTargetsList = true };
            const string expected =
@"
INSERT  INTO t (
    a,
    b
)
VALUES        (1, 2),
(3, 4);";

            AssertGenerated(input, options, expected);
        }

        // -----------------------------------------------------------------------------------------
        // MERGE ... WHEN NOT MATCHED THEN INSERT
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMergeInsertTargetsSingleLineIsDefault()
        {
            // The INSERT action of a MERGE keeps its target list single-line by default (unchanged
            // behavior), just like a top-level INSERT.
            const string input = "MERGE INTO t USING s ON t.id = s.id WHEN NOT MATCHED THEN INSERT (a, b, c) VALUES (s.a, s.b, s.c);";
            var options = new SqlScriptGeneratorOptions();
            const string expected =
@"
MERGE INTO t

USING s ON t.id = s.id
WHEN NOT MATCHED THEN INSERT (a, b, c) VALUES (s.a, s.b, s.c);";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMergeInsertTargetsMultilineWhenEnabled()
        {
            // The INSERT action of a MERGE honors MultilineInsertTargetsList, spreading its target
            // list one column per line - parity with the top-level INSERT statement.
            const string input = "MERGE INTO t USING s ON t.id = s.id WHEN NOT MATCHED THEN INSERT (a, b, c) VALUES (s.a, s.b, s.c);";
            var options = new SqlScriptGeneratorOptions { MultilineInsertTargetsList = true };
            const string expected =
@"
MERGE INTO t

USING s ON t.id = s.id
WHEN NOT MATCHED THEN INSERT (
    a,
    b,
    c
) VALUES (s.a, s.b, s.c);";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMergeInsertTargetsMultilineRealWorld()
        {
            // A realistic MERGE with an aliased target/source and UPDATE / INSERT / DELETE actions:
            // only the INSERT action's target list is spread onto multiple lines.
            const string input =
                "MERGE INTO dbo.Target AS tgt USING dbo.Source AS src ON tgt.Id = src.Id " +
                "WHEN MATCHED THEN UPDATE SET tgt.Name = src.Name, tgt.Amount = src.Amount " +
                "WHEN NOT MATCHED BY TARGET THEN INSERT (Id, Name, Amount) VALUES (src.Id, src.Name, src.Amount) " +
                "WHEN NOT MATCHED BY SOURCE THEN DELETE;";
            var options = new SqlScriptGeneratorOptions { MultilineInsertTargetsList = true };
            const string expected =
@"
MERGE INTO dbo.Target
 AS tgt
USING dbo.Source AS src ON tgt.Id = src.Id
WHEN MATCHED THEN UPDATE 
SET tgt.Name   = src.Name,
    tgt.Amount = src.Amount
WHEN NOT MATCHED BY TARGET THEN INSERT (
    Id,
    Name,
    Amount
) VALUES (src.Id, src.Name, src.Amount)
WHEN NOT MATCHED BY SOURCE THEN DELETE;";

            AssertGenerated(input, options, expected);
        }
    }
}
