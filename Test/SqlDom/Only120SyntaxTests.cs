//------------------------------------------------------------------------------
// <copyright file="Only120SyntaxTests.cs" company="Microsoft">
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
        private static readonly ParserTest[] Only120TestInfos = {
			new ParserTest120("AlterCreateDatabaseStatementTests120.sql", 2, 1, 2, 2),
			new ParserTest120("ColumnDefinitionTests120.sql", 8, 6, 6, 6),
			new ParserTest120("CreateProcedureStatementTests120.sql", 6, 5, 5, 5),	
			new ParserTest120("CreateTableTests120.sql", 11, 9, 9, 9),
			new ParserTest120("DeclareVariableElementTest.sql", 1, 1, 1, 1),
			new ParserTest120("FromClauseTests120.sql", 2, 2, 2, 2),			
			new ParserTest120("TableTypeTests120.sql", 6, 3, 3, 3),
			new ParserTest120("UniqueConstraintTests120.sql", 6, 4, 3, 3),			
			new ParserTest120("AlterIndexStatementTests120.sql", 2, 15, 15, 15),
			new ParserTest120("AlterTableStatementTests120.sql", 16, 16, 16, 16),
			new ParserTest120("AlterTableSwitchStatementTests120.sql", 6, 6, 6, 6),
			new ParserTest120("AlterCreateResourcePoolStatement120.sql", 2, 8, 8, 8),
			new ParserTest120("CreateIndexStatementTests120.sql", 2, 14, 14, 14),
			new ParserTest120("BackupStatementTests120.sql", 8, 8, 8, 8),
			new ParserTest120("CommitTransactionStatementTests120.sql", 7, 10, 10, 10),
			new ParserTest120("AlterDatabaseOptionsTests120.sql", 4, 4, 4, 4),
			new ParserTest120("TruncatePartitions120.sql", 1, 3, 3, 3),
			new ParserTest120("IncrementalStatsTests.sql", 9, 8, 8, 8),
			new ParserTest120("AlterProcedureStatementTests120.sql", 5, 5, 5, 5),
			new ParserTest120("CreateAlterDatabaseStatementTestsAzure120.sql", 16, 16, 16, 15),
			new ParserTest120("CreateAggregateStatementTests120.sql", 6, 2, 2, 2),
        };


        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]
        public void TSql120SyntaxIn120ParserTest()
        {
            TSql120Parser parser = new TSql120Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql120);
            foreach (ParserTest ti in Only120TestInfos)
            {
				ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result120);	            
            }
        }

        [TestMethod]
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]
        public void TSql120SyntaxIn110ParserTest()
        {
            TSql110Parser parser = new TSql110Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql110);
            foreach (ParserTest ti in Only120TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result110);
            }
        }

        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]
        public void TSql120SyntaxIn100ParserTest()
        {
            TSql100Parser parser = new TSql100Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql100);
            foreach (ParserTest ti in Only120TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result100);
            }
        }

        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]
        public void TSql120SyntaxIn90ParserTest()
        {
            TSql90Parser parser = new TSql90Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql90);
            foreach (ParserTest ti in Only120TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result90);
            }
        }

        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]
        public void TSql120SyntaxIn80ParserTest()
        {
            TSql80Parser parser = new TSql80Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql80);
            foreach (ParserTest ti in Only120TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result80);
            }
        }
    }
}