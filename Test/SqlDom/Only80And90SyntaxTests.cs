//------------------------------------------------------------------------------
// <copyright file="Only80And90SyntaxTests.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using Microsoft.SqlServer.TransactSql.ScriptDom;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlStudio.Tests.AssemblyTools.TestCategory;

namespace SqlStudio.Tests.UTSqlScriptDom
{
    public partial class SqlDomTests
    {
        // Note: These filenames are case sensitive, make sure they match the checked-in file exactly
        private static readonly ParserTest[] Only80And90TestInfos = {
            new ParserTest80And90("DumpLoadStatementTests.sql", 
                new ParserErrorInfo(34, "SQL46010", "DUMP"),
                new ParserErrorInfo(193, "SQL46010", "TRANSACTION"),
                new ParserErrorInfo(234, "SQL46010", "TRAN"),
                new ParserErrorInfo(298, "SQL46010", "LOAD"),
                new ParserErrorInfo(382, "SQL46010", "TRANSACTION"),
                new ParserErrorInfo(429, "SQL46010", "TRAN")),
            new ParserTest90("DumpLoadStatement90Tests.sql",
                new ParserTestOutput(4), 
                    new ParserErrorInfo(17, "SQL46010", "c1"),
                    new ParserErrorInfo(55, "SQL46010", "master"),
                    new ParserErrorInfo(132, "SQL46010", "master"),
                    new ParserErrorInfo(203, "SQL46010", "MASTER")),
            new ParserTest80And90("MiscDeprecatedIn100Tests.sql",
                new ParserErrorInfo(36, "SQL46010", "CONCURRENCYVIOLATION"),
                new ParserErrorInfo(69, "SQL46010", "MEMOBJLIST"),
                new ParserErrorInfo(92, "SQL46010", "MEMORYMAP"))
        };


        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]
        public void TSql80And90SyntaxIn120ParserTest()
        {
            TSql120Parser parser = new TSql120Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql120);
            foreach (ParserTest ti in Only80And90TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result120);
            }
        }

        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]
        public void TSql80And90SyntaxIn110ParserTest()
        {
            TSql110Parser parser = new TSql110Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql110);
            foreach (ParserTest ti in Only80And90TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result100);
            }
        }

        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]
        public void TSql80And90SyntaxIn100ParserTest()
        {
            TSql100Parser parser = new TSql100Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql100);
            foreach (ParserTest ti in Only80And90TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result100);
            }
        }
 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]
        public void TSql80And90SyntaxIn90ParserTest()
        {
            TSql90Parser parser = new TSql90Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql90);
            foreach (ParserTest ti in Only80And90TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result90);
            }
        }

        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]
        public void TSql80And90SyntaxIn80ParserTest()
        {
            TSql80Parser parser = new TSql80Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql80);
            foreach (ParserTest ti in Only80And90TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result80);
            }
        }
    }
}
