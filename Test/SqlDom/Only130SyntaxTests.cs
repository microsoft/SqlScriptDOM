//------------------------------------------------------------------------------
// <copyright file="Only130SyntaxTests.cs" company="Microsoft">
//		 Copyright (c) Microsoft Corporation.  All rights reserved.
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
        private static readonly ParserTest[] Only130TestInfos =
        {
            new ParserTest130("AlterDatabaseOptionsTests130.sql", 23, 23, 23, 23, 23),
            new ParserTest130("AlterDatabaseScopedConfigurationTests130.sql", 24, 24, 24, 24, 24),
            new ParserTest130("AlterDatabaseStatementTests130.sql", 4, 4, 4, 4, 4),
            new ParserTest130("EventSessionDbScopeStatementTests.sql", 10, 64, 32, 32, 32),
            new ParserTest130("AlterTableAddIndexTests.sql", 1, 1, 1, 1, 1),
            new ParserTest130("AlterTableStatementTests130.sql", 24, 24, 24, 24, 24),
            new ParserTest130("AlterTableAlterIndex130.sql", 1, 1, 1, 1, 1),
            new ParserTest130("AlterIndexStatementTests130.sql", 2, 7, 7, 7, 7),
            new ParserTest130("AlterTableSwitchStatementTests130.sql", 2, 2, 2, 2, 2),
            new ParserTest130("AlterTableDropTableElementStatementTests130.sql", 1, 1, 1, 1, 1),
            new ParserTest130("ColumnStoreInlineIndex130.sql", 10, 10, 10, 10, 10),
            new ParserTest130("CreateIndexStatementTests130.sql", 6, 6, 6, 6, 6),
            new ParserTest130("CreateTableTests130.sql", 62, 62, 62, 62, 62),
            new ParserTest130("CreateAlterSecurityPolicyStatementTests130.sql", 2, 33, 33, 33, 33),
            new ParserTest130("JsonForClauseTests130.sql", 14, 14, 14, 14, 14),
            new ParserTest130("DropStatementsTests130.sql", 10, 10, 9, 9, 9),
            new ParserTest130("RemoteDataArchiveTableTests130.sql", 13, 13, 13, 13, 13),
            new ParserTest130("RemoteDataArchiveDatabaseTests130.sql", 7, 7, 7, 7, 7),
            new ParserTest130("TemporalSelectTest130.sql", 22, 22, 22, 22, 22),
            new ParserTest130("CreateColumnStoreIndexTests130.sql", 4, 4, 4, 4, 4),
            new ParserTest130("OptimizerHintsTests130.sql", 6, 6, 6, 6, 6),
            new ParserTest130("SecurityStatement130Tests.sql", 2, 0, 0, 0, 0),
            new ParserTest130("CreateExternalDataSourceStatementTests130.sql", 2, 2, 2, 2, 2),
            new ParserTest130("AlterExternalDataSourceStatementTests130.sql", 2, 2, 2, 2, 2),
            new ParserTest130("CreateExternalFileFormatStatementTests130.sql", 2, 2, 2, 2, 2),
            new ParserTest130("CreateExternalTableStatementTests130.sql", 2, 2, 2, 2, 2),
            new ParserTest130("CreateUserFromExternalProvider130.sql", 4, 2, 2, 2, 2),
            new ParserTest130("CreateHekatonTriggerStatementTest.sql", 1, 1, 1, 1, 0),
            new ParserTest130("AlterTableAlterColumnStatementTests130.sql", 1, 39, 39, 39, 39),
            new ParserTest130("NativelyCompiledScalarUDFTests130.sql",4, 4, 4, 4, 4),
            new ParserTest130("AlwaysEncryptedTests130.sql",1, 2, 2, 2, 2),
            new ParserTest130("UniqueInlineIndex130.sql", 9, 7, 7, 7, 5),
            new ParserTest130("AlterDatabaseScopedCredentialStatementTests130.sql", 2, 2, 2, 2, 2),
            new ParserTest130("CreateDatabaseScopedCredentialStatementTests130.sql", 2, 2, 2, 2, 2),
            new ParserTest130("DropDatabaseScopedCredentialStatementTests130.sql", 1, 1, 1, 1, 1),
            new ParserTest130("AlterServerConfigurationSoftNumaTests130.sql", 2, 2, 2, 2, 2),
            new ParserTest130("DropIfExistsTests130.sql", 30, 22, 22, 21, 21),
            new ParserTest130("FromClauseTests130.sql", 12, 12, 10, 10, 10),
            new ParserTest130("CreateMasterKeyStatementTests130.sql", 2, 1, 1, 1, 1),
            new ParserTest130("AtTimeZoneTests130.sql", 9, 9, 9, 9, 9),
            new ParserTest130("CreateExternalResourcePoolStatementTests130.sql", 2, 2, 2, 2, 2),
            new ParserTest130("AlterExternalResourcePoolStatementTests130.sql", 2, 2, 2, 2, 2),
            new ParserTest130("CreateWorkloadGroupStatementTests130.sql", 2, 6, 2, 2, 2),
            new ParserTest130("CreateWorkloadGroupStatementSqlDwTests130.sql", 2, 9, 0, 0, 0),
            new ParserTest130("AlterWorkloadGroupStatementTests130.sql", 2, 6, 2, 2, 2),
            new ParserTest130("SelectWithCollation.sql", 4, 4, 4, 0, 0),
            new ParserTest130("CreateOrAlterStatementTests130.sql", 4, 2, 2, 2, 2),
            new ParserTest130("RenameStatementTests.sql", 7, 7, 7, 7, 7),
            new ParserTest130("CtasStatementTests.sql", 15, 15, 15, 15, 15),
            new ParserTest130("ExternalTableCtasStatementTests.sql", 8, 2, 2, 2, 2),
            new ParserTest130("GroupByClauseTests130.sql", 4, 4, 2, 2, 2),
            new ParserTest130("WithinGroupTests130.sql", 5, 5, 5, 3, 3),
            new ParserTest130("MaterializedViewTests130.sql", 19, 13, 13, 13, 13),
            new ParserTest130("CreateColumnStoreIndexTestsDw.sql", 1, 1, 1, 1, 1),
            new ParserTest130("CreateTriggerStatementTests130.sql", 1, 1, 0, 0, 0),
            new ParserTest130("CopyCommandTestsDw.sql", 1, 1, 1, 1, 1),
            new ParserTest130("DataClassificationTests130.sql", 1, 4, 4, 4, 4),
            new ParserTest130("AlterWorkloadGroupStatementSqlDwTests.sql", 12, 6, 5, 5, 5),
            new ParserTest130("CreateWorkloadClassifierStatementSqlDwTests.sql", 2, 1, 1, 1, 1),
            new ParserTest130("PredictSqlDwTests.sql", 3, 3, 3, 3, 3),
            new ParserTest130("CreateEventSessionNotLikePredicate.sql", 2, 1, 1, 1, 1)
        };

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql130SyntaxIn130ParserTest()
        {
            TSql130Parser parser = new TSql130Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql130);
            foreach (ParserTest ti in Only130TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result130);
            }
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql130SyntaxIn120ParserTest()
        {
            TSql120Parser parser = new TSql120Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql120);
            foreach (ParserTest ti in Only130TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result120);
            }
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql130SyntaxIn110ParserTest()
        {
            TSql110Parser parser = new TSql110Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql110);
            foreach (ParserTest ti in Only130TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result110);
            }
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql130SyntaxIn100ParserTest()
        {
            TSql100Parser parser = new TSql100Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql100);
            foreach (ParserTest ti in Only130TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result100);
            }
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql130SyntaxIn90ParserTest()
        {
            TSql90Parser parser = new TSql90Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql90);
            foreach (ParserTest ti in Only130TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result90);
            }
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql130SyntaxIn80ParserTest()
        {
            TSql80Parser parser = new TSql80Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql80);
            foreach (ParserTest ti in Only130TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result80);
            }
        }

        /// <summary>
        /// Let's make sure the older syntax tests still pass in the new parser
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql120SyntaxIn130ParserTest()
        {
            TSql130Parser parser = new TSql130Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql120);
            foreach (ParserTest ti in Only120TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result120);
            }
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql110SyntaxIn130ParserTest()
        {
            TSql130Parser parser = new TSql130Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql110);
            foreach (ParserTest ti in Only110TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result110);
            }
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql100SyntaxIn130ParserTest()
        {
            TSql130Parser parser = new TSql130Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql100);
            foreach (ParserTest ti in Only100TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result100);
            }
        }
    }
}
