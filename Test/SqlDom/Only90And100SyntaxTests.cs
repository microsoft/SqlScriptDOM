//------------------------------------------------------------------------------
// <copyright file="Only90And100SyntaxTests.cs" company="Microsoft">
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
        private static readonly ParserTest[] Only90And100TestInfos = {
                // 90 statements, we don't care about individual errors here
            new ParserTest90And100("IPv4AddressTests.sql", 14),
            new ParserTest90And100("AddDropSignatureStatementTests.sql", 1),
            new ParserTest90And100("AlterAssemblyStatementTests.sql", 2),
            new ParserTest90And100("AlterCertificateStatementTests.sql", 2),
            new ParserTest90And100("AlterCreateCredentialStatementTests.sql", 4),
            new ParserTest90And100("AlterCreateServiceStatementTests.sql", 4),
            new ParserTest90And100("AlterEndpointStatementTests.sql", 2),
            new ParserTest90And100("AlterFulltextCatalogStatementTests.sql", 2),
            new ParserTest90And100("AlterMasterKeyStatementTests.sql", 2),
            new ParserTest90And100("AlterMessageTypeStatementTests.sql", 2),
            new ParserTest90And100("AlterPartitionStatementsTests.sql", 2),
            new ParserTest90And100("AlterRemoteServiceBindingStatementTests.sql", 2),
            new ParserTest90And100("AlterSchemaStatementTests.sql", 1),
            new ParserTest90And100("AlterServiceMasterKeyStatementTests.sql", 2),
            new ParserTest90And100("AlterXmlSchemaCollectionStatementTests.sql", 4),
            new ParserTest90And100("ApplicationRoleStatementTests.sql", 4),
            new ParserTest90And100("BackupCertificateStatementTests.sql", 1),
            new ParserTest90And100("BackupRestoreServiceMasterKeyStatementTests.sql", 1),
            new ParserTest90And100("BeginConversationStatementTests.sql", 1),
            new ParserTest90And100("CreateAggregateStatementTests.sql", 2),
            new ParserTest90And100("SymmetricKeyStatementTests.sql", 5),
            new ParserTest90And100("CreateCertificateStatementTests.sql", 2),
            new ParserTest90And100("CreateContractStatementTests.sql", 2),
            new ParserTest90And100("CreateCredentialStatementTests.sql", 2),
            new ParserTest90And100("CreateEndpointStatementTests.sql", 2),
            new ParserTest90And100("CreateEventNotificationStatementTests.sql", 2),
            new ParserTest90And100("CreateFulltextCatalogStatementTests.sql", 2),
            new ParserTest90And100("FulltextIndexStatementTests.sql", 5),
            new ParserTest90And100("LoginStatementTests.sql", 5),
            new ParserTest90And100("CreateMasterKeyStatementTests.sql", 2),
            new ParserTest90And100("CreateMessageTypeStatementTests.sql", 2),
            new ParserTest90And100("CreatePartitionFunctionStatementTests.sql", 2),
            new ParserTest90And100("CreatePartitionSchemeStatementTests.sql", 2),
            new ParserTest90And100("CreateRemoteServiceBindingStatementTests.sql", 2),
            new ParserTest90And100("CreateSynonymStatementTests.sql", 2),
            new ParserTest90And100("CreateTypeStatementTests.sql", 2),
            new ParserTest90And100("CreateXmlIndexStatementTests.sql", 1),
            new ParserTest90And100("CreateXmlSchemaCollectionStatementTests.sql", 4),
            new ParserTest90And100("DeprecatedKeywordsIn90Tests.sql", 1),
            new ParserTest90And100("EndConversationStatementTests.sql", 1),
            new ParserTest90And100("ExecuteAsStatementTests.sql", 1),
            new ParserTest90And100("MoveConversationStatementTests.sql", 1),
            new ParserTest90And100("ReceiveStatementTests.sql", 1),
            new ParserTest90And100("GetConversationGroupStatementTests.sql", 1),
            new ParserTest90("SendStatementTests.sql", new ParserTestOutput(1), new ParserTestOutput("Baselines90"), new ParserTestOutput("Baselines110")),
            new ParserTest90And100("CreateAssemblyStatementTests.sql",2),
            new ParserTest90And100("AsymmetricKeyStatementTests.sql",7),
            new ParserTest90And100("OpenSymmetricKeyStatementTests.sql", 1),
            new ParserTest90And100("QueueStatementTests.sql", 6),
            new ParserTest90And100("RoleStatementTests.sql", 4),
            new ParserTest90And100("RouteStatementTests.sql", 4),
            new ParserTest90And100("CTEStatementTests.sql",4),
            new ParserTest90And100("TryCatchStatementTests.sql", 2),
            new ParserTest90And100("UserStatementTests.sql",4),
            new ParserTest90And100("RevertStatementTests.sql", 1),
            new ParserTest90And100("AlterIndexStatementTests.sql", 2),
            new ParserTest90And100("SecurityStatement90Tests.sql", 6),
            new ParserTest90And100("EnableDisableTriggerStatementTests.sql", 8),
            // Tests that all new DROP cause an error
            new ParserTest90And100("DropStatementsTests2.sql", 
                new ParserErrorInfo(238,"SQL46010", "schema"),
                new ParserErrorInfo(381,"SQL46027"),
                new ParserErrorInfo(648,"SQL46010", "on"),
                new ParserErrorInfo(739,"SQL46010", "user"),
                new ParserErrorInfo(762,"SQL46010", "partition"),
                new ParserErrorInfo(820,"SQL46010", "synonym"),
                new ParserErrorInfo(853,"SQL46010", "application"),
                new ParserErrorInfo(885,"SQL46010", "fulltext"),
                new ParserErrorInfo(919,"SQL46010", "role"),
                new ParserErrorInfo(939,"SQL46010", "type"),
                new ParserErrorInfo(981,"SQL46010", "aggregate"),
                new ParserErrorInfo(1041,"SQL46010", "assembly"),
                new ParserErrorInfo(1100,"SQL46010", "certificate"),
                new ParserErrorInfo(1127,"SQL46010", "credential"),
                new ParserErrorInfo(1153,"SQL46010", "master"),
                new ParserErrorInfo(1176,"SQL46010", "xml"),
                new ParserErrorInfo(1248,"SQL46010", "contract"),
                new ParserErrorInfo(1272,"SQL46010", "endpoint"),
                new ParserErrorInfo(1296,"SQL46010", "message"),
                new ParserErrorInfo(1325,"SQL46010", "queue"),
                new ParserErrorInfo(1389,"SQL46010", "remote"),
                new ParserErrorInfo(1429,"SQL46010", "route"),
                new ParserErrorInfo(1450,"SQL46010", "service"),
                new ParserErrorInfo(1473,"SQL46010", "event")),
    // We care about each error here, because we test extensions to existing statements
                // SQL Sever 80 doesn't allow semicolons in many places...
            new ParserTest90And100("SemicolonsBeforeStatementTests1.sql", 
                new ParserErrorInfo(128,"SQL46010", ";"),
                new ParserErrorInfo(206,"SQL46010", "begin"),
                new ParserErrorInfo(214,"SQL46010", ";")),
            new ParserTest90And100("SemicolonsBeforeStatementTests2.sql", 
                new ParserErrorInfo(159,"SQL46010", ";"),
                new ParserErrorInfo(263,"SQL46010", ";"),
                new ParserErrorInfo(315,"SQL46010", "begin"),
                new ParserErrorInfo(327,"SQL46010", ";")),
            new ParserTest90And100("TrivialStatementTests90.sql",
                new ParserErrorInfo(74, "SQL46010", "symmetric"),
                new ParserErrorInfo(263, "SQL46010", "stats"),
                new ParserErrorInfo(347, "SQL46010", "query"),
                new ParserErrorInfo(440, "SQL46010", "12")),
            new ParserTest90And100("CreateFunctionStatementTests90.sql", 
                new ParserErrorInfo(59, "SQL46010", "execute"),
                new ParserErrorInfo(349, "SQL46010", "execute"),
                new ParserErrorInfo(446, "SQL46010", "WITH"),
                new ParserErrorInfo(568, "SQL46010", "EXTERNAL"),
                new ParserErrorInfo(637, "SQL46010", "("),
                new ParserErrorInfo(760, "SQL46010", "(")),
            new ParserTest90And100("WaitForStatementTests90.sql",
                new ParserErrorInfo(43, "SQL46010", "("),
                new ParserErrorInfo(101, "SQL46010", "(")),
            new ParserTest90And100("RowsetsInSelectTests90.sql", 
                new ParserErrorInfo(48, "SQL46010", "BULK"),
                new ParserErrorInfo(113, "SQL46010", "BULK"),
                new ParserErrorInfo(196, "SQL46010", "BULK")),
            new ParserTest90And100("ScalarDataTypeTests90.sql", 
                new ParserErrorInfo(22, "SQL46010", "("),
                new ParserErrorInfo(78, "SQL46010", "("),
                new ParserErrorInfo(147, "SQL46010", "("),
                new ParserErrorInfo(625, "SQL46010", ".")),
            new ParserTest90And100("MiscTests90.sql", 
                new ParserErrorInfo(32, "SQL46005", "SERIALIZABLE", "SNAPSHOT"),
                new ParserErrorInfo(82, "SQL46010", "PERSISTED"),
                new ParserErrorInfo(134, "SQL46010", "PERSISTED"),
                new ParserErrorInfo(192, "SQL46010", "TO"),
                new ParserErrorInfo(230, "SQL46010", "@device1"),
                new ParserErrorInfo(282, "SQL46010", "xml"),
                new ParserErrorInfo(328, "SQL46010", "DATABASE"),
                new ParserErrorInfo(410, "SQL46010", "ALL"),
                new ParserErrorInfo(482, "SQL46010", "Table"),
                new ParserErrorInfo(574, "SQL46010", "("),
                new ParserErrorInfo(625, "SQL46010", "("),
				new ParserErrorInfo(769, "SQL46010", "stats_stream"),
				new ParserErrorInfo(850, "SQL46010", "0x100"),
				new ParserErrorInfo(912, "SQL46010", "0x100")),
            new ParserTest90And100("SetVariableStatementTests90.sql", 
                new ParserErrorInfo(60, "SQL46010", "."),
                new ParserErrorInfo(109, "SQL46010", "::"),
                new ParserErrorInfo(161, "SQL46010", "."),
                new ParserErrorInfo(199, "SQL46010", "."),
                new ParserErrorInfo(273, "SQL46010", "::"),
                new ParserErrorInfo(320, "SQL46010", "::")),
            new ParserTest90And100("UniqueConstraintTests90.sql", 
                new ParserErrorInfo(96, "SQL46010", "("),
                new ParserErrorInfo(205, "SQL46010", "("),
                new ParserErrorInfo(493, "SQL46010", "(")),
            new ParserTest90And100("InsertStatementTests90.sql", 
                new ParserErrorInfo(23, "SQL46010", "insert"),
                new ParserErrorInfo(156, "SQL46010", "output")),
            new ParserTest90And100("AlterDatabaseOptionsTests90.sql",  
                new ParserErrorInfo(22, "SQL46010", "emergency"),
                new ParserErrorInfo(57, "SQL46010", "db_chaining"),
                new ParserErrorInfo(112, "SQL46010", "trustworthy"),
                new ParserErrorInfo(169, "SQL46010", "auto_update_statistics_async"),
                new ParserErrorInfo(227, "SQL46010", "page_verify"),
                new ParserErrorInfo(381, "SQL46010", "partner"),
                new ParserErrorInfo(446, "SQL46010", "witness"),
                new ParserErrorInfo(544, "SQL46010", "error_broker_conversations"),
                new ParserErrorInfo(632, "SQL46010", "enable_broker"),
                new ParserErrorInfo(709, "SQL46010", "disable_broker"),
                new ParserErrorInfo(760, "SQL46010", "new_broker"),
                new ParserErrorInfo(866, "SQL46010", "DATE_CORRELATION_OPTIMIZATION"),
                new ParserErrorInfo(947, "SQL46010", "PARAMETERIZATION"),
                new ParserErrorInfo(1053, "SQL46010", "ALLOW_SNAPSHOT_ISOLATION"),
                new ParserErrorInfo(1132, "SQL46010", "READ_COMMITTED_SNAPSHOT"),
                new ParserErrorInfo(1215, "SQL46010", "supplemental_logging")),
            new ParserTest90And100("AlterCreateDatabaseStatementTests90.sql", 
                new ParserErrorInfo(39, "SQL46010", "read_only"),
                new ParserErrorInfo(112, "SQL46010", "readwrite"),
                new ParserErrorInfo(193, "SQL46010", "read_write"),
                new ParserErrorInfo(268, "SQL46010", "read_write"),
                new ParserErrorInfo(431, "SQL46010", "FG"),
                new ParserErrorInfo(469, "SQL46010", "offline"),
                new ParserErrorInfo(526, "SQL46010", "rebuild"),
                new ParserErrorInfo(558, "SQL46010", "rebuild"),
                new ParserErrorInfo(694, "SQL46005", "ATTACH", "attach_rebuild_log"),
                new ParserErrorInfo(785, "SQL46005", "ATTACH", "attach_force_rebuild_log"),
                new ParserErrorInfo(905, "SQL46010", "WITH"),
                new ParserErrorInfo(982, "SQL46010", "WITH"),
                new ParserErrorInfo(1056, "SQL46010", "WITH"),
                new ParserErrorInfo(1172, "SQL46010", "AS")),
            new ParserTest90And100("AlterTableDropTableElementStatementTests90.sql", 
                new ParserErrorInfo(76, "SQL46010", "with"),
                new ParserErrorInfo(193, "SQL46010", "with")),
            new ParserTest90And100("AlterTableSwitchStatementTests.sql", 
                new ParserErrorInfo(73, "SQL46010", "switch"),
                new ParserErrorInfo(102, "SQL46010", "switch"),
                new ParserErrorInfo(180, "SQL46010", "switch"),
                new ParserErrorInfo(273, "SQL46010", "switch"),
                new ParserErrorInfo(348, "SQL46010", "switch")),
            new ParserTest90And100("CreateTableTests90.sql", 
                new ParserErrorInfo(82, "SQL46010", "persisted"),
                new ParserErrorInfo(646, "SQL46010", "someColumn"),
                new ParserErrorInfo(756, "SQL46010", ".")),
            new ParserTest90And100("CreateIndexStatementTests90.sql", 
                new ParserErrorInfo(81, "SQL46010", "include"),
                new ParserErrorInfo(126, "SQL46010", "include"),
                new ParserErrorInfo(242, "SQL46010", "("),
                new ParserErrorInfo(538, "SQL46010", "[columnName]"),
                new ParserErrorInfo(603, "SQL46010", "c1")),
            new ParserTest90And100("CreateProcedureStatementTests90.sql", 
                new ParserErrorInfo(87, "SQL46010", "execute"),
                new ParserErrorInfo(217, "SQL46010", "execute"),
                new ParserErrorInfo(324, "SQL46010", "execute"),
                new ParserErrorInfo(429, "SQL46010", "execute")),
            new ParserTest90And100("CreateTriggerStatementTests90.sql", 
                new ParserErrorInfo(56, "SQL46010", "execute"),
                new ParserErrorInfo(160, "SQL46010", "execute"),
                new ParserErrorInfo(290, "SQL46010", "execute"),
                new ParserErrorInfo(399, "SQL46010", "execute"),
                new ParserErrorInfo(599, "SQL46010", "all"),
                new ParserErrorInfo(706, "SQL46010", "database"),
                new ParserErrorInfo(842, "SQL46010", "database")),
            new ParserTest90And100("DeleteStatementTests90.sql", 
                new ParserErrorInfo(30, "SQL46010", "top"),
                new ParserErrorInfo(140, "SQL46010", "output")),
            new ParserTest90And100("ExecuteStatementTests90.sql", 
                new ParserErrorInfo(108, "SQL46010", "AS"),
                new ParserErrorInfo(199, "SQL46010", "as")),
            new ParserTest90And100("ExpressionTests90.sql", 
                new ParserErrorInfo(31, "SQL46010", "t"),
                new ParserErrorInfo(294, "SQL46010", "@a"),
                new ParserErrorInfo(365, "SQL46010", "."),
                new ParserErrorInfo(597, "SQL46010", "::"),
                new ParserErrorInfo(654, "SQL46010", "over")),
            new ParserTest90And100("FromClauseTests90.sql",
                new ParserErrorInfo(77, "SQL46010", "+"),
                new ParserErrorInfo(345, "SQL46010", "p"),
                new ParserErrorInfo(487, "SQL46022", "READCOMMITTEDLOCK"),
                new ParserErrorInfo(657, "SQL46010", "."),
                new ParserErrorInfo(1039, "SQL46010", "PIVOT"),
                new ParserErrorInfo(1276, "SQL46010", "UNPIVOT"),
                new ParserErrorInfo(1592, "SQL46022", "dbo"),
                new ParserErrorInfo(2092, "SQL46010", "tablesample")),
            new ParserTest90And100("CreateSchemaStatementTests90.sql",
                new ParserErrorInfo(46, "SQL46010", "to"),
                new ParserErrorInfo(110, "SQL46010", "deny")),
            new ParserTest90And100("OptimizerHintsTests90.sql",
                new ParserErrorInfo(48, "SQL46005", "BYPASS", "PARAMETERIZATION"),
                new ParserErrorInfo(147, "SQL46010", "OPTIMIZE"),
                new ParserErrorInfo(238, "SQL46010", "CHECKCONSTRAINTS")),
            new ParserTest90And100("UpdateStatementTests90.sql",
                new ParserErrorInfo(72, "SQL46010", "[udt]"),
                new ParserErrorInfo(262, "SQL46010", "output"),
                new ParserErrorInfo(534, "SQL46010", "("))
        };


        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]
        public void TSql90And100SyntaxIn120ParserTest()
        {
            TSql120Parser parser = new TSql120Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql120);

            foreach (ParserTest ti in Only90And100TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result120);
            }
        }

        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]
        public void TSql90And100SyntaxIn110ParserTest()
        {
            TSql110Parser parser = new TSql110Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql110);

            foreach (ParserTest ti in Only90And100TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result110);
            }
        }

        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]
        public void TSql90And100SyntaxIn100ParserTest()
        {
            TSql100Parser parser = new TSql100Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql100);

            foreach (ParserTest ti in Only90And100TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result100);
            }
        }

        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]
        public void TSql90And100SyntaxIn90ParserTest()
        {
            TSql90Parser parser = new TSql90Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql90);
            foreach (ParserTest ti in Only90And100TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result90);
            }
        }

        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]
        public void TSql90And100SyntaxIn80ParserTest()
        {
            TSql80Parser parser = new TSql80Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql80);
            foreach (ParserTest ti in Only90And100TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result80);
            }
        }
    }
}