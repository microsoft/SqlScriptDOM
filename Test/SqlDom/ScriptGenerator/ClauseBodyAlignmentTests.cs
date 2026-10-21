//------------------------------------------------------------------------------
// <copyright file="ClauseBodyAlignmentTests.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using Microsoft.SqlServer.TransactSql.ScriptDom;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlStudio.Tests.AssemblyTools.TestCategory;
using static SqlStudio.Tests.UTSqlScriptDom.ScriptGeneratorTestHelper;

namespace SqlStudio.Tests.UTSqlScriptDom
{
    // Tests for the ClauseBodyAlignment script-generation option, which controls how the body of a
    // clause (the part after FROM, WHERE, GROUP BY, etc.) is laid out: Aligned (the classic
    // keyword-width "rivers of whitespace" style) or Indented (each body on its own line, one indent
    // level past the keyword, so nesting grows linearly instead of drifting right).
    //
    // Work item: Formatter option: Indentation vs alignment (ClauseBodyAlignment)
    [TestClass]
    public class ClauseBodyAlignmentTests
    {
        // -----------------------------------------------------------------------------------------
        // Default
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestClauseBodyAlignmentDefaultIsAligned()
        {
            Assert.AreEqual(ClauseBodyAlignment.Aligned, new SqlScriptGeneratorOptions().ClauseBodyAlignment);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIndentedIgnoresAlignClauseBodies()
        {
            // When Indented, AlignClauseBodies has no effect: both settings produce the same output.
            const string input = "SELECT a FROM t WHERE b = 1;";
            var alignOn = new SqlScriptGeneratorOptions { ClauseBodyAlignment = ClauseBodyAlignment.Indented, AlignClauseBodies = true };
            var alignOff = new SqlScriptGeneratorOptions { ClauseBodyAlignment = ClauseBodyAlignment.Indented, AlignClauseBodies = false };

            Assert.AreEqual(Normalize(Generate(input, alignOn)), Normalize(Generate(input, alignOff)));
        }

        // --- Basic SELECT / FROM / WHERE ---------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestAlignedMatchesLegacyRiverLayout()
        {
            // Aligned mode reproduces the existing keyword-width "river" alignment.
            const string input = "SELECT a FROM t WHERE b = 1;";
            var options = new SqlScriptGeneratorOptions { ClauseBodyAlignment = ClauseBodyAlignment.Aligned };
            const string expected =
@"
SELECT a
FROM   t
WHERE  b = 1;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIndentedPutsEachClauseBodyOnItsOwnLine()
        {
            // Indented mode drops each clause body onto its own line, one indent level past the keyword.
            const string input = "SELECT a FROM t WHERE b = 1;";
            var options = new SqlScriptGeneratorOptions { ClauseBodyAlignment = ClauseBodyAlignment.Indented };
            const string expected =
@"
SELECT
    a
FROM
    t
WHERE
    b = 1;";

            AssertGenerated(input, options, expected);
        }

        // --- Multi-column SELECT list ------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestAlignedMultiColumnSelectList()
        {
            const string input = "SELECT a, b, c FROM t;";
            var options = new SqlScriptGeneratorOptions { ClauseBodyAlignment = ClauseBodyAlignment.Aligned };
            const string expected =
@"
SELECT a,
       b,
       c
FROM   t;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIndentedMultiColumnSelectList()
        {
            const string input = "SELECT a, b, c FROM t;";
            var options = new SqlScriptGeneratorOptions { ClauseBodyAlignment = ClauseBodyAlignment.Indented };
            const string expected =
@"
SELECT
    a,
    b,
    c
FROM
    t;";

            AssertGenerated(input, options, expected);
        }

        // --- DISTINCT / TOP stay on the SELECT line ----------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestAlignedKeepsDistinctAndTopOnSelectLine()
        {
            const string input = "SELECT DISTINCT TOP 5 a FROM t;";
            var options = new SqlScriptGeneratorOptions { ClauseBodyAlignment = ClauseBodyAlignment.Aligned };
            const string expected =
@"
SELECT DISTINCT TOP 5 a
FROM   t;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIndentedKeepsDistinctAndTopOnSelectLine()
        {
            const string input = "SELECT DISTINCT TOP 5 a FROM t;";
            var options = new SqlScriptGeneratorOptions { ClauseBodyAlignment = ClauseBodyAlignment.Indented };
            const string expected =
@"
SELECT DISTINCT TOP 5
    a
FROM
    t;";

            AssertGenerated(input, options, expected);
        }

        // --- GROUP BY / HAVING / ORDER BY --------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestAlignedGroupByAndHaving()
        {
            const string input = "SELECT a, COUNT(*) FROM t GROUP BY a HAVING COUNT(*) > 1 ORDER BY a;";
            var options = new SqlScriptGeneratorOptions { ClauseBodyAlignment = ClauseBodyAlignment.Aligned };
            const string expected =
@"
SELECT   a,
         COUNT(*)
FROM     t
GROUP BY a
HAVING   COUNT(*) > 1
ORDER BY a;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIndentedGroupByAndHaving()
        {
            const string input = "SELECT a, COUNT(*) FROM t GROUP BY a HAVING COUNT(*) > 1 ORDER BY a;";
            var options = new SqlScriptGeneratorOptions { ClauseBodyAlignment = ClauseBodyAlignment.Indented };
            const string expected =
@"
SELECT
    a,
    COUNT(*)
FROM
    t
GROUP BY
    a
HAVING
    COUNT(*) > 1
ORDER BY
    a;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIndentedLegacyGroupByAllColumnsStillIndent()
        {
            // Legacy GROUP BY ALL <columns> (accepted by the box parsers too): ALL stays on the
            // GROUP BY line as a keyword modifier, but the explicit column list still drops to its
            // own indented lines under Indented layout.
            const string input = "SELECT a, COUNT(*) FROM t GROUP BY ALL a, b;";
            var options = new SqlScriptGeneratorOptions { ClauseBodyAlignment = ClauseBodyAlignment.Indented };
            const string expected =
@"
SELECT
    a,
    COUNT(*)
FROM
    t
GROUP BY ALL
    a, b;";

            AssertGenerated(input, options, expected);
        }

        // --- GROUP BY ALL / ORDER BY ALL shorthands (Fabric DW first syntax) ----------------------
        // GROUP BY ALL (no column list) and ORDER BY ALL are Fabric DW first, so these go through the
        // Fabric DW pipeline (AssertGeneratedFabric). They confirm the ALL keyword is a clause-keyword
        // modifier that stays on the GROUP BY / ORDER BY line under both Aligned and Indented layouts.

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestAlignedGroupByAll()
        {
            // Aligned: clause bodies line up under the widest keyword that has a body (here SELECT /
            // FROM). The ALL shorthand has no body, so GROUP BY does not widen the alignment column;
            // ALL simply stays on the GROUP BY line.
            const string input = "SELECT City, COUNT(*) AS NumEmps FROM dbo.Employees GROUP BY ALL;";
            var options = new SqlScriptGeneratorOptions { ClauseBodyAlignment = ClauseBodyAlignment.Aligned };
            const string expected =
@"
SELECT City,
       COUNT(*) AS NumEmps
FROM   dbo.Employees
GROUP BY ALL;";

            AssertGeneratedFabric(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIndentedGroupByAllKeepsAllInline()
        {
            // Indented: every clause body drops to its own indented line, but the ALL shorthand is a
            // keyword modifier, so it stays on the GROUP BY line (no empty indented body line).
            const string input = "SELECT City, COUNT(*) AS NumEmps FROM dbo.Employees GROUP BY ALL;";
            var options = new SqlScriptGeneratorOptions { ClauseBodyAlignment = ClauseBodyAlignment.Indented };
            const string expected =
@"
SELECT
    City,
    COUNT(*) AS NumEmps
FROM
    dbo.Employees
GROUP BY ALL;";

            AssertGeneratedFabric(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestAlignedOrderByAll()
        {
            const string input = "SELECT c1, c2 FROM t1 ORDER BY ALL;";
            var options = new SqlScriptGeneratorOptions { ClauseBodyAlignment = ClauseBodyAlignment.Aligned };
            const string expected =
@"
SELECT c1,
       c2
FROM   t1
ORDER BY ALL;";

            AssertGeneratedFabric(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIndentedOrderByAllKeepsAllInline()
        {
            // Indented: the ALL shorthand (with its optional sort order) stays on the ORDER BY line
            // rather than being pushed onto its own indented line.
            const string input = "SELECT c1, c2 FROM t1 ORDER BY ALL DESC;";
            var options = new SqlScriptGeneratorOptions { ClauseBodyAlignment = ClauseBodyAlignment.Indented };
            const string expected =
@"
SELECT
    c1,
    c2
FROM
    t1
ORDER BY ALL DESC;";

            AssertGeneratedFabric(input, options, expected);
        }

        // --- Nested derived table (the motivating case: linear growth vs. rightward drift) -------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestAlignedNestedDerivedTable()
        {
            // Aligned pushes the inner query to the right of the outer FROM keyword; Indented (below)
            // adds a single fixed level per nesting instead.
            const string input = "SELECT o.a FROM (SELECT x AS a FROM t1 WHERE x > 0) AS o WHERE o.a < 10;";
            var options = new SqlScriptGeneratorOptions { ClauseBodyAlignment = ClauseBodyAlignment.Aligned };
            const string expected =
@"
SELECT o.a
FROM   (SELECT x AS a
        FROM   t1
        WHERE  x > 0) AS o
WHERE  o.a < 10;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIndentedNestedDerivedTableGrowsLinearly()
        {
            const string input = "SELECT o.a FROM (SELECT x AS a FROM t1 WHERE x > 0) AS o WHERE o.a < 10;";
            var options = new SqlScriptGeneratorOptions { ClauseBodyAlignment = ClauseBodyAlignment.Indented };
            const string expected =
@"
SELECT
    o.a
FROM
    (SELECT
         x AS a
     FROM
         t1
     WHERE
         x > 0) AS o
WHERE
    o.a < 10;";

            AssertGenerated(input, options, expected);
        }

        // --- UNION -------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestAlignedUnion()
        {
            const string input = "SELECT a FROM t1 UNION SELECT b FROM t2;";
            var options = new SqlScriptGeneratorOptions { ClauseBodyAlignment = ClauseBodyAlignment.Aligned };
            const string expected =
@"
SELECT a
FROM   t1
UNION
SELECT b
FROM   t2;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIndentedUnion()
        {
            const string input = "SELECT a FROM t1 UNION SELECT b FROM t2;";
            var options = new SqlScriptGeneratorOptions { ClauseBodyAlignment = ClauseBodyAlignment.Indented };
            const string expected =
@"
SELECT
    a
FROM
    t1
UNION
SELECT
    b
FROM
    t2;";

            AssertGenerated(input, options, expected);
        }

        // --- Common table expression -------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestAlignedCommonTableExpression()
        {
            const string input = "WITH c AS (SELECT a FROM t) SELECT a FROM c;";
            var options = new SqlScriptGeneratorOptions { ClauseBodyAlignment = ClauseBodyAlignment.Aligned };
            const string expected =
@"
WITH   c
AS     (SELECT a
        FROM   t)
SELECT a
FROM   c;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIndentedCommonTableExpression()
        {
            const string input = "WITH c AS (SELECT a FROM t) SELECT a FROM c;";
            var options = new SqlScriptGeneratorOptions { ClauseBodyAlignment = ClauseBodyAlignment.Indented };
            const string expected =
@"
WITH c
AS (SELECT
        a
    FROM
        t)
SELECT
    a
FROM
    c;";

            AssertGenerated(input, options, expected);
        }

        // --- Subquery in WHERE -------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestAlignedSubqueryInWhere()
        {
            const string input = "SELECT a FROM t WHERE x IN (SELECT id FROM u);";
            var options = new SqlScriptGeneratorOptions { ClauseBodyAlignment = ClauseBodyAlignment.Aligned };
            const string expected =
@"
SELECT a
FROM   t
WHERE  x IN (SELECT id
             FROM   u);";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIndentedSubqueryInWhere()
        {
            const string input = "SELECT a FROM t WHERE x IN (SELECT id FROM u);";
            var options = new SqlScriptGeneratorOptions { ClauseBodyAlignment = ClauseBodyAlignment.Indented };
            const string expected =
@"
SELECT
    a
FROM
    t
WHERE
    x IN (SELECT
              id
          FROM
              u);";

            AssertGenerated(input, options, expected);
        }

        // --- Custom indentation size -------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestAlignedIgnoresCustomIndentationSize()
        {
            // Aligned uses keyword width, not IndentationSize, so the size setting has no effect here:
            // the output is identical to the default-IndentationSize layout.
            const string input = "SELECT a FROM t WHERE b = 1;";
            var options = new SqlScriptGeneratorOptions { ClauseBodyAlignment = ClauseBodyAlignment.Aligned, IndentationSize = 2 };
            const string expected =
@"
SELECT a
FROM   t
WHERE  b = 1;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIndentedHonorsCustomIndentationSize()
        {
            const string input = "SELECT a FROM t WHERE b = 1;";
            var options = new SqlScriptGeneratorOptions { ClauseBodyAlignment = ClauseBodyAlignment.Indented, IndentationSize = 2 };
            const string expected =
@"
SELECT
  a
FROM
  t
WHERE
  b = 1;";

            AssertGenerated(input, options, expected);
        }

        // --- DELETE ... WHERE --------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestAlignedDeleteWhereClause()
        {
            const string input = "DELETE FROM t WHERE x = 1;";
            var options = new SqlScriptGeneratorOptions { ClauseBodyAlignment = ClauseBodyAlignment.Aligned };
            const string expected =
@"
DELETE t
WHERE  x = 1;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIndentedDeleteWhereClause()
        {
            const string input = "DELETE FROM t WHERE x = 1;";
            var options = new SqlScriptGeneratorOptions { ClauseBodyAlignment = ClauseBodyAlignment.Indented };
            const string expected =
@"
DELETE t
WHERE
    x = 1;";

            AssertGenerated(input, options, expected);
        }

        // --- UPDATE ------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestAlignedUpdateStatement()
        {
            const string input = "UPDATE t SET a = 1 WHERE b = 2;";
            var options = new SqlScriptGeneratorOptions { ClauseBodyAlignment = ClauseBodyAlignment.Aligned };
            const string expected =
@"
UPDATE t
SET    a = 1
WHERE  b = 2;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIndentedUpdateStatement()
        {
            const string input = "UPDATE t SET a = 1 WHERE b = 2;";
            var options = new SqlScriptGeneratorOptions { ClauseBodyAlignment = ClauseBodyAlignment.Indented };
            const string expected =
@"
UPDATE t
SET a = 1
WHERE
    b = 2;";

            AssertGenerated(input, options, expected);
        }

        // --- SELECT ... INTO ---------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestAlignedSelectInto()
        {
            const string input = "SELECT a INTO t2 FROM t1;";
            var options = new SqlScriptGeneratorOptions { ClauseBodyAlignment = ClauseBodyAlignment.Aligned };
            const string expected =
@"
SELECT a
INTO   t2
FROM   t1;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIndentedSelectInto()
        {
            const string input = "SELECT a INTO t2 FROM t1;";
            var options = new SqlScriptGeneratorOptions { ClauseBodyAlignment = ClauseBodyAlignment.Indented };
            const string expected =
@"
SELECT
    a
INTO
    t2
FROM
    t1;";

            AssertGenerated(input, options, expected);
        }

        // --- WHERE CURRENT OF --------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestAlignedWhereCurrentOf()
        {
            const string input = "DELETE FROM t WHERE CURRENT OF c;";
            var options = new SqlScriptGeneratorOptions { ClauseBodyAlignment = ClauseBodyAlignment.Aligned };
            const string expected =
@"
DELETE t
WHERE  CURRENT OF c;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIndentedWhereCurrentOf()
        {
            const string input = "DELETE FROM t WHERE CURRENT OF c;";
            var options = new SqlScriptGeneratorOptions { ClauseBodyAlignment = ClauseBodyAlignment.Indented };
            const string expected =
@"
DELETE t
WHERE
    CURRENT OF c;";

            AssertGenerated(input, options, expected);
        }

        // --- SELECT ... INTO ... ON filegroup ----------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestAlignedSelectIntoOnFilegroup()
        {
            const string input = "SELECT c1 INTO t2 ON fg FROM t1;";
            var options = new SqlScriptGeneratorOptions { ClauseBodyAlignment = ClauseBodyAlignment.Aligned };
            const string expected =
@"
SELECT c1
INTO   t2
ON     fg
FROM   t1;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIndentedSelectIntoOnFilegroup()
        {
            const string input = "SELECT c1 INTO t2 ON fg FROM t1;";
            var options = new SqlScriptGeneratorOptions { ClauseBodyAlignment = ClauseBodyAlignment.Indented };
            const string expected =
@"
SELECT
    c1
INTO
    t2
ON
    fg
FROM
    t1;";

            AssertGenerated(input, options, expected);
        }

        // --- JOIN with ON (does the ON search condition follow the clause-body option?) ----------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestAlignedInnerJoin()
        {
            const string input = "SELECT a.x FROM a INNER JOIN b ON a.id = b.id WHERE a.x > 0;";
            var options = new SqlScriptGeneratorOptions { ClauseBodyAlignment = ClauseBodyAlignment.Aligned };
            const string expected =
@"
SELECT a.x
FROM   a
       INNER JOIN
       b
       ON a.id = b.id
WHERE  a.x > 0;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIndentedInnerJoin()
        {
            const string input = "SELECT a.x FROM a INNER JOIN b ON a.id = b.id WHERE a.x > 0;";
            var options = new SqlScriptGeneratorOptions { ClauseBodyAlignment = ClauseBodyAlignment.Indented };
            const string expected =
@"
SELECT
    a.x
FROM
    a
    INNER JOIN
    b
    ON a.id = b.id
WHERE
    a.x > 0;";

            AssertGenerated(input, options, expected);
        }

        // --- JOIN with ON, NewLineBeforeJoinClause = false and NewLineBeforeOnClause = false ------
        // (both the JOIN keyword and the ON search condition stay on the table-source line) ---------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestAlignedInnerJoinNoNewLineBeforeJoin()
        {
            const string input = "SELECT a.x FROM a INNER JOIN b ON a.id = b.id WHERE a.x > 0;";
            var options = new SqlScriptGeneratorOptions { ClauseBodyAlignment = ClauseBodyAlignment.Aligned, NewLineBeforeJoinClause = false, NewLineBeforeOnClause = false };
            const string expected =
@"
SELECT a.x
FROM   a INNER JOIN
       b ON a.id = b.id
WHERE  a.x > 0;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIndentedInnerJoinNoNewLineBeforeJoin()
        {
            const string input = "SELECT a.x FROM a INNER JOIN b ON a.id = b.id WHERE a.x > 0;";
            var options = new SqlScriptGeneratorOptions { ClauseBodyAlignment = ClauseBodyAlignment.Indented, NewLineBeforeJoinClause = false, NewLineBeforeOnClause = false };
            const string expected =
@"
SELECT
    a.x
FROM
    a INNER JOIN
    b ON a.id = b.id
WHERE
    a.x > 0;";

            AssertGenerated(input, options, expected);
        }

        // --- NewLineBeforeXxxClause = false (the clause body stays on the keyword line) -----------
        // Indented only moves a clause body onto its own line when that clause is configured to
        // start on a new line. With the NewLineBefore* options off, FROM/WHERE keep their bodies
        // inline exactly as in Aligned mode. The SELECT list has no NewLineBefore* option of its
        // own, so it still breaks onto its own line.

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestAlignedNewLineBeforeClauseOptionsDisabled()
        {
            const string input = "SELECT a FROM t WHERE b = 1;";
            var options = new SqlScriptGeneratorOptions
            {
                ClauseBodyAlignment = ClauseBodyAlignment.Aligned,
                NewLineBeforeFromClause = false,
                NewLineBeforeWhereClause = false
            };
            const string expected = @"
SELECT a FROM t WHERE b = 1;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIndentedNewLineBeforeClauseOptionsDisabled()
        {
            const string input = "SELECT a FROM t WHERE b = 1;";
            var options = new SqlScriptGeneratorOptions
            {
                ClauseBodyAlignment = ClauseBodyAlignment.Indented,
                NewLineBeforeFromClause = false,
                NewLineBeforeWhereClause = false
            };
            const string expected = @"
SELECT
    a FROM t WHERE b = 1;";

            AssertGenerated(input, options, expected);
        }

        // --- AlignClauseBodies = false in Aligned mode -------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestAlignedWithAlignClauseBodiesDisabled()
        {
            // With AlignClauseBodies off, Aligned falls back to a single space after each keyword
            // instead of padding to the shared column. The SELECT list still aligns under its own
            // first item, which is a separate alignment point.
            const string input = "SELECT a, b FROM t WHERE c = 1;";
            var options = new SqlScriptGeneratorOptions
            {
                ClauseBodyAlignment = ClauseBodyAlignment.Aligned,
                AlignClauseBodies = false
            };
            const string expected =
@"
SELECT a,
       b
FROM t
WHERE c = 1;";

            AssertGenerated(input, options, expected);
        }

        // --- MultilineSelectElementsList = false --------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIndentedWithSingleLineSelectElementsList()
        {
            // The select list stays on one line but is still indented one level under SELECT.
            const string input = "SELECT a, b, c FROM t;";
            var options = new SqlScriptGeneratorOptions
            {
                ClauseBodyAlignment = ClauseBodyAlignment.Indented,
                MultilineSelectElementsList = false
            };
            const string expected =
@"
SELECT
    a, b, c
FROM
    t;";

            AssertGenerated(input, options, expected);
        }

        // --- Interaction with IndentationMode.Tabs ------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIndentedWithTabsIndentation()
        {
            // The indent that Indented adds is emitted through the normal indentation path, so it
            // is rendered with tab characters when IndentationMode is Tabs. The expected constant
            // below contains real tab characters.
            const string input = "SELECT a FROM t WHERE b = 1;";
            var options = new SqlScriptGeneratorOptions
            {
                ClauseBodyAlignment = ClauseBodyAlignment.Indented,
                IndentationMode = IndentationMode.Tabs
            };
            const string expected =
@"
SELECT
	a
FROM
	t
WHERE
	b = 1;";

            AssertGenerated(input, options, expected);
        }

        // --- Interaction with CommaPlacement.Leading ---------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIndentedWithLeadingCommas()
        {
            // Leading commas reserve their width inside the indent, so every select item still
            // starts at the same column as the first one.
            const string input = "SELECT a, b, c FROM t;";
            var options = new SqlScriptGeneratorOptions
            {
                ClauseBodyAlignment = ClauseBodyAlignment.Indented,
                CommaPlacement = CommaPlacement.Leading
            };
            const string expected =
@"
SELECT
    a
  , b
  , c
FROM
    t;";

            AssertGenerated(input, options, expected);
        }

        // --- OFFSET/FETCH is not a clause body and stays inline ----------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIndentedOffsetFetchIsNotIndented()
        {
            // ORDER BY is a clause body and indents; the OFFSET/FETCH clause that follows it is
            // not part of clause-body handling and keeps its existing inline layout.
            const string input = "SELECT a FROM t ORDER BY a OFFSET 10 ROWS FETCH NEXT 5 ROWS ONLY;";
            var options = new SqlScriptGeneratorOptions { ClauseBodyAlignment = ClauseBodyAlignment.Indented };
            const string expected =
@"
SELECT
    a
FROM
    t
ORDER BY
    a
OFFSET 10 ROWS FETCH NEXT 5 ROWS ONLY;";

            AssertGenerated(input, options, expected);
        }

        // --- FOR clause --------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestAlignedForClause()
        {
            const string input = "SELECT a FROM t FOR XML AUTO;";
            var options = new SqlScriptGeneratorOptions { ClauseBodyAlignment = ClauseBodyAlignment.Aligned };
            const string expected =
@"
SELECT a
FROM   t
FOR    XML AUTO;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIndentedForClause()
        {
            // The FOR clause body is treated like any other clause body, so it moves onto its own
            // indented line rather than staying on the FOR line.
            const string input = "SELECT a FROM t FOR XML AUTO;";
            var options = new SqlScriptGeneratorOptions { ClauseBodyAlignment = ClauseBodyAlignment.Indented };
            const string expected =
@"
SELECT
    a
FROM
    t
FOR
    XML AUTO;";

            AssertGenerated(input, options, expected);
        }
    }
}

