//------------------------------------------------------------------------------
// <copyright file="NumNewlinesAfterBatchesTests.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using Microsoft.SqlServer.TransactSql.ScriptDom;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlStudio.Tests.AssemblyTools.TestCategory;
using static SqlStudio.Tests.UTSqlScriptDom.ScriptGeneratorTestHelper;

namespace SqlStudio.Tests.UTSqlScriptDom
{
    // Tests for the NumNewlinesAfterBatches script-generation option, which controls how many
    // newlines follow the GO separator between two batches. GO is always preceded by a newline of
    // its own, and the blank lines before it come from NumNewlinesAfterBatchStatement.
    [TestClass]
    public class NumNewlinesAfterBatchesTests
    {
        private const string TwoBatches = @"
SELECT * FROM sys.databases;
GO
SELECT * FROM sys.databases;";

        private const string BatchWithNestedBlockThenBatch = @"
SELECT * FROM sys.databases;
BEGIN
SELECT * FROM sys.databases;
SELECT * FROM sys.databases;
END
GO
SELECT * FROM sys.databases;";

        private static SqlScriptGeneratorOptions MakeOptions(int numNewlinesAfterBatches)
        {
            return new SqlScriptGeneratorOptions { NumNewlinesAfterBatches = numNewlinesAfterBatches };
        }

        // -----------------------------------------------------------------------------------------
        // Default
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestNumNewlinesAfterBatchesDefaultIsOne()
        {
            Assert.AreEqual(1, new SqlScriptGeneratorOptions().NumNewlinesAfterBatches);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestTwoBatchesWithDefaultOptions()
        {
            // The two blank lines before GO come from NumNewlinesAfterBatchStatement, which is
            // applied after the last statement of the batch as well.
            AssertGenerated(TwoBatches, new SqlScriptGeneratorOptions(), @"
SELECT *
FROM   sys.databases;


GO
SELECT *
FROM   sys.databases;");
        }

        // -----------------------------------------------------------------------------------------
        // Newlines after GO
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestTwoBatchesWithThreeNewlines()
        {
            AssertGenerated(TwoBatches, MakeOptions(3), @"
SELECT *
FROM   sys.databases;


GO


SELECT *
FROM   sys.databases;");
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestTwoBatchesWithZeroNewlinesClampsToOne()
        {
            // The next batch must start on its own line, so the setting is clamped to its minimum of 1.
            Assert.AreEqual(1, MakeOptions(0).NumNewlinesAfterBatches);
        }

        // -----------------------------------------------------------------------------------------
        // Interaction with NumNewlinesAfterBatchStatement
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestGoWithNoBlankLineOnEitherSide()
        {
            SqlScriptGeneratorOptions options = new SqlScriptGeneratorOptions
            {
                NumNewlinesAfterBatchStatement = 0,
                NumNewlinesAfterBatches = 1
            };

            AssertGenerated(TwoBatches, options, @"
SELECT *
FROM   sys.databases;
GO
SELECT *
FROM   sys.databases;");
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestOneBlankLineOnEachSideOfGo()
        {
            SqlScriptGeneratorOptions options = new SqlScriptGeneratorOptions
            {
                NumNewlinesAfterBatchStatement = 1,
                NumNewlinesAfterBatches = 2
            };

            AssertGenerated(TwoBatches, options, @"
SELECT *
FROM   sys.databases;

GO

SELECT *
FROM   sys.databases;");
        }

        // -----------------------------------------------------------------------------------------
        // All three newline options together
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestAllThreeNewlineOptionsTogether()
        {
            SqlScriptGeneratorOptions options = new SqlScriptGeneratorOptions
            {
                NumNewlinesAfterStatement = 2,
                NumNewlinesAfterBatchStatement = 2,
                NumNewlinesAfterBatches = 2
            };

            // One blank line between statements, nested and top level alike. The batch closes with
            // two blank lines because the newline that carries GO stacks on top of the two written
            // after END.
            AssertGenerated(BatchWithNestedBlockThenBatch, options, @"
SELECT *
FROM   sys.databases;

BEGIN
    SELECT *
    FROM   sys.databases;
    
    SELECT *
    FROM   sys.databases;
END


GO

SELECT *
FROM   sys.databases;");
        }
    }
}
