//------------------------------------------------------------------------------
// <copyright file="ScriptGeneratorTests.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlStudio.Tests.AssemblyTools.TestCategory;

namespace SqlStudio.Tests.UTSqlScriptDom
{
    // These tests ensure that we get the correct SqlVersion for each type of SqlScriptGenerator's Options
    [TestClass]
    public class SqlScriptGeneratorTests
    {
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestSql100ScriptGenerator()
        {
            var options = new SqlScriptGeneratorOptions();
            var scriptGenerator = new Sql100ScriptGenerator(options);
            Assert.AreEqual(SqlVersion.Sql100, scriptGenerator.Options.SqlVersion);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestSql110ScriptGenerator()
        {
            var options = new SqlScriptGeneratorOptions();
            var scriptGenerator = new Sql110ScriptGenerator(options);
            Assert.AreEqual(SqlVersion.Sql110, scriptGenerator.Options.SqlVersion);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestSql120ScriptGenerator()
        {
            var options = new SqlScriptGeneratorOptions();
            var scriptGenerator = new Sql120ScriptGenerator(options);
            Assert.AreEqual(SqlVersion.Sql120, scriptGenerator.Options.SqlVersion);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestSql130ScriptGenerator()
        {
            var options = new SqlScriptGeneratorOptions();
            var scriptGenerator = new Sql130ScriptGenerator(options);
            Assert.AreEqual(SqlVersion.Sql130, scriptGenerator.Options.SqlVersion);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestSql140ScriptGenerator()
        {
            var options = new SqlScriptGeneratorOptions();
            var scriptGenerator = new Sql140ScriptGenerator(options);
            Assert.AreEqual(SqlVersion.Sql140, scriptGenerator.Options.SqlVersion);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestSql150ScriptGenerator()
        {
            var options = new SqlScriptGeneratorOptions();
            var scriptGenerator = new Sql150ScriptGenerator(options);
            Assert.AreEqual(SqlVersion.Sql150, scriptGenerator.Options.SqlVersion);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestSql160ScriptGenerator()
        {
            var options = new SqlScriptGeneratorOptions();
            var scriptGenerator = new Sql160ScriptGenerator(options);
            Assert.AreEqual(SqlVersion.Sql160, scriptGenerator.Options.SqlVersion);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestSql80ScriptGenerator()
        {
            var options = new SqlScriptGeneratorOptions();
            var scriptGenerator = new Sql80ScriptGenerator(options);
            Assert.AreEqual(SqlVersion.Sql80, scriptGenerator.Options.SqlVersion);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestSql90ScriptGenerator()
        {
            var options = new SqlScriptGeneratorOptions();
            var scriptGenerator = new Sql90ScriptGenerator(options);
            Assert.AreEqual(SqlVersion.Sql90, scriptGenerator.Options.SqlVersion);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestSqlServerlessScriptGenerator()
        {
            var options = new SqlScriptGeneratorOptions();
            var scriptGenerator = new SqlServerlessScriptGenerator(options);
            Assert.AreEqual(SqlVersion.Sql160, scriptGenerator.Options.SqlVersion);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestNewlinesBetweenStatementsGeneratorOption() {
            var tableName = new SchemaObjectName();
            tableName.Identifiers.Add(new Identifier { Value = "TableName" });

            var tableStatement = new CreateTableStatement
            {
                SchemaObjectName = tableName
            };
            var tableStatementString = "CREATE TABLE TableName;";

            var statements = new StatementList();
            statements.Statements.Add(tableStatement);
            statements.Statements.Add(tableStatement);

            var generatorOptions = new SqlScriptGeneratorOptions {
                KeywordCasing = KeywordCasing.Uppercase,
                IncludeSemicolons = true,
                NumNewlinesAfterStatement = 0
            };

            var generator = new Sql80ScriptGenerator(generatorOptions);

            generator.GenerateScript(statements, out var sql);

            Assert.AreEqual(tableStatementString + tableStatementString, sql);

            generatorOptions.NumNewlinesAfterStatement = 1;
            generator = new Sql80ScriptGenerator(generatorOptions);

            generator.GenerateScript(statements, out sql);

            Assert.AreEqual(tableStatementString + Environment.NewLine + tableStatementString, sql);

            generatorOptions.NumNewlinesAfterStatement = 2;
            generator = new Sql80ScriptGenerator(generatorOptions);

            generator.GenerateScript(statements, out sql);
            Assert.AreEqual(tableStatementString + Environment.NewLine + Environment.NewLine + tableStatementString, sql);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestNewLineFormattedIndexDefinitionDefault() {
            Assert.AreEqual(false, new SqlScriptGeneratorOptions().NewLineFormattedIndexDefinition);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestNewlineFormattedCheckConstraintDefault() {
            Assert.AreEqual(false, new SqlScriptGeneratorOptions().NewlineFormattedCheckConstraint);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestSpaceBetweenDataTypeAndParametersDefault() {
            Assert.AreEqual(true, new SqlScriptGeneratorOptions().SpaceBetweenDataTypeAndParameters);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestSpaceBetweenParametersInDataTypeDefault() {
            Assert.AreEqual(true, new SqlScriptGeneratorOptions().SpaceBetweenParametersInDataType);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestSpaceBetweenDataTypeAndParametersWhenFalse() {
            var expectedSqlText = @"CREATE TABLE DummyTable (
    ColumnName VARCHAR(50)
);";

            ParseAndAssertEquality(expectedSqlText, new SqlScriptGeneratorOptions {
                SpaceBetweenDataTypeAndParameters = false
            });
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestSpaceBetweenDataTypeAndParametersWhenTrue() {
            var expectedSqlText = @"CREATE TABLE DummyTable (
    ColumnName VARCHAR (50)
);";

            ParseAndAssertEquality(expectedSqlText, new SqlScriptGeneratorOptions {
                SpaceBetweenDataTypeAndParameters = true
            });
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestSpaceBetweenParametersInDataTypeWhenFalse() {
            var expectedSqlText = @"CREATE TABLE DummyTable (
    ColumnName DECIMAL (5,2)
);";

            ParseAndAssertEquality(expectedSqlText, new SqlScriptGeneratorOptions {
                SpaceBetweenParametersInDataType = false
            });
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestSpaceBetweenParametersInDataTypeWhenTrue() {
            var expectedSqlText = @"CREATE TABLE DummyTable (
    ColumnName DECIMAL (5, 2)
);";

            ParseAndAssertEquality(expectedSqlText, new SqlScriptGeneratorOptions {
                SpaceBetweenParametersInDataType = true
            });
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestNewlineFormattedCheckConstraintWhenFalse() {
            var expectedSqlText = @"CREATE TABLE DummyTable (
    CONSTRAINT ComplicatedConstraint CHECK ((Col1 IS NULL
                                             AND (Col2 <> ''
                                                  OR Col3 = 0))
                                            OR (Col1 IS NOT NULL
                                                AND ((Col2 = ''
                                                      AND Col3 <> 0)
                                                     OR (Col4 IN ('', 'ABC', 'JKL', 'XYZ')
                                                         AND Col3 < 0
                                                         AND (Col5 <> ''
                                                              OR Col6 <> '')))))
);";

            ParseAndAssertEquality(expectedSqlText, new SqlScriptGeneratorOptions {
                NewlineFormattedCheckConstraint = false
            });
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestNewlineFormattedCheckConstraintWhenTrue() {
            var expectedSqlText = @"CREATE TABLE DummyTable (
    CONSTRAINT ComplicatedConstraint
        CHECK ((Col1 IS NULL
                AND (Col2 <> ''
                     OR Col3 = 0))
               OR (Col1 IS NOT NULL
                   AND ((Col2 = ''
                         AND Col3 <> 0)
                        OR (Col4 IN ('', 'ABC', 'JKL', 'XYZ')
                            AND Col3 < 0
                            AND (Col5 <> ''
                                 OR Col6 <> '')))))
);";

            ParseAndAssertEquality(expectedSqlText, new SqlScriptGeneratorOptions {
                NewlineFormattedCheckConstraint = true
            });
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestNewLineFormattedIndexDefinitionWhenFalse() {
            var expectedSqlText = @"CREATE TABLE DummyTable (
    INDEX ComplicatedIndex UNIQUE (Col1, Col2, Col3) INCLUDE (Col4, Col5, Col6, Col7, Col8) WHERE Col4 = 'AR'
                                                                                                  AND Col3 IN ('ABC', 'XYZ')
                                                                                                      AND Col5 = 0
                                                                                                          AND Col6 = 1
                                                                                                              AND Col7 = 0
                                                                                                                  AND Col8 IS NOT NULL
);";

            ParseAndAssertEquality(expectedSqlText, new SqlScriptGeneratorOptions {
                NewLineFormattedIndexDefinition = false
            });
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestNewLineFormattedIndexDefinitionWhenTrue() {
            var expectedSqlText = @"CREATE TABLE DummyTable (
    INDEX ComplicatedIndex
        UNIQUE (Col1, Col2, Col3)
        INCLUDE (Col4, Col5, Col6, Col7, Col8)
        WHERE Col4 = 'AR'
              AND Col3 IN ('ABC', 'XYZ')
                  AND Col5 = 0
                      AND Col6 = 1
                          AND Col7 = 0
                              AND Col8 IS NOT NULL
);";

            ParseAndAssertEquality(expectedSqlText, new SqlScriptGeneratorOptions {
                NewLineFormattedIndexDefinition = true
            });
        }

        void ParseAndAssertEquality(string sqlText, SqlScriptGeneratorOptions generatorOptions) {
            var parser = new TSql160Parser(true);
            var fragment = parser.ParseStatementList(new StringReader(sqlText), out var errors);

            Assert.AreEqual(0, errors.Count);

            var generator = new Sql160ScriptGenerator(generatorOptions);
            generator.GenerateScript(fragment, out var generatedSqlText);

            Assert.AreEqual(sqlText, generatedSqlText);
        }

        #region Generator Whitespace Regression Tests

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestAlterTableAddColumnThenIndex_EmitsSeparatorBeforeIndex()
        {
            // Verbatim sample from docs/relational-databases/in-memory-oltp/
            // altering-memory-optimized-tables.md (line 119). Generator was
            // omitting the separator between the trailing column constraint
            // ('NOT NULL') and the inline INDEX, producing 'NOT NULLINDEX
            // ix_Customer' which fails to reparse.
            var sql =
                "ALTER TABLE Sales.SalesOrderDetail_inmem  " + Environment.NewLine +
                "       ADD    CustomerID int NOT NULL DEFAULT -1 WITH VALUES,  " + Environment.NewLine +
                "              ShipMethodID int NOT NULL DEFAULT -1 WITH VALUES,  " + Environment.NewLine +
                "              INDEX ix_Customer (CustomerID);  " + Environment.NewLine +
                "GO  " + Environment.NewLine;

            var parser = new TSql170Parser(true);
            TSqlFragment fragment;
            IList<ParseError> parseErrors;
            using (var reader = new StringReader(sql))
            {
                fragment = parser.Parse(reader, out parseErrors);
            }
            Assert.AreEqual(0, parseErrors.Count, "Input must parse.");

            var generator = new Sql170ScriptGenerator(new SqlScriptGeneratorOptions
            {
                IncludeSemicolons = true,
            });
            generator.GenerateScript(fragment, out var generated);

            Assert.IsFalse(generated.Contains("NULLINDEX"),
                "Column constraint 'NULL' must not run into the 'INDEX' keyword. Actual:\n" + generated);
            Assert.IsTrue(generated.Contains("INDEX ix_Customer"),
                "INDEX keyword must be present and separated. Actual:\n" + generated);

            var reparser = new TSql170Parser(true);
            IList<ParseError> reparseErrors;
            using (var reader = new StringReader(generated))
            {
                reparser.Parse(reader, out reparseErrors);
            }
            Assert.AreEqual(0, reparseErrors.Count,
                "Generated SQL must reparse. Actual:\n" + generated);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestAlterTableAddIndexOnly_EmitsNoLeadingSeparator()
        {
            // Verbatim sample from docs/relational-databases/in-memory-oltp/
            // altering-memory-optimized-tables.md (line 111). With no preceding
            // column or constraint, the generator must NOT emit a leading
            // comma before the INDEX (which would produce invalid syntax).
            var sql =
                "ALTER TABLE Sales.SalesOrderDetail_inmem  " + Environment.NewLine +
                "       ADD INDEX ix_ModifiedDate (ModifiedDate);  " + Environment.NewLine +
                "GO  " + Environment.NewLine;

            var parser = new TSql170Parser(true);
            TSqlFragment fragment;
            IList<ParseError> parseErrors;
            using (var reader = new StringReader(sql))
            {
                fragment = parser.Parse(reader, out parseErrors);
            }
            Assert.AreEqual(0, parseErrors.Count, "Input must parse.");

            var generator = new Sql170ScriptGenerator(new SqlScriptGeneratorOptions
            {
                IncludeSemicolons = true,
            });
            generator.GenerateScript(fragment, out var generated);

            Assert.IsFalse(generated.Contains("ADD ,") || generated.Contains("ADD\n,"),
                "ADD must not be followed by a stray separator. Actual:\n" + generated);
            Assert.IsTrue(generated.Contains("ADD INDEX ix_ModifiedDate"),
                "INDEX clause must follow ADD directly. Actual:\n" + generated);

            var reparser = new TSql170Parser(true);
            IList<ParseError> reparseErrors;
            using (var reader = new StringReader(generated))
            {
                reparser.Parse(reader, out reparseErrors);
            }
            Assert.AreEqual(0, reparseErrors.Count,
                "Generated SQL must reparse. Actual:\n" + generated);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestWindowDefinition_RefWindowFollowedByPartitionByEmitsSpace()
        {
            // Verbatim sample from docs/t-sql/queries/select-window-transact-sql.md
            // (line 312). Generator was emitting 'win2PARTITION' (no space)
            // because the visitor didn't separate the inherited window-name
            // reference from the PARTITION keyword.
            var sql =
                "ALTER DATABASE AdventureWorks2025" + Environment.NewLine +
                "SET COMPATIBILITY_LEVEL = 160;" + Environment.NewLine +
                "GO" + Environment.NewLine +
                Environment.NewLine +
                "USE AdventureWorks2025;" + Environment.NewLine +
                "GO" + Environment.NewLine +
                Environment.NewLine +
                "SELECT SalesOrderID AS OrderNumber," + Environment.NewLine +
                "       ProductID," + Environment.NewLine +
                "       OrderQty AS Qty," + Environment.NewLine +
                "       SUM(OrderQty) OVER win2 AS Total," + Environment.NewLine +
                "       AVG(OrderQty) OVER win1 AS Avg" + Environment.NewLine +
                "FROM Sales.SalesOrderDetail" + Environment.NewLine +
                "WHERE SalesOrderID IN (43659, 43664)" + Environment.NewLine +
                "      AND ProductID LIKE '71%'" + Environment.NewLine +
                "WINDOW win1 AS (win3)," + Environment.NewLine +
                "       win2 AS (ORDER BY SalesOrderID, ProductID)," + Environment.NewLine +
                "       win3 AS (win2 PARTITION BY SalesOrderID);" + Environment.NewLine;

            var parser = new TSql170Parser(true);
            TSqlFragment fragment;
            IList<ParseError> parseErrors;
            using (var reader = new StringReader(sql))
            {
                fragment = parser.Parse(reader, out parseErrors);
            }
            Assert.AreEqual(0, parseErrors.Count, "Input must parse.");

            var generator = new Sql170ScriptGenerator(new SqlScriptGeneratorOptions
            {
                IncludeSemicolons = true,
            });
            generator.GenerateScript(fragment, out var generated);

            Assert.IsFalse(generated.Contains("win2PARTITION"),
                "Window-name reference must not run into PARTITION keyword. Actual:\n" + generated);
            Assert.IsTrue(generated.Contains("win2 PARTITION"),
                "Generated window must read 'win2 PARTITION'. Actual:\n" + generated);

            var reparser = new TSql170Parser(true);
            IList<ParseError> reparseErrors;
            using (var reader = new StringReader(generated))
            {
                reparser.Parse(reader, out reparseErrors);
            }
            Assert.AreEqual(0, reparseErrors.Count,
                "Generated SQL must reparse. Actual:\n" + generated);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestWindowDefinition_RefWindowFollowedByOrderByEmitsSpace()
        {
            // Same bug class as above, exercised through the ORDER BY branch.
            // 'WINDOW name AS (refname ORDER BY ...)' must not emit
            // 'refnameORDER'. This shape isn't in the docs but the same code
            // path can produce it; included as belt-and-suspenders coverage.
            var sql =
                "SELECT SalesOrderID, SUM(OrderQty) OVER win2 AS Total" + Environment.NewLine +
                "FROM   Sales.SalesOrderDetail" + Environment.NewLine +
                "WINDOW win1 AS (PARTITION BY ProductID)," + Environment.NewLine +
                "       win2 AS (win1 ORDER BY SalesOrderID);";

            var parser = new TSql170Parser(true);
            TSqlFragment fragment;
            IList<ParseError> parseErrors;
            using (var reader = new StringReader(sql))
            {
                fragment = parser.Parse(reader, out parseErrors);
            }
            Assert.AreEqual(0, parseErrors.Count, "Input must parse.");

            var generator = new Sql170ScriptGenerator(new SqlScriptGeneratorOptions
            {
                IncludeSemicolons = true,
            });
            generator.GenerateScript(fragment, out var generated);

            Assert.IsFalse(generated.Contains("win1ORDER"),
                "Window-name reference must not run into ORDER keyword. Actual:\n" + generated);
            Assert.IsTrue(generated.Contains("win1 ORDER"),
                "Generated window must read 'win1 ORDER'. Actual:\n" + generated);

            var reparser = new TSql170Parser(true);
            IList<ParseError> reparseErrors;
            using (var reader = new StringReader(generated))
            {
                reparser.Parse(reader, out reparseErrors);
            }
            Assert.AreEqual(0, reparseErrors.Count,
                "Generated SQL must reparse. Actual:\n" + generated);
        }

        #endregion

        #region Comment Preservation Tests

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestPreserveCommentsDefault()
        {
            // Verify default is false
            var options = new SqlScriptGeneratorOptions();
            Assert.AreEqual(false, options.PreserveComments);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestPreserveCommentsDisabled()
        {
            // When PreserveComments is false (default), comments should be stripped
            var sqlWithComments = "-- This is a leading comment\nSELECT 1; -- trailing comment";
            var parser = new TSql170Parser(true);
            var fragment = parser.Parse(new StringReader(sqlWithComments), out var errors);

            Assert.AreEqual(0, errors.Count);

            var generatorOptions = new SqlScriptGeneratorOptions
            {
                PreserveComments = false  // default
            };
            var generator = new Sql170ScriptGenerator(generatorOptions);
            generator.GenerateScript(fragment, out var generatedSql);

            // Without PreserveComments, comments should not appear
            Assert.IsFalse(generatedSql.Contains("--"), "Comments should be stripped when PreserveComments is false");
            Assert.IsFalse(generatedSql.Contains("leading comment"), "Comment text should not appear");
            Assert.IsFalse(generatedSql.Contains("trailing comment"), "Trailing comment should not appear");
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestPreserveCommentsEnabled_SingleLineLeading()
        {
            // When PreserveComments is true, leading single-line comments should be preserved
            var sqlWithComments = "-- This is a leading comment\nSELECT 1;";
            var parser = new TSql170Parser(true);
            var fragment = parser.Parse(new StringReader(sqlWithComments), out var errors);

            Assert.AreEqual(0, errors.Count);

            var generatorOptions = new SqlScriptGeneratorOptions
            {
                PreserveComments = true
            };
            var generator = new Sql170ScriptGenerator(generatorOptions);
            generator.GenerateScript(fragment, out var generatedSql);

            // With PreserveComments, the comment should appear in output
            Assert.IsTrue(generatedSql.Contains("-- This is a leading comment"), 
                "Leading comment should be preserved when PreserveComments is true");
            
            // Verify comment appears BEFORE the SELECT keyword (correct positioning)
            int commentIndex = generatedSql.IndexOf("-- This is a leading comment");
            int selectIndex = generatedSql.IndexOf("SELECT", StringComparison.OrdinalIgnoreCase);
            Assert.IsTrue(commentIndex < selectIndex, 
                $"Leading comment should appear before SELECT. Comment at {commentIndex}, SELECT at {selectIndex}");
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestPreserveCommentsEnabled_SingleLineTrailing()
        {
            // When PreserveComments is true, trailing single-line comments should be preserved
            var sqlWithComments = "SELECT 1; -- trailing comment";
            var parser = new TSql170Parser(true);
            var fragment = parser.Parse(new StringReader(sqlWithComments), out var errors);

            Assert.AreEqual(0, errors.Count);

            var generatorOptions = new SqlScriptGeneratorOptions
            {
                PreserveComments = true
            };
            var generator = new Sql170ScriptGenerator(generatorOptions);
            generator.GenerateScript(fragment, out var generatedSql);

            // With PreserveComments, the trailing comment should appear
            Assert.IsTrue(generatedSql.Contains("-- trailing comment"), 
                "Trailing comment should be preserved when PreserveComments is true");
            
            // Verify trailing comment appears AFTER the SELECT (correct positioning)
            int selectIndex = generatedSql.IndexOf("SELECT", StringComparison.OrdinalIgnoreCase);
            int commentIndex = generatedSql.IndexOf("-- trailing comment");
            Assert.IsTrue(commentIndex > selectIndex, 
                $"Trailing comment should appear after SELECT. SELECT at {selectIndex}, comment at {commentIndex}");
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestPreserveCommentsDoesNotBreakColumnDefinitionAlignment()
        {
            // Regression: enabling PreserveComments must not defeat AlignColumnDefinitionFields.
            // When there are no comments in the input, the generated script must be identical
            // whether PreserveComments is on or off, and the column fields must stay aligned.
            const string sql =
                "CREATE TABLE dbo.t (id INT NOT NULL, long_column_name DECIMAL(10, 2) NULL, c VARCHAR(20) NOT NULL);";

            const string expected =
@"CREATE TABLE dbo.t (
    id               INT             NOT NULL,
    long_column_name DECIMAL (10, 2) NULL,
    c                VARCHAR (20)    NOT NULL
);";

            // Aligned output must be produced when PreserveComments is enabled (AssertGenerated also
            // verifies the generated script reparses without errors)...
            ScriptGeneratorTestHelper.AssertGenerated(
                sql,
                new SqlScriptGeneratorOptions { AlignColumnDefinitionFields = true, PreserveComments = true },
                expected);

            // ...and it must match the output produced with PreserveComments disabled (no comments present).
            ScriptGeneratorTestHelper.AssertGenerated(
                sql,
                new SqlScriptGeneratorOptions { AlignColumnDefinitionFields = true, PreserveComments = false },
                expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestPreserveCommentsKeepsColumnDefinitionAlignmentWithTrailingComments()
        {
            // Each column has a trailing '--' comment. With PreserveComments enabled the comments
            // must be preserved and the column definition fields must remain aligned.
            const string sql =
@"CREATE TABLE dbo.t (
    id INT NOT NULL, -- the identifier
    long_column_name DECIMAL(10, 2) NULL, -- a decimal value
    c VARCHAR(20) NOT NULL -- a string
);";

            const string expected =
@"CREATE TABLE dbo.t (
    id               INT             NOT NULL, -- the identifier
    long_column_name DECIMAL (10, 2) NULL, -- a decimal value
    c                VARCHAR (20)    NOT NULL -- a string
);";

            // AssertGenerated also verifies the generated script (comments included) reparses cleanly.
            ScriptGeneratorTestHelper.AssertGenerated(
                sql,
                new SqlScriptGeneratorOptions { AlignColumnDefinitionFields = true, PreserveComments = true },
                expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestPreserveCommentsColumnAlignmentIsIsolatedPerTable()
        {
            // Two CREATE TABLE column lists are rendered in the same enclosing scope. Each table's
            // column fields must align only against that table's own columns; the wide column names
            // in the first table must not widen the (independently aligned) second table.
            const string sql =
@"CREATE TABLE dbo.wide (a_very_long_column_name INT NOT NULL, b INT NULL);
CREATE TABLE dbo.t (x INT NOT NULL, y INT NULL);";

            const string expected =
@"CREATE TABLE dbo.wide (
    a_very_long_column_name INT NOT NULL,
    b                       INT NULL
);

CREATE TABLE dbo.t (
    x INT NOT NULL,
    y INT NULL
);";

            ScriptGeneratorTestHelper.AssertGenerated(
                sql,
                new SqlScriptGeneratorOptions { AlignColumnDefinitionFields = true, PreserveComments = true },
                expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestPreserveCommentsMultilineViewColumnsWithEmptyAlignmentScope()
        {
            // CREATE VIEW generates its multiline column list without first pushing an enclosing
            // alignment point, so the list-level named scope is pushed while the newline-restoration
            // stack is empty. This exercises the null newline-point path (PushNamedAlignmentScope
            // reusing "no current point", and the null guard in NewLine): it must not throw and must
            // still emit each column on its own line with PreserveComments enabled.
            const string sql = "CREATE VIEW dbo.v (col1, long_column_name, c) AS SELECT 1, 2, 3;";

            const string expected =
@"CREATE VIEW dbo.v (
    col1,
    long_column_name,
    c
)
AS
SELECT 1,
       2,
       3;";

            ScriptGeneratorTestHelper.AssertGenerated(
                sql,
                new SqlScriptGeneratorOptions { MultilineViewColumnsList = true, PreserveComments = true },
                expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestPreserveCommentsEnabled_MultipleStatements()
        {
            // Test comments between multiple statements
            var sqlWithComments = @"-- First statement
SELECT 1;
-- Comment between statements
SELECT 2;";
            var parser = new TSql170Parser(true);
            var fragment = parser.Parse(new StringReader(sqlWithComments), out var errors);

            Assert.AreEqual(0, errors.Count);

            var generatorOptions = new SqlScriptGeneratorOptions
            {
                PreserveComments = true
            };
            var generator = new Sql170ScriptGenerator(generatorOptions);
            generator.GenerateScript(fragment, out var generatedSql);

            // Both comments should be preserved
            Assert.IsTrue(generatedSql.Contains("-- First statement"), 
                "First comment should be preserved");
            Assert.IsTrue(generatedSql.Contains("-- Comment between statements"), 
                "Comment between statements should be preserved");
            
            // Verify ordering: first comment -> SELECT 1 -> between comment -> SELECT 2
            int firstCommentIndex = generatedSql.IndexOf("-- First statement");
            int firstSelectIndex = generatedSql.IndexOf("SELECT 1", StringComparison.OrdinalIgnoreCase);
            int betweenCommentIndex = generatedSql.IndexOf("-- Comment between statements");
            int secondSelectIndex = generatedSql.IndexOf("SELECT 2", StringComparison.OrdinalIgnoreCase);
            
            Assert.IsTrue(firstCommentIndex < firstSelectIndex, 
                "First comment should appear before first SELECT");
            Assert.IsTrue(firstSelectIndex < betweenCommentIndex, 
                "Between comment should appear after first SELECT");
            Assert.IsTrue(betweenCommentIndex < secondSelectIndex, 
                "Between comment should appear before second SELECT");
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestPreserveCommentsEnabled_MultiLineComment()
        {
            // When PreserveComments is true, multi-line comments should be preserved
            var sqlWithComments = "/* This is a multi-line comment */\nSELECT 1;";
            var parser = new TSql170Parser(true);
            var fragment = parser.Parse(new StringReader(sqlWithComments), out var errors);

            Assert.AreEqual(0, errors.Count);

            var generatorOptions = new SqlScriptGeneratorOptions
            {
                PreserveComments = true
            };
            var generator = new Sql170ScriptGenerator(generatorOptions);
            generator.GenerateScript(fragment, out var generatedSql);

            // With PreserveComments, the multi-line comment should appear
            Assert.IsTrue(generatedSql.Contains("/* This is a multi-line comment */"), 
                "Multi-line comment should be preserved when PreserveComments is true");
            
            // Verify comment appears BEFORE the SELECT keyword (correct positioning)
            int commentIndex = generatedSql.IndexOf("/* This is a multi-line comment */");
            int selectIndex = generatedSql.IndexOf("SELECT", StringComparison.OrdinalIgnoreCase);
            Assert.IsTrue(commentIndex < selectIndex, 
                $"Multi-line comment should appear before SELECT. Comment at {commentIndex}, SELECT at {selectIndex}");
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestPreserveCommentsEnabled_MultiLineBlockComment()
        {
            // Test that decorative patterns are preserved
            var sqlWithComments = @"/***** Header Comment *****/
SELECT 1;";
            var parser = new TSql170Parser(true);
            var fragment = parser.Parse(new StringReader(sqlWithComments), out var errors);

            Assert.AreEqual(0, errors.Count);

            var generatorOptions = new SqlScriptGeneratorOptions
            {
                PreserveComments = true
            };
            var generator = new Sql170ScriptGenerator(generatorOptions);
            generator.GenerateScript(fragment, out var generatedSql);

            // Decorative pattern should be preserved exactly
            Assert.IsTrue(generatedSql.Contains("/***** Header Comment *****/"), 
                "Decorative comment pattern should be preserved exactly");
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestPreserveCommentsEnabled_Subquery()
        {
            // Test comments with subqueries
            var sqlWithComments = @"-- Outer query comment
SELECT * FROM (
    -- Inner subquery comment
    SELECT id, name FROM users
) AS subq;";
            var parser = new TSql170Parser(true);
            var fragment = parser.Parse(new StringReader(sqlWithComments), out var errors);

            Assert.AreEqual(0, errors.Count);

            var generatorOptions = new SqlScriptGeneratorOptions
            {
                PreserveComments = true
            };
            var generator = new Sql170ScriptGenerator(generatorOptions);
            generator.GenerateScript(fragment, out var generatedSql);

            // Outer comment should be preserved
            Assert.IsTrue(generatedSql.Contains("-- Outer query comment"), 
                "Outer query comment should be preserved");
            
            // Verify comment appears BEFORE the SELECT keyword (correct positioning)
            int commentIndex = generatedSql.IndexOf("-- Outer query comment");
            int selectIndex = generatedSql.IndexOf("SELECT", StringComparison.OrdinalIgnoreCase);
            Assert.IsTrue(commentIndex < selectIndex, 
                $"Outer query comment should appear before SELECT. Comment at {commentIndex}, SELECT at {selectIndex}");
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestPreserveCommentsEnabled_CTE()
        {
            // Test comments with Common Table Expressions
            var sqlWithComments = @"-- CTE definition comment
WITH cte AS (
    SELECT id FROM users
)
-- Main query comment
SELECT * FROM cte;";
            var parser = new TSql170Parser(true);
            var fragment = parser.Parse(new StringReader(sqlWithComments), out var errors);

            Assert.AreEqual(0, errors.Count);

            var generatorOptions = new SqlScriptGeneratorOptions
            {
                PreserveComments = true
            };
            var generator = new Sql170ScriptGenerator(generatorOptions);
            generator.GenerateScript(fragment, out var generatedSql);

            // CTE comment should be preserved AND appear before the WITH keyword
            Assert.IsTrue(generatedSql.Contains("-- CTE definition comment"), 
                "CTE definition comment should be preserved");
            
            // Verify comment appears BEFORE the WITH keyword (correct positioning)
            int commentIndex = generatedSql.IndexOf("-- CTE definition comment");
            int withIndex = generatedSql.IndexOf("WITH", StringComparison.OrdinalIgnoreCase);
            Assert.IsTrue(commentIndex < withIndex, 
                $"CTE comment should appear before WITH keyword. Comment at {commentIndex}, WITH at {withIndex}");
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestPreserveCommentsEnabled_InsertSelect()
        {
            // Test comments with INSERT...SELECT statements
            var sqlWithComments = @"-- Insert with select comment
INSERT INTO target_table (col1, col2)
-- Select portion comment
SELECT a, b FROM source_table;";
            var parser = new TSql170Parser(true);
            var fragment = parser.Parse(new StringReader(sqlWithComments), out var errors);

            Assert.AreEqual(0, errors.Count);

            var generatorOptions = new SqlScriptGeneratorOptions
            {
                PreserveComments = true
            };
            var generator = new Sql170ScriptGenerator(generatorOptions);
            generator.GenerateScript(fragment, out var generatedSql);

            // Insert comment should be preserved
            Assert.IsTrue(generatedSql.Contains("-- Insert with select comment"), 
                "Insert statement comment should be preserved");
            
            // Verify comment appears BEFORE the INSERT keyword (correct positioning)
            int commentIndex = generatedSql.IndexOf("-- Insert with select comment");
            int insertIndex = generatedSql.IndexOf("INSERT", StringComparison.OrdinalIgnoreCase);
            Assert.IsTrue(commentIndex < insertIndex, 
                $"Insert comment should appear before INSERT. Comment at {commentIndex}, INSERT at {insertIndex}");
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestPreserveCommentsEnabled_StoredProcedure()
        {
            // Test comments within stored procedure body
            var sqlWithComments = @"-- Procedure header comment
CREATE PROCEDURE TestProc
AS
BEGIN
    -- First statement in proc
    SELECT 1;
    -- Second statement in proc
    SELECT 2;
END;";
            var parser = new TSql170Parser(true);
            var fragment = parser.Parse(new StringReader(sqlWithComments), out var errors);

            Assert.AreEqual(0, errors.Count);

            var generatorOptions = new SqlScriptGeneratorOptions
            {
                PreserveComments = true
            };
            var generator = new Sql170ScriptGenerator(generatorOptions);
            generator.GenerateScript(fragment, out var generatedSql);

            // Procedure header comment should be preserved
            Assert.IsTrue(generatedSql.Contains("-- Procedure header comment"), 
                "Procedure header comment should be preserved");
            
            // Verify comment appears BEFORE the CREATE keyword (correct positioning)
            int commentIndex = generatedSql.IndexOf("-- Procedure header comment");
            int createIndex = generatedSql.IndexOf("CREATE", StringComparison.OrdinalIgnoreCase);
            Assert.IsTrue(commentIndex < createIndex, 
                $"Procedure comment should appear before CREATE. Comment at {commentIndex}, CREATE at {createIndex}");
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestPreserveCommentsEnabled_MixedCommentStyles()
        {
            // Test mixing single-line and multi-line comments
            var sqlWithComments = @"/* Block comment at start */
-- Single line after block
SELECT 1; /* inline block */ -- trailing single";
            var parser = new TSql170Parser(true);
            var fragment = parser.Parse(new StringReader(sqlWithComments), out var errors);

            Assert.AreEqual(0, errors.Count);

            var generatorOptions = new SqlScriptGeneratorOptions
            {
                PreserveComments = true
            };
            var generator = new Sql170ScriptGenerator(generatorOptions);
            generator.GenerateScript(fragment, out var generatedSql);

            // Both comment styles should be preserved
            Assert.IsTrue(generatedSql.Contains("/* Block comment at start */"), 
                "Block comment should be preserved");
            Assert.IsTrue(generatedSql.Contains("-- Single line after block"), 
                "Single line comment should be preserved");
            
            // Verify ordering: block comment -> single line comment -> SELECT
            int blockCommentIndex = generatedSql.IndexOf("/* Block comment at start */");
            int singleLineIndex = generatedSql.IndexOf("-- Single line after block");
            int selectIndex = generatedSql.IndexOf("SELECT", StringComparison.OrdinalIgnoreCase);
            
            Assert.IsTrue(blockCommentIndex < singleLineIndex, 
                "Block comment should appear before single-line comment");
            Assert.IsTrue(singleLineIndex < selectIndex, 
                "Single-line comment should appear before SELECT");
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestPreserveCommentsEnabled_CreateTable()
        {
            // Test comments with CREATE TABLE statement
            var sqlWithComments = @"-- Table creation comment
CREATE TABLE TestTable (
    -- Primary key column
    Id INT PRIMARY KEY,
    -- Name column
    Name NVARCHAR(100)
);";
            var parser = new TSql170Parser(true);
            var fragment = parser.Parse(new StringReader(sqlWithComments), out var errors);

            Assert.AreEqual(0, errors.Count);

            var generatorOptions = new SqlScriptGeneratorOptions
            {
                PreserveComments = true
            };
            var generator = new Sql170ScriptGenerator(generatorOptions);
            generator.GenerateScript(fragment, out var generatedSql);

            // Table creation comment should be preserved
            Assert.IsTrue(generatedSql.Contains("-- Table creation comment"), 
                "Table creation comment should be preserved");
            
            // Verify comment appears BEFORE the CREATE keyword (correct positioning)
            int commentIndex = generatedSql.IndexOf("-- Table creation comment");
            int createIndex = generatedSql.IndexOf("CREATE", StringComparison.OrdinalIgnoreCase);
            Assert.IsTrue(commentIndex < createIndex, 
                $"Table comment should appear before CREATE. Comment at {commentIndex}, CREATE at {createIndex}");
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestPreserveCommentsEnabled_EndOfScriptComment()
        {
            // Test comments at the very end of the script (after the last statement)
            // This is an edge case: there's no "next fragment" to capture these as leading comments
            var sqlWithComments = @"SELECT 1;
-- End of script comment";
            var parser = new TSql170Parser(true);
            var fragment = parser.Parse(new StringReader(sqlWithComments), out var errors);

            Assert.AreEqual(0, errors.Count);

            var generatorOptions = new SqlScriptGeneratorOptions
            {
                PreserveComments = true
            };
            var generator = new Sql170ScriptGenerator(generatorOptions);
            generator.GenerateScript(fragment, out var generatedSql);

            // End-of-script comment should be preserved
            Assert.IsTrue(generatedSql.Contains("-- End of script comment"), 
                "End-of-script comment should be preserved. Actual output: " + generatedSql);
            
            // Verify comment appears AFTER the SELECT (correct positioning)
            int selectIndex = generatedSql.IndexOf("SELECT", StringComparison.OrdinalIgnoreCase);
            int commentIndex = generatedSql.IndexOf("-- End of script comment");
            Assert.IsTrue(commentIndex > selectIndex, 
                $"End-of-script comment should appear after SELECT. SELECT at {selectIndex}, comment at {commentIndex}");
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestPreserveCommentsEnabled_EndOfScriptMultiLineComment()
        {
            // Test multi-line comments at the very end of the script
            var sqlWithComments = @"SELECT 1;
/* End of script
   multi-line comment */";
            var parser = new TSql170Parser(true);
            var fragment = parser.Parse(new StringReader(sqlWithComments), out var errors);

            Assert.AreEqual(0, errors.Count);

            var generatorOptions = new SqlScriptGeneratorOptions
            {
                PreserveComments = true
            };
            var generator = new Sql170ScriptGenerator(generatorOptions);
            generator.GenerateScript(fragment, out var generatedSql);

            // End-of-script multi-line comment should be preserved
            Assert.IsTrue(generatedSql.Contains("/* End of script"), 
                "End-of-script multi-line comment should be preserved. Actual output: " + generatedSql);
            Assert.IsTrue(generatedSql.Contains("multi-line comment */"), 
                "End-of-script multi-line comment should be complete");
            
            // Verify comment appears AFTER the SELECT
            int selectIndex = generatedSql.IndexOf("SELECT", StringComparison.OrdinalIgnoreCase);
            int commentIndex = generatedSql.IndexOf("/* End of script");
            Assert.IsTrue(commentIndex > selectIndex, 
                $"End-of-script comment should appear after SELECT. SELECT at {selectIndex}, comment at {commentIndex}");
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestPreserveCommentsEnabled_CommentInWhereClause()
        {
            // Test comments within WHERE clause expressions
            // This tests the improved centralized comment handling in GenerateFragmentIfNotNull
            var sqlWithComments = @"SELECT id, name
FROM users
WHERE /* filter active users */ status = 'active';";
            var parser = new TSql170Parser(true);
            var fragment = parser.Parse(new StringReader(sqlWithComments), out var errors);

            Assert.AreEqual(0, errors.Count);

            var generatorOptions = new SqlScriptGeneratorOptions
            {
                PreserveComments = true
            };
            var generator = new Sql170ScriptGenerator(generatorOptions);
            generator.GenerateScript(fragment, out var generatedSql);

            // Comment in WHERE clause should be preserved
            Assert.IsTrue(generatedSql.Contains("/* filter active users */"), 
                "Comment in WHERE clause should be preserved. Actual output: " + generatedSql);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestPreserveCommentsEnabled_CommentInSelectList()
        {
            // Test comments within SELECT list (between columns)
            var sqlWithComments = @"SELECT 
    id, -- primary key
    name, -- user name
    email -- contact info
FROM users;";
            var parser = new TSql170Parser(true);
            var fragment = parser.Parse(new StringReader(sqlWithComments), out var errors);

            Assert.AreEqual(0, errors.Count);

            var generatorOptions = new SqlScriptGeneratorOptions
            {
                PreserveComments = true
            };
            var generator = new Sql170ScriptGenerator(generatorOptions);
            generator.GenerateScript(fragment, out var generatedSql);

            // At least one of the column comments should be preserved
            Assert.IsTrue(
                generatedSql.Contains("-- primary key") || 
                generatedSql.Contains("-- user name") ||
                generatedSql.Contains("-- contact info"), 
                "At least one column comment should be preserved. Actual output: " + generatedSql);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestPreserveCommentsEnabled_CommentInColumnDefinitions()
        {
            // Test comments inside column definitions in CREATE TABLE
            var sqlWithComments = @"CREATE TABLE TestTable (
    -- Primary key column
    Id INT NOT NULL,
    -- User's full name
    FullName NVARCHAR(100),
    /* Email address for notifications */
    Email NVARCHAR(255),
    -- Timestamp for auditing
    CreatedDate DATETIME DEFAULT GETDATE()
);";
            var parser = new TSql170Parser(true);
            var fragment = parser.Parse(new StringReader(sqlWithComments), out var errors);

            Assert.AreEqual(0, errors.Count);

            var generatorOptions = new SqlScriptGeneratorOptions
            {
                PreserveComments = true
            };
            var generator = new Sql170ScriptGenerator(generatorOptions);
            generator.GenerateScript(fragment, out var generatedSql);

            // Column definition comments should be preserved and in correct positions
            Assert.IsTrue(generatedSql.Contains("-- Primary key column"), 
                "Primary key column comment should be preserved. Actual: " + generatedSql);
            Assert.IsTrue(generatedSql.Contains("-- User's full name"), 
                "FullName column comment should be preserved. Actual: " + generatedSql);
            Assert.IsTrue(generatedSql.Contains("/* Email address for notifications */"), 
                "Email column block comment should be preserved. Actual: " + generatedSql);
            Assert.IsTrue(generatedSql.Contains("-- Timestamp for auditing"), 
                "CreatedDate column comment should be preserved. Actual: " + generatedSql);
            
            // Verify position: comments should appear before their respective columns
            int pkCommentIdx = generatedSql.IndexOf("-- Primary key column");
            int idColumnIdx = generatedSql.IndexOf("Id", StringComparison.OrdinalIgnoreCase);
            int nameCommentIdx = generatedSql.IndexOf("-- User's full name");
            int fullNameColumnIdx = generatedSql.IndexOf("FullName", StringComparison.OrdinalIgnoreCase);
            int emailCommentIdx = generatedSql.IndexOf("/* Email address for notifications */");
            int emailColumnIdx = generatedSql.IndexOf("Email", StringComparison.OrdinalIgnoreCase);
            
            Assert.IsTrue(pkCommentIdx < idColumnIdx, 
                $"Primary key comment should appear before Id column. Comment at {pkCommentIdx}, Id at {idColumnIdx}");
            Assert.IsTrue(nameCommentIdx < fullNameColumnIdx, 
                $"FullName comment should appear before FullName column. Comment at {nameCommentIdx}, FullName at {fullNameColumnIdx}");
            Assert.IsTrue(emailCommentIdx < emailColumnIdx, 
                $"Email comment should appear before Email column. Comment at {emailCommentIdx}, Email at {emailColumnIdx}");
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestPreserveCommentsEnabled_CommentInCaseExpression()
        {
            // Test comments within CASE expressions
            var sqlWithComments = @"SELECT 
    CASE 
        -- Check for high priority
        WHEN priority = 1 THEN 'High'
        /* Medium priority items */
        WHEN priority = 2 THEN 'Medium'
        -- Default to low priority
        ELSE 'Low'
    END AS PriorityLevel
FROM tasks;";
            var parser = new TSql170Parser(true);
            var fragment = parser.Parse(new StringReader(sqlWithComments), out var errors);

            Assert.AreEqual(0, errors.Count);

            var generatorOptions = new SqlScriptGeneratorOptions
            {
                PreserveComments = true
            };
            var generator = new Sql170ScriptGenerator(generatorOptions);
            generator.GenerateScript(fragment, out var generatedSql);

            // CASE expression comments should be preserved
            Assert.IsTrue(generatedSql.Contains("-- Check for high priority"), 
                "High priority WHEN comment should be preserved. Actual: " + generatedSql);
            Assert.IsTrue(generatedSql.Contains("/* Medium priority items */"), 
                "Medium priority block comment should be preserved. Actual: " + generatedSql);
            Assert.IsTrue(generatedSql.Contains("-- Default to low priority"), 
                "ELSE comment should be preserved. Actual: " + generatedSql);

            // The generated SQL must reparse cleanly: a '--' comment must never
            // be placed where it would absorb a following keyword or symbol.
            var reparser = new TSql170Parser(true);
            reparser.Parse(new StringReader(generatedSql), out var reparseErrors);
            Assert.AreEqual(0, reparseErrors.Count,
                "Generated SQL should reparse without errors. Actual: " + generatedSql);

            // Verify position: comments anchored to a preceding clause should
            // appear before their associated WHEN/ELSE when a natural newline
            // separates them in the output.
            int highPriorityCommentIdx = generatedSql.IndexOf("-- Check for high priority");
            int firstWhenIdx = generatedSql.IndexOf("WHEN", StringComparison.OrdinalIgnoreCase);
            int mediumCommentIdx = generatedSql.IndexOf("/* Medium priority items */");
            int secondWhenIdx = generatedSql.IndexOf("WHEN", firstWhenIdx + 1, StringComparison.OrdinalIgnoreCase);

            Assert.IsTrue(highPriorityCommentIdx < firstWhenIdx, 
                $"High priority comment should appear before first WHEN. Comment at {highPriorityCommentIdx}, WHEN at {firstWhenIdx}");
            Assert.IsTrue(mediumCommentIdx < secondWhenIdx, 
                $"Medium priority comment should appear before second WHEN. Comment at {mediumCommentIdx}, WHEN at {secondWhenIdx}");
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestPreserveCommentsEnabled_CommentInJoinClauses()
        {
            // Test comments in JOIN clauses
            var sqlWithComments = @"SELECT u.name, o.order_date
FROM users u
-- Join to get user orders
INNER JOIN orders o ON u.id = o.user_id
/* Left join for optional address */
LEFT JOIN addresses a ON u.id = a.user_id
-- Cross join for all combinations
CROSS JOIN products p;";
            var parser = new TSql170Parser(true);
            var fragment = parser.Parse(new StringReader(sqlWithComments), out var errors);

            Assert.AreEqual(0, errors.Count);

            var generatorOptions = new SqlScriptGeneratorOptions
            {
                PreserveComments = true
            };
            var generator = new Sql170ScriptGenerator(generatorOptions);
            generator.GenerateScript(fragment, out var generatedSql);

            // JOIN clause comments should be preserved
            Assert.IsTrue(generatedSql.Contains("-- Join to get user orders"), 
                "INNER JOIN comment should be preserved. Actual: " + generatedSql);
            Assert.IsTrue(generatedSql.Contains("/* Left join for optional address */"), 
                "LEFT JOIN block comment should be preserved. Actual: " + generatedSql);
            Assert.IsTrue(generatedSql.Contains("-- Cross join for all combinations"), 
                "CROSS JOIN comment should be preserved. Actual: " + generatedSql);

            // The generated SQL must reparse cleanly: a '--' comment must never
            // be placed where it would absorb a following keyword or symbol.
            var reparser = new TSql170Parser(true);
            reparser.Parse(new StringReader(generatedSql), out var reparseErrors);
            Assert.AreEqual(0, reparseErrors.Count,
                "Generated SQL should reparse without errors. Actual: " + generatedSql);

            // CRITICAL ASSERTIONS: Comments must appear BEFORE their JOIN keywords
            // and NOT be on the same line as the previous table reference
            
            // Find the comment for INNER JOIN
            int innerJoinCommentIdx = generatedSql.IndexOf("-- Join to get user orders");
            int innerJoinIdx = generatedSql.IndexOf("INNER JOIN", StringComparison.OrdinalIgnoreCase);
            Assert.IsTrue(innerJoinCommentIdx >= 0, "INNER JOIN comment must exist");
            Assert.IsTrue(innerJoinCommentIdx < innerJoinIdx, 
                $"INNER JOIN comment MUST appear before INNER JOIN keyword. Comment at {innerJoinCommentIdx}, JOIN at {innerJoinIdx}. Generated SQL:\n{generatedSql}");
            
            // Ensure comment is on its own line BEFORE INNER JOIN, not trailing after "users u"
            string beforeInnerJoinComment = generatedSql.Substring(0, innerJoinCommentIdx);
            int lastNewlineBeforeComment = Math.Max(
                beforeInnerJoinComment.LastIndexOf("\n"),
                beforeInnerJoinComment.LastIndexOf("\r")
            );
            string lineBeforeComment = lastNewlineBeforeComment >= 0 
                ? beforeInnerJoinComment.Substring(lastNewlineBeforeComment).Trim()
                : beforeInnerJoinComment.Trim();
            
            Assert.IsFalse(lineBeforeComment.Contains("users") && lineBeforeComment.Contains("--"),
                $"Comment should NOT be trailing on the 'users u' line. Line before comment: '{lineBeforeComment}'. Generated SQL:\n{generatedSql}");
            
            // Verify there's a newline between the comment and INNER JOIN
            string betweenCommentAndJoin = generatedSql.Substring(
                innerJoinCommentIdx + "-- Join to get user orders".Length,
                innerJoinIdx - (innerJoinCommentIdx + "-- Join to get user orders".Length)
            );
            Assert.IsTrue(betweenCommentAndJoin.Contains("\n") || betweenCommentAndJoin.Contains("\r"),
                $"There must be a newline between comment and INNER JOIN. Text between: '{betweenCommentAndJoin}'. Generated SQL:\n{generatedSql}");
            
            // Same checks for LEFT JOIN comment
            int leftJoinCommentIdx = generatedSql.IndexOf("/* Left join for optional address */");
            int leftJoinIdx = generatedSql.IndexOf("LEFT JOIN", StringComparison.OrdinalIgnoreCase);
            Assert.IsTrue(leftJoinCommentIdx >= 0, "LEFT JOIN comment must exist");
            Assert.IsTrue(leftJoinCommentIdx < leftJoinIdx, 
                $"LEFT JOIN comment MUST appear before LEFT JOIN keyword. Comment at {leftJoinCommentIdx}, JOIN at {leftJoinIdx}. Generated SQL:\n{generatedSql}");
            
            // Verify comment is on its own line, not trailing after previous JOIN
            string beforeLeftJoinComment = generatedSql.Substring(0, leftJoinCommentIdx);
            int lastNewlineBeforeLeftComment = Math.Max(
                beforeLeftJoinComment.LastIndexOf("\n"),
                beforeLeftJoinComment.LastIndexOf("\r")
            );
            string lineBeforeLeftComment = lastNewlineBeforeLeftComment >= 0 
                ? beforeLeftJoinComment.Substring(lastNewlineBeforeLeftComment).Trim()
                : beforeLeftJoinComment.Trim();
            
            Assert.IsFalse(lineBeforeLeftComment.Contains("JOIN") && lineBeforeLeftComment.Contains("/*"),
                $"Comment should NOT be trailing on the previous JOIN line. Line before comment: '{lineBeforeLeftComment}'. Generated SQL:\n{generatedSql}");
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestPreserveCommentsEnabled_CommentInWherePredicates()
        {
            // Test comments within WHERE clause predicates
            var sqlWithComments = @"SELECT * FROM orders
WHERE 
    -- Filter by active status
    status = 'active'
    /* Date range filter */
    AND order_date >= '2024-01-01'
    -- Exclude test orders
    AND is_test = 0
    -- Amount threshold
    AND amount > 100;";
            var parser = new TSql170Parser(true);
            var fragment = parser.Parse(new StringReader(sqlWithComments), out var errors);

            Assert.AreEqual(0, errors.Count);

            var generatorOptions = new SqlScriptGeneratorOptions
            {
                PreserveComments = true
            };
            var generator = new Sql170ScriptGenerator(generatorOptions);
            generator.GenerateScript(fragment, out var generatedSql);

            // WHERE predicate comments should be preserved
            Assert.IsTrue(generatedSql.Contains("-- Filter by active status"), 
                "Status filter comment should be preserved. Actual: " + generatedSql);
            Assert.IsTrue(generatedSql.Contains("/* Date range filter */"), 
                "Date range block comment should be preserved. Actual: " + generatedSql);
            Assert.IsTrue(generatedSql.Contains("-- Exclude test orders"), 
                "Test orders exclusion comment should be preserved. Actual: " + generatedSql);
            Assert.IsTrue(generatedSql.Contains("-- Amount threshold"), 
                "Amount threshold comment should be preserved. Actual: " + generatedSql);
            
            // Verify position: comments should appear before their respective predicates
            int statusCommentIdx = generatedSql.IndexOf("-- Filter by active status");
            int statusPredicateIdx = generatedSql.IndexOf("status", StringComparison.OrdinalIgnoreCase);
            int dateCommentIdx = generatedSql.IndexOf("/* Date range filter */");
            int datePredicateIdx = generatedSql.IndexOf("order_date", StringComparison.OrdinalIgnoreCase);
            int testCommentIdx = generatedSql.IndexOf("-- Exclude test orders");
            int testPredicateIdx = generatedSql.IndexOf("is_test", StringComparison.OrdinalIgnoreCase);
            int amountCommentIdx = generatedSql.IndexOf("-- Amount threshold");
            int amountPredicateIdx = generatedSql.IndexOf("amount", StringComparison.OrdinalIgnoreCase);
            
            Assert.IsTrue(statusCommentIdx < statusPredicateIdx, 
                $"Status comment should appear before status predicate. Comment at {statusCommentIdx}, predicate at {statusPredicateIdx}");
            Assert.IsTrue(dateCommentIdx < datePredicateIdx, 
                $"Date comment should appear before order_date predicate. Comment at {dateCommentIdx}, predicate at {datePredicateIdx}");
            Assert.IsTrue(testCommentIdx < testPredicateIdx, 
                $"Test orders comment should appear before is_test predicate. Comment at {testCommentIdx}, predicate at {testPredicateIdx}");
            Assert.IsTrue(amountCommentIdx < amountPredicateIdx, 
                $"Amount comment should appear before amount predicate. Comment at {amountCommentIdx}, predicate at {amountPredicateIdx}");
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestPreserveCommentsEnabled_CommentInGroupByHaving()
        {
            // Test comments in GROUP BY and HAVING clauses
            var sqlWithComments = @"SELECT department, COUNT(*) as emp_count
FROM employees
-- Group by department
GROUP BY department
/* Filter groups with more than 5 employees */
HAVING COUNT(*) > 5;";
            var parser = new TSql170Parser(true);
            var fragment = parser.Parse(new StringReader(sqlWithComments), out var errors);

            Assert.AreEqual(0, errors.Count);

            var generatorOptions = new SqlScriptGeneratorOptions
            {
                PreserveComments = true
            };
            var generator = new Sql170ScriptGenerator(generatorOptions);
            generator.GenerateScript(fragment, out var generatedSql);

            // GROUP BY and HAVING comments should be preserved
            Assert.IsTrue(generatedSql.Contains("-- Group by department"), 
                "GROUP BY comment should be preserved. Actual: " + generatedSql);
            Assert.IsTrue(generatedSql.Contains("/* Filter groups with more than 5 employees */"), 
                "HAVING block comment should be preserved. Actual: " + generatedSql);
            
            // Verify position: comments should appear before their respective clauses
            int groupByCommentIdx = generatedSql.IndexOf("-- Group by department");
            int groupByIdx = generatedSql.IndexOf("GROUP BY", StringComparison.OrdinalIgnoreCase);
            int havingCommentIdx = generatedSql.IndexOf("/* Filter groups with more than 5 employees */");
            int havingIdx = generatedSql.IndexOf("HAVING", StringComparison.OrdinalIgnoreCase);
            
            Assert.IsTrue(groupByCommentIdx < groupByIdx, 
                $"GROUP BY comment should appear before GROUP BY. Comment at {groupByCommentIdx}, GROUP BY at {groupByIdx}");
            Assert.IsTrue(havingCommentIdx < havingIdx, 
                $"HAVING comment should appear before HAVING. Comment at {havingCommentIdx}, HAVING at {havingIdx}");
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestPreserveCommentsEnabled_CommentInOrderBy()
        {
            // Test comments in ORDER BY clause
            var sqlWithComments = @"SELECT name, created_date, priority
FROM tasks
ORDER BY 
    -- Primary sort: highest priority first
    priority ASC,
    /* Secondary sort: newest first */
    created_date DESC;";
            var parser = new TSql170Parser(true);
            var fragment = parser.Parse(new StringReader(sqlWithComments), out var errors);

            Assert.AreEqual(0, errors.Count);

            var generatorOptions = new SqlScriptGeneratorOptions
            {
                PreserveComments = true
            };
            var generator = new Sql170ScriptGenerator(generatorOptions);
            generator.GenerateScript(fragment, out var generatedSql);

            // ORDER BY comments should be preserved
            Assert.IsTrue(generatedSql.Contains("-- Primary sort: highest priority first"), 
                "Primary sort comment should be preserved. Actual: " + generatedSql);
            Assert.IsTrue(generatedSql.Contains("/* Secondary sort: newest first */"), 
                "Secondary sort block comment should be preserved. Actual: " + generatedSql);
            
            // Verify position: comments should appear before their respective sort columns
            int primarySortCommentIdx = generatedSql.IndexOf("-- Primary sort: highest priority first");
            int orderByIdx = generatedSql.IndexOf("ORDER BY", StringComparison.OrdinalIgnoreCase);
            // Find priority AFTER ORDER BY (since it also appears in SELECT list)
            int priorityInOrderByIdx = generatedSql.IndexOf("priority", orderByIdx, StringComparison.OrdinalIgnoreCase);
            int secondarySortCommentIdx = generatedSql.IndexOf("/* Secondary sort: newest first */");
            // Find created_date AFTER ORDER BY (since it also appears in SELECT list)
            int createdDateInOrderByIdx = generatedSql.IndexOf("created_date", orderByIdx, StringComparison.OrdinalIgnoreCase);
            
            Assert.IsTrue(primarySortCommentIdx > orderByIdx && primarySortCommentIdx < priorityInOrderByIdx, 
                $"Primary sort comment should appear between ORDER BY and priority. ORDER BY at {orderByIdx}, Comment at {primarySortCommentIdx}, priority at {priorityInOrderByIdx}");
            Assert.IsTrue(secondarySortCommentIdx < createdDateInOrderByIdx, 
                $"Secondary sort comment should appear before created_date column. Comment at {secondarySortCommentIdx}, created_date at {createdDateInOrderByIdx}");
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestPreserveCommentsEnabled_CommentInUnionQueries()
        {
            // Test comments between UNION queries
            var sqlWithComments = @"-- First query: active users
SELECT id, name FROM users WHERE status = 'active'
/* Combine with archived users */
UNION ALL
-- Second query: archived users
SELECT id, name FROM archived_users;";
            var parser = new TSql170Parser(true);
            var fragment = parser.Parse(new StringReader(sqlWithComments), out var errors);

            Assert.AreEqual(0, errors.Count);

            var generatorOptions = new SqlScriptGeneratorOptions
            {
                PreserveComments = true
            };
            var generator = new Sql170ScriptGenerator(generatorOptions);
            generator.GenerateScript(fragment, out var generatedSql);

            // UNION query comments should be preserved
            Assert.IsTrue(generatedSql.Contains("-- First query: active users"), 
                "First query comment should be preserved. Actual: " + generatedSql);
            Assert.IsTrue(generatedSql.Contains("/* Combine with archived users */"), 
                "UNION block comment should be preserved. Actual: " + generatedSql);
            
            // CRITICAL ASSERTIONS: Comments must appear BEFORE UNION keyword
            // and NOT be on the same line as the previous SELECT statement
            
            int firstQueryCommentIdx = generatedSql.IndexOf("-- First query: active users");
            int firstSelectIdx = generatedSql.IndexOf("SELECT", StringComparison.OrdinalIgnoreCase);
            int unionCommentIdx = generatedSql.IndexOf("/* Combine with archived users */");
            int unionIdx = generatedSql.IndexOf("UNION", StringComparison.OrdinalIgnoreCase);
            
            Assert.IsTrue(firstQueryCommentIdx >= 0, "First query comment must exist");
            Assert.IsTrue(firstQueryCommentIdx < firstSelectIdx, 
                $"First query comment MUST appear before first SELECT. Comment at {firstQueryCommentIdx}, SELECT at {firstSelectIdx}. Generated SQL:\n{generatedSql}");
            
            Assert.IsTrue(unionCommentIdx >= 0, "UNION comment must exist");
            Assert.IsTrue(unionCommentIdx < unionIdx, 
                $"UNION comment MUST appear before UNION keyword. Comment at {unionCommentIdx}, UNION at {unionIdx}. Generated SQL:\n{generatedSql}");
            
            // Ensure UNION comment is on its own line BEFORE UNION, not trailing after previous SELECT
            string beforeUnionComment = generatedSql.Substring(0, unionCommentIdx);
            int lastNewlineBeforeUnionComment = Math.Max(
                beforeUnionComment.LastIndexOf("\n"),
                beforeUnionComment.LastIndexOf("\r")
            );
            string lineBeforeUnionComment = lastNewlineBeforeUnionComment >= 0 
                ? beforeUnionComment.Substring(lastNewlineBeforeUnionComment).Trim()
                : beforeUnionComment.Trim();
            
            Assert.IsFalse(lineBeforeUnionComment.Contains("SELECT") && lineBeforeUnionComment.Contains("/*"),
                $"UNION comment should NOT be trailing on the previous SELECT line. Line before comment: '{lineBeforeUnionComment}'. Generated SQL:\n{generatedSql}");
            
            // Verify there's a newline between the comment and UNION keyword
            string betweenCommentAndUnion = generatedSql.Substring(
                unionCommentIdx + "/* Combine with archived users */".Length,
                unionIdx - (unionCommentIdx + "/* Combine with archived users */".Length)
            );
            Assert.IsTrue(betweenCommentAndUnion.Contains("\n") || betweenCommentAndUnion.Contains("\r"),
                $"There must be a newline between comment and UNION. Text between: '{betweenCommentAndUnion}'. Generated SQL:\n{generatedSql}");
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestPreserveCommentsEnabled_CommentInNestedSubquery()
        {
            // Test comments in deeply nested subqueries
            var sqlWithComments = @"SELECT * FROM (
    -- Outer subquery
    SELECT * FROM (
        /* Inner subquery */
        SELECT id, name FROM users
    ) AS inner_q
) AS outer_q;";
            var parser = new TSql170Parser(true);
            var fragment = parser.Parse(new StringReader(sqlWithComments), out var errors);

            Assert.AreEqual(0, errors.Count);

            var generatorOptions = new SqlScriptGeneratorOptions
            {
                PreserveComments = true
            };
            var generator = new Sql170ScriptGenerator(generatorOptions);
            generator.GenerateScript(fragment, out var generatedSql);

            // Nested subquery comments should be preserved
            Assert.IsTrue(generatedSql.Contains("-- Outer subquery"), 
                "Outer subquery comment should be preserved. Actual: " + generatedSql);
            Assert.IsTrue(generatedSql.Contains("/* Inner subquery */"), 
                "Inner subquery block comment should be preserved. Actual: " + generatedSql);
            
            // Verify position: outer comment before inner comment (based on nesting order)
            int outerCommentIdx = generatedSql.IndexOf("-- Outer subquery");
            int innerCommentIdx = generatedSql.IndexOf("/* Inner subquery */");
            int innerQAliasIdx = generatedSql.IndexOf("inner_q", StringComparison.OrdinalIgnoreCase);
            int outerQAliasIdx = generatedSql.IndexOf("outer_q", StringComparison.OrdinalIgnoreCase);
            
            Assert.IsTrue(outerCommentIdx < innerCommentIdx, 
                $"Outer subquery comment should appear before inner subquery comment. Outer at {outerCommentIdx}, inner at {innerCommentIdx}");
            Assert.IsTrue(innerCommentIdx < innerQAliasIdx, 
                $"Inner subquery comment should appear before inner_q alias. Comment at {innerCommentIdx}, alias at {innerQAliasIdx}");
            Assert.IsTrue(innerQAliasIdx < outerQAliasIdx, 
                $"inner_q alias should appear before outer_q alias. inner_q at {innerQAliasIdx}, outer_q at {outerQAliasIdx}");
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestPreserveCommentsEnabled_SemicolonBeforeTrailingComment()
        {
            // Test that semicolons are placed BEFORE trailing single-line comments,
            // not after them (which would make the semicolon part of the comment text).
            // Bug fix: previously "SELECT 1 -- comment;" was generated instead of "SELECT 1; -- comment"
            var sqlWithComments = "SELECT 1 -- trailing comment";
            var parser = new TSql170Parser(true);
            var fragment = parser.Parse(new StringReader(sqlWithComments), out var errors);

            Assert.AreEqual(0, errors.Count);

            var generatorOptions = new SqlScriptGeneratorOptions
            {
                PreserveComments = true,
                IncludeSemicolons = true
            };
            var generator = new Sql170ScriptGenerator(generatorOptions);
            generator.GenerateScript(fragment, out var generatedSql);

            // The semicolon must appear BEFORE the trailing comment
            int semicolonIndex = generatedSql.IndexOf(";");
            int commentIndex = generatedSql.IndexOf("-- trailing comment");

            Assert.IsTrue(semicolonIndex >= 0, "Semicolon should be present in output. Actual: " + generatedSql);
            Assert.IsTrue(commentIndex >= 0, "Trailing comment should be preserved. Actual: " + generatedSql);
            Assert.IsTrue(semicolonIndex < commentIndex,
                $"Semicolon should appear before trailing comment. Semicolon at {semicolonIndex}, comment at {commentIndex}. Actual: " + generatedSql);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestPreserveCommentsEnabled_SemicolonBeforeTrailingComment_MultipleStatements()
        {
            // Test semicolon placement with multiple statements each having trailing comments
            var sqlWithComments = @"SELECT 1 -- first comment
SELECT 2 -- second comment";
            var parser = new TSql170Parser(true);
            var fragment = parser.Parse(new StringReader(sqlWithComments), out var errors);

            Assert.AreEqual(0, errors.Count);

            var generatorOptions = new SqlScriptGeneratorOptions
            {
                PreserveComments = true,
                IncludeSemicolons = true
            };
            var generator = new Sql170ScriptGenerator(generatorOptions);
            generator.GenerateScript(fragment, out var generatedSql);

            // Both comments should be preserved
            Assert.IsTrue(generatedSql.Contains("-- first comment"),
                "First trailing comment should be preserved. Actual: " + generatedSql);
            Assert.IsTrue(generatedSql.Contains("-- second comment"),
                "Second trailing comment should be preserved. Actual: " + generatedSql);

            // Verify semicolons appear before their respective comments, not after
            Assert.IsFalse(generatedSql.Contains("-- first comment;"),
                "Semicolon should not appear after first comment text. Actual: " + generatedSql);
            Assert.IsFalse(generatedSql.Contains("-- second comment;"),
                "Semicolon should not appear after second comment text. Actual: " + generatedSql);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestPreserveCommentsEnabled_SemicolonBeforeTrailingBlockComment()
        {
            // Test semicolon placement with trailing block comments
            var sqlWithComments = "SELECT 1 /* trailing block comment */";
            var parser = new TSql170Parser(true);
            var fragment = parser.Parse(new StringReader(sqlWithComments), out var errors);

            Assert.AreEqual(0, errors.Count);

            var generatorOptions = new SqlScriptGeneratorOptions
            {
                PreserveComments = true,
                IncludeSemicolons = true
            };
            var generator = new Sql170ScriptGenerator(generatorOptions);
            generator.GenerateScript(fragment, out var generatedSql);

            // The semicolon must appear BEFORE the trailing block comment
            int semicolonIndex = generatedSql.IndexOf(";");
            int commentIndex = generatedSql.IndexOf("/* trailing block comment */");

            Assert.IsTrue(semicolonIndex >= 0, "Semicolon should be present in output. Actual: " + generatedSql);
            Assert.IsTrue(commentIndex >= 0, "Trailing block comment should be preserved. Actual: " + generatedSql);
            Assert.IsTrue(semicolonIndex < commentIndex,
                $"Semicolon should appear before trailing block comment. Semicolon at {semicolonIndex}, comment at {commentIndex}. Actual: " + generatedSql);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestPreserveComments_StandaloneCommentNotAttachedAsTrailing()
        {
            // Regression: a comment on its own line (after a newline) between two
            // statements must remain a leading comment of the NEXT statement, not
            // be promoted to a trailing comment of the previous statement.
            var sqlWithComments =
                "SELECT Instructions\nFROM T;\n\n-- Now replace value of lot size\nUPDATE T SET Instructions = 1;";

            var parser = new TSql170Parser(true);
            var fragment = parser.Parse(new StringReader(sqlWithComments), out var errors);
            Assert.AreEqual(0, errors.Count);

            var generatorOptions = new SqlScriptGeneratorOptions
            {
                PreserveComments = true,
                IncludeSemicolons = true
            };
            var generator = new Sql170ScriptGenerator(generatorOptions);
            generator.GenerateScript(fragment, out var generatedSql);

            int firstSemicolonIndex = generatedSql.IndexOf(";");
            int commentIndex = generatedSql.IndexOf("-- Now replace value of lot size");
            int updateIndex = generatedSql.IndexOf("UPDATE", StringComparison.OrdinalIgnoreCase);

            Assert.IsTrue(commentIndex >= 0, "Comment should be preserved. Actual: " + generatedSql);
            Assert.IsTrue(updateIndex > commentIndex,
                $"Standalone comment should appear before UPDATE. Comment at {commentIndex}, UPDATE at {updateIndex}. Actual: " + generatedSql);
            Assert.IsTrue(firstSemicolonIndex >= 0 && firstSemicolonIndex < commentIndex,
                $"Semicolon for SELECT must come before the standalone comment. Semicolon at {firstSemicolonIndex}, comment at {commentIndex}. Actual: " + generatedSql);

            // The comment must not appear on the same line as 'FROM ... T;'.
            // The generator may pretty-print with extra spaces (e.g. 'FROM   T'),
            // so locate the 'FROM' keyword and require a newline between it and the comment.
            int fromIndex = generatedSql.IndexOf("FROM", StringComparison.OrdinalIgnoreCase);
            Assert.IsTrue(fromIndex >= 0, "FROM must be present in output. Actual: " + generatedSql);
            int newlineAfterFrom = generatedSql.IndexOf('\n', fromIndex);
            Assert.IsTrue(newlineAfterFrom > 0 && newlineAfterFrom < commentIndex,
                $"Standalone comment should be on its own line, not appended to the FROM line. Actual: " + generatedSql);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestPreserveComments_MultiLineCommentSpansLines_NextLineCommentNotTrailing()
        {
            // Edge case: a block comment that starts on the same line as the previous
            // statement but contains a newline must not pull in a subsequent line's
            // comment as if it were also trailing.
            var sql =
                "SELECT 1; /* spans\nlines */\n-- belongs to next\nSELECT 2;";

            var parser = new TSql170Parser(true);
            var fragment = parser.Parse(new StringReader(sql), out var errors);
            Assert.AreEqual(0, errors.Count);

            var generatorOptions = new SqlScriptGeneratorOptions
            {
                PreserveComments = true,
                IncludeSemicolons = true
            };
            var generator = new Sql170ScriptGenerator(generatorOptions);
            generator.GenerateScript(fragment, out var generatedSql);

            int spansIdx = generatedSql.IndexOf("spans");
            int nextIdx = generatedSql.IndexOf("-- belongs to next");
            int selectTwoIdx = generatedSql.IndexOf("SELECT 2", StringComparison.OrdinalIgnoreCase);

            Assert.IsTrue(spansIdx >= 0, "Multi-line comment should be preserved. Actual: " + generatedSql);
            Assert.IsTrue(nextIdx >= 0, "Following single-line comment should be preserved. Actual: " + generatedSql);
            Assert.IsTrue(nextIdx < selectTwoIdx,
                "Following comment must appear before SELECT 2 (i.e., as leading of next statement). Actual: " + generatedSql);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestPreserveComments_SameLineBlockThenSingleLine()
        {
            // Two trailing comments on the same line as the statement.
            var sql = "SELECT 1; /* block */ -- and single\nSELECT 2;";

            var parser = new TSql170Parser(true);
            var fragment = parser.Parse(new StringReader(sql), out var errors);
            Assert.AreEqual(0, errors.Count);

            var generatorOptions = new SqlScriptGeneratorOptions
            {
                PreserveComments = true,
                IncludeSemicolons = true
            };
            var generator = new Sql170ScriptGenerator(generatorOptions);
            generator.GenerateScript(fragment, out var generatedSql);

            int blockIdx = generatedSql.IndexOf("/* block */");
            int singleIdx = generatedSql.IndexOf("-- and single");
            int selectTwoIdx = generatedSql.IndexOf("SELECT 2", StringComparison.OrdinalIgnoreCase);

            Assert.IsTrue(blockIdx >= 0 && singleIdx >= 0, "Both comments should be preserved. Actual: " + generatedSql);
            Assert.IsTrue(blockIdx < singleIdx && singleIdx < selectTwoIdx,
                "Same-line trailing comments must precede the next statement. Actual: " + generatedSql);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestPreserveComments_SingleLineInsideArgListDoesNotAbsorbClosingTokens()
        {
            // Real-world pattern from RAISERROR docs: '--' comments inside an
            // argument list previously absorbed the ')' and ';' that followed,
            // producing un-reparseable output. The fix defers each '--' to a
            // safe end-of-line position.
            var sql = "RAISERROR('msg', -- text\n  16, -- severity\n  1 -- state\n);\n";

            var parser = new TSql170Parser(true);
            var fragment = parser.Parse(new StringReader(sql), out var errors);
            Assert.AreEqual(0, errors.Count);

            var generator = new Sql170ScriptGenerator(new SqlScriptGeneratorOptions
            {
                PreserveComments = true,
                IncludeSemicolons = true,
            });
            generator.GenerateScript(fragment, out var generatedSql);

            Assert.IsTrue(generatedSql.Contains("-- text"), "Actual: " + generatedSql);
            Assert.IsTrue(generatedSql.Contains("-- severity"), "Actual: " + generatedSql);
            Assert.IsTrue(generatedSql.Contains("-- state"), "Actual: " + generatedSql);

            var reparser = new TSql170Parser(true);
            reparser.Parse(new StringReader(generatedSql), out var reparseErrors);
            Assert.AreEqual(0, reparseErrors.Count,
                "Generated SQL must reparse without errors. Actual: " + generatedSql);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestPreserveComments_LeadingCommentAfterGoBatch()
        {
            // Real-world pattern: a '--' comment between GO and the next batch
            // must remain a leading comment of that batch, not be absorbed onto
            // the prior batch's last line.
            var sql = "SELECT 1;\nGO\n-- leading before next batch\nSELECT 2;\n";

            var parser = new TSql170Parser(true);
            var fragment = parser.Parse(new StringReader(sql), out var errors);
            Assert.AreEqual(0, errors.Count);

            var generator = new Sql170ScriptGenerator(new SqlScriptGeneratorOptions
            {
                PreserveComments = true,
                IncludeSemicolons = true,
            });
            generator.GenerateScript(fragment, out var generatedSql);

            int commentIdx = generatedSql.IndexOf("-- leading before next batch");
            int selectTwoIdx = generatedSql.IndexOf("SELECT 2", StringComparison.OrdinalIgnoreCase);
            int selectOneIdx = generatedSql.IndexOf("SELECT 1", StringComparison.OrdinalIgnoreCase);

            Assert.IsTrue(commentIdx > selectOneIdx && commentIdx < selectTwoIdx,
                "Comment must appear between the two batches. Actual: " + generatedSql);

            var reparser = new TSql170Parser(true);
            reparser.Parse(new StringReader(generatedSql), out var reparseErrors);
            Assert.AreEqual(0, reparseErrors.Count, "Generated SQL must reparse. Actual: " + generatedSql);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestPreserveCommentsEnabled_MultipleCommentsInTableDefinition()
        {
            var sqlWithComments = @"CREATE TABLE dbo.VETask 
(
Id INT,
TaskNo INT,
Status INT,
--IsActive BIT NOT NULL CONSTRAINT DF_VETask_IsActive DEFAULT 1
CONSTRAINT PK_VETask PRIMARY KEY CLUSTERED (Id),
--CONSTRAINT UQ_VETask_TaskNo UNIQUE (TaskNo)
INDEX IX_VETask_Status NONCLUSTERED (Status)
);";
            var parser = new TSql170Parser(true);
            var fragment = parser.Parse(new StringReader(sqlWithComments), out var errors);

            Assert.AreEqual(0, errors.Count, "Input SQL should parse without errors");

            var generatorOptions = new SqlScriptGeneratorOptions
            {
                PreserveComments = true
            };
            var generator = new Sql170ScriptGenerator(generatorOptions);
            generator.GenerateScript(fragment, out var generatedSql);

            // CRITICAL ASSERTIONS: Comments must NOT merge with previous lines
            // and CONSTRAINT must NOT merge with "Status INT" line
            
            // 1. Comment should NOT be trailing on Status INT line
            Assert.IsFalse(generatedSql.Contains("Status INT --IsActive"), 
                "Comment should NOT be trailing on Status INT line (no comma)");
            Assert.IsFalse(generatedSql.Contains("Status INT, --IsActive"),
                "Comment should NOT be trailing on Status INT line (with comma)");
            Assert.IsFalse(generatedSql.Contains("Status INT,  --IsActive"),
                "Comment should NOT be trailing on Status INT line (with comma and space)");
            
            // 2. CONSTRAINT must NOT be on the same line as Status INT
            Assert.IsFalse(generatedSql.Contains("Status INT CONSTRAINT"),
                $"CONSTRAINT keyword must NOT merge with Status INT line. Generated SQL:\n{generatedSql}");
            Assert.IsFalse(generatedSql.Contains("Status INT, CONSTRAINT"),
                $"CONSTRAINT keyword must NOT be on same line as Status INT (with comma). Generated SQL:\n{generatedSql}");
                
            // 3. Comment must appear BEFORE CONSTRAINT on its own line
            int commentIdx = generatedSql.IndexOf("--IsActive BIT NOT NULL");
            int constraintIdx = generatedSql.IndexOf("CONSTRAINT PK_VETask", StringComparison.OrdinalIgnoreCase);
            
            Assert.IsTrue(commentIdx >= 0, "Comment for IsActive column must exist");
            Assert.IsTrue(constraintIdx >= 0, "CONSTRAINT PK_VETask must exist");
            Assert.IsTrue(commentIdx < constraintIdx,
                $"Comment must appear BEFORE CONSTRAINT. Comment at {commentIdx}, CONSTRAINT at {constraintIdx}. Generated SQL:\n{generatedSql}");
            
            // 4. Verify Status INT is on a separate line from the comment
            int statusIdx = generatedSql.IndexOf("Status INT", StringComparison.OrdinalIgnoreCase);
            Assert.IsTrue(statusIdx >= 0, "Status INT column must exist");
            
            // Extract the line containing "Status INT"
            int lineStartIdx = statusIdx;
            while (lineStartIdx > 0 && generatedSql[lineStartIdx - 1] != '\n' && generatedSql[lineStartIdx - 1] != '\r')
            {
                lineStartIdx--;
            }
            int lineEndIdx = statusIdx;
            while (lineEndIdx < generatedSql.Length && generatedSql[lineEndIdx] != '\n' && generatedSql[lineEndIdx] != '\r')
            {
                lineEndIdx++;
            }
            string statusLine = generatedSql.Substring(lineStartIdx, lineEndIdx - lineStartIdx).Trim();
            
            Assert.IsFalse(statusLine.Contains("--"),
                $"Status INT line must NOT contain the comment. Actual line: '{statusLine}'. Generated SQL:\n{generatedSql}");
            Assert.IsFalse(statusLine.Contains("CONSTRAINT"),
                $"Status INT line must NOT contain CONSTRAINT keyword. Actual line: '{statusLine}'. Generated SQL:\n{generatedSql}");

            var reparser = new TSql170Parser(true);
            reparser.Parse(new StringReader(generatedSql), out var reparseErrors);
            Assert.AreEqual(0, reparseErrors.Count, $"Generated SQL must reparse without errors. Generated SQL:\n{generatedSql}");
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestPreserveComments_SingleLineCommentsInsideIfBeginEndBlock()
        {
            // Real-world pattern from sql-docs IF/BEGIN/END examples: '--'
            // comments interleaved between statements inside a BEGIN block
            // must each appear before their associated SELECT in the output.
            var sql =
                "IF (1 = 1)\n" +
                "BEGIN\n" +
                "    -- inside if\n" +
                "    SELECT 1;\n" +
                "    -- after first stmt inside if\n" +
                "    SELECT 2;\n" +
                "END;\n";

            var parser = new TSql170Parser(true);
            var fragment = parser.Parse(new StringReader(sql), out var errors);
            Assert.AreEqual(0, errors.Count);

            var generator = new Sql170ScriptGenerator(new SqlScriptGeneratorOptions
            {
                PreserveComments = true,
                IncludeSemicolons = true,
            });
            generator.GenerateScript(fragment, out var generatedSql);

            int insideIdx = generatedSql.IndexOf("-- inside if");
            int afterIdx = generatedSql.IndexOf("-- after first stmt inside if");
            int select1Idx = generatedSql.IndexOf("SELECT 1", StringComparison.OrdinalIgnoreCase);
            int select2Idx = generatedSql.IndexOf("SELECT 2", StringComparison.OrdinalIgnoreCase);

            Assert.IsTrue(insideIdx >= 0 && insideIdx < select1Idx,
                "'-- inside if' must precede SELECT 1. Actual: " + generatedSql);
            Assert.IsTrue(afterIdx > select1Idx && afterIdx < select2Idx,
                "'-- after first stmt' must be between SELECT 1 and SELECT 2. Actual: " + generatedSql);

            var reparser = new TSql170Parser(true);
            reparser.Parse(new StringReader(generatedSql), out var reparseErrors);
            Assert.AreEqual(0, reparseErrors.Count, "Generated SQL must reparse. Actual: " + generatedSql);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestPreserveComments_BeginEndBlockPreservesCommentBeforeEnd()
        {
            // Real-world pattern: comments placed between the last statement
            // in a BEGIN block and the END keyword must be preserved.
            var sql =
                "BEGIN\n" +
                "    SELECT 1;\n" +
                "    /* between stmt and closer */\n" +
                "    -- also a trailing note\n" +
                "END;\n";

            var parser = new TSql170Parser(true);
            var fragment = parser.Parse(new StringReader(sql), out var errors);
            Assert.AreEqual(0, errors.Count);

            var generator = new Sql170ScriptGenerator(new SqlScriptGeneratorOptions
            {
                PreserveComments = true,
                IncludeSemicolons = true,
            });
            generator.GenerateScript(fragment, out var generatedSql);

            Assert.IsTrue(generatedSql.Contains("/* between stmt and closer */"),
                "Block comment before END must be preserved. Actual: " + generatedSql);
            Assert.IsTrue(generatedSql.Contains("-- also a trailing note"),
                "Line comment before END must be preserved. Actual: " + generatedSql);

            int endIdx = generatedSql.IndexOf("END", StringComparison.Ordinal);
            int blockIdx = generatedSql.IndexOf("/* between stmt and closer */");
            int lineIdx = generatedSql.IndexOf("-- also a trailing note");
            Assert.IsTrue(blockIdx >= 0 && blockIdx < endIdx,
                "Block comment must appear before END. Actual: " + generatedSql);
            Assert.IsTrue(lineIdx >= 0 && lineIdx < endIdx,
                "Line comment must appear before END. Actual: " + generatedSql);

            var reparser = new TSql170Parser(true);
            reparser.Parse(new StringReader(generatedSql), out var reparseErrors);
            Assert.AreEqual(0, reparseErrors.Count, "Generated SQL must reparse. Actual: " + generatedSql);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestPreserveComments_StandaloneBlockCommentBetweenStatementsPreserved()
        {
            // '/* lonely */;' parses as an empty statement absorbed into the
            // previous statement's token range. Without a sweep through the
            // statement's range, the comment was dropped.
            var sql = "SELECT 1;\n/* lonely */;\nSELECT 2;\n";

            var parser = new TSql170Parser(true);
            var fragment = parser.Parse(new StringReader(sql), out var errors);
            Assert.AreEqual(0, errors.Count);

            var generator = new Sql170ScriptGenerator(new SqlScriptGeneratorOptions
            {
                PreserveComments = true,
                IncludeSemicolons = true,
            });
            generator.GenerateScript(fragment, out var generatedSql);

            Assert.IsTrue(generatedSql.Contains("/* lonely */"),
                "Standalone block comment must be preserved. Actual: " + generatedSql);

            int select1Idx = generatedSql.IndexOf("SELECT 1", StringComparison.OrdinalIgnoreCase);
            int commentIdx = generatedSql.IndexOf("/* lonely */");
            int select2Idx = generatedSql.IndexOf("SELECT 2", StringComparison.OrdinalIgnoreCase);
            Assert.IsTrue(select1Idx < commentIdx && commentIdx < select2Idx,
                "Standalone comment must appear between the two SELECT statements. Actual: " + generatedSql);

            var reparser = new TSql170Parser(true);
            reparser.Parse(new StringReader(generatedSql), out var reparseErrors);
            Assert.AreEqual(0, reparseErrors.Count, "Generated SQL must reparse. Actual: " + generatedSql);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestPreserveComments_LeadingCommentBeforeSemicolonWithCte()
        {
            // The leading ';' of ';WITH cte ...' is parsed as part of the
            // previous statement's token range. A leading comment that sits
            // between the prior statement's terminator and the ';WITH' was
            // previously dropped.
            var sql = "SELECT 1;\n-- before with\n;WITH cte AS (SELECT 1 AS a) SELECT a FROM cte;\n";

            var parser = new TSql170Parser(true);
            var fragment = parser.Parse(new StringReader(sql), out var errors);
            Assert.AreEqual(0, errors.Count);

            var generator = new Sql170ScriptGenerator(new SqlScriptGeneratorOptions
            {
                PreserveComments = true,
                IncludeSemicolons = true,
            });
            generator.GenerateScript(fragment, out var generatedSql);

            Assert.IsTrue(generatedSql.Contains("-- before with"),
                "Leading comment before ';WITH' must be preserved. Actual: " + generatedSql);

            int select1Idx = generatedSql.IndexOf("SELECT 1", StringComparison.OrdinalIgnoreCase);
            int commentIdx = generatedSql.IndexOf("-- before with");
            int withIdx = generatedSql.IndexOf("WITH", StringComparison.OrdinalIgnoreCase);
            Assert.IsTrue(select1Idx < commentIdx && commentIdx < withIdx,
                "Comment must appear between SELECT 1 and WITH. Actual: " + generatedSql);

            var reparser = new TSql170Parser(true);
            reparser.Parse(new StringReader(generatedSql), out var reparseErrors);
            Assert.AreEqual(0, reparseErrors.Count, "Generated SQL must reparse. Actual: " + generatedSql);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestPreserveComments_MultiLineBlockCommentBetweenStatements()
        {
            // Real-world pattern: a '/* ... */' block comment that itself
            // spans multiple source lines, placed between two statements.
            // Exercises the ContainsLineBreak path for block-comment tokens.
            var sql =
                "SELECT 1;\n" +
                "/* multi-line\n" +
                "   comment\n" +
                "   spans three lines */\n" +
                "SELECT 2;\n";

            var parser = new TSql170Parser(true);
            var fragment = parser.Parse(new StringReader(sql), out var errors);
            Assert.AreEqual(0, errors.Count);

            var generator = new Sql170ScriptGenerator(new SqlScriptGeneratorOptions
            {
                PreserveComments = true,
                IncludeSemicolons = true,
            });
            generator.GenerateScript(fragment, out var generatedSql);

            Assert.IsTrue(generatedSql.Contains("multi-line") &&
                          generatedSql.Contains("spans three lines"),
                "Full text of the multi-line block comment must be preserved. Actual: " + generatedSql);

            int select1Idx = generatedSql.IndexOf("SELECT 1", StringComparison.OrdinalIgnoreCase);
            int blockIdx = generatedSql.IndexOf("/* multi-line");
            int select2Idx = generatedSql.IndexOf("SELECT 2", StringComparison.OrdinalIgnoreCase);
            Assert.IsTrue(select1Idx < blockIdx && blockIdx < select2Idx,
                "Multi-line block must appear between the two SELECTs. Actual: " + generatedSql);

            var reparser = new TSql170Parser(true);
            reparser.Parse(new StringReader(generatedSql), out var reparseErrors);
            Assert.AreEqual(0, reparseErrors.Count, "Generated SQL must reparse. Actual: " + generatedSql);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestPreserveComments_RealWorldXmlModifyBatchFromDocs()
        {
            // End-to-end regression for the pattern that produced the original
            // bug screenshot: a series of UPDATE statements that call
            // XML .modify(...) with long string-literal arguments, separated
            // by '--' leading comments. The leading comment of the LAST UPDATE
            // ('-- Now replace value of lot size') was being absorbed onto the
            // previous statement's trailing 'FROM T;' line.
            var sql =
                "SELECT Instructions\n" +
                "FROM T;\n" +
                "\n" +
                "-- Now replace value of lot size\n" +
                "UPDATE T\n" +
                "SET Instructions = 1;\n";

            var parser = new TSql170Parser(true);
            var fragment = parser.Parse(new StringReader(sql), out var errors);
            Assert.AreEqual(0, errors.Count);

            var generator = new Sql170ScriptGenerator(new SqlScriptGeneratorOptions
            {
                PreserveComments = true,
                IncludeSemicolons = true,
            });
            generator.GenerateScript(fragment, out var generatedSql);

            int fromTIdx = generatedSql.IndexOf("FROM   T;", StringComparison.Ordinal);
            if (fromTIdx < 0) fromTIdx = generatedSql.IndexOf("FROM T;", StringComparison.Ordinal);
            int commentIdx = generatedSql.IndexOf("-- Now replace value of lot size");
            int updateIdx = generatedSql.IndexOf("UPDATE", StringComparison.OrdinalIgnoreCase);

            Assert.IsTrue(commentIdx >= 0, "Leading comment must be preserved. Actual: " + generatedSql);
            Assert.IsTrue(fromTIdx >= 0 && fromTIdx < commentIdx,
                "Comment must appear AFTER 'FROM T;'. Actual: " + generatedSql);
            Assert.IsTrue(commentIdx < updateIdx,
                "Comment must appear BEFORE 'UPDATE'. Actual: " + generatedSql);

            // The original bug: the comment was attached as trailing to FROM,
            // making the FROM line read 'FROM T; -- Now replace value of lot size'.
            int fromLineEnd = generatedSql.IndexOf('\n', fromTIdx);
            Assert.IsTrue(fromLineEnd > 0 && fromLineEnd < commentIdx,
                "Comment must be on a separate line from 'FROM T;'. Actual: " + generatedSql);

            var reparser = new TSql170Parser(true);
            reparser.Parse(new StringReader(generatedSql), out var reparseErrors);
            Assert.AreEqual(0, reparseErrors.Count, "Generated SQL must reparse. Actual: " + generatedSql);
        }

        #endregion

        #region CommaPlacement

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
            var sql = "SELECT a, b, c FROM t;";
            var parser = new TSql170Parser(true);
            var fragment = parser.Parse(new StringReader(sql), out var errors);
            Assert.AreEqual(0, errors.Count);

            var generator = new Sql170ScriptGenerator(new SqlScriptGeneratorOptions
            {
                CommaPlacement = CommaPlacement.Leading
            });
            generator.GenerateScript(fragment, out var generated);

            // Leading comma style for a keyword-aligned list: the columns stay aligned with the
            // first element and each comma is placed two columns before them.
            string expected =
                "SELECT a" + Environment.NewLine +
                "     , b" + Environment.NewLine +
                "     , c" + Environment.NewLine +
                "FROM   t;" + Environment.NewLine + Environment.NewLine;
            Assert.AreEqual(expected, generated);

            var reparser = new TSql170Parser(true);
            reparser.Parse(new StringReader(generated), out var reErrors);
            Assert.AreEqual(0, reErrors.Count, "Generated SQL must reparse. Actual:\n" + generated);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCommaPlacementTrailingSelectList()
        {
            var sql = "SELECT a, b, c FROM t;";
            var parser = new TSql170Parser(true);
            var fragment = parser.Parse(new StringReader(sql), out var errors);
            Assert.AreEqual(0, errors.Count);

            var generator = new Sql170ScriptGenerator(new SqlScriptGeneratorOptions
            {
                CommaPlacement = CommaPlacement.Trailing
            });
            generator.GenerateScript(fragment, out var generated);

            // Trailing comma style (default): the comma is placed at the end of each line.
            string expected =
                "SELECT a," + Environment.NewLine +
                "       b," + Environment.NewLine +
                "       c" + Environment.NewLine +
                "FROM   t;" + Environment.NewLine + Environment.NewLine;
            Assert.AreEqual(expected, generated);

            var reparser = new TSql170Parser(true);
            reparser.Parse(new StringReader(generated), out var reErrors);
            Assert.AreEqual(0, reErrors.Count, "Generated SQL must reparse. Actual:\n" + generated);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCommaPlacementLeadingParenthesizedList()
        {
            var sql = "CREATE TABLE t (a INT, b INT, c INT);";
            var parser = new TSql170Parser(true);
            var fragment = parser.Parse(new StringReader(sql), out var errors);
            Assert.AreEqual(0, errors.Count);

            var generator = new Sql170ScriptGenerator(new SqlScriptGeneratorOptions
            {
                CommaPlacement = CommaPlacement.Leading
            });
            generator.GenerateScript(fragment, out var generated);

            // Leading comma style in a parenthesized (CREATE TABLE) column list.
            // Elements stay at the list indentation level (4); the comma is indented two
            // characters fewer (column 2), per the CommaPlacement=Leading rule.
            string expected =
                "CREATE TABLE t (" + Environment.NewLine +
                "    a INT" + Environment.NewLine +
                "  , b INT" + Environment.NewLine +
                "  , c INT" + Environment.NewLine +
                ");" + Environment.NewLine + Environment.NewLine;
            Assert.AreEqual(expected, generated);

            var reparser = new TSql170Parser(true);
            reparser.Parse(new StringReader(generated), out var reErrors);
            Assert.AreEqual(0, reErrors.Count, "Generated SQL must reparse. Actual:\n" + generated);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCommaPlacementTrailingParenthesizedList()
        {
            var sql = "CREATE TABLE t (a INT, b INT, c INT);";
            var parser = new TSql170Parser(true);
            var fragment = parser.Parse(new StringReader(sql), out var errors);
            Assert.AreEqual(0, errors.Count);

            var generator = new Sql170ScriptGenerator(new SqlScriptGeneratorOptions
            {
                CommaPlacement = CommaPlacement.Trailing
            });
            generator.GenerateScript(fragment, out var generated);

            // Trailing comma style (default) in a parenthesized (CREATE TABLE) column list:
            // each comma follows the element on the same line and the elements stay at the
            // list indentation level (4).
            string expected =
                "CREATE TABLE t (" + Environment.NewLine +
                "    a INT," + Environment.NewLine +
                "    b INT," + Environment.NewLine +
                "    c INT" + Environment.NewLine +
                ");" + Environment.NewLine + Environment.NewLine;
            Assert.AreEqual(expected, generated);

            var reparser = new TSql170Parser(true);
            reparser.Parse(new StringReader(generated), out var reErrors);
            Assert.AreEqual(0, reErrors.Count, "Generated SQL must reparse. Actual:\n" + generated);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCommaPlacementLeadingInsertTargets()
        {
            // With MultilineInsertTargetsList = true the INSERT column target list is emitted as a
            // multi-line parenthesized list (like CREATE TABLE / VIEW columns): each column on its
            // own line indented one level. CommaPlacement = Leading places each column's comma at
            // the start of its line (indented two characters fewer than the column).
            var sql = "INSERT INTO t (a, b, c) VALUES (1, 2, 3);";
            var parser = new TSql170Parser(true);
            var fragment = parser.Parse(new StringReader(sql), out var errors);
            Assert.AreEqual(0, errors.Count);

            var generator = new Sql170ScriptGenerator(new SqlScriptGeneratorOptions
            {
                CommaPlacement = CommaPlacement.Leading,
                MultilineInsertTargetsList = true
            });
            generator.GenerateScript(fragment, out var generated);

            string expected =
                "INSERT  INTO t (" + Environment.NewLine +
                "    a" + Environment.NewLine +
                "  , b" + Environment.NewLine +
                "  , c" + Environment.NewLine +
                ")" + Environment.NewLine +
                "VALUES        (1, 2, 3);" + Environment.NewLine + Environment.NewLine;
            Assert.AreEqual(expected, generated);

            var reparser = new TSql170Parser(true);
            reparser.Parse(new StringReader(generated), out var reErrors);
            Assert.AreEqual(0, reErrors.Count, "Generated SQL must reparse. Actual:\n" + generated);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCommaPlacementTrailingInsertTargets()
        {
            // With MultilineInsertTargetsList = true and CommaPlacement = Trailing (default) the
            // INSERT column target list is emitted multi-line with each column on its own line and
            // its comma trailing the column.
            var sql = "INSERT INTO t (a, b, c) VALUES (1, 2, 3);";
            var parser = new TSql170Parser(true);
            var fragment = parser.Parse(new StringReader(sql), out var errors);
            Assert.AreEqual(0, errors.Count);

            var generator = new Sql170ScriptGenerator(new SqlScriptGeneratorOptions
            {
                CommaPlacement = CommaPlacement.Trailing,
                MultilineInsertTargetsList = true
            });
            generator.GenerateScript(fragment, out var generated);

            string expected =
                "INSERT  INTO t (" + Environment.NewLine +
                "    a," + Environment.NewLine +
                "    b," + Environment.NewLine +
                "    c" + Environment.NewLine +
                ")" + Environment.NewLine +
                "VALUES        (1, 2, 3);" + Environment.NewLine + Environment.NewLine;
            Assert.AreEqual(expected, generated);

            var reparser = new TSql170Parser(true);
            reparser.Parse(new StringReader(generated), out var reErrors);
            Assert.AreEqual(0, reErrors.Count, "Generated SQL must reparse. Actual:\n" + generated);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCommaPlacementLeadingInsertSources()
        {
            // The INSERT source (VALUES) row list is a multi-line comma-separated list, so
            // CommaPlacement = Leading places each continuation row's comma at the start of its
            // line. (The rows are not aligned under the first row: this list is emitted via the
            // newline comma-list path, so continuation rows begin at column 0.) The target column
            // list is emitted multi-line because MultilineInsertTargetsList is explicitly enabled
            // (it is no longer the default).
            var sql = "INSERT INTO t (a, b, c) VALUES (1, 2, 3), (4, 5, 6), (7, 8, 9);";
            var parser = new TSql170Parser(true);
            var fragment = parser.Parse(new StringReader(sql), out var errors);
            Assert.AreEqual(0, errors.Count);

            var generator = new Sql170ScriptGenerator(new SqlScriptGeneratorOptions
            {
                CommaPlacement = CommaPlacement.Leading,
                MultilineInsertSourcesList = true,
                MultilineInsertTargetsList = true
            });
            generator.GenerateScript(fragment, out var generated);

            string expected =
                "INSERT  INTO t (" + Environment.NewLine +
                "    a" + Environment.NewLine +
                "  , b" + Environment.NewLine +
                "  , c" + Environment.NewLine +
                ")" + Environment.NewLine +
                "VALUES        (1, 2, 3)" + Environment.NewLine +
                ", (4, 5, 6)" + Environment.NewLine +
                ", (7, 8, 9);" + Environment.NewLine + Environment.NewLine;
            Assert.AreEqual(expected, generated);

            var reparser = new TSql170Parser(true);
            reparser.Parse(new StringReader(generated), out var reErrors);
            Assert.AreEqual(0, reErrors.Count, "Generated SQL must reparse. Actual:\n" + generated);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCommaPlacementTrailingInsertSources()
        {
            // The INSERT source (VALUES) row list with CommaPlacement = Trailing (default):
            // each row's comma follows it at the end of the line. The target column list stays
            // on a single line because MultilineInsertTargetsList is left at its default (false),
            // exercising the common case of enabling only the source list.
            var sql = "INSERT INTO t (a, b, c) VALUES (1, 2, 3), (4, 5, 6), (7, 8, 9);";
            var parser = new TSql170Parser(true);
            var fragment = parser.Parse(new StringReader(sql), out var errors);
            Assert.AreEqual(0, errors.Count);

            var generator = new Sql170ScriptGenerator(new SqlScriptGeneratorOptions
            {
                CommaPlacement = CommaPlacement.Trailing,
                MultilineInsertSourcesList = true
            });
            generator.GenerateScript(fragment, out var generated);

            string expected =
                "INSERT  INTO t (a, b, c)" + Environment.NewLine +
                "VALUES        (1, 2, 3)," + Environment.NewLine +
                "(4, 5, 6)," + Environment.NewLine +
                "(7, 8, 9);" + Environment.NewLine + Environment.NewLine;
            Assert.AreEqual(expected, generated);

            var reparser = new TSql170Parser(true);
            reparser.Parse(new StringReader(generated), out var reErrors);
            Assert.AreEqual(0, reErrors.Count, "Generated SQL must reparse. Actual:\n" + generated);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCommaPlacementLeadingViewColumns()
        {
            // The CREATE VIEW column list is a parenthesized, indented list (like CREATE TABLE):
            // with CommaPlacement = Leading the columns stay at the list indentation level (4)
            // and each comma is indented two characters fewer (column 2). The SELECT list in the
            // view body is keyword-aligned, so its leading commas sit two columns before the
            // aligned expression column.
            var sql = "CREATE VIEW v (a, b, c) AS SELECT 1, 2, 3;";
            var parser = new TSql170Parser(true);
            var fragment = parser.Parse(new StringReader(sql), out var errors);
            Assert.AreEqual(0, errors.Count);

            var generator = new Sql170ScriptGenerator(new SqlScriptGeneratorOptions
            {
                CommaPlacement = CommaPlacement.Leading,
                MultilineViewColumnsList = true
            });
            generator.GenerateScript(fragment, out var generated);

            string expected =
                "CREATE VIEW v (" + Environment.NewLine +
                "    a" + Environment.NewLine +
                "  , b" + Environment.NewLine +
                "  , c" + Environment.NewLine +
                ")" + Environment.NewLine +
                "AS" + Environment.NewLine +
                "SELECT 1" + Environment.NewLine +
                "     , 2" + Environment.NewLine +
                "     , 3;" + Environment.NewLine + Environment.NewLine;
            Assert.AreEqual(expected, generated);

            var reparser = new TSql170Parser(true);
            reparser.Parse(new StringReader(generated), out var reErrors);
            Assert.AreEqual(0, reErrors.Count, "Generated SQL must reparse. Actual:\n" + generated);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCommaPlacementTrailingViewColumns()
        {
            // Trailing comma style (default): the CREATE VIEW column list keeps each comma on the
            // element's line at the list indentation level (4), and the SELECT list in the view
            // body places each comma at the end of its line.
            var sql = "CREATE VIEW v (a, b, c) AS SELECT 1, 2, 3;";
            var parser = new TSql170Parser(true);
            var fragment = parser.Parse(new StringReader(sql), out var errors);
            Assert.AreEqual(0, errors.Count);

            var generator = new Sql170ScriptGenerator(new SqlScriptGeneratorOptions
            {
                CommaPlacement = CommaPlacement.Trailing,
                MultilineViewColumnsList = true
            });
            generator.GenerateScript(fragment, out var generated);

            string expected =
                "CREATE VIEW v (" + Environment.NewLine +
                "    a," + Environment.NewLine +
                "    b," + Environment.NewLine +
                "    c" + Environment.NewLine +
                ")" + Environment.NewLine +
                "AS" + Environment.NewLine +
                "SELECT 1," + Environment.NewLine +
                "       2," + Environment.NewLine +
                "       3;" + Environment.NewLine + Environment.NewLine;
            Assert.AreEqual(expected, generated);

            var reparser = new TSql170Parser(true);
            reparser.Parse(new StringReader(generated), out var reErrors);
            Assert.AreEqual(0, reErrors.Count, "Generated SQL must reparse. Actual:\n" + generated);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCommaPlacementLeadingSetClauseItems()
        {
            // The UPDATE SET item list is keyword-aligned: with CommaPlacement = Leading the items
            // stay aligned with the first item and each comma is placed two columns before them
            // (the '=' signs remain aligned via a separate alignment point), matching the SELECT
            // list behavior.
            var sql = "UPDATE t SET a = 1, b = 2, c = 3;";
            var parser = new TSql170Parser(true);
            var fragment = parser.Parse(new StringReader(sql), out var errors);
            Assert.AreEqual(0, errors.Count);

            var generator = new Sql170ScriptGenerator(new SqlScriptGeneratorOptions
            {
                CommaPlacement = CommaPlacement.Leading,
                MultilineSetClauseItems = true
            });
            generator.GenerateScript(fragment, out var generated);

            string expected =
                "UPDATE t" + Environment.NewLine +
                "SET    a = 1" + Environment.NewLine +
                "     , b = 2" + Environment.NewLine +
                "     , c = 3;" + Environment.NewLine + Environment.NewLine;
            Assert.AreEqual(expected, generated);

            var reparser = new TSql170Parser(true);
            reparser.Parse(new StringReader(generated), out var reErrors);
            Assert.AreEqual(0, reErrors.Count, "Generated SQL must reparse. Actual:\n" + generated);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCommaPlacementTrailingSetClauseItems()
        {
            // Trailing comma style (default): the UPDATE SET items stay aligned with the first
            // item and each comma follows the item at the end of its line.
            var sql = "UPDATE t SET a = 1, b = 2, c = 3;";
            var parser = new TSql170Parser(true);
            var fragment = parser.Parse(new StringReader(sql), out var errors);
            Assert.AreEqual(0, errors.Count);

            var generator = new Sql170ScriptGenerator(new SqlScriptGeneratorOptions
            {
                CommaPlacement = CommaPlacement.Trailing,
                MultilineSetClauseItems = true
            });
            generator.GenerateScript(fragment, out var generated);

            string expected =
                "UPDATE t" + Environment.NewLine +
                "SET    a = 1," + Environment.NewLine +
                "       b = 2," + Environment.NewLine +
                "       c = 3;" + Environment.NewLine + Environment.NewLine;
            Assert.AreEqual(expected, generated);

            var reparser = new TSql170Parser(true);
            reparser.Parse(new StringReader(generated), out var reErrors);
            Assert.AreEqual(0, reErrors.Count, "Generated SQL must reparse. Actual:\n" + generated);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCommaPlacementLeadingSingleColumnHasNoComma()
        {
            // A list with a single element must never emit a comma regardless of placement.
            var sql = "SELECT a FROM t;";
            var parser = new TSql170Parser(true);
            var fragment = parser.Parse(new StringReader(sql), out var errors);
            Assert.AreEqual(0, errors.Count);

            var generator = new Sql170ScriptGenerator(new SqlScriptGeneratorOptions
            {
                CommaPlacement = CommaPlacement.Leading
            });
            generator.GenerateScript(fragment, out var generated);

            // A list with a single element must never emit a comma regardless of placement.
            string expected =
                "SELECT a" + Environment.NewLine +
                "FROM   t;" + Environment.NewLine + Environment.NewLine;
            Assert.AreEqual(expected, generated);

            var reparser = new TSql170Parser(true);
            reparser.Parse(new StringReader(generated), out var reErrors);
            Assert.AreEqual(0, reErrors.Count, "Generated SQL must reparse. Actual:\n" + generated);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCommaPlacementLeadingWithPreserveCommentsDoesNotAbsorbComma()
        {
            // Interaction rule (FeedbackTicket 3016816 "SQL Formatter reformats leading
            // commas to invalid SQL"): with
            // CommaPlacement = Leading and PreserveComments = true, a line comment trailing
            // an element must not cause the following comma to land on the comment line
            // (which would comment out the comma). The comma belongs on the next element's line.
            var sql =
                "SELECT col1, -- first column" + Environment.NewLine +
                "       col2, -- second column" + Environment.NewLine +
                "       col3" + Environment.NewLine +
                "FROM t;";
            var parser = new TSql170Parser(true);
            var fragment = parser.Parse(new StringReader(sql), out var errors);
            Assert.AreEqual(0, errors.Count);

            var generator = new Sql170ScriptGenerator(new SqlScriptGeneratorOptions
            {
                CommaPlacement = CommaPlacement.Leading,
                PreserveComments = true
            });
            generator.GenerateScript(fragment, out var generated);

            // The full generated script: each line comment stays on its element's line, and the
            // leading comma is placed at the start of the next element's line (before a column,
            // never before a comment), so the comment is never commented out and the script
            // reparses cleanly.
            string expected =
                "SELECT col1 -- first column" + Environment.NewLine +
                "     , col2 -- second column" + Environment.NewLine +
                "     , col3" + Environment.NewLine +
                "FROM   t;" + Environment.NewLine + Environment.NewLine;
            Assert.AreEqual(expected, generated);

            // The generated script must reparse cleanly.
            var reparser = new TSql170Parser(true);
            reparser.Parse(new StringReader(generated), out var reErrors);
            Assert.AreEqual(0, reErrors.Count, "Generated SQL must reparse. Actual:\n" + generated);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCommaPlacementTrailingWithPreserveCommentsKeepsCommentsAfterComma()
        {
            // Counterpart to TestCommaPlacementLeadingWithPreserveCommentsDoesNotAbsorbComma
            // (FeedbackTicket 3016816 "SQL Formatter reformats leading commas to invalid SQL"). With
            // CommaPlacement = Trailing (default) and PreserveComments = true, each trailing comma
            // stays on the element's line and its line comment follows the comma, so the generated
            // script still reparses cleanly.
            var sql =
                "SELECT col1, -- first column" + Environment.NewLine +
                "       col2, -- second column" + Environment.NewLine +
                "       col3" + Environment.NewLine +
                "FROM t;";
            var parser = new TSql170Parser(true);
            var fragment = parser.Parse(new StringReader(sql), out var errors);
            Assert.AreEqual(0, errors.Count);

            var generator = new Sql170ScriptGenerator(new SqlScriptGeneratorOptions
            {
                CommaPlacement = CommaPlacement.Trailing,
                PreserveComments = true
            });
            generator.GenerateScript(fragment, out var generated);

            // The full generated script: the trailing comma stays on the element's line and its
            // line comment follows the comma on that same line, so the script reparses cleanly.
            string expected =
                "SELECT col1, -- first column" + Environment.NewLine +
                "       col2, -- second column" + Environment.NewLine +
                "       col3" + Environment.NewLine +
                "FROM   t;" + Environment.NewLine + Environment.NewLine;
            Assert.AreEqual(expected, generated);

            // The generated script must reparse cleanly.
            var reparser = new TSql170Parser(true);
            reparser.Parse(new StringReader(generated), out var reErrors);
            Assert.AreEqual(0, reErrors.Count, "Generated SQL must reparse. Actual:\n" + generated);
        }

        // ---- Interaction rule: CommaPlacement = Leading has no visual effect when the
        // ---- corresponding Multiline* option is false (the list stays on a single line).

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCommaPlacementLeadingSelectListMultilineFalse()
        {
            // With MultilineSelectElementsList = false the SELECT list is emitted on a single
            // line, so CommaPlacement = Leading has no visual effect (commas remain inline).
            var sql = "SELECT a, b, c FROM t;";
            var parser = new TSql170Parser(true);
            var fragment = parser.Parse(new StringReader(sql), out var errors);
            Assert.AreEqual(0, errors.Count);

            var generator = new Sql170ScriptGenerator(new SqlScriptGeneratorOptions
            {
                CommaPlacement = CommaPlacement.Leading,
                MultilineSelectElementsList = false
            });
            generator.GenerateScript(fragment, out var generated);

            string expected =
                "SELECT a, b, c" + Environment.NewLine +
                "FROM   t;" + Environment.NewLine + Environment.NewLine;
            Assert.AreEqual(expected, generated);

            var reparser = new TSql170Parser(true);
            reparser.Parse(new StringReader(generated), out var reErrors);
            Assert.AreEqual(0, reErrors.Count, "Generated SQL must reparse. Actual:\n" + generated);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCommaPlacementLeadingViewColumnsMultilineFalse()
        {
            // With MultilineViewColumnsList = false the VIEW column list is emitted as a single
            // parenthesized line, so CommaPlacement = Leading has no visual effect there. (The
            // SELECT body still honors leading placement via MultilineSelectElementsList, which
            // defaults to true.)
            var sql = "CREATE VIEW v (a, b, c) AS SELECT 1, 2, 3;";
            var parser = new TSql170Parser(true);
            var fragment = parser.Parse(new StringReader(sql), out var errors);
            Assert.AreEqual(0, errors.Count);

            var generator = new Sql170ScriptGenerator(new SqlScriptGeneratorOptions
            {
                CommaPlacement = CommaPlacement.Leading,
                MultilineViewColumnsList = false,
                MultilineSelectElementsList = false
            });
            generator.GenerateScript(fragment, out var generated);

            string expected =
                "CREATE VIEW v (a, b, c)" + Environment.NewLine +
                "AS" + Environment.NewLine +
                "SELECT 1, 2, 3;" + Environment.NewLine + Environment.NewLine;
            Assert.AreEqual(expected, generated);

            var reparser = new TSql170Parser(true);
            reparser.Parse(new StringReader(generated), out var reErrors);
            Assert.AreEqual(0, reErrors.Count, "Generated SQL must reparse. Actual:\n" + generated);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCommaPlacementLeadingSetClauseItemsMultilineFalse()
        {
            // With MultilineSetClauseItems = false the UPDATE SET item list is emitted on a
            // single line, so CommaPlacement = Leading has no visual effect (commas remain
            // inline).
            var sql = "UPDATE t SET a = 1, b = 2, c = 3;";
            var parser = new TSql170Parser(true);
            var fragment = parser.Parse(new StringReader(sql), out var errors);
            Assert.AreEqual(0, errors.Count);

            var generator = new Sql170ScriptGenerator(new SqlScriptGeneratorOptions
            {
                CommaPlacement = CommaPlacement.Leading,
                MultilineSetClauseItems = false
            });
            generator.GenerateScript(fragment, out var generated);

            string expected =
                "UPDATE t" + Environment.NewLine +
                "SET    a = 1, b = 2, c = 3;" + Environment.NewLine + Environment.NewLine;
            Assert.AreEqual(expected, generated);

            var reparser = new TSql170Parser(true);
            reparser.Parse(new StringReader(generated), out var reErrors);
            Assert.AreEqual(0, reErrors.Count, "Generated SQL must reparse. Actual:\n" + generated);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCommaPlacementLeadingInsertTargetsMultilineFalse()
        {
            // The INSERT target list is always emitted on a single line, so CommaPlacement =
            // Leading has no visual effect regardless of MultilineInsertTargetsList.
            var sql = "INSERT INTO t (a, b, c) VALUES (1, 2, 3);";
            var parser = new TSql170Parser(true);
            var fragment = parser.Parse(new StringReader(sql), out var errors);
            Assert.AreEqual(0, errors.Count);

            var generator = new Sql170ScriptGenerator(new SqlScriptGeneratorOptions
            {
                CommaPlacement = CommaPlacement.Leading,
                MultilineInsertTargetsList = false
            });
            generator.GenerateScript(fragment, out var generated);

            string expected =
                "INSERT  INTO t (a, b, c)" + Environment.NewLine +
                "VALUES        (1, 2, 3);" + Environment.NewLine + Environment.NewLine;
            Assert.AreEqual(expected, generated);

            var reparser = new TSql170Parser(true);
            reparser.Parse(new StringReader(generated), out var reErrors);
            Assert.AreEqual(0, reErrors.Count, "Generated SQL must reparse. Actual:\n" + generated);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCommaPlacementLeadingInsertSourcesMultilineFalse()
        {
            // The INSERT source (VALUES) row list is always emitted multi-line (the generator
            // does not gate it on MultilineInsertSourcesList), so setting that option to false
            // does NOT collapse it to one line: CommaPlacement = Leading still applies to the
            // row separators. The target column list stays on a single line because
            // MultilineInsertTargetsList is left at its default (false).
            var sql = "INSERT INTO t (a, b, c) VALUES (1, 2, 3), (4, 5, 6), (7, 8, 9);";
            var parser = new TSql170Parser(true);
            var fragment = parser.Parse(new StringReader(sql), out var errors);
            Assert.AreEqual(0, errors.Count);

            var generator = new Sql170ScriptGenerator(new SqlScriptGeneratorOptions
            {
                CommaPlacement = CommaPlacement.Leading,
                MultilineInsertSourcesList = false
            });
            generator.GenerateScript(fragment, out var generated);

            string expected =
                "INSERT  INTO t (a, b, c)" + Environment.NewLine +
                "VALUES        (1, 2, 3)" + Environment.NewLine +
                ", (4, 5, 6)" + Environment.NewLine +
                ", (7, 8, 9);" + Environment.NewLine + Environment.NewLine;
            Assert.AreEqual(expected, generated);

            var reparser = new TSql170Parser(true);
            reparser.Parse(new StringReader(generated), out var reErrors);
            Assert.AreEqual(0, reErrors.Count, "Generated SQL must reparse. Actual:\n" + generated);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCommaPlacementLeadingParenthesizedListAlwaysMultiline()
        {
            // The CREATE TABLE column list has no single-line toggle: it is always emitted
            // multi-line. There is therefore no "Multiline = false" state for it, so
            // CommaPlacement = Leading always applies (comma at indent - 2).
            var sql = "CREATE TABLE t (a INT, b INT, c INT);";
            var parser = new TSql170Parser(true);
            var fragment = parser.Parse(new StringReader(sql), out var errors);
            Assert.AreEqual(0, errors.Count);

            var generator = new Sql170ScriptGenerator(new SqlScriptGeneratorOptions
            {
                CommaPlacement = CommaPlacement.Leading
            });
            generator.GenerateScript(fragment, out var generated);

            string expected =
                "CREATE TABLE t (" + Environment.NewLine +
                "    a INT" + Environment.NewLine +
                "  , b INT" + Environment.NewLine +
                "  , c INT" + Environment.NewLine +
                ");" + Environment.NewLine + Environment.NewLine;
            Assert.AreEqual(expected, generated);

            var reparser = new TSql170Parser(true);
            reparser.Parse(new StringReader(generated), out var reErrors);
            Assert.AreEqual(0, reErrors.Count, "Generated SQL must reparse. Actual:\n" + generated);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCommaPlacementLeadingIndentedOptionList()
        {
            // The WITH-parameter list of CREATE COLUMN MASTER KEY is generated through the indented
            // multi-line comma-list path (GenerateCommaSeparatedList with insertNewLine and indent
            // both true). With CommaPlacement = Leading the leading comma is emitted at the start
            // of the next parameter's (indented) line.
            var sql = "CREATE COLUMN MASTER KEY CMK1 WITH (KEY_STORE_PROVIDER_NAME = 'MSSQL_CERTIFICATE_STORE', KEY_PATH = 'some/path');";
            var parser = new TSql170Parser(true);
            var fragment = parser.Parse(new StringReader(sql), out var errors);
            Assert.AreEqual(0, errors.Count);

            var generator = new Sql170ScriptGenerator(new SqlScriptGeneratorOptions
            {
                CommaPlacement = CommaPlacement.Leading
            });
            generator.GenerateScript(fragment, out var generated);

            // The continuation parameter's comma is emitted at the start of its (indented) line
            // via the 4-arg GenerateCommaSeparatedList leading branch. The comma width is reserved
            // inside the indentation, so the parameter stays aligned with the first parameter and
            // the leading comma sits in the reserved columns before it.
            string expected =
                "CREATE COLUMN MASTER KEY CMK1" + Environment.NewLine +
                "WITH (" + Environment.NewLine +
                "     KEY_STORE_PROVIDER_NAME = 'MSSQL_CERTIFICATE_STORE'" + Environment.NewLine +
                "  ,  KEY_PATH = 'some/path'" + Environment.NewLine +
                ");" + Environment.NewLine + Environment.NewLine;
            Assert.AreEqual(expected, generated);

            var reparser = new TSql170Parser(true);
            reparser.Parse(new StringReader(generated), out var reErrors);
            Assert.AreEqual(0, reErrors.Count, "Generated SQL must reparse. Actual:\n" + generated);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCommaPlacementLeadingSetClauseItemsIndented()
        {
            // With IndentSetClause = true the SET keyword is indented; CommaPlacement = Leading
            // still aligns the items and places each comma two columns before them.
            var sql = "UPDATE t SET a = 1, b = 2, c = 3;";
            var parser = new TSql170Parser(true);
            var fragment = parser.Parse(new StringReader(sql), out var errors);
            Assert.AreEqual(0, errors.Count);

            var generator = new Sql170ScriptGenerator(new SqlScriptGeneratorOptions
            {
                CommaPlacement = CommaPlacement.Leading,
                MultilineSetClauseItems = true,
                IndentSetClause = true
            });
            generator.GenerateScript(fragment, out var generated);

            string expected =
                "UPDATE  t" + Environment.NewLine +
                "    SET a = 1" + Environment.NewLine +
                "      , b = 2" + Environment.NewLine +
                "      , c = 3;" + Environment.NewLine + Environment.NewLine;
            Assert.AreEqual(expected, generated);

            var reparser = new TSql170Parser(true);
            reparser.Parse(new StringReader(generated), out var reErrors);
            Assert.AreEqual(0, reErrors.Count, "Generated SQL must reparse. Actual:\n" + generated);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCommaPlacementLeadingCreateTableWithComments()
        {
            // PreserveComments + CommaPlacement.Leading in an indented (CREATE TABLE) column list:
            // each trailing line comment stays on its column's line and the leading comma is
            // emitted at the start of the next column's line (comma at indent - 2). Verifies the
            // comment/comma-skip fix in the indent-based leading path, not just the SELECT list.
            var sql =
                "CREATE TABLE t (a INT, -- first" + Environment.NewLine +
                "b INT, -- second" + Environment.NewLine +
                "c INT);";
            var parser = new TSql170Parser(true);
            var fragment = parser.Parse(new StringReader(sql), out var errors);
            Assert.AreEqual(0, errors.Count);

            var generator = new Sql170ScriptGenerator(new SqlScriptGeneratorOptions
            {
                CommaPlacement = CommaPlacement.Leading,
                PreserveComments = true
            });
            generator.GenerateScript(fragment, out var generated);

            string expected =
                "CREATE TABLE t (" + Environment.NewLine +
                "    a INT -- first" + Environment.NewLine +
                "  , b INT -- second" + Environment.NewLine +
                "  , c INT" + Environment.NewLine +
                ");" + Environment.NewLine + Environment.NewLine;
            Assert.AreEqual(expected, generated);

            var reparser = new TSql170Parser(true);
            reparser.Parse(new StringReader(generated), out var reErrors);
            Assert.AreEqual(0, reErrors.Count, "Generated SQL must reparse. Actual:\n" + generated);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCommaPlacementLeadingWithBlockCommentTrailingElement()
        {
            // PreserveComments + CommaPlacement.Leading with a block comment trailing an element.
            // Block comments are emitted inline (a different path than the deferred '--' comments),
            // so this confirms leading placement is not broken by an inline block comment.
            var sql = "SELECT a /* note */, b, c FROM t;";
            var parser = new TSql170Parser(true);
            var fragment = parser.Parse(new StringReader(sql), out var errors);
            Assert.AreEqual(0, errors.Count);

            var generator = new Sql170ScriptGenerator(new SqlScriptGeneratorOptions
            {
                CommaPlacement = CommaPlacement.Leading,
                PreserveComments = true
            });
            generator.GenerateScript(fragment, out var generated);

            string expected =
                "SELECT a /* note */" + Environment.NewLine +
                "     , b" + Environment.NewLine +
                "     , c" + Environment.NewLine +
                "FROM   t;" + Environment.NewLine + Environment.NewLine;
            Assert.AreEqual(expected, generated);

            var reparser = new TSql170Parser(true);
            reparser.Parse(new StringReader(generated), out var reErrors);
            Assert.AreEqual(0, reErrors.Count, "Generated SQL must reparse. Actual:\n" + generated);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCommaPlacementLeadingSpaceCountZero()
        {
            // LeadingCommaSpaceCount = 0: the comma occupies a single column (no trailing space).
            // Items stay aligned with the first element; only the comma column shifts.
            const string selectSql = "SELECT a, b, c FROM t;";
            const string tableSql = "CREATE TABLE t (a INT, b INT, c INT);";

            // Keyword-aligned list (SELECT): comma ends immediately before the aligned column.
            string expectedSelect =
                "SELECT a" + Environment.NewLine +
                "      ,b" + Environment.NewLine +
                "      ,c" + Environment.NewLine +
                "FROM   t;" + Environment.NewLine + Environment.NewLine;

            // Indented list (CREATE TABLE): elements stay at indentation column 4; the comma is
            // indented one column fewer (column 3) so the comma+item still occupy 4 columns.
            string expectedTable =
                "CREATE TABLE t (" + Environment.NewLine +
                "    a INT" + Environment.NewLine +
                "   ,b INT" + Environment.NewLine +
                "   ,c INT" + Environment.NewLine +
                ");" + Environment.NewLine + Environment.NewLine;

            AssertLeadingCommaSpaceCount(0, selectSql, expectedSelect);
            AssertLeadingCommaSpaceCount(0, tableSql, expectedTable);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCommaPlacementLeadingSpaceCountOne()
        {
            // LeadingCommaSpaceCount = 1 (the default): comma + one space, occupying 2 columns.
            const string selectSql = "SELECT a, b, c FROM t;";
            const string tableSql = "CREATE TABLE t (a INT, b INT, c INT);";

            string expectedSelect =
                "SELECT a" + Environment.NewLine +
                "     , b" + Environment.NewLine +
                "     , c" + Environment.NewLine +
                "FROM   t;" + Environment.NewLine + Environment.NewLine;

            string expectedTable =
                "CREATE TABLE t (" + Environment.NewLine +
                "    a INT" + Environment.NewLine +
                "  , b INT" + Environment.NewLine +
                "  , c INT" + Environment.NewLine +
                ");" + Environment.NewLine + Environment.NewLine;

            AssertLeadingCommaSpaceCount(1, selectSql, expectedSelect);
            AssertLeadingCommaSpaceCount(1, tableSql, expectedTable);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCommaPlacementLeadingSpaceCountTwo()
        {
            // LeadingCommaSpaceCount = 2: comma + two spaces, occupying 3 columns.
            const string selectSql = "SELECT a, b, c FROM t;";
            const string tableSql = "CREATE TABLE t (a INT, b INT, c INT);";

            string expectedSelect =
                "SELECT a" + Environment.NewLine +
                "    ,  b" + Environment.NewLine +
                "    ,  c" + Environment.NewLine +
                "FROM   t;" + Environment.NewLine + Environment.NewLine;

            // Elements stay at indentation column 4; the comma is indented three columns fewer
            // (column 1) so the comma plus its two trailing spaces still occupy 4 columns.
            string expectedTable =
                "CREATE TABLE t (" + Environment.NewLine +
                "    a INT" + Environment.NewLine +
                " ,  b INT" + Environment.NewLine +
                " ,  c INT" + Environment.NewLine +
                ");" + Environment.NewLine + Environment.NewLine;

            AssertLeadingCommaSpaceCount(2, selectSql, expectedSelect);
            AssertLeadingCommaSpaceCount(2, tableSql, expectedTable);
        }

        // Generates the given SQL with CommaPlacement.Leading and the specified leading-comma space
        // count, asserts the generated script matches the expectation, and that it reparses cleanly.
        private static void AssertLeadingCommaSpaceCount(int spaceCount, string sql, string expected)
        {
            var parser = new TSql170Parser(true);
            var fragment = parser.Parse(new StringReader(sql), out var errors);
            Assert.AreEqual(0, errors.Count);

            var generator = new Sql170ScriptGenerator(new SqlScriptGeneratorOptions
            {
                CommaPlacement = CommaPlacement.Leading,
                LeadingCommaSpaceCount = spaceCount
            });
            generator.GenerateScript(fragment, out var generated);

            Assert.AreEqual(expected, generated,
                "LeadingCommaSpaceCount=" + spaceCount + " produced unexpected output. Actual:\n" + generated);

            var reparser = new TSql170Parser(true);
            reparser.Parse(new StringReader(generated), out var reErrors);
            Assert.AreEqual(0, reErrors.Count, "Generated SQL must reparse. Actual:\n" + generated);
        }

        #endregion
    }
}
