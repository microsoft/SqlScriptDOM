//------------------------------------------------------------------------------
// <copyright file="CommonSyntaxTests.cs" company="Microsoft">
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
        private static readonly ParserTest[] CommonTestInfos = {
            new ParserTestCommon("AlterDatabaseStatementTests.sql"),
            new ParserTestCommon("AlterProcedureStatementTests.sql"),
            new ParserTestCommon("AlterTableAlterColumnStatementTests.sql"),
            new ParserTestCommon("AlterTableConstraintModificationStatementTests.sql"),
            new ParserTestCommon("AlterTableDropTableElementStatementTests.sql"),
            new ParserTestCommon("AlterTableTriggerModificationStatementTests.sql"),
            new ParserTestCommon("AlterTriggerStatementTests.sql"),
            new ParserTestCommon("BackupStatementTests.sql"),
            new ParserTest("CreateSchemaStatementTests.sql", "Baselines80", "Baselines90", "Baselines90"),
            new ParserTest("ColumnDefinitionTests.sql", "Baselines80", "Baselines90", "Baselines90"),
            new ParserTestCommon("RestoreStatementTests.sql"),
            new ParserTestCommon("BeginEndBlockStatementTests.sql"),
            new ParserTestCommon("BeginTransactionStatementTests.sql"),
            new ParserTestCommon("BulkInsertStatementTests.sql"),
            new ParserTestCommon("CommitTransactionStatementTests.sql"),
            new ParserTest("CreateDefaultStatementTests.sql", "Baselines80", "Baselines90", "Baselines90"),
            new ParserTest("CreateRuleStatementTests.sql", "Baselines80", "Baselines90", "Baselines90"),
            new ParserTestCommon("CreateStatisticsStatementTests.sql"),
            new ParserTestCommon("IdentifierTests.sql"),
            new ParserTestCommon("BaseTableNameTests.sql"),
            new ParserTestCommon("BooleanExpressionTests.sql"),
            new ParserTestCommon("ForeignKeyConstraintTests.sql"),
            new ParserTestCommon("IntegerTests.sql"),
            new ParserTestCommon("NullableConstraintTests.sql"),
            new ParserTestCommon("ScalarDataTypeTests.sql"),
            new ParserTest("UniqueConstraintTests.sql", "Baselines80", "Baselines90", "Baselines90"),
            new ParserTestCommon("CursorStatementsTests.sql"),
            new ParserTestCommon("DbccStatementsTests.sql"),
            new ParserTestCommon("DeclareCursorStatementTests.sql"),
            new ParserTestCommon("DeclareVariableStatementTests.sql"),
            new ParserTestCommon("GotoStatementTests.sql"),
            new ParserTestCommon("IfStatementTests.sql"),
            new ParserTestCommon("InsertStatementTests.sql"),
            new ParserTestCommon("PrintStatementTests.sql"),
            new ParserTestCommon("RaiseErrorStatementTests.sql"),
            new ParserTestCommon("ReadTextStatementTests.sql"),
            new ParserTestCommon("ReturnStatementTests.sql"),
            new ParserTestCommon("RollbackTransactionStatementTests.sql"),
            new ParserTestCommon("SaveTransactionStatementTests.sql"),
            new ParserTestCommon("SelectStatementTests.sql"),
            new ParserTestCommon("SelectExpressionTests.sql"),
            new ParserTestCommon("QueryExpressionTests.sql"),
            new ParserTestCommon("ForClauseTests.sql"),
            new ParserTestCommon("OptimizerHintsTests.sql"),
            new ParserTestCommon("RowsetsInSelectTests.sql"),
            new ParserTestCommon("PredicateSetTests.sql"),
            new ParserTestCommon("SetOffsetsAndOnOffSetTests.sql"),
            new ParserTestCommon("SetCommandsAndMiscTests.sql"),
            new ParserTestCommon("SetVariableStatementTests.sql"),
            new ParserTestCommon("TrivialStatementTests.sql"),
            new ParserTestCommon("UpdateStatementTests.sql"),
            new ParserTestCommon("UpdateStatisticsStatementTests.sql"),
            new ParserTestCommon("UpdateTextStatementTests.sql"),
            new ParserTest("ViewStatementTests.sql", "Baselines80", "Baselines90", "Baselines90"),
            new ParserTestCommon("WaitForStatementTests.sql"),
            new ParserTestCommon("WhileStatementTests.sql"),
            new ParserTestCommon("WriteTextStatementTests.sql"),
            new ParserTestCommon("VariableTests.sql"),
            new ParserTestCommon("FunctionStatementTests.sql"),
            new ParserTestCommon("TSqlParserTestScript1.sql"),
            new ParserTestCommon("TSqlParserTestScript2.sql"),
            new ParserTestCommon("TSqlParserTestScript3.sql"),
            new ParserTestCommon("TSqlParserTestScript5.sql"), 
            new ParserTestCommon("ZeroLengthFile.sql"),
            new ParserTestCommon("AlterDatabaseOptionsTests.sql"),
            new ParserTestCommon("CreateDatabaseStatementTests.sql"),
            new ParserTest("CreateIndexStatementTests.sql", "Baselines80", "Baselines90", "Baselines90"),
            new ParserTestCommon("CreateProcedureStatementTests.sql"),
            new ParserTestCommon("CreateTriggerStatementTests.sql"),
            new ParserTestCommon("DeclareTableStatementTests.sql"),
            new ParserTestCommon("DeleteStatementTests.sql"),
            new ParserTestCommon("DropStatementsTests.sql"),
            new ParserTestCommon("ExecuteStatementTests.sql"),
            new ParserTestCommon("ExpressionTests.sql"),
            new ParserTestCommon("FromClauseTests.sql"),
            // Added the test to validate the support for Big Int values of RowCount and PageCount
            new ParserTestCommon("BigIntRowCountPageCountTests.sql")
        };

        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]
        public void CommonSyntaxIn80ParserTest()
        {
            TSql80Parser parser = new TSql80Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql80);

            foreach (ParserTest ti in CommonTestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result80);
            }
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]
        public void CommonSyntaxIn90ParserTest()
        {
            TSql90Parser parser = new TSql90Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql90);
            foreach (ParserTest ti in CommonTestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result90);
            }
        }


 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]
        public void CommonSyntaxIn100ParserTest()
        {
            TSql100Parser parser = new TSql100Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql100);
            foreach (ParserTest ti in CommonTestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result100);
            }
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]
        public void CommonSyntaxIn110ParserTest()
        {
            TSql110Parser parser = new TSql110Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql110);
            foreach (ParserTest ti in CommonTestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result100);
            }
        }


        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]
        public void CommonSyntaxIn120ParserTest()
        {
            TSql120Parser parser = new TSql120Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql120);
            foreach (ParserTest ti in CommonTestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result120);
            }
        }


        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]
        public void CommonSyntaxIn130ParserTest()
        {
            TSql130Parser parser = new TSql130Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql130);
            foreach (ParserTest ti in CommonTestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result130);
            }
        }


        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]
        public void CommonSyntaxIn140ParserTest()
        {
            TSql140Parser parser = new TSql140Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql140);
            foreach (ParserTest ti in CommonTestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result140);
            }
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void CommonSyntaxIn150ParserTest()
        {
            TSql150Parser parser = new TSql150Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql150);
            foreach (ParserTest ti in CommonTestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result150);
            }
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void CommonSyntaxIn160ParserTest()
        {
            TSql160Parser parser = new TSql160Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql160);
            foreach (ParserTest ti in CommonTestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result160);
            }
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void CommonSyntaxIn170ParserTest()
        {
            TSql170Parser parser = new TSql170Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql170);
            foreach (ParserTest ti in CommonTestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result170);
            }
        }
    }
}
