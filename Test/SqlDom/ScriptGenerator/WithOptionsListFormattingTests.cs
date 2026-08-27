//------------------------------------------------------------------------------
// <copyright file="WithOptionsListFormattingTests.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using Microsoft.SqlServer.TransactSql.ScriptDom;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlStudio.Tests.AssemblyTools.TestCategory;
using static SqlStudio.Tests.UTSqlScriptDom.ScriptGeneratorTestHelper;

namespace SqlStudio.Tests.UTSqlScriptDom
{
    // Tests for the MultilineWithOptionsList script-generation option, which controls whether the
    // options in a WITH clause (index options, table hints, BACKUP/RESTORE options) or an OPTION
    // clause (query hints) are written on a single line (default) or one per line. Kept in a
    // dedicated file to avoid churn in ScriptGeneratorTests.cs.
    //
    // Work item: Formatter option: WITH clause options width
    [TestClass]
    public class WithOptionsListFormattingTests
    {
        // Builds options that isolate the WITH/OPTION option-list layout: clause bodies are not
        // aligned and clauses are not broken onto their own lines, so the surrounding statement stays
        // compact and the expectations focus on the option list itself.
        private static SqlScriptGeneratorOptions MakeOptions(bool multilineWithOptionsList)
        {
            return new SqlScriptGeneratorOptions
            {
                MultilineWithOptionsList = multilineWithOptionsList,
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
        public void TestMultilineWithOptionsListDefaultIsFalse()
        {
            Assert.IsFalse(new SqlScriptGeneratorOptions().MultilineWithOptionsList);
        }

        // -----------------------------------------------------------------------------------------
        // CREATE INDEX (parenthesized index options)
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestDefaultKeepsCreateIndexOptionsOnSingleLine()
        {
            const string input = "CREATE INDEX i1 ON t1 (c1) WITH (PAD_INDEX = ON, FILLFACTOR = 50);";
            var options = MakeOptions(false);
            const string expected =
@"
CREATE INDEX i1
    ON t1(c1) WITH (PAD_INDEX = ON, FILLFACTOR = 50);";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineCreateIndexOptionsTrailingComma()
        {
            const string input = "CREATE INDEX i1 ON t1 (c1) WITH (PAD_INDEX = ON, FILLFACTOR = 50);";
            var options = MakeOptions(true);
            const string expected =
@"
CREATE INDEX i1
    ON t1(c1) WITH (
    PAD_INDEX = ON,
    FILLFACTOR = 50
);";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineCreateIndexOptionsLeadingComma()
        {
            // CommaPlacement = Leading applies within the options list: each option sits at the list
            // indentation level and the comma is indented two characters fewer.
            const string input = "CREATE INDEX i1 ON t1 (c1) WITH (PAD_INDEX = ON, FILLFACTOR = 50);";
            var options = MakeOptions(true);
            options.CommaPlacement = CommaPlacement.Leading;
            const string expected =
@"
CREATE INDEX i1
    ON t1(c1) WITH (
    PAD_INDEX = ON
  , FILLFACTOR = 50
);";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineCreateIndexOptionsNewLineBeforeOpenParenthesis()
        {
            // NewLineBeforeOpenParenthesisInMultilineList moves the open parenthesis onto its own line.
            const string input = "CREATE INDEX i1 ON t1 (c1) WITH (PAD_INDEX = ON, FILLFACTOR = 50);";
            var options = MakeOptions(true);
            options.NewLineBeforeOpenParenthesisInMultilineList = true;
            const string expected =
@"
CREATE INDEX i1
    ON t1(c1) WITH
(
    PAD_INDEX = ON,
    FILLFACTOR = 50
);";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineCreateIndexOptionsCloseParenthesisOnLastItemLine()
        {
            // NewLineBeforeCloseParenthesisInMultilineList = false keeps the close parenthesis on the
            // same line as the last option instead of on its own line.
            const string input = "CREATE INDEX i1 ON t1 (c1) WITH (PAD_INDEX = ON, FILLFACTOR = 50);";
            var options = MakeOptions(true);
            options.NewLineBeforeCloseParenthesisInMultilineList = false;
            const string expected =
@"
CREATE INDEX i1
    ON t1(c1) WITH (
    PAD_INDEX = ON,
    FILLFACTOR = 50);";

            AssertGenerated(input, options, expected);
        }

        // -----------------------------------------------------------------------------------------
        // ALTER INDEX ... REBUILD (index options WITHOUT a space before the parenthesis by default)
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestDefaultKeepsAlterIndexRebuildOptionsOnSingleLine()
        {
            // The default single-line ALTER INDEX ... REBUILD form emits WITH(...) with no space
            // before the open parenthesis; enabling the option must not change the default output.
            const string input = "ALTER INDEX i1 ON t1 REBUILD WITH (ONLINE = ON, MAXDOP = 2);";
            var options = MakeOptions(false);
            const string expected =
@"
ALTER INDEX i1
    ON t1 REBUILD WITH(ONLINE = ON, MAXDOP = 2);";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineAlterIndexRebuildOptions()
        {
            const string input = "ALTER INDEX i1 ON t1 REBUILD WITH (ONLINE = ON, MAXDOP = 2);";
            var options = MakeOptions(true);
            const string expected =
@"
ALTER INDEX i1
    ON t1 REBUILD WITH (
    ONLINE = ON,
    MAXDOP = 2
);";

            AssertGenerated(input, options, expected);
        }

        // -----------------------------------------------------------------------------------------
        // ALTER TABLE ... REBUILD (index options)
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineAlterTableRebuildOptions()
        {
            const string input = "ALTER TABLE t1 REBUILD WITH (PAD_INDEX = ON, FILLFACTOR = 50);";
            var options = MakeOptions(true);
            const string expected =
@"
ALTER TABLE t1 REBUILD WITH (
    PAD_INDEX = ON,
    FILLFACTOR = 50
);";

            AssertGenerated(input, options, expected);
        }

        // -----------------------------------------------------------------------------------------
        // Table hints (WITH (...))
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestDefaultKeepsTableHintsOnSingleLine()
        {
            const string input = "SELECT * FROM t1 WITH (NOLOCK, INDEX (i1));";
            var options = MakeOptions(false);
            const string expected = "SELECT * FROM t1 WITH (NOLOCK, INDEX (i1));";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineTableHints()
        {
            // The table hints align under the table reference, with each hint indented one level past
            // that alignment point.
            const string input = "SELECT * FROM t1 WITH (NOLOCK, INDEX (i1));";
            var options = MakeOptions(true);
            const string expected =
@"
SELECT * FROM t1 WITH (
                  NOLOCK,
                  INDEX (i1)
              );";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineTableHintsLeadingComma()
        {
            const string input = "SELECT * FROM t1 WITH (NOLOCK, INDEX (i1));";
            var options = MakeOptions(true);
            options.CommaPlacement = CommaPlacement.Leading;
            const string expected =
@"
SELECT * FROM t1 WITH (
                  NOLOCK
                , INDEX (i1)
              );";

            AssertGenerated(input, options, expected);
        }

        // -----------------------------------------------------------------------------------------
        // Query hints (OPTION (...))
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestDefaultKeepsQueryHintsOnSingleLine()
        {
            const string input = "SELECT * FROM t1 OPTION (RECOMPILE, MAXDOP 2);";
            var options = MakeOptions(false);
            const string expected =
@"
SELECT * FROM t1
OPTION (RECOMPILE, MAXDOP 2);";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineQueryHints()
        {
            const string input = "SELECT * FROM t1 OPTION (RECOMPILE, MAXDOP 2);";
            var options = MakeOptions(true);
            const string expected =
@"
SELECT * FROM t1
OPTION (
    RECOMPILE,
    MAXDOP 2
);";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineQueryHintsLeadingComma()
        {
            const string input = "SELECT * FROM t1 OPTION (RECOMPILE, MAXDOP 2);";
            var options = MakeOptions(true);
            options.CommaPlacement = CommaPlacement.Leading;
            const string expected =
@"
SELECT * FROM t1
OPTION (
    RECOMPILE
  , MAXDOP 2
);";

            AssertGenerated(input, options, expected);
        }

        // -----------------------------------------------------------------------------------------
        // BACKUP (non-parenthesized WITH options)
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestDefaultKeepsBackupOptionsOnSingleLine()
        {
            const string input = "BACKUP DATABASE d1 TO DISK = 'd:' WITH BLOCKSIZE = 10, CHECKSUM;";
            var options = MakeOptions(false);
            const string expected =
@"
BACKUP DATABASE d1
    TO DISK = 'd:'
    WITH BLOCKSIZE = 10, CHECKSUM;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineBackupOptions()
        {
            // Each option is written on its own line, indented one level from the statement keyword
            // (aligned beneath the WITH keyword).
            const string input = "BACKUP DATABASE d1 TO DISK = 'd:' WITH BLOCKSIZE = 10, CHECKSUM;";
            var options = MakeOptions(true);
            const string expected =
@"
BACKUP DATABASE d1
    TO DISK = 'd:'
    WITH
    BLOCKSIZE = 10,
    CHECKSUM;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineBackupOptionsLeadingComma()
        {
            const string input = "BACKUP DATABASE d1 TO DISK = 'd:' WITH BLOCKSIZE = 10, CHECKSUM;";
            var options = MakeOptions(true);
            options.CommaPlacement = CommaPlacement.Leading;
            const string expected =
@"
BACKUP DATABASE d1
    TO DISK = 'd:'
    WITH
    BLOCKSIZE = 10
  , CHECKSUM;";

            AssertGenerated(input, options, expected);
        }

        // -----------------------------------------------------------------------------------------
        // RESTORE (non-parenthesized WITH options)
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestDefaultKeepsRestoreOptionsOnSingleLine()
        {
            const string input = "RESTORE DATABASE db1 FROM DISK = 'z:' WITH REPLACE, RECOVERY;";
            var options = MakeOptions(false);
            const string expected =
@"
RESTORE DATABASE db1 FROM DISK = 'z:'
    WITH REPLACE, RECOVERY;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineRestoreOptions()
        {
            const string input = "RESTORE DATABASE db1 FROM DISK = 'z:' WITH REPLACE, RECOVERY;";
            var options = MakeOptions(true);
            const string expected =
@"
RESTORE DATABASE db1 FROM DISK = 'z:'
    WITH
    REPLACE,
    RECOVERY;";

            AssertGenerated(input, options, expected);
        }

        // -----------------------------------------------------------------------------------------
        // CREATE SELECTIVE XML INDEX (trailing WITH index options)
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineSelectiveXmlIndexOptions()
        {
            const string input =
                "CREATE SELECTIVE XML INDEX sxi1 ON t1 (c1) " +
                "FOR (path1 = '/a/b/c' AS SQL NVARCHAR(50)) " +
                "WITH (DROP_EXISTING = ON, FILLFACTOR = 2);";
            var options = MakeOptions(true);
            const string expected =
@"
CREATE SELECTIVE XML INDEX sxi1 ON t1(c1)
FOR(    path1 = '/a/b/c' AS SQL NVARCHAR (50)
)
WITH (
    DROP_EXISTING = ON,
    FILLFACTOR = 2
);";

            AssertGenerated(input, options, expected);
        }

        // -----------------------------------------------------------------------------------------
        // Indentation: when IndentationMode is Tabs, the multiline option list is indented with tab
        // characters rather than spaces.
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineCreateIndexOptionsWithTabIndentation()
        {
            const string input = "CREATE INDEX i1 ON t1 (c1) WITH (PAD_INDEX = ON, FILLFACTOR = 50);";
            var options = MakeOptions(true);
            options.IndentationMode = IndentationMode.Tabs;
            // Explicit \t escapes (regular string, not verbatim) so the tab alignment characters are
            // visible in source rather than hidden inside the literal.
            const string expected =
                "CREATE INDEX i1\n" +
                "\tON t1(c1) WITH (\n" +
                "\tPAD_INDEX = ON,\n" +
                "\tFILLFACTOR = 50\n" +
                ");";

            AssertGenerated(input, options, expected);
        }

        // -----------------------------------------------------------------------------------------
        // Real-world: a production online index build whose long WITH option list is exactly the case
        // this formatter option targets. One option per line makes the options readable and diffable.
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineProductionOnlineIndexBuild()
        {
            const string input =
                "CREATE NONCLUSTERED INDEX IX_SalesOrderHeader_CustomerID " +
                "ON Sales.SalesOrderHeader (CustomerID) " +
                "INCLUDE (OrderDate, TotalDue) " +
                "WITH (PAD_INDEX = ON, FILLFACTOR = 80, ONLINE = ON, DATA_COMPRESSION = PAGE);";
            var options = MakeOptions(true);
            const string expected =
@"
CREATE NONCLUSTERED INDEX IX_SalesOrderHeader_CustomerID
    ON Sales.SalesOrderHeader(CustomerID)
    INCLUDE(OrderDate, TotalDue) WITH (
    PAD_INDEX = ON,
    FILLFACTOR = 80,
    ONLINE = ON,
    DATA_COMPRESSION = PAGE
);";

            AssertGenerated(input, options, expected);
        }

        // -----------------------------------------------------------------------------------------
        // Shared GenerateIndexOptions entry points: inline index definitions and UNIQUE constraints
        // in CREATE TABLE, and ALTER TABLE ... ALTER INDEX, all funnel through the same virtual
        // GenerateIndexOptions method as CREATE INDEX, so the option applies to them too. These
        // guard that the shared path stays multiline through those distinct entry points.
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineInlineIndexDefinitionOptions()
        {
            const string input =
                "CREATE TABLE t1 (c1 INT, INDEX ix1 NONCLUSTERED (c1) WITH (PAD_INDEX = ON, FILLFACTOR = 50));";
            var options = MakeOptions(true);
            const string expected =
@"
CREATE TABLE t1 (
    c1 INT,
    INDEX ix1 NONCLUSTERED (c1) WITH (
    PAD_INDEX = ON,
    FILLFACTOR = 50
)
);";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineUniqueConstraintOptions()
        {
            const string input =
                "CREATE TABLE t1 (c1 INT, CONSTRAINT uq1 UNIQUE (c1) WITH (PAD_INDEX = ON, FILLFACTOR = 50));";
            var options = MakeOptions(true);
            const string expected =
@"
CREATE TABLE t1 (
    c1 INT,
    CONSTRAINT uq1 UNIQUE (c1) WITH (
    PAD_INDEX = ON,
    FILLFACTOR = 50
)
);";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestDefaultKeepsAlterTableAlterIndexOptionsOnSingleLine()
        {
            // Guards the default (option off) rendering through the shared GenerateIndexOptions
            // virtual chain for the ALTER TABLE ... ALTER INDEX entry point: WITH ( with a space.
            const string input =
                "ALTER TABLE t1 ALTER INDEX i1 REBUILD WITH (BUCKET_COUNT = 1);";
            var options = MakeOptions(false);
            const string expected =
@"
ALTER TABLE t1 ALTER INDEX i1 REBUILD WITH (BUCKET_COUNT = 1);";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineAlterTableAlterIndexOptions()
        {
            const string input =
                "ALTER TABLE t1 ALTER INDEX i1 REBUILD WITH (BUCKET_COUNT = 1);";
            var options = MakeOptions(true);
            const string expected =
@"
ALTER TABLE t1 ALTER INDEX i1 REBUILD WITH (
    BUCKET_COUNT = 1
);";

            AssertGenerated(input, options, expected);
        }

        // -----------------------------------------------------------------------------------------
        // Interaction: combining MultilineWithOptionsList with another multiline layout option
        // (MultilineSelectElementsList) must format both lists independently without interfering.
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineWithOptionsListCombinedWithMultilineSelectElements()
        {
            const string input =
                "SELECT c1, c2 FROM t1 OPTION (RECOMPILE, MAXDOP 2);";
            var options = MakeOptions(true);
            options.MultilineSelectElementsList = true;
            const string expected =
@"
SELECT c1,
       c2 FROM t1
OPTION (
    RECOMPILE,
    MAXDOP 2
);";

            AssertGenerated(input, options, expected);
        }

        // -----------------------------------------------------------------------------------------
        // Scope: the option must NOT affect the WITH keyword of a common table expression.
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineDoesNotAffectCommonTableExpression()
        {
            const string input = "WITH cte AS (SELECT 1 AS c) SELECT * FROM cte;";
            var options = MakeOptions(true);
            const string expected =
@"
WITH cte
AS (SELECT 1 AS c)
SELECT * FROM cte;";

            AssertGenerated(input, options, expected);
        }
    }
}
