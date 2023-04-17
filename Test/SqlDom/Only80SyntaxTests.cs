//------------------------------------------------------------------------------
// <copyright file="Only80SyntaxTests.cs" company="Microsoft">
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
        private static readonly ParserTest[] Only80TestInfos = {
                // Scripts, which cause errors in later versions
            new ParserTest80("ParserModeTests.sql", 1),
            new ParserTest80("MiscTests80.sql",  
                new ParserErrorInfo(63, "SQL46010", "LOAD"),
				new ParserErrorInfo(88, "SQL46010", "sysobjects"),
				new ParserErrorInfo(131, "SQL46010", "INDEX"),
				new ParserErrorInfo(200, "SQL46018", "ROWS"),
				new ParserErrorInfo(236, "SQL46020", "ROWS"),
                new ParserErrorInfo(257, "SQL46010", "NAME"),
                new ParserErrorInfo(357, "SQL46010", "size"),
                new ParserErrorInfo(436, "SQL46010", "NAME"),
                new ParserErrorInfo(571, "SQL46010", "(")),
             // Different pretty-printing
            new ParserTest("SecurityStatement80Tests.sql", "Baselines80", "Baselines90", "Baselines90"),
            new ParserTest("AlterTableAddTableElementStatementTests.sql", "Baselines80", "Baselines90", "Baselines90"),
        };

        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]
        public void TSql80SyntaxIn120ParserTest()
        {
            TSql120Parser parser = new TSql120Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql120);

            foreach (ParserTest ti in Only80TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result120);
            }
        }

        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]
        public void TSql80SyntaxIn110ParserTest()
        {
            TSql110Parser parser = new TSql110Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql110);

            foreach (ParserTest ti in Only80TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result110);
            }
        }
 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]
        public void TSql80SyntaxIn100ParserTest()
        {
            TSql100Parser parser = new TSql100Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql100);

            foreach (ParserTest ti in Only80TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result100);
            }
        }

        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]
        public void TSql80SyntaxIn90ParserTest()
        {
            TSql90Parser parser = new TSql90Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql90);

            foreach (ParserTest ti in Only80TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result90);
            }
        }

        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]
        public void TSql80SyntaxIn80ParserTest()
        {
            TSql80Parser parser = new TSql80Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql80);

            foreach (ParserTest ti in Only80TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result80);
            }
        }
    }
}
