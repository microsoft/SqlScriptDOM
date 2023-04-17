//------------------------------------------------------------------------------
// <copyright file="Only100SyntaxTests.cs" company="Microsoft">
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
        private static readonly ParserTest[] Only110TestInfos = {
            new ParserTest110("AlterAvailabilityGroupStatementTests.sql", 24, 12, 12),
            new ParserTest110("AlterSearchPropertyListStatementTests.sql", 4, 4, 4),
            new ParserTest110("AlterSelectiveXmlIndexStatementTests.sql", 2, 6, 6),
            new ParserTest110("AlterSequenceStatementTests.sql", 2, 6, 6),
            new ParserTest110("AlterServerConfigurationStatementTests110.sql", 2, 26, 26),
            new ParserTest110("AuditSpecificationStatementTests110.sql", 6, 4, 4),
            new ParserTest110("CreateAvailabilityGroupStatementTests.sql", 4, 2, 2),
            new ParserTest110("CreateAlterDatabaseStatementTestsAzure110.sql", 7, 7, 7),
            new ParserTest110("CreateAlterDatabaseStatementTests110.sql", 20, 20, 20),
            new ParserTest110("CreateAlterResourcePoolStatementTests110.sql", 2, 4, 4),
            new ParserTest110("CreateAlterTableStatementTests110.sql", 10, 10, 10),
            new ParserTest110("CreateAlterUserStatementTests110.sql", 4, 5, 5),
            new ParserTest110("CreateAlterWorkloadGroupStatementTests110.sql", 2, 4, 4),
            new ParserTest110("CreateAlterFederationStatementTestsAzure110.sql", 8, 4, 4),
            new ParserTest110("CreateIndexStatementTests110.sql", 2, 8, 8),
            new ParserTest110("CreateSearchPropertyListStatementTests.sql", 2, 3, 3),
            new ParserTest110("CreateSelectiveXmlIndexStatementTests.sql", 2, 15, 15),
            new ParserTest110("CreateSequenceStatementTests.sql", 2, 6, 6),
            new ParserTest110("CreateSpatialIndexStatementTests110.sql", 2, 3, 3),
            new ParserTest110("DropStatementsTests110.sql", 5, 5, 5),
            new ParserTest110("ExecuteStatementTests110.sql", 6, 6, 6),
            new ParserTest110("ExpressionTests110.sql", 9, 9, 9),
            new ParserTest110("FromClauseTests110.sql", 5, 5, 5),
            new ParserTest110("FulltextIndexStatementTests110.sql", 12, 15, 14),
            new ParserTest110("MiscTests110.sql", 1, 1, 1),
            new ParserTest110("OffsetClause.sql", 
                new ParserErrorInfo(27, "SQL46010", "offset")),
            new ParserTest110("RoleStatementTests110.sql", 2, 2, 2),
            new ParserTest110("ServerAuditStatementTests110.sql", 8, 4, 4),
            new ParserTest110("ServerRoleStatementTests.sql", 4, 7, 7),
            new ParserTest110("SendStatementTests110.sql", 1, 1, 1),
            new ParserTest110("ThrowStatementTests.sql", 
                new ParserErrorInfo(7, "SQL46010", "throw")),
            new ParserTest110("OverClauseTests110.sql", 14, 12, 12),
            new ParserTest110("UseFederationTests110.sql", 3, 3, 3), 
		    new ParserTest110("OptimizerHintsTests110.sql", 1, 1, 1),
		    new ParserTest110("WhitespaceTests.sql", 1, 1, 1),
        };


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql110SyntaxIn120ParserTest()
        {
            TSql120Parser parser = new TSql120Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql120);
            foreach (ParserTest ti in Only110TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result120);
            }
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql110SyntaxIn110ParserTest()
        {
            TSql110Parser parser = new TSql110Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql110);
            foreach (ParserTest ti in Only110TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result110);
            }
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql110SyntaxIn100ParserTest()
        {
            TSql100Parser parser = new TSql100Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql100);
            foreach (ParserTest ti in Only110TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result100);
            }
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql110SyntaxIn90ParserTest()
        {
            TSql90Parser parser = new TSql90Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql90);
            foreach (ParserTest ti in Only110TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result90);
            }
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql110SyntaxIn80ParserTest()
        {
            TSql80Parser parser = new TSql80Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql80);
            foreach (ParserTest ti in Only110TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result80);
            }
        }
    }
}