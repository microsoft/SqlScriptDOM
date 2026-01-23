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

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIdentifierCasingDefault()
        {
            Assert.AreEqual(IdentifierCasing.PreserveOriginal, new SqlScriptGeneratorOptions().IdentifierCasing);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIdentifierCasingPreserveOriginal()
        {
            var expectedSqlText = @"CREATE TABLE MyTableName (
    MyColumnName VARCHAR (50),
    anotherColumn INT,
    MixedCaseColumn DECIMAL (10, 2)
);";

            ParseAndAssertEquality(expectedSqlText, new SqlScriptGeneratorOptions {
                IdentifierCasing = IdentifierCasing.PreserveOriginal,
                AlignColumnDefinitionFields = false
            });
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIdentifierCasingLowercase()
        {
            var inputSqlText = @"CREATE TABLE MyTableName (
    MyColumnName VARCHAR (50),
    AnotherColumn INT,
    MixedCaseColumn DECIMAL (10, 2)
);";

            var expectedSqlText = @"CREATE TABLE mytablename (
    mycolumnname VARCHAR (50),
    anothercolumn INT,
    mixedcasecolumn DECIMAL (10, 2)
);";

            ParseTransformAndAssertEquality(inputSqlText, expectedSqlText, new SqlScriptGeneratorOptions {
                IdentifierCasing = IdentifierCasing.Lowercase,
                AlignColumnDefinitionFields = false
            });
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIdentifierCasingUppercase()
        {
            var inputSqlText = @"CREATE TABLE MyTableName (
    MyColumnName VARCHAR (50),
    anotherColumn INT,
    MixedCaseColumn DECIMAL (10, 2)
);";

            var expectedSqlText = @"CREATE TABLE MYTABLENAME (
    MYCOLUMNNAME VARCHAR (50),
    ANOTHERCOLUMN INT,
    MIXEDCASECOLUMN DECIMAL (10, 2)
);";

            ParseTransformAndAssertEquality(inputSqlText, expectedSqlText, new SqlScriptGeneratorOptions {
                IdentifierCasing = IdentifierCasing.Uppercase,
                AlignColumnDefinitionFields = false
            });
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIdentifierCasingPascalCase()
        {
            var inputSqlText = @"CREATE TABLE MyTableName (
    MyColumnName VARCHAR (50),
    anotherColumn INT,
    MIXEDCASECOLUMN DECIMAL (10, 2)
);";

            var expectedSqlText = @"CREATE TABLE Mytablename (
    Mycolumnname VARCHAR (50),
    Anothercolumn INT,
    Mixedcasecolumn DECIMAL (10, 2)
);";

            ParseTransformAndAssertEquality(inputSqlText, expectedSqlText, new SqlScriptGeneratorOptions {
                IdentifierCasing = IdentifierCasing.PascalCase,
                AlignColumnDefinitionFields = false
            });
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIdentifierCasingWithAccentedCharactersLowercase()
        {
            var inputSqlText = @"CREATE TABLE [Příliš Žluťoučký Kůň] (
    [Úpěl Ďábelské] VARCHAR (50),
    [Ódy] INT
);";

            var expectedSqlText = @"CREATE TABLE [příliš žluťoučký kůň] (
    [úpěl ďábelské] VARCHAR (50),
    [ódy] INT
);";

            ParseTransformAndAssertEquality(inputSqlText, expectedSqlText, new SqlScriptGeneratorOptions {
                IdentifierCasing = IdentifierCasing.Lowercase,
                AlignColumnDefinitionFields = false
            });
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIdentifierCasingWithAccentedCharactersUppercase()
        {
            var inputSqlText = @"CREATE TABLE [Příliš Žluťoučký Kůň] (
    [Úpěl Ďábelské] VARCHAR (50),
    [Ódy] INT
);";

            var expectedSqlText = @"CREATE TABLE [PŘÍLIŠ ŽLUŤOUČKÝ KŮŇ] (
    [ÚPĚL ĎÁBELSKÉ] VARCHAR (50),
    [ÓDY] INT
);";

            ParseTransformAndAssertEquality(inputSqlText, expectedSqlText, new SqlScriptGeneratorOptions {
                IdentifierCasing = IdentifierCasing.Uppercase,
                AlignColumnDefinitionFields = false
            });
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIdentifierCasingWithAccentedCharactersPascalCase()
        {
            var inputSqlText = @"CREATE TABLE [PŘÍLIŠ ŽLUŤOUČKÝ KŮŇ] (
    [ÚPĚL ĎÁBELSKÉ ÓDY] VARCHAR (50),
    [ódy] INT
);";

            // Note: PascalCase with quoted identifiers containing spaces produces lowercase after first character
            // because the GetPascalCase method only capitalizes the first character of the entire string
            var expectedSqlText = @"CREATE TABLE [příliš žluťoučký kůň] (
    [úpěl ďábelské ódy] VARCHAR (50),
    [ódy] INT
);";

            ParseTransformAndAssertEquality(inputSqlText, expectedSqlText, new SqlScriptGeneratorOptions {
                IdentifierCasing = IdentifierCasing.PascalCase,
                AlignColumnDefinitionFields = false
            });
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIdentifierCasingDoesNotAffectKeywords()
        {
            var inputSqlText = @"Create Table MyTable (
    MyColumn VarChar (50) not null,
    MyId Int Primary Key
);";

            var expectedSqlTextLowercaseKeywords = @"create table MYTABLE (
    MYCOLUMN varchar (50) not null,
    MYID int primary key
);";

            var expectedSqlTextUppercaseKeywords = @"CREATE TABLE mytable (
    mycolumn VARCHAR (50) NOT NULL,
    myid INT PRIMARY KEY
);";

            var parser = new TSql160Parser(true);
            var fragment = parser.ParseStatementList(new StringReader(inputSqlText), out var errors);

            Assert.AreEqual(0, errors.Count);

            // The setting trigger both keyword and identifier casing, set opposite to each other,
            // and hence it's clear which part is affected by which setting.

            // Test with lowercase keywords and uppercase identifiers
            var generatorLowerKeywords = new Sql160ScriptGenerator(new SqlScriptGeneratorOptions {
                KeywordCasing = KeywordCasing.Lowercase,
                IdentifierCasing = IdentifierCasing.Uppercase,
                AlignColumnDefinitionFields = false
            });
            generatorLowerKeywords.GenerateScript(fragment, out var generatedSqlTextLower);
            Assert.AreEqual(expectedSqlTextLowercaseKeywords, generatedSqlTextLower);

            // Test with uppercase keywords and lowercase identifiers
            var generatorUpperKeywords = new Sql160ScriptGenerator(new SqlScriptGeneratorOptions {
                KeywordCasing = KeywordCasing.Uppercase,
                IdentifierCasing = IdentifierCasing.Lowercase,
                AlignColumnDefinitionFields = false
            });
            generatorUpperKeywords.GenerateScript(fragment, out var generatedSqlTextUpper);
            Assert.AreEqual(expectedSqlTextUppercaseKeywords, generatedSqlTextUpper);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIdentifierCasingWithComplexStatement()
        {
            var inputSqlText = @"SELECT T1.MyColumn,
       T2.AnotherColumn
FROM MySchema.MyTable AS T1
     INNER JOIN AnotherSchema.AnotherTable AS T2 ON T1.Id = T2.ForeignId
WHERE T1.StatusCode = 'ACTIVE';";

            var expectedSqlText = @"SELECT t1.mycolumn,
       t2.anothercolumn
FROM   myschema.mytable AS t1
       INNER JOIN
       anotherschema.anothertable AS t2
       ON t1.id = t2.foreignid
WHERE  t1.statuscode = 'ACTIVE';";

            ParseTransformAndAssertEquality(inputSqlText, expectedSqlText, new SqlScriptGeneratorOptions {
                IdentifierCasing = IdentifierCasing.Lowercase,
                AlignClauseBodies = true,
                NewLineBeforeFromClause = true,
                NewLineBeforeWhereClause = true,
                NewLineBeforeJoinClause = true
            });
        }

        void ParseTransformAndAssertEquality(string inputSqlText, string expectedSqlText, SqlScriptGeneratorOptions generatorOptions)
        {
            var parser = new TSql160Parser(true);
            var fragment = parser.ParseStatementList(new StringReader(inputSqlText), out var errors);

            Assert.AreEqual(0, errors.Count);

            var generator = new Sql160ScriptGenerator(generatorOptions);
            generator.GenerateScript(fragment, out var generatedSqlText);

            Assert.AreEqual(expectedSqlText, generatedSqlText);
        }
    }
}
