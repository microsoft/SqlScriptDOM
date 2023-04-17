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
        private static readonly ParserTest[] Only100TestInfos = {
            new ParserTest100("MergeStatementTests.sql", 8, 9),
            new ParserTest100("DeclareVariableWithAssignmentTests.sql", 2, 1),
            new ParserTest100("AssignmentWithOperationTests.sql", 25, 25),
            new ParserTest100("TableParametersTests.sql", 6, 4),
            new ParserTest100("DatabaseAuditSpecificationStatementTests.sql", 7, 7),
            new ParserTest100("ServerAuditSpecificationStatementTests.sql", 5, 8),
            new ParserTest100("DatabaseEncryptionKeyStatementTests.sql", 6, 6),
            new ParserTest100("CreateAlterDropResourcePoolStatementTests.sql", 5, 11),
            new ParserTest100("AlterCreateDatabaseStatementTests100.sql",
                new ParserTestOutput(
                    new ParserErrorInfo(77, "SQL46010", "CONTAINS"),
                    new ParserErrorInfo(242, "SQL46010", "CONTAINS"),
					new ParserErrorInfo(500, "SQL46010", "WITH"),
					new ParserErrorInfo(814, "SQL46010", "WITH")
                    ),
                new ParserTestOutput(
                    new ParserErrorInfo(77, "SQL46010", "CONTAINS")
                    )),
            new ParserTest100("CreateAlterDropWorkloadGroupStatementTests.sql", 5, 12),
            new ParserTest100("AlterDatabaseOptionsTests100.sql",
                    new ParserErrorInfo(53, "SQL46010", "compatibility_level"),
                    new ParserErrorInfo(101, "SQL46010", "compatibility_level"),
                    new ParserErrorInfo(174, "SQL46010", "encryption"),
                    new ParserErrorInfo(227, "SQL46010", "honor_broker_priority"),
                    new ParserErrorInfo(306, "SQL46010", "vardecimal_storage_format"),
                    new ParserErrorInfo(414, "SQL46010", "change_tracking"),
                    new ParserErrorInfo(459, "SQL46010", "change_tracking"),
                    new ParserErrorInfo(503, "SQL46010", "change_tracking"),
                    new ParserErrorInfo(567, "SQL46010", "change_tracking"),
                    new ParserErrorInfo(657, "SQL46010", "change_tracking"),
                    new ParserErrorInfo(724, "SQL46010", "change_tracking")),
            new ParserTest100("CreateAlterDropBrokerPriorityStatementTests.sql", 5, 10),
            new ParserTest100("CreateAlterDropFulltextStoplistStatementTests.sql", 5, 17),
            new ParserTest100("AlterTableStatementTests100.sql",
                new ParserTestOutput(
                    new ParserErrorInfo(27, "SQL46010", "rebuild"),
                    new ParserErrorInfo(51, "SQL46010", "rebuild"),
                    new ParserErrorInfo(91, "SQL46010", "rebuild"),
                    new ParserErrorInfo(147, "SQL46010", "rebuild"),
                    new ParserErrorInfo(254, "SQL46010", "change_tracking"),
                    new ParserErrorInfo(294, "SQL46010", "change_tracking"),
                    new ParserErrorInfo(333, "SQL46010", "change_tracking"),
                    new ParserErrorInfo(406, "SQL46010", "change_tracking"),
                    new ParserErrorInfo(483, "SQL46010", "set"),
                    new ParserErrorInfo(553, "SQL46010", "set"),
                    new ParserErrorInfo(628, "SQL46010", "set"),
                    new ParserErrorInfo(737, "SQL46010", "sparse"),
                    new ParserErrorInfo(781, "SQL46010", "sparse"),
                    new ParserErrorInfo(882, "SQL46010", "max"),
                    new ParserErrorInfo(950, "SQL46010", "sparse"),
                    new ParserErrorInfo(993, "SQL46010", "column_set"),
                    new ParserErrorInfo(1104, "SQL46010", "sparse")),
                new ParserTestOutput(
                    new ParserErrorInfo(27, "SQL46010", "rebuild"),
                    new ParserErrorInfo(51, "SQL46005", "SWITCH", "rebuild"),
                    new ParserErrorInfo(91, "SQL46005", "SWITCH", "rebuild"),
                    new ParserErrorInfo(147, "SQL46010", "rebuild"),
                    new ParserErrorInfo(247, "SQL46005", "SWITCH", "enable"),
                    new ParserErrorInfo(286, "SQL46005", "SWITCH", "disable"),
                    new ParserErrorInfo(326, "SQL46005", "SWITCH", "enable"),
                    new ParserErrorInfo(399, "SQL46005", "SWITCH", "enable"),
                    new ParserErrorInfo(483, "SQL46010", "set"),
                    new ParserErrorInfo(553, "SQL46010", "set"),
                    new ParserErrorInfo(628, "SQL46010", "set"),
                    new ParserErrorInfo(737, "SQL46005", "PERSISTED", "sparse"),
                    new ParserErrorInfo(781, "SQL46005", "PERSISTED", "sparse"),
                    new ParserErrorInfo(887, "SQL46010", "filestream"),
                    new ParserErrorInfo(950, "SQL46010", "sparse"),
                    new ParserErrorInfo(993, "SQL46010", "column_set"),
                    new ParserErrorInfo(1104, "SQL46010", "sparse"))),
            new ParserTest100("AlterIndexStatementTests100.sql", 
                new ParserTestOutput(2),
                new ParserTestOutput(
                    new ParserErrorInfo(69, "SQL46010", "ALL"),
                    new ParserErrorInfo(116, "SQL46010", "ALL"))),
            new ParserTest100("CreateTableTests100.sql", 
                new ParserTestOutput(
                    new ParserErrorInfo(102, "SQL46010", "WITH"),
                    new ParserErrorInfo(208, "SQL46010", "WITH"),
                    new ParserErrorInfo(318, "SQL46005", "TEXTIMAGE_ON", "FILESTREAM_ON"),
                    new ParserErrorInfo(446, "SQL46010", "FILESTREAM_ON"),
                    new ParserErrorInfo(505, "SQL46005", "TEXTIMAGE_ON", "FILESTREAM_ON")),
                new ParserTestOutput(
                    new ParserErrorInfo(107, "SQL46010", "("),
                    new ParserErrorInfo(213, "SQL46010", "("),
                    new ParserErrorInfo(318, "SQL46005", "TEXTIMAGE_ON", "FILESTREAM_ON"),
                    new ParserErrorInfo(446, "SQL46010", "FILESTREAM_ON"),
                    new ParserErrorInfo(505, "SQL46005", "TEXTIMAGE_ON", "FILESTREAM_ON"))),
            new ParserTest100("ColumnDefinitionTests100.sql",
                new ParserTestOutput(
                    new ParserErrorInfo(51, "SQL46010", "sparse"),
                    new ParserErrorInfo(284, "SQL46010", "max"),
                    new ParserErrorInfo(334, "SQL46010", "sparse"),
                    new ParserErrorInfo(375, "SQL46010", "column_set"),
                    new ParserErrorInfo(618, "SQL46010", "table")),
                new ParserTestOutput(
                    new ParserErrorInfo(51, "SQL46010", "sparse"),
                    new ParserErrorInfo(289, "SQL46010", "filestream"),
                    new ParserErrorInfo(334, "SQL46010", "sparse"),
                    new ParserErrorInfo(375, "SQL46010", "column_set"),
                    new ParserErrorInfo(632, "SQL46010", "sparse"))),
            new ParserTest100("ServerAuditStatementTests.sql", 5, 9),
            new ParserTest100("CryptographicProviderStatementTests.sql", 5, 5),
            new ParserTest100("CreateIndexStatementTests100.sql", 
                new ParserTestOutput(
                    new ParserErrorInfo(50, "SQL46010", "WHERE"),
                    new ParserErrorInfo(96, "SQL46010", "WHERE"),
                    new ParserErrorInfo(149, "SQL46010", "WHERE"),
                    new ParserErrorInfo(199, "SQL46010", "WHERE"),
                    new ParserErrorInfo(268, "SQL46010", "WHERE"),
                    new ParserErrorInfo(335, "SQL46010", "WHERE"),
                    new ParserErrorInfo(430, "SQL46010", "WHERE"),
                    new ParserErrorInfo(486, "SQL46010", "WHERE"),
                    new ParserErrorInfo(579, "SQL46010", "WHERE"),
                    new ParserErrorInfo(724, "SQL46010", "("),
                    new ParserErrorInfo(849, "SQL46010", "FILESTREAM_ON"),
                    new ParserErrorInfo(918, "SQL46010", "INCLUDE"),
					new ParserErrorInfo(1043, "SQL46010", "WHERE")),
                new ParserTestOutput(
                    new ParserErrorInfo(50, "SQL46010", "WHERE"),
                    new ParserErrorInfo(96, "SQL46010", "WHERE"),
                    new ParserErrorInfo(149, "SQL46010", "WHERE"),
                    new ParserErrorInfo(199, "SQL46010", "WHERE"),
                    new ParserErrorInfo(268, "SQL46010", "WHERE"),
                    new ParserErrorInfo(335, "SQL46010", "WHERE"),
                    new ParserErrorInfo(430, "SQL46010", "WHERE"),
                    new ParserErrorInfo(486, "SQL46010", "WHERE"),
                    new ParserErrorInfo(579, "SQL46010", "WHERE"),
                    new ParserErrorInfo(725, "SQL46010", "DATA_COMPRESSION"),
                    new ParserErrorInfo(849, "SQL46010", "FILESTREAM_ON"),
                    new ParserErrorInfo(931, "SQL46010", "WHERE"),
					new ParserErrorInfo(1043, "SQL46010", "WHERE"))),
            new ParserTest100("EventSessionStatementTests.sql", 5, 32),
            new ParserTest100("AlterResourceGovernorStatementTests.sql", 2, 5),
            new ParserTest100("CreateSpatialIndexStatementTests.sql", 2, 9),
            new ParserTest100("SymmetricKeyStatementTests100.sql", 
                    new ParserTestOutput(3), 
                    new ParserTestOutput(
                        new ParserErrorInfo(53, "SQL46010", "from"),
                        new ParserErrorInfo(95, "SQL46010", "from"),
                        new ParserErrorInfo(176, "SQL46010", "from"),
                        new ParserErrorInfo(307, "SQL46010", "from"),
                        new ParserErrorInfo(416, "SQL46010", "remove"))),
            new ParserTest100("AsymmetricKeyStatementTests100.sql", 
                new ParserTestOutput(3), 
                new ParserTestOutput(
                        new ParserErrorInfo(30, "SQL46005", "ASSEMBLY", "provider"),
                        new ParserErrorInfo(94, "SQL46005", "ASSEMBLY", "provider"),
                        new ParserErrorInfo(220, "SQL46005", "ASSEMBLY", "provider"),
                        new ParserErrorInfo(324, "SQL46010", "remove"))),
            new ParserTest100("LoginStatementTests100.sql", 
                new ParserTestOutput(2), 
                new ParserTestOutput(
                        new ParserErrorInfo(15, "SQL46010", "ADD"),
                        new ParserErrorInfo(50, "SQL46010", "DROP"))),
            new ParserTest100("CreateCredentialStatementTests100.sql", 
                new ParserTestOutput(2),
                new ParserTestOutput(
                    new ParserErrorInfo(53, "SQL46010", "FOR"),
                    new ParserErrorInfo(158, "SQL46010", "FOR"))),
            new ParserTest100("FulltextIndexStatementTests100.sql",
                new ParserTestOutput(4),
                new ParserTestOutput(
                    new ParserErrorInfo(98, "SQL46010", "("),
                    new ParserErrorInfo(150, "SQL46010", "("),
                    new ParserErrorInfo(212, "SQL46010", "("),
                    new ParserErrorInfo(278, "SQL46010", "("),
                    new ParserErrorInfo(403, "SQL46010", "="),
                    new ParserErrorInfo(461, "SQL46010", "("),
                    new ParserErrorInfo(551, "SQL46005", "CHANGE_TRACKING", "STOPLIST"),
                    new ParserErrorInfo(618, "SQL46010", "("),
                    new ParserErrorInfo(725, "SQL46010", "("),
                    new ParserErrorInfo(873, "SQL46005", "CHANGE_TRACKING", "STOPLIST"),
                    new ParserErrorInfo(920, "SQL46005", "CHANGE_TRACKING", "STOPLIST"),
                    new ParserErrorInfo(989, "SQL46005", "CHANGE_TRACKING", "STOPLIST"))),
            new ParserTest100("CreateFunctionStatementTests100.sql",
                new ParserTestOutput(3),
                new ParserTestOutput(
                    new ParserErrorInfo(90, "SQL46010", "ORDER"),
                    new ParserErrorInfo(185, "SQL46010", "ORDER"),
                    new ParserErrorInfo(410, "SQL46010", "ORDER"))),
            new ParserTest100("DropIndexStatementTests100.sql", 
                new ParserTestOutput(5),
                new ParserTestOutput(
                    new ParserErrorInfo(52, "SQL46010", "filestream_on"),
                    new ParserErrorInfo(172, "SQL46010", "filestream_on"),
                    new ParserErrorInfo(319, "SQL46010", "data_compression"),
                    new ParserErrorInfo(371, "SQL46010", "data_compression"),
                    new ParserErrorInfo(436, "SQL46010", "filestream_on"))),
            new ParserTest100("CreateAggregateStatementTests100.sql",
                new ParserTestOutput(2),
                new ParserTestOutput(new ParserErrorInfo(63, "SQL46010", ","))),
            new ParserTest100("CreateStatisticsStatementTests100.sql", 
                new ParserTestOutput(4),
                new ParserTestOutput(
                    new ParserErrorInfo(63, "SQL46010", "WHERE"),
                    new ParserErrorInfo(136, "SQL46010", "WHERE"),
                    new ParserErrorInfo(265, "SQL46010", "WHERE"),
                    new ParserErrorInfo(361, "SQL46010", "WHERE"))),
            new ParserTest100("CTEStatementTests100.sql", 
                new ParserTestOutput(4),
                new ParserTestOutput(
                    new ParserErrorInfo(80, "SQL46010", "0xff"),
                    new ParserErrorInfo(263, "SQL46010", "0xff"),
                    new ParserErrorInfo(373, "SQL46010", "0xff"),
                    new ParserErrorInfo(536, "SQL46010", "0xff"))),
            new ParserTest100("InsertStatementTests100.sql",
                new ParserErrorInfo(178, "SQL46010", ","),
                new ParserErrorInfo(485, "SQL46010", "table"),
                new ParserErrorInfo(676, "SQL46010", "$ACTION")),
            new ParserTest100("FromClauseTests100.sql",
                new ParserTestOutput(
                    new ParserErrorInfo(62, "SQL46010", "INSERT"),
                    new ParserErrorInfo(168, "SQL46010", "UPDATE"),
                    new ParserErrorInfo(263, "SQL46010", "DELETE"),
                    new ParserErrorInfo(350, "SQL46010", "MERGE"),
                    new ParserErrorInfo(538, "SQL46010", "t1"),
                    new ParserErrorInfo(888, "SQL46010", "("),
                    new ParserErrorInfo(1066, "SQL46022", "dbo"),
                    new ParserErrorInfo(1225, "SQL46022", "FORCESEEK")
                ),
                new ParserTestOutput(
                    new ParserErrorInfo(62, "SQL46010", "INSERT"),
                    new ParserErrorInfo(168, "SQL46010", "UPDATE"),
                    new ParserErrorInfo(263, "SQL46010", "DELETE"),
                    new ParserErrorInfo(350, "SQL46010", "MERGE"),
                    new ParserErrorInfo(538, "SQL46010", "t1"),
                    new ParserErrorInfo(888, "SQL46010", "("),
                    new ParserErrorInfo(1097, "SQL46010", ","),
                    new ParserErrorInfo(1225, "SQL46022", "FORCESEEK"))),
            new ParserTest100("RowsetsInSelectTests100.sql",
                new ParserTestOutput(1),
                new ParserTestOutput(
                    new ParserErrorInfo(103, "SQL46010", "ORDER"))),
            new ParserTest100("GroupByClauseTests100.sql",
                new ParserErrorInfo(177, "SQL46010", "SETS"),
                new ParserErrorInfo(293, "SQL46010", ","),
                new ParserErrorInfo(537, "SQL46010", "SETS"),
                new ParserErrorInfo(974, "SQL46010", "(")),
            new ParserTest100("ExpressionTests100.sql",
                new ParserTestOutput(
                    new ParserErrorInfo(49, "SQL46010", "t"),
                    new ParserErrorInfo(91, "SQL46010", "."),
                    new ParserErrorInfo(236, "SQL46010", "."),
                    new ParserErrorInfo(391, "SQL46010", ","),
                    new ParserErrorInfo(442, "SQL46010", ",")
                    ),
                new ParserTestOutput(
                    new ParserErrorInfo(52, "SQL46010", "a"),
                    new ParserErrorInfo(105, "SQL46010", "COLLATE"),
                    new ParserErrorInfo(250, "SQL46010", "COLLATE"),
                    new ParserErrorInfo(391, "SQL46010", ","),
                    new ParserErrorInfo(442, "SQL46010", ","))),
            new ParserTest100("EventNotificationStatementTests100.sql",
                new ParserTestOutput(4),
                new ParserTestOutput(
                    new ParserErrorInfo(131, "SQL46010", "DDL_CREDENTIAL_EVENTS"),
                    new ParserErrorInfo(261, "SQL46010", "DROP_REMOTE_SERVER"))),
            new ParserTest100("AlterServerConfigurationStatementTests.sql",
                new ParserTestOutput(
                    new ParserErrorInfo(0, "SQL46010", "ALTER"),
                    new ParserErrorInfo(6, "SQL46010", "SERVER")),
                new ParserTestOutput(
                    new ParserErrorInfo(6, "SQL46010", "SERVER"),
                    new ParserErrorInfo(68, "SQL46010", "SERVER"),
                    new ParserErrorInfo(138, "SQL46010", "SERVER"))),
            new ParserTest100("BackupStatementTests100.sql",
                new ParserTestOutput(
                    new ParserErrorInfo(96, "SQL46010", "COMPRESSION"),
                    new ParserErrorInfo(169, "SQL46010", "NO_COMPRESSION")),
                new ParserTestOutput(
                    new ParserErrorInfo(96, "SQL46010", "COMPRESSION"),
                    new ParserErrorInfo(169, "SQL46010", "NO_COMPRESSION"))),
            new ParserTest100("QueueStatementTests100.sql",
                new ParserTestOutput(4),
                new ParserTestOutput(
                    new ParserErrorInfo(108, "SQL46005", "ACTIVATION", "poison_message_handling"),
                    new ParserErrorInfo(195, "SQL46005", "ACTIVATION", "poison_message_handling"))),
            new ParserTest100("MiscTests100.sql",
                new ParserTestOutput(
                    new ParserErrorInfo(39, "SQL46010", "$rowguid"),
                    new ParserErrorInfo(112, "SQL46010", "OPTIMIZE"),
                    new ParserErrorInfo(238, "SQL46010", "extended_logical_checks")),
                new ParserTestOutput(
                    new ParserErrorInfo(39, "SQL46010", "$rowguid"),
                    new ParserErrorInfo(190, "SQL46010", "unknown"),
                    new ParserErrorInfo(238, "SQL46010", "extended_logical_checks"))),
        };


        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]
        public void TSql100SyntaxIn120ParserTest()
        {
            TSql120Parser parser = new TSql120Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql120);
            foreach (ParserTest ti in Only100TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result120);
            }
        }


        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]
        public void TSql100SyntaxIn110ParserTest()
        {
            TSql110Parser parser = new TSql110Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql110);
            foreach (ParserTest ti in Only100TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result110);
            }
        }


        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]
        public void TSql100SyntaxIn100ParserTest()
        {
            TSql100Parser parser = new TSql100Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql100);
            foreach (ParserTest ti in Only100TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result100);
            }
        }


        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]
        public void TSql100SyntaxIn90ParserTest()
        {
            TSql90Parser parser = new TSql90Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql90);
            foreach (ParserTest ti in Only100TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result90);
            }
        }


        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]
        public void TSql100SyntaxIn80ParserTest()
        {
            TSql80Parser parser = new TSql80Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql80);
            foreach (ParserTest ti in Only100TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result80);
            }
        }
    }
}