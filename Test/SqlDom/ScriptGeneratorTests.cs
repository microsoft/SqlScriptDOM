//------------------------------------------------------------------------------
// <copyright file="ScriptGeneratorTests.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

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
    }
}
