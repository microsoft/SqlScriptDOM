//------------------------------------------------------------------------------
// <copyright file="NumNewlinesAfterStatementTests.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using Microsoft.SqlServer.TransactSql.ScriptDom;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlStudio.Tests.AssemblyTools.TestCategory;
using static SqlStudio.Tests.UTSqlScriptDom.ScriptGeneratorTestHelper;

namespace SqlStudio.Tests.UTSqlScriptDom
{
    // Tests for the NumNewlinesAfterStatement script-generation option, which controls how many
    // newlines separate consecutive statements of a StatementList. It does not apply to statements
    // held directly by a TSqlBatch, which are governed by NumNewlinesAfterBatchStatement.
    [TestClass]
    public class NumNewlinesAfterStatementTests
    {
        private const string TwoSelectsInBatch = @"
SELECT * FROM sys.databases;
SELECT * FROM sys.databases;";

        private const string TwoSelectsInBeginEnd = @"
BEGIN
SELECT * FROM sys.databases;
SELECT * FROM sys.databases;
END";

        private static SqlScriptGeneratorOptions MakeOptions(int numNewlinesAfterStatement)
        {
            return new SqlScriptGeneratorOptions { NumNewlinesAfterStatement = numNewlinesAfterStatement };
        }

        // -----------------------------------------------------------------------------------------
        // Default
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestNumNewlinesAfterStatementDefaultIsOne()
        {
            Assert.AreEqual(1, new SqlScriptGeneratorOptions().NumNewlinesAfterStatement);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestNegativeNumNewlinesAfterStatementClampsToZero()
        {
            Assert.AreEqual(0, MakeOptions(-1).NumNewlinesAfterStatement);
        }

        // -----------------------------------------------------------------------------------------
        // Batch-level statements (option does not apply)
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestBatchStatementsWithDefaultOptions()
        {
            AssertGenerated(TwoSelectsInBatch, new SqlScriptGeneratorOptions(), @"
SELECT *
FROM   sys.databases;

SELECT *
FROM   sys.databases;");
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestBatchStatementsWithFourNewlines()
        {
            // Statements owned by a TSqlBatch follow NumNewlinesAfterBatchStatement instead, so
            // this matches the default.
            AssertGenerated(TwoSelectsInBatch, MakeOptions(4), @"
SELECT *
FROM   sys.databases;

SELECT *
FROM   sys.databases;");
        }

        // -----------------------------------------------------------------------------------------
        // Nested statement lists (option applies)
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestNestedStatementListWithDefaultOptions()
        {
            AssertGenerated(TwoSelectsInBeginEnd, new SqlScriptGeneratorOptions(), @"
BEGIN
    SELECT *
    FROM   sys.databases;
    SELECT *
    FROM   sys.databases;
END");
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestNestedStatementListWithTwoNewlines()
        {
            // Every newline is followed by the current indentation, so the separating blank line
            // below is not empty: it carries the four-space indent.
            AssertGenerated(TwoSelectsInBeginEnd, MakeOptions(2), @"
BEGIN
    SELECT *
    FROM   sys.databases;
    
    SELECT *
    FROM   sys.databases;
END");
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestNestedStatementListWithFourNewlines()
        {
            // Every newline is followed by the current indentation, so the separating blank lines
            // below are not empty: each one carries the four-space indent.
            AssertGenerated(TwoSelectsInBeginEnd, MakeOptions(4), @"
BEGIN
    SELECT *
    FROM   sys.databases;
    
    
    
    SELECT *
    FROM   sys.databases;
END");
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestNestedStatementListWithZeroNewlines()
        {
            // With no separating newline the second statement starts on the first one's last line,
            // so its remaining clauses are aligned under that column.
            AssertGenerated(TwoSelectsInBeginEnd, MakeOptions(0), @"
BEGIN
    SELECT *
    FROM   sys.databases;SELECT *
                         FROM   sys.databases;
END");
        }
    }
}
