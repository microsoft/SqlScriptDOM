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

        #endregion
    }
}
