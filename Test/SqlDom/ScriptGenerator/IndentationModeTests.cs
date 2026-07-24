//------------------------------------------------------------------------------
// <copyright file="IndentationModeTests.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using Microsoft.SqlServer.TransactSql.ScriptDom;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlStudio.Tests.AssemblyTools.TestCategory;
using static SqlStudio.Tests.UTSqlScriptDom.ScriptGeneratorTestHelper;

namespace SqlStudio.Tests.UTSqlScriptDom
{
    // These tests verify the IndentationMode script generation option (Spaces vs Tabs). The
    // expected constants are verbatim strings whose leading whitespace is real tab characters, so
    // the expected output reads the same way it is written to the script.
    [TestClass]
    public class IndentationModeTests
    {
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIndentationModeDefaultIsSpaces()
        {
            Assert.AreEqual(IndentationMode.Spaces, new SqlScriptGeneratorOptions().IndentationMode);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIndentationModeTabsIndentsBlockBody()
        {
            // In Tabs mode the leading whitespace of every line is written with tab characters,
            // and the gap between a clause keyword and its body is also a tab, so clause bodies
            // line up on a tab stop. The SELECT-list and WHERE-predicate continuation lines round
            // their indentation up to the same tab stop.
            const string input = "BEGIN SELECT a, b FROM t WHERE a = 1 AND b = 2; END";
            var options = new SqlScriptGeneratorOptions { IndentationMode = IndentationMode.Tabs };
            const string expected =
@"
BEGIN
	SELECT	a,
			b
	FROM	t
	WHERE	a = 1
			AND b = 2;
END";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIndentationModeTabsColumnDefinitionsSkipCommaPadding()
        {
            // The CREATE TABLE column definitions are indented with a tab and the gap between the
            // column name and its data type is a tab. A column whose only remaining token is the
            // comma is not padded to line up the commas, so the comma follows the data type directly.
            const string input = "CREATE TABLE t (a INT, b NVARCHAR(10), c INT);";
            var options = new SqlScriptGeneratorOptions { IndentationMode = IndentationMode.Tabs };
            const string expected =
@"
CREATE TABLE t (
	a	INT,
	b	NVARCHAR (10),
	c	INT
);";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIndentationModeTabsColumnDefinitionsAlignConstraintsWithTabs()
        {
            // When columns have constraints, every column-definition field (name, data type,
            // constraint) is aligned on a tab stop. The gaps between fields are rendered with tabs,
            // and each field lines up on the next tab stop after the widest field in that column.
            const string input = "CREATE TABLE t (a INT NOT NULL, b NVARCHAR(10) NULL, c INT DEFAULT 0);";
            var options = new SqlScriptGeneratorOptions { IndentationMode = IndentationMode.Tabs };
            const string expected =
@"
CREATE TABLE t (
	a	INT				NOT NULL,
	b	NVARCHAR (10)	NULL,
	c	INT				DEFAULT 0
);";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIndentationModeTabsColumnDefinitionsCascadeAlignFields()
        {
            // Robust cascade: with varying column-name and data-type widths, each field column is
            // snapped to the next tab stop after the widest field to its left, and the snap of an
            // earlier field is carried forward so later fields stay aligned across all rows.
            const string input = "CREATE TABLE t (id INT NOT NULL, description NVARCHAR(200) NULL, x BIT DEFAULT 0);";
            var options = new SqlScriptGeneratorOptions { IndentationMode = IndentationMode.Tabs };
            const string expected =
@"
CREATE TABLE t (
	id			INT				NOT NULL,
	description	NVARCHAR (200)	NULL,
	x			BIT				DEFAULT 0
);";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIndentationModeTabsNestedBlocksUseMultipleTabs()
        {
            // Each nested indentation level adds one more tab character.
            const string input = "CREATE PROCEDURE p AS BEGIN IF (1=1) BEGIN SELECT 1; END END";
            var options = new SqlScriptGeneratorOptions { IndentationMode = IndentationMode.Tabs };
            const string expected =
@"
CREATE PROCEDURE p
AS
BEGIN
	IF (1 = 1)
		BEGIN
			SELECT	1;
		END
END";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIndentationModeTabsWithLeadingCommaFallsBackToSpaces()
        {
            // Interaction rule: leading-comma placement offsets the comma two columns before the
            // alignment column using sub-indent-level spacing, which cannot be represented with tab
            // characters. When both IndentationMode = Tabs and CommaPlacement = Leading are set,
            // indentation falls back to spaces so the leading comma still aligns, and no tab
            // character is ever emitted.
            const string input = "BEGIN SELECT a, b FROM t WHERE a = 1 AND b = 2; END";

            string tabsLeading = Generate(input, new SqlScriptGeneratorOptions
            {
                IndentationMode = IndentationMode.Tabs,
                CommaPlacement = CommaPlacement.Leading
            });

            string spacesLeading = Generate(input, new SqlScriptGeneratorOptions
            {
                IndentationMode = IndentationMode.Spaces,
                CommaPlacement = CommaPlacement.Leading
            });

            // Tabs mode is ignored under leading-comma placement: output is identical to spaces
            // mode and contains no tab characters.
            Assert.IsFalse(tabsLeading.Contains("\t"), "No tab characters should be emitted when CommaPlacement is Leading. Actual:\n" + tabsLeading);
            Assert.AreEqual(spacesLeading, tabsLeading);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIndentationModeTabsViewColumnListUsesTabs()
        {
            // The CREATE VIEW column list is indented one tab per line, and the view's SELECT body
            // uses tabs both for the clause-keyword gap and for the continuation lines of the
            // select list.
            const string input = "CREATE VIEW v (a, b, c) AS SELECT 1, 2, 3;";
            var options = new SqlScriptGeneratorOptions { IndentationMode = IndentationMode.Tabs };
            const string expected =
@"
CREATE VIEW v (
	a,
	b,
	c
)
AS
SELECT	1,
		2,
		3;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIndentationModeTabsClauseBodyUsesTab()
        {
            // The gap between a clause keyword and its body is rendered as a tab, so even a
            // single-line SELECT places its body on a tab stop.
            const string input = "SELECT 1;";
            var options = new SqlScriptGeneratorOptions { IndentationMode = IndentationMode.Tabs };
            const string expected =
@"
SELECT	1;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIndentationModeTabsUpdateSetClauseAlignsWithTabs()
        {
            // The UPDATE SET items are indented with tabs and their "=" signs are aligned on a tab
            // stop. The single-predicate WHERE clause has no alignment, so it keeps spaces.
            const string input = "UPDATE t SET a = 1, b = 2, c = 3 WHERE d = 4;";
            var options = new SqlScriptGeneratorOptions { IndentationMode = IndentationMode.Tabs };
            const string expected =
@"
UPDATE	t
SET		a	= 1,
		b	= 2,
		c	= 3
WHERE	d = 4;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIndentationModeTabsHonorsIndentationSize()
        {
            // The tab-stop math depends on IndentationSize: a clause body is placed on the first
            // tab stop strictly past the widest keyword, so the number of tabs between a keyword
            // and its body changes with the size, while each nested level is still exactly one tab.
            //
            // NOTE: unlike the other tests in this file, the two expected blocks below do NOT look
            // aligned when the file is viewed with the usual 4-column tab width. Their tab runs are
            // sized for IndentationSize 2 and 8, so they only line up when the editor's tab width is
            // set to 2 (and 8) respectively - which is exactly the tab-width/IndentationSize coupling
            // this test asserts.
            const string input = "BEGIN SELECT a, b FROM t WHERE a = 1 AND b = 2; END";

            // With a small tab width (2), the 5-6 character clause keywords need two tabs to clear
            // the widest keyword, and the select-list / predicate continuations land five tabs in.
            var optionsSize2 = new SqlScriptGeneratorOptions { IndentationMode = IndentationMode.Tabs, IndentationSize = 2 };
            const string expectedSize2 =
@"
BEGIN
	SELECT	a,
					b
	FROM		t
	WHERE		a = 1
					AND b = 2;
END";

            AssertGenerated(input, optionsSize2, expectedSize2);

            // With a wide tab width (8), a single tab already clears the widest keyword, so the
            // clause gaps collapse to one tab and the continuations land two tabs in.
            var optionsSize8 = new SqlScriptGeneratorOptions { IndentationMode = IndentationMode.Tabs, IndentationSize = 8 };
            const string expectedSize8 =
@"
BEGIN
	SELECT	a,
		b
	FROM	t
	WHERE	a = 1
		AND b = 2;
END";

            AssertGenerated(input, optionsSize8, expectedSize8);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIndentationModeTabsWithZeroIndentationSizeMatchesSpaces()
        {
            // IndentationSize = 0 means no tab can be emitted, so every tab path falls back to the
            // space layout. Tabs mode must then produce exactly the same output as Spaces mode -
            // including the column-definition layout for columns without constraints - and emit no
            // tab characters at all.
            foreach (string sql in new[]
            {
                "CREATE TABLE t (a INT NOT NULL, b INT, c INT DEFAULT 0);",
                "BEGIN SELECT a, b FROM t WHERE a = 1 AND b = 2; END",
            })
            {
                string tabs = Generate(sql, new SqlScriptGeneratorOptions { IndentationMode = IndentationMode.Tabs, IndentationSize = 0 });
                string spaces = Generate(sql, new SqlScriptGeneratorOptions { IndentationMode = IndentationMode.Spaces, IndentationSize = 0 });

                Assert.AreEqual(spaces, tabs, "Tabs mode with IndentationSize 0 should match Spaces mode. SQL:\n" + sql);
                Assert.IsFalse(tabs.Contains("\t"), "No tab characters should be emitted when IndentationSize is 0. SQL:\n" + sql);
            }
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIndentationModeTabsRegenerationIsStable()
        {
            // Regenerating a script that was already produced in Tabs mode must reproduce it
            // exactly (the generator normalizes whitespace, so tabs in the input do not perturb the
            // output). This guards against round-trip instability.
            var options = new SqlScriptGeneratorOptions { IndentationMode = IndentationMode.Tabs };

            foreach (string sql in new[]
            {
                "BEGIN SELECT a, b FROM t WHERE a = 1 AND b = 2; END",
                "CREATE TABLE t (a INT NOT NULL, b NVARCHAR(10) NULL, c INT DEFAULT 0);",
                "UPDATE t SET a = 1, b = 2, c = 3 WHERE d = 4;",
            })
            {
                string first = Generate(sql, options);
                string second = Generate(first, options);
                Assert.AreEqual(first, second, "Tabs-mode output should be stable when regenerated. First:\n" + first);
            }
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIndentationModeTabsSubqueryInWhereUsesTabs()
        {
            // A subquery nested in a WHERE predicate keeps aligning its own clause keywords with
            // tabs; the inner clauses are indented past the outer predicate on tab stops.
            const string input = "SELECT a FROM t WHERE a IN (SELECT b FROM u WHERE b > 1 AND c < 2);";
            var options = new SqlScriptGeneratorOptions { IndentationMode = IndentationMode.Tabs };
            const string expected =
@"
SELECT	a
FROM	t
WHERE	a IN (SELECT	b
				FROM		u
				WHERE		b > 1
					AND c < 2);";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIndentationModeTabsDerivedTableUsesTabs()
        {
            // A derived table in FROM with its own GROUP BY: the inner query's clause keywords and
            // select-list continuation are aligned with tabs, indented past the outer FROM.
            const string input = "SELECT o.total FROM (SELECT SUM(x) AS total, y FROM t1 GROUP BY y) AS o WHERE o.total > 10;";
            var options = new SqlScriptGeneratorOptions { IndentationMode = IndentationMode.Tabs };
            const string expected =
@"
SELECT	o.total
FROM	(SELECT		SUM(x) AS total,
					y
		FROM		t1
		GROUP BY	y) AS o
WHERE	o.total > 10;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIndentationModeTabsWithoutClauseBodyAlignmentStillIndentsWithTabs()
        {
            // With AlignClauseBodies off, the clause keyword and its body are separated by a single
            // space (no tab-stop snapping), but the structural indentation of each line is still
            // written with tabs, and continuation lines still round up to a tab stop.
            const string input = "BEGIN SELECT a, b FROM t WHERE a = 1 AND b = 2; END";
            var options = new SqlScriptGeneratorOptions { IndentationMode = IndentationMode.Tabs, AlignClauseBodies = false };
            const string expected =
@"
BEGIN
	SELECT a,
			b
	FROM t
	WHERE a = 1
			AND b = 2;
END";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIndentationModeTabsIndentsLeadingCommentWithTabs()
        {
            // The leading-whitespace-to-tabs pass also applies to a line that begins with a
            // preserved comment: the comment is indented with a tab, not spaces.
            const string input = "BEGIN /* c1 */ SELECT a FROM t; END";
            var options = new SqlScriptGeneratorOptions { IndentationMode = IndentationMode.Tabs, PreserveComments = true };
            const string expected =
@"
BEGIN
	/* c1 */
	SELECT	a
	FROM	t;
END";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIndentationModeTabsMultilineCommentDoesNotIndentFollowingContent()
        {
            // A multi-line block comment is a single token that CONTAINS newlines but ENDS with
            // "*/", so the leading-whitespace-to-tabs pass must not treat content that follows it on
            // the same line as a new line's indentation. Here the space in " + 2" after the comment
            // must stay a space and not be converted into a tab.
            const string input = "SELECT 1 /* a\r\nb */ + 2 FROM t;";
            var options = new SqlScriptGeneratorOptions { IndentationMode = IndentationMode.Tabs, PreserveComments = true };
            const string expected =
@"
SELECT	1 /* a
b */ + 2
FROM	t;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIndentationModeTabsAlignsToWidestClauseKeyword()
        {
            // When a query mixes short and long clause keywords (SELECT/FROM vs GROUP BY/ORDER BY),
            // every clause body is snapped to the same tab stop past the widest keyword, so the
            // shorter keywords are followed by two tabs and the widest by one.
            const string input = "SELECT a, b FROM t GROUP BY a, b HAVING COUNT(*) > 1 ORDER BY a;";
            var options = new SqlScriptGeneratorOptions { IndentationMode = IndentationMode.Tabs };
            const string expected =
@"
SELECT		a,
			b
FROM		t
GROUP BY	a, b
HAVING		COUNT(*) > 1
ORDER BY	a;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestColumnAliasStyleEqualsSignAlignsVaryingAliasAndColumnLengths()
        {
            // ColumnAliasStyle.EqualsSign in the default Spaces indentation mode: aliases and
            // source expressions of differing lengths, with the '=' signs padded with spaces so
            // they align to the widest alias.
            const string input = "SELECT a AS shortName, bb AS x, ccc AS mediumAlias FROM t;";
            var options = new SqlScriptGeneratorOptions { ColumnAliasStyle = ColumnAliasStyle.EqualsSign };
            const string expected =
@"
SELECT shortName   = a,
       x           = bb,
       mediumAlias = ccc
FROM   t;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIndentationModeTabsColumnAliasEqualsSignAlignsVaryingLengths()
        {
            // The same varying-length EqualsSign projection as the Spaces test above, but in Tabs
            // mode: the leading indentation and the gaps that align the aliases and '=' signs are
            // all rendered with tabs instead of padded spaces.
            const string input = "SELECT a AS shortName, bb AS x, ccc AS mediumAlias FROM t;";
            var options = new SqlScriptGeneratorOptions
            {
                ColumnAliasStyle = ColumnAliasStyle.EqualsSign,
                IndentationMode = IndentationMode.Tabs
            };
            const string expected =
@"
SELECT	shortName	= a,
		x			= bb,
		mediumAlias	= ccc
FROM	t;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIndentationModeTabsMostRequestedJoinStyleUsesTabs()
        {
            // Most-requested JOIN style (NewLineAfterJoinKeyword = false, NewLineBeforeOnClause =
            // true) in Tabs mode: the table source stays on the JOIN line and the ON keyword is
            // placed one indentation level past the JOIN, written with tab characters.
            const string input = "SELECT * FROM a INNER JOIN b ON a.x = b.x;";
            var options = new SqlScriptGeneratorOptions
            {
                IndentationMode = IndentationMode.Tabs,
                NewLineAfterJoinKeyword = false,
                NewLineBeforeOnClause = true
            };
            const string expected =
@"
SELECT	*
FROM	a
		INNER JOIN b
			ON a.x = b.x;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIndentationModeTabsMultiPredicateOnWrapsWithTabs()
        {
            // Multi-predicate ON clause in Tabs mode: the ON keyword is one level past the JOIN and
            // the wrapped AND predicate aligns under the first predicate, all using tab characters.
            const string input = "SELECT * FROM a INNER JOIN b ON a.x = b.x AND a.y = b.y;";
            var options = new SqlScriptGeneratorOptions
            {
                IndentationMode = IndentationMode.Tabs,
                NewLineAfterJoinKeyword = false,
                NewLineBeforeOnClause = true,
                MultilineWherePredicatesList = true
            };
            const string expected =
@"
SELECT	*
FROM	a
		INNER JOIN b
			ON a.x = b.x
				AND a.y = b.y;";

            AssertGenerated(input, options, expected);
        }

        // ---------------------------------------------------------------------------------------
        // Edge cases: empty / whitespace-only scripts and inputs padded with blank lines. The
        // generator reparses the AST and re-emits normalized whitespace, so blank lines from the
        // input never survive into the output, and Tabs mode never emits a tab for a line that has
        // no structural indentation.
        // ---------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIndentationModeTabsEmptyScriptProducesEmptyOutput()
        {
            var options = new SqlScriptGeneratorOptions { IndentationMode = IndentationMode.Tabs };

            AssertGenerated("", options, "");
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIndentationModeTabsWhitespaceOnlyScriptProducesEmptyOutput()
        {
            // A script that is nothing but spaces, tabs and blank lines produces no output at all -
            // in particular, no stray tab characters.
            const string input = "   \r\n\t\r\n  \t  \r\n";
            var options = new SqlScriptGeneratorOptions { IndentationMode = IndentationMode.Tabs };

            AssertGenerated(input, options, "");
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIndentationModeTabsCollapsesBlankLinesBetweenStatements()
        {
            // Many blank lines between two statements collapse to the single blank line implied by
            // NumNewlinesAfterStatement. Both statements are single-line, so no tab is emitted for
            // structural indentation (only the clause-keyword gap is a tab).
            const string input = "SELECT 1;\r\n\r\n\r\n\r\n\r\nSELECT 2;";
            var options = new SqlScriptGeneratorOptions { IndentationMode = IndentationMode.Tabs };
            const string expected =
@"
SELECT	1;

SELECT	2;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIndentationModeTabsBlankLinesWithinStatementAreNormalized()
        {
            // Blank lines sprinkled inside a single statement are discarded; the statement is
            // reformatted with tab indentation as if the blank lines were never there.
            const string input = "SELECT a,\r\n\r\n\r\nb\r\n\r\nFROM t\r\nWHERE x = 1;";
            var options = new SqlScriptGeneratorOptions { IndentationMode = IndentationMode.Tabs };
            const string expected =
@"
SELECT	a,
		b
FROM	t
WHERE	x = 1;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIndentationModeTabsExtraSpacesWithinStatementAreNormalized()
        {
            // Runs of extra spaces (leading, trailing and between tokens) are collapsed and the
            // statement is re-emitted with tab indentation, producing the same output as the
            // well-formatted equivalent.
            const string input = "      SELECT     a,        b       FROM      t     WHERE      x   =   1;      ";
            var options = new SqlScriptGeneratorOptions { IndentationMode = IndentationMode.Tabs };
            const string expected =
@"
SELECT	a,
		b
FROM	t
WHERE	x = 1;";

            AssertGenerated(input, options, expected);
        }
    }
}
