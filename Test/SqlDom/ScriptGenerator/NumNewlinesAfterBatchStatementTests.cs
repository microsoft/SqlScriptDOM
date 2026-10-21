//------------------------------------------------------------------------------
// <copyright file="NumNewlinesAfterBatchStatementTests.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using Microsoft.SqlServer.TransactSql.ScriptDom;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlStudio.Tests.AssemblyTools.TestCategory;
using static SqlStudio.Tests.UTSqlScriptDom.ScriptGeneratorTestHelper;

namespace SqlStudio.Tests.UTSqlScriptDom
{
    // Tests for the NumNewlinesAfterBatchStatement script-generation option, which controls how many
    // newlines follow each statement owned directly by a TSqlBatch. Statements nested inside another
    // statement are governed by NumNewlinesAfterStatement instead.
    [TestClass]
    public class NumNewlinesAfterBatchStatementTests
    {
        private const string TwoSelectsInBatch = @"
SELECT * FROM sys.databases;
SELECT * FROM sys.databases;";

        private const string TwoSelectsInBeginEnd = @"
BEGIN
SELECT * FROM sys.databases;
SELECT * FROM sys.databases;
END";

        private const string OneSelectInBatch = @"SELECT * FROM sys.databases;";

        private static SqlScriptGeneratorOptions MakeOptions(int numNewlinesAfterBatchStatement)
        {
            return new SqlScriptGeneratorOptions { NumNewlinesAfterBatchStatement = numNewlinesAfterBatchStatement };
        }

        // -----------------------------------------------------------------------------------------
        // Default
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestNumNewlinesAfterBatchStatementDefaultIsTwo()
        {
            Assert.AreEqual(2, new SqlScriptGeneratorOptions().NumNewlinesAfterBatchStatement);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestNegativeNumNewlinesAfterBatchStatementClampsToZero()
        {
            Assert.AreEqual(0, MakeOptions(-1).NumNewlinesAfterBatchStatement);
        }

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

        // -----------------------------------------------------------------------------------------
        // Batch-level statements (option applies)
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestBatchStatementsWithOneNewline()
        {
            AssertGenerated(TwoSelectsInBatch, MakeOptions(1), @"
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
            AssertGenerated(TwoSelectsInBatch, MakeOptions(4), @"
SELECT *
FROM   sys.databases;



SELECT *
FROM   sys.databases;");
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestBatchStatementsWithZeroNewlines()
        {
            // With no separating newline the second statement starts on the first one's last line,
            // so its remaining clauses are aligned under that column.
            AssertGenerated(TwoSelectsInBatch, MakeOptions(0), @"
SELECT *
FROM   sys.databases;SELECT *
                     FROM   sys.databases;");
        }

        // Exact (non-trimmed) assertions that pin the trailing newlines emitted after the final
        // batch's last statement, which AssertGenerated's Trim() would otherwise discard.
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestTrailingNewlinesAfterLastStatementDefault()
        {
            AssertGeneratedExact(OneSelectInBatch, new SqlScriptGeneratorOptions(),
                "SELECT *\nFROM   sys.databases;\n\n");
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestTrailingNewlinesAfterLastStatementZero()
        {
            AssertGeneratedExact(OneSelectInBatch, MakeOptions(0),
                "SELECT *\nFROM   sys.databases;");
        }

        // -----------------------------------------------------------------------------------------
        // Nested statement lists (option does not apply)
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestNestedStatementListWithFourNewlines()
        {
            // Statements nested in a BEGIN...END block follow NumNewlinesAfterStatement instead, so
            // this matches the default.
            AssertGenerated(TwoSelectsInBeginEnd, MakeOptions(4), @"
BEGIN
    SELECT *
    FROM   sys.databases;
    SELECT *
    FROM   sys.databases;
END");
        }
    }
}
