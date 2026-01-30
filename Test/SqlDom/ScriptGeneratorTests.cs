//------------------------------------------------------------------------------
// <copyright file="ScriptGeneratorTests.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
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
            
            // Verify position: comments should appear before their respective WHEN/ELSE clauses
            int highPriorityCommentIdx = generatedSql.IndexOf("-- Check for high priority");
            int firstWhenIdx = generatedSql.IndexOf("WHEN", StringComparison.OrdinalIgnoreCase);
            int mediumCommentIdx = generatedSql.IndexOf("/* Medium priority items */");
            int secondWhenIdx = generatedSql.IndexOf("WHEN", firstWhenIdx + 1, StringComparison.OrdinalIgnoreCase);
            int elseCommentIdx = generatedSql.IndexOf("-- Default to low priority");
            int elseIdx = generatedSql.IndexOf("ELSE", StringComparison.OrdinalIgnoreCase);
            
            Assert.IsTrue(highPriorityCommentIdx < firstWhenIdx, 
                $"High priority comment should appear before first WHEN. Comment at {highPriorityCommentIdx}, WHEN at {firstWhenIdx}");
            Assert.IsTrue(mediumCommentIdx < secondWhenIdx, 
                $"Medium priority comment should appear before second WHEN. Comment at {mediumCommentIdx}, WHEN at {secondWhenIdx}");
            Assert.IsTrue(elseCommentIdx < elseIdx, 
                $"ELSE comment should appear before ELSE. Comment at {elseCommentIdx}, ELSE at {elseIdx}");
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
            
            // Verify position: comments should appear before their respective JOIN clauses
            int innerJoinCommentIdx = generatedSql.IndexOf("-- Join to get user orders");
            int innerJoinIdx = generatedSql.IndexOf("INNER JOIN", StringComparison.OrdinalIgnoreCase);
            int leftJoinCommentIdx = generatedSql.IndexOf("/* Left join for optional address */");
            int leftJoinIdx = generatedSql.IndexOf("LEFT", StringComparison.OrdinalIgnoreCase);
            int crossJoinCommentIdx = generatedSql.IndexOf("-- Cross join for all combinations");
            int crossJoinIdx = generatedSql.IndexOf("CROSS JOIN", StringComparison.OrdinalIgnoreCase);
            
            Assert.IsTrue(innerJoinCommentIdx < innerJoinIdx, 
                $"INNER JOIN comment should appear before INNER JOIN. Comment at {innerJoinCommentIdx}, JOIN at {innerJoinIdx}");
            Assert.IsTrue(leftJoinCommentIdx < leftJoinIdx, 
                $"LEFT JOIN comment should appear before LEFT JOIN. Comment at {leftJoinCommentIdx}, JOIN at {leftJoinIdx}");
            Assert.IsTrue(crossJoinCommentIdx < crossJoinIdx, 
                $"CROSS JOIN comment should appear before CROSS JOIN. Comment at {crossJoinCommentIdx}, JOIN at {crossJoinIdx}");
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
            
            // Verify position: comments should appear at correct positions relative to queries
            int firstQueryCommentIdx = generatedSql.IndexOf("-- First query: active users");
            int firstSelectIdx = generatedSql.IndexOf("SELECT", StringComparison.OrdinalIgnoreCase);
            int unionCommentIdx = generatedSql.IndexOf("/* Combine with archived users */");
            int unionIdx = generatedSql.IndexOf("UNION", StringComparison.OrdinalIgnoreCase);
            
            Assert.IsTrue(firstQueryCommentIdx < firstSelectIdx, 
                $"First query comment should appear before first SELECT. Comment at {firstQueryCommentIdx}, SELECT at {firstSelectIdx}");
            Assert.IsTrue(unionCommentIdx < unionIdx, 
                $"UNION comment should appear before UNION. Comment at {unionCommentIdx}, UNION at {unionIdx}");
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

        #endregion
    }
}
