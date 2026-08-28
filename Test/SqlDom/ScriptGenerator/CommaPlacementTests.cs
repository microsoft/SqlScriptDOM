//------------------------------------------------------------------------------
// <copyright file="CommaPlacementTests.cs" company="Microsoft">
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
    public class CommaPlacementTests
    {
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCommaPlacementDefaultIsTrailing()
        {
            Assert.AreEqual(CommaPlacement.Trailing, new SqlScriptGeneratorOptions().CommaPlacement);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCommaPlacementLeadingSelectList()
        {
            const string input = "SELECT a, b, c FROM t;";
            var options = new SqlScriptGeneratorOptions { CommaPlacement = CommaPlacement.Leading };
            const string expected = @"
SELECT a
     , b
     , c
FROM   t;";
            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCommaPlacementTrailingSelectList()
        {
            const string input = "SELECT a, b, c FROM t;";
            var options = new SqlScriptGeneratorOptions { CommaPlacement = CommaPlacement.Trailing };
            const string expected = @"
SELECT a,
       b,
       c
FROM   t;";
            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCommaPlacementLeadingParenthesizedList()
        {
            const string input = "CREATE TABLE t (a INT, b INT, c INT);";
            var options = new SqlScriptGeneratorOptions { CommaPlacement = CommaPlacement.Leading };
            const string expected = @"
CREATE TABLE t (
    a INT
  , b INT
  , c INT
);";
            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCommaPlacementTrailingParenthesizedList()
        {
            const string input = "CREATE TABLE t (a INT, b INT, c INT);";
            var options = new SqlScriptGeneratorOptions { CommaPlacement = CommaPlacement.Trailing };
            const string expected = @"
CREATE TABLE t (
    a INT,
    b INT,
    c INT
);";
            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCommaPlacementLeadingInsertTargets()
        {
            const string input = "INSERT INTO t (a, b, c) VALUES (1, 2, 3);";
            var options = new SqlScriptGeneratorOptions
            {
                CommaPlacement = CommaPlacement.Leading,
                MultilineInsertTargetsList = true,
            };
            const string expected = @"
INSERT  INTO t (
    a
  , b
  , c
)
VALUES         (1, 2, 3);";
            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCommaPlacementTrailingInsertTargets()
        {
            const string input = "INSERT INTO t (a, b, c) VALUES (1, 2, 3);";
            var options = new SqlScriptGeneratorOptions
            {
                CommaPlacement = CommaPlacement.Trailing,
                MultilineInsertTargetsList = true,
            };
            const string expected = @"
INSERT  INTO t (
    a,
    b,
    c
)
VALUES         (1, 2, 3);";
            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCommaPlacementLeadingInsertSources()
        {
            const string input = "INSERT INTO t (a, b, c) VALUES (1, 2, 3), (4, 5, 6), (7, 8, 9);";
            var options = new SqlScriptGeneratorOptions
            {
                CommaPlacement = CommaPlacement.Leading,
                MultilineInsertSourcesList = true,
                MultilineInsertTargetsList = true,
            };
            const string expected = @"
INSERT  INTO t (
    a
  , b
  , c
)
VALUES         (1, 2, 3)
             , (4, 5, 6)
             , (7, 8, 9);";
            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCommaPlacementTrailingInsertSources()
        {
            const string input = "INSERT INTO t (a, b, c) VALUES (1, 2, 3), (4, 5, 6), (7, 8, 9);";
            var options = new SqlScriptGeneratorOptions
            {
                CommaPlacement = CommaPlacement.Trailing,
                MultilineInsertSourcesList = true,
            };
            const string expected = @"
INSERT  INTO t (a, b, c)
VALUES         (1, 2, 3),
               (4, 5, 6),
               (7, 8, 9);";
            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCommaPlacementLeadingViewColumns()
        {
            const string input = "CREATE VIEW v (a, b, c) AS SELECT 1, 2, 3;";
            var options = new SqlScriptGeneratorOptions
            {
                CommaPlacement = CommaPlacement.Leading,
                MultilineViewColumnsList = true,
            };
            const string expected = @"
CREATE VIEW v (
    a
  , b
  , c
)
AS
SELECT 1
     , 2
     , 3;";
            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCommaPlacementTrailingViewColumns()
        {
            const string input = "CREATE VIEW v (a, b, c) AS SELECT 1, 2, 3;";
            var options = new SqlScriptGeneratorOptions
            {
                CommaPlacement = CommaPlacement.Trailing,
                MultilineViewColumnsList = true,
            };
            const string expected = @"
CREATE VIEW v (
    a,
    b,
    c
)
AS
SELECT 1,
       2,
       3;";
            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCommaPlacementLeadingSetClauseItems()
        {
            const string input = "UPDATE t SET a = 1, b = 2, c = 3;";
            var options = new SqlScriptGeneratorOptions
            {
                CommaPlacement = CommaPlacement.Leading,
                MultilineSetClauseItems = true,
            };
            const string expected = @"
UPDATE t
SET    a = 1
     , b = 2
     , c = 3;";
            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCommaPlacementTrailingSetClauseItems()
        {
            const string input = "UPDATE t SET a = 1, b = 2, c = 3;";
            var options = new SqlScriptGeneratorOptions
            {
                CommaPlacement = CommaPlacement.Trailing,
                MultilineSetClauseItems = true,
            };
            const string expected = @"
UPDATE t
SET    a = 1,
       b = 2,
       c = 3;";
            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCommaPlacementLeadingSingleColumnHasNoComma()
        {
            const string input = "SELECT a FROM t;";
            var options = new SqlScriptGeneratorOptions { CommaPlacement = CommaPlacement.Leading };
            const string expected = @"
SELECT a
FROM   t;";
            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCommaPlacementLeadingWithPreserveCommentsDoesNotAbsorbComma()
        {
            // A line comment after an element must not absorb the following leading comma.
            const string input = @"SELECT col1, -- first column
       col2, -- second column
       col3
FROM t;";
            var options = new SqlScriptGeneratorOptions
            {
                CommaPlacement = CommaPlacement.Leading,
                PreserveComments = true,
            };
            const string expected = @"
SELECT col1 -- first column
     , col2 -- second column
     , col3
FROM   t;";
            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCommaPlacementTrailingWithPreserveCommentsKeepsCommentsAfterComma()
        {
            const string input = @"SELECT col1, -- first column
       col2, -- second column
       col3
FROM t;";
            var options = new SqlScriptGeneratorOptions
            {
                CommaPlacement = CommaPlacement.Trailing,
                PreserveComments = true,
            };
            const string expected = @"
SELECT col1, -- first column
       col2, -- second column
       col3
FROM   t;";
            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCommaPlacementTrailingWithCommentBeforeCommaForcesNewLine()
        {
            const string input = @"SELECT col1 -- first column
, col2
FROM t;";
            var options = new SqlScriptGeneratorOptions
            {
                CommaPlacement = CommaPlacement.Trailing,
                PreserveComments = true,
            };
            const string expected = @"
SELECT col1, -- first column
       col2
FROM   t;";
            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCommaPlacementLeadingSelectListMultilineFalse()
        {
            const string input = "SELECT a, b, c FROM t;";
            var options = new SqlScriptGeneratorOptions
            {
                CommaPlacement = CommaPlacement.Leading,
                MultilineSelectElementsList = false,
            };
            const string expected = @"
SELECT a, b, c
FROM   t;";
            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCommaPlacementLeadingViewColumnsMultilineFalse()
        {
            const string input = "CREATE VIEW v (a, b, c) AS SELECT 1, 2, 3;";
            var options = new SqlScriptGeneratorOptions
            {
                CommaPlacement = CommaPlacement.Leading,
                MultilineViewColumnsList = false,
                MultilineSelectElementsList = false,
            };
            const string expected = @"
CREATE VIEW v (a, b, c)
AS
SELECT 1, 2, 3;";
            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCommaPlacementLeadingSetClauseItemsMultilineFalse()
        {
            const string input = "UPDATE t SET a = 1, b = 2, c = 3;";
            var options = new SqlScriptGeneratorOptions
            {
                CommaPlacement = CommaPlacement.Leading,
                MultilineSetClauseItems = false,
            };
            const string expected = @"
UPDATE t
SET    a = 1, b = 2, c = 3;";
            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCommaPlacementLeadingInsertTargetsMultilineFalse()
        {
            const string input = "INSERT INTO t (a, b, c) VALUES (1, 2, 3);";
            var options = new SqlScriptGeneratorOptions
            {
                CommaPlacement = CommaPlacement.Leading,
                MultilineInsertTargetsList = false,
            };
            const string expected = @"
INSERT  INTO t (a, b, c)
VALUES         (1, 2, 3);";
            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCommaPlacementLeadingInsertSourcesMultilineFalse()
        {
            // VALUES rows remain multiline even when MultilineInsertSourcesList is false.
            const string input = "INSERT INTO t (a, b, c) VALUES (1, 2, 3), (4, 5, 6), (7, 8, 9);";
            var options = new SqlScriptGeneratorOptions
            {
                CommaPlacement = CommaPlacement.Leading,
                MultilineInsertSourcesList = false,
            };
            const string expected = @"
INSERT  INTO t (a, b, c)
VALUES         (1, 2, 3)
             , (4, 5, 6)
             , (7, 8, 9);";
            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCommaPlacementLeadingParenthesizedListAlwaysMultiline()
        {
            // CREATE TABLE columns have no single-line toggle.
            const string input = "CREATE TABLE t (a INT, b INT, c INT);";
            var options = new SqlScriptGeneratorOptions { CommaPlacement = CommaPlacement.Leading };
            const string expected = @"
CREATE TABLE t (
    a INT
  , b INT
  , c INT
);";
            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCommaPlacementLeadingIndentedOptionList()
        {
            // Exercises the indented comma-list path used by WITH parameters.
            const string input = "CREATE COLUMN MASTER KEY CMK1 WITH (KEY_STORE_PROVIDER_NAME = 'MSSQL_CERTIFICATE_STORE', KEY_PATH = 'some/path');";
            var options = new SqlScriptGeneratorOptions { CommaPlacement = CommaPlacement.Leading };
            const string expected = @"
CREATE COLUMN MASTER KEY CMK1
WITH (
     KEY_STORE_PROVIDER_NAME = 'MSSQL_CERTIFICATE_STORE'
  ,  KEY_PATH = 'some/path'
);";
            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCommaPlacementLeadingSetClauseItemsIndented()
        {
            const string input = "UPDATE t SET a = 1, b = 2, c = 3;";
            var options = new SqlScriptGeneratorOptions
            {
                CommaPlacement = CommaPlacement.Leading,
                MultilineSetClauseItems = true,
                IndentSetClause = true,
            };
            const string expected = @"
UPDATE  t
    SET a = 1
      , b = 2
      , c = 3;";
            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCommaPlacementLeadingCreateTableWithComments()
        {
            // Exercises preserved line comments in the indented leading-comma path.
            const string input = @"CREATE TABLE t (a INT, -- first
b INT, -- second
c INT);";
            var options = new SqlScriptGeneratorOptions
            {
                CommaPlacement = CommaPlacement.Leading,
                PreserveComments = true,
            };
            const string expected = @"
CREATE TABLE t (
    a INT -- first
  , b INT -- second
  , c INT
);";
            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCommaPlacementLeadingWithBlockCommentTrailingElement()
        {
            // Block comments use the inline path rather than the deferred line-comment path.
            const string input = "SELECT a /* note */, b, c FROM t;";
            var options = new SqlScriptGeneratorOptions
            {
                CommaPlacement = CommaPlacement.Leading,
                PreserveComments = true,
            };
            const string expected = @"
SELECT a /* note */
     , b
     , c
FROM   t;";
            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCommaPlacementLeadingSpaceCountDefaultIsOne()
        {
            Assert.AreEqual(1, new SqlScriptGeneratorOptions().LeadingCommaSpaceCount);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCommaPlacementLeadingSpaceCountAboveOneClampsToOne()
        {
            Assert.AreEqual(1, new SqlScriptGeneratorOptions { LeadingCommaSpaceCount = 2 }.LeadingCommaSpaceCount);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestLeadingCommaSpaceCountZeroInSelectList()
        {
            const string input = "SELECT a, b, c FROM t;";
            var options = new SqlScriptGeneratorOptions
            {
                CommaPlacement = CommaPlacement.Leading,
                LeadingCommaSpaceCount = 0,
            };
            const string expected = @"
SELECT a
      ,b
      ,c
FROM   t;";
            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestLeadingCommaSpaceCountZeroInCreateTableColumns()
        {
            const string input = "CREATE TABLE t (a INT, b INT, c INT);";
            var options = new SqlScriptGeneratorOptions
            {
                CommaPlacement = CommaPlacement.Leading,
                LeadingCommaSpaceCount = 0,
            };
            const string expected = @"
CREATE TABLE t (
    a INT
   ,b INT
   ,c INT
);";
            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCommaPlacementLeadingSpaceCountOne()
        {
            var options = new SqlScriptGeneratorOptions
            {
                CommaPlacement = CommaPlacement.Leading,
                LeadingCommaSpaceCount = 1,
            };
            const string selectInput = "SELECT a, b, c FROM t;";
            const string selectExpected = @"
SELECT a
     , b
     , c
FROM   t;";
            AssertGenerated(selectInput, options, selectExpected);

            const string tableInput = "CREATE TABLE t (a INT, b INT, c INT);";
            const string tableExpected = @"
CREATE TABLE t (
    a INT
  , b INT
  , c INT
);";
            AssertGenerated(tableInput, options, tableExpected);
        }
    }
}
