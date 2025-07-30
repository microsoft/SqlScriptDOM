//------------------------------------------------------------------------------
// <copyright file="ParserErrorsTests.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlStudio.Tests.AssemblyTools.TestCategory;

namespace SqlStudio.Tests.UTSqlScriptDom
{
    public partial class SqlDomTests
    {
        /// <summary>
        /// Negative tests for create table with generated always
        /// as user id/name start/end columns
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void CreateTableGeneratedAlwaysNegativeTest()
        {
            // Generated always as UserId start column must be unique
            ParserTestUtils.ErrorTest130("CREATE TABLE T (C0 INT PRIMARY KEY CLUSTERED, C1 VARBINARY(85) GENERATED ALWAYS AS SUSER_SID START NOT NULL, C2 VARBINARY(85) GENERATED ALWAYS AS SUSER_SID START NOT NULL)",
                new ParserErrorInfo(16, "SQL46109"));

            // Generated always as UserId end column must be unique
            ParserTestUtils.ErrorTest130("CREATE TABLE T (C0 INT PRIMARY KEY CLUSTERED, C1 VARBINARY(85) GENERATED ALWAYS AS SUSER_SID END NOT NULL, C2 VARBINARY(85) GENERATED ALWAYS AS SUSER_SID END NOT NULL)",
                new ParserErrorInfo(16, "SQL46110"));

            // Generated always as UserName start column must be unique
            ParserTestUtils.ErrorTest130("CREATE TABLE T (C0 INT PRIMARY KEY CLUSTERED, C1 NVARCHAR(128) GENERATED ALWAYS AS SUSER_SNAME START NOT NULL, C2 NVARCHAR(128) GENERATED ALWAYS AS SUSER_SNAME START NOT NULL)",
                new ParserErrorInfo(16, "SQL46111"));

            // Generated always as UserName end column must be unique
            ParserTestUtils.ErrorTest130("CREATE TABLE T (C0 INT PRIMARY KEY CLUSTERED, C1 NVARCHAR(128) GENERATED ALWAYS AS SUSER_SNAME END NOT NULL, C2 NVARCHAR(128) GENERATED ALWAYS AS SUSER_SNAME END NOT NULL)",
                new ParserErrorInfo(16, "SQL46112"));

            // Cannot create generated always column when SYSTEM_TIME period is not defined
            ParserTestUtils.ErrorTest130("CREATE TABLE T (C0 INT PRIMARY KEY CLUSTERED, C1 VARBINARY(85) GENERATED ALWAYS AS SUSER_SID START NOT NULL)",
                new ParserErrorInfo(16, "SQL46113"));

            // Incorrect syntax
            ParserTestUtils.ErrorTest130("CREATE TABLE T (C0 INT PRIMARY KEY CLUSTERED, C1 VARBINARY(85) GENERATED ALWAYS AS SUSER_SD START NOT NULL)",
                new ParserErrorInfo(83, "SQL46010", "SUSER_SD"));

            ParserTestUtils.ErrorTest130("CREATE TABLE T (C0 INT PRIMARY KEY CLUSTERED, C1 VARBINARY(85) GENERATED ALWAYS AS SUSER_SD END NOT NULL)",
                new ParserErrorInfo(83, "SQL46010", "SUSER_SD"));

            ParserTestUtils.ErrorTest130("CREATE TABLE T (C0 INT PRIMARY KEY CLUSTERED, C1 VARBINARY(85) GENERATED ALWAYS AS SUSER_SID STAR NOT NULL)",
                new ParserErrorInfo(93, "SQL46010", "STAR"));

            ParserTestUtils.ErrorTest130("CREATE TABLE T (C0 INT PRIMARY KEY CLUSTERED, C1 VARBINARY(85) GENERATED ALWAYS AS SUSER_SID START NOT NULL HIDDEN)",
                new ParserErrorInfo(108, "SQL46010", "HIDDEN"));

            ParserTestUtils.ErrorTest130("CREATE TABLE T (C0 INT PRIMARY KEY CLUSTERED, C1 VARBINARY(85) GENERATED ALWAYS AS SUSER_SID START HIDEN)",
                new ParserErrorInfo(99, "SQL46005", "HIDDEN", "HIDEN"));

            // Generated always columns without period definition
            ParserTestUtils.ErrorTest130("CREATE TABLE T (A INT PRIMARY KEY, B DATETIME2 GENERATED ALWAYS AS ROW START, C DATETIME2 GENERATED ALWAYS AS ROW END)",
                new ParserErrorInfo(16, "SQL46113"));

            //Incorrect keyword for Distribution Replicate table option
            ParserTestUtils.ErrorTest130("CREATE TABLE t (COL0 INT NOT NULL, COL1 VARCHAR (20), COL2 VARCHAR (6)) WITH (DISTRIBUTION = REPLICAT)",
                new ParserErrorInfo(93, "SQL46010", "REPLICAT", "REPLICAT"));

            //Incorrect syntax for Clustered Index table option
            ParserTestUtils.ErrorTest130("CREATE TABLE t (COL0 INT NOT NULL, COL1 VARCHAR (20), COL2 VARCHAR (6)) WITH (CLUSTERED INDEX)",
                new ParserErrorInfo(93, "SQL46010", ")", "Incorrect syntax near "));

            //Incorrect number of parameters for the Distribution Hash table option
            ParserTestUtils.ErrorTest130("CREATE TABLE t (COL0 INT NOT NULL, COL1 VARCHAR (20), COL2 VARCHAR (6)) WITH (DISTRIBUTION = HASH())",
                new ParserErrorInfo(98, "SQL46010", ")", "Incorrect syntax near "));

            //Incorrect number of parameters for the Distribution Hash table - Multiple Column Distribution option
            ParserTestUtils.ErrorTest130("CREATE TABLE t (COL0 INT NOT NULL, COL1 VARCHAR (20), COL2 VARCHAR (6)) WITH (DISTRIBUTION = HASH(COL1,,COL2))",
                new ParserErrorInfo(103, "SQL46010", ",", "Incorrect syntax near "));

            //Incorrect number of parameters for the Partition table option
            ParserTestUtils.ErrorTest130("CREATE TABLE t (COL0 INT NOT NULL, COL1 VARCHAR (20), COL2 VARCHAR (6)) WITH (PARTITION(RANGE RIGHT FOR VALUES (10, 20)))",
               new ParserErrorInfo(94, "SQL46010", "RIGHT", "Incorrect syntax near RIGHT.>."));

            //Incorrect location value for the Location table option
            string invalidLocationSyntax = @"CREATE TABLE TableWithLocation
  (
    id INT NOT NULL,
    lastName VARCHAR(20),
    zipCode VARCHAR(6)
  )
WITH
  (
    DISTRIBUTION = REPLICATE,
    HEAP,
    LOCATION = INVALID_LOCATION
  );";
            ParserTestUtils.ErrorTest130(invalidLocationSyntax,
               new ParserErrorInfo(invalidLocationSyntax.IndexOf("INVALID_LOCATION"), "SQL46010", "INVALID_LOCATION"));

            //Incorrect syntax of Ordered columns
            string orderedColumnsSyntax = @"CREATE TABLE TableWithOrderColumns
  (
    id INT NOT NULL,
    lastName VARCHAR(20),
    zipCode VARCHAR(6)
  )
WITH
  (
    DISTRIBUTION = REPLICATE,
    CLUSTERED COLUMNSTORE INDEX (id, lastName)
  );";
            ParserTestUtils.ErrorTest130(orderedColumnsSyntax,
               new ParserErrorInfo(orderedColumnsSyntax.IndexOf("(id, lastName)"), "SQL46010", "("));

            //Incorrect syntax of Ordered columns
            string orderedColumnsSyntax2 = @"CREATE TABLE TableCIWithOrderColumns
  (
    id INT NOT NULL,
    lastName VARCHAR(20),
    zipCode VARCHAR(6)
  )
WITH
  (
    DISTRIBUTION = REPLICATE,
    CLUSTERED INDEX ORDER(id, lastName)
  );";
            ParserTestUtils.ErrorTest130(orderedColumnsSyntax2,
               new ParserErrorInfo(orderedColumnsSyntax2.IndexOf("ORDER(id, lastName)"), "SQL46010", "ORDER"));


            // PRIMARY KEY and UNIQUE constraint are only supported with NOT ENFORCED
            string primaryKeySyntax = @"CREATE TABLE [dbo].[table8] (
    [col1] INT NOT NULL,
    [col2] INT NOT NULL,
    PRIMARY KEY NONCLUSTERED ([col1] DESC) ENFORCED
);";
            ParserTestUtils.ErrorTest130(primaryKeySyntax,
               new ParserErrorInfo(primaryKeySyntax.IndexOf("ENFORCED"), "SQL46010", "ENFORCED"));

            ParserTestUtils.ErrorTest130(@"CREATE TABLE table6 (c1 INT, c2 INT UNIQUE ENFORCED);",
               new ParserErrorInfo(43, "SQL46010", "ENFORCED"));

            string notEnforcedSyntax = @"CREATE TABLE [dbo].[table8] (
    [col1] INT NOT NULL,
    [col2] INT NOT NULL,
    PRIMARY KEY NONCLUSTERED ([col1] DESC) NOT ENFORCE
);";
            ParserTestUtils.ErrorTest130(notEnforcedSyntax,
               new ParserErrorInfo(notEnforcedSyntax.IndexOf("ENFORCE"), "SQL46010", "ENFORCE"));
        }

        /// <summary>
        /// Ledger Negative tests for create table with generated always
        /// as TRANSACTION_ID/SEQUENCE_NUMBER start/end columns
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void CreateTableLedgerGeneratedAlwaysNegativeTest()
        {
            // Generated always as TRANSACTION_ID start column must be unique
            //
            ParserTestUtils.ErrorTest160("CREATE TABLE table1 (a INT,b INT, TRANSACTIONID_START BIGINT GENERATED ALWAYS AS TRANSACTION_ID START NOT NULL, TRANSACTIONID_START BIGINT GENERATED ALWAYS AS TRANSACTION_ID START NOT NULL)",
                new ParserErrorInfo(21, "SQL46136"));

            // Generated always as TRANSACTION_ID end column must be unique
            //
            ParserTestUtils.ErrorTest160("CREATE TABLE table1 (a INT, b INT,TRANSACTIONID_START BIGINT GENERATED ALWAYS AS TRANSACTION_ID START NOT NULL, " +
                "TRANSACTIONID_END BIGINT GENERATED ALWAYS AS TRANSACTION_ID END NOT NULL, TRANSACTIONID_END BIGINT GENERATED ALWAYS AS TRANSACTION_ID END NOT NULL)",
                new ParserErrorInfo(21, "SQL46137"));

            // Generated always as SEQUENCE_NUMBER start column must be unique
            //
            ParserTestUtils.ErrorTest160("CREATE TABLE table1 (a INT, b INT, SEQUENCENUMBER_START BIGINT GENERATED ALWAYS AS SEQUENCE_NUMBER START NOT NULL, " +
                "SEQUENCENUMBER_START BIGINT GENERATED ALWAYS AS SEQUENCE_NUMBER START HIDDEN NOT NULL)",
                new ParserErrorInfo(21, "SQL46138"));

            // Generated always as SEQUENCE_NUMBER end column must be unique
            //
            ParserTestUtils.ErrorTest160("CREATE TABLE table1 (a INT, b INT, SEQUENCENUMBER_END BIGINT GENERATED ALWAYS AS SEQUENCE_NUMBER END NOT NULL, " +
                "SEQUENCENUMBER_END BIGINT GENERATED ALWAYS AS SEQUENCE_NUMBER END HIDDEN NOT NULL)",
                new ParserErrorInfo(21, "SQL46139"));

            // Wrong Keyword TRANSACTION_D
            //
            ParserTestUtils.ErrorTest160("CREATE TABLE T (C0 INT PRIMARY KEY CLUSTERED, C1 BIGINT GENERATED ALWAYS AS TRANSACTION_D START NOT NULL)",
               new ParserErrorInfo(76, "SQL46010", "TRANSACTION_D"));

            // Wrong Keyword TRANSACTION_D
            //
            ParserTestUtils.ErrorTest160("CREATE TABLE T (C0 INT PRIMARY KEY CLUSTERED, C1 BIGINT GENERATED ALWAYS AS TRANSACTION_D END NOT NULL)",
                new ParserErrorInfo(76, "SQL46010", "TRANSACTION_D"));

            // Wrong Keyword STAR
            //
            ParserTestUtils.ErrorTest160("CREATE TABLE T (C0 INT PRIMARY KEY CLUSTERED, C1 BIGINT GENERATED ALWAYS AS TRANSACTION_ID STAR NOT NULL)",
                new ParserErrorInfo(91, "SQL46010", "STAR"));

            // Wrong Keyword HIDEN
            //
            ParserTestUtils.ErrorTest160("CREATE TABLE T (C0 INT PRIMARY KEY CLUSTERED, C1 BIGINT GENERATED ALWAYS AS TRANSACTION_ID START NOT NULL HIDEN)",
                new ParserErrorInfo(106, "SQL46010", "HIDEN"));
        }

        /// <summary>
        /// Negative tests for HIDDEN columns
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void HiddenColumnsNegativeTest()
        {
            // multiple hidden tokens
            //
            ParserTestUtils.ErrorTest140("CREATE TABLE T(A INT HIDDEN HIDDEN, B INT)",
                  new ParserErrorInfo(28, "SQL46010", "HIDDEN"));

            ParserTestUtils.ErrorTest140("CREATE TABLE T([HIDDEN] INT [HIDDEN], B INT)",
                new ParserErrorInfo(28, "SQL46010", "[HIDDEN]"));

            ParserTestUtils.ErrorTest140("alter table t1 alter column c1 varbinary(max) filestream sparse hidden hidden not null;",
                new ParserErrorInfo(71, "SQL46010", "hidden"));

            ParserTestUtils.ErrorTest140("alter table t1 alter column c1 varbinary(max) filestream hidden sparse;",
                new ParserErrorInfo(64, "SQL46010", "sparse"));

            // hidden token on unexpected places
            //
            ParserTestUtils.ErrorTest140("CREATE TABLE T(A HIDDEN INT, B INT)",
                new ParserErrorInfo(24, "SQL46010", "INT"));

            ParserTestUtils.ErrorTest140("ALTER TABLE t1 ALTER COLUMN c1 VARCHAR (100) COLLATE Traditional_Spanish_ci_ai NULL HIDden;",
                new ParserErrorInfo(84, "SQL46010", "HIDden"));

            ParserTestUtils.ErrorTest140("ALTER TABLE t1 ALTER COLUMN c1 DROP HIDDEN HIDDEN",
                new ParserErrorInfo(43, "SQL46010", "HIDDEN"));

            ParserTestUtils.ErrorTest140(@"CREATE TABLE tab_with_ignoredupkey_suppressmessages_on_uk (COL0 INT UNIQUE HIDDEN WITH (ALLOW_PAGE_LOCKS = ON, IGNORE_DUP_KEY = ON (SUPPRESS_MESSAGES = ON), ALLOW_ROW_LOCKS = ON));",
                new ParserErrorInfo(75, "SQL46010", "HIDDEN"));

            string hiddenColsSyntax = @"CREATE TABLE tab_with_hidden_cols (
                                            [K] INT HIDDEN              UNIQUE,
                                            [L] UNIQUEIDENTIFIER  DEFAULT '123' ROWGUIDCOL NOT NULL HIDDEN UNIQUE";
            ParserTestUtils.ErrorTest140(hiddenColsSyntax,
                new ParserErrorInfo(hiddenColsSyntax.IndexOf("HIDDEN UNIQUE"), "SQL46010", "HIDDEN"));

            ParserTestUtils.ErrorTest140("CREATE TABLE T(HIDDEN HIDDEN INT, B CHAR(32) HIDDEN)",
                 new ParserErrorInfo(29, "SQL46010", "INT"));

            string hiddenMaskedSyntax = @"CREATE TABLE tab_with_hidden_and_masked (
                                            [PK]    INT                                                       NOT NULL,
                                            [Data1] NVARCHAR (32) HIDDEN MASKED WITH (FUNCTION = 'default()')  DEFAULT 'a' NOT NULL,
                                            [Data2] NVARCHAR (32) MASKED WITH (FUNCTION = 'default()')        DEFAULT 'b' NOT NULL,
                                            CONSTRAINT [constr1] PRIMARY KEY ([PK]));";
            ParserTestUtils.ErrorTest140(hiddenMaskedSyntax,
                 new ParserErrorInfo(hiddenMaskedSyntax.IndexOf("HIDDEN MASKED") + 7, "SQL46010", "MASKED"));

        }

        /// <summary>
        /// Negative tests for BULK INSERT command
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void BulkInsertNegativeTestCases()
        {
            ParserTestUtils.ErrorTest140("BULK INSERT t1 FROM 'f1' WITH (INCLUDE_HIDDEN, INCLUDE_HIDDEN)",
                new ParserErrorInfo(47, "SQL46049", "INCLUDE_HIDDEN"));

            ParserTestUtils.ErrorTest140("BULK INSERT t1 FROM 'f1' WITH (INCLUDE HIDDEN)",
                 new ParserErrorInfo(31, "SQL46010", "INCLUDE"));

            ParserTestUtils.ErrorTest140("BULK INSERT t1 FROM 'f1' WITH ( )",
                 new ParserErrorInfo(32, "SQL46010", ")"));
        }

        /// <summary>
        /// Negative tests for alter table with generated always
        /// as user id/name start/end columns
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void AlterTableAddGeneratedAlwaysNegativeTest()
        {
            // Generated always as UserId start column must be unique
            ParserTestUtils.ErrorTest130("ALTER TABLE T ADD C0 INT PRIMARY KEY CLUSTERED, C1 VARBINARY(85) GENERATED ALWAYS AS SUSER_SID START NOT NULL, C2 VARBINARY(85) GENERATED ALWAYS AS SUSER_SID START NOT NULL",
                new ParserErrorInfo(18, "SQL46109"));

            // Generated always as UserId end column must be unique
            ParserTestUtils.ErrorTest130("ALTER TABLE T ADD C0 INT PRIMARY KEY CLUSTERED, C1 VARBINARY(85) GENERATED ALWAYS AS SUSER_SID END NOT NULL, C2 VARBINARY(85) GENERATED ALWAYS AS SUSER_SID END NOT NULL",
                new ParserErrorInfo(18, "SQL46110"));

            // Generated always as UserName start column must be unique
            ParserTestUtils.ErrorTest130("ALTER TABLE T ADD C0 INT PRIMARY KEY CLUSTERED, C1 NVARCHAR(128) GENERATED ALWAYS AS SUSER_SNAME START NOT NULL, C2 NVARCHAR(128) GENERATED ALWAYS AS SUSER_SNAME START NOT NULL",
                new ParserErrorInfo(18, "SQL46111"));

            // Generated always as UserName end column must be unique
            ParserTestUtils.ErrorTest130("ALTER TABLE T ADD C0 INT PRIMARY KEY CLUSTERED, C1 NVARCHAR(128) GENERATED ALWAYS AS SUSER_SNAME END NOT NULL, C2 NVARCHAR(128) GENERATED ALWAYS AS SUSER_SNAME END NOT NULL",
                new ParserErrorInfo(18, "SQL46112"));

            // Generated always as UserId start column must be unique (attempt to add together with period for system_time)
            ParserTestUtils.ErrorTest130("ALTER TABLE T ADD C0 INT PRIMARY KEY CLUSTERED, PERIOD FOR SYSTEM_TIME (S0, S1), C1 VARBINARY(85) GENERATED ALWAYS AS SUSER_SID START NOT NULL, C2 VARBINARY(85) GENERATED ALWAYS AS SUSER_SID START NOT NULL",
                new ParserErrorInfo(18, "SQL46109"));
        }

        /// <summary>
        /// Negative tests for alter column with generated always
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void AlterColumnAddGeneratedAlwaysNegativeTest()
        {
            ParserTestUtils.ErrorTest130("ALTER TABLE T ALTER COLUMN C GENERATED ALWAYS AS SUSER_SID START",
                new ParserErrorInfo(39, "SQL46010", "ALWAYS"));

            ParserTestUtils.ErrorTest130("ALTER TABLE T ALTER COLUMN C VARBINARY(85) GENERATED AS SUSER_SID START",
                new ParserErrorInfo(43, "SQL46010", "GENERATED"));

            ParserTestUtils.ErrorTest130("ALTER TABLE T ALTER COLUMN C VARBINARY(85) GENERATED ALWAYS AS SUSER_SID HIDDEN",
                new ParserErrorInfo(73, "SQL46010", "HIDDEN"));

            ParserTestUtils.ErrorTest130("ALTER TABLE T ALTER COLUMN C VARBINARY(85) GENERATED ALWAYS AS SUSER_SID END NOT NULL HIDDEN",
                new ParserErrorInfo(86, "SQL46010", "HIDDEN"));

            ParserTestUtils.ErrorTest130("ALTER TABLE T ALTER COLUMN C ADD GENERATED ALWAYS AS SUSER_SID END",
                new ParserErrorInfo(33, "SQL46010", "GENERATED"));

            ParserTestUtils.ErrorTest130("ALTER TABLE T ALTER COLUMN C DROP GENERATED ALWAYS AS SUSER_SID END",
                new ParserErrorInfo(34, "SQL46010", "GENERATED"));

            ParserTestUtils.ErrorTest130("ALTER TABLE T ALTER COLUMN C VARBINARY(85) GENERATED ALWAYS AS SUSER_SD START HIDDEN",
                new ParserErrorInfo(63, "SQL46010", "SUSER_SD"));

            ParserTestUtils.ErrorTest130("ALTER TABLE T ALTER COLUMN C VARBINARY(85) GENERATED ALWAYS AS SUSER_SD END HIDDEN",
                new ParserErrorInfo(63, "SQL46010", "SUSER_SD"));

            ParserTestUtils.ErrorTest130("ALTER TABLE T ALTER COLUMN C VARBINARY(85) GENERATED ALWAYS AS SUSER_SNAME START HIDEN",
                new ParserErrorInfo(81, "SQL46005", "HIDDEN", "HIDEN"));
        }

        /// <summary>
        /// Negative tests for ADD/DROP HIDDEN syntax
        /// for temporal generated always columns
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void AddDropHiddenNegativeTests()
        {
            ParserTestUtils.ErrorTest130("ALTER TABLE T1 ALTER COLUMN C1 ADD DROP HIDDEN",
                new ParserErrorInfo(35, "SQL46010", "DROP"));

            ParserTestUtils.ErrorTest130("ALTER TABLE T1 ALTER COLUMN C1 ADD HIDDEN DROP HIDDEN",
                new ParserErrorInfo(47, "SQL46010", "HIDDEN"));

            ParserTestUtils.ErrorTest130("ALTER TABLE T1 ALTER COLUMN C1 ADD MASKED (FUNCTION = 'default()') HIDDEN",
                new ParserErrorInfo(35, "SQL46010", "MASKED"));

            ParserTestUtils.ErrorTest130("ALTER TABLE T1 ALTER COLUMN C1 ADD HIDDEN NOT NULL",
                new ParserErrorInfo(35, "SQL46010", "HIDDEN"));

            ParserTestUtils.ErrorTest130("ALTER TABLE T1 ALTER COLUMN C1 ADD SPARSE HIDDEN",
                new ParserErrorInfo(42, "SQL46010", "HIDDEN"));

            ParserTestUtils.ErrorTest130("ALTER TABLE T1 ALTER COLUMN C1 DROP HIDDEN NULL",
                new ParserErrorInfo(36, "SQL46010", "HIDDEN"));
        }

        /// <summary>
        /// Negative tests for SPLIT/MERGE syntax
        /// for ALTER TABLE
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void AddSplitMergeNegativeTests()
        {
            ParserTestUtils.ErrorTest130("ALTER TABLE T SPLI RANGE ('2004-01-01')",
                new ParserErrorInfo(14, "SQL46010", "SPLI"));

            ParserTestUtils.ErrorTest130("ALTER TABLE T MERG RANGE ('2004-01-01')",
                new ParserErrorInfo(14, "SQL46010", "MERG"));

            ParserTestUtils.ErrorTest130("ALTER TABLE T SPLIT RANGE ()",
               new ParserErrorInfo(27, "SQL46010", ")"));

            ParserTestUtils.ErrorTest130("ALTER TABLE T SPLIT RANGE (10, 20)",
               new ParserErrorInfo(29, "SQL46010", ","));

            ParserTestUtils.ErrorTest130("ALTER TABLE T MERGE RANGE (10, 20)",
               new ParserErrorInfo(29, "SQL46010", ","));
        }

        /// <summary>
        /// Negative tests for OPENJSON syntax.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void OpenJsonNegativeTest()
        {
            ParserTestUtils.ErrorTest130("select * from openjson()",
                new ParserErrorInfo(23, "SQL46010", ")"));

            ParserTestUtils.ErrorTest130("select * from openjson('{}', '$', '$')",
                new ParserErrorInfo(32, "SQL46010", ","));

            ParserTestUtils.ErrorTest130("select * from openjson('{}', '$') with",
                new ParserErrorInfo(34, "SQL46010", "with"));

            ParserTestUtils.ErrorTest130("select * from openjson('{}', '$') with ()",
                new ParserErrorInfo(40, "SQL46010", ")"));

            ParserTestUtils.ErrorTest130("select * from openjson('{}', '$') with ('$.a' nvarchar(max))",
                new ParserErrorInfo(40, "SQL46010", "'$.a'"));

            ParserTestUtils.ErrorTest130("select * from openjson('[{\"a\":\"string\"}]', '$[0]') with (a nvarchar(max) json)",
                new ParserErrorInfo(73, "SQL46010", "json"));

            ParserTestUtils.ErrorTest130("select * from openjson('{\"a\":\"string\"}', '$') with (a nvarchar(max) as json N'$.a')",
                new ParserErrorInfo(76, "SQL46010", "N'$.a'"));

            ParserTestUtils.ErrorTest130("select * from openjson('{\"column1\":\"string\"}') with (column1 as json )",
                new ParserErrorInfo(61, "SQL46010", "as"));
        }

        /// <summary>
        /// Negative tests for JSON_OBJECT syntax in functions
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void JsonObjectSyntaxNegativeTest()
        {
            // Incorrect placing of colon
            ParserTestUtils.ErrorTest160("SELECT JSON_OBJECT('name':'value''type':1)",
               new ParserErrorInfo(39, "SQL46010", ":"));

            // Cannot use empty key
            ParserTestUtils.ErrorTest160("SELECT JSON_OBJECT('name':'value',:1)",
               new ParserErrorInfo(34, "SQL46010", ":"));

            // cannot use expression without colon
            ParserTestUtils.ErrorTest160("SELECT JSON_OBJECT('name')",
                new ParserErrorInfo(19, "SQL46010", "'name'"));

            // cannot use expression without colon
            ParserTestUtils.ErrorTest160("SELECT JSON_OBJECT('name' ABSENT ON NULL)",
               new ParserErrorInfo(26, "SQL46010", "ABSENT"));

            // Cannot use key:value pair with another non JSON_OBJECT Syntax
            ParserTestUtils.ErrorTest160("SELECT TRIM('name':'value','type':1)",
               new ParserErrorInfo(18, "SQL46010", ":"));

            // Cannot use Absent On Null with another non JSON_OBJECT Syntax
            ParserTestUtils.ErrorTest160("SELECT TRIM('name' ABSENT ON NULL)",
               new ParserErrorInfo(19, "SQL46010", "ABSENT"));

            // Cannot use empty value
            ParserTestUtils.ErrorTest160("SELECT JSON_OBJECT('name':'value','type':)",
               new ParserErrorInfo(41, "SQL46010", ")"));

            // Cannot use incomplete absent on null clause cases
            ParserTestUtils.ErrorTest160("SELECT JSON_OBJECT('name':'value', 'type':NULL ABSENT ON)",
                new ParserErrorInfo(56, "SQL46010", ")"));

            // Cannot use incomplete null on null clause cases
            ParserTestUtils.ErrorTest160("SELECT JSON_OBJECT('name':'value', 'type':NULL NULL ON)",
                new ParserErrorInfo(54, "SQL46010", ")"));
        }

        /// <summary>
        /// Negative tests for JSON_ARRAY syntax in functions
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void JsonArraySyntaxNegativeTest()
        {
            // Incorrect placing of colon
            ParserTestUtils.ErrorTest160("SELECT JSON_ARRAY('name':'value')",
               new ParserErrorInfo(18, "SQL46010", "'name'"));

            // Incorrect placing of single quotes
            ParserTestUtils.ErrorTest160("SELECT JSON_ARRAY('name)",
               new ParserErrorInfo(18, "SQL46030", "'name)"));

            // Cannot use incomplete absent on null clause cases
            ParserTestUtils.ErrorTest160("SELECT JSON_ARRAY('name', 'value', 'type', NULL ABSENT ON)",
                new ParserErrorInfo(57, "SQL46010", ")"));

            // Cannot use incomplete null on null clause cases
            ParserTestUtils.ErrorTest160("SELECT JSON_ARRAY('name', 'value', 'type', NULL NULL ON)",
                new ParserErrorInfo(55, "SQL46010", ")"));

            // Cannot use incomplete null on null clause cases
            ParserTestUtils.ErrorTest160("SELECT JSON_ARRAY('name', 'value', NULL, 'type' ON NULL)",
                new ParserErrorInfo(48, "SQL46010", "ON"));
        }

        /// <summary>
        /// Negative tests for JSON_OBJECTAGG syntax in functions
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void JsonObjectAGGSyntaxNegativeTest()
        {
            // Incorrect key value parameter number
            ParserTestUtils.ErrorTest170("SELECT JSON_OBJECTAGG('name':'value', 'type':1)",
               new ParserErrorInfo(36, "SQL46010", ","));

            // No closing quotation mark
            ParserTestUtils.ErrorTest170("SELECT JSON_OBJECTAGG('name':'value)",
               new ParserErrorInfo(29, "SQL46030", "'value)"));

            // Incorrect placing of colon
            ParserTestUtils.ErrorTest170("SELECT JSON_OBJECTAGG('name':'value''type':1)",
               new ParserErrorInfo(42, "SQL46010", ":"));

            // cannot use expression without colon
            ParserTestUtils.ErrorTest170("SELECT JSON_OBJECTAGG('name')",
                new ParserErrorInfo(22, "SQL46010", "'name'"));

            // cannot use expression without colon
            ParserTestUtils.ErrorTest170("SELECT JSON_OBJECTAGG('name' ABSENT ON NULL)",
               new ParserErrorInfo(29, "SQL46010", "ABSENT"));

            // Cannot use empty value
            ParserTestUtils.ErrorTest170("SELECT JSON_OBJECTAGG('name':)",
               new ParserErrorInfo(29, "SQL46010", ")"));

            // Cannot use incomplete absent on null clause cases
            ParserTestUtils.ErrorTest170("SELECT JSON_OBJECTAGG('name':NULL ABSENT ON)",
                new ParserErrorInfo(43, "SQL46010", ")"));

            // Cannot use Incomplete RETURNING clause
            ParserTestUtils.ErrorTest170("SELECT JSON_OBJECTAGG('name':NULL RETURNING ON)",
                    new ParserErrorInfo(46, "SQL46010", ")"));

            // Cannot use anything other than JSON in RETURNING clause
            ParserTestUtils.ErrorTest170("SELECT JSON_OBJECTAGG('name':NULL RETURNING INT)",
                    new ParserErrorInfo(44, "SQL46005", "JSON", "INT"));
        }

        /// <summary>
        /// Negative tests for Data Masking Alter Column syntax.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void DataMaskingAlterColumnSyntaxNegativeTest()
        {
            // Add data masking: missing WITH keyword
            //
            ParserTestUtils.ErrorTest130("ALTER TABLE T1 ALTER COLUMN C1 ADD MASKED (FUNCTION = 'default()'));",
                new ParserErrorInfo(35, "SQL46010", "MASKED"));

            // Add data masking: misspelled MASKED keyword
            //
            ParserTestUtils.ErrorTest130("ALTER TABLE T1 ALTER COLUMN C1 ADD MASKD WITH (FUNCTION = 'default()'));",
                new ParserErrorInfo(35, "SQL46010", "MASKD"));

            // Add data masking: missing parenthesis
            //
            ParserTestUtils.ErrorTest130("ALTER TABLE T1 ALTER COLUMN C1 ADD MASKED WITH FUNCTION = 'default()');",
                new ParserErrorInfo(47, "SQL46010", "FUNCTION"));

            // Add data masking: missing equals sign
            //
            ParserTestUtils.ErrorTest130("ALTER TABLE T1 ALTER COLUMN C1 ADD MASKED WITH (FUNCTION 'default()'));",
                new ParserErrorInfo(57, "SQL46010", "'default()'"));

            // Add data masking: missing string literal single quotes
            //
            ParserTestUtils.ErrorTest130("ALTER TABLE T1 ALTER COLUMN C1 ADD MASKED WITH (FUNCTION = default()));",
                new ParserErrorInfo(59, "SQL46010", "default"));

            // Add data masking: missing FUNCTION keyword
            //
            ParserTestUtils.ErrorTest130("ALTER TABLE T1 ALTER COLUMN C1 ADD MASKED WITH ('default()'));",
                new ParserErrorInfo(48, "SQL46010", "'default()'"));

            // Add data masking: misspelled WITH keyword
            //
            ParserTestUtils.ErrorTest130("ALTER TABLE T1 ALTER COLUMN C1 ADD MASKED WTH (FUNCTION = 'default()'));",
                new ParserErrorInfo(35, "SQL46010", "MASKED"));

            // Add data masking: misspelled FUNCTION keyword
            //
            ParserTestUtils.ErrorTest130("ALTER TABLE T1 ALTER COLUMN C1 ADD MASKED WITH (FNCTION = 'default()'));",
                new ParserErrorInfo(48, "SQL46010", "FNCTION"));

            // Drop data masking: misspelled MASKED keyword
            //
            ParserTestUtils.ErrorTest130("ALTER TABLE T1 ALTER COLUMN C1 DROP MSKED;",
                new ParserErrorInfo(36, "SQL46010", "MSKED"));

            // Change data masking: missing WITH keyword
            //
            ParserTestUtils.ErrorTest130("ALTER TABLE T1 ALTER COLUMN C1 INT MASKED (FUNCTION = 'default()'));",
                new ParserErrorInfo(35, "SQL46005", "FILESTREAM", "MASKED"));

            // Change data masking: misspelled MASKED keyword
            //
            ParserTestUtils.ErrorTest130("ALTER TABLE T1 ALTER COLUMN C1 INT MASKD WITH (FUNCTION = 'default()'));",
                new ParserErrorInfo(35, "SQL46005", "FILESTREAM", "MASKD"));

            // Change data masking: missing parenthesis
            //
            ParserTestUtils.ErrorTest130("ALTER TABLE T1 ALTER COLUMN C1 INT MASKED WITH FUNCTION = 'default()');",
                new ParserErrorInfo(47, "SQL46010", "FUNCTION"));

            // Change data masking: missing equals sign
            //
            ParserTestUtils.ErrorTest130("ALTER TABLE T1 ALTER COLUMN C1 INT MASKED WITH (FUNCTION 'default()'));",
                new ParserErrorInfo(57, "SQL46010", "'default()'"));

            // Change data masking: missing string literal single quotes
            //
            ParserTestUtils.ErrorTest130("ALTER TABLE T1 ALTER COLUMN C1 INT MASKED WITH (FUNCTION = default()));",
                new ParserErrorInfo(59, "SQL46010", "default"));

            // Change data masking: missing FUNCTION keyword
            //
            ParserTestUtils.ErrorTest130("ALTER TABLE T1 ALTER COLUMN C1 INT MASKED WITH ('default()'));",
                new ParserErrorInfo(48, "SQL46010", "'default()'"));

            // Change data masking: misspelled WITH keyword
            //
            ParserTestUtils.ErrorTest130("ALTER TABLE T1 ALTER COLUMN C1 INT MASKED WTH (FUNCTION = 'default()'));",
                new ParserErrorInfo(35, "SQL46005", "FILESTREAM", "MASKED"));

            // Change data masking: misspelled FUNCTION keyword
            //
            ParserTestUtils.ErrorTest130("ALTER TABLE T1 ALTER COLUMN C1 INT MASKED WITH (FNCTION = 'default()'));",
                new ParserErrorInfo(48, "SQL46010", "FNCTION"));
        }

        /// <summary>
        /// Negative tests for Data Masking Column Definition syntax.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void DataMaskingColumnDefinitionSyntaxNegativeTest()
        {
            // Missing WITH keyword
            //
            ParserTestUtils.ErrorTest130("CREATE TABLE T1 (C1 INT MASKED (FUNCTION = 'default()'));",
                new ParserErrorInfo(24, "SQL46010", "MASKED"));

            // Mispelled MASKED keyword
            //
            ParserTestUtils.ErrorTest130("CREATE TABLE T1 (C1 INT MASKD WITH (FUNCTION = 'default()'));",
                new ParserErrorInfo(24, "SQL46010", "MASKD"));

            // Missing parenthesis
            //
            ParserTestUtils.ErrorTest130("CREATE TABLE T1 (C1 INT MASKED WITH FUNCTION = 'default()');",
                new ParserErrorInfo(36, "SQL46010", "FUNCTION"));

            // Missing equals sign
            //
            ParserTestUtils.ErrorTest130("CREATE TABLE T1 (C1 INT MASKED WITH (FUNCTION 'default()'));",
                new ParserErrorInfo(46, "SQL46010", "'default()'"));

            // Missing string literal single quotes
            //
            ParserTestUtils.ErrorTest130("CREATE TABLE T1 (C1 INT MASKED WITH (FUNCTION = default()));",
                new ParserErrorInfo(48, "SQL46010", "default"));

            // Missing FUNCTION keyword
            //
            ParserTestUtils.ErrorTest130("CREATE TABLE T1 (C1 INT MASKED WITH ('default()'));",
                new ParserErrorInfo(37, "SQL46010", "'default()'"));

            // Mispelled WITH keyword
            //
            ParserTestUtils.ErrorTest130("CREATE TABLE T1 (C1 INT MASKED WTH (FUNCTION = 'default()'));",
                new ParserErrorInfo(24, "SQL46010", "MASKED"));

            // Misspelled FUNCTION keyword
            //
            ParserTestUtils.ErrorTest130("CREATE TABLE T1 (C1 INT MASKED WITH (FNCTION = 'default()'));",
                new ParserErrorInfo(37, "SQL46010", "FNCTION"));

            // Constraints appear before masking definitions
            //
            ParserTestUtils.ErrorTest130("CREATE TABLE T1 (C1 VARCHAR(32) NOT NULL MASKED WITH (FUNCTION = 'default()'));",
                new ParserErrorInfo(41, "SQL46010", "MASKED"));
        }

        /// <summary>
        /// Only ON and OFF options are allowed for ALTER INDEX REORGANIZE's COMPRESS_ALL_ROW_GROUPS option.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void AlterIndexReorganizeCompressAllRowGroupsOptionNegativeTest()
        {
            ParserTestUtils.ErrorTest130("ALTER INDEX cci ON cciTable REORGANIZE WITH (COMPRESS_ALL_ROW_GROUPS = ONN);",
                 new ParserErrorInfo(71, "SQL46010", "ONN"));

            ParserTestUtils.ErrorTest130("ALTER INDEX cci ON cciTable REORGANIZE WITH (COMPRESS_ALL_ROW_GROUPS = OF);",
                 new ParserErrorInfo(71, "SQL46010", "OF"));

            ParserTestUtils.ErrorTest130("ALTER INDEX cci ON cciTable REORGANIZE WITH (COMPRESS_ALL_ROW_GROUPS = ON;",
                 new ParserErrorInfo(73, "SQL46010", ";"));

            ParserTestUtils.ErrorTest130("ALTER INDEX cci ON cciTable REORGANIZE WITH COMPRESS_ALL_ROW_GROUPS = ON;",
                 new ParserErrorInfo(44, "SQL46010", "COMPRESS_ALL_ROW_GROUPS"));

            ParserTestUtils.ErrorTest130("ALTER INDEX cci ON cciTable REORGANIZE WITH (COMPRESS_ALL_ROW_GROUPS = NULL);",
                 new ParserErrorInfo(71, "SQL46010", "NULL"));

            ParserTestUtils.ErrorTest130("ALTER INDEX cci ON cciTable REORGANIZE WITH (COMPRESS_ALL_ROWGROUPS = ON);",
                 new ParserErrorInfo(45, "SQL46010", "COMPRESS_ALL_ROWGROUPS"));

            ParserTestUtils.ErrorTest130("ALTER INDEX cci ON cciTable REORGANIZE WITH (COMPRESS_ALL_ROW_\nGROUPS = ON);",
                 new ParserErrorInfo(45, "SQL46010", "COMPRESS_ALL_ROW_"));

            // Check that parsers below TSql130 don't recognize COMPRESS_ALL_ROW_GROUPS as a valid option.
            //
            ParserTestUtils.ErrorTest120("ALTER INDEX cci ON cciTable REORGANIZE WITH (COMPRESS_ALL_ROW_GROUPS = ON);",
                 new ParserErrorInfo(45, "SQL46010", "COMPRESS_ALL_ROW_GROUPS"));
        }

        /// <summary>
        /// Only columnstore indexes should accept compression delay on inline index create.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void CreateTableWithInlineIndexWithCompressionDelayNegativeTests()
        {
            ParserTestUtils.ErrorTest130("CREATE TABLE t (col0 INT, INDEX ind (col0) WITH (COMPRESSION_DELAY = 2));",
                 new ParserErrorInfo(49, "SQL46010", "COMPRESSION_DELAY"));
            ParserTestUtils.ErrorTest130("CREATE TABLE t (col0 INT, INDEX ind NONCLUSTERED COLUMNSTORE (col0) WITH (COMPRESSION_DELAY = 2) WHERE col0 > 4);",
                 new ParserErrorInfo(97, "SQL46010", "WHERE"));
        }

        /// <summary>
        /// Only columnstore indexes should accept compression delay on index create.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void CreateIndexWithCompressionDelayNegativeTests()
        {
            ParserTestUtils.ErrorTest130("CREATE INDEX ind ON table1 (col0) WITH (COMPRESSION_DELAY = 2));",
                 new ParserErrorInfo(40, "SQL46010", "COMPRESSION_DELAY"));
        }

        /// <summary>
        /// ALTER INDEX [indexName] ON [tableName] SET (COMPRESSION_DELAY = value MINUTES) accepts AUTO and numbers in [0, maxint].
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void AlterIndexSetCompressionDelayOptionNegativeTest()
        {
            ParserTestUtils.ErrorTest130("ALTER INDEX cci ON cciTable SET(COMPRESSION_DELAY = ON);",
                 new ParserErrorInfo(52, "SQL46010", "ON"));

            ParserTestUtils.ErrorTest130("ALTER INDEX cci ON cciTable SET(COMPRESSION_DELAY = -1);",
                 new ParserErrorInfo(52, "SQL46010", "-"));

            ParserTestUtils.ErrorTest130("ALTER INDEX cci ON cciTable SET(COMPRESSION_DELAY = AUTO);",
                 new ParserErrorInfo(52, "SQL46010", "AUTO"));

            ParserTestUtils.ErrorTest130("ALTER INDEX cci ON cciTable SET(COMPRESSION_DELAY = 10081);",
                 new ParserErrorInfo(52, "SQL46114", "10081"));

            ParserTestUtils.ErrorTest130("ALTER INDEX cci ON cciTable SET(COMPRESSION_DELAY = 2;",
                 new ParserErrorInfo(53, "SQL46010", ";"));

            ParserTestUtils.ErrorTest130("ALTER INDEX cci ON cciTable SET COMPRESSION_DELAY = 2;",
                 new ParserErrorInfo(32, "SQL46010", "COMPRESSION_DELAY"));

            ParserTestUtils.ErrorTest130("ALTER INDEX cci ON cciTable SET (COMPRESSION_DELAY = NULL);",
                 new ParserErrorInfo(53, "SQL46010", "NULL"));

            ParserTestUtils.ErrorTest130("ALTER INDEX cci ON cciTable REORGANIZE WITH (COMPRESSIONDELAY = 2);",
                 new ParserErrorInfo(45, "SQL46010", "COMPRESSIONDELAY"));

            ParserTestUtils.ErrorTest130("ALTER INDEX cci ON cciTable SET (COMPRESSION_\nDELAY = 2);",
                 new ParserErrorInfo(33, "SQL46010", "COMPRESSION_"));

            // Check that parsers below TSql130 don't recognize COMPRESSION_DELAY as a valid option.
            //
            ParserTestUtils.ErrorTest120("ALTER INDEX cci ON cciTable SET (COMPRESSION_DELAY = 2);",
                 new ParserErrorInfo(33, "SQL46010", "COMPRESSION_DELAY"));
        }


        /// <summary>
        /// Within a create statement, SUPPRESS_MESSAGES can only be set when IGNORE_DUP_KEY is ON, and it must be a suboption of IGNORE_DUP_KEY.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void CreateTableWithInlineIndexWithSuppressMessagesNegativeTests()
        {
            ParserTestUtils.ErrorTest140("CREATE UNIQUE NONCLUSTERED INDEX nci ON table1(col1) WITH (SUPPRESS_MESSAGES = ON);",
                 new ParserErrorInfo(59, "SQL46010", "SUPPRESS_MESSAGES"));

            ParserTestUtils.ErrorTest140("CREATE UNIQUE NONCLUSTERED INDEX nci ON table1(col1) WITH (SUPPRESS_MESSAGES = OFF);",
                 new ParserErrorInfo(59, "SQL46010", "SUPPRESS_MESSAGES"));

            ParserTestUtils.ErrorTest140("CREATE UNIQUE NONCLUSTERED INDEX nci ON table1(col1) WITH (IGNORE_DUP_KEY = OFF (SUPPRESS_MESSAGES = ON));",
                 new ParserErrorInfo(80, "SQL46010", "("));

            ParserTestUtils.ErrorTest140("CREATE UNIQUE NONCLUSTERED INDEX nci ON table1(col1) WITH (IGNORE_DUP_KEY = OFF (SUPPRESS_MESSAGES = OFF));",
                 new ParserErrorInfo(80, "SQL46010", "("));

            ParserTestUtils.ErrorTest140("CREATE UNIQUE NONCLUSTERED INDEX nci ON table1(col1) WITH (IGNORE_DUP_KEY = ON, SUPPRESS_MESSAGES = ON));",
                 new ParserErrorInfo(80, "SQL46010", "SUPPRESS_MESSAGES"));

            ParserTestUtils.ErrorTest140("CREATE UNIQUE NONCLUSTERED INDEX nci ON table1(col1) WITH (IGNORE_DUP_KEY = ON, SUPPRESS_MESSAGES = OFF));",
                 new ParserErrorInfo(80, "SQL46010", "SUPPRESS_MESSAGES"));

        }

        /// <summary>
        /// Within an ALTER statement, SUPPRESS_MESSAGES is a suboption of IGNORE_DUP_KEY only when IGNORE_DUP_KEY is ON.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void AlterIndexSetSuppressMessagesOptionNegativeTest()
        {
            ParserTestUtils.ErrorTest140("ALTER INDEX ci ON ciTable SET (SUPPRESS_MESSAGES = ON);",
                 new ParserErrorInfo(31, "SQL46010", "SUPPRESS_MESSAGES"));

            ParserTestUtils.ErrorTest140("ALTER INDEX ci ON ciTable SET (SUPPRESS_MESSAGES = OFF);",
                 new ParserErrorInfo(31, "SQL46010", "SUPPRESS_MESSAGES"));

            ParserTestUtils.ErrorTest140("ALTER INDEX ci ON ciTable SET (IGNORE_DUP_KEY = OFF (SUPPRESS_MESSAGES = ON));",
                 new ParserErrorInfo(52, "SQL46010", "("));

            ParserTestUtils.ErrorTest140("ALTER INDEX ci ON ciTable SET (IGNORE_DUP_KEY = OFF (SUPPRESS_MESSAGES = OFF));",
                 new ParserErrorInfo(52, "SQL46010", "("));

            ParserTestUtils.ErrorTest140("ALTER INDEX ci ON ciTable SET (IGNORE_DUP_KEY = ON, SUPPRESS_MESSAGES = ON);",
                 new ParserErrorInfo(52, "SQL46010", "SUPPRESS_MESSAGES"));

            ParserTestUtils.ErrorTest140("ALTER INDEX ci ON ciTable SET (IGNORE_DUP_KEY = ON, SUPPRESS_MESSAGES = OFF);",
                 new ParserErrorInfo(52, "SQL46010", "SUPPRESS_MESSAGES"));

            // Check that parsers below 140 don't recognize SUPPRESS_MESSAGES as valid.
            //
            ParserTestUtils.ErrorTest130("ALTER INDEX ci ON ciTable SET (IGNORE_DUP_KEY = ON (SUPPRESS_MESSAGES = ON));",
                new ParserErrorInfo(51, "SQL46010", "("));
        }

        /// <summary>
        /// Column level and table level filtered indexes on hekaton tables are not allowed.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void HekatonTableInlineFilteredIndex()
        {
            ParserTestUtils.ErrorTest130("CREATE TABLE T1 (C1 INT NOT NULL PRIMARY KEY NONCLUSTERED, C2 INT NOT NULL INDEX idx NONCLUSTERED WHERE C2 > 10) WITH (MEMORY_OPTIMIZED=on);",
                new ParserErrorInfo(17, "SQL46107"));

            ParserTestUtils.ErrorTest130("CREATE TABLE T1 (C1 INT NOT NULL PRIMARY KEY NONCLUSTERED, C2 INT NOT NULL, INDEX idx NONCLUSTERED (C2) WHERE C2 > 10) WITH (MEMORY_OPTIMIZED=on);",
                new ParserErrorInfo(17, "SQL46107"));
        }

        /// <summary>
        /// Nonclustered columnstore indexes are not allowed on hekaton tables.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void HekatonTableNonClusteredColumnStoreIndex()
        {
            ParserTestUtils.ErrorTest130("CREATE TABLE T1 (C1 INT NOT NULL PRIMARY KEY NONCLUSTERED, C2 INT NOT NULL, INDEX idx NONCLUSTERED COLUMNSTORE (C2)) WITH (MEMORY_OPTIMIZED=ON);",
                new ParserErrorInfo(17, "SQL46108"));
        }

        /// <summary>
        /// Negative tests for FOR JSON syntax.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void JsonForClauseNegativeTest()
        {
            ParserTestUtils.ErrorTest130("select * from t1 for json",
                new ParserErrorInfo(21, "SQL46010", "json"));

            ParserTestUtils.ErrorTest130("select * from t1 for json xml",
                new ParserErrorInfo(26, "SQL46010", "xml"));

            ParserTestUtils.ErrorTest130("select * from t1 for json for xml",
                new ParserErrorInfo(21, "SQL46010", "json"));

            ParserTestUtils.ErrorTest130("select * from t1 for json root",
                new ParserErrorInfo(26, "SQL46010", "root"));

            ParserTestUtils.ErrorTest130("select * from t1 for json auto include_null_values",
                new ParserErrorInfo(31, "SQL46010", "include_null_values"));

            ParserTestUtils.ErrorTest130("select * from t1 for json path, root, root",
                new ParserErrorInfo(38, "SQL46010", "root"));

            ParserTestUtils.ErrorTest130("select * from t1 for json path, root, without_array_wrapper",
                new ParserErrorInfo(38, "SQL46010", "without_array_wrapper"));

            ParserTestUtils.ErrorTest130("select 1 as a for json path, without_array_wrapper, root",
                new ParserErrorInfo(52, "SQL46010", "root"));

            ParserTestUtils.ErrorTest130("select 1 as a for json path, without_array_wrapper('Root')",
                new ParserErrorInfo(50, "SQL46010", "("));

            ParserTestUtils.ErrorTest130("select 1 as a for json without_array_wrapper",
                new ParserErrorInfo(23, "SQL46010", "without_array_wrapper"));

            ParserTestUtils.ErrorTest130("select * from t where t.c1 = t.c2 for json auto, without_array_wrapper, without_array_wrapper",
                new ParserErrorInfo(72, "SQL46010", "without_array_wrapper"));

            ParserTestUtils.ErrorTest130("select 1 as a for xml path, without_array_wrapper",
                new ParserErrorInfo(28, "SQL46010", "without_array_wrapper"));

            ParserTestUtils.ErrorTest130("select * from t for xml auto, include_null_values",
                new ParserErrorInfo(30, "SQL46010", "include_null_values"));
        }

        /// <summary>
        /// Negative tests for Temporal syntax.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TemporalSyntaxNegativeTest()
        {
            // Issuing select statement using temporal clause and an expression as source.
            //
            ParserTestUtils.ErrorTest130("SELECT * FROM (SELECT * FROM T1 UNION ALL SELECT * FROM T2) FOR SYSTEM_TIME AS OF '01/02/03' AS X", new ParserErrorInfo(60, "SQL46010", "FOR"));
            ParserTestUtils.ErrorTest130("SELECT * FROM dbo.my_function(1, 2, 3) FOR SYSTEM_TIME AS OF '01/02/03'", new ParserErrorInfo(43, "SQL46010", "SYSTEM_TIME"));

            // Issuing create table statement with wrongly specified retention period.
            //
            string query1 = @"CREATE TABLE tab_with_retention (
    COL0      INT                                        ,
    COL1      XML                                        ,
    COL2      FLOAT                                      ,
    COL3      NVARCHAR (64)                              ,
    SYS_START DATETIME2 (7) GENERATED ALWAYS AS ROW START NOT NULL,
    SYS_END   DATETIME2 (7) GENERATED ALWAYS AS ROW END   NOT NULL,
    CONSTRAINT PK_MY_PRIMARY_KEY PRIMARY KEY CLUSTERED (COL0),
    PERIOD FOR SYSTEM_TIME (SYS_START, SYS_END)
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = dbo.t_history, DATA_CONSISTENCY_CHECK = ON, HISTORY_RETENTION_PERIOD = NOINFINITE))";
            ParserTestUtils.ErrorTest140(query1,
                                        new ParserErrorInfo(query1.IndexOf("NOINFINITE"), "SQL46005", "INFINITE", "NOINFINITE"));

            string query2 = @"CREATE TABLE tab_with_retention (
    COL0      INT                                        ,
    COL1      XML                                        ,
    COL2      FLOAT                                      ,
    COL3      NVARCHAR (64)                              ,
    SYS_START DATETIME2 (7) GENERATED ALWAYS AS ROW START NOT NULL,
    SYS_END   DATETIME2 (7) GENERATED ALWAYS AS ROW END   NOT NULL,
    CONSTRAINT PK_MY_PRIMARY_KEY PRIMARY KEY CLUSTERED (COL0),
    PERIOD FOR SYSTEM_TIME (SYS_START, SYS_END)
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = dbo.t_history, DATA_CONSISTENCY_CHECK = ON, HISTORY_RETENTION_PERIOD = 5 INVALIDUNIT))";
            ParserTestUtils.ErrorTest140(query2,
                                        new ParserErrorInfo(query2.IndexOf("INVALIDUNIT"), "SQL46010", "INVALIDUNIT"));

            string query3 = @"CREATE TABLE tab_with_retention (
    COL0      INT                                        ,
    COL1      XML                                        ,
    COL2      FLOAT                                      ,
    COL3      NVARCHAR (64)                              ,
    SYS_START DATETIME2 (7) GENERATED ALWAYS AS ROW START NOT NULL,
    SYS_END   DATETIME2 (7) GENERATED ALWAYS AS ROW END   NOT NULL,
    CONSTRAINT PK_MY_PRIMARY_KEY PRIMARY KEY CLUSTERED (COL0),
    PERIOD FOR SYSTEM_TIME (SYS_START, SYS_END)
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = dbo.t_history, DATA_CONSISTENCY_CHECK = ON, HISTORY_RETENTION_PERIOD = NOTINTEGER DAYS))";
            ParserTestUtils.ErrorTest140(query3,
                                        new ParserErrorInfo(query3.IndexOf("NOTINTEGER"), "SQL46005", "INFINITE", "NOTINTEGER"));

            string query4 = @"CREATE TABLE tab_with_retention (
    COL0      INT                                        ,
    COL1      XML                                        ,
    COL2      FLOAT                                      ,
    COL3      NVARCHAR (64)                              ,
    SYS_START DATETIME2 (7) GENERATED ALWAYS AS ROW START NOT NULL,
    SYS_END   DATETIME2 (7) GENERATED ALWAYS AS ROW END   NOT NULL,
    CONSTRAINT PK_MY_PRIMARY_KEY PRIMARY KEY CLUSTERED (COL0),
    PERIOD FOR SYSTEM_TIME (SYS_START, SYS_END)
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = dbo.t_history, DATA_CONSISTENCY_CHECK = ON, , HISTORY_RETENTION_PERIOD = 5 DAYS))";
            ParserTestUtils.ErrorTest140(query4,
                                        new ParserErrorInfo(query4.IndexOf(", ,") + 2, "SQL46010", ","));

            string query5 = @"CREATE TABLE tab_with_retention (
    COL0      INT                                        ,
    COL1      XML                                        ,
    COL2      FLOAT                                      ,
    COL3      NVARCHAR (64)                              ,
    SYS_START DATETIME2 (7) GENERATED ALWAYS AS ROW START NOT NULL,
    SYS_END   DATETIME2 (7) GENERATED ALWAYS AS ROW END   NOT NULL,
    CONSTRAINT PK_MY_PRIMARY_KEY PRIMARY KEY CLUSTERED (COL0),
    PERIOD FOR SYSTEM_TIME (SYS_START, SYS_END)
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = dbo.t_history, DATA_CONSISTENCY_CHECK = ON, EXTRA_IDENTIFIER HISTORY_RETENTION_PERIOD = 5 DAYS))";
            ParserTestUtils.ErrorTest140(query5,
                                        new ParserErrorInfo(query5.IndexOf("EXTRA_IDENTIFIER"), "SQL46010", "EXTRA_IDENTIFIER"));

            string query6 = @"CREATE TABLE tab_with_retention (
    COL0      INT                                        ,
    COL1      XML                                        ,
    COL2      FLOAT                                      ,
    COL3      NVARCHAR (64)                              ,
    SYS_START DATETIME2 (7) GENERATED ALWAYS AS ROW START NOT NULL,
    SYS_END   DATETIME2 (7) GENERATED ALWAYS AS ROW END   NOT NULL,
    CONSTRAINT PK_MY_PRIMARY_KEY PRIMARY KEY CLUSTERED (COL0),
    PERIOD FOR SYSTEM_TIME (SYS_START, SYS_END)
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = dbo.t_history, DATA_CONSISTENCY_CHECK = ON, HISTORY_RETENTION_PERIOD EXTRA_IDENTIFIER = 5 DAYS))";
            ParserTestUtils.ErrorTest140(query6,
                                        new ParserErrorInfo(query6.IndexOf("HISTORY_RETENTION_PERIOD"), "SQL46010", "HISTORY_RETENTION_PERIOD"));

            string query7 = @"CREATE TABLE tab_with_retention (
    COL0      INT                                        ,
    COL1      XML                                        ,
    COL2      FLOAT                                      ,
    COL3      NVARCHAR (64)                              ,
    SYS_START DATETIME2 (7) GENERATED ALWAYS AS ROW START NOT NULL,
    SYS_END   DATETIME2 (7) GENERATED ALWAYS AS ROW END   NOT NULL,
    CONSTRAINT PK_MY_PRIMARY_KEY PRIMARY KEY CLUSTERED (COL0),
    PERIOD FOR SYSTEM_TIME (SYS_START, SYS_END)
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = dbo.t_history, DATA_CONSISTENCY_CHECK = ON, HISTORY_RETENTION_PERIOD <> 5 DAYS))";
            ParserTestUtils.ErrorTest140(query7,
                                        new ParserErrorInfo(query7.IndexOf("HISTORY_RETENTION_PERIOD"), "SQL46010", "HISTORY_RETENTION_PERIOD"));

            string query8 = @"CREATE TABLE tab_with_retention (
    COL0      INT                                        ,
    COL1      XML                                        ,
    COL2      FLOAT                                      ,
    COL3      NVARCHAR (64)                              ,
    SYS_START DATETIME2 (7) GENERATED ALWAYS AS ROW START NOT NULL,
    SYS_END   DATETIME2 (7) GENERATED ALWAYS AS ROW END   NOT NULL,
    CONSTRAINT PK_MY_PRIMARY_KEY PRIMARY KEY CLUSTERED (COL0),
    PERIOD FOR SYSTEM_TIME (SYS_START, SYS_END)
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = dbo.t_history, DATA_CONSISTENCY_CHECK = ON, HISTORY_RETENTION_PERIOD = 9000000000 DAYS))";
            ParserTestUtils.ErrorTest140(query8,
                                        new ParserErrorInfo(query8.IndexOf("9000000000"), "SQL46010", "9000000000"));

            string query9 = @"CREATE TABLE tab_with_retention (
    COL0      INT                                        ,
    COL1      XML                                        ,
    COL2      FLOAT                                      ,
    COL3      NVARCHAR (64)                              ,
    SYS_START DATETIME2 (7) GENERATED ALWAYS AS ROW START NOT NULL,
    SYS_END   DATETIME2 (7) GENERATED ALWAYS AS ROW END   NOT NULL,
    CONSTRAINT PK_MY_PRIMARY_KEY PRIMARY KEY CLUSTERED (COL0),
    PERIOD FOR SYSTEM_TIME (SYS_START, SYS_END)
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = dbo.t_history, DATA_CONSISTENCY_CHECK = ON, HISTORY_RETENTION_PERIOD = @x DAYS))";
            ParserTestUtils.ErrorTest140(query9,
                                        new ParserErrorInfo(query9.IndexOf("@x"), "SQL46010", "@x"));

            string query10 = @"CREATE TABLE tab_with_retention (
    COL0      INT                                        ,
    COL1      XML                                        ,
    COL2      FLOAT                                      ,
    COL3      NVARCHAR (64)                              ,
    SYS_START DATETIME2 (7) GENERATED ALWAYS AS ROW START NOT NULL,
    SYS_END   DATETIME2 (7) GENERATED ALWAYS AS ROW END   NOT NULL,
    CONSTRAINT PK_MY_PRIMARY_KEY PRIMARY KEY CLUSTERED (COL0),
    PERIOD FOR SYSTEM_TIME (SYS_START, SYS_END)
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = dbo.t_history, DATA_CONSISTENCY_CHECK = ON, HISTORY_RETENTION_PERIOD = '5' DAYS))";
            ParserTestUtils.ErrorTest140(query10,
                                        new ParserErrorInfo(query10.IndexOf("'5'"), "SQL46010", "'5'"));

            string query11 = @"CREATE TABLE tab_with_retention (
    COL0      INT                                        ,
    COL1      XML                                        ,
    COL2      FLOAT                                      ,
    COL3      NVARCHAR (64)                              ,
    SYS_START DATETIME2 (7) GENERATED ALWAYS AS ROW START NOT NULL,
    SYS_END   DATETIME2 (7) GENERATED ALWAYS AS ROW END   NOT NULL,
    CONSTRAINT PK_MY_PRIMARY_KEY PRIMARY KEY CLUSTERED (COL0),
    PERIOD FOR SYSTEM_TIME (SYS_START, SYS_END)
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = dbo.t_history, DATA_CONSISTENCY_CHECK = ON, HISTORY_RETENTION_PERIOD = 10 'DAYS'))";
            ParserTestUtils.ErrorTest140(query11,
                                        new ParserErrorInfo(query11.IndexOf("'DAYS'"), "SQL46010", "'DAYS'"));

            string query12 = @"CREATE TABLE tab_with_retention (
    COL0      INT                                        ,
    COL1      XML                                        ,
    COL2      FLOAT                                      ,
    COL3      NVARCHAR (64)                              ,
    SYS_START DATETIME2 (7) GENERATED ALWAYS AS ROW START NOT NULL,
    SYS_END   DATETIME2 (7) GENERATED ALWAYS AS ROW END   NOT NULL,
    CONSTRAINT PK_MY_PRIMARY_KEY PRIMARY KEY CLUSTERED (COL0),
    PERIOD FOR SYSTEM_TIME (SYS_START, SYS_END)
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = dbo.t_history, DATA_CONSISTENCY_CHECK = ON, HISTORY_RETENTION_PERIOD = [COL0] DAYS))";
            ParserTestUtils.ErrorTest140(query12,
                                        new ParserErrorInfo(query12.IndexOf("[COL0]"), "SQL46010", "[COL0]"));

            string query13 = @"CREATE TABLE tab_with_retention (
    COL0      INT                                        ,
    COL1      XML                                        ,
    COL2      FLOAT                                      ,
    COL3      NVARCHAR (64)                              ,
    SYS_START DATETIME2 (7) GENERATED ALWAYS AS ROW START NOT NULL,
    SYS_END   DATETIME2 (7) GENERATED ALWAYS AS ROW END   NOT NULL,
    CONSTRAINT PK_MY_PRIMARY_KEY PRIMARY KEY CLUSTERED (COL0),
    PERIOD FOR SYSTEM_TIME (SYS_START, SYS_END)
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = dbo.t_history, DATA_CONSISTENCY_CHECK = ON, HISTORY_RETENTION_PERIOD = 2+3 WEEKS))";
            ParserTestUtils.ErrorTest140(query13,
                                        new ParserErrorInfo(query13.IndexOf("+"), "SQL46010", "+"));

            string query14 = @"CREATE TABLE tab_with_retention (
    COL0      INT                                        ,
    COL1      XML                                        ,
    COL2      FLOAT                                      ,
    COL3      NVARCHAR (64)                              ,
    SYS_START DATETIME2 (7) GENERATED ALWAYS AS ROW START NOT NULL,
    SYS_END   DATETIME2 (7) GENERATED ALWAYS AS ROW END   NOT NULL,
    CONSTRAINT PK_MY_PRIMARY_KEY PRIMARY KEY CLUSTERED (COL0),
    PERIOD FOR SYSTEM_TIME (SYS_START, SYS_END)
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = dbo.t_history, DATA_CONSISTENCY_CHECK = ON, HISTORY_RETENTION_PERIOD = f(10) DAYS))";
            ParserTestUtils.ErrorTest140(query14,
                                        new ParserErrorInfo(query14.IndexOf("f(10)"), "SQL46005", CodeGenerationSupporter.Infinite, "f"));

            // Issuing create table statement with invalid    suboptions.
            //
            string query15 = @"CREATE TABLE tab (
    COL0      INT                                        ,
    COL1      XML                                        ,
    COL2      FLOAT                                      ,
    COL3      NVARCHAR (64)                              ,
    SYS_START DATETIME2 (7) GENERATED ALWAYS AS ROW START NOT NULL,
    SYS_END   DATETIME2 (7) GENERATED ALWAYS AS ROW END   NOT NULL,
    CONSTRAINT PK_MY_PRIMARY_KEY PRIMARY KEY CLUSTERED (COL0),
    PERIOD FOR SYSTEM_TIME (SYS_START, SYS_END)
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = dbo.t_history, HISTORY_TABLE = dbo.t_history_2))";
            ParserTestUtils.ErrorTest130(query15,
                                        new ParserErrorInfo(query15.IndexOf("HISTORY_TABLE", 550), "SQL46005", "DATA_CONSISTENCY_CHECK", "HISTORY_TABLE"));

            string query16 = @"CREATE TABLE tab (
    COL0      INT                                        ,
    COL1      XML                                        ,
    COL2      FLOAT                                      ,
    COL3      NVARCHAR (64)                              ,
    SYS_START DATETIME2 (7) GENERATED ALWAYS AS ROW START NOT NULL,
    SYS_END   DATETIME2 (7) GENERATED ALWAYS AS ROW END   NOT NULL,
    CONSTRAINT PK_MY_PRIMARY_KEY PRIMARY KEY CLUSTERED (COL0),
    PERIOD FOR SYSTEM_TIME (SYS_START, SYS_END)
)
WITH (SYSTEM_VERSIONING = ON (DATA_CONSISTENCY_CHECK = ON, DATA_CONSISTENCY_CHECK = OFF))";
            ParserTestUtils.ErrorTest130(query16,
                                        new ParserErrorInfo(query16.IndexOf("DATA_CONSISTENCY_CHECK"), "SQL46005", "HISTORY_TABLE", "DATA_CONSISTENCY_CHECK"));

            string query17 = @"CREATE TABLE tab (
    COL0      INT                                        ,
    COL1      XML                                        ,
    COL2      FLOAT                                      ,
    COL3      NVARCHAR (64)                              ,
    SYS_START DATETIME2 (7) GENERATED ALWAYS AS ROW START NOT NULL,
    SYS_END   DATETIME2 (7) GENERATED ALWAYS AS ROW END   NOT NULL,
    CONSTRAINT PK_MY_PRIMARY_KEY PRIMARY KEY CLUSTERED (COL0),
    PERIOD FOR SYSTEM_TIME (SYS_START, SYS_END)
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_RETENTION_PERIOD = 5 DAYS, HISTORY_RETENTION_PERIOD = 5 DAYS))";
            ParserTestUtils.ErrorTest140(query17,
                                        new ParserErrorInfo(query17.IndexOf("SYSTEM_VERSIONING"), "SQL46010", "HISTORY_RETENTION_PERIOD"));

            string query18 = @"CREATE TABLE tab (
    COL0      INT                                        ,
    COL1      XML                                        ,
    COL2      FLOAT                                      ,
    COL3      NVARCHAR (64)                              ,
    SYS_START DATETIME2 (7) GENERATED ALWAYS AS ROW START NOT NULL,
    SYS_END   DATETIME2 (7) GENERATED ALWAYS AS ROW END   NOT NULL,
    CONSTRAINT PK_MY_PRIMARY_KEY PRIMARY KEY CLUSTERED (COL0),
    PERIOD FOR SYSTEM_TIME (SYS_START, SYS_END)
)
WITH (SYSTEM_VERSIONING = ON ())";
            ParserTestUtils.ErrorTest130(query18,
                                        new ParserErrorInfo(query18.IndexOf("())") + 1, "SQL46010", ")"));

            string query19 = @"CREATE TABLE tab_with_retention (
    COL0      INT                                        ,
    COL1      XML                                        ,
    COL2      FLOAT                                      ,
    COL3      NVARCHAR (64)                              ,
    SYS_START DATETIME2 (7) GENERATED ALWAYS AS ROW START NOT NULL,
    SYS_END   DATETIME2 (7) GENERATED ALWAYS AS ROW END   NOT NULL,
    CONSTRAINT PK_MY_PRIMARY_KEY PRIMARY KEY CLUSTERED (COL0),
    PERIOD FOR SYSTEM_TIME (SYS_START, SYS_END)
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = dbo.t_history, DATA_CONSISTENCY_CHECK = ON, HISTORY_RETENTION_PERIOD = INFINITE DAYS))";
            ParserTestUtils.ErrorTest140(query19,
                                        new ParserErrorInfo(query19.IndexOf("DAYS"), "SQL46010", "DAYS"));

            string query20 = @"CREATE TABLE tab_with_retention (
    COL0      INT                                        ,
    COL1      XML                                        ,
    COL2      FLOAT                                      ,
    COL3      NVARCHAR (64)                              ,
    SYS_START DATETIME2 (7) GENERATED ALWAYS AS ROW START NOT NULL,
    SYS_END   DATETIME2 (7) GENERATED ALWAYS AS ROW END   NOT NULL,
    CONSTRAINT PK_MY_PRIMARY_KEY PRIMARY KEY CLUSTERED (COL0),
    PERIOD FOR SYSTEM_TIME (SYS_START, SYS_END)
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = dbo.t_history, DATA_CONSISTENCY_CHECK = ON, HISTORY_RETENTION_PERIOD = -5 DAYS))";
            ParserTestUtils.ErrorTest130(query20,
                                        new ParserErrorInfo(query20.IndexOf(", HISTORY_RETENTION_PERIO"), "SQL46010", ","));

            string query21 = @"CREATE TABLE tab_with_retention (
    COL0      INT                                        ,
    COL1      XML                                        ,
    COL2      FLOAT                                      ,
    COL3      NVARCHAR (64)                              ,
    SYS_START DATETIME2 (7) GENERATED ALWAYS AS ROW START NOT NULL,
    SYS_END   DATETIME2 (7) GENERATED ALWAYS AS ROW END   NOT NULL,
    CONSTRAINT PK_MY_PRIMARY_KEY PRIMARY KEY CLUSTERED (COL0),
    PERIOD FOR SYSTEM_TIME (SYS_START, SYS_END)
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = dbo.t_history, DATA_CONSISTENCY_CHECK = ON, HISTORY_RETENTION_PERIOD = 0 DAYS))";
            ParserTestUtils.ErrorTest140(query21,
                                        new ParserErrorInfo(query21.IndexOf("0 DAYS"), "SQL46116", "0"));
        }

        /// <summary>
        /// Negative tests for inline COLUMNSTORE indexes
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void ColumnStoreInlineIndexNegativeTest()
        {
            // Columns in CCI
            //
            ParserTestUtils.ErrorTest130("create table t_cci(c int, index cci clustered columnstore(c))", new ParserErrorInfo(46, "SQL46010", "columnstore"));

            // No columns in CI/NCI
            //
            ParserTestUtils.ErrorTest130("create table t_ci(c int, index idx clustered)", new ParserErrorInfo(25, "SQL46010", "index"));
            ParserTestUtils.ErrorTest130("create table t_nci(c int, index idx nonclustered)", new ParserErrorInfo(26, "SQL46010", "index"));

            // No columns in NCCI
            //
            ParserTestUtils.ErrorTest130("create table t_cci(c int, index cci nonclustered columnstore)", new ParserErrorInfo(49, "SQL46010", "columnstore"));

            // Inline column syntax
            //
            ParserTestUtils.ErrorTest130("create table t_ncci_inline_column(c int index ncci nonclustered columnstore(c))", new ParserErrorInfo(64, "SQL46010", "columnstore"));
            ParserTestUtils.ErrorTest130("create table t_cci(c int index cci clustered columnstore)", new ParserErrorInfo(45, "SQL46010", "columnstore"));

            // Primary key
            //
            ParserTestUtils.ErrorTest130("create table t_ncci(c int, constraint ncci primary key clustered columnstore (c))", new ParserErrorInfo(65, "SQL46010", "columnstore"));

            // Unique constraint
            //
            ParserTestUtils.ErrorTest130("create table t_ncci(c int, constraint ncci unique clustered columnstore)", new ParserErrorInfo(60, "SQL46010", "columnstore"));

            // Filtered CCI
            //
            ParserTestUtils.ErrorTest130("create table t_cci(c int, index cci clustered columnstore where c > 1)", new ParserErrorInfo(58, "SQL46010", "where"));
        }

        /// <summary>
        /// Negative tests for DROP IF EXISTS statements
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void DropIfExistsNegativeTest()
        {
            // DROP DATABASE
            //
            ParserTestUtils.ErrorTest130("DROP IF EXISTS DATABASE dbX", new ParserErrorInfo(5, "SQL46010", "IF"));
            ParserTestUtils.ErrorTest130("DROP IF DATABASE EXISTS dbX", new ParserErrorInfo(5, "SQL46010", "IF"));
            ParserTestUtils.ErrorTest130("DROP DATABASE dbX IF EXISTS", new ParserErrorInfo(27, "SQL46029", ""));
            ParserTestUtils.ErrorTest130("DROP DATABASE IF EXISTS", new ParserErrorInfo(23, "SQL46029", ""));
            ParserTestUtils.ErrorTest130("DROP DATABASE IF EXIST dbX", new ParserErrorInfo(17, "SQL46010", "EXIST"));

            // DROP SCHEMA
            //
            ParserTestUtils.ErrorTest130("DROP IF EXISTS SCHEMA schemaX", new ParserErrorInfo(5, "SQL46010", "IF"));
            ParserTestUtils.ErrorTest130("DROP IF SCHEMA EXISTS schemaX", new ParserErrorInfo(5, "SQL46010", "IF"));
            ParserTestUtils.ErrorTest130("DROP SCHEMA schemaX IF EXISTS", new ParserErrorInfo(29, "SQL46029", ""));
            ParserTestUtils.ErrorTest130("DROP SCHEMA IF EXISTS", new ParserErrorInfo(21, "SQL46029", ""));
            ParserTestUtils.ErrorTest130("DROP SCHEMA IF EXIST schemaX", new ParserErrorInfo(15, "SQL46010", "EXIST"));

            // DROP TYPE
            //
            ParserTestUtils.ErrorTest130("DROP IF EXISTS TYPE typeX", new ParserErrorInfo(5, "SQL46010", "IF"));
            ParserTestUtils.ErrorTest130("DROP IF TYPE EXISTS typeX", new ParserErrorInfo(5, "SQL46010", "IF"));
            ParserTestUtils.ErrorTest130("DROP TYPE typeX IF EXISTS", new ParserErrorInfo(25, "SQL46029", ""));
            ParserTestUtils.ErrorTest130("DROP TYPE IF EXISTS", new ParserErrorInfo(19, "SQL46029", ""));
            ParserTestUtils.ErrorTest130("DROP TYPE IF EXIST typeX", new ParserErrorInfo(13, "SQL46010", "EXIST"));

            // DROP INDEX
            //
            ParserTestUtils.ErrorTest130("DROP IF EXISTS INDEX idX ON tableX", new ParserErrorInfo(5, "SQL46010", "IF"));
            ParserTestUtils.ErrorTest130("DROP IF INDEX EXISTS idX ON tableX", new ParserErrorInfo(5, "SQL46010", "IF"));
            ParserTestUtils.ErrorTest130("DROP INDEX idX IF EXISTS ON tableX", new ParserErrorInfo(11, "SQL46027", ""));
            ParserTestUtils.ErrorTest130("DROP INDEX IF EXISTS ON tableX", new ParserErrorInfo(21, "SQL46010", "ON"));
            ParserTestUtils.ErrorTest130("DROP INDEX IF EXIST idX ON tableX", new ParserErrorInfo(14, "SQL46010", "EXIST"));

            // DROP TABLE
            //
            ParserTestUtils.ErrorTest130("DROP IF EXISTS TABLE tableX", new ParserErrorInfo(5, "SQL46010", "IF"));
            ParserTestUtils.ErrorTest130("DROP IF TABLE EXISTS tableX", new ParserErrorInfo(5, "SQL46010", "IF"));
            ParserTestUtils.ErrorTest130("DROP TABLE tableX IF EXISTS", new ParserErrorInfo(27, "SQL46029", ""));
            ParserTestUtils.ErrorTest130("DROP TABLE IF EXISTS", new ParserErrorInfo(20, "SQL46029", ""));
            ParserTestUtils.ErrorTest130("DROP TABLE IF EXIST tableX", new ParserErrorInfo(14, "SQL46010", "EXIST"));

            // DROP VIEW
            //
            ParserTestUtils.ErrorTest130("DROP IF EXISTS VIEW viewX", new ParserErrorInfo(5, "SQL46010", "IF"));
            ParserTestUtils.ErrorTest130("DROP IF VIEW EXISTS viewX", new ParserErrorInfo(5, "SQL46010", "IF"));
            ParserTestUtils.ErrorTest130("DROP VIEW viewX IF EXISTS", new ParserErrorInfo(25, "SQL46029", ""));
            ParserTestUtils.ErrorTest130("DROP VIEW IF EXISTS", new ParserErrorInfo(19, "SQL46029", ""));
            ParserTestUtils.ErrorTest130("DROP VIEW IF EXIST viewX", new ParserErrorInfo(13, "SQL46010", "EXIST"));

            // DROP FUNCTION
            //
            ParserTestUtils.ErrorTest130("DROP IF EXISTS FUNCTION funcX", new ParserErrorInfo(5, "SQL46010", "IF"));
            ParserTestUtils.ErrorTest130("DROP IF FUNCTION EXISTS funcX", new ParserErrorInfo(5, "SQL46010", "IF"));
            ParserTestUtils.ErrorTest130("DROP FUNCTION funcX IF EXISTS", new ParserErrorInfo(29, "SQL46029", ""));
            ParserTestUtils.ErrorTest130("DROP FUNCTION IF EXISTS", new ParserErrorInfo(23, "SQL46029", ""));
            ParserTestUtils.ErrorTest130("DROP FUNCTION IF EXIST funcX", new ParserErrorInfo(17, "SQL46010", "EXIST"));

            // DROP PROCEDURE
            //
            ParserTestUtils.ErrorTest130("DROP IF EXISTS PROCEDURE procX", new ParserErrorInfo(5, "SQL46010", "IF"));
            ParserTestUtils.ErrorTest130("DROP IF PROCEDURE EXISTS procX", new ParserErrorInfo(5, "SQL46010", "IF"));
            ParserTestUtils.ErrorTest130("DROP PROCEDURE procX IF EXISTS", new ParserErrorInfo(30, "SQL46029", ""));
            ParserTestUtils.ErrorTest130("DROP PROCEDURE IF EXISTS", new ParserErrorInfo(24, "SQL46029", ""));
            ParserTestUtils.ErrorTest130("DROP PROCEDURE IF EXIST procX", new ParserErrorInfo(18, "SQL46010", "EXIST"));

            // DROP TRIGGER
            //
            ParserTestUtils.ErrorTest130("DROP IF EXISTS TRIGGER trgX", new ParserErrorInfo(5, "SQL46010", "IF"));
            ParserTestUtils.ErrorTest130("DROP IF TRIGGER EXISTS trgX", new ParserErrorInfo(5, "SQL46010", "IF"));
            ParserTestUtils.ErrorTest130("DROP TRIGGER trgX IF EXISTS", new ParserErrorInfo(27, "SQL46029", ""));
            ParserTestUtils.ErrorTest130("DROP TRIGGER IF EXISTS", new ParserErrorInfo(22, "SQL46029", ""));
            ParserTestUtils.ErrorTest130("DROP TRIGGER IF EXIST trgX", new ParserErrorInfo(16, "SQL46010", "EXIST"));

            // DROP RULE
            //
            ParserTestUtils.ErrorTest130("DROP IF EXISTS RULE ruleX", new ParserErrorInfo(5, "SQL46010", "IF"));
            ParserTestUtils.ErrorTest130("DROP IF RULE EXISTS ruleX", new ParserErrorInfo(5, "SQL46010", "IF"));
            ParserTestUtils.ErrorTest130("DROP RULE ruleX IF EXISTS", new ParserErrorInfo(25, "SQL46029", ""));
            ParserTestUtils.ErrorTest130("DROP RULE IF EXISTS", new ParserErrorInfo(19, "SQL46029", ""));
            ParserTestUtils.ErrorTest130("DROP RULE IF EXIST ruleX", new ParserErrorInfo(13, "SQL46010", "EXIST"));

            // DROP DEFAULT
            //
            ParserTestUtils.ErrorTest130("DROP IF EXISTS DEFAULT defX", new ParserErrorInfo(5, "SQL46010", "IF"));
            ParserTestUtils.ErrorTest130("DROP IF DEFAULT EXISTS defX", new ParserErrorInfo(5, "SQL46010", "IF"));
            ParserTestUtils.ErrorTest130("DROP DEFAULT defX IF EXISTS", new ParserErrorInfo(27, "SQL46029", ""));
            ParserTestUtils.ErrorTest130("DROP DEFAULT IF EXISTS", new ParserErrorInfo(22, "SQL46029", ""));
            ParserTestUtils.ErrorTest130("DROP DEFAULT IF EXIST defX", new ParserErrorInfo(16, "SQL46010", "EXIST"));

            // DROP AGGREGATE
            //
            ParserTestUtils.ErrorTest130("DROP IF EXISTS AGGREGATE aggX", new ParserErrorInfo(5, "SQL46010", "IF"));
            ParserTestUtils.ErrorTest130("DROP IF AGGREGATE EXISTS aggX", new ParserErrorInfo(5, "SQL46010", "IF"));
            ParserTestUtils.ErrorTest130("DROP AGGREGATE aggX IF EXISTS", new ParserErrorInfo(29, "SQL46029", ""));
            ParserTestUtils.ErrorTest130("DROP AGGREGATE IF EXISTS", new ParserErrorInfo(24, "SQL46029", ""));
            ParserTestUtils.ErrorTest130("DROP AGGREGATE IF EXIST aggX", new ParserErrorInfo(18, "SQL46010", "EXIST"));

            // DROP SYNONYM
            //
            ParserTestUtils.ErrorTest130("DROP IF EXISTS SYNONYM synX", new ParserErrorInfo(5, "SQL46010", "IF"));
            ParserTestUtils.ErrorTest130("DROP IF SYNONYM EXISTS synX", new ParserErrorInfo(5, "SQL46010", "IF"));
            ParserTestUtils.ErrorTest130("DROP SYNONYM synX IF EXISTS", new ParserErrorInfo(27, "SQL46029", ""));
            ParserTestUtils.ErrorTest130("DROP SYNONYM IF EXISTS", new ParserErrorInfo(22, "SQL46029", ""));
            ParserTestUtils.ErrorTest130("DROP SYNONYM IF EXIST synX", new ParserErrorInfo(16, "SQL46010", "EXIST"));

            // DROP SEQUENCE
            //
            ParserTestUtils.ErrorTest130("DROP IF EXISTS SEQUENCE seqX", new ParserErrorInfo(5, "SQL46010", "IF"));
            ParserTestUtils.ErrorTest130("DROP IF SEQUENCE EXISTS seqX", new ParserErrorInfo(5, "SQL46010", "IF"));
            ParserTestUtils.ErrorTest130("DROP SEQUENCE seqX IF EXISTS", new ParserErrorInfo(28, "SQL46029", ""));
            ParserTestUtils.ErrorTest130("DROP SEQUENCE IF EXISTS", new ParserErrorInfo(23, "SQL46029", ""));
            ParserTestUtils.ErrorTest130("DROP SEQUENCE IF EXIST seqX", new ParserErrorInfo(17, "SQL46010", "EXIST"));

            // DROP SECURITY POLICY
            //
            ParserTestUtils.ErrorTest130("DROP IF EXISTS SECURITY POLICY secX", new ParserErrorInfo(5, "SQL46010", "IF"));
            ParserTestUtils.ErrorTest130("DROP IF SECURITY POLICY EXISTS secX", new ParserErrorInfo(5, "SQL46010", "IF"));
            ParserTestUtils.ErrorTest130("DROP SECURITY POLICY secX IF EXISTS", new ParserErrorInfo(35, "SQL46029", ""));
            ParserTestUtils.ErrorTest130("DROP SECURITY POLICY IF EXISTS", new ParserErrorInfo(30, "SQL46029", ""));
            ParserTestUtils.ErrorTest130("DROP SECURITY POLICY IF EXIST secX", new ParserErrorInfo(24, "SQL46010", "EXIST"));

            // DROP USER
            //
            ParserTestUtils.ErrorTest130("DROP IF EXISTS USER userX", new ParserErrorInfo(5, "SQL46010", "IF"));
            ParserTestUtils.ErrorTest130("DROP IF USER EXISTS userX", new ParserErrorInfo(5, "SQL46010", "IF"));
            ParserTestUtils.ErrorTest130("DROP USER userX IF EXISTS", new ParserErrorInfo(25, "SQL46029", ""));
            ParserTestUtils.ErrorTest130("DROP USER IF EXISTS", new ParserErrorInfo(19, "SQL46029", ""));
            ParserTestUtils.ErrorTest130("DROP USER IF EXIST userX", new ParserErrorInfo(13, "SQL46010", "EXIST"));

            // DROP ROLE
            //
            ParserTestUtils.ErrorTest130("DROP IF EXISTS ROLE roleX", new ParserErrorInfo(5, "SQL46010", "IF"));
            ParserTestUtils.ErrorTest130("DROP IF ROLE EXISTS roleX", new ParserErrorInfo(5, "SQL46010", "IF"));
            ParserTestUtils.ErrorTest130("DROP ROLE roleX IF EXISTS", new ParserErrorInfo(25, "SQL46029", ""));
            ParserTestUtils.ErrorTest130("DROP ROLE IF EXISTS", new ParserErrorInfo(19, "SQL46029", ""));
            ParserTestUtils.ErrorTest130("DROP ROLE IF EXIST roleX", new ParserErrorInfo(13, "SQL46010", "EXIST"));

            // DROP ASSEMBLY
            //
            ParserTestUtils.ErrorTest130("DROP IF EXISTS ASSEMBLY asmX", new ParserErrorInfo(5, "SQL46010", "IF"));
            ParserTestUtils.ErrorTest130("DROP IF ASSEMBLY EXISTS asmX", new ParserErrorInfo(5, "SQL46010", "IF"));
            ParserTestUtils.ErrorTest130("DROP ASSEMBLY asmX IF EXISTS", new ParserErrorInfo(28, "SQL46029", ""));
            ParserTestUtils.ErrorTest130("DROP ASSEMBLY IF EXISTS", new ParserErrorInfo(23, "SQL46029", ""));
            ParserTestUtils.ErrorTest130("DROP ASSEMBLY IF EXIST asmX", new ParserErrorInfo(17, "SQL46010", "EXIST"));

            // DROP CONSTRAINT
            //
            ParserTestUtils.ErrorTest130("ALTER TABLE tableX DROP IF EXISTS CONSTRAINT cnstrtX", new ParserErrorInfo(24, "SQL46010", "IF"));
            ParserTestUtils.ErrorTest130("ALTER TABLE tableX DROP IF CONSTRAINT EXISTS cnstrtX", new ParserErrorInfo(24, "SQL46010", "IF"));
            ParserTestUtils.ErrorTest130("ALTER TABLE tableX DROP CONSTRAINT cnstrtX IF EXISTS", new ParserErrorInfo(52, "SQL46029", ""));
            ParserTestUtils.ErrorTest130("ALTER TABLE tableX DROP CONSTRAINT IF EXISTS", new ParserErrorInfo(44, "SQL46029", ""));
            ParserTestUtils.ErrorTest130("ALTER TABLE tableX DROP CONSTRAINT IF EXIST cnstrtX", new ParserErrorInfo(38, "SQL46010", "EXIST"));

            // DROP COLUMN
            //
            ParserTestUtils.ErrorTest130("ALTER TABLE tableX DROP IF EXISTS COLUMN colX", new ParserErrorInfo(24, "SQL46010", "IF"));
            ParserTestUtils.ErrorTest130("ALTER TABLE tableX DROP IF COLUMN EXISTS colX", new ParserErrorInfo(24, "SQL46010", "IF"));
            ParserTestUtils.ErrorTest130("ALTER TABLE tableX DROP COLUMN colX IF EXISTS", new ParserErrorInfo(45, "SQL46029", ""));
            ParserTestUtils.ErrorTest130("ALTER TABLE tableX DROP COLUMN IF EXISTS", new ParserErrorInfo(40, "SQL46029", ""));
            ParserTestUtils.ErrorTest130("ALTER TABLE tableX DROP COLUMN IF EXIST colX", new ParserErrorInfo(34, "SQL46010", "EXIST"));
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void CreateTableInlineFilteredIndexNegativeTests()
        {
            // Table level inline clustered index with filter
            //
            ParserTestUtils.ErrorTest130(
                "CREATE TABLE T1 ( C1 INT NOT NULL, INDEX idx CLUSTERED (C1) WHERE C1 > 10 )",
                new ParserErrorInfo(60, "SQL46010", "WHERE"));

            // Column level inline clustered index with filter
            //
            ParserTestUtils.ErrorTest130(
                "CREATE TABLE T1 ( C1 INT NOT NULL INDEX idx CLUSTERED WHERE C1 > 10, C2 INT NOT NULL )",
                new ParserErrorInfo(54, "SQL46010", "WHERE"));
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void FederationStatementErrorTest()
        {
            //Two and Three-Part name
            //
            ParserTestUtils.ErrorTest110("CREATE FEDERATION [dbo].FED (K1 INT RANGE)", new ParserErrorInfo(23, "SQL46010", "."));
            ParserTestUtils.ErrorTest110("CREATE FEDERATION [server].[dbo].FED (K1 INT RANGE)", new ParserErrorInfo(26, "SQL46010", "."));
            ParserTestUtils.ErrorTest110("ALTER FEDERATION [dbo].FED SPLIT at (K1 = 10)", new ParserErrorInfo(22, "SQL46010", "."));
            ParserTestUtils.ErrorTest110("ALTER FEDERATION [server].[dbo].FED SPLIT at (K1 = 10)", new ParserErrorInfo(25, "SQL46010", "."));
            ParserTestUtils.ErrorTest110("DROP FEDERATION [dbo].FED", new ParserErrorInfo(21, "SQL46010", "."));
            ParserTestUtils.ErrorTest110("DROP FEDERATION [server].[dbo].FED", new ParserErrorInfo(24, "SQL46010", "."));
            // We wanted to make sure �USE FEDERATION;� didn�t throw an error (since it should be considered as USE statement for a database named FEDERATION).
            // SQL46010: Incorrect syntax near [dbo]. at offset 15, line 1, column 16.
            //
            ParserTestUtils.ErrorTest110("USE FEDERATION [dbo].FED (K1 = 20) WITH FILTERING = ON, reset", new ParserErrorInfo(15, "SQL46010", "[dbo]"));
            ParserTestUtils.ErrorTest110("USE FEDERATION [server].[dbo].FED (K1 = 20) WITH FILTERING = ON, reset", new ParserErrorInfo(15, "SQL46010", "[server]"));

            // Create
            //
            ParserTestUtils.ErrorTest110("CREATE FEDERATION FED", new ParserErrorInfo(21, "SQL46029", "FEDERATION"));
            ParserTestUtils.ErrorTest110("CREATE FEDERATION FED (K1 int KEY)", new ParserErrorInfo(30, "SQL46010", "KEY"));
            ParserTestUtils.ErrorTest110("CREATE FEDERATION FED (KEY int RANGE)", new ParserErrorInfo(23, "SQL46010", "KEY"));
            ParserTestUtils.ErrorTest110("CREATE FEDERATION KEY (K1 int RANGE)", new ParserErrorInfo(18, "SQL46010", "KEY"));

            // Alter
            //
            ParserTestUtils.ErrorTest110("ALTER FEDERATION FED SPLIT", new ParserErrorInfo(26, "SQL46029", ""));
            ParserTestUtils.ErrorTest110("ALTER FEDERATION FED SPLIT (K1 = 10)", new ParserErrorInfo(27, "SQL46010", "("));
            ParserTestUtils.ErrorTest110("ALTER FEDERATION FED SPLIT AT (K1 10)", new ParserErrorInfo(34, "SQL46010", "10"));
            ParserTestUtils.ErrorTest110("ALTER FEDERATION FED DROP (low K1 = 10)", new ParserErrorInfo(26, "SQL46010", "("));

            // This is a limitation of SQLDOM parser. There are a number of instances where this happens, and an UNDONE to allow errors to take into account multiple options. So here can't say expected HIGH or LOW
            // SQL46005: Expected HIGH but encountered K1 instead.
            //
            ParserTestUtils.ErrorTest110("ALTER FEDERATION FED DROP AT (K1 = 10)", new ParserErrorInfo(30, "SQL46005", new string[] { "HIGH", "K1" }));
            ParserTestUtils.ErrorTest110("ALTER FEDERATION FED DROP AT (low K1 10)", new ParserErrorInfo(37, "SQL46010", "10"));

            // USE
            //
            ParserTestUtils.ErrorTest110("USE FEDERATION FED (K1, 20)", new ParserErrorInfo(22, "SQL46010", ","));
            ParserTestUtils.ErrorTest110("USE FEDERATION FED (K1 = 20) FILTERING = ON, reset", new ParserErrorInfo(29, "SQL46010", "FILTERING"));
            ParserTestUtils.ErrorTest110("USE FEDERATION FED (K1 = 20) WITH FILTERING = ON reset", new ParserErrorInfo(49, "SQL46010", "reset"));
            // Another limitation. Can't say expect ', reset'
            // SQL46029: Unexpected end of file occurred. at offset 48, line 1, column 49.
            //
            ParserTestUtils.ErrorTest110("USE FEDERATION FED (K1 = 20) WITH FILTERING = ON", new ParserErrorInfo(48, "SQL46029", ""));
            ParserTestUtils.ErrorTest110("USE FEDERATION FED (K1 = 20) WITH FILTERING = ready, reset", new ParserErrorInfo(46, "SQL46010", "ready"));
            ParserTestUtils.ErrorTest110("USE FEDERATION FED WITH FILTERING = ON, reset", new ParserErrorInfo(15, "SQL46010", "FED"));
            ParserTestUtils.ErrorTest110("USE FEDERATION FED (K1, 20) WITH FILTERING = ON, reset", new ParserErrorInfo(22, "SQL46010", ","));
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void CreateTableDanglingCommaTest()
        {
            // One dangling comma is ok - but only in create table
            ParserTestUtils.ErrorTestAllParsers("create table t1 (c1 int, c2 int,)");

            // But more than one is not...
            ParserTestUtils.ErrorTestAllParsers("create table t1 (c1 int, c2 int, ,)",
                new ParserErrorInfo(33, "SQL46010", ","));
            // And extra comma can happen only at the end!
            ParserTestUtils.ErrorTestAllParsers("create table t1 (c1 int, c2 int, , c3 int)",
                new ParserErrorInfo(33, "SQL46010", ","));

            // Dangling comma is not allowed in table-valued function...
            ParserTestUtils.ErrorTestAllParsers("create function f1() returns @retVal table(c1 int, c2 int,) as begin return end",
                new ParserErrorInfo(58, "SQL46010", ")"));
            // ... and in table valued variables
            ParserTestUtils.ErrorTest90AndAbove("declare @v1 as table(c1 int, c2 int,)",
                new ParserErrorInfo(36, "SQL46010", ")"));
            // ... and in table types
            ParserTestUtils.ErrorTest100("create type t1 as table(c1 int, c2 int,)",
                new ParserErrorInfo(39, "SQL46010", ")"));
        }

        private void TestErrorRecoverySingleErrorTest(TSqlParser parser, string fileName, ParserErrorInfo errorVerifier)
        {
            IList<ParseError> errors;
            TSqlFragment fragment = ParserTestUtils.ParseFromResource(parser, fileName, out errors);

            TSqlScript script = (TSqlScript)fragment;
            Assert.AreEqual<int>(0, script.Batches.Count);

            Assert.AreEqual<int>(1, errors.Count);

            errorVerifier.VerifyError(errors[0]);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void CreateTriggerStatementErrorTests()
        {
            ParserTestUtils.ExecuteTestForAllParsers(delegate (TSqlParser parser)
            {
                TestErrorRecoverySingleErrorTest(parser, "CreateTriggerStatementErrorTests.sql",
                    new ParserErrorInfo(141, "SQL46010", ";"));
            }, true);

            ParserTestUtils.ErrorTest90AndAbove("CREATE TRIGGER Trigger1 ON dbo.SomeTableOrView FOR DELETE, DELETE, UPDATE AS BEGIN SET NOCOUNT ON END",
                new ParserErrorInfo(59, "SQL46090", "Delete"));

            //positive case for trigger RENAME
            ParserTestUtils.ErrorTest100(@"CREATE TRIGGER [test] ON DATABASE AFTER RENAME As
BEGIN
   PRINT 'Test' 
END
GO");
            //negative case for trigger
            ParserTestUtils.ErrorTest130(@"CREATE TRIGGER [test] ON DATABASE AFTER RNAME AS 
BEGIN
   PRINT 'Test'
END
GO
", new ParserErrorInfo(40, "SQL46010", "RNAME"));
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void CreateSchemaStatementErrorTests()
        {
            ParserTestUtils.ExecuteTestForAllParsers(delegate (TSqlParser parser)
            {
                TestErrorRecoverySingleErrorTest(parser, "CreateSchemaStatementErrorTests.sql",
                    new ParserErrorInfo(102, "SQL46010", "on"));
            }, true);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void MultipleErrorTests()
        {
            ParserTestUtils.ExecuteTestForAllParsers(MultipleErrorTestImpl, true);
        }

        public void MultipleErrorTestImpl(TSqlParser parser)
        {
            IList<ParseError> errors;
            TSqlFragment fragment = ParserTestUtils.ParseFromResource(parser, "MultipleErrorTests.sql", out errors);

            TSqlScript script = (TSqlScript)fragment;
            Assert.AreEqual<int>(3, script.Batches.Count);
            Assert.AreEqual<int>(3, script.Batches[0].Statements.Count);
            Assert.AreEqual<int>(1, script.Batches[1].Statements.Count);
            Assert.AreEqual<int>(2, script.Batches[2].Statements.Count);

            Assert.AreEqual<int>(5, errors.Count);

            Assert.AreEqual<int>(46010, errors[0].Number);
            Assert.AreEqual<int>(37, errors[0].Offset);
            Assert.AreEqual<int>(46010, errors[1].Number);
            Assert.AreEqual<int>(97, errors[1].Offset);
            Assert.AreEqual<int>(46010, errors[2].Number);
            Assert.AreEqual<int>(178, errors[2].Offset);
            Assert.AreEqual<int>(46003, errors[3].Number);
            Assert.AreEqual<int>(275, errors[3].Offset);
            Assert.AreEqual<int>(46010, errors[4].Number);
            Assert.AreEqual<int>(384, errors[4].Offset);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void ViewOptionRepeatErrorTest()
        {
            const string source = "CREATE View v1 WITH encryption, schemabinding, encryption as select 10;";

            ParserTestUtils.ErrorTestAllParsers(source,
                new ParserErrorInfo(47, "SQL46049", "encryption"));
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void QueryOptimizerHintErrorTest()
        {
            const string source = "SELECT * FROM t1 OPTION (SOME_HINT_NEVER_EXISTS);";

            ParserTestUtils.ErrorTest100(source, new ParserErrorInfo(25, "SQL46005", CodeGenerationSupporter.Recompile, "SOME_HINT_NEVER_EXISTS"));

            ParserTestUtils.ErrorTest110(source, new ParserErrorInfo(25, "SQL46005", CodeGenerationSupporter.IgnoreNonClusteredColumnStoreIndex, "SOME_HINT_NEVER_EXISTS"));
        }

        /// <summary>
        /// Tests the USE HINT query hint mechanism
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void QueryOptimizerUseHintClauseErrorTest()
        {
            // SQL DOM doesn't check for valid/invalid strings, so this should only fail on parsers
            // from before the syntax was added (120 and below).
            const string sourceInvalidHint = "SELECT * FROM t1 OPTION (USE HINT('SOME_HINT_NEVER_EXISTS'));";

            ParserTestUtils.ErrorTest100(sourceInvalidHint, new ParserErrorInfo(29, "SQL46010", CodeGenerationSupporter.Hint));
            ParserTestUtils.ErrorTest110(sourceInvalidHint, new ParserErrorInfo(29, "SQL46010", CodeGenerationSupporter.Hint));
            ParserTestUtils.ErrorTest120(sourceInvalidHint, new ParserErrorInfo(29, "SQL46010", CodeGenerationSupporter.Hint));
            // Should succeed on 130 and above

            // Again validity is not checked, should fail only on 120 and below.
            const string sourceValidHint = "SELECT * FROM t1 OPTION (USE HINT('DISABLE_OPTIMIZED_NESTED_LOOP'));";

            ParserTestUtils.ErrorTest100(sourceValidHint, new ParserErrorInfo(29, "SQL46010", CodeGenerationSupporter.Hint));
            ParserTestUtils.ErrorTest110(sourceValidHint, new ParserErrorInfo(29, "SQL46010", CodeGenerationSupporter.Hint));
            ParserTestUtils.ErrorTest120(sourceValidHint, new ParserErrorInfo(29, "SQL46010", CodeGenerationSupporter.Hint));
            // Should succeed on 130 and above

            // This syntax is invalid, so we should get an error on all parsers, including 140.
            const string sourceEmptyHint = "SELECT * FROM t1 OPTION (USE HINT());";

            ParserTestUtils.ErrorTest100(sourceEmptyHint, new ParserErrorInfo(29, "SQL46010", CodeGenerationSupporter.Hint));
            ParserTestUtils.ErrorTest110(sourceEmptyHint, new ParserErrorInfo(29, "SQL46010", CodeGenerationSupporter.Hint));
            ParserTestUtils.ErrorTest120(sourceEmptyHint, new ParserErrorInfo(29, "SQL46010", CodeGenerationSupporter.Hint));
            ParserTestUtils.ErrorTest130(sourceEmptyHint, new ParserErrorInfo(34, "SQL46010", ")"));
            ParserTestUtils.ErrorTest140(sourceEmptyHint, new ParserErrorInfo(34, "SQL46010", ")"));

            // No quotes around the hint, should fail on all parsers.
            const string sourceValidHintNoString = "SELECT * FROM t1 OPTION (USE HINT(DISABLE_OPTIMIZED_NESTED_LOOP));";

            ParserTestUtils.ErrorTest100(sourceValidHintNoString, new ParserErrorInfo(29, "SQL46010", CodeGenerationSupporter.Hint));
            ParserTestUtils.ErrorTest110(sourceValidHintNoString, new ParserErrorInfo(29, "SQL46010", CodeGenerationSupporter.Hint));
            ParserTestUtils.ErrorTest120(sourceValidHintNoString, new ParserErrorInfo(29, "SQL46010", CodeGenerationSupporter.Hint));
            ParserTestUtils.ErrorTest130(sourceValidHintNoString, new ParserErrorInfo(34, "SQL46010", "DISABLE_OPTIMIZED_NESTED_LOOP"));
            ParserTestUtils.ErrorTest140(sourceValidHintNoString, new ParserErrorInfo(34, "SQL46010", "DISABLE_OPTIMIZED_NESTED_LOOP"));

            // HINT keyword is omitted, should fail on all parsers. Error will be on (, since "USE PLAN" is expected on old parsers.
            const string sourceNoHintKeyword = "SELECT * FROM t1 OPTION (USE ('DISABLE_OPTIMIZED_NESTED_LOOP'));";

            ParserTestUtils.ErrorTest100(sourceNoHintKeyword, new ParserErrorInfo(29, "SQL46010", "("));
            ParserTestUtils.ErrorTest110(sourceNoHintKeyword, new ParserErrorInfo(29, "SQL46010", "("));
            ParserTestUtils.ErrorTest120(sourceNoHintKeyword, new ParserErrorInfo(29, "SQL46010", "("));
            ParserTestUtils.ErrorTest130(sourceNoHintKeyword, new ParserErrorInfo(25, "SQL46010", "USE"));
            ParserTestUtils.ErrorTest140(sourceNoHintKeyword, new ParserErrorInfo(25, "SQL46010", "USE"));

            // USE keyword is omitted, should fail on all parsers.
            const string sourceNoUseKeyword = "SELECT * FROM t1 OPTION (HINT ('DISABLE_OPTIMIZED_NESTED_LOOP'));";

            ParserTestUtils.ErrorTest100(sourceNoUseKeyword, new ParserErrorInfo(25, "SQL46010", CodeGenerationSupporter.Hint));
            ParserTestUtils.ErrorTest110(sourceNoUseKeyword, new ParserErrorInfo(25, "SQL46010", CodeGenerationSupporter.Hint));
            ParserTestUtils.ErrorTest120(sourceNoUseKeyword, new ParserErrorInfo(25, "SQL46010", CodeGenerationSupporter.Hint));
            ParserTestUtils.ErrorTest130(sourceNoUseKeyword, new ParserErrorInfo(25, "SQL46010", CodeGenerationSupporter.Hint));
            ParserTestUtils.ErrorTest140(sourceNoUseKeyword, new ParserErrorInfo(25, "SQL46010", CodeGenerationSupporter.Hint));

            // Order of USE and HINT keywords flipped, should fail on all parsers.
            const string sourceFlipOrderKeywords = "SELECT * FROM t1 OPTION (HINT USE ('DISABLE_OPTIMIZED_NESTED_LOOP'));";

            ParserTestUtils.ErrorTest100(sourceFlipOrderKeywords, new ParserErrorInfo(25, "SQL46010", CodeGenerationSupporter.Hint));
            ParserTestUtils.ErrorTest110(sourceFlipOrderKeywords, new ParserErrorInfo(25, "SQL46010", CodeGenerationSupporter.Hint));
            ParserTestUtils.ErrorTest120(sourceFlipOrderKeywords, new ParserErrorInfo(25, "SQL46010", CodeGenerationSupporter.Hint));
            ParserTestUtils.ErrorTest130(sourceFlipOrderKeywords, new ParserErrorInfo(25, "SQL46010", CodeGenerationSupporter.Hint));
            ParserTestUtils.ErrorTest140(sourceFlipOrderKeywords, new ParserErrorInfo(25, "SQL46010", CodeGenerationSupporter.Hint));

        }
        /// <summary>
        /// Tests the LABEL query option
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void QueryLabelOptionClauseErrorTest()
        {
            const string sourceInvalidHint = "SELECT * FROM t1 OPTION (LABEL 'test');";
            const string sourceInvalidHint1 = "SELECT * FROM t1 OPTION (LABEL = test2);";
            const string sourceInvalidHint2 = "SELECT * FROM t1 OPTION ('test');";

            ParserTestUtils.ErrorTest130(sourceInvalidHint, new ParserErrorInfo(25, "SQL46010", CodeGenerationSupporter.Label));
            ParserTestUtils.ErrorTest140(sourceInvalidHint, new ParserErrorInfo(25, "SQL46010", CodeGenerationSupporter.Label));
            ParserTestUtils.ErrorTest150(sourceInvalidHint, new ParserErrorInfo(25, "SQL46010", CodeGenerationSupporter.Label));

            ParserTestUtils.ErrorTest130(sourceInvalidHint1, new ParserErrorInfo(33, "SQL46010", "test2"));
            ParserTestUtils.ErrorTest140(sourceInvalidHint1, new ParserErrorInfo(33, "SQL46010", "test2"));
            ParserTestUtils.ErrorTest150(sourceInvalidHint1, new ParserErrorInfo(33, "SQL46010", "test2"));

            ParserTestUtils.ErrorTest130(sourceInvalidHint2, new ParserErrorInfo(25, "SQL46010", "'test'"));
            ParserTestUtils.ErrorTest140(sourceInvalidHint2, new ParserErrorInfo(25, "SQL46010", "'test'"));
            ParserTestUtils.ErrorTest150(sourceInvalidHint2, new ParserErrorInfo(25, "SQL46010", "'test'"));
        }
        /// <summary>
        /// Tests the Create External Data Source query
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void ExternalDatasourceInvalidPushdownOptionErrorTest()
        {
            string externalDataSourceCreateCommand = @"
                         CREATE EXTERNAL DATA SOURCE InvalidPushdown_ExternalDataSource
                         WITH
                         (
                             LOCATION = 'hdfs://10.10.10.10:8050',
                             PUSHDOWN = TEST
                         )";
            ParserTestUtils.ErrorTest150(externalDataSourceCreateCommand, new ParserErrorInfo(externalDataSourceCreateCommand.IndexOf("TEST"), "SQL46010", "TEST"));
        }
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void BeginEndStatementErrorTests()
        {
            ParserTestUtils.ExecuteTestForAllParsers(BeginEndStatementErrorTestImpl, true);
        }

        public void BeginEndStatementErrorTestImpl(TSqlParser parser)
        {
            IList<ParseError> errors;
            TSqlScript script = (TSqlScript)ParserTestUtils.ParseFromResource(parser, "BeginEndStatementErrorTests.sql", out errors);

            Assert.AreEqual<int>(2, errors.Count);

            Assert.AreEqual<int>(46010, errors[0].Number);
            Assert.AreEqual<int>(161, errors[0].Offset);

            Assert.AreEqual<int>(46010, errors[1].Number);
            Assert.AreEqual<int>(304, errors[1].Offset);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void MLPOptionErrorTest()
        {
            // Syntax errors
            //
            ParserTestUtils.ErrorTest120("ALTER INDEX idx1 ON t1 REBUILD WITH (ONLINE = OFF (WAIT_AT_LOW_PRIORITY (MAX_DURATION = 1, ABORT_AFTER_WAIT = NONE)))",
                new ParserErrorInfo(50, "SQL46010", "("));
            ParserTestUtils.ErrorTest120("ALTER INDEX idx1 ON t1 REBUILD WITH (ONLINE = ON (WAIT_AT_LOW_PRIORITY (MAX_DURATION = -1, ABORT_AFTER_WAIT = NONE)))",
                new ParserErrorInfo(87, "SQL46010", "-"));
            ParserTestUtils.ErrorTest120("ALTER INDEX idx1 ON t1 REBUILD WITH (ONLINE = ON (WAIT_AT_LOW_PRIORITY (MAX_DURATION = 1, ABORT_AFTER_WAIT = NO)))",
                new ParserErrorInfo(109, "SQL46010", "NO"));

            ParserTestUtils.ErrorTest120("ALTER TABLE t1 REBUILD WITH (ONLINE = OFF (WAIT_AT_LOW_PRIORITY (MAX_DURATION = 1, ABORT_AFTER_WAIT = NONE)))",
                new ParserErrorInfo(42, "SQL46010", "("));
            ParserTestUtils.ErrorTest120("ALTER TABLE t1 REBUILD WITH (ONLINE = ON (WAIT_AT_LOW_PRIORITY (MAX_DURATION = -1, ABORT_AFTER_WAIT = NONE)))",
                new ParserErrorInfo(79, "SQL46010", "-"));
            ParserTestUtils.ErrorTest120("ALTER TABLE t1 REBUILD WITH (ONLINE = ON (WAIT_AT_LOW_PRIORITY (MAX_DURATION = 1, ABORT_AFTER_WAIT = OTHER)))",
                new ParserErrorInfo(101, "SQL46010", "OTHER"));

            ParserTestUtils.ErrorTest120("ALTER TABLE t1 SWITCH TO t2 WITH (WAIT_AT_LOW_PRIORITY (MAX_DURATION = -1, ABORT_AFTER_WAIT = NONE))",
                new ParserErrorInfo(71, "SQL46010", "-"));
            ParserTestUtils.ErrorTest120("ALTER TABLE t1 SWITCH TO t2 WITH (WAIT_AT_LOW_PRIORITY (MAX_DURATION = 1, ABORT_AFTER_WAIT = NONENONE))",
                new ParserErrorInfo(93, "SQL46010", "NONENONE"));

            // Note: SQL46101Test and SQL46102Test cover other invalid MAX_DURATION values

            // Unsupported statement types
            // Expected Error: 46101 syntax error or 46057 Option 'WAIT_AT_LOW_PRIORITY' is not a valid index option in '******' statement.
            //
            ParserTestUtils.ErrorTest120("CREATE INDEX idx1 ON t1(c1) WITH (ONLINE = ON (WAIT_AT_LOW_PRIORITY (MAX_DURATION = 1, ABORT_AFTER_WAIT = NONE)))",
                new ParserErrorInfo(47, "SQL46057", "WAIT_AT_LOW_PRIORITY", "CREATE INDEX"));
            ParserTestUtils.ErrorTest120("ALTER TABLE t1 ADD CONSTRAINT constraint1 UNIQUE (c1) WITH (ONLINE = ON (WAIT_AT_LOW_PRIORITY (MAX_DURATION = 1, ABORT_AFTER_WAIT = NONE)))",
                new ParserErrorInfo(73, "SQL46057", "WAIT_AT_LOW_PRIORITY", "ALTER TABLE"));
            ParserTestUtils.ErrorTest120("ALTER TABLE t1 DROP CONSTRAINT constraint1 WITH (ONLINE = ON (WAIT_AT_LOW_PRIORITY (MAX_DURATION = 1, ABORT_AFTER_WAIT = NONE)))",
                new ParserErrorInfo(61, "SQL46010", "("));
            ParserTestUtils.ErrorTest120("CREATE XML INDEX idx1 ON t1(c1) USING XML INDEX xidx FOR VALUE WITH (ONLINE = ON (WAIT_AT_LOW_PRIORITY (MAX_DURATION = 1, ABORT_AFTER_WAIT = NONE)))",
                new ParserErrorInfo(82, "SQL46057", "WAIT_AT_LOW_PRIORITY", "CREATE XML INDEX"));
            ParserTestUtils.ErrorTest120("CREATE SPATIAL INDEX idx1 ON t1(c1) WITH (ONLINE = ON (WAIT_AT_LOW_PRIORITY (MAX_DURATION = 1, ABORT_AFTER_WAIT = NONE)))",
                new ParserErrorInfo(55, "SQL46057", "WAIT_AT_LOW_PRIORITY", "CREATE SPATIAL INDEX"));
            ParserTestUtils.ErrorTest120("CREATE COLUMNSTORE INDEX idx1 ON t1(c1) WITH (ONLINE = ON (WAIT_AT_LOW_PRIORITY (MAX_DURATION = 1, ABORT_AFTER_WAIT = NONE)))",
                new ParserErrorInfo(46, "SQL46057", "ONLINE", "CREATE COLUMNSTORE INDEX"));

            //'WAIT_AT_LOW_PRIORITY' is a valid option for 140 for alter table but not with online
            //
            ParserTestUtils.ErrorTest140("ALTER TABLE t1 DROP CONSTRAINT constraint1 WITH (ONLINE = ON (WAIT_AT_LOW_PRIORITY (MAX_DURATION = 1, ABORT_AFTER_WAIT = NONE)))",
                new ParserErrorInfo(61, "SQL46010", "("));
            ParserTestUtils.ErrorTest140("ALTER TABLE t1 DROP CONSTRAINT constraint1 WITH (ONLINE = ON, WAIT_AT_LOW_PRIORITY (MAX_DURATION = -1, ABORT_AFTER_WAIT = NONE))",
                new ParserErrorInfo(99, "SQL46010", "-"));
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void InlineIndexErrors()
        {
            // Syntax errors
            //
            ParserTestUtils.ErrorTest120("CREATE TABLE T (i INT INDEX);",
                new ParserErrorInfo(27, "SQL46010", ")"));

            ParserTestUtils.ErrorTest120("CREATE TABLE T (i INT, INDEX);",
                new ParserErrorInfo(28, "SQL46010", ")"));

            ParserTestUtils.ErrorTest120("CREATE TABLE T (i INT, INDEX idx);",
                new ParserErrorInfo(32, "SQL46010", ")"));

            ParserTestUtils.ErrorTest120("CREATE TABLE T (i INT NOT NULL INDEX idx CLUSTERED HASH);",
                new ParserErrorInfo(51, "SQL46010", "HASH"));
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void AlterDatabaseSetEqualsOnOfOptionErrors()
        {
            // Syntax errors
            //
            ParserTestUtils.ErrorTest120("ALTER DATABASE db SET MEMORY_OPTIMIZED_ELEVATE_TO_SNAPSHOT ON;",
                new ParserErrorInfo(22, "SQL46010", "MEMORY_OPTIMIZED_ELEVATE_TO_SNAPSHOT"));
        }

        /// <summary>
        /// Only Secondary database can accept a PRIMARY value for configuration option
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void AlterDatabaseScopedConfigurationOptionNegativeTests()
        {
            // Case: Setting invalid values for primary database
            ParserTestUtils.ErrorTest130("ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = PRIMARY;",
                   new ParserErrorInfo(40, "SQL46115"));
            ParserTestUtils.ErrorTest130("ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;",
                 new ParserErrorInfo(40, "SQL46115"));
            ParserTestUtils.ErrorTest130("ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = PRIMARY;",
                 new ParserErrorInfo(40, "SQL46115"));
            ParserTestUtils.ErrorTest130("ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;",
                 new ParserErrorInfo(40, "SQL46115"));
            ParserTestUtils.ErrorTest130("ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ;",
                 new ParserErrorInfo(61, "SQL46010", ";"));
            ParserTestUtils.ErrorTest130("ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = ;",
                 new ParserErrorInfo(67, "SQL46010", ";"));
            ParserTestUtils.ErrorTest130("ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = ;",
                 new ParserErrorInfo(72, "SQL46010", ";"));
            ParserTestUtils.ErrorTest130("ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = -1;",
                 new ParserErrorInfo(49, "SQL46010", "-"));
            ParserTestUtils.ErrorTest140("ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = VALUE;",
                new ParserErrorInfo(49, "SQL46010", "VALUE"));
            ParserTestUtils.ErrorTest140("ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = VALUE",
                 new ParserErrorInfo(72, "SQL46010", "VALUE"));
            ParserTestUtils.ErrorTest140("ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = 1",
                 new ParserErrorInfo(72, "SQL46010", "1"));
            ParserTestUtils.ErrorTest130("ALTER DATABASE SCOPED CONFIGURATION SET DW_COMPATIBILITY_LEVEL = -1;",
                new ParserErrorInfo(65, "SQL46010", "-"));
            ParserTestUtils.ErrorTest130("ALTER DATABASE SCOPED CONFIGURATION SET DW_COMPATIBILITY_LEVEL = VALUE;",
                new ParserErrorInfo(65, "SQL46010", "VALUE"));
            ParserTestUtils.ErrorTest130("ALTER DATABASE SCOPED CONFIGURATION SET DW_COMPATIBILITY_LEVEL = ;",
                 new ParserErrorInfo(65, "SQL46010", ";"));

            // Case: Setting invalid options and values
            ParserTestUtils.ErrorTest140("ALTER DATABASE SCOPED CONFIGURATION SET 'quoted_option' = NEW_VALUE;",
                 new ParserErrorInfo(40, "SQL46010", "'quoted_option'"));
            ParserTestUtils.ErrorTest140("ALTER DATABASE SCOPED CONFIGURATION SET 2 = NEW_VALUE;",
                 new ParserErrorInfo(40, "SQL46010", "2"));
            ParserTestUtils.ErrorTest140("ALTER DATABASE SCOPED CONFIGURATION SET ON = NEW_VALUE;",
                 new ParserErrorInfo(40, "SQL46010", "ON"));
            ParserTestUtils.ErrorTest140("ALTER DATABASE SCOPED CONFIGURATION SET OFF = NEW_VALUE;",
                 new ParserErrorInfo(40, "SQL46010", "OFF"));
            ParserTestUtils.ErrorTest140("ALTER DATABASE SCOPED CONFIGURATION SET PRIMARY = NEW_VALUE;",
                 new ParserErrorInfo(40, "SQL46010", "PRIMARY"));
            ParserTestUtils.ErrorTest140("ALTER DATABASE SCOPED CONFIGURATION SET NEW_OPTION = 1.2;",
                 new ParserErrorInfo(53, "SQL46010", "1.2"));
            ParserTestUtils.ErrorTest140("ALTER DATABASE SCOPED CONFIGURATION SET NEW_OPTION = -1.2;",
                 new ParserErrorInfo(54, "SQL46010", "1.2"));
            ParserTestUtils.ErrorTest140("ALTER DATABASE SCOPED CONFIGURATION SET NEW_OPTION = +1;",
                 new ParserErrorInfo(53, "SQL46010", "+"));
            ParserTestUtils.ErrorTest140("ALTER DATABASE SCOPED CONFIGURATION SET NEW_OPTION = 2-3;",
                 new ParserErrorInfo(54, "SQL46010", "-"));
            ParserTestUtils.ErrorTest140("ALTER DATABASE SCOPED CONFIGURATION SET NEW_OPTION = ASC;",
                 new ParserErrorInfo(53, "SQL46010", "ASC"));
            ParserTestUtils.ErrorTest140("ALTER DATABASE SCOPED CONFIGURATION SET NEW_OPTION = PRIMARY;",
                 new ParserErrorInfo(40, "SQL46115"));
            ParserTestUtils.ErrorTest140("ALTER DATABASE SCOPED CONFIGURATION SET = NEW_VALUE",
                 new ParserErrorInfo(40, "SQL46010", "="));
            ParserTestUtils.ErrorTest140("ALTER DATABASE SCOPED CONFIGURATION SET NEW_OPTION = ;",
                 new ParserErrorInfo(53, "SQL46010", ";"));
            ParserTestUtils.ErrorTest140("ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET = NEW_VALUE;",
                 new ParserErrorInfo(54, "SQL46010", "="));
            ParserTestUtils.ErrorTest140("ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET NEW_OPTION = ;",
                 new ParserErrorInfo(67, "SQL46010", ";"));

            // Case: Incorrect syntax and misspelled key words
            ParserTestUtils.ErrorTest130("ALTER DATABASE SCOPED CONFIGURATION CLEAR PROCEDURE CACHE;",
                 new ParserErrorInfo(42, "SQL46010", "PROCEDURE"));
            ParserTestUtils.ErrorTest130("ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY CLEAR PROCEDURE CACHE;",
                 new ParserErrorInfo(56, "SQL46010", "PROCEDURE"));


        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TruncatePartitionsErrorTests()
        {
            // Syntax errors
            //
            ParserTestUtils.ErrorTest120("TRUNCATE TABLE t WITH ( 1 );",
                new ParserErrorInfo(24, "SQL46010", "1"));
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void LexerErrorTest()
        {
            ParserTestUtils.ErrorTestAllParsers("CREATE TABLE table1 (c1 int) [])",
                new ParserErrorInfo(30, "SQL46010", "]"));
            ParserTestUtils.ErrorTestAllParsers("CREATE VIEW v1 AS SELECT dbo.column1] FROM dbo.table1",
                new ParserErrorInfo(36, "SQL46010", "]"));
        }

        /// <summary>
        /// Negative tests for WINDOW syntax.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void WindowClauseNegativeTest()
        {
            // WINDOW clause should fail in 150 and below
            //
            const string windowInOverClause = "SELECT Sum(c1) OVER Win1 FROM t1 WINDOW Win1 AS (PARTITION BY c1);";

            ParserTestUtils.ErrorTest150(windowInOverClause, new ParserErrorInfo(15, "SQL46010", "OVER"));
            ParserTestUtils.ErrorTest140(windowInOverClause, new ParserErrorInfo(15, "SQL46010", "OVER"));
            ParserTestUtils.ErrorTest130(windowInOverClause, new ParserErrorInfo(15, "SQL46010", "OVER"));

            const string unusedWindowClause = "SELECT Sum(c1) OVER (PARTITION BY c1) FROM t1 WINDOW Win1 AS (PARTITION BY c1);";
            ParserTestUtils.ErrorTest150(unusedWindowClause, new ParserErrorInfo(53, "SQL46010", "Win1"));
            ParserTestUtils.ErrorTest140(unusedWindowClause, new ParserErrorInfo(53, "SQL46010", "Win1"));
            ParserTestUtils.ErrorTest130(unusedWindowClause, new ParserErrorInfo(53, "SQL46010", "Win1"));

            // Cannot use 'window' as table alias in 160
            //
            ParserTestUtils.ErrorTest160("SELECT Sum(Window.c1) OVER Win1 FROM t1 Window WINDOW Win1 AS (PARTITION BY Window.c1)",
                new ParserErrorInfo(54, "SQL46010", "Win1"));

            // 'order by' clause must be used after window in 160
            //
            ParserTestUtils.ErrorTest160("SELECT Sum(c1) OVER Win1 FROM t1 GROUP BY c1 ORDER BY c1 WINDOW Win1 AS (PARTITION BY c1)",
                new ParserErrorInfo(57, "SQL46005", "OFFSET", "WINDOW"));
        }

        /// <summary>
        /// Negative tests for IGNORE/RESPECT NULLS syntax in functions besides LAST_VALUE and FIRST_VALUE.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void IgnoreOrRecpectNullsSyntaxNegativeTest()
        {
            // Cannot use only 'NULLS' with functions besides LEAD in 160
            //
            ParserTestUtils.ErrorTest160("SELECT Name, ListPrice, LEAD(Name) NULLS OVER (ORDER BY ListPrice ASC) AS LeastExpensive",
                new ParserErrorInfo(35, "SQL46010", "NULLS"));

            // Cannot use only 'NULLS' with functions besides LAG in 160
            //
            ParserTestUtils.ErrorTest160("SELECT Name, ListPrice, LAG(Name) NULLS OVER (ORDER BY ListPrice ASC) AS LeastExpensive",
                new ParserErrorInfo(34, "SQL46010", "NULLS"));

            // Only 'NULLS' cannot be used
            //
            ParserTestUtils.ErrorTest160("SELECT Name, ListPrice, FIRST_VALUE(Name) NULLS OVER (ORDER BY ListPrice ASC) AS LeastExpensive",
                new ParserErrorInfo(42, "SQL46010", "NULLS"));

            // Only 'NULLS' cannot be used
            //
            ParserTestUtils.ErrorTest160("SELECT Name, ListPrice, LAST_VALUE(Name) NULLS OVER (ORDER BY ListPrice ASC) AS LeastExpensive",
                new ParserErrorInfo(41, "SQL46010", "NULLS"));
        }

        /// <summary>
        /// Negative tests for IS [NOT] DISTINCT FROM syntax.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void IsNotDistinctFromNegativeTest()
        {
            // IS [NOT] DISTINCT FROM should fail in 150 and below
            //
            const string isDistinctFrom = "SELECT * FROM t1 WHERE c1 IS DISTINCT FROM NULL;";
            const string isNotDistinctFrom = "SELECT * FROM t1 WHERE c1 IS NOT DISTINCT FROM NULL;";

            ParserTestUtils.ErrorTestAllParsersUntil150(isDistinctFrom, new ParserErrorInfo(29, "SQL46010", "DISTINCT"));
            ParserTestUtils.ErrorTest150(isDistinctFrom, new ParserErrorInfo(29, "SQL46010", "DISTINCT"));
            ParserTestUtils.ErrorTestAllParsersUntil150(isNotDistinctFrom, new ParserErrorInfo(33, "SQL46010", "DISTINCT"));
            ParserTestUtils.ErrorTest150(isNotDistinctFrom, new ParserErrorInfo(33, "SQL46010", "DISTINCT"));
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46002Test()
        {
            ParserTestUtils.ErrorTestAllParsers("create table t1 (c1 national int varying);",
                new ParserErrorInfo(20, "SQL46002", SqlDataTypeOption.Int.ToString()));

            ParserTestUtils.ErrorTestAllParsers("CREATE TABLE t1(c1 national aaa varying)",
                new ParserErrorInfo(19, "SQL46003", TSqlParserResource.UserDefined));
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46003Test()
        {
            string testNumber = "SQL46003";

            ParserTestUtils.ErrorTestAllParsers("create table t1 (column1 national real);",
                new ParserErrorInfo(25, testNumber, SqlDataTypeOption.Real.ToString()));

            ParserTestUtils.ErrorTestAllParsers("CREATE TABLE t1(c1 national aaa)",
                new ParserErrorInfo(19, testNumber, TSqlParserResource.UserDefined));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46004Test()
        {
            ParserTestUtils.ErrorTestAllParsers("create table t1 (column1 float varying);",
                new ParserErrorInfo(25, "SQL46004", SqlDataTypeOption.Float.ToString()));

            ParserTestUtils.ErrorTestAllParsers("CREATE TABLE t1(c1 aaa varying)",
                new ParserErrorInfo(23, "SQL46010", "varying"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46005Test()
        {
            string testNumber = "SQL46005";

            ParserTestUtils.ErrorTestAllParsers("create table t1 (column1 float References t2 ON UPDATE NO ACION);",
                new ParserErrorInfo(58, testNumber, CodeGenerationSupporter.Action, "ACION"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46006Test()
        {
            string testNumber = "SQL46006";

            ParserTestUtils.ErrorTestAllParsers(
@"CREATE PROCEDURE p1 @param1 int WITH RECOPILE
AS
CREATE TABLE t1 (int i1)",
                new ParserErrorInfo(37, testNumber, "RECOPILE"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46007Test()
        {
            string testNumber = "SQL46007";

            ParserTestUtils.ErrorTestAllParsers(
@"create trigger trig1 on dbo.employees with encrypton
for insert
as
create table t1 (c1 int);",
              new ParserErrorInfo(43, testNumber, "encrypton"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46008Test()
        {
            string testNumber = "SQL46008";

            ParserTestUtils.ErrorTestAllParsers("create table t1 (c1 int(10));",
                new ParserErrorInfo(20, testNumber, SqlDataTypeOption.Int.ToString()));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46009Test()
        {
            string testNumber = "SQL46009";

            ParserTestUtils.ErrorTestAllParsers("create table t1 (c1 float(20,10));",
                new ParserErrorInfo(20, testNumber, SqlDataTypeOption.Float.ToString()));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46010Test()
        {
            string testNumber = "SQL46010";
            string sql46010TestSyntax = @"create table t1 column1 float varying);
-- tests label having quotes
goto [labelName];
go
CREATE TABLE ...t1 (c1 INT);
go
-- test the paranthesis checking code.
select * from ((t1));";

            ParserTestUtils.ErrorTestAllParsers(
                sql46010TestSyntax,
                new ParserErrorInfo(16, testNumber, "column1"),
                new ParserErrorInfo(sql46010TestSyntax.IndexOf(@"[labelName]"), testNumber, "[labelName]"),
                new ParserErrorInfo(sql46010TestSyntax.IndexOf(@"...t1") + 3, testNumber, "t1"),
                new ParserErrorInfo(sql46010TestSyntax.IndexOf(@"t1));"), testNumber, "t1")
                );
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46011Test()
        {
            string testScript = "create table t1 (c1 int, c2 as -c1 check (c2 < 10));";
            ParserTestUtils.ErrorTest(new TSql80Parser(true), testScript,
                new ParserErrorInfo(35, "SQL46010", "check"));

            ParserTestUtils.ErrorTest90AndAbove(testScript,
                new ParserErrorInfo(35, "SQL46011"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46012Test()
        {
            string testNumber = "SQL46012";

            ParserTestUtils.ErrorTestAllParsers("create table t1 (c1 int Default 23 default 12);",
                new ParserErrorInfo(35, testNumber));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46013Test()
        {
            string testNumber = "SQL46013";

            ParserTestUtils.ErrorTestAllParsers("create table t1 (c1 int Default 23 with values);",
                new ParserErrorInfo(24, testNumber));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46014Test()
        {
            string testNumber = "SQL46014";

            ParserTestUtils.ErrorTestAllParsers("create table t1 (c1 int, constraint cons1 default 23 for c1);",
                new ParserErrorInfo(42, testNumber));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46015Test()
        {
            string testNumber = "SQL46015";

            ParserTestUtils.ErrorTestAllParsers(
                "CREATE Index ind1 ON dbo.t1(c1 desc) WITH encryption",
                new ParserErrorInfo(42, testNumber, "encryption"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46016Test()
        {
            string testNumber = "SQL46016";
            string sql46016TestSyntax = @"CREATE Table t11 (c1 as master...d);
go
READTEXT c1 @ptrval 1 25;
READTEXT t1..c1 @ptrval 1 25";

            ParserTestUtils.ErrorTestAllParsers(
                sql46016TestSyntax,
                new ParserErrorInfo(24, testNumber),
                new ParserErrorInfo(sql46016TestSyntax.IndexOf(@"c1 @ptrval"), testNumber),
                new ParserErrorInfo(sql46016TestSyntax.IndexOf(@"t1..c1"), testNumber));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void ForeignKeyInDeclareTableErrorTest()
        {
            string testScript =
@"declare @t1 Table (c1 int references t2);
declare @t1 Table (c1 int, foreign key references t2);";

            ParserTestUtils.ErrorTestAllParsers(testScript,
                new ParserErrorInfo(26, "SQL46010", "references"),
                new ParserErrorInfo(testScript.IndexOf(@"foreign"), "SQL46010", "foreign"));
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void GraphDbEdgeInDeclareTableError()
        {
            string testScript =
@"declare @t1 table (c1 int, constraint cx connection(n1 to n2)) as edge;
declare @t2 table (c1 int, connection(n1 to n2)) as edge;";

            ParserTestUtils.ErrorTest150(
                testScript,
                new ParserErrorInfo(27, "SQL46010", "constraint"),
                new ParserErrorInfo(testScript.IndexOf(@"to n2", 60), "SQL46010", "to"));
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46018Test()
        {
            string testNumber = "SQL46018";

            ParserTestUtils.ErrorTestAllParsers("create statistics s1 on t1 (c1) with encryption;",
                new ParserErrorInfo(37, testNumber, "encryption"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46019Test()
        {
            string testNumber = "SQL46019";

            ParserTestUtils.ErrorTestAllParsers("create statistics s1 on t1 (c1) with sample 12 encryption;",
                new ParserErrorInfo(47, testNumber, "encryption"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46020Test()
        {
            string testNumber = "SQL46020";

            ParserTestUtils.ErrorTestAllParsers("update statistics s1 with encryption;",
                new ParserErrorInfo(26, testNumber, "encryption"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46021Test()
        {
            string testNumber = "SQL46021";
            string sql46021TestSyntax = @"create default dbName.dbo.r1 as (-10)
GO

create rule dbName.dbo.r1 as @a1 > 10
GO

create trigger dbName..trig1 on dbo.employees
for insert
as
select * from t1;
GO

CREATE PROCEDURE dbName..p1 AS select * from t1
GO";

            ParserTestUtils.ErrorTestAllParsers(
                sql46021TestSyntax,
                new ParserErrorInfo(15, testNumber, CodeGenerationSupporter.Default),
                new ParserErrorInfo(sql46021TestSyntax.IndexOf(@"rule dbName") + 5, testNumber, CodeGenerationSupporter.Rule),
                new ParserErrorInfo(sql46021TestSyntax.IndexOf(@"trigger dbName") + 8, testNumber, CodeGenerationSupporter.Trigger),
                new ParserErrorInfo(sql46021TestSyntax.IndexOf(@"PROCEDURE dbName") + 10, testNumber, CodeGenerationSupporter.Procedure)
                );
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46022Test()
        {
            string testNumber = "SQL46022";

            ParserTestUtils.ErrorTestAllParsers("select c1 from t1 as table1 with (XLOC);",
                new ParserErrorInfo(34, testNumber, "XLOC"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46023Test()
        {
            string testNumber = "SQL46023";

            ParserTestUtils.ErrorTestAllParsers("select * from t1 group by c1, c2 with square",
                new ParserErrorInfo(38, testNumber, "square"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46025Test()
        {
            string testNumber = "SQL46025";

            ParserTestUtils.ErrorTestAllParsers(
@"CREATE VIEW schema1.view1 WITH SCHEMABINDING, RECOMPILE
AS SELECT c1 FROM schema1.table2",
               new ParserErrorInfo(46, testNumber, "RECOMPILE"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46026Test()
        {
            string testNumber = "SQL46026";

            ParserTestUtils.ErrorTestAllParsers(
@"CREATE FUNCTION f1() returns int with something
BEGIN
    return 0
END",
                new ParserErrorInfo(38, testNumber, "something"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46027Test()
        {
            string testNumber = "SQL46027";

            ParserTestUtils.ErrorTestAllParsers("DROP INDEX i1",
                new ParserErrorInfo(11, testNumber, "i1"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46028Test()
        {
            string testNumber = "SQL46028";
            string sql46028TestSyntax = @"select a.b.c.d.*
go
select a.b.c.d.rowguidcol
go
select a.b.c.d.identitycol
go
select a.b.c.d.e.*
go
select * from t where contains(a.b.c.d.e.*, 'foo')";

            ParserTestUtils.ErrorTestAllParsers(
                sql46028TestSyntax,
                new ParserErrorInfo(7, testNumber),
                new ParserErrorInfo(sql46028TestSyntax.IndexOf("a.b.c", 10), testNumber),
                new ParserErrorInfo(sql46028TestSyntax.IndexOf("a.b.c", 40), testNumber),
                new ParserErrorInfo(sql46028TestSyntax.IndexOf("a.b.c", 70), testNumber),
                new ParserErrorInfo(sql46028TestSyntax.IndexOf("a.b.c", 100), testNumber)
                );
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46029Test()
        {
            string testNumber = "SQL46029";

            ParserTestUtils.ErrorTestAllParsers("create table",
                new ParserErrorInfo(12, testNumber));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46049Test()
        {
            //Function Execute AS
            //
            string testScript = "create function f1() returns int with execute as caller, execute as owner begin break end";
            ParserTestUtils.ErrorTest90AndAbove(testScript, new ParserErrorInfo(57, "SQL46049", "execute"));

            string createFunction = "create function udf() returns int with schemabinding, schemabinding as begin return 1 end";
            ParserTestUtils.ErrorTestAllParsers(createFunction, new ParserErrorInfo(54, "SQL46049", "schemabinding"));

            //Procedure Execute AS
            //
            string createProcedureExecuteAs = "create proc p1 with execute as caller, execute as owner begin break end";
            ParserTestUtils.ErrorTest90AndAbove(createProcedureExecuteAs, new ParserErrorInfo(39, "SQL46049", "execute"));

            string createProcedure = "create proc p1 with encryption, encryption begin break end";
            ParserTestUtils.ErrorTestAllParsers(createProcedure, new ParserErrorInfo(32, "SQL46049", "encryption"));

            //Begin Dialog Conversation
            //
            string beginDialogConversation = @"BEGIN DIALOG CONVERSATION @dialog_handle
   FROM SERVICE [//Adventure-Works.com/ExpenseClient]
   TO SERVICE '//Adventure-Works.com/Expenses'
   ON CONTRACT [//Adventure-Works.com/Expenses/ExpenseSubmission]
   WITH LIFETIME=60, ENCRYPTION=ON";
            ParserTestUtils.ErrorTest90AndAbove(beginDialogConversation + ", LIFETIME=10;", new ParserErrorInfo(beginDialogConversation.Length + 2, "SQL46049", "LIFETIME"));
            ParserTestUtils.ErrorTest90AndAbove(beginDialogConversation + ", RELATED_CONVERSATION_GROUP = @existing_conversation_handle, RELATED_CONVERSATION = @existing_conversation_handle ;", new ParserErrorInfo(beginDialogConversation.Length + 62, "SQL46049", "RELATED_CONVERSATION"));
            ParserTestUtils.ErrorTest90AndAbove(beginDialogConversation + ", ENCRYPTION=OFF;", new ParserErrorInfo(beginDialogConversation.Length + 2, "SQL46049", "ENCRYPTION"));

            //Create Database
            //
            string createDatabase = "create database foo with trustworthy on, trustworthy off";
            ParserTestUtils.ErrorTest90AndAbove(createDatabase, new ParserErrorInfo(41, "SQL46049", "trustworthy"));

            //Alter Assembly
            //
            string alterAssembly = "alter assembly foo with visibility=on, permission_set=unsafe, unchecked data";
            ParserTestUtils.ErrorTest90AndAbove(alterAssembly + ", visibility=off", new ParserErrorInfo(78, "SQL46049", "visibility"));
            ParserTestUtils.ErrorTest90AndAbove(alterAssembly + ", permission_set=safe", new ParserErrorInfo(78, "SQL46049", "permission_set"));
            ParserTestUtils.ErrorTest90AndAbove(alterAssembly + ", unchecked data", new ParserErrorInfo(78, "SQL46049", "unchecked"));

            //Bulk Insert
            //
            string bulkInsert = @"BULK INSERT AdventureWorks2008R2.Sales.SalesOrderDetail
   FROM 'f:\orders\lineitem.tbl'
   WITH
      (
         FIELDTERMINATOR =' |', FIELDTERMINATOR =' |',
         ROWTERMINATOR =' |\n'
      )";
            ParserTestUtils.ErrorTestAllParsers(bulkInsert, new ParserErrorInfo(bulkInsert.IndexOf("FIELDTERMINATOR", 135), "SQL46049", "FIELDTERMINATOR"));

            string alterDatabase = @"ALTER DATABASE AdventureWorks2008R2
ADD FILE
(
    NAME = Test1dat2,
    FILENAME = 'C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\t1dat2.ndf',
    SIZE = 5MB,
    MAXSIZE = 100MB,
    SIZE = 10MB,
    FILEGROWTH = 5MB
);";
            ParserTestUtils.ErrorTestAllParsers(alterDatabase, new ParserErrorInfo(alterDatabase.IndexOf("SIZE", 205), "SQL46049", "SIZE"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46035Test()
        {
            ParserTestUtils.ErrorTestAllParsers("select * from { oj t1 cross join t2 }",
                new ParserErrorInfo(14, "SQL46035"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46036Test()
        {
            ParserTestUtils.ErrorTestAllParsers(@"select * from --(* vendor(microsoft),product(odbc) oj t1 cross join t2 *)--",
                new ParserErrorInfo(14, "SQL46036"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46038Test()
        {
            ParserTestUtils.ErrorTestAllParsers("drop statistics s1",
                new ParserErrorInfo(16, "SQL46038"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46039Test()
        {
            ParserTestUtils.ErrorTestAllParsers("create function f1(@p int output) returns int as begin return 1; end",
                new ParserErrorInfo(26, "SQL46039"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46040Test()
        {
            TSql100Parser parser = new TSql100Parser(true);
            ParserTestUtils.ErrorTest(parser,
@"MERGE pi USING (SELECT * FROM t1) AS src ON (pi.ProductID = src.ProductID) WHEN MATCHED THEN INSERT DEFAULT VALUES;",
                new ParserErrorInfo(93, "SQL46040"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46041Test()
        {
            TSql100Parser parser = new TSql100Parser(true);
            ParserTestUtils.ErrorTest(parser,
@"MERGE pi USING (SELECT * FROM t1) AS src ON (pi.ProductID = src.ProductID) WHEN NOT MATCHED THEN DELETE;",
                new ParserErrorInfo(97, "SQL46041", "Delete"));
            ParserTestUtils.ErrorTest(parser,
@"MERGE pi USING (SELECT * FROM t1) AS src ON (pi.ProductID = src.ProductID) WHEN NOT MATCHED THEN UPDATE SET pi.Q = 10;",
                new ParserErrorInfo(97, "SQL46041", "Update"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46042Test()
        {
            ParserTestUtils.ErrorTestAllParsers(
                "CREATE TABLE t1(c1 int rowguidcol rowguidcol)",
                new ParserErrorInfo(34, "SQL46042"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46043Test()
        {
            ParserTestUtils.ErrorTestAllParsers(
                "CREATE TABLE t1(c1 int identity identity)",
                new ParserErrorInfo(32, "SQL46043"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46047Test()
        {
            ParserTestUtils.ErrorTestAllParsers(
                "CREATE VIEW v1 AS SELECT c1 FROM t1 ORDER BY c1",
                new ParserErrorInfo(18, "SQL46047"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46048Test()
        {
            ParserTestUtils.ErrorTestAllParsers(
                "CREATE VIEW v1 AS SELECT TOP 2 WITH TIES c1 FROM t1",
                new ParserErrorInfo(18, "SQL46048"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46050Test()
        {
            ParserTestUtils.ErrorTest100(
                "ALTER DATABASE db1 SET CHANGE_TRACKING (AUTO_CLEANUP = ON, AUTO_CLEANUP = OFF)",
                new ParserErrorInfo(59, "SQL46050", "AUTO_CLEANUP"));
            ParserTestUtils.ErrorTest100(
                "ALTER DATABASE db1 SET CHANGE_TRACKING (CHANGE_RETENTION = 1 DAYS, AUTO_CLEANUP = ON, CHANGE_RETENTION = 2 MINUTES)",
                new ParserErrorInfo(86, "SQL46050", "CHANGE_RETENTION"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46058Test()
        {
            TSql100Parser parser100 = new TSql100Parser(true);

            ParserTestUtils.ErrorTest(parser100, "CREATE TABLE t1(c1 INT) FILESTREAM_ON aaa FILESTREAM_ON 'default'",
                new ParserErrorInfo(42, "SQL46058", "FILESTREAM_ON"));
            ParserTestUtils.ErrorTest(parser100, "CREATE TABLE t1(c1 INT) TEXTIMAGE_ON aaa TEXTIMAGE_ON 'default'",
                new ParserErrorInfo(41, "SQL46058", "TEXTIMAGE_ON"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46059Test()
        {
            ParserTestUtils.ErrorTest100("CREATE INDEX i1 ON t1(c1) WHERE c1 < CONVERT(int, CONVERT(int, 20))",
                new ParserErrorInfo(50, "SQL46059"));
            ParserTestUtils.ErrorTest100("CREATE INDEX i1 ON t1(c1) WHERE c1 < CAST(CAST (10 AS int) AS int)",
                new ParserErrorInfo(42, "SQL46059"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46060Test()
        {
            ParserTestUtils.ErrorTestAllParsers("CREATE TABLE t1(c1 INT PRIMARY KEY WITH FILLFACTOR = 105)",
                new ParserErrorInfo(53, "SQL46060", "105"));
            ParserTestUtils.ErrorTestAllParsers("CREATE TABLE t1(c1 INT, UNIQUE(c1) WITH FILLFACTOR = 0)",
                new ParserErrorInfo(53, "SQL46060", "0"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46061Test()
        {
            TSql100Parser parser100 = new TSql100Parser(true);

            // Error, if no partition specified in the statements, but partition specified in DATA_COMPRESSION option
            ParserTestUtils.ErrorTest(parser100, "ALTER INDEX i1 ON t1 REBUILD WITH (DATA_COMPRESSION = ROW ON PARTITIONS (1))",
                new ParserErrorInfo(35, "SQL46061"));
            ParserTestUtils.ErrorTest(parser100, "ALTER TABLE t1 REBUILD WITH (DATA_COMPRESSION = ROW ON PARTITIONS (1))",
                new ParserErrorInfo(29, "SQL46061"));

            // If partition specified in the statement, no errors...
            ParserTestUtils.ErrorTest(parser100, "ALTER INDEX i1 ON t1 REBUILD PARTITION = 1 WITH (DATA_COMPRESSION = ROW ON PARTITIONS (1))");
            ParserTestUtils.ErrorTest(parser100, "ALTER TABLE t1 REBUILD PARTITION = 2 WITH (DATA_COMPRESSION = ROW ON PARTITIONS (1))");
            ParserTestUtils.ErrorTest(parser100, "ALTER INDEX i1 ON t1 REBUILD PARTITION = ALL WITH (DATA_COMPRESSION = ROW ON PARTITIONS (1))");
            ParserTestUtils.ErrorTest(parser100, "ALTER TABLE t1 REBUILD PARTITION = ALL WITH (DATA_COMPRESSION = ROW ON PARTITIONS (1))");
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46061Test_160()
        {
            TSql160Parser parser160 = new TSql160Parser(true);

            // Error, if no partition specified in the statements, but partition specified in XML_COMPRESSION option
            ParserTestUtils.ErrorTest(parser160, "ALTER INDEX i1 ON t1 REBUILD WITH (XML_COMPRESSION = ON ON PARTITIONS (1))",
                new ParserErrorInfo(35, "SQL46061"));
            ParserTestUtils.ErrorTest(parser160, "ALTER TABLE t1 REBUILD WITH (XML_COMPRESSION = ON ON PARTITIONS (1))",
                new ParserErrorInfo(29, "SQL46061"));

            // If partition specified in the statement, no errors...
            ParserTestUtils.ErrorTest(parser160, "ALTER INDEX i1 ON t1 REBUILD PARTITION = 1 WITH (XML_COMPRESSION = ON ON PARTITIONS (1))");
            ParserTestUtils.ErrorTest(parser160, "ALTER TABLE t1 REBUILD PARTITION = 2 WITH (XML_COMPRESSION = ON ON PARTITIONS (1))");
            ParserTestUtils.ErrorTest(parser160, "ALTER INDEX i1 ON t1 REBUILD PARTITION = ALL WITH (XML_COMPRESSION = ON ON PARTITIONS (1))");
            ParserTestUtils.ErrorTest(parser160, "ALTER TABLE t1 REBUILD PARTITION = ALL WITH (XML_COMPRESSION = ON ON PARTITIONS (1))");
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46062Test()
        {
            ParserTestUtils.ErrorTestAllParsers("CREATE DATABASE db1 ON (NAME=n1, NEWNAME=n2, FILENAME='zzz')",
                new ParserErrorInfo(33, "SQL46062"));
            ParserTestUtils.ErrorTestAllParsers("CREATE DATABASE db1 LOG ON (NAME=n1, NEWNAME=n2, FILENAME='zzz')",
                new ParserErrorInfo(37, "SQL46062"));
            ParserTestUtils.ErrorTestAllParsers("CREATE DATABASE db1 ON (NAME=n1, FILENAME='zzz'), FILEGROUP fg1 (NAME=n1, NEWNAME=n2, FILENAME='zzz')",
                new ParserErrorInfo(74, "SQL46062"));
            ParserTestUtils.ErrorTestAllParsers("ALTER DATABASE db1 ADD FILE(NAME=n1, NEWNAME=n2, FILENAME='zzz')",
                new ParserErrorInfo(37, "SQL46062"));

            ParserTestUtils.ErrorTest90AndAbove("ALTER DATABASE db1 REBUILD LOG ON (NAME=n1, NEWNAME=n2, FILENAME='zzz')",
                new ParserErrorInfo(44, "SQL46062"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46063Test()
        {
            // Empty strings in file options are not allowed
            ParserTestUtils.ErrorTestAllParsers("ALTER DATABASE db1 MODIFY FILE (NAME='')",
                new ParserErrorInfo(37, "SQL46063"));
            ParserTestUtils.ErrorTestAllParsers("ALTER DATABASE db1 MODIFY FILE (NAME=N'')",
                new ParserErrorInfo(37, "SQL46063"));
            ParserTestUtils.ErrorTestAllParsers("ALTER DATABASE db1 MODIFY FILE (NEWNAME='')",
                new ParserErrorInfo(40, "SQL46063"));
            ParserTestUtils.ErrorTestAllParsers("ALTER DATABASE db1 MODIFY FILE (NEWNAME=N'')",
                new ParserErrorInfo(40, "SQL46063"));
            ParserTestUtils.ErrorTestAllParsers("ALTER DATABASE db1 MODIFY FILE (FILENAME='')",
                new ParserErrorInfo(41, "SQL46063"));
            ParserTestUtils.ErrorTestAllParsers("ALTER DATABASE db1 MODIFY FILE (FILENAME=N'')",
                new ParserErrorInfo(41, "SQL46063"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46065Test()
        {
            // Missing FILENAME option should cause an error for
            // all parsers until 150, where this argument is optional and used
            // by SQL Managed Instance...
            //
            ParserTestUtils.ErrorTestAllParsersUntil150("CREATE DATABASE db1 ON (NAME=n1)",
                new ParserErrorInfo(23, "SQL46065"));
            ParserTestUtils.ErrorTestAllParsersUntil150("CREATE DATABASE db1 LOG ON (NAME=n1)",
                new ParserErrorInfo(27, "SQL46065"));
            ParserTestUtils.ErrorTestAllParsersUntil150("CREATE DATABASE db1 ON (NAME=n1, FILENAME='zzz'), FILEGROUP fg1 (NAME=n1)",
                new ParserErrorInfo(64, "SQL46065"));
            ParserTestUtils.ErrorTestAllParsersUntil150("ALTER DATABASE db1 ADD FILE(NAME=n1)",
                new ParserErrorInfo(27, "SQL46065"));

            ParserTestUtils.ErrorTestAllParsers("ALTER DATABASE db1 ADD FILE(NAME=n1, FILENAME='')",
                new ParserErrorInfo(46, "SQL46063"));

            ParserTestUtils.ErrorTestAllParsers("ALTER DATABASE db1 ADD FILE(NAME=n1, FILENAME= N'')",
                 new ParserErrorInfo(47, "SQL46063"));

            ParserTestUtils.ErrorTest90andAboveUntil150("ALTER DATABASE db1 REBUILD LOG ON (NAME=n1)",
                new ParserErrorInfo(34, "SQL46065"));

            // ... except in ALTER DATABASE MODIFY FILE STATEMENT!
            ParserTestUtils.ErrorTestAllParsers("ALTER DATABASE db1 MODIFY FILE (NAME=n1, NEWNAME=n2)");
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void IndexFilterErrorTests()
        {
            // Filter clause is not allowed on CLUSTERED index
            ParserTestUtils.ErrorTest100("CREATE CLUSTERED INDEX i1 ON t1(c1) WHERE c1 < 10",
                new ParserErrorInfo(36, "SQL46010", "WHERE"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void IntoClauseErrorTests()
        {
            ParserTestUtils.ErrorTestAllParsers("insert t1 select c1 into t2",
                new ParserErrorInfo(20, "SQL46010", "into"));
            ParserTestUtils.ErrorTestAllParsers("select c1 from t1 union select c1 into t4 from t1;",
                new ParserErrorInfo(34, "SQL46010", "into"));
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void OnClauseErrorTests()
        {
            ParserTestUtils.ErrorTestAllParsers("select c1 into t1 from t1 on fg;",
                new ParserErrorInfo(26, "SQL46010", "on"));
            ParserTestUtils.ErrorTestAllParsers("select c1 from t1 on fg;",
                new ParserErrorInfo(18, "SQL46010", "on"));
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void UnclosedTokenTest()
        {
            ParserTestUtils.ErrorTestAllParsers("create table t1(c1 int) 'zz",
                new ParserErrorInfo(24, "SQL46030", "'zz"));
            ParserTestUtils.ErrorTestAllParsers("create table t1(c1 int) N'zz",
                new ParserErrorInfo(24, "SQL46030", "N'zz"));
            ParserTestUtils.ErrorTestAllParsers("create table t1(c int) /* aaa",
                new ParserErrorInfo(29, "SQL46032"));
            ParserTestUtils.ErrorTestAllParsers("create /* \r\nz",
                new ParserErrorInfo(13, "SQL46032"));
            ParserTestUtils.ErrorTestAllParsers("create table [tableName",
                new ParserErrorInfo(13, "SQL46031", "[tableName"));
            ParserTestUtils.ErrorTestAllParsers("create table $(tableName",
                new ParserErrorInfo(13, "SQL46033", "$(tableName"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TokenStreamRecognitionExceptionTest()
        {
            ParserTestUtils.ErrorTestAllParsers(
@"Create Table dbo.Table1(keyid nchar(10) Not Null ]    on [Primary];",
                  new ParserErrorInfo(49, "SQL46010", "]"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void IPAddressErrorTest()
        {
            // T-SQL 80 doesn't have IPs!

            // Checking re-parsing of real token for IP creation purposes
            ParserTestUtils.ErrorTest90AndAbove("create endpoint e1 state = stopped as tcp(listener_ip = (1.2e1 .3.4)) for tsql()",
                new ParserErrorInfo(57, "SQL46010", "1.2e1"));

            ParserTestUtils.ErrorTest90AndAbove("create endpoint e1 state = stopped as tcp(listener_ip = (1.2 3.4 5.6)) for tsql()",
                new ParserErrorInfo(61, "SQL46010", "3.4"));

            ParserTestUtils.ErrorTest90AndAbove("create endpoint e1 state = stopped as tcp(listener_ip = (1.2 3 . 4 .5)) for tsql()",
                new ParserErrorInfo(57, "SQL46010", "1.2"));

            ParserTestUtils.ErrorTest90AndAbove("create endpoint e1 state = stopped as tcp(listener_ip = (1.2 .3 4.)) for tsql()",
                new ParserErrorInfo(64, "SQL46010", "4."));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void EmptyParserInputTest()
        {
            ParserTestUtils.ErrorTestAllParsers(string.Empty);
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void QuotedIdentifierErrorTest()
        {
            // Check, that tokens like "aaa" are treated as ascii literals if quoted identifiers are off
            const string testScript = "CREATE DATABASE Sales ON (NAME = A, FILENAME = \"aaa\")";
            ParserTestUtils.ErrorTestAllParsers("set quoted_identifier off; " + testScript);

            ParserTestUtils.ErrorTestAllParsers("set quoted_identifier on; " + testScript,
                new ParserErrorInfo(73, "SQL46010", "\"aaa\""));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void QuotedIdentfierPerBatchResetTest()
        {
            string SwitchingOffScript =
"CREATE TABLE \"t1\"(c1 int);\n\r GO SET QUOTED_IDENTIFIER OFF; CREATE TABLE \"t2\"(c1 int);";
            ParserTestUtils.ExecuteTestForAllParsers(delegate (TSqlParser parser)
            {
                ParserTestUtils.ErrorTest(parser, SwitchingOffScript,
                    new ParserErrorInfo(72, "SQL46010", "\"t2\""));
            }, true);

            string SwitchingOnScript =
"SET QUOTED_IDENTIFIER ON; CREATE TABLE \"t1\"(c1 int);\n\r GO CREATE TABLE \"t2\"(c1 int)";

            ParserTestUtils.ExecuteTestForAllParsers(delegate (TSqlParser parser)
            {
                ParserTestUtils.ErrorTest(parser, SwitchingOnScript,
                    new ParserErrorInfo(71, "SQL46010", "\"t2\""));
            }, false);
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void ProcNameSemiColonErrorTest()
        {
            ParserTestUtils.ErrorTestAllParsers(
@"exec p1 ;
    2",
                new ParserErrorInfo(8, "SQL46010", ";"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void RequiredSemiColonTest()
        {
            ParserTestUtils.ErrorTest100AndAbove(
@"MERGE a USING b
ON (a.ProductID = b.ProductID)
WHEN MATCHED
    THEN UPDATE SET pi.Quantity = src.OrderQty",
                new ParserErrorInfo(0, "SQL46097", "MERGE"));

            ParserTestUtils.ErrorTest100AndAbove(
@"create fulltext stoplist a
select 1",
                new ParserErrorInfo(25, "SQL46097", "FullText Stoplist"));
            ParserTestUtils.ErrorTest100AndAbove(
@"drop fulltext stoplist a
--comment",
                new ParserErrorInfo(23, "SQL46097", "FullText Stoplist"));

            ParserTestUtils.ErrorTest100AndAbove(
@"alter fulltext stoplist a add 'stopword' language 1033",
    new ParserErrorInfo(24, "SQL46097", "FullText Stoplist"));

            ParserTestUtils.ErrorTest110(
@"create search property list list1",
    new ParserErrorInfo(28, "SQL46097", "Search Property List"));

            ParserTestUtils.ErrorTest110(
@"ALTER SEARCH PROPERTY LIST foo DROP 'bax'",
    new ParserErrorInfo(27, "SQL46097", "Search Property List"));

            ParserTestUtils.ErrorTest110(
@"drop search property list list1",
 new ParserErrorInfo(26, "SQL46097", "Search Property List"));

        }



        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void CreateEventNotificationStatementErrorTest()
        {
            // T-SQL 80 doesn't have CREATE EVENT NOTIFICATION statement
            // Check, that invalid event types and event groups are caught
            ParserTestUtils.ErrorTest90AndAbove("CREATE EVENT NOTIFICATION log_ddl1 ON SERVER FOR BLAH_BLAH TO SERVICE 'NotifyService', '8140';",
                new ParserErrorInfo(49, "SQL46010", "BLAH_BLAH"));
            ParserTestUtils.ErrorTest90AndAbove("CREATE EVENT NOTIFICATION log_ddl1 ON SERVER FOR BLAH_BLAH_BLAH TO SERVICE 'NotifyService', '8140';",
                new ParserErrorInfo(49, "SQL46010", "BLAH_BLAH_BLAH"));
            ParserTestUtils.ErrorTest90AndAbove("CREATE EVENT NOTIFICATION log_ddl1 ON SERVER FOR OBJECT_CREATED,BLAH_BLAH TO SERVICE 'NotifyService', '8140';",
                new ParserErrorInfo(64, "SQL46010", "BLAH_BLAH"));
            ParserTestUtils.ErrorTest90AndAbove("CREATE EVENT NOTIFICATION log_ddl1 ON SERVER FOR DDL_INDEX_EVENTS,BLAH_BLAH TO SERVICE 'NotifyService', '8140';",
                new ParserErrorInfo(66, "SQL46010", "BLAH_BLAH"));
            ParserTestUtils.ErrorTest90AndAbove("CREATE EVENT NOTIFICATION log_ddl1 ON SERVER FOR BLAH_BLAH,ALTER_CERTFCATE TO SERVICE 'NotifyService', '8140';",
                new ParserErrorInfo(49, "SQL46010", "BLAH_BLAH"));
            ParserTestUtils.ErrorTest90AndAbove("CREATE EVENT NOTIFICATION log_ddl1 ON SERVER FOR TO SERVICE 'NotifyService', '8140';",
                new ParserErrorInfo(49, "SQL46010", "TO"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void CreateLogonTriggerStatementErrorTest()
        {
            ParserTestUtils.ErrorTest90AndAbove("create trigger trig1 on database for logon as Create Table t1 (int i1);",
                new ParserErrorInfo(37, "SQL46044"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void CreateTriggerStatementWithUnkownEventsErrorTest()
        {
            ParserTestUtils.ErrorTest90AndAbove("create trigger trig1 on database for blah_blah as Create Table t1 (int i1);",
                new ParserErrorInfo(37, "SQL46010", "blah_blah"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void CreateResourcePoolStatementErrorTest()
        {
            ParserTestUtils.ErrorTest100("create resource pool res_pool with (MIN_CPU_PERCENT = 105);",
                  new ParserErrorInfo(36, "SQL46045", "MIN_CPU_PERCENT"));
            ParserTestUtils.ErrorTest100("create resource pool res_pool with (MAX_MEMORY_PERCENT = 0);",
                  new ParserErrorInfo(36, "SQL46045", "MAX_MEMORY_PERCENT"));
            ParserTestUtils.ErrorTest100("create resource pool res_pool with ;",
                new ParserErrorInfo(30, "SQL46010", "with"));
            ParserTestUtils.ErrorTest100("create resource pool res_pool with ( ;",
                new ParserErrorInfo(37, "SQL46010", ";"));
            ParserTestUtils.ErrorTest100("create resource pool res_pool with ) ;",
              new ParserErrorInfo(30, "SQL46010", "with"));
            ParserTestUtils.ErrorTest100("create resource pool res_pool with (MIN_CPU_PERCENT=20,);",
                new ParserErrorInfo(55, "SQL46010", ")"));
            ParserTestUtils.ErrorTest100("alter resource pool res_pool with (,MIN_CPU_PERCENT=20);",
                new ParserErrorInfo(35, "SQL46010", ","));
            ParserTestUtils.ErrorTest100("create resource pool res_pool with MIN_MEMORY_PERCENT=20, MAX_CPU_PERCENT=30;",
                new ParserErrorInfo(35, "SQL46010", "MIN_MEMORY_PERCENT"));
            ParserTestUtils.ErrorTest100("alter resource pool res_pool with (MIN_CPU_PERCENT=20),(MAX_CPU_PERCENT=30);",
                new ParserErrorInfo(54, "SQL46010", ","));
            ParserTestUtils.ErrorTest100("create resource pool res_pool with MIN_CPU_PERCENT ;",
                new ParserErrorInfo(35, "SQL46010", "MIN_CPU_PERCENT"));
            ParserTestUtils.ErrorTest100("alter resource pool res_pool with (blah_blah = 20);",
                new ParserErrorInfo(35, "SQL46010", "blah_blah"));
            ParserTestUtils.ErrorTest100("alter resource pool res_pool with (MIN_CPU_PERCENT = ab);",
                new ParserErrorInfo(53, "SQL46010", "ab"));

            ParserTestUtils.ErrorTest120("alter resource pool res_pool with (MIN_IOPS_PER_VOLUME = ab);",
                new ParserErrorInfo(57, "SQL46010", "ab"));
            ParserTestUtils.ErrorTest120("alter resource pool res_pool with (MAX_IOPS_PER_VOLUME = ab);",
                new ParserErrorInfo(57, "SQL46010", "ab"));
            ParserTestUtils.ErrorTest120("alter resource pool res_pool with (MIN_IOPS_PER_VOLUME = -2);",
                new ParserErrorInfo(57, "SQL46010", "-"));
            ParserTestUtils.ErrorTest120("alter resource pool res_pool with (MAX_IOPS_PER_VOLUME = -2);",
                new ParserErrorInfo(57, "SQL46010", "-"));
        }


        [TestMethod]
        [Priority(0)]
        public void CreateExternalResourcePoolStatementErrorTest()
        {
            ParserTestUtils.ErrorTest130("create external resource pool res_pool with (MAX_MEMORY_PERCENT = 0);",
                new ParserErrorInfo(45, "SQL46045", "MAX_MEMORY_PERCENT"));
            ParserTestUtils.ErrorTest130("create external resource pool res_pool with (MAX_MEMORY_PERCENT = 101);",
                new ParserErrorInfo(45, "SQL46045", "MAX_MEMORY_PERCENT"));
            ParserTestUtils.ErrorTest130("create external resource pool res_pool with (MAX_CPU_PERCENT = 0);",
                new ParserErrorInfo(45, "SQL46045", "MAX_CPU_PERCENT"));
            ParserTestUtils.ErrorTest130("create external resource pool res_pool with (MAX_CPU_PERCENT = 101);",
                new ParserErrorInfo(45, "SQL46045", "MAX_CPU_PERCENT"));
            ParserTestUtils.ErrorTest130("create external resource pool res_pool with (MAX_PROCESSES = -1);",
                new ParserErrorInfo(61, "SQL46010", "-"));

            ParserTestUtils.ErrorTest130("create external resource pool res_pool with (blah_blah = 20);",
                new ParserErrorInfo(45, "SQL46010", "blah_blah"));

            ParserTestUtils.ErrorTest130("create external resource pool res_pool with (MAX_CPU_PERCENT=20,);",
                new ParserErrorInfo(64, "SQL46010", ")"));
            ParserTestUtils.ErrorTest130("create external resource pool res_pool with (,MAX_CPU_PERCENT=20);",
                new ParserErrorInfo(45, "SQL46010", ","));
            ParserTestUtils.ErrorTest130("create external resource pool res_pool with ;",
                new ParserErrorInfo(39, "SQL46010", "with"));
            ParserTestUtils.ErrorTest130("create external resource pool res_pool with ( ;",
                new ParserErrorInfo(46, "SQL46010", ";"));
            ParserTestUtils.ErrorTest130("create external resource pool res_pool with ) ;",
                new ParserErrorInfo(39, "SQL46010", "with"));

            ParserTestUtils.ErrorTest130("create external resource pool res_pool with MAX_MEMORY_PERCENT=20, MAX_CPU_PERCENT=30;",
                new ParserErrorInfo(44, "SQL46010", "MAX_MEMORY_PERCENT"));
            ParserTestUtils.ErrorTest130("create external resource pool res_pool with (MAX_MEMORY_PERCENT=20),(MAX_CPU_PERCENT=30);",
                new ParserErrorInfo(67, "SQL46010", ","));
            ParserTestUtils.ErrorTest130("create external resource pool res_pool with MAX_PROCESSES ;",
                new ParserErrorInfo(44, "SQL46010", "MAX_PROCESSES"));

            ParserTestUtils.ErrorTest130("create external resource pool res_pool with (MAX_PROCESSES = ab);",
                new ParserErrorInfo(61, "SQL46010", "ab"));

            ParserTestUtils.ErrorTest130("create external resource pool res_pool with (MAX_MEMORY_PERCENT = 20, MAX_CPU_PERCENT = 30, MAX_MEMORY_PERCENT = 20);",
                new ParserErrorInfo(92, "SQL46049", "MAX_MEMORY_PERCENT"));
            ParserTestUtils.ErrorTest130("create external resource pool res_pool with (MAX_CPU_PERCENT = 20, MAX_MEMORY_PERCENT = 30, MAX_CPU_PERCENT = 20);",
                new ParserErrorInfo(92, "SQL46049", "MAX_CPU_PERCENT"));
            ParserTestUtils.ErrorTest130("create external resource pool res_pool with (MAX_PROCESSES = 20, MAX_MEMORY_PERCENT = 30, MAX_PROCESSES = 20);",
                new ParserErrorInfo(90, "SQL46049", "MAX_PROCESSES"));

            ParserTestUtils.ErrorTest130("create external resource pool res_pool with (AFFINITY CPU = AUTO, AFFINITY CPU = AUTO);",
                new ParserErrorInfo(66, "SQL46049", "AFFINITY"));
            ParserTestUtils.ErrorTest130("create external resource pool res_pool with (AFFINITY CPU = AUTO, AFFINITY CPU = (1));",
                new ParserErrorInfo(66, "SQL46049", "AFFINITY"));
            ParserTestUtils.ErrorTest130("create external resource pool res_pool with (AFFINITY CPU = AUTO, CPU = AUTO);",
                new ParserErrorInfo(66, "SQL46010", "CPU"));
            ParserTestUtils.ErrorTest130("create external resource pool res_pool with (AFFINITY CPU = AUTO, CPU = (1));",
                new ParserErrorInfo(66, "SQL46010", "CPU"));
            ParserTestUtils.ErrorTest130("create external resource pool res_pool with (AFFINITY CPU = AUTO, AFFINITY NUMANODE = (1));",
                new ParserErrorInfo(66, "SQL46049", "AFFINITY"));
            ParserTestUtils.ErrorTest130("create external resource pool res_pool with (AFFINITY CPU = blah);",
                new ParserErrorInfo(60, "SQL46005", "AUTO", "blah"));
            ParserTestUtils.ErrorTest130("create external resource pool res_pool with (AFFINITY NUMANODE = AUTO);",
                new ParserErrorInfo(65, "SQL46010", "AUTO"));
            ParserTestUtils.ErrorTest130("create external resource pool res_pool with (AFFINITY NUMANODE = (2), AFFINITY NUMANODE = (1));",
                new ParserErrorInfo(70, "SQL46049", "AFFINITY"));
            ParserTestUtils.ErrorTest130("create external resource pool res_pool with (AFFINITY CPU = (a));",
                new ParserErrorInfo(61, "SQL46010", "a"));
            ParserTestUtils.ErrorTest130("create external resource pool res_pool with (AFFINITY NUMANODE = (a));",
                new ParserErrorInfo(66, "SQL46010", "a"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void AlterExternalResourcePoolStatementErrorTest()
        {
            ParserTestUtils.ErrorTest130("alter external resource pool res_pool with (MAX_MEMORY_PERCENT = 0);",
                new ParserErrorInfo(44, "SQL46045", "MAX_MEMORY_PERCENT"));
            ParserTestUtils.ErrorTest130("alter external resource pool res_pool with (MAX_MEMORY_PERCENT = 101);",
                new ParserErrorInfo(44, "SQL46045", "MAX_MEMORY_PERCENT"));
            ParserTestUtils.ErrorTest130("alter external resource pool res_pool with (MAX_CPU_PERCENT = 0);",
                new ParserErrorInfo(44, "SQL46045", "MAX_CPU_PERCENT"));
            ParserTestUtils.ErrorTest130("alter external resource pool res_pool with (MAX_CPU_PERCENT = 101);",
                new ParserErrorInfo(44, "SQL46045", "MAX_CPU_PERCENT"));
            ParserTestUtils.ErrorTest130("alter external resource pool res_pool with (MAX_PROCESSES = -1);",
                new ParserErrorInfo(60, "SQL46010", "-"));

            ParserTestUtils.ErrorTest130("alter external resource pool res_pool with (blah_blah = 20);",
                new ParserErrorInfo(44, "SQL46010", "blah_blah"));

            ParserTestUtils.ErrorTest130("alter external resource pool res_pool with (MAX_CPU_PERCENT=20,);",
                new ParserErrorInfo(63, "SQL46010", ")"));
            ParserTestUtils.ErrorTest130("alter external resource pool res_pool with (,MAX_CPU_PERCENT=20);",
                new ParserErrorInfo(44, "SQL46010", ","));
            ParserTestUtils.ErrorTest130("alter external resource pool res_pool with ;",
                new ParserErrorInfo(38, "SQL46010", "with"));
            ParserTestUtils.ErrorTest130("alter external resource pool res_pool with ( ;",
                new ParserErrorInfo(45, "SQL46010", ";"));
            ParserTestUtils.ErrorTest130("alter external resource pool res_pool with ) ;",
                new ParserErrorInfo(38, "SQL46010", "with"));

            ParserTestUtils.ErrorTest130("alter external resource pool res_pool with MAX_MEMORY_PERCENT=20, MAX_CPU_PERCENT=30;",
                new ParserErrorInfo(43, "SQL46010", "MAX_MEMORY_PERCENT"));
            ParserTestUtils.ErrorTest130("alter external resource pool res_pool with (MAX_MEMORY_PERCENT=20),(MAX_CPU_PERCENT=30);",
                new ParserErrorInfo(66, "SQL46010", ","));
            ParserTestUtils.ErrorTest130("alter external resource pool res_pool with MAX_PROCESSES ;",
                new ParserErrorInfo(43, "SQL46010", "MAX_PROCESSES"));

            ParserTestUtils.ErrorTest130("alter external resource pool res_pool with (MAX_PROCESSES = ab);",
                new ParserErrorInfo(60, "SQL46010", "ab"));

            ParserTestUtils.ErrorTest130("alter external resource pool res_pool with (MAX_MEMORY_PERCENT = 20, MAX_CPU_PERCENT = 30, MAX_MEMORY_PERCENT = 20);",
                new ParserErrorInfo(91, "SQL46049", "MAX_MEMORY_PERCENT"));
            ParserTestUtils.ErrorTest130("alter external resource pool res_pool with (MAX_CPU_PERCENT = 20, MAX_MEMORY_PERCENT = 30, MAX_CPU_PERCENT = 20);",
                new ParserErrorInfo(91, "SQL46049", "MAX_CPU_PERCENT"));
            ParserTestUtils.ErrorTest130("alter external resource pool res_pool with (MAX_PROCESSES = 20, MAX_MEMORY_PERCENT = 30, MAX_PROCESSES = 20);",
                new ParserErrorInfo(89, "SQL46049", "MAX_PROCESSES"));

            ParserTestUtils.ErrorTest130("alter external resource pool res_pool with (AFFINITY CPU = AUTO, AFFINITY CPU = AUTO);",
                new ParserErrorInfo(65, "SQL46049", "AFFINITY"));
            ParserTestUtils.ErrorTest130("alter external resource pool res_pool with (AFFINITY CPU = AUTO, AFFINITY CPU = (1));",
                new ParserErrorInfo(65, "SQL46049", "AFFINITY"));
            ParserTestUtils.ErrorTest130("alter external resource pool res_pool with (AFFINITY CPU = AUTO, CPU = AUTO);",
                new ParserErrorInfo(65, "SQL46010", "CPU"));
            ParserTestUtils.ErrorTest130("alter external resource pool res_pool with (AFFINITY CPU = AUTO, CPU = (1));",
                new ParserErrorInfo(65, "SQL46010", "CPU"));
            ParserTestUtils.ErrorTest130("alter external resource pool res_pool with (AFFINITY CPU = AUTO, AFFINITY NUMANODE = (1));",
                new ParserErrorInfo(65, "SQL46049", "AFFINITY"));
            ParserTestUtils.ErrorTest130("alter external resource pool res_pool with (AFFINITY CPU = blah);",
                new ParserErrorInfo(59, "SQL46005", "AUTO", "blah"));
            ParserTestUtils.ErrorTest130("alter external resource pool res_pool with (AFFINITY NUMANODE = AUTO);",
                new ParserErrorInfo(64, "SQL46010", "AUTO"));
            ParserTestUtils.ErrorTest130("alter external resource pool res_pool with (AFFINITY NUMANODE = (2), AFFINITY NUMANODE = (1));",
                new ParserErrorInfo(69, "SQL46049", "AFFINITY"));
            ParserTestUtils.ErrorTest130("alter external resource pool res_pool with (AFFINITY CPU = (a));",
                new ParserErrorInfo(60, "SQL46010", "a"));
            ParserTestUtils.ErrorTest130("alter external resource pool res_pool with (AFFINITY NUMANODE = (a));",
                new ParserErrorInfo(65, "SQL46010", "a"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void CreateWorkloadGroupStatementErrorTest()
        {
            ParserTestUtils.ErrorTest100("create workload group wg1 with (IMPORTANC = HIGH);",
                new ParserErrorInfo(32, "SQL46005", "IMPORTANCE", "IMPORTANC"));
            ParserTestUtils.ErrorTest100("create workload group wg1 with (MAX_DOP = 67) using res_pool;",
                new ParserErrorInfo(32, "SQL46045", "MAX_DOP"));
            ParserTestUtils.ErrorTest100("create workload group wg1 with (blah_blah = 20);",
                new ParserErrorInfo(32, "SQL46010", "blah_blah"));
            ParserTestUtils.ErrorTest100("create workload group wg1 with (MAX_DOP = ab);",
                new ParserErrorInfo(32, "SQL46005", "IMPORTANCE", "MAX_DOP"));
            ParserTestUtils.ErrorTest100("create workload group wg1 with (IMPORTANCE = ab);",
                new ParserErrorInfo(45, "SQL46010", "ab"));
            ParserTestUtils.ErrorTest100("create workload group wg1 with (IMPORTANCE = HIGH, MAX_DOP = 20, IMPORTANCE = HIGH) using res_pool",
                 new ParserErrorInfo(65, "SQL46049", "IMPORTANCE"));
            ParserTestUtils.ErrorTest100("create workload group wg1 with (MAX_DOP = 20, IMPORTANCE = HIGH, MAX_DOP = 20, IMPORTANCE = HIGH) using res_pool",
                new ParserErrorInfo(65, "SQL46049", "MAX_DOP"));
            ParserTestUtils.ErrorTest100("create workload group wg1 with (IMPORTANCE = HIGH, IMPORTANCE = HIGH, IMPORTANCE = HIGH) using res_pool",
                new ParserErrorInfo(51, "SQL46049", "IMPORTANCE"));
            ParserTestUtils.ErrorTest100("CREATE WORKLOAD GROUP wg1 WITH (REQUEST_MAX_MEMORY_GRANT_PERCENT = 20, REQUEST_MAX_CPU_TIME_SEC = 10, REQUEST_MEMORY_GRANT_TIMEOUT_SEC = 40, MAX_DOP = 20, GROUP_MAX_REQUESTS = 10, IMPORTANCE = HIGH, REQUEST_MAX_MEMORY_GRANT_PERCENT = 20) USING res_pool;",
                new ParserErrorInfo(199, "SQL46049", "REQUEST_MAX_MEMORY_GRANT_PERCENT"));
            ParserTestUtils.ErrorTest100("create workload group wg1 using res_pool, res_pool",
                new ParserErrorInfo(40, "SQL46010", ","));
            ParserTestUtils.ErrorTest130("create workload group wg1 WITH (MIN_PERCENTAGE_RESOURCE = 10,CAP_PERCENTAGE_RESOURCE = 100, REQUEST_MIN_RESOURCE_GRANT_PERCENT = 7.00, REQUEST_MIN_RESOURCE_GRANT_PERCENT = 7.00)",
                new ParserErrorInfo(135, "SQL46049", "REQUEST_MIN_RESOURCE_GRANT_PERCENT"));
            ParserTestUtils.ErrorTest130("create workload group wg1 WITH (MIN_PERCENTAGE_RESOURCE = 101, CAP_PERCENTAGE_RESOURCE = 100, REQUEST_MIN_RESOURCE_GRANT_PERCENT = 7.00)",
                new ParserErrorInfo(32, "SQL46045", "MIN_PERCENTAGE_RESOURCE"));
            ParserTestUtils.ErrorTest130("create workload group wg1 WITH (MIN_PERCENTAGE_RESOURCE = 10, CAP_PERCENTAGE_RESOURCE = 0, REQUEST_MIN_RESOURCE_GRANT_PERCENT = 7.00)",
                new ParserErrorInfo(62, "SQL46045", "CAP_PERCENTAGE_RESOURCE"));
            ParserTestUtils.ErrorTest130("create workload group wg1 WITH (MIN_PERCENTAGE_RESOURCE = 10, CAP_PERCENTAGE_RESOURCE = 101, REQUEST_MIN_RESOURCE_GRANT_PERCENT = 7.00)",
                new ParserErrorInfo(62, "SQL46045", "CAP_PERCENTAGE_RESOURCE"));
            ParserTestUtils.ErrorTest130("create workload group wg1 WITH (MIN_PERCENTAGE_RESOURCE = 10, REQUEST_MIN_RESOURCE_GRANT_PERCENT = 0.50, CAP_PERCENTAGE_RESOURCE = 100)",
                new ParserErrorInfo(62, "SQL46045", "REQUEST_MIN_RESOURCE_GRANT_PERCENT"));
            ParserTestUtils.ErrorTest130("create workload group wg1 WITH (MIN_PERCENTAGE_RESOURCE = 10, REQUEST_MIN_RESOURCE_GRANT_PERCENT = 101.50, CAP_PERCENTAGE_RESOURCE = 100)",
                new ParserErrorInfo(62, "SQL46045", "REQUEST_MIN_RESOURCE_GRANT_PERCENT"));
            ParserTestUtils.ErrorTest130("create workload group wg1 WITH (MIN_PERCENTAGE_RESOURCE = 10, REQUEST_MAX_RESOURCE_GRANT_PERCENT = 0.50, CAP_PERCENTAGE_RESOURCE = 100)",
                new ParserErrorInfo(62, "SQL46045", "REQUEST_MAX_RESOURCE_GRANT_PERCENT"));
            ParserTestUtils.ErrorTest130("create workload group wg1 WITH (MIN_PERCENTAGE_RESOURCE = 10, REQUEST_MAX_RESOURCE_GRANT_PERCENT = 101.50, CAP_PERCENTAGE_RESOURCE = 100)",
                new ParserErrorInfo(62, "SQL46045", "REQUEST_MAX_RESOURCE_GRANT_PERCENT"));
            ParserTestUtils.ErrorTest130("create workload group wg1 using res_pool1, res_pool2",
                new ParserErrorInfo(43, "SQL46049", "pool_name"));
            ParserTestUtils.ErrorTest130("create workload group wg1 using external res_pool, external res_pool",
                 new ParserErrorInfo(51, "SQL46049", "external"));
            ParserTestUtils.ErrorTest130("create workload group wg1 using external pool_ext, pool_int, external pool_ext",
                 new ParserErrorInfo(61, "SQL46049", "external"));
            ParserTestUtils.ErrorTest130("create workload group wg1 using external pool_ext, pool_int1, pool_int2",
                 new ParserErrorInfo(62, "SQL46049", "pool_name"));
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void CreateWorkloadClassifierStatementErrorTest()
        {
            ParserTestUtils.ErrorTest130("create workload classifier wc1 with (WORKLOAD_GROUP = 'wgDefaultParams', MEMBERNAME = 'ELTRole', IMPORTANC = HIGH);",
                new ParserErrorInfo(97, "SQL46010", "IMPORTANC"));
            ParserTestUtils.ErrorTest130("create workload classifier wc1 with (blah_blah = 20);",
                new ParserErrorInfo(37, "SQL46010", "blah_blah"));
            ParserTestUtils.ErrorTest130("create workload classifier wc1 with (IMPORTANCE = ab);",
                new ParserErrorInfo(50, "SQL46010", "ab"));

            string query1 = @"CREATE WORKLOAD CLASSIFIER wcAllOptions 
                                            WITH(
                                            WORKLOAD_GROUP = 'wgDefaultParams',
                                            MEMBERNAME = 'ELTRole',
                                            WLM_CONTEXT = 'dim_load',
                                            START_TIME = '09:20:30',
                                            END_TIME = '02:00',
                                            WLM_LABEL = 'label',
                                            IMPORTANCE = HIGH
                                            )";
            ParserTestUtils.ErrorTest130(query1,
                new ParserErrorInfo(query1.IndexOf("START_TIME") + 13, "SQL46134", "START_TIME"));

            string query2 = @"CREATE WORKLOAD CLASSIFIER wcAllOptions 
                                            WITH(
                                            WORKLOAD_GROUP = 'wgDefaultParams',
                                            MEMBERNAME = 'ELTRole',
                                            WLM_CONTEXT = 'dim_load',
                                            START_TIME = '09:20',
                                            END_TIME = '11:40:10',
                                            WLM_LABEL = 'label',
                                            IMPORTANCE = HIGH
                                            )";
            ParserTestUtils.ErrorTest130(query2,
                new ParserErrorInfo(query2.IndexOf("END_TIME") + 11, "SQL46134", "END_TIME"));

            string query3 = @"CREATE WORKLOAD CLASSIFIER wcAllOptions 
                                            WITH(
                                            WORKLOAD_GROUP = 'wgDefaultParams',
                                            WORKLOAD_GROUP = 'wgDefaultParams',
                                            MEMBERNAME = 'ELTRole',
                                            WLM_CONTEXT = 'dim_load',
                                            START_TIME = '09:20',
                                            END_TIME = '11:40',
                                            WLM_LABEL = 'label',
                                            IMPORTANCE = HIGH
                                            )";
            ParserTestUtils.ErrorTest130(query3,
                new ParserErrorInfo(query3.IndexOf("WORKLOAD_GROUP", 160), "SQL46049", "WORKLOAD_GROUP"));

            string query4 = @"CREATE WORKLOAD CLASSIFIER wcAllOptions 
                                            WITH(
                                            WORKLOAD_GROUP = 'wgDefaultParams',
                                            MEMBERNAME = 'ELTRole',
                                            WLM_CONTEXT = 'dim_load',
                                            MEMBERNAME = 'ELTRole',
                                            START_TIME = '09:20',
                                            END_TIME = '11:40',
                                            WLM_LABEL = 'label',
                                            IMPORTANCE = HIGH
                                            )";
            ParserTestUtils.ErrorTest130(query4,
                new ParserErrorInfo(query4.IndexOf("MEMBERNAME", 300), "SQL46049", "MEMBERNAME"));

            string query5 = @"CREATE WORKLOAD CLASSIFIER wcAllOptions 
                                            WITH(
                                            WORKLOAD_GROUP = 'wgDefaultParams',
                                            MEMBERNAME = 'ELTRole',
                                            WLM_CONTEXT = 'dim_load',
                                            START_TIME = '09:20',
                                            END_TIME = '11:40',
                                            WLM_LABEL = 'label',
                                            WLM_CONTEXT = 'dim_load',
                                            IMPORTANCE = HIGH
                                            )";
            ParserTestUtils.ErrorTest130(query5,
                new ParserErrorInfo(query5.IndexOf("WLM_CONTEXT", 400), "SQL46049", "WLM_CONTEXT"));

            string query6 = @"CREATE WORKLOAD CLASSIFIER wcAllOptions 
                                            WITH(
                                            WORKLOAD_GROUP = 'wgDefaultParams',
                                            MEMBERNAME = 'ELTRole',
                                            WLM_CONTEXT = 'dim_load',
                                            WLM_LABEL = 'label',
                                            START_TIME = '09:20',
                                            END_TIME = '11:40',
                                            WLM_LABEL = 'label',
                                            IMPORTANCE = HIGH
                                            )";
            ParserTestUtils.ErrorTest130(query6,
                new ParserErrorInfo(query6.IndexOf("WLM_LABEL", 500), "SQL46049", "WLM_LABEL"));

            string query7 = @"CREATE WORKLOAD CLASSIFIER wcAllOptions 
                                            WITH(
                                            WORKLOAD_GROUP = 'wgDefaultParams',
                                            MEMBERNAME = 'ELTRole',
                                            WLM_CONTEXT = 'dim_load',
                                            WLM_LABEL = 'label',
                                            IMPORTANCE = HIGH,
                                            START_TIME = '09:20',
                                            END_TIME = '11:40',
                                            IMPORTANCE = HIGH
                                            )";
            ParserTestUtils.ErrorTest130(query7,
                new ParserErrorInfo(query7.IndexOf("IMPORTANCE", 550), "SQL46049", "IMPORTANCE"));

            string query8 = @"CREATE WORKLOAD CLASSIFIER wcAllOptions 
                                            WITH(
                                            WORKLOAD_GROUP = 'wgDefaultParams',
                                            MEMBERNAME = 'ELTRole',
                                            WLM_CONTEXT = 'dim_load',
                                            WLM_LABEL = 'label',
                                            START_TIME = '09:20',
                                            START_TIME = '09:20',
                                            END_TIME = '11:40',
                                            IMPORTANCE = HIGH
                                            )";
            ParserTestUtils.ErrorTest130(query8,
                new ParserErrorInfo(query8.IndexOf("START_TIME", 430), "SQL46049", "START_TIME"));

            string query9 = @"CREATE WORKLOAD CLASSIFIER wcAllOptions 
                                            WITH(
                                            WORKLOAD_GROUP = 'wgDefaultParams',
                                            MEMBERNAME = 'ELTRole',
                                            WLM_CONTEXT = 'dim_load',
                                            WLM_LABEL = 'label',
                                            START_TIME = '09:20',
                                            END_TIME = '11:40',
                                            END_TIME = '11:40',
                                            IMPORTANCE = HIGH
                                            )";
            ParserTestUtils.ErrorTest130(query9,
                new ParserErrorInfo(query9.IndexOf("END_TIME", 492), "SQL46049", "END_TIME"));
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void PredictErrorTest()
        {
            ParserTestUtils.ErrorTest130(@"SELECT d.*, p.Score FROM PREDICT(MODE = (SELECT Model FROM Models WHERE Id = 4), DATA = testData AS d, RUNTIME=ONNX) WITH(Score float) AS p;",
                new ParserErrorInfo(33, "SQL46005", "MODEL", "MODE"));

            ParserTestUtils.ErrorTest130(@"SELECT d.*, p.Score FROM PREDICT(MODEL = (SELECT Model FROM trafficModel WHERE Id = 4), DATA = testData AS d, RUNTIME=ONNX) ITH(Score float) AS p;",
                new ParserErrorInfo(124, "SQL46010", "ITH"));

            ParserTestUtils.ErrorTest130(@"SELECT d.*, p.Score FROM PREDICT(MODEL = (SELECT Model FROM Models WHERE Id = 4), DATA = testData AS d, RUNTIME=ONNX, MODEL = (SELECT Model FROM Models WHERE Id = 4)) WITH(Score float) AS p;",
                new ParserErrorInfo(116, "SQL46010", ","));
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void CreateBrokerPriorityStatementErrorTest()
        {
            ParserTestUtils.ErrorTest100("create broker priority bp1 for conversatio;",
                new ParserErrorInfo(31, "SQL46005", "CONVERSATION", "conversatio"));
            ParserTestUtils.ErrorTest100("create broker priority bp1 for conversation set (PRIORITY_LEVEL = ab);",
                new ParserErrorInfo(49, "SQL46010", "PRIORITY_LEVEL"));
            ParserTestUtils.ErrorTest100("create broker priority bp1 for conversation set (REMOTE_SERVICE_NAME = ab);",
                new ParserErrorInfo(49, "SQL46010", "REMOTE_SERVICE_NAME"));
            ParserTestUtils.ErrorTest100("create broker priority bp1 for conversation set (CONTRACT_NAME = 'ab');",
                new ParserErrorInfo(49, "SQL46010", "CONTRACT_NAME"));
            ParserTestUtils.ErrorTest100("create broker priority bp1 for conversation set (LOCAL_SERVICE_NAME = 5);",
                new ParserErrorInfo(49, "SQL46010", "LOCAL_SERVICE_NAME"));
            ParserTestUtils.ErrorTest100("create broker priority bp1 for conversation set (LOCAL_SERVICE_NAME = gh, CONTRACT_NAME = ab, LOCAL_SERVICE_NAME = a, PRIORITY_LEVEL=5, CONTRACT_NAME = bd);",
                new ParserErrorInfo(94, "SQL46010", "LOCAL_SERVICE_NAME"));
            // ANY option can only be used with CONTRACT_NAME, LOCAL_SERVICE_NAME and REMOTE_SERVICE_NAME options
            ParserTestUtils.ErrorTest100("create broker priority bp1 for conversation set (PRIORITY_LEVEL = ANY);",
                new ParserErrorInfo(49, "SQL46010", "PRIORITY_LEVEL"));
            // DEFAULT option can only be used with PRIORITY_LEVEL option
            ParserTestUtils.ErrorTest100("create broker priority bp1 for conversation set (REMOTE_SERVICE_NAME = DEFAULT);",
               new ParserErrorInfo(49, "SQL46010", "REMOTE_SERVICE_NAME"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void AlterTableAlterIndexElementStatementErrorTest()
        {
            ParserTestUtils.ErrorTest130("ALTER TABLE t1 ALTER INDEX i1 REBUILD WITH (BUCKET_COUNT = 1, BUCKET_COUNT = 1);",
                new ParserErrorInfo(60, "SQL46010", ","));
            ParserTestUtils.ErrorTest130("ALTER TABLE t1 ALTER INDEX i1 REBUILD WITH (STATISTICS_INCREMENTAL = OFF);",
                new ParserErrorInfo(44, "SQL46057", "STATISTICS_INCREMENTAL", "ALTER TABLE ALTER INDEX REBUILD"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void AlterTableChangeTrackingDisableWithOptionsErrorTest()
        {
            ParserTestUtils.ErrorTest100("ALTER TABLE t1 DISABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = ON)",
                new ParserErrorInfo(15, "SQL46010", "DISABLE"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void MultipleForClauseTest()
        {
            ParserTestUtils.ErrorTest100("select 1 for browse for xml",
               new ParserErrorInfo(20, "SQL46010", "for"));

            ParserTestUtils.ErrorTest100("select * from (select 1 for browse for xml auto) t(c1)",
                new ParserErrorInfo(35, "SQL46010", "for"));

            ParserTestUtils.ErrorTest130("select 1 for browse for json",
                new ParserErrorInfo(20, "SQL46010", "for"));

            ParserTestUtils.ErrorTest130("select * from (select 1 for browse for json auto) t(c1)",
                new ParserErrorInfo(35, "SQL46010", "for"));

            ParserTestUtils.ErrorTest130("select 1 for xml auto for json auto",
                new ParserErrorInfo(22, "SQL46010", "for"));

            ParserTestUtils.ErrorTest130("select * from (select 1 for json auto for xml auto) t(c1)",
                new ParserErrorInfo(38, "SQL46010", "for"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void ColumnDefinitionStorageAttributesErrorTest()
        {
            TSql100Parser parser = new TSql100Parser(true);

            // Can't specify same attribute twice
            ParserTestUtils.ErrorTest(parser, "CREATE TABLE t1 (c7 VARBINARY (MAX) SPARSE SPARSE)",
                new ParserErrorInfo(43, "SQL46005", "FILESTREAM", "SPARSE"));
            ParserTestUtils.ErrorTest(parser, "CREATE TABLE t1 (c7 VARBINARY (MAX) FILESTREAM FILESTREAM)",
                new ParserErrorInfo(47, "SQL46005", "SPARSE", "FILESTREAM"));

            // Can't have filestream on something other than VARBINARY(MAX)
            ParserTestUtils.ErrorTest(parser, "ALTER TABLE t1 ADD c1 VARBINARY FILESTREAM",
                new ParserErrorInfo(32, "SQL46051"));
            ParserTestUtils.ErrorTest(parser, "ALTER TABLE t1 ADD c1 VARCHAR(MAX) FILESTREAM",
                new ParserErrorInfo(35, "SQL46051"));
            ParserTestUtils.ErrorTest(parser, "ALTER TABLE t1 ADD c1 VARBINARY(10) FILESTREAM",
                new ParserErrorInfo(36, "SQL46051"));

            // Filestream is not allowed anywhere but table definition
            ParserTestUtils.ErrorTest(parser, "declare @v2 as table (c0 varbinary(max) filestream)",
                new ParserErrorInfo(40, "SQL46010", "filestream"));
            ParserTestUtils.ErrorTest(parser,
                "create function f2() returns @v1 table (c0 varbinary(max) filestream) begin return end",
                new ParserErrorInfo(58, "SQL46010", "filestream"));

            ParserTestUtils.ErrorTest(parser, "create type t2 as table (c0 varbinary(max) filestream)",
                new ParserErrorInfo(43, "SQL46010", "filestream"));

            // Sparse is not allowed in table-valued functions and table types (but ok in table variables!)
            ParserTestUtils.ErrorTest(parser,
                "create function f1() returns @v1 table (c1 xml column_set for all_sparse_columns) begin return end",
                new ParserErrorInfo(47, "SQL46010", "column_set"));
            ParserTestUtils.ErrorTest(parser,
                "create type t1 as table (c0 int sparse)",
                new ParserErrorInfo(32, "SQL46010", "sparse"));
            ParserTestUtils.ErrorTest(parser,
                "create type t1 as table (c1 xml column_set for all_sparse_columns)",
                new ParserErrorInfo(32, "SQL46010", "column_set"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void NotForReplicationNotAllowedInTableVariablesFunctionsTypesErrorTest()
        {
            // Column check constraint
            ParserTestUtils.ErrorTestAllParsers("declare @v1 table (c1 int check not for replication (c1 > 10))",
                new ParserErrorInfo(32, "SQL46010", "not"));
            ParserTestUtils.ErrorTestAllParsers("create function f1() returns @v1 table (c1 int check not for replication (c1 > 10)) begin return end",
                new ParserErrorInfo(53, "SQL46010", "not"));
            ParserTestUtils.ErrorTest100("create type t1 as table (c1 int check not for replication (c1 > 10))",
                new ParserErrorInfo(38, "SQL46010", "not"));

            // Table check constraint
            ParserTestUtils.ErrorTestAllParsers("declare @v1 table (c1 int, check not for replication (c1 > 10))",
                new ParserErrorInfo(33, "SQL46010", "not"));
            ParserTestUtils.ErrorTestAllParsers("create function f1() returns @v1 table (c1 int, check not for replication (c1 > 10)) begin return end",
                new ParserErrorInfo(54, "SQL46010", "not"));
            ParserTestUtils.ErrorTest100("create type t1 as table (c1 int, check not for replication (c1 > 10))",
                new ParserErrorInfo(39, "SQL46010", "not"));

            // Column identity constraint
            ParserTestUtils.ErrorTestAllParsersUntil150("declare @v1 table (c1 int identity(1,1) not for replication)",
                new ParserErrorInfo(40, "SQL46010", "not"));
            ParserTestUtils.ErrorTest160("declare @v1 table (c1 int identity(1,1) not for replication)",
                new ParserErrorInfo(40, "SQL46010", "not"));
            ParserTestUtils.ErrorTest170("declare @v1 table (c1 int identity(1,1) not for replication)",
                new ParserErrorInfo(40, "SQL46010", "not"));
            ParserTestUtils.ErrorTestAllParsersUntil150("create function f1() returns @v1 table (c1 int identity(1,1) not for replication) begin return end",
                new ParserErrorInfo(61, "SQL46010", "not"));
            ParserTestUtils.ErrorTest160("create function f1() returns @v1 table (c1 int identity(1,1) not for replication) begin return end",
                new ParserErrorInfo(61, "SQL46010", "not"));
            ParserTestUtils.ErrorTest170("create function f1() returns @v1 table (c1 int identity(1,1) not for replication) begin return end",
                new ParserErrorInfo(61, "SQL46010", "not"));
            ParserTestUtils.ErrorTest100("create type t1 as table (c1 int identity(1,1) not for replication)",
                new ParserErrorInfo(46, "SQL46010", "not"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void NoConstraintNamesInTableVariablesFunctionsTypesErrorTest()
        {
            // Named column constraints
            ParserTestUtils.ErrorTestAllParsers("declare @t1 table (c1 int constraint ct1 primary key)",
                new ParserErrorInfo(26, "SQL46010", "constraint"));
            ParserTestUtils.ErrorTestAllParsers("create function f2() returns @v1 table (c0 int constraint ct1 primary key) begin return end",
                new ParserErrorInfo(47, "SQL46010", "constraint"));

            ParserTestUtils.ErrorTest100("create type t1 as table (c0 int constraint ct1 primary key)",
                new ParserErrorInfo(32, "SQL46010", "constraint"));

            // Named table constraints
            ParserTestUtils.ErrorTestAllParsers("declare @t1 table (c1 int, constraint ct1 primary key (c1))",
                new ParserErrorInfo(27, "SQL46010", "constraint"));
            ParserTestUtils.ErrorTestAllParsers("create function f2() returns @v1 table (c0 int, constraint ct1 primary key (c1)) begin return end",
                new ParserErrorInfo(48, "SQL46010", "constraint"));

            ParserTestUtils.ErrorTest100("create type t1 as table (c0 int, constraint ct1 primary key (c1))",
                new ParserErrorInfo(33, "SQL46010", "constraint"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void ServerAuditStatementsErrorTest()
        {
            TSql100Parser parser100 = new TSql100Parser(true);

            TSql150Parser parser150 = new TSql150Parser(true);

            // SQL46053 - Max size for audit file
            ParserTestUtils.ErrorTest(parser100, "CREATE SERVER AUDIT a1 TO FILE (FILEPATH = 'a', MAXSIZE = 30000000 TB)",
                new ParserErrorInfo(58, "SQL46054"));

            // SQL46054 - Wrong guid format
            ParserTestUtils.ErrorTest(parser100, "CREATE SERVER AUDIT a1 TO SECURITY_LOG WITH (AUDIT_GUID = 'a')",
                new ParserErrorInfo(58, "SQL46055"));

            // AUDIT_GUID option is not allowed in ALTER SERVER AUDIT
            string guid = "'ca761232-ed42-11ce-bacd-00aa0057b223'";
            ParserTestUtils.ErrorTest(parser100, "ALTER SERVER AUDIT a1 WITH (AUDIT_GUID = " + guid + ")",
                new ParserErrorInfo(41, "SQL46010", guid));

            // STATE option is not allowed in CREATE SERVE AUDIT
            ParserTestUtils.ErrorTest(parser100, "CREATE SERVER AUDIT a1 TO SECURITY_LOG WITH (STATE = OFF)",
                new ParserErrorInfo(53, "SQL46010", "OFF"));

            // Path (for URL target) must be specified in CREATE (but optional in ALTER SERVER AUDIT)
            ParserTestUtils.ErrorTest(parser150, "CREATE SERVER AUDIT a1 TO URL (RETENTION_DAYS = 13)",
                new ParserErrorInfo(26, "SQL46126"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SecurityStatementsErrorTest()
        {
            //Columns cannot be specified on both the permission and the object  in grant/deny/revoke
            //
            ParserTestUtils.ErrorTest90AndAbove("grant update, select(c1) on t1(c1) to public", new ParserErrorInfo(31, "SQL46010", "c1"));

            //Columns cannot be specified on either permission or object for audit specification
            //
            ParserTestUtils.ErrorTest100AndAbove("CREATE DATABASE AUDIT SPECIFICATION AuditSpec1 FOR SERVER AUDIT a1 ADD (update ON t1(c1) BY PUBLIC)", new ParserErrorInfo(85, "SQL46010", "c1"));
            ParserTestUtils.ErrorTest100AndAbove("CREATE DATABASE AUDIT SPECIFICATION AuditSpec1 FOR SERVER AUDIT a1 ADD (update(c1) ON t1 BY PUBLIC)", new ParserErrorInfo(72, "SQL46010", "update"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void EventSessionStatementErrorTest()
        {
            TSql100Parser parser = new TSql100Parser(true);

            // Cannot have a SET parameter with value as string within quotes.
            ParserTestUtils.ErrorTest(parser, "CREATE EVENT SESSION es1 ON SERVER ADD EVENT a.b.c (SET b = -'5.1');",
                new ParserErrorInfo(61, "SQL46010", "'5.1'"));

            // MAX_DISPATCH_LATENCY option must be of the format <number><unit> | <identifier>.
            ParserTestUtils.ErrorTest(parser, "CREATE EVENT SESSION es1 ON SERVER ADD EVENT b.c (SET b = -5.1) WITH (MAX_DISPATCH_LATENCY = 5);",
                new ParserErrorInfo(94, "SQL46010", ")"));

            //MAX_MEMORY option cannot have anything other than KB/MB as units
            ParserTestUtils.ErrorTest(parser, "ALTER EVENT SESSION es1 ON SERVER WITH (MAX_MEMORY = 5 SECONDS);",
                new ParserErrorInfo(55, "SQL46010", "SECONDS"));

            //MAX_DISPATCH_LATENCY option cannot have anything other than SECONDS as units
            ParserTestUtils.ErrorTest(parser, "ALTER EVENT SESSION es1 ON SERVER ADD EVENT a.b.c WITH (MAX_DISPATCH_LATENCY = 5 KB);",
                new ParserErrorInfo(81, "SQL46005", CodeGenerationSupporter.Seconds, "KB"));

            // Event Declaration Predicate Parameter's source declaration has to be of type a or a.b or a.b.c
            // Cannot have .a.b or ..b
            ParserTestUtils.ErrorTest(parser, "CREATE EVENT SESSION es1 ON SERVER ADD EVENT b.d (WHERE ..c = 5);",
                new ParserErrorInfo(56, "SQL46010", "."));
            ParserTestUtils.ErrorTest(parser, "CREATE EVENT SESSION es1 ON SERVER ADD EVENT b.d (WHERE .a.c = 5);",
                new ParserErrorInfo(56, "SQL46010", "."));

            ParserTestUtils.ErrorTest(parser, "CREATE EVENT SESSION es1 ON DATABASE;",
                new ParserErrorInfo(28, "SQL46010", "DATABASE"));

            ParserTestUtils.ErrorTest(parser, "ALTER EVENT SESSION es1 ON DATABASE;",
                new ParserErrorInfo(27, "SQL46010", "DATABASE"));

            ParserTestUtils.ErrorTest(parser, "DROP EVENT SESSION es1 ON DATABASE;",
                new ParserErrorInfo(26, "SQL46010", "DATABASE"));

        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void When_session_scope_is_other_than_database_or_server_CREATE_EVENT_SESSION_fails_for_130()
        {
            var tSql130Parser = new TSql130Parser(true);
            ParserTestUtils.ErrorTest(tSql130Parser, "CREATE EVENT SESSION es1 ON TABLE fooTable;",
                new ParserErrorInfo(28, "SQL46010", "TABLE"));

            ParserTestUtils.ErrorTest(tSql130Parser, "CREATE EVENT SESSION es1 ADD EVENT a.b;",
                new ParserErrorInfo(25, "SQL46010", "ADD"));

            ParserTestUtils.ErrorTest(tSql130Parser, "DROP EVENT SESSION es1 ON TABLE fooTable;",
                new ParserErrorInfo(26, "SQL46010", "TABLE"));

            ParserTestUtils.ErrorTest(tSql130Parser, "ALTER EVENT SESSION es1 ADD EVENT a.b;",
                new ParserErrorInfo(24, "SQL46010", "ADD"));
        }

        // Test the key option duplication is not allowed

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void KeyOptionDuplicationErrorTest()
        {
            ParserTestUtils.ErrorTest90AndAbove(
                "CREATE SYMMETRIC KEY k1 WITH KEY_SOURCE='a', KEY_SOURCE='b' ENCRYPTION BY PASSWORD='PLACEHOLDER'",
                new ParserErrorInfo(45, "SQL46010", "KEY_SOURCE"));
            ParserTestUtils.ErrorTest90AndAbove(
                "CREATE SYMMETRIC KEY k1 WITH ALGORITHM=RC4, ALGORITHM=RC2 ENCRYPTION BY PASSWORD='PLACEHOLDER'",
                new ParserErrorInfo(44, "SQL46010", "ALGORITHM"));
            ParserTestUtils.ErrorTest90AndAbove(
                "CREATE SYMMETRIC KEY k1 WITH IDENTITY_VALUE='y', IDENTITY_VALUE='z' ENCRYPTION BY PASSWORD='PLACEHOLDER'",
                new ParserErrorInfo(49, "SQL46010", "IDENTITY_VALUE"));

            ParserTestUtils.ErrorTest100(
                "CREATE SYMMETRIC KEY k1 FROM PROVIDER p1 WITH ALGORITHM=RC4, ALGORITHM=DES",
                new ParserErrorInfo(61, "SQL46010", "ALGORITHM"));
            ParserTestUtils.ErrorTest100(
                "CREATE SYMMETRIC KEY k1 FROM PROVIDER p1 WITH PROVIDER_KEY_NAME='p', PROVIDER_KEY_NAME='p'",
                new ParserErrorInfo(69, "SQL46010", "PROVIDER_KEY_NAME"));
            ParserTestUtils.ErrorTest100(
                "CREATE SYMMETRIC KEY k1 FROM PROVIDER p1 WITH CREATION_DISPOSITION=CREATE_NEW, CREATION_DISPOSITION=OPEN_EXISTING",
                new ParserErrorInfo(79, "SQL46010", "CREATION_DISPOSITION"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46069Test()
        {
            ParserTestUtils.ErrorTest90AndAbove("create queue q1 with status = on, Activation(status = on, procedure_name = dbo..p1,  execute as self), retention = off;",
                new ParserErrorInfo(13, "SQL46069"));
            ParserTestUtils.ErrorTest90AndAbove("create queue q1 with status = on, Activation(status = on, execute as self), retention = off;",
                new ParserErrorInfo(13, "SQL46069"));
            ParserTestUtils.ErrorTest90AndAbove("create queue q1 with status = on, Activation(status = on, max_queue_readers = 23,  execute as self), retention = off;",
                new ParserErrorInfo(13, "SQL46069"));
            ParserTestUtils.ErrorTest90AndAbove("create queue q1 with status = on, Activation(status = on, max_queue_readers = 23,  procedure_name=dbo..p1), retention = off;",
                new ParserErrorInfo(13, "SQL46069"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void MaxParameterInDataTypesErrorTest()
        {
            ParserTestUtils.ErrorTest90AndAbove("CREATE TABLE t1(c1 CHAR(max))",
                new ParserErrorInfo(24, "SQL46010", "max"));
            ParserTestUtils.ErrorTest90AndAbove("CREATE TABLE t1(c1 NCHAR(max))",
                new ParserErrorInfo(25, "SQL46010", "max"));
            ParserTestUtils.ErrorTest90AndAbove("CREATE TABLE t1(c1 BINARY(MAX))",
                new ParserErrorInfo(26, "SQL46010", "MAX"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void CreateSpatialIndexStatementErrorTest()
        {
            TSql100Parser parser = new TSql100Parser(true);

            ParserTestUtils.ErrorTest(parser, "CREATE SPATIAL INDEX sp1 ON a.c (d) USING GEOMETRY_GRID WITH (BOUNDING_BOX = (XMAX=5,YMAX=6,XMIN=5));",
                new ParserErrorInfo(62, "SQL46066", "BOUNDING_BOX"));
            ParserTestUtils.ErrorTest(parser, "CREATE SPATIAL INDEX sp1 ON a.c (d) WITH(DATA_COMPRESSION=PAGE)",
                new ParserErrorInfo(41, "SQL46057", "DATA_COMPRESSION", "CREATE SPATIAL INDEX"));
            ParserTestUtils.ErrorTest(parser, "CREATE SPATIAL INDEX sp1 ON a.c (d) USING GEOGRAPHY_GRID WITH (BOUNDING_BOX = (XMAX=5,YMAX=6,XMIN=5,YMIN=3));",
                new ParserErrorInfo(63, "SQL46067", "BOUNDING_BOX"));
            ParserTestUtils.ErrorTest(parser, "CREATE SPATIAL INDEX sp1 ON a.c (d) USING GEOGRAPHY_GRID WITH (GRIDS = (LEVEL_1=HIGH,blah_blah=LOW,LEVEL_3=MEDIUM,LEVEL_1=HIGH));",
                new ParserErrorInfo(85, "SQL46010", "blah_blah"));
            ParserTestUtils.ErrorTest(parser, "CREATE SPATIAL INDEX sp1 ON a.c (d) USING GEOGRAPHY_GRID WITH (GRIDS = (LEVEL_1=blah_blah,LEVEL_2=LOW,LEVEL_3=MEDIUM,LEVEL_1=HIGH));",
                new ParserErrorInfo(80, "SQL46010", "blah_blah"));
            ParserTestUtils.ErrorTest(parser, "CREATE SPATIAL INDEX sp1 ON a.c (d) USING GEOGRAPHY_GRID WITH (blah_blah = (LEVEL_1=HIGH,LEVEL_2=LOW,LEVEL_3=MEDIUM,LEVEL_1=HIGH));",
                new ParserErrorInfo(63, "SQL46010", "blah_blah"));
            // Tests if the value of CELLS_PER_OBJECT option is between 1 and 8192
            ParserTestUtils.ErrorTest(parser, "CREATE SPATIAL INDEX sp1 ON a.c (d) USING GEOGRAPHY_GRID WITH (CELLS_PER_OBJECT = 9000);",
                new ParserErrorInfo(82, "SQL46073", "blah_blah"));
            // Tests that BOUNDING_BOX parameter accepts only positive or negative integer or float values.
            ParserTestUtils.ErrorTest(parser, "CREATE SPATIAL INDEX sp1 ON a.c (d) USING GEOMETRY_GRID WITH (BOUNDING_BOX = (XMAX = a, YMAX = b, XMIN = c, YMIN = d));",
                new ParserErrorInfo(85, "SQL46010", "a"));

            // Tests that spatial index options do not appear after general index options.
            ParserTestUtils.ErrorTest(parser, "CREATE SPATIAL INDEX sp1 ON a..c (d) USING GEOGRAPHY_GRID WITH  (ALLOW_ROW_LOCKS = OFF, GRIDS = (LEVEL_1 = HIGH, LEVEL_2 = LOW, LEVEL_3 = MEDIUM, LEVEL_4 = LOW), CELLS_PER_OBJECT = 5, PAD_INDEX = OFF);",
                new ParserErrorInfo(88, "SQL46081", "GRIDS"));
            ParserTestUtils.ErrorTest(parser, "CREATE SPATIAL INDEX sp1 ON a..c (d) USING GEOGRAPHY_GRID WITH  (GRIDS = (LEVEL_1 = HIGH, LEVEL_2 = LOW, LEVEL_3 = MEDIUM, LEVEL_4 = LOW), ALLOW_ROW_LOCKS = OFF, CELLS_PER_OBJECT = 5, PAD_INDEX = OFF);",
                new ParserErrorInfo(162, "SQL46081", "CELLS_PER_OBJECT"));
            ParserTestUtils.ErrorTest(parser, "CREATE SPATIAL INDEX sp1 ON a..c (d) USING GEOGRAPHY_GRID WITH  (GRIDS = (LEVEL_1 = HIGH, LEVEL_2 = LOW, LEVEL_3 = MEDIUM, LEVEL_4 = LOW), ALLOW_ROW_LOCKS = OFF, BOUNDING_BOX =(4,5,6,5), PAD_INDEX = OFF);",
                new ParserErrorInfo(162, "SQL46081", "BOUNDING_BOX"));
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void CreateSpatialIndexStatementErrorTest160()
        {
            TSql160Parser parser = new TSql160Parser(true);
            ParserTestUtils.ErrorTest(parser, "CREATE SPATIAL INDEX sp1 ON a.c (d) WITH(XML_COMPRESSION=ON)",
                new ParserErrorInfo(41, "SQL46057", "XML_COMPRESSION", "CREATE SPATIAL INDEX"));
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void CreateColumnStoreIndexStatementErrorTest160()
        {
            TSql160Parser parser = new TSql160Parser(true);
            ParserTestUtils.ErrorTest(parser, "CREATE CLUSTERED COLUMNSTORE INDEX cci ON Sales.OrderLines WITH(XML_COMPRESSION = ON); ",
                new ParserErrorInfo(64, "SQL46057", "XML_COMPRESSION", "CREATE COLUMNSTORE INDEX"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SequenceStatementErrorTest()
        {
            //Three-Part name
            //
            ParserTestUtils.ErrorTest110("ALTER SEQUENCE [SOMEDBA].[SOMESCHEMA].[S]", new ParserErrorInfo(37, "SQL46010", "."));
            ParserTestUtils.ErrorTest110("CREATE SEQUENCE [SOMEDBA].[SOMESCHEMA].[S]", new ParserErrorInfo(38, "SQL46010", "."));
            ParserTestUtils.ErrorTest110("DROP SEQUENCE [SOMEDBA].[SOMESCHEMA].[S]", new ParserErrorInfo(14, "SQL46021", "DROP"));

            //Create
            //
            ParserTestUtils.ErrorTest110("CREATE SEQUENCE [SOMESCHEMA].[S] AS INT AS INTALIAS", new ParserErrorInfo(40, "SQL46049", "AS"));
            ParserTestUtils.ErrorTest110("CREATE SEQUENCE [SOMESCHEMA].[S] CYCLE CYCLE", new ParserErrorInfo(39, "SQL46049", "CYCLE"));
            ParserTestUtils.ErrorTest110("CREATE SEQUENCE [SOMESCHEMA].[S] NO MINVALUE MINVALUE 30", new ParserErrorInfo(45, "SQL46049", "MINVALUE"));
            ParserTestUtils.ErrorTest110("CREATE SEQUENCE [SOMESCHEMA].[S] RESTART", new ParserErrorInfo(33, "SQL46010", "RESTART"));

            //Alter
            //
            ParserTestUtils.ErrorTest110("ALTER SEQUENCE [SOMESCHEMA].[S] CYCLE CYCLE", new ParserErrorInfo(38, "SQL46049", "CYCLE"));
            ParserTestUtils.ErrorTest110("ALTER SEQUENCE [SOMESCHEMA].[S] AS INT", new ParserErrorInfo(32, "SQL46010", "AS"));
            ParserTestUtils.ErrorTest110("ALTER SEQUENCE [SOMESCHEMA].[S] START WITH 1", new ParserErrorInfo(32, "SQL46010", "START"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void OffsetErrorTest()
        {
            //Offset no order by
            //
            ParserTestUtils.ErrorTest110("select * from T offset 5 rows fetch next 2 rows only", new ParserErrorInfo(16, "SQL46010", "offset"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void CommitTransactionErrorTest()
        {
            ParserTestUtils.ErrorTest120("COMMIT TRANSACTION WITH (DELAYED_DURABILITY = ONN)", new ParserErrorInfo(46, "SQL46010", "ONN"));
            ParserTestUtils.ErrorTest120("COMMIT TRANSACTION WITH (DELAYED_DURABILITY = OF)", new ParserErrorInfo(46, "SQL46010", "OF"));
            ParserTestUtils.ErrorTest120("COMMIT TRANSACTION WITH DELAYED_DURABILITY = ON", new ParserErrorInfo(24, "SQL46010", "DELAYED_DURABILITY"));
            ParserTestUtils.ErrorTest120("COMMIT TRANSACTION (DELAYED_DURABILITY = ON)", new ParserErrorInfo(20, "SQL46010", "DELAYED_DURABILITY"));
            ParserTestUtils.ErrorTest120("COMMIT TRANSACTION WITH (DELAYED_DURABILITY = 1)", new ParserErrorInfo(46, "SQL46010", "1"));
            ParserTestUtils.ErrorTest120("COMMIT TRANSACTION WITH (DELAYED_DURABILITY2 = ON)", new ParserErrorInfo(25, "SQL46005", new string[] { "DELAYED_DURABILITY", "DELAYED_DURABILITY2" }));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void AlterDatabaseErrorTest()
        {
            ParserTestUtils.ErrorTest120("ALTER DATABASE testdb SET DELAYED_DURABILITY = ALLOWEDD)", new ParserErrorInfo(47, "SQL46010", "ALLOWEDD"));
            ParserTestUtils.ErrorTest120("ALTER DATABASE testdb SET DELAYED_DURA = ALLOWED)", new ParserErrorInfo(26, "SQL46010", "DELAYED_DURA"));
            ParserTestUtils.ErrorTest120("ALTER DATABASE testdb SET DELAYED_DURABILITY ALLOWED)", new ParserErrorInfo(26, "SQL46010", "DELAYED_DURABILITY"));

            ParserTestUtils.ErrorTest130("ALTER DATABASE testdb SET MIXED_PAGE_ALLOCATION NONE)", new ParserErrorInfo(26, "SQL46010", "MIXED_PAGE_ALLOCATION"));
            ParserTestUtils.ErrorTest130("ALTER DATABASE testdb SET MIXED_PAGE_ALLOC ON)", new ParserErrorInfo(26, "SQL46010", "MIXED_PAGE_ALLOC"));
            ParserTestUtils.ErrorTest130("ALTER DATABASE testdb SET MIXED_PAGE_ALLOCATION)", new ParserErrorInfo(26, "SQL46010", "MIXED_PAGE_ALLOCATION"));
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void RenameStatementErrorTest()
        {
            ParserTestUtils.ErrorTest130("RENAME DATABASE testdb TO myDb)", new ParserErrorInfo(7, "SQL46010", "DATABASE"));
            ParserTestUtils.ErrorTest130("RENAME DATABASE :: testdb TO myDb)", new ParserErrorInfo(7, "SQL46010", "DATABASE"));
            ParserTestUtils.ErrorTest130("RENAME DATAWAREHOUSE testdb TO myDb)", new ParserErrorInfo(7, "SQL46010", "DATAWAREHOUSE"));
            ParserTestUtils.ErrorTest130("RENAME STATE testdb TO myDb)", new ParserErrorInfo(7, "SQL46010", "STATE"));
            ParserTestUtils.ErrorTest130("RENAME OBJECT [] TO T2)", new ParserErrorInfo(15, "SQL46010", "]"));
            ParserTestUtils.ErrorTest130("RENAME OBJECT T1 TO [])", new ParserErrorInfo(21, "SQL46010", "]"));
            ParserTestUtils.ErrorTest130("RENAME OBJECT dbo.T1 TO dbo.test)", new ParserErrorInfo(27, "SQL46010", "."));
            ParserTestUtils.ErrorTest130("RENAME OBJECT dbo.T1 TO DB.dbo.test)", new ParserErrorInfo(26, "SQL46010", "."));
            ParserTestUtils.ErrorTest130("RENAME OBJECT ;; T1 TO T2)", new ParserErrorInfo(14, "SQL46010", ";"), new ParserErrorInfo(17, "SQL46010", "T1"));
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void CtasStatementErrorTest()
        {
            ParserTestUtils.ErrorTest130("CREATE TABLE dbo.TableInProc AS SELECT 1; ", new ParserErrorInfo(32, "SQL46010", "SELECT"));
            ParserTestUtils.ErrorTest130("CREATE TABLE dbo.TableInProc WITH (CLUSTERED COLUMNSTORE INDEX, DISTRIBUTION=ROUND_ROBIN)) AS SELECT 1; ", new ParserErrorInfo(89, "SQL46010", ")"));
            ParserTestUtils.ErrorTest130("CREATE TABLE dbo.TableInProc WITH (CLUSTERED COLUMNSTORE INDEX) AS SELECT 1; ", new ParserErrorInfo(0, "SQL46127"));
            ParserTestUtils.ErrorTest130("CREATE TABLE dbo.TableInProc WITH (CLUSTERED INDEX(ID)) AS SELECT 1; ", new ParserErrorInfo(0, "SQL46127"));
            ParserTestUtils.ErrorTest130("CREATE TABLE dbo.TableInProc WITH (HEAP) AS SELECT 1; ", new ParserErrorInfo(0, "SQL46127"));
            ParserTestUtils.ErrorTest130("CREATE TABLE dbo.TableInProc WITH(DISTRIBUTION = REPLICATE) SELECT 1; ", new ParserErrorInfo(60, "SQL46010", "SELECT"));
            ParserTestUtils.ErrorTest130("CREATE TABLE dbo.TableInProc WITH(DISTRIBUTION = REPLICATE) AS 1 ", new ParserErrorInfo(63, "SQL46010", "1"));
            ParserTestUtils.ErrorTest130("CREATE PROCEDURE dwsyntaxforsqldom AS BEGIN CREATE TABLE [dbo].[DimSalesTerritory_REPLICATE] WITH(CLUSTERED COLUMNSTORE INDEX,DISTRIBUTION = REPLICATE) AS SELECT * FROM[dbo].[DimSalesTerritory]",
                new ParserErrorInfo(193, "SQL46029"));
            ParserTestUtils.ErrorTest130("CREATE TABLE #tmp_fct WITH(DISTRIBUTION = ROUND_ROBIN)AS SELECT p.ProductKey FROM dbo.DimProduct p RIGHT JOIN dbo.stg_DimProduct s; ", new ParserErrorInfo(130, "SQL46010", ";"));

            // CTAS with Column Names, but no Distribution
            ParserTestUtils.ErrorTest130("CREATE TABLE t1 ( c1 ) AS SELECT 1 ", new ParserErrorInfo(23, "SQL46010", "AS"));

            // Empty Column Name list is not supported for CTAS
            ParserTestUtils.ErrorTest130("CREATE TABLE t1 ( ) WITH(DISTRIBUTION = HASH([BCSD])) AS SELECT 1 ", new ParserErrorInfo(16, "SQL46010", "("));

            // Dangling commas are not supported in CTAS column name list
            ParserTestUtils.ErrorTest130("CREATE TABLE t1 ( c1, ) WITH(DISTRIBUTION = HASH([c1])) AS SELECT 1 ", new ParserErrorInfo(22, "SQL46010", ")"));
            ParserTestUtils.ErrorTest130("CREATE TABLE t1 ( c1, c2, ) WITH(DISTRIBUTION = HASH([c1])) AS SELECT 1 ", new ParserErrorInfo(26, "SQL46010", ")"));

            // Column Defintion not supported for CTAS
            ParserTestUtils.ErrorTest130("CREATE TABLE t1 ( c1 INT ) WITH(DISTRIBUTION = HASH([c1])) AS SELECT 1 ", new ParserErrorInfo(59, "SQL46010", "AS"));

            // Combination of Column Names and Column Definition is not supported
            ParserTestUtils.ErrorTest130("CREATE TABLE t1 ( c1, c2 INT ) WITH(DISTRIBUTION = HASH([c1])) AS SELECT 1 ", new ParserErrorInfo(25, "SQL46010", "INT"));

            // Location other than USER_DB is not supported
            ParserTestUtils.ErrorTest130("CREATE TABLE t1 ( c1, c2) WITH(DISTRIBUTION = HASH([c1]), LOCATION = XYZ) AS SELECT 1 ", new ParserErrorInfo(69, "SQL46010", "XYZ"));

            // Incorrect syntax of Ordered columns
            ParserTestUtils.ErrorTest130("CREATE TABLE t1 ( c1, c2) WITH(DISTRIBUTION = HASH([c1]), CLUSTERED COLUMNSTORE INDEX (c1, c2)) AS SELECT 1 ", new ParserErrorInfo(86, "SQL46010", "("));
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void AlterDatabaseModifyFilegroupErrorTest()
        {
            ParserTestUtils.ErrorTest130("ALTER DATABASE testdb MODIFY FILEGROUP [PRIMARY] READ ONLY)", new ParserErrorInfo(49, "SQL46010", "READ"));
            ParserTestUtils.ErrorTest130("ALTER DATABASE testdb MODIFY FILEGROUP [PRIMARY] AUTOGROW)", new ParserErrorInfo(49, "SQL46010", "AUTOGROW"));
            ParserTestUtils.ErrorTest130("ALTER DATABASE testdb MODIFY FILEGROUP FG1 AUTO_GROW_ALL_FILES)", new ParserErrorInfo(43, "SQL46010", "AUTO_GROW_ALL_FILES"));
            ParserTestUtils.ErrorTest130("ALTER DATABASE testdb MODIFY FILEGROUP [FG2] AUTOGROW SINGLE FILE)", new ParserErrorInfo(45, "SQL46010", "AUTOGROW"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void IncrementalStatisticsErrorTest()
        {
            ParserTestUtils.ErrorTest120("ALTER DATABASE testdb SET AUTO_CREATE_STATISTICS ON (INCREMENTAL)", new ParserErrorInfo(64, "SQL46010", ")"));
            ParserTestUtils.ErrorTest120("ALTER DATABASE testdb SET AUTO_CREATE_STATISTICS OFF (INCREMENTAL = ON)", new ParserErrorInfo(54, "SQL46010", "INCREMENTAL"));
            ParserTestUtils.ErrorTest120("ALTER DATABASE testdb SET AUTO_CREATE_STATISTICS = OFF", new ParserErrorInfo(26, "SQL46010", "AUTO_CREATE_STATISTICS"));
            ParserTestUtils.ErrorTest120("ALTER DATABASE testdb SET AUTO_CREATE_STATISTICS OF (INCREMENTAL = ON)", new ParserErrorInfo(26, "SQL46010", "AUTO_CREATE_STATISTICS"));
            ParserTestUtils.ErrorTest120("ALTER INDEX foo ON bar REBUILD WITH(STATISTICS_INCREMENTAL = OF);", new ParserErrorInfo(61, "SQL46010", "OF"));
            ParserTestUtils.ErrorTest120("ALTER INDEX foo ON bar REBUILD WITH(STATISTICS_INCREMETAL = OFF);", new ParserErrorInfo(36, "SQL46010", "STATISTICS_INCREMETAL"));
            ParserTestUtils.ErrorTest120("CREATE INDEX foo ON bar(a) WITH(STATISTICS_INCREMENTAL = OF);", new ParserErrorInfo(57, "SQL46010", "OF"));
            ParserTestUtils.ErrorTest120("CREATE INDEX foo ON bar(a) WITH(STATISTICS_INREMENTAL = OFF);", new ParserErrorInfo(32, "SQL46010", "STATISTICS_INREMENTAL"));
            ParserTestUtils.ErrorTest120("CREATE STATISTICS foo ON bar(a) WITH(INREMENTAL = OFF);", new ParserErrorInfo(36, "SQL46010", "("));
            ParserTestUtils.ErrorTest120("CREATE STATISTICS foo ON bar(a) WITH(INCREMENTAL = OF);", new ParserErrorInfo(36, "SQL46010", "("));
            ParserTestUtils.ErrorTest120("UPDATE STATISTICS foo (bar) WITH FULLSCAN, INCREMENTAL = ON, RESAMPLE ON PARTITIONS (1, 3 TO , 10);", new ParserErrorInfo(93, "SQL46010", ","));
            ParserTestUtils.ErrorTest120("UPDATE STATISTICS foo (bar) WITH FULLSCAN, INCREMENTAL = ON, RESAMPLE ON PARTITINS (1, 3, 10);", new ParserErrorInfo(73, "SQL46005", "PARTITIONS", "PARTITINS"));
            ParserTestUtils.ErrorTest120("UPDATE STATISTICS foo (bar) WITH FULLSCAN, INCREMENTAL = ON, RESAMPLE ON PARTITIONS;", new ParserErrorInfo(83, "SQL46010", ";"));
            ParserTestUtils.ErrorTest120("UPDATE STATISTICS foo (bar) WITH FULLSCAN, INCREMENTAL = ON, RESAMPLE ON PARTITIONS();", new ParserErrorInfo(84, "SQL46010", ")"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void Dev10Bug462552()
        {
            ParserTestUtils.ErrorTestAllParsers("CREATE PROCEDURE Proc1 @CurrencyCursor CURSOR AS SET @CurrencyCursor = CURSOR FORWARD_ONLY STATIC FOR SELECT column_1 FROM Table1;",
                new ParserErrorInfo(46, "SQL46010", "AS"));
            ParserTestUtils.ErrorTestAllParsers("CREATE PROCEDURE Proc1 @CurrencyCursor CURSOR VARYING AS SET @CurrencyCursor = CURSOR FORWARD_ONLY STATIC FOR SELECT column_1 FROM Table1;",
                new ParserErrorInfo(54, "SQL46010", "AS"));
            ParserTestUtils.ErrorTestAllParsers("CREATE PROCEDURE Proc1 @CurrencyCursor CURSOR OUTPUT AS SET @CurrencyCursor = CURSOR FORWARD_ONLY STATIC FOR SELECT column_1 FROM Table1;",
                new ParserErrorInfo(46, "SQL46010", "OUTPUT"));

            ParserTestUtils.ErrorTestAllParsers("CREATE FUNCTION [dbo].[Function1] (@param1 CURSOR, @param2 char(5)) RETURNS TABLE (c1 int, c2 char(5)) AS EXTERNAL NAME a1.c1.f1;",
                new ParserErrorInfo(43, "SQL46010", "CURSOR"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46070Test()
        {
            // Tests for checking that duplication of options in DROP INDEX STATEMENT is not allowed
            ParserTestUtils.ErrorTest90AndAbove("DROP INDEX i1 ON t1 WITH (ONLINE = ON, MOVE TO fg1, ONLINE = OFF);",
                new ParserErrorInfo(52, "SQL46049", "ONLINE"));
            ParserTestUtils.ErrorTest90AndAbove("DROP INDEX i1 ON t1 WITH (MAXDOP = 2, MOVE TO fg1, MAXDOP = 3);",
                new ParserErrorInfo(51, "SQL46049", "MAXDOP"));
            ParserTestUtils.ErrorTest90AndAbove("DROP INDEX i1 ON t1 WITH (MOVE TO fg1, MAXDOP = 2, MOVE TO fg2, MAXDOP = 3);",
                new ParserErrorInfo(51, "SQL46049", "MOVE"));
            // Checks for duplication of the new options  - DATA_COMPRESSION and FILESTREAM_ON
            ParserTestUtils.ErrorTest100("DROP INDEX i1 ON t1 WITH (DATA_COMPRESSION = PAGE, MOVE TO fg1, MAXDOP = 3, DATA_COMPRESSION = ROW);",
                new ParserErrorInfo(76, "SQL46049", "DATA_COMPRESSION"));
            ParserTestUtils.ErrorTest100("DROP INDEX i1 ON t1 WITH (FILESTREAM_ON ab, MOVE TO fg1, MAXDOP = 3, FILESTREAM_ON \"ab\");",
                new ParserErrorInfo(69, "SQL46049", "FILESTREAM_ON"));
            ParserTestUtils.ErrorTest140("DROP INDEX i1 ON t1 WITH (WAIT_AT_LOW_PRIORITY (MAX_DURATION = 10 MINUTES, ABORT_AFTER_WAIT = NONE), WAIT_AT_LOW_PRIORITY (MAX_DURATION = 10 MINUTES, ABORT_AFTER_WAIT = NONE));",
                new ParserErrorInfo(101, "SQL46049", "WAIT_AT_LOW_PRIORITY"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46071Test()
        {
            ParserTestUtils.ErrorTestAllParsers("CREATE STATISTICS [stat] ON t1 (c1, c2) WITH FULLSCAN, NORECOMPUTE, SAMPLE 12 ROWS;",
                new ParserErrorInfo(68, "SQL46071"));
            ParserTestUtils.ErrorTestAllParsers("CREATE STATISTICS [stat] ON t1 (c1, c2) WITH FULLSCAN, NORECOMPUTE, SAMPLE 12 PERCENT;",
                new ParserErrorInfo(68, "SQL46071"));
            ParserTestUtils.ErrorTestAllParsers("UPDATE STATISTICS [stat] WITH SAMPLE 12 ROWS, NORECOMPUTE, FULLSCAN;",
                new ParserErrorInfo(59, "SQL46071"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46072Test()
        {
            ParserTestUtils.ErrorTest100("WITH change_tracking_context (0xff),DIRREPS(C1,c2) as (SELECT c1 FROM t1) select c1 from t1",
                new ParserErrorInfo(30, "SQL46072"));
            ParserTestUtils.ErrorTest100("CREATE FUNCTION f1() RETURNS TABLE RETURN (WITH change_tracking_context (0xff),DIRREPS(C1,c2) as (SELECT c1 FROM t1) select c1 from t1);",
                new ParserErrorInfo(73, "SQL46072"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46074Test()
        {
            ParserTestUtils.ErrorTestAllParsers("INSERT INTO pi WITH (INDEX (i1)) DEFAULT VALUES",
                new ParserErrorInfo(21, "SQL46074"));
            ParserTestUtils.ErrorTestAllParsers("DELETE FROM pi WITH (INDEX (i1))",
                new ParserErrorInfo(21, "SQL46074"));
            ParserTestUtils.ErrorTestAllParsers("UPDATE pi WITH (INDEX (i1)) SET c1 = NULL",
                new ParserErrorInfo(16, "SQL46074"));
            ParserTestUtils.ErrorTest100AndAbove("INSERT INTO pi WITH (FORCESEEK (i1 (c1))) DEFAULT VALUES",
                new ParserErrorInfo(21, "SQL46074"));
        }

        #region Sub-DML related tests
        string[] InnerDMLs = new string[4] {
                "INSERT INTO t3 OUTPUT c1 DEFAULT VALUES",
                "UPDATE t3 SET c1 = 10 OUTPUT c1",
                "DELETE FROM t3 OUTPUT c1",
                @"MERGE pi USING t1 ON (pi.PID = t1.PID)
                    WHEN MATCHED THEN UPDATE SET pi.Qty = t1.Qty
                    OUTPUT c1"
            };

        /// <summary>
        /// Checks, that we can't have nested DML inside another nested DML.
        /// Currently, only INSERT allows sub-DMLs
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46075Test()
        {
            TSql100Parser parser = new TSql100Parser(true);

            const string nestingTemplate =
                "INSERT t1 SELECT * FROM (INSERT t1 OUTPUT c1 SELECT * FROM ({0}) AS aInner) AS aOuter";
            int errorPosition = nestingTemplate.IndexOf('{') - 1;


            foreach (string innerDml in InnerDMLs)
            {
                string errorScript = string.Format(CultureInfo.InvariantCulture, nestingTemplate, innerDml);
                //System.Console.WriteLine(errorScript);
                ParserTestUtils.ErrorTest(parser, errorScript,
                    new ParserErrorInfo(errorPosition, "SQL46075"));
            }

            // Verifies, that stuff other than DML is allowed there
            string correctScript = string.Format(CultureInfo.InvariantCulture, nestingTemplate, "SELECT * FROM zzz");
            ParserTestUtils.ErrorTest(parser, correctScript);
        }

        /// <summary>
        /// Checks, that sub-DML are not allowed in SELECT which is not source for insert
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46076Test()
        {
            TSql100Parser parser = new TSql100Parser(true);

            string[] templates = new string[] {
                "IF EXISTS (SELECT * FROM {0}) BEGIN PRINT '' END", // Exists
                "WITH i1 AS (SELECT * FROM {0}) INSERT INTO t1 DEFAULT VALUES", // CTE
                "IF 1 = SOME (SELECT * FROM {0}) BEGIN PRINT '' END", // SOME
                "IF 1 IN (SELECT * FROM {0}) BEGIN PRINT '' END", // IN
                "SELECT TOP (SELECT * FROM {0}) * FROM t1", // TOP in SELECT
                "UPDATE TOP (SELECT * FROM {0}) t1 SET c1 = 10", // TOP in DML
            };

            foreach (string subDml in InnerDMLs)
            {
                string subDmlWithAlias = "(" + subDml + ") AS z";
                foreach (string template in templates)
                {
                    string errorScript = string.Format(CultureInfo.InvariantCulture, template, subDmlWithAlias);
                    ParserTestUtils.ErrorTest(parser, errorScript,
                        new ParserErrorInfo(template.IndexOf('{') + 1, "SQL46076"));

                    string correctScript = string.Format(CultureInfo.InvariantCulture, template, "zzz");
                    ParserTestUtils.ErrorTest(parser, correctScript);
                }
            }
        }

        /// <summary>
        /// Checks, that sub-DMLs are not allowed in UPDATE or DELETE FROM clauses
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46077Test()
        {
            TSql100Parser parser = new TSql100Parser(true);

            const string updateTemplate = "UPDATE t1 SET c1 = 10 FROM ({0}) AS a";
            int updateErrorPosition = updateTemplate.IndexOf('{');
            const string deleteTemplate = "DELETE t1 FROM ({0}) AS a";
            int deleteErrorPosition = deleteTemplate.IndexOf('{');

            foreach (string subDml in InnerDMLs)
            {
                string updateScript = string.Format(CultureInfo.InvariantCulture, updateTemplate, subDml);
                ParserTestUtils.ErrorTest(parser, updateScript,
                    new ParserErrorInfo(updateErrorPosition, "SQL46077"));
                string deleteScript = string.Format(CultureInfo.InvariantCulture, deleteTemplate, subDml);
                ParserTestUtils.ErrorTest(parser, deleteScript,
                    new ParserErrorInfo(deleteErrorPosition, "SQL46077"));
            }
        }

        /// <summary>
        /// Checks, that sub-DMLs are not allowed in USING clause of MERGE statement
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46078Test()
        {
            TSql100Parser parser = new TSql100Parser(true);

            const string mergeTemplate =
                "MERGE t1 USING ({0}) AS a ON (t1.id = a.id) WHEN MATCHED THEN UPDATE SET t1.Qty = a.Qty;";
            int errorPosition = mergeTemplate.IndexOf('{');

            foreach (string subDml in InnerDMLs)
            {
                string errorScript = string.Format(CultureInfo.InvariantCulture, mergeTemplate, subDml);
                ParserTestUtils.ErrorTest(parser, errorScript,
                    new ParserErrorInfo(errorPosition, "SQL46078"));
            }
        }

        /// <summary>
        /// Checks, that sub-DMLs have OUTPUT clause
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46079Test()
        {
            TSql100Parser parser = new TSql100Parser(true);

            const string template = "INSERT INTO t1 SELECT * FROM ({0}) AS a";
            int errorPosition = template.IndexOf('{');

            string[] subDMLsWithoutOutput = new string[4] {
                "INSERT INTO t3 DEFAULT VALUES",
                "UPDATE t3 SET c1 = 10",
                "DELETE FROM t3",
                @"MERGE pi USING t1 ON (pi.PID = t1.PID)
                    WHEN MATCHED THEN UPDATE SET pi.Qty = t1.Qty"
            };

            foreach (string subDml in subDMLsWithoutOutput)
            {
                string errorScript = string.Format(CultureInfo.InvariantCulture, template, subDml);
                ParserTestUtils.ErrorTest(parser, errorScript,
                    new ParserErrorInfo(errorPosition, "SQL46079"));
            }
        }

        /// <summary>
        /// Checks, that WHERE CURRENT OF is not allowed in sub-dmls
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46083Test()
        {
            TSql100Parser parser = new TSql100Parser(true);
            ParserTestUtils.ErrorTest(parser,
                "INSERT t1 SELECT * FROM (UPDATE t3 SET c1 = 10 OUTPUT c1 WHERE CURRENT OF zzz) AS ao;",
                new ParserErrorInfo(57, "SQL46083"));

            ParserTestUtils.ErrorTest(parser,
                "INSERT t1 SELECT * FROM (DELETE t3 OUTPUT c1 WHERE CURRENT OF zzz) AS ao;",
                new ParserErrorInfo(45, "SQL46083"));
        }

        #endregion

        /// <summary>
        /// Checks that OPENROWSETBULK has at SINGLE_BLOB, SINGLE_CLOB, SINGLE_NCLOB or FORMATFILE option
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46082Test()
        {
            ParserTestUtils.ErrorTest90andAboveUntil150(
                "SELECT * FROM OPENROWSET(BULK 'df1') AS a",
                new ParserErrorInfo(30, "SQL46082"));
        }

        /// <summary>
        /// Checks that CUBE, ROLLUP and GROUPING SETS are not allowed in GROUP BY ALL clause
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46084Test()
        {
            ParserTestUtils.ErrorTestAllParsers("SELECT c1 FROM t1 GROUP BY ALL c1 WITH CUBE",
                new ParserErrorInfo(39, "SQL46084"));
            ParserTestUtils.ErrorTestAllParsers("SELECT c1 FROM t1 GROUP BY ALL c1 WITH ROLLUP",
                new ParserErrorInfo(39, "SQL46084"));

            TSql100Parser parser100 = new TSql100Parser(true);
            ParserTestUtils.ErrorTest(parser100, "SELECT c1 FROM t1 GROUP BY ALL CUBE (c1)",
                new ParserErrorInfo(31, "SQL46084"));
            ParserTestUtils.ErrorTest(parser100, "SELECT c1 FROM t1 GROUP BY ALL ROLLUP (c1)",
                new ParserErrorInfo(31, "SQL46084"));
            ParserTestUtils.ErrorTest(parser100, "SELECT c1 FROM t1 GROUP BY ALL GROUPING SETS (())",
                new ParserErrorInfo(31, "SQL46084"));
        }

        /// <summary>
        /// Checks that WITH CUBE and WITH ROLLUP are not allowed when CUBE, ROLLUP or GROUPING SETS specified
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46085Test()
        {
            TSql100Parser parser100 = new TSql100Parser(true);
            ParserTestUtils.ErrorTest(parser100, "SELECT c1 FROM t1 GROUP BY CUBE (c1) WITH ROLLUP",
                new ParserErrorInfo(42, "SQL46085"));
            ParserTestUtils.ErrorTest(parser100, "SELECT c1 FROM t1 GROUP BY ROLLUP (c1) WITH CUBE",
                new ParserErrorInfo(44, "SQL46085"));
            ParserTestUtils.ErrorTest(parser100, "SELECT c1 FROM t1 GROUP BY GROUPING SETS (()) WITH ROLLUP",
                new ParserErrorInfo(51, "SQL46085"));
        }

        /// <summary>
        /// Checks that the column expression or ROLLUP option is not allowed after the WITH DISTRIBUTED_AGG option is specified
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void GroupByDistributedAggErrorTest()
        {
            const string invalidGroupByWithDistributedAgg = "SELECT c1, c2, c3 FROM t1 GROUP BY c1 WITH (DISTRIBUTED_AGG), (c2, c3)";
            const string invalidGroupByWithDistributedAgg1 = "CREATE PROCEDURE sp1 AS BEGIN SELECT c1, c2, c3 FROM t1 GROUP BY c1 WITH (DISTRIBUTED_AGG), (c2, c3) END";
            const string invalidGroupByWithDistributedAgg2 = "SELECT c1, c2, c3 FROM t1 GROUP BY c1 WITH (DISTRIBUTED_AGG), c2 WITH (DISTRIBUTED_AGG), c3";
            const string invalidGroupByWithDistributedAgg3 = "CREATE PROCEDURE sp1 AS BEGIN SELECT c1, c2, c3 FROM t1 GROUP BY c1 WITH (DISTRIBUTED_AGG), c2 WITH (DISTRIBUTED_AGG), c3 END";

            ParserTestUtils.ErrorTest130(invalidGroupByWithDistributedAgg, new ParserErrorInfo(65, "SQL46010", ","));
            ParserTestUtils.ErrorTest140(invalidGroupByWithDistributedAgg, new ParserErrorInfo(65, "SQL46010", ","));
            ParserTestUtils.ErrorTest150(invalidGroupByWithDistributedAgg, new ParserErrorInfo(65, "SQL46010", ","));

            ParserTestUtils.ErrorTest130(invalidGroupByWithDistributedAgg1, new ParserErrorInfo(95, "SQL46010", ","));
            ParserTestUtils.ErrorTest140(invalidGroupByWithDistributedAgg1, new ParserErrorInfo(95, "SQL46010", ","));
            ParserTestUtils.ErrorTest150(invalidGroupByWithDistributedAgg1, new ParserErrorInfo(95, "SQL46010", ","));

            ParserTestUtils.ErrorTest130(invalidGroupByWithDistributedAgg2, new ParserErrorInfo(71, "SQL46129"));
            ParserTestUtils.ErrorTest140(invalidGroupByWithDistributedAgg2, new ParserErrorInfo(71, "SQL46129"));
            ParserTestUtils.ErrorTest150(invalidGroupByWithDistributedAgg2, new ParserErrorInfo(71, "SQL46129"));

            ParserTestUtils.ErrorTest130(invalidGroupByWithDistributedAgg3, new ParserErrorInfo(101, "SQL46129"));
            ParserTestUtils.ErrorTest140(invalidGroupByWithDistributedAgg3, new ParserErrorInfo(101, "SQL46129"));
            ParserTestUtils.ErrorTest150(invalidGroupByWithDistributedAgg3, new ParserErrorInfo(101, "SQL46129"));
        }

        /// <summary>
        /// Checks that DISTINCT is not allowed in aggregate calls with OVER clause
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46086Test()
        {
            // User-defined aggregate
            const string userAggregateWithOverScript = "SELECT dbo.MyAgg(DISTINCT c1) OVER ()";
            // In 100, this is error
            ParserTestUtils.ErrorTest(new TSql100Parser(true),
                userAggregateWithOverScript, new ParserErrorInfo(17, "SQL46086"));
            // but it is fine in 90!
            ParserTestUtils.ErrorTest(new TSql90Parser(true), userAggregateWithOverScript);

            // Built-in aggregate - error in both 90 and 100
            ParserTestUtils.ErrorTest90AndAbove(
                "SELECT SUM(DISTINCT c1) OVER ()", new ParserErrorInfo(11, "SQL46086"));
        }

        /// <summary>
        /// Check that OUTPUT only allowed when passing variable to procedure
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46088Test()
        {
            ParserTestUtils.ErrorTestAllParsers("EXEC zzz 1 OUTPUT",
                new ParserErrorInfo(11, "SQL46088"));
            ParserTestUtils.ErrorTestAllParsers("EXEC ('dynamic sql', 'aaa' OUTPUT)",
                new ParserErrorInfo(27, "SQL46088"));
        }

        /// <summary>
        /// Check that simple parameters ('value') are not allowed in procedure call after '@name = value' syntax
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46089Test()
        {
            ParserTestUtils.ErrorTestAllParsers("EXEC zzz 1, N'q', @v1 = 2, @v2 = 'a'");

            ParserTestUtils.ErrorTestAllParsers("EXEC zzz 1, @v1 = 2, 'a'",
                new ParserErrorInfo(21, "SQL46089", "3"));
            ParserTestUtils.ErrorTestAllParsers("EXEC ('dynamic sql', @v2 = N'zzz', 15)",
                new ParserErrorInfo(35, "SQL46089", "2"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void CreateServerAuditStatementErrorTest()
        {
            // CHECKS IF THE VALUE OF MAX_ROLLOVER_FILES IS LESS THAN 2147483647
            ParserTestUtils.ErrorTest100("create server audit test to file (filepath='c:\abc', MAX_ROLLOVER_FILES = 2147483648)",
                new ParserErrorInfo(73, "SQL46010", "2147483648"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void CreateEndpointStatementErrorTest()
        {
            // CHECKS IF THE VALUE FOR THE OPTIONS AUTH_REALM AND DEFAULT_LOGON_DOMAIN ARE NON-EMPTY STRINGS
            ParserTestUtils.ErrorTest90AndAbove("CREATE ENDPOINT [HttpEndpoint_DefaultLogonDomainEmpty] AS HTTP (PATH = '/D',AUTHENTICATION = (BASIC),PORTS = (SSL),AUTH_REALM = '') FOR SOAP()",
                new ParserErrorInfo(128, "SQL46063", ""));
            ParserTestUtils.ErrorTest90AndAbove("CREATE ENDPOINT [HttpEndpoint_DefaultLogonDomainEmpty] AS HTTP (PATH = '/D',AUTHENTICATION = (BASIC),PORTS = (SSL),DEFAULT_LOGON_DOMAIN = '') FOR SOAP()",
                new ParserErrorInfo(138, "SQL46063", ""));

            // CHECKS IF THE ROLE PARAMETER HAS BEEN SPECIFIED FOR DATABASE_MIRRORING/DATA_MIRRORING OPTION
            ParserTestUtils.ErrorTest90AndAbove("CREATE ENDPOINT [TcpEndpoint_FOR_DATABASE_MIRRORING] AS TCP (LISTENER_PORT = 4022)FOR DATABASE_MIRRORING()",
                new ParserErrorInfo(86, "SQL46080"));
            ParserTestUtils.ErrorTest90AndAbove("CREATE ENDPOINT e1 AS TCP(LISTENER_PORT = 4022) FOR DATA_MIRRORING(ENCRYPTION = SUPPORTED, AUTHENTICATION = WINDOWS)",
                new ParserErrorInfo(52, "SQL46080"));

            // CHECKS IF THE VALUE FOR LISTENER_PORT OPTION IS WITHIN THE ALLOWED RANGE (1024 - 32767)
            ParserTestUtils.ErrorTest90AndAbove("CREATE ENDPOINT [Endpoint1] AS TCP (LISTENER_PORT = 1000)FOR SERVICE_BROKER()",
                new ParserErrorInfo(52, "SQL46087", "1000"));
            ParserTestUtils.ErrorTest90AndAbove("CREATE ENDPOINT [Endpoint1] AS TCP (LISTENER_PORT = 32768)FOR SERVICE_BROKER()",
                new ParserErrorInfo(52, "SQL46087", "32768"));

        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void CreateIndexStatementErrorTest()
        {
            ParserTestUtils.ErrorTest100("CREATE INDEX ind1 ON t1(c1) WHERE (...f1.IDENTITYCOL IS NULL)",
                new ParserErrorInfo(35, "SQL46028"));
            ParserTestUtils.ErrorTestAllParsers("CREATE INDEX ind1 ON t1(t1.c1)",
                new ParserErrorInfo(27, "SQL46010", "c1"));
            ParserTestUtils.ErrorTest110("CREATE NONCLUSTERED COLUMNSTORE INDEX cindx ON t",
                new ParserErrorInfo(47, "SQL46010", "t"));
            ParserTestUtils.ErrorTest110("CREATE COLUMNSTORE INDEX cindx ON t",
                new ParserErrorInfo(34, "SQL46010", "t"));

            // Incorrect syntax of Ordered Columns
            ParserTestUtils.ErrorTest130("CREATE CLUSTERED COLUMNSTORE INDEX cindx ON t (col1, col2)",
               new ParserErrorInfo(47, "SQL46010", "col1"));
        }

        /// <summary>
        /// JSON Index error tests - ensure JSON Index syntax is rejected in older versions and malformed syntax produces appropriate errors
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void CreateJsonIndexStatementErrorTest()
        {
            // JSON Index syntax should not be supported in SQL Server versions prior to 2025 (TSql170)
            // Test basic JSON Index syntax in older versions
            ParserTestUtils.ErrorTest160("CREATE JSON INDEX idx1 ON table1 (jsonColumn)",
                new ParserErrorInfo(7, "SQL46010", "JSON"));
            ParserTestUtils.ErrorTest150("CREATE JSON INDEX idx1 ON table1 (jsonColumn)",
                new ParserErrorInfo(7, "SQL46010", "JSON"));
            ParserTestUtils.ErrorTest140("CREATE JSON INDEX idx1 ON table1 (jsonColumn)",
                new ParserErrorInfo(7, "SQL46010", "JSON"));
            ParserTestUtils.ErrorTest130("CREATE JSON INDEX idx1 ON table1 (jsonColumn)",
                new ParserErrorInfo(7, "SQL46010", "JSON"));
            ParserTestUtils.ErrorTest120("CREATE JSON INDEX idx1 ON table1 (jsonColumn)",
                new ParserErrorInfo(7, "SQL46010", "JSON"));
            ParserTestUtils.ErrorTest110("CREATE JSON INDEX idx1 ON table1 (jsonColumn)",
                new ParserErrorInfo(7, "SQL46010", "JSON"));



            // Test that UNIQUE and CLUSTERED/NONCLUSTERED are not allowed with JSON indexes in TSql170
            TSql170Parser parser170 = new TSql170Parser(true);
            ParserTestUtils.ErrorTest(parser170, "CREATE UNIQUE JSON INDEX idx1 ON table1 (jsonColumn)",
                new ParserErrorInfo(14, "SQL46010", "JSON"));
            ParserTestUtils.ErrorTest(parser170, "CREATE CLUSTERED JSON INDEX idx1 ON table1 (jsonColumn)",
                new ParserErrorInfo(17, "SQL46005", "COLUMNSTORE", "JSON"));
            ParserTestUtils.ErrorTest(parser170, "CREATE NONCLUSTERED JSON INDEX idx1 ON table1 (jsonColumn)",
                new ParserErrorInfo(20, "SQL46005", "COLUMNSTORE", "JSON"));

            // Test malformed JSON Index syntax in TSql170
            // Missing column specification
            ParserTestUtils.ErrorTest(parser170, "CREATE JSON INDEX idx1 ON table1",
                new ParserErrorInfo(32, "SQL46029"));
            
            // Empty FOR clause
            ParserTestUtils.ErrorTest(parser170, "CREATE JSON INDEX idx1 ON table1 (jsonColumn) FOR ()",
                new ParserErrorInfo(51, "SQL46010", ")"));
            
            // Invalid JSON path (missing quotes)
            ParserTestUtils.ErrorTest(parser170, "CREATE JSON INDEX idx1 ON table1 (jsonColumn) FOR ($.name)",
                new ParserErrorInfo(51, "SQL46010", "$"));

            // Missing table name
            ParserTestUtils.ErrorTest(parser170, "CREATE JSON INDEX idx1 ON (jsonColumn)",
                new ParserErrorInfo(26, "SQL46010", "("));
        }

        /// <summary>
        /// Check that the value of MAXDOP index option is within range
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46091Test()
        {
            ParserTestUtils.ErrorTest90AndAbove("CREATE INDEX ind1 ON dbo.t1 (c1) WITH (MAXDOP = 40000)",
                new ParserErrorInfo(48, "SQL46091", "40000"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void RouteOptionDuplicationErrorTest()
        {
            string duplicateServiceNameSyntax = @"CREATE ROUTE [Route1] WITH
    SERVICE_NAME = '//Adventure-Works.com/Expenses',
    SERVICE_NAME = '//Adventure-Works.com/Expenses'";
            ParserTestUtils.ErrorTest90AndAbove(
                duplicateServiceNameSyntax,
                new ParserErrorInfo(duplicateServiceNameSyntax.IndexOf(@"SERVICE_NAME", 75), "SQL46049", "SERVICE_NAME"));

            ParserTestUtils.ErrorTest90AndAbove(
@"CREATE ROUTE [Route1] WITH LIFETIME = 10, LIFETIME = 10",
                new ParserErrorInfo(42, "SQL46049", "LIFETIME"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46092Test()
        {
            ParserTestUtils.ErrorTestAllParsers("CREATE VIEW #v_temp AS SELECT * FROM sysobjects",
                new ParserErrorInfo(12, "SQL46092", "#v_temp"));

            ParserTestUtils.ErrorTestAllParsers("ALTER VIEW dbo.#v_temp AS SELECT * FROM sysobjects",
                new ParserErrorInfo(11, "SQL46092", "#v_temp"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46093Test()
        {
            ParserTestUtils.ErrorTestAllParsers("CREATE FUNCTION #fun_test (@param1 int) RETURNS TABLE AS RETURN (SELECT @param1 AS c1)",
                new ParserErrorInfo(16, "SQL46093", "#fun_test"));

            ParserTestUtils.ErrorTestAllParsers("ALTER FUNCTION dbo.#fun_test (@param1 int) RETURNS TABLE AS RETURN (SELECT @param1 AS c1)",
                new ParserErrorInfo(15, "SQL46093", "#fun_test"));
        }

        /// <summary>
        /// Check that the PERCENT value is between 0 and 100
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46094Test()
        {
            TSql80Parser parser80 = new TSql80Parser(true);
            ParserTestUtils.ErrorTest(parser80, "CREATE VIEW [dbo].[View1] AS SELECT top 105.5 PERCENT * FROM Table1",
                new ParserErrorInfo(40, "SQL46094"));

            ParserTestUtils.ErrorTest90AndAbove("CREATE VIEW [dbo].[View1] AS SELECT top (-5) PERCENT * FROM Table1",
                new ParserErrorInfo(41, "SQL46094"));

            ParserTestUtils.ErrorTest90AndAbove("CREATE VIEW [dbo].[View1] AS SELECT top (105) PERCENT * FROM Table1",
                new ParserErrorInfo(41, "SQL46094"));

            ParserTestUtils.ErrorTest90AndAbove("CREATE VIEW [dbo].[View1] AS SELECT top (102.5) PERCENT * FROM Table1",
                new ParserErrorInfo(41, "SQL46094"));

            ParserTestUtils.ErrorTestAllParsers("CREATE VIEW [dbo].[View1] AS SELECT top 130 PERCENT * FROM Table1",
                new ParserErrorInfo(40, "SQL46094"));
        }

        /// <summary>
        /// Check that the identifiers are less than 128 characters
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46095Test()
        {
            string longIdentifier = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA";
            ParserTestUtils.ErrorTestAllParsers(String.Format("create table {0}(c1 int)", longIdentifier),
                new ParserErrorInfo(13, "SQL46095", longIdentifier.Substring(0, 128)));

            ParserTestUtils.ErrorTestAllParsers(String.Format("exec p {0}", longIdentifier),
                new ParserErrorInfo(7, "SQL46095", longIdentifier.Substring(0, 128)));

            ParserTestUtils.ErrorTestAllParsers(String.Format("exec p [{0}]", longIdentifier),
                new ParserErrorInfo(7, "SQL46095", longIdentifier.Substring(0, 128)));
        }

        /// <summary>
        /// Check contained options are not specified on non-contained users
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46096Test()
        {
            ParserTestUtils.ErrorTest110("CREATE USER [BadUser] for login [login1] WITH password = 'PLACEHOLDER1'",
                new ParserErrorInfo(46, "SQL46096", "password"));
            ParserTestUtils.ErrorTest110("CREATE USER [BadUser] for login [login1] WITH default_language = 1033",
                new ParserErrorInfo(46, "SQL46096", "default_language"));
            ParserTestUtils.ErrorTest110("CREATE USER [BadUser] for login [login1] WITH sid = 0xDEADBEEF",
                new ParserErrorInfo(46, "SQL46096", "sid"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TableHintAsParamTest()
        {
            // This should be ok
            ParserTestUtils.ErrorTestAllParsers("SELECT * FROM t1(NOLOCK) AS S");

            // But this is not - alias is not allowed after hints (unless there is single hint which looks like parameter)
            ParserTestUtils.ErrorTest90AndAbove("SELECT * FROM t1 WITH(NOLOCK) AS S",
                new ParserErrorInfo(30, "SQL46010", "AS"));
            // This is function call, also ok for all parsers 90 and above because 80 parser must support multiple
            // old-style table hints and will thus parse this as a table reference (see IsTableReference())
            ParserTestUtils.ErrorTest90AndAbove("SELECT * FROM t1(NOLOCK, c1) AS S");
            ParserTestUtils.ErrorTestAllParsers("SELECT * FROM t1(c0, c1) AS S");
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void CreateMessageTypeStatementErrorTest()
        {
            // CREATE MESSAGE TYPE m1  VALIDATION = VALID_XML
            // This is ok in Sql 2005 but not in Sql 2008

            // Sql 2005
            TSql90Parser parser90 = new TSql90Parser(true);
            ParserTestUtils.ErrorTest(parser90, "CREATE MESSAGE TYPE m1  VALIDATION = VALID_XML");

            // Sql 2008
            ParserTestUtils.ErrorTest100("CREATE MESSAGE TYPE m1  VALIDATION = VALID_XML",
                new ParserErrorInfo(37, "SQL46010", "VALID_XML"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46098Test()
        {
            // Missing FILENAME option should cause an error...
            ParserTestUtils.ErrorTestAllParsers("alter table t add constraint def default (select c1 from t where c1 = 5) for a",
                new ParserErrorInfo(42, "SQL46098"));
            ParserTestUtils.ErrorTestAllParsers("alter table t add constraint chk check ((select c1 from t where c1 = 5) = 42)",
                new ParserErrorInfo(41, "SQL46098"));
            ParserTestUtils.ErrorTestAllParsers("alter table t add constraint chk check (exists (select * from t))",
                new ParserErrorInfo(48, "SQL46098"));
            ParserTestUtils.ErrorTestAllParsers("alter table t add constraint chk check (x in (select * from t))",
                new ParserErrorInfo(46, "SQL46098"));
            ParserTestUtils.ErrorTestAllParsers("alter table t add constraint chk check (x between (select * from t) and 42)",
                new ParserErrorInfo(51, "SQL46098"));
            ParserTestUtils.ErrorTestAllParsers("alter table t add constraint chk check (x like (select * from t))",
                new ParserErrorInfo(48, "SQL46098"));
            ParserTestUtils.ErrorTestAllParsers("alter table t add constraint chk check ((Case c when foo then (select * from t) when bar then (select * from t)))",
                new ParserErrorInfo(63, "SQL46098"));
            ParserTestUtils.ErrorTestAllParsers("alter table t add constraint chk check ((Case When col1 = (select * from t) Then 1 else 0 end) = 0)",
                new ParserErrorInfo(59, "SQL46098"));
            ParserTestUtils.ErrorTestAllParsers("alter table t add constraint chk check ((Case When col1 = 42 Then (select * from t) else 0 end) = 0)",
                new ParserErrorInfo(67, "SQL46098"));
            ParserTestUtils.ErrorTestAllParsers("alter table t add constraint chk check ((select * from t) is null)",
                new ParserErrorInfo(41, "SQL46098"));
            ParserTestUtils.ErrorTestAllParsers("alter table t add constraint chk check (A1 in (((select c1 from t1) union select * from t1) union select * from t1))",
                new ParserErrorInfo(47, "SQL46098"));
            ParserTestUtils.ErrorTestAllParsers("alter table t add constraint chk check (nullif((select * from t), (select * from t)) is null)",
                new ParserErrorInfo(48, "SQL46098"));
            ParserTestUtils.ErrorTestAllParsers("alter table t add constraint chk check (coalesce((select * from t), (select * from t)))",
                new ParserErrorInfo(50, "SQL46098"));
            ParserTestUtils.ErrorTestAllParsers("CREATE TABLE T1 (C1 AS (SELECT C2 FROM T1), C2 int)",
                new ParserErrorInfo(24, "SQL46098"));
            ParserTestUtils.ErrorTestAllParsers("CREATE TABLE T (col int CONSTRAINT def DEFAULT (select c1 from t where c1 = 5) for a);",
                new ParserErrorInfo(48, "SQL46098"));
            ParserTestUtils.ErrorTestAllParsers("CREATE TABLE T (col money CONSTRAINT salary_cap CHECK (salary < (select c1 from t where c1 = 5)));",
                new ParserErrorInfo(65, "SQL46098"));

        }

        /// <summary>
        /// Check that frame bounds that are not supported by RANGE are not allowed
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46099Test()
        {
            ParserTestUtils.ErrorTest110("SELECT SUM(a) OVER (ORDER BY shuffled_id RANGE BETWEEN 1 FOLLOWING AND 1 PRECEDING) FROM table",
                new ParserErrorInfo(41, "SQL46099"));
        }

        /// <summary>
        /// Check that invalid frame specification inside Over clause is caught.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46100Test()
        {
            ParserTestUtils.ErrorTest110("SELECT id, AVG(f) OVER ( ORDER BY shuffled_id  ROWS BETWEEN 1 FOLLOWING AND 1 PRECEDING) FROM table",
                new ParserErrorInfo(47, "SQL46100"));
            ParserTestUtils.ErrorTest110("SELECT id, AVG(f) OVER ( ORDER BY shuffled_id  ROWS 1 FOLLOWING) FROM table",
                new ParserErrorInfo(47, "SQL46100"));
            ParserTestUtils.ErrorTest110("SELECT id, AVG(f) OVER ( ORDER BY shuffled_id  ROWS UNBOUNDED FOLLOWING) FROM table",
                new ParserErrorInfo(47, "SQL46100"));
            ParserTestUtils.ErrorTest110("SELECT id, AVG(f) OVER ( ORDER BY shuffled_id  ROWS BETWEEN 1 FOLLOWING AND UNBOUNDED PRECEDING) FROM table",
                new ParserErrorInfo(47, "SQL46100"));
            ParserTestUtils.ErrorTest110("SELECT id, AVG(f) OVER ( ORDER BY shuffled_id  ROWS BETWEEN UNBOUNDED FOLLOWING AND UNBOUNDED PRECEDING) FROM table",
                new ParserErrorInfo(47, "SQL46100"));
            ParserTestUtils.ErrorTest110("SELECT id, AVG(f) OVER ( ORDER BY shuffled_id  ROWS BETWEEN 2 FOLLOWING AND CURRENT ROW) FROM table",
                new ParserErrorInfo(47, "SQL46100"));
            ParserTestUtils.ErrorTest110("SELECT id, AVG(f) OVER ( ORDER BY shuffled_id  ROWS BETWEEN CURRENT ROW AND 1 PRECEDING) FROM table",
                new ParserErrorInfo(47, "SQL46100"));
        }

        /// <summary>
        /// Check the max_duration value of low priority lock wait option.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46101Test()
        {
            ParserTestUtils.ErrorTest120("ALTER INDEX idx1 ON t1 REBUILD WITH (ONLINE = ON (WAIT_AT_LOW_PRIORITY (MAX_DURATION = 71583, ABORT_AFTER_WAIT = NONE)))",
                new ParserErrorInfo(87, "SQL46101", "71583"));
            ParserTestUtils.ErrorTest120("ALTER TABLE t1 REBUILD WITH (ONLINE = ON (WAIT_AT_LOW_PRIORITY (MAX_DURATION = 71583, ABORT_AFTER_WAIT = NONE)))",
                new ParserErrorInfo(79, "SQL46101", "71583"));
            ParserTestUtils.ErrorTest120("ALTER TABLE t1 SWITCH TO t2 WITH (WAIT_AT_LOW_PRIORITY (MAX_DURATION = 71583, ABORT_AFTER_WAIT = NONE))",
                new ParserErrorInfo(71, "SQL46101", "71583"));
        }

        /// <summary>
        /// Check the max_duration value of low priority lock wait option.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SQL46102Test()
        {
            ParserTestUtils.ErrorTest120("ALTER INDEX idx1 ON t1 REBUILD WITH (ONLINE = ON (WAIT_AT_LOW_PRIORITY (MAX_DURATION = 0, ABORT_AFTER_WAIT = SELF)))",
                new ParserErrorInfo(87, "SQL46102", "0", "SELF"));
            ParserTestUtils.ErrorTest120("ALTER TABLE t1 REBUILD WITH (ONLINE = ON (WAIT_AT_LOW_PRIORITY (MAX_DURATION = 0, ABORT_AFTER_WAIT = SELF)))",
                new ParserErrorInfo(79, "SQL46102", "0", "SELF"));
            ParserTestUtils.ErrorTest120("ALTER TABLE t1 SWITCH TO t2 WITH (WAIT_AT_LOW_PRIORITY (MAX_DURATION = 0, ABORT_AFTER_WAIT = SELF))",
                new ParserErrorInfo(71, "SQL46102", "0", "SELF"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SecurityPolicyStatementErrorsTest()
        {
            // Three part names
            //
            ParserTestUtils.ErrorTest130("CREATE SECURITY POLICY [database].[dbo].sec1 ADD FILTER PREDICATE [dbo].f1(c1) ON dbo.t1", new ParserErrorInfo(23, "SQL46021", "SECURITY_POLICY"));
            ParserTestUtils.ErrorTest130("CREATE SECURITY POLICY [dbo].sec1 ADD FILTER PREDICATE [dbo].f1(c1) ON [database].[dbo].t1", new ParserErrorInfo(87, "SQL46010", "."));
            ParserTestUtils.ErrorTest130("ALTER SECURITY POLICY [database].[dbo].sec1 ADD FILTER PREDICATE [dbo].f1(c1) ON dbo.t1", new ParserErrorInfo(22, "SQL46021", "SECURITY_POLICY"));
            ParserTestUtils.ErrorTest130("ALTER SECURITY POLICY [dbo].sec1 ADD FILTER PREDICATE [dbo].f1(c1) ON [database].[dbo].t1", new ParserErrorInfo(86, "SQL46010", "."));
            ParserTestUtils.ErrorTest130("ALTER SECURITY POLICY [dbo].sec1 ALTER FILTER PREDICATE [dbo].f1(c1) ON [database].[dbo].t1", new ParserErrorInfo(88, "SQL46010", "."));
            ParserTestUtils.ErrorTest130("ALTER SECURITY POLICY [dbo].sec1 DROP FILTER PREDICATE ON [database].[dbo].t1", new ParserErrorInfo(74, "SQL46010", "."));
            ParserTestUtils.ErrorTest130("DROP SECURITY POLICY [database].[dbo].sec2", new ParserErrorInfo(21, "SQL46021", "DROP"));

            // ALTER DDL with multiple action types
            //
            ParserTestUtils.ErrorTest130("ALTER SECURITY POLICY [dbo].sec1 ALTER FILTER PREDICATE [dbo].f1(c1) ON [dbo].t1 WITH (STATE = ON)", new ParserErrorInfo(86, "SQL46010", "("));
            ParserTestUtils.ErrorTest130("ALTER SECURITY POLICY [dbo].sec1 ALTER FILTER PREDICATE [dbo].f1(c1) ON [dbo].t1 NOT FOR REPLICATION", new ParserErrorInfo(81, "SQL46010", "NOT"));
            ParserTestUtils.ErrorTest130("ALTER SECURITY POLICY [dbo].sec1 WITH (STATE = ON) NOT FOR REPLICATION", new ParserErrorInfo(51, "SQL46010", "NOT"));

            // Create Keyword typos
            //
            ParserTestUtils.ErrorTest130("CREAT SECURITY POLICY [dbo].sec1 ADD FILTER PREDICATE [dbo].f1(c1) ON dbo.t1", new ParserErrorInfo(15, "SQL46010", "POLICY"));
            ParserTestUtils.ErrorTest130("CREATE SECRUITY POLICY [dbo].sec1 ADD FILTER PREDICATE [dbo].f1(c1) ON dbo.t1", new ParserErrorInfo(7, "SQL46010", "SECRUITY"));
            ParserTestUtils.ErrorTest130("CREATE SECURITY POLICYY [dbo].sec1 ADD FILTER PREDICATE [dbo].f1(c1) ON dbo.t1", new ParserErrorInfo(16, "SQL46005", "POLICY", "POLICYY"));
            ParserTestUtils.ErrorTest130("CREATE SECURITY POLICY [dbo].sec1 CREATE FILTER PREDICATE [dbo].f1(c1) ON dbo.t1", new ParserErrorInfo(41, "SQL46010", "FILTER"));
            ParserTestUtils.ErrorTest130("CREATE SECURITY POLICY [dbo].sec1 AD FILTER PREDICATE [dbo].f1(c1) ON dbo.t1", new ParserErrorInfo(34, "SQL46010", "AD"));
            ParserTestUtils.ErrorTest130("CREATE SECURITY POLICY [dbo].sec1 ADD FILER PREDICATE [dbo].f1(c1) ON dbo.t1", new ParserErrorInfo(38, "SQL46010", "FILER"));
            ParserTestUtils.ErrorTest130("CREATE SECURITY POLICY [dbo].sec1 ADD FILTER PERDICATE [dbo].f1(c1) ON dbo.t1", new ParserErrorInfo(45, "SQL46005", "PREDICATE", "PERDICATE"));
            ParserTestUtils.ErrorTest130("CREATE SECURITY POLICY [dbo].sec1 ADD FILTER PREDICATE [dbo].f1(c1) TO dbo.t1", new ParserErrorInfo(68, "SQL46010", "TO"));
            ParserTestUtils.ErrorTest130("CREATE SECURITY POLICY [dbo].sec1 ADD FILTER PREDICATE [dbo].f1(c1) ONN dbo.t1", new ParserErrorInfo(68, "SQL46010", "ONN"));
            ParserTestUtils.ErrorTest130("CREATE SECURITY POLICY [dbo].sec1 ADD FILTER PREDICATE [dbo].f1(c1) ON dbo.t1 WIT (STATE = ON)", new ParserErrorInfo(78, "SQL46010", "WIT"));
            ParserTestUtils.ErrorTest130("CREATE SECURITY POLICY [dbo].sec1 ADD FILTER PREDICATE [dbo].f1(c1) ON dbo.t1 WITH S(TATE = ON)", new ParserErrorInfo(90, "SQL46010", "="));
            ParserTestUtils.ErrorTest130("CREATE SECURITY POLICY [dbo].sec1 ADD FILTER PREDICATE [dbo].f1(c1) ON dbo.t1 WITH (STAE = ON)", new ParserErrorInfo(84, "SQL46010", "STAE"));
            ParserTestUtils.ErrorTest130("CREATE SECURITY POLICY [dbo].sec1 ADD FILTER PREDICATE [dbo].f1(c1) ON dbo.t1 WITH (STATE = N)", new ParserErrorInfo(92, "SQL46010", "N"));
            ParserTestUtils.ErrorTest130("CREATE SECURITY POLICY [dbo].sec1 ADD FILTER PREDICATE [dbo].f1(c1) ON dbo.t1 NO FOR REPLICATION", new ParserErrorInfo(78, "SQL46010", "NO"));
            ParserTestUtils.ErrorTest130("CREATE SECURITY POLICY [dbo].sec1 ADD FILTER PREDICATE [dbo].f1(c1) ON dbo.t1 NOT FOUR REPLICATION", new ParserErrorInfo(82, "SQL46010", "FOUR"));
            ParserTestUtils.ErrorTest130("CREATE SECURITY POLICY [dbo].sec1 ADD FILTER PREDICATE [dbo].f1(c1) ON dbo.t1 NOT FOR RELPICATION", new ParserErrorInfo(86, "SQL46010", "RELPICATION"));
            ParserTestUtils.ErrorTest130("CREATE SECURITY POLICY [dbo].sec1 ADD BLOC PREDICATE [dbo].f1(c1) ON dbo.t1", new ParserErrorInfo(38, "SQL46010", "BLOC"));
            ParserTestUtils.ErrorTest130("CREATE SECURITY POLICY [dbo].sec1 ADD BLOCK PREDICATE [dbo].f1(c1) ON dbo.t1 AFTER INSER", new ParserErrorInfo(77, "SQL46010", "AFTER"));
            ParserTestUtils.ErrorTest130("CREATE SECURITY POLICY [dbo].sec1 ADD BLOCK PREDICATE [dbo].f1(c1) ON dbo.t1 ATER INSERT", new ParserErrorInfo(77, "SQL46005", "AFTER", "ATER"));
            ParserTestUtils.ErrorTest130("CREATE SECURITY POLICY [dbo].sec1 ADD BLOCK PREDICATE [dbo].f1(c1) ON dbo.t1 AFTER UPDAT", new ParserErrorInfo(77, "SQL46010", "AFTER"));
            ParserTestUtils.ErrorTest130("CREATE SECURITY POLICY [dbo].sec1 ADD BLOCK PREDICATE [dbo].f1(c1) ON dbo.t1 ATER UPDATE", new ParserErrorInfo(77, "SQL46010", "ATER"));
            ParserTestUtils.ErrorTest130("CREATE SECURITY POLICY [dbo].sec1 ADD BLOCK PREDICATE [dbo].f1(c1) ON dbo.t1 BEFORE UPDAT", new ParserErrorInfo(77, "SQL46010", "BEFORE"));
            ParserTestUtils.ErrorTest130("CREATE SECURITY POLICY [dbo].sec1 ADD BLOCK PREDICATE [dbo].f1(c1) ON dbo.t1 BEFOR UPDATE", new ParserErrorInfo(77, "SQL46010", "BEFOR"));
            ParserTestUtils.ErrorTest130("CREATE SECURITY POLICY [dbo].sec1 ADD BLOCK PREDICATE [dbo].f1(c1) ON dbo.t1 BEFORE DELET", new ParserErrorInfo(77, "SQL46010", "BEFORE"));
            ParserTestUtils.ErrorTest130("CREATE SECURITY POLICY [dbo].sec1 ADD BLOCK PREDICATE [dbo].f1(c1) ON dbo.t1 BEFOR DELETE", new ParserErrorInfo(77, "SQL46005", "BEFORE", "BEFOR"));
            ParserTestUtils.ErrorTest130("CREATE SECURITY POLICY [dbo].sec1 ADD BLOCK PREDICATE [dbo].f1(c1 ON dbo.t1 BEFORE DELETE", new ParserErrorInfo(66, "SQL46010", "ON"));

            // Duplicate or invalid options in CREATE statements.
            //
            ParserTestUtils.ErrorTest130("CREATE SECURITY POLICY [dbo].sec1 ADD FILTER PREDICATE [dbo].f1(c1) ON dbo.t1 WITH (STATE = ON, STATE = ON)", new ParserErrorInfo(96, "SQL46049", "STATE"));
            ParserTestUtils.ErrorTest130("CREATE SECURITY POLICY [dbo].sec1 ADD FILTER PREDICATE [dbo].f1(c1) ON dbo.t1 WITH (STATE = ON, STATE = OFF)", new ParserErrorInfo(96, "SQL46049", "STATE"));
            ParserTestUtils.ErrorTest130("CREATE SECURITY POLICY [dbo].sec1 ADD FILTER PREDICATE [dbo].f1(c1) ON dbo.t1 WITH (STATE = ON, SCHEMABINDING = ON, STATE = ON)", new ParserErrorInfo(116, "SQL46049", "STATE"));
            ParserTestUtils.ErrorTest130("CREATE SECURITY POLICY [dbo].sec1 ADD FILTER PREDICATE [dbo].f1(c1) ON dbo.t1 WITH (SCHEMABINDING = ON, SCHEMABINDING = OFF)", new ParserErrorInfo(104, "SQL46049", "SCHEMABINDING"));
            ParserTestUtils.ErrorTest130("CREATE SECURITY POLICY [dbo].sec1 ADD FILTER PREDICATE [dbo].f1(c1) ON dbo.t1 WITH (STATE = ON, NOTANOPTION = OFF, SCHEMABINDING = ON)", new ParserErrorInfo(96, "SQL46010", "NOTANOPTION"));
            ParserTestUtils.ErrorTest130("CREATE SECURITY POLICY [dbo].sec1 ADD FILTER PREDICATE [dbo].f1(c1) ON dbo.t1 WITH (SCHEMABINDING = 5)", new ParserErrorInfo(100, "SQL46010", "5"));

            // Creates with Granular Operations on Filter Predicates.
            //
            ParserTestUtils.ErrorTest130("CREATE SECURITY POLICY [dbo].sec1 ADD FILTER PREDICATE [dbo].f1(c1) ON dbo.t1 AFTER UPDATE NOT FOR REPLICATION", new ParserErrorInfo(78, "SQL46010", "AFTER"));
            ParserTestUtils.ErrorTest130("CREATE SECURITY POLICY [dbo].sec1 ADD FILTER PREDICATE [dbo].f1(c1) ON dbo.t1 BEFORE UPDATE", new ParserErrorInfo(78, "SQL46010", "BEFORE"));
            ParserTestUtils.ErrorTest130("CREATE SECURITY POLICY [dbo].sec1 ADD FILTER PREDICATE [dbo].f1(c1) ON dbo.t1 AFTER INSERT NOT FOR REPLICATION", new ParserErrorInfo(78, "SQL46010", "AFTER"));
            ParserTestUtils.ErrorTest130("CREATE SECURITY POLICY [dbo].sec1 ADD FILTER PREDICATE [dbo].f1(c1) ON dbo.t1 BEFORE DELETE", new ParserErrorInfo(78, "SQL46010", "BEFORE"));

            // Alter Keyword Typos
            //
            ParserTestUtils.ErrorTest130("LTER SECURITY POLICY [dbo].sec1 ADD FILTER PREDICATE [dbo].f1(c1) ON dbo.t1", new ParserErrorInfo(14, "SQL46010", "POLICY"));
            ParserTestUtils.ErrorTest130("ALTER SECRUITY POLICY [dbo].sec1 ADD FILTER PREDICATE [dbo].f1(c1) ON dbo.t1", new ParserErrorInfo(6, "SQL46010", "SECRUITY"));
            ParserTestUtils.ErrorTest130("ALTER SECURITY POLICYY [dbo].sec1 ADD FILTER PREDICATE [dbo].f1(c1) ON dbo.t1", new ParserErrorInfo(15, "SQL46005", "POLICY", "POLICYY"));
            ParserTestUtils.ErrorTest130("ALTER SECURITY POLICY [dbo].sec1 CREATE FILTER PREDICATE [dbo].f1(c1) ON dbo.t1", new ParserErrorInfo(33, "SQL46010", "CREATE"), new ParserErrorInfo(40, "SQL46010", "FILTER"));
            ParserTestUtils.ErrorTest130("ALTER SECURITY POLICY [dbo].sec1 AD FILTER PREDICATE [dbo].f1(c1) ON dbo.t1", new ParserErrorInfo(33, "SQL46010", "AD"));
            ParserTestUtils.ErrorTest130("ALTER SECURITY POLICY [dbo].sec1 ALER FILTER PERDICATE [dbo].f1(c1) ON dbo.t1", new ParserErrorInfo(33, "SQL46010", "ALER"));
            ParserTestUtils.ErrorTest130("ALTER SECURITY POLICY [dbo].sec1 DRAP FILTER PERDICATE ON dbo.t1", new ParserErrorInfo(33, "SQL46010", "DRAP"));
            ParserTestUtils.ErrorTest130("ALTER SECURITY POLICY [dbo].sec1 DROP FILTER PREDICATE [dbo].f1(c1) ON dbo.t1", new ParserErrorInfo(55, "SQL46010", "[dbo]"));
            ParserTestUtils.ErrorTest130("ALTER SECURITY POLICY [dbo].sec1 WIT (STATE = ON)", new ParserErrorInfo(33, "SQL46010", "WIT"));
            ParserTestUtils.ErrorTest130("ALTER SECURITY POLICY [dbo].sec1 WITH S(TATE = ON)", new ParserErrorInfo(38, "SQL46010", "S"));
            ParserTestUtils.ErrorTest130("ALTER SECURITY POLICY [dbo].sec1 WITH (STAE = ON)", new ParserErrorInfo(39, "SQL46010", "STAE"));
            ParserTestUtils.ErrorTest130("ALTER SECURITY POLICY [dbo].sec1 WITH (STATE = N)", new ParserErrorInfo(47, "SQL46010", "N"));
            ParserTestUtils.ErrorTest130("ALTER SECURITY POLICY [dbo].sec1 DD NO FOR REPLICATION", new ParserErrorInfo(33, "SQL46010", "DD"));
            ParserTestUtils.ErrorTest130("ALTER SECURITY POLICY [dbo].sec1 ADD NO FOR REPLICATION", new ParserErrorInfo(40, "SQL46010", "FOR"));
            ParserTestUtils.ErrorTest130("ALTER SECURITY POLICY [dbo].sec1 ADD NOT FOUR REPLICATION", new ParserErrorInfo(41, "SQL46010", "FOUR"));
            ParserTestUtils.ErrorTest130("ALTER SECURITY POLICY [dbo].sec1 ADD NOT FOR RELPICATION", new ParserErrorInfo(45, "SQL46010", "RELPICATION"));
            ParserTestUtils.ErrorTest130("ALTER SECURITY POLICY [dbo].sec1 DORP NO FOR REPLICATION", new ParserErrorInfo(33, "SQL46010", "DORP"));
            ParserTestUtils.ErrorTest130("ALTER SECURITY POLICY [dbo].sec1 DROP NO FOR REPLICATION", new ParserErrorInfo(41, "SQL46010", "FOR"));
            ParserTestUtils.ErrorTest130("ALTER SECURITY POLICY [dbo].sec1 DROP NOT FOUR REPLICATION", new ParserErrorInfo(42, "SQL46010", "FOUR"));
            ParserTestUtils.ErrorTest130("ALTER SECURITY POLICY [dbo].sec1 DROP NOT FOR RELPICATION", new ParserErrorInfo(46, "SQL46010", "RELPICATION"));
            ParserTestUtils.ErrorTest130("ALTER SECURITY POLICY [dbo].sec1 DROP BLOCK PREDICATE ON dbo.t1 AFTER INSER", new ParserErrorInfo(64, "SQL46010", "AFTER"));
            ParserTestUtils.ErrorTest130("ALTER SECURITY POLICY [dbo].sec1 DROP BLOCK PREDICATE ON dbo.t1 ATER INSERT", new ParserErrorInfo(64, "SQL46005", "AFTER", "ATER"));
            ParserTestUtils.ErrorTest130("ALTER SECURITY POLICY [dbo].sec1 DROP BLOCK PREDICATE ON dbo.t1 AFTER UPDAT", new ParserErrorInfo(64, "SQL46010", "AFTER"));
            ParserTestUtils.ErrorTest130("ALTER SECURITY POLICY [dbo].sec1 DROP BLOCK PREDICATE ON dbo.t1 ATER UPDATE", new ParserErrorInfo(64, "SQL46010", "ATER"));
            ParserTestUtils.ErrorTest130("ALTER SECURITY POLICY [dbo].sec1 DROP BLOCK PREDICATE ON dbo.t1 BEFORE UPDAT", new ParserErrorInfo(64, "SQL46010", "BEFORE"));
            ParserTestUtils.ErrorTest130("ALTER SECURITY POLICY [dbo].sec1 DROP BLOCK PREDICATE ON dbo.t1 BEFOR UPDATE", new ParserErrorInfo(64, "SQL46010", "BEFOR"));
            ParserTestUtils.ErrorTest130("ALTER SECURITY POLICY [dbo].sec1 DROP BLOCK PREDICATE ON dbo.t1 BEFORE DELET", new ParserErrorInfo(64, "SQL46010", "BEFORE"));
            ParserTestUtils.ErrorTest130("ALTER SECURITY POLICY [dbo].sec1 DROP BLOCK PREDICATE ON dbo.t1 BEFOR DELETE", new ParserErrorInfo(64, "SQL46005", "BEFORE", "BEFOR"));

            // Alters with granular operations on filter predicates.
            //
            ParserTestUtils.ErrorTest130("ALTER SECURITY POLICY [dbo].sec1 ADD FILTER PREDICATE [dbo].f1(c1) ON dbo.t1 AFTER UPDATE NOT FOR REPLICATION", new ParserErrorInfo(77, "SQL46010", "AFTER"));
            ParserTestUtils.ErrorTest130("ALTER SECURITY POLICY [dbo].sec1 ADD FILTER PREDICATE [dbo].f1(c1) ON dbo.t1 BEFORE UPDATE", new ParserErrorInfo(77, "SQL46010", "BEFORE"));
            ParserTestUtils.ErrorTest130("ALTER SECURITY POLICY [dbo].sec1 ADD FILTER PREDICATE [dbo].f1(c1) ON dbo.t1 AFTER INSERT NOT FOR REPLICATION", new ParserErrorInfo(77, "SQL46010", "AFTER"));
            ParserTestUtils.ErrorTest130("ALTER SECURITY POLICY [dbo].sec1 ADD FILTER PREDICATE [dbo].f1(c1) ON dbo.t1 BEFORE DELETE", new ParserErrorInfo(77, "SQL46010", "BEFORE"));
            ParserTestUtils.ErrorTest130("ALTER SECURITY POLICY [dbo].sec1 ALTER FILTER PREDICATE [dbo].f1(c1) ON dbo.t1 AFTER UPDATE NOT FOR REPLICATION", new ParserErrorInfo(79, "SQL46010", "AFTER"));
            ParserTestUtils.ErrorTest130("ALTER SECURITY POLICY [dbo].sec1 ALTER FILTER PREDICATE [dbo].f1(c1) ON dbo.t1 BEFORE UPDATE", new ParserErrorInfo(79, "SQL46010", "BEFORE"));
            ParserTestUtils.ErrorTest130("ALTER SECURITY POLICY [dbo].sec1 ALTER FILTER PREDICATE [dbo].f1(c1) ON dbo.t1 AFTER INSERT NOT FOR REPLICATION", new ParserErrorInfo(79, "SQL46010", "AFTER"));
            ParserTestUtils.ErrorTest130("ALTER SECURITY POLICY [dbo].sec1 ALTER FILTER PREDICATE [dbo].f1(c1) ON dbo.t1 BEFORE DELETE", new ParserErrorInfo(79, "SQL46010", "BEFORE"));
            ParserTestUtils.ErrorTest130("ALTER SECURITY POLICY [dbo].sec1 DROP FILTER PREDICATE ON dbo.t1 AFTER UPDATE NOT FOR REPLICATION", new ParserErrorInfo(65, "SQL46010", "AFTER"));
            ParserTestUtils.ErrorTest130("ALTER SECURITY POLICY [dbo].sec1 DROP FILTER PREDICATE ON dbo.t1 BEFORE UPDATE", new ParserErrorInfo(65, "SQL46010", "BEFORE"));
            ParserTestUtils.ErrorTest130("ALTER SECURITY POLICY [dbo].sec1 DROP FILTER PREDICATE ON dbo.t1 AFTER INSERT NOT FOR REPLICATION", new ParserErrorInfo(65, "SQL46010", "AFTER"));
            ParserTestUtils.ErrorTest130("ALTER SECURITY POLICY [dbo].sec1 DROP FILTER PREDICATE ON dbo.t1 BEFORE DELETE", new ParserErrorInfo(65, "SQL46010", "BEFORE"));

            // Missing comma in multi operation add or alter
            //
            ParserTestUtils.ErrorTest130("CREATE SECURITY POLICY [dbo].sec1 ADD FILTER PREDICATE [dbo].f1(c1) ON dbo.t1 ADD FILTER PREDICATE f2(c2) on dbo.t2", new ParserErrorInfo(82, "SQL46005", "COUNTER", "FILTER"));
            ParserTestUtils.ErrorTest130("ALTER SECURITY POLICY [dbo].sec1 ADD FILTER PREDICATE [dbo].f1(c1) ON dbo.t1 ADD BLOCK PREDICATE f2(c2) on dbo.t2", new ParserErrorInfo(81, "SQL46005", "COUNTER", "BLOCK"));
            ParserTestUtils.ErrorTest130("ALTER SECURITY POLICY [dbo].sec1 DROP FILTER PREDICATE ON dbo.t1 DROP FILTER PREDICATE on dbo.t2", new ParserErrorInfo(70, "SQL46005", "SERVER", "FILTER"));
            ParserTestUtils.ErrorTest130("ALTER SECURITY POLICY [dbo].sec1 ALTER FILTER PREDICATE [dbo].f1(c1) ON dbo.t1 DROP BLOCK PREDICATE on dbo.t2 AFTER UPDATE", new ParserErrorInfo(84, "SQL46005", "SERVER", "BLOCK"));

            // Duplicate and invalid options in the ALTER statement
            //
            ParserTestUtils.ErrorTest130("ALTER SECURITY POLICY [dbo].sec1 WITH (STATE = ON, STATE = ON)", new ParserErrorInfo(51, "SQL46049", "STATE"));
            ParserTestUtils.ErrorTest130("ALTER SECURITY POLICY [dbo].sec1 WITH (STATE = ON, STATE = OFF)", new ParserErrorInfo(51, "SQL46049", "STATE"));
            ParserTestUtils.ErrorTest130("ALTER SECURITY POLICY [dbo].sec1 WITH (SCHEMABINDING = ON)", new ParserErrorInfo(39, "SQL46010", "SCHEMABINDING"));
            ParserTestUtils.ErrorTest130("ALTER SECURITY POLICY [dbo].sec1 WITH (STATE = ON, SCHEMABINDING = OFF)", new ParserErrorInfo(51, "SQL46010", "SCHEMABINDING"));
        }

        /// <summary>
        /// Negative tests for a table with REMOTE_DATA_ARCHIVE
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void RemoteDataArchiveTableNegativeTest()
        {
            // Create table with RDA
            //
            ParserTestUtils.ErrorTest130("create table t_stretch(c int) with REMOTE_DATA_ARCHIVE=PAUSED", new ParserErrorInfo(35, "SQL46010", "REMOTE_DATA_ARCHIVE"));
            ParserTestUtils.ErrorTest130("create table t_stretch(c int) with REMOTE_DATA_ARCHIVE PAUSED", new ParserErrorInfo(35, "SQL46010", "REMOTE_DATA_ARCHIVE"));
            ParserTestUtils.ErrorTest130("create table t_stretch(c int) SET (REMOTE_DATA_ARCHIVE PAUSED)", new ParserErrorInfo(34, "SQL46010", "("));
            ParserTestUtils.ErrorTest130("create table t_stretch(c int) SET (REMOTE_DATA_ARCHIVE=PAUSED)", new ParserErrorInfo(34, "SQL46010", "("));
            ParserTestUtils.ErrorTest130("create table t_stretch(c int) WITH (REMOTE_DATA_ARCHIVE=NONEXISTENTOPTION)", new ParserErrorInfo(56, "SQL46010", "NONEXISTENTOPTION"));
            ParserTestUtils.ErrorTest130("create table t_stretch(c int) WITH (REMOTE_DATA_ARCHIVE=on (MIGRATION_STATE=INVALID))", new ParserErrorInfo(76, "SQL46010", "INVALID"));
            ParserTestUtils.ErrorTest130("create table t_stretch(c int) WITH (REMOTE_DATA_ARCHIVE=on (MIGRATION_STATE=OUTBOUND, FILTER_PREDICATE = INVALID))", new ParserErrorInfo(86, "SQL46005", "MIGRATION_STATE", "FILTER_PREDICATE"));

            // Alter table with RDA, filter predicate and migration state: invalid/ mis-spelled/ missing/ old migration states and filter predicates
            //
            ParserTestUtils.ErrorTest130("alter table t_stretch SET (REMOTE_DATA_ARCHIVE = ON (MIGRATION_STATE = OUTBOUND, FILTER PREDICATE = dbo.f1(c1)))", new ParserErrorInfo(88, "SQL46010", "PREDICATE"));
            ParserTestUtils.ErrorTest130("alter table t_stretch SET (REMOTE_DATA_ARCHIVE = ON (MIGRATION_STATE ON, FILTER_PREDICATE = database.dbo.f1(c1)))", new ParserErrorInfo(69, "SQL46010", "ON"));
            ParserTestUtils.ErrorTest130("alter table t_stretch SET (REMOTE_DATA_ARCHIVE = ON (MIGRATION_STATE = OUTBOUND, FILTER_PREDICATE dbo.f1(c1)))", new ParserErrorInfo(98, "SQL46010", "dbo"));
            ParserTestUtils.ErrorTest130("alter table t_stretch SET (REMOTE_DATA_ARCHIVE = ON (MIIGRATION_STATE = OUTBOUND, FILTER_PREDICATE = dbo.f1(c1)))", new ParserErrorInfo(53, "SQL46005", "MIGRATION_STATE", "MIIGRATION_STATE"));
            ParserTestUtils.ErrorTest130("alter table t_stretch SET (REMOTE_DATA_ARCHIVE = ON (MIGRATION_STATE = OUTBOUND, FILTER_PREDICATE = NUL))", new ParserErrorInfo(81, "SQL46005", "MIGRATION_STATE", "FILTER_PREDICATE"));
            ParserTestUtils.ErrorTest130("alter table t_stretch SET (REMOTE_DATA_ARCHIVE = ON (MIGRATION_STATE = OUTBOUND, FIILTER_PREDICATE = dbo.f1(c1)))", new ParserErrorInfo(81, "SQL46005", "FILTER_PREDICATE", "FIILTER_PREDICATE"));
            ParserTestUtils.ErrorTest130("alter table t_stretch SET (REMOTE_DATA_ARCHIVE = ON (MIGRATIONSTATE = OUTBOUND, FILTER_PREDICATE = dbo.f1(c1)))", new ParserErrorInfo(53, "SQL46005", "MIGRATION_STATE", "MIGRATIONSTATE"));
            ParserTestUtils.ErrorTest130("alter table t_stretch SET (REMOTE_DATA_ARCHIVE = ON (MIGRATION_STATE = OUTBOUND, FILTERPREDICATE = dbo.f1(c1)))", new ParserErrorInfo(81, "SQL46005", "FILTER_PREDICATE", "FILTERPREDICATE"));
            ParserTestUtils.ErrorTest130("alter table t_stretch SET (REMOTE_DATA_ARCHIVE = ON (MIGRATION_SSTATE = OUTBOUND, FILTERR_PREDICATE = dbo.f1(c1)))", new ParserErrorInfo(53, "SQL46005", "MIGRATION_STATE", "MIGRATION_SSTATE"));

            ParserTestUtils.ErrorTest130("ALTER TABLE T1 SET (REMOTE_DATA_ARCHIVE = ON (MIGRATION_STATE = MigrationOutbound))", new ParserErrorInfo(64, "SQL46010", "MigrationOutbound"));
            ParserTestUtils.ErrorTest130("ALTER TABLE T1 SET (REMOTE_DATA_ARCHIVE = ON (MIGRATION_STATE = MigrationState))", new ParserErrorInfo(64, "SQL46010", "MigrationState"));
            ParserTestUtils.ErrorTest130("ALTER TABLE T1 SET (REMOTE_DATA_ARCHIVE = ON (MIGRATION_STATE = OTBOUND))", new ParserErrorInfo(64, "SQL46010", "OTBOUND"));
            ParserTestUtils.ErrorTest130("ALTER TABLE T1 SET (REMOTE_DATA_ARCHIVE = ON (MIGRATION_STATE = OUT]BOUND))", new ParserErrorInfo(67, "SQL46010", "]"));
            ParserTestUtils.ErrorTest130("ALTER TABLE T1 SET (REMOTE_DATA_ARCHIVE = ON (MIGRATION_STATE = ))", new ParserErrorInfo(64, "SQL46010", ")"));
            ParserTestUtils.ErrorTest130("ALTER TABLE T1 SET (REMOTE_DATA_ARCHIVE = ON (MIGRATION_STATE = OFF))", new ParserErrorInfo(64, "SQL46010", "OFF"));
            ParserTestUtils.ErrorTest130("ALTER TABLE T1 SET (REMOTE_DATA_ARCHIVE = ON (MIGRATION_STATE = ON))", new ParserErrorInfo(64, "SQL46010", "ON"));
            ParserTestUtils.ErrorTest130("ALTER TABLE T1 SET (REMOTE_DATA_ARCHIVE = ON (MIGRATION_STATE = DISABLE))", new ParserErrorInfo(64, "SQL46010", "DISABLE"));

            ParserTestUtils.ErrorTest130("alter table t_stretch SET (REMOTE_DATA_ARCHIVE = OFF (MIGRATION_STATE = OUTBOUND, FILTER PREDICATE = dbo.f1(c1)))", new ParserErrorInfo(89, "SQL46010", "PREDICATE"));
            ParserTestUtils.ErrorTest130("alter table t_stretch SET (REMOTE_DATA_ARCHIVE = OFF (MIGRATION_STATE ON, FILTER_PREDICATE = database.dbo.f1(c1)))", new ParserErrorInfo(70, "SQL46010", "ON"));
            ParserTestUtils.ErrorTest130("alter table t_stretch SET (REMOTE_DATA_ARCHIVE = OFF (MIGRATION_STATE = OUTBOUND, FILTER_PREDICATE dbo.f1(c1)))", new ParserErrorInfo(99, "SQL46010", "dbo"));
            ParserTestUtils.ErrorTest130("alter table t_stretch SET (REMOTE_DATA_ARCHIVE = OFF (MIIGRATION_STATE = OUTBOUND, FILTER_PREDICATE = dbo.f1(c1)))", new ParserErrorInfo(54, "SQL46005", "MIGRATION_STATE", "MIIGRATION_STATE"));
            ParserTestUtils.ErrorTest130("alter table t_stretch SET (REMOTE_DATA_ARCHIVE = OFF (MIGRATION_STATE = OUTBOUND, FILTER_PREDICATE = NUL))", new ParserErrorInfo(82, "SQL46005", "MIGRATION_STATE", "FILTER_PREDICATE"));
            ParserTestUtils.ErrorTest130("alter table t_stretch SET (REMOTE_DATA_ARCHIVE = OFF (MIGRATION_STATE = OUTBOUND, FIILTER_PREDICATE = dbo.f1(c1)))", new ParserErrorInfo(82, "SQL46005", "FILTER_PREDICATE", "FIILTER_PREDICATE"));
            ParserTestUtils.ErrorTest130("alter table t_stretch SET (REMOTE_DATA_ARCHIVE = OFF (MIGRATIONSTATE = OUTBOUND, FILTER_PREDICATE = dbo.f1(c1)))", new ParserErrorInfo(54, "SQL46005", "MIGRATION_STATE", "MIGRATIONSTATE"));
            ParserTestUtils.ErrorTest130("alter table t_stretch SET (REMOTE_DATA_ARCHIVE = OFF (MIGRATION_STATE = OUTBOUND, FILTERPREDICATE = dbo.f1(c1)))", new ParserErrorInfo(82, "SQL46005", "FILTER_PREDICATE", "FILTERPREDICATE"));
            ParserTestUtils.ErrorTest130("alter table t_stretch SET (REMOTE_DATA_ARCHIVE = OFF (MIGRATION_SSTATE = OUTBOUND, FILTERR_PREDICATE = dbo.f1(c1)))", new ParserErrorInfo(54, "SQL46005", "MIGRATION_STATE", "MIGRATION_SSTATE"));

            ParserTestUtils.ErrorTest130("ALTER TABLE T1 SET (REMOTE_DATA_ARCHIVE = OFF (MIGRATION_STATE = MigrationOutbound))", new ParserErrorInfo(65, "SQL46010", "MigrationOutbound"));
            ParserTestUtils.ErrorTest130("ALTER TABLE T1 SET (REMOTE_DATA_ARCHIVE = OFF (MIGRATION_STATE = MigrationState))", new ParserErrorInfo(65, "SQL46010", "MigrationState"));
            ParserTestUtils.ErrorTest130("ALTER TABLE T1 SET (REMOTE_DATA_ARCHIVE = OFF (MIGRATION_STATE = OTBOUND))", new ParserErrorInfo(65, "SQL46010", "OTBOUND"));
            ParserTestUtils.ErrorTest130("ALTER TABLE T1 SET (REMOTE_DATA_ARCHIVE = OFF (MIGRATION_STATE = OUT]BOUND))", new ParserErrorInfo(68, "SQL46010", "]"));
            ParserTestUtils.ErrorTest130("ALTER TABLE T1 SET (REMOTE_DATA_ARCHIVE = OFF (MIGRATION_STATE = ))", new ParserErrorInfo(65, "SQL46010", ")"));
            ParserTestUtils.ErrorTest130("ALTER TABLE T1 SET (REMOTE_DATA_ARCHIVE = OFF (MIGRATION_STATE = OFF))", new ParserErrorInfo(65, "SQL46010", "OFF"));
            ParserTestUtils.ErrorTest130("ALTER TABLE T1 SET (REMOTE_DATA_ARCHIVE = OFF (MIGRATION_STATE = ON))", new ParserErrorInfo(65, "SQL46010", "ON"));
            ParserTestUtils.ErrorTest130("ALTER TABLE T1 SET (REMOTE_DATA_ARCHIVE = OFF (MIGRATION_STATE = DISABLE))", new ParserErrorInfo(65, "SQL46010", "DISABLE"));

            ParserTestUtils.ErrorTest130("alter table t_stretch SET (REMOTE_DATA_ARCHIVE = OFF_WITHOUT_DATA_RECOVERY (MIGRATION_STATE = OUTBOUND, FILTER PREDICATE = dbo.f1(c1)))", new ParserErrorInfo(111, "SQL46010", "PREDICATE"));
            ParserTestUtils.ErrorTest130("alter table t_stretch SET (REMOTE_DATA_ARCHIVE = OFF_WITHOUT_DATA_RECOVERY (MIGRATION_STATE ON, FILTER_PREDICATE = database.dbo.f1(c1)))", new ParserErrorInfo(92, "SQL46010", "ON"));
            ParserTestUtils.ErrorTest130("alter table t_stretch SET (REMOTE_DATA_ARCHIVE = OFF_WITHOUT_DATA_RECOVERY (MIGRATION_STATE = OUTBOUND, FILTER_PREDICATE dbo.f1(c1)))", new ParserErrorInfo(121, "SQL46010", "dbo"));
            ParserTestUtils.ErrorTest130("alter table t_stretch SET (REMOTE_DATA_ARCHIVE = OFF_WITHOUT_DATA_RECOVERY (MIIGRATION_STATE = OUTBOUND, FILTER_PREDICATE = dbo.f1(c1)))", new ParserErrorInfo(76, "SQL46005", "MIGRATION_STATE", "MIIGRATION_STATE"));
            ParserTestUtils.ErrorTest130("alter table t_stretch SET (REMOTE_DATA_ARCHIVE = OFF_WITHOUT_DATA_RECOVERY (MIGRATION_STATE = OUTBOUND, FILTER_PREDICATE = NUL))", new ParserErrorInfo(104, "SQL46005", "MIGRATION_STATE", "FILTER_PREDICATE"));
            ParserTestUtils.ErrorTest130("alter table t_stretch SET (REMOTE_DATA_ARCHIVE = OFF_WITHOUT_DATA_RECOVERY (MIGRATION_STATE = OUTBOUND, FIILTER_PREDICATE = dbo.f1(c1)))", new ParserErrorInfo(104, "SQL46005", "FILTER_PREDICATE", "FIILTER_PREDICATE"));
            ParserTestUtils.ErrorTest130("alter table t_stretch SET (REMOTE_DATA_ARCHIVE = OFF_WITHOUT_DATA_RECOVERY (MIGRATIONSTATE = OUTBOUND, FILTER_PREDICATE = dbo.f1(c1)))", new ParserErrorInfo(76, "SQL46005", "MIGRATION_STATE", "MIGRATIONSTATE"));
            ParserTestUtils.ErrorTest130("alter table t_stretch SET (REMOTE_DATA_ARCHIVE = OFF_WITHOUT_DATA_RECOVERY (MIGRATION_STATE = OUTBOUND, FILTERPREDICATE = dbo.f1(c1)))", new ParserErrorInfo(104, "SQL46005", "FILTER_PREDICATE", "FILTERPREDICATE"));
            ParserTestUtils.ErrorTest130("alter table t_stretch SET (REMOTE_DATA_ARCHIVE = OFF_WITHOUT_DATA_RECOVERY (MIGRATION_SSTATE = OUTBOUND, FILTERR_PREDICATE = dbo.f1(c1)))", new ParserErrorInfo(76, "SQL46005", "MIGRATION_STATE", "MIGRATION_SSTATE"));

            ParserTestUtils.ErrorTest130("ALTER TABLE T1 SET (REMOTE_DATA_ARCHIVE = OFF_WITHOUT_DATA_RECOVERY (MIGRATION_STATE = MigrationOutbound))", new ParserErrorInfo(87, "SQL46010", "MigrationOutbound"));
            ParserTestUtils.ErrorTest130("ALTER TABLE T1 SET (REMOTE_DATA_ARCHIVE = OFF_WITHOUT_DATA_RECOVERY (MIGRATION_STATE = MigrationState))", new ParserErrorInfo(87, "SQL46010", "MigrationState"));
            ParserTestUtils.ErrorTest130("ALTER TABLE T1 SET (REMOTE_DATA_ARCHIVE = OFF_WITHOUT_DATA_RECOVERY (MIGRATION_STATE = OTBOUND))", new ParserErrorInfo(87, "SQL46010", "OTBOUND"));
            ParserTestUtils.ErrorTest130("ALTER TABLE T1 SET (REMOTE_DATA_ARCHIVE = OFF_WITHOUT_DATA_RECOVERY (MIGRATION_STATE = OUT]BOUND))", new ParserErrorInfo(90, "SQL46010", "]"));
            ParserTestUtils.ErrorTest130("ALTER TABLE T1 SET (REMOTE_DATA_ARCHIVE = OFF_WITHOUT_DATA_RECOVERY (MIGRATION_STATE = ))", new ParserErrorInfo(87, "SQL46010", ")"));
            ParserTestUtils.ErrorTest130("ALTER TABLE T1 SET (REMOTE_DATA_ARCHIVE = OFF_WITHOUT_DATA_RECOVERY (MIGRATION_STATE = OFF))", new ParserErrorInfo(87, "SQL46010", "OFF"));
            ParserTestUtils.ErrorTest130("ALTER TABLE T1 SET (REMOTE_DATA_ARCHIVE = OFF_WITHOUT_DATA_RECOVERY (MIGRATION_STATE = ON))", new ParserErrorInfo(87, "SQL46010", "ON"));
            ParserTestUtils.ErrorTest130("ALTER TABLE T1 SET (REMOTE_DATA_ARCHIVE = OFF_WITHOUT_DATA_RECOVERY (MIGRATION_STATE = DISABLE))", new ParserErrorInfo(87, "SQL46010", "DISABLE"));

            // Alter table with invalid/ mis-spelled RDA
            //
            ParserTestUtils.ErrorTest130("ALTER TABLE T1 SET REMOTE_DATA_ARCHIVE WITH (MIGRATION_STATE = INBOUND)", new ParserErrorInfo(19, "SQL46010", "REMOTE_DATA_ARCHIVE"));
            ParserTestUtils.ErrorTest130("ALTER TABLE T1 SET REMTE_DATA_ARCHIVE WITH (MIGRATION_STATE = INBOUND)", new ParserErrorInfo(19, "SQL46010", "REMTE_DATA_ARCHIVE"));

            // Alter Table Specify Database on Function
            ParserTestUtils.ErrorTest130("alter table t_stretch SET (REMOTE_DATA_ARCHIVE = ON (MIGRATION_STATE = OUTBOUND, FILTER_PREDICATE = database.dbo.f1(c1)))", new ParserErrorInfo(100, "SQL46010", "database"));

            // Duplicate Parameters
            //
            ParserTestUtils.ErrorTest130("alter table t_stretch SET (REMOTE_DATA_ARCHIVE = ON (MIGRATION_STATE = OUTBOUND, FILTER_PREDICATE = dbo.f1(c1), MIGRATION_STATE = OUTBOUND))", new ParserErrorInfo(112, "SQL46010", "MIGRATION_STATE"));
            ParserTestUtils.ErrorTest130("alter table t_stretch SET (REMOTE_DATA_ARCHIVE = ON (MIGRATION_STATE = OUTBOUND, MIGRATION_STATE = OUTBOUND, FILTER_PREDICATE = dbo.f1(c1)))", new ParserErrorInfo(81, "SQL46010", "MIGRATION_STATE"));
            ParserTestUtils.ErrorTest130("alter table t_stretch SET (REMOTE_DATA_ARCHIVE = ON (MIGRATION_STATE = PAUSED, FILTER_PREDICATE = dbo.f1(c1), MIGRATION_STATE = OUTBOUND))", new ParserErrorInfo(110, "SQL46010", "MIGRATION_STATE"));
            ParserTestUtils.ErrorTest130("alter table t_stretch SET (REMOTE_DATA_ARCHIVE = ON (MIGRATION_STATE = PAUSED, MIGRATION_STATE = OUTBOUND, FILTER_PREDICATE = dbo.f1(c1)))", new ParserErrorInfo(79, "SQL46010", "MIGRATION_STATE"));
            ParserTestUtils.ErrorTest130("alter table t_stretch SET (REMOTE_DATA_ARCHIVE = ON (FILTER_PREDICATE = dbo.f1(c1), MIGRATION_STATE = OUTBOUND, FILTER_PREDICATE = dbo.f1(c1)))", new ParserErrorInfo(112, "SQL46010", "FILTER_PREDICATE"));
            ParserTestUtils.ErrorTest130("alter table t_stretch SET (REMOTE_DATA_ARCHIVE = ON (FILTER_PREDICATE = NULL, MIGRATION_STATE = OUTBOUND, FILTER_PREDICATE = dbo.f1(c1)))", new ParserErrorInfo(106, "SQL46010", "FILTER_PREDICATE"));
            ParserTestUtils.ErrorTest130("alter table t_stretch SET (REMOTE_DATA_ARCHIVE = ON (FILTER_PREDICATE = dbo.f1(c1), FILTER_PREDICATE = dbo.f1(c1), MIGRATION_STATE = OUTBOUND))", new ParserErrorInfo(84, "SQL46010", "FILTER_PREDICATE"));
            ParserTestUtils.ErrorTest130("alter table t_stretch SET (REMOTE_DATA_ARCHIVE = ON (FILTER_PREDICATE = NULL, FILTER_PREDICATE = dbo.f1(c1), MIGRATION_STATE = OUTBOUND))", new ParserErrorInfo(78, "SQL46010", "FILTER_PREDICATE"));

            // Alter table remote_data_archive with mis-spelled option
            //
            ParserTestUtils.ErrorTest130("alter table t_stretch SET (REMOTE_DATA_ARCHIVE = OF (MIGRATION_STATE = PAUSED)", new ParserErrorInfo(49, "SQL46010", "OF"));
            ParserTestUtils.ErrorTest130("alter table t_stretch SET (REMOTE_DATA_ARCHIVE = OFF_WITHOUT_DATA_RECOVER (MIGRATION_STATE = PAUSED)", new ParserErrorInfo(49, "SQL46005", "OFF_WITHOUT_DATA_RECOVERY", "OFF_WITHOUT_DATA_RECOVER"));
        }

        /// <summary>
        /// Negative tests for a database with REMOTE_DATA_ARCHIVE
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void RemoteDataArchiveDatabaseNegativeTest()
        {
            ParserTestUtils.ErrorTest130("ALTER database db_test WITH SET REMOTE_DATA_ARCHIVE= OFF (SERVER = N'Test')", new ParserErrorInfo(15, "SQL46010", "db_test"));
            ParserTestUtils.ErrorTest130("ALTER database db_test SET REMOTE_DATA_ARCHIVE=NONEXISTENT", new ParserErrorInfo(46, "SQL46010", "="));

            // Server and Credential
            //
            ParserTestUtils.ErrorTest130("ALTER database db_test SET REMOTE_DATA_ARCHIVE = ON (SERVER = N'Badly Quoted's', CREDENTIAL = [Test])",
                new ParserErrorInfo(78, "SQL46030", "', CREDENTIAL = [Test])"));
            ParserTestUtils.ErrorTest130("ALTER database db_test SET REMOTE_DATA_ARCHIVE = ON (CREDENTIAL = [Badly Bracketed] Value], SERVER = N'Test')",
                new ParserErrorInfo(89, "SQL46010", "]"));
            ParserTestUtils.ErrorTest130("ALTER database db_test SET REMOTE_DATA_ARCHIVE = ON (SERVER = LiteralString, CREDENTIAL = 'Test')",
                new ParserErrorInfo(62, "SQL46010", "LiteralString"));
            ParserTestUtils.ErrorTest130("ALTER database db_test SET REMOTE_DATA_ARCHIVE = ON (SERVER = 'Test', CREDENTIAL = 'Test')",
                new ParserErrorInfo(83, "SQL46010", "'Test'"));
            ParserTestUtils.ErrorTest130("ALTER database db_test SET REMOTE_DATA_ARCHIVE = ON (SERVER_NAME = 'String', CREDENTIAL = [Test])",
                new ParserErrorInfo(53, "SQL46010", "SERVER_NAME"));
            ParserTestUtils.ErrorTest130("ALTER database db_test SET REMOTE_DATA_ARCHIVE = ON (FEDERATED_SERVICE_ACCOUNT = ONN, SERVER = 'String')",
                new ParserErrorInfo(81, "SQL46010", "ONN"));
            ParserTestUtils.ErrorTest130("ALTER database db_test SET REMOTE_DATA_ARCHIVE = ON (CREDENTIAL = [Test], CREDENTIAL = [Test])",
                new ParserErrorInfo(74, "SQL46049", "CREDENTIAL"));
            ParserTestUtils.ErrorTest130("ALTER database db_test SET REMOTE_DATA_ARCHIVE = ON (FEDERATED_SERVICE_ACCOUNT = ON, FEDERATED_SERVICE_ACCOUNT = ON)",
                new ParserErrorInfo(85, "SQL46049", "FEDERATED_SERVICE_ACCOUNT"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void CreateColumnStoreIndexNegativeTest()
        {
            // cannot create clustered index with filter predicate
            //
            ParserTestUtils.ErrorTest130("create clustered columnstore index cci on t where a > 0", new ParserErrorInfo(44, "SQL46010", "where"));
            // wrong filter clause syntax
            //
            ParserTestUtils.ErrorTest130("create nonclustered columnstore index cci on t(a) (where a > 0)", new ParserErrorInfo(50, "SQL46010", "("));
            ParserTestUtils.ErrorTest130("create nonclustered columnstore index cci on t(a) where a > 0)", new ParserErrorInfo(61, "SQL46010", ")"));
            ParserTestUtils.ErrorTest130("create nonclustered columnstore index cci on t(a) (where a > 0", new ParserErrorInfo(50, "SQL46010", "("));
            ParserTestUtils.ErrorTest130("create nonclustered columnstore index cci on t(a) with (compression_delay = 27) where a > 0", new ParserErrorInfo(80, "SQL46010", "where"));
            // cannot assign sort order to order hint option
            //
            ParserTestUtils.ErrorTest130("create clustered columnstore index cci on t with (order (a asc))", new ParserErrorInfo(59, "SQL46010", "asc"));
            ParserTestUtils.ErrorTest130("create clustered columnstore index cci on t with (order (a desc))", new ParserErrorInfo(59, "SQL46010", "desc"));
            ParserTestUtils.ErrorTest130("create clustered columnstore index cci on t with (order (a, b asc))", new ParserErrorInfo(62, "SQL46010", "asc"));
            // wrong order hint option column list syntax
            //
            ParserTestUtils.ErrorTest130("create clustered columnstore index cci on t with (order a)", new ParserErrorInfo(56, "SQL46010", "a"));
            ParserTestUtils.ErrorTest130("create clustered columnstore index cci on t with (order a, b)", new ParserErrorInfo(56, "SQL46010", "a"));
            ParserTestUtils.ErrorTest130("create clustered columnstore index cci on t with (order a b)", new ParserErrorInfo(56, "SQL46010", "a"));
            // wrong sort_in_tempdb option syntax
            //
            ParserTestUtils.ErrorTest130("create clustered columnstore index cci on t with (sort_in_tempdb)", new ParserErrorInfo(50, "SQL46010", "sort_in_tempdb"));
            // order option is only valid for clustered columnstore index now
            //
            ParserTestUtils.ErrorTest130("create clustered index idx on t(a) with (order(a,b))", new ParserErrorInfo(41, "SQL46057", "order", "CREATE INDEX"));
            ParserTestUtils.ErrorTest130("create nonclustered index idx on t(a) with (order(a,b))", new ParserErrorInfo(44, "SQL46057", "order", "CREATE INDEX"));
            ParserTestUtils.ErrorTest130("create nonclustered columnstore index idx on t(a) with (order(a,b))", new ParserErrorInfo(56, "SQL46010", "order"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void CreateExternalDataSourceNegativeTest()
        {
            // Create external data source keyword typos
            //
            ParserTestUtils.ErrorTest130("CREAT EXTERNAL DATA SOURCE eds1 WITH (TYPE = HADOOP, LOCATION = 'protocol://ip_address:port')", new ParserErrorInfo(0, "SQL46010", "CREAT"));
            ParserTestUtils.ErrorTest130("CREATE EXTRNAL DATA SOURCE eds1 WITH (TYPE = HADOOP, LOCATION = 'protocol://ip_address:port')", new ParserErrorInfo(7, "SQL46010", "EXTRNAL"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNALD ATA SOURCE eds1 WITH (TYPE = HADOOP, LOCATION = 'protocol://ip_address:port')", new ParserErrorInfo(7, "SQL46010", "EXTERNALD"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL DTA SOURCE eds1 WITH (TYPE = HADOOP, LOCATION = 'protocol://ip_address:port')", new ParserErrorInfo(16, "SQL46010", "DTA"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL DATA SROUCE eds1 WITH (TYPE = HADOOP, LOCATION = 'protocol://ip_address:port')", new ParserErrorInfo(21, "SQL46005", "SOURCE", "SROUCE"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL DATAS ROUCE eds1 WITH (TYPE = HADOOP, LOCATION = 'protocol://ip_address:port')", new ParserErrorInfo(16, "SQL46010", "DATAS"));

            ParserTestUtils.ErrorTest130("CREATE EXTERNAL DATA SOURCE eds1 WIT (TYPE = HADOOP, LOCATION = 'protocol://ip_address:port')", new ParserErrorInfo(33, "SQL46010", "WIT"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL DATA SOURCE eds1 WITH TYPE = HADOOP, LOCATION = 'protocol://ip_address:port')", new ParserErrorInfo(38, "SQL46010", "TYPE"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL DATA SOURCE eds1 WITH T(YPE = HADOOP, LOCATION = 'protocol://ip_address:port')", new ParserErrorInfo(38, "SQL46010", "T"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL DATA SOURCE eds1 WITH (YPE = HADOOP, LOCATION = 'protocol://ip_address:port')", new ParserErrorInfo(39, "SQL46010", "YPE"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL DATA SOURCE eds1 WITH (TYPE HADOOP, LOCATION = 'protocol://ip_address:port')", new ParserErrorInfo(39, "SQL46010", "TYPE"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL DATA SOURCE eds1 WITH (TYPE = HADOOP LOCATION = 'protocol://ip_address:port')", new ParserErrorInfo(53, "SQL46010", "LOCATION"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL DATA SOURCE eds1 WITH (TYPE = HADOOP, LOCTION = 'protocol://ip_address:port')", new ParserErrorInfo(54, "SQL46010", "LOCTION"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL DATA SOURCE eds1 WITH (TYPE = HADOOP, LOCATION 'protocol://ip_address:port')", new ParserErrorInfo(54, "SQL46010", "LOCATION"));

            ParserTestUtils.ErrorTest130("CREATE EXTERNAL DATA SOURCE eds1 WITH (TYPE = HADOOP, LOCATION = 'protocol://ip_address:port' RESOURCE_MANAGER_LOCATION = 'ip_address:port')", new ParserErrorInfo(94, "SQL46010", "RESOURCE_MANAGER_LOCATION"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL DATA SOURCE eds1 WITH (TYPE = HADOOP, LOCATION = 'protocol://ip_address:port', RESURCE_MANAGER_LOCATION = 'ip_address:port')", new ParserErrorInfo(95, "SQL46010", "RESURCE_MANAGER_LOCATION"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL DATA SOURCE eds1 WITH (TYPE = HADOOP, LOCATION = 'protocol://ip_address:port', RESOURCE_MANAGER_LOCATION 'ip_address:port')", new ParserErrorInfo(95, "SQL46010", "RESOURCE_MANAGER_LOCATION"));

            ParserTestUtils.ErrorTest130("CREATE EXTERNAL DATA SOURCE eds1 WITH (TYPE = HADOOP, LOCATION = 'protocol://ip_address:port', RESOURCE_MANAGER_LOCATION = 'ip_address:port' CREDENTIAL = cred1)", new ParserErrorInfo(141, "SQL46010", "CREDENTIAL"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL DATA SOURCE eds1 WITH (TYPE = HADOOP, LOCATION = 'protocol://ip_address:port', RESOURCE_MANAGER_LOCATION = 'ip_address:port', CREDNTIAL = cred1)", new ParserErrorInfo(142, "SQL46010", "CREDNTIAL"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL DATA SOURCE eds1 WITH (TYPE = HADOOP, LOCATION = 'protocol://ip_address:port', RESOURCE_MANAGER_LOCATION = 'ip_address:port', CREDENTIAL cred1)", new ParserErrorInfo(142, "SQL46010", "CREDENTIAL"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL DATA SOURCE eds1 WITH (TYPE = HADOOP, LOCATION = 'protocol://ip_address:port', RESOURCE_MANAGER_LOCATION = 'ip_address:port', CREDENTIAL = cred1", new ParserErrorInfo(160, "SQL46029"));

            ParserTestUtils.ErrorTest130("CREATE EXTERNAL DATA SOURCE eds1 WITH (TYPE = SHARD_MAP_MANAGER, LOCATION = 'someServer' DATABASE_NAME = 'someDatabase')", new ParserErrorInfo(89, "SQL46010", "DATABASE_NAME"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL DATA SOURCE eds1 WITH (TYPE = SHARD_MAP_MANAGER, LOCATION = 'someServer', DATABASE_NAMES = 'someDatabase')", new ParserErrorInfo(90, "SQL46010", "DATABASE_NAMES"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL DATA SOURCE eds1 WITH (TYPE = SHARD_MAP_MANAGER, LOCATION = 'someServer', DATABSE_NAME = 'someDatabase')", new ParserErrorInfo(90, "SQL46010", "DATABSE_NAME"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL DATA SOURCE eds1 WITH (TYPE = SHARD_MAP_MANAGER, LOCATION = 'someServer', DATABASE_NAME 'someDatabase')", new ParserErrorInfo(90, "SQL46010", "DATABASE_NAME"));

            ParserTestUtils.ErrorTest130("CREATE EXTERNAL DATA SOURCE eds1 WITH (TYPE = SHARD_MAP_MANAGER, LOCATION = 'someServer', DATABASE_NAME = 'someDatabase' SHARD_MAP_NAME = 'somShardMap')", new ParserErrorInfo(121, "SQL46010", "SHARD_MAP_NAME"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL DATA SOURCE eds1 WITH (TYPE = SHARD_MAP_MANAGER, LOCATION = 'someServer', DATABASE_NAME = 'someDatabase', SHARDS_MAP_NAME = 'someShardMap')", new ParserErrorInfo(122, "SQL46010", "SHARDS_MAP_NAME"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL DATA SOURCE eds1 WITH (TYPE = SHARD_MAP_MANAGER, LOCATION = 'someServer', DATABASE_NAME = 'someDatabase', SHRD_MAP_NAME = 'someShardMap')", new ParserErrorInfo(122, "SQL46010", "SHRD_MAP_NAME"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL DATA SOURCE eds1 WITH (TYPE = SHARD_MAP_MANAGER, LOCATION = 'someServer', DATABASE_NAME = 'someDatabase', SHARD_MAP_NAME 'someShardMap')", new ParserErrorInfo(122, "SQL46010", "SHARD_MAP_NAME"));

            ParserTestUtils.ErrorTest130("CREATE EXTERNAL DATA SOURCE eds1 WITH (TYPE = RDBMS, LOCATION = 'someServer' DATABASE_NAME = 'someDatabase')", new ParserErrorInfo(77, "SQL46010", "DATABASE_NAME"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL DATA SOURCE eds1 WITH (TYPE = RDBMS, LOCATION = 'someServer', DATABASE_NAMES = 'someDatabase')", new ParserErrorInfo(78, "SQL46010", "DATABASE_NAMES"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL DATA SOURCE eds1 WITH (TYPE = RDBMS, LOCATION = 'someServer', DATABSE_NAME = 'someDatabase')", new ParserErrorInfo(78, "SQL46010", "DATABSE_NAME"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL DATA SOURCE eds1 WITH (TYPE = RDBMS, LOCATION = 'someServer', DATABASE_NAME 'someDatabase')", new ParserErrorInfo(78, "SQL46010", "DATABASE_NAME"));

            // Missing required properties
            //
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL DATA SOURCE", new ParserErrorInfo(27, "SQL46029"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL DATA SOURCE eds1", new ParserErrorInfo(32, "SQL46029"));

            // Create external data source with incorrect data source type value
            //
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL DATA SOURCE eds1 WITH (TYPE = HADOP, LOCATION = 'protocol://ip_address:port', RESOURCE_MANAGER_LOCATION = 'ip_address:port', CREDENTIAL = cred1)", new ParserErrorInfo(46, "SQL46010", "HADOP"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL DATA SOURCE eds1 WITH (TYPE = SHARD_MAP_MANGER, LOCATION = 'someServer', DATABASE_NAME = 'someDatabase', SHARD_MAP_NAME = 'someDatabase')", new ParserErrorInfo(46, "SQL46010", "SHARD_MAP_MANGER"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL DATA SOURCE eds1 WITH (TYPE = 'SHARD_MAP_MANAGER', LOCATION = 'someServer', DATABASE_NAME = 'someDatabase', SHARD_MAP_NAME = 'someShardMap', CREDENTIAL = someCred)", new ParserErrorInfo(46, "SQL46010", "'SHARD_MAP_MANAGER'"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL DATA SOURCE eds1 WITH (TYPE = RDMBS, LOCATION = 'someServer', DATABASE_NAME = 'someDatabase')", new ParserErrorInfo(46, "SQL46010", "RDMBS"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL DATA SOURCE eds1 WITH (TYPE = 'RDBMS', LOCATION = 'someServer', DATABASE_NAME = 'someDatabase', CREDENTIAL = someCred)", new ParserErrorInfo(46, "SQL46010", "'RDBMS'"));

            // Create external data source with incorrect data source location value
            //
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL DATA SOURCE eds1 WITH (TYPE = HADOOP, LOCATION = protocol://ip_address:port, RESOURCE_MANAGER_LOCATION = 'ip_address:port', CREDENTIAL = cred1)", new ParserErrorInfo(65, "SQL46010", "protocol:"));

            // Create external data source with incorrect resource manager location value
            //
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL DATA SOURCE eds1 WITH (TYPE = HADOOP, LOCATION = 'protocol://ip_address:port', RESOURCE_MANAGER_LOCATION = ip_address:port, CREDENTIAL = cred1)", new ParserErrorInfo(123, "SQL46010", "ip_address:"));

            // Create external smm data source with various incorrect values
            //
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL DATA SOURCE eds1 WITH (TYPE = SHARD_MAP_MANAGER, LOCATION = someServer, DATABASE_NAME = 'someDatabase', SHARD_MAP_NAME = 'someShardMap', CREDENTIAL = someCred)", new ParserErrorInfo(76, "SQL46010", "someServer"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL DATA SOURCE eds1 WITH (TYPE = SHARD_MAP_MANAGER, LOCATION = 'someServer', DATABASE_NAME = someDatabase, SHARD_MAP_NAME = 'someShardMap', CREDENTIAL = someCred)", new ParserErrorInfo(90, "SQL46010", "DATABASE_NAME"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL DATA SOURCE eds1 WITH (TYPE = SHARD_MAP_MANAGER, LOCATION = 'someServer', DATABASE_NAME = 'someDatabase', SHARD_MAP_NAME = someShardMap, CREDENTIAL = someCred)", new ParserErrorInfo(122, "SQL46010", "SHARD_MAP_NAME"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL DATA SOURCE eds1 WITH (TYPE = SHARD_MAP_MANAGER, LOCATION = 'someServer', DATABASE_NAME = 'someDatabase', SHARD_MAP_NAME = 'someShardMap', CREDENTIAL = 'someCred')", new ParserErrorInfo(155, "SQL46010", "CREDENTIAL"));

            // Create external rdbms data source with various incorrect values
            //
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL DATA SOURCE eds1 WITH (TYPE = RDBMS, LOCATION = someServer, DATABASE_NAME = 'someDatabase', CREDENTIAL = someCred)", new ParserErrorInfo(64, "SQL46010", "someServer"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL DATA SOURCE eds1 WITH (TYPE = RDBMS, LOCATION = 'someServer', DATABASE_NAME = someDatabase, CREDENTIAL = someCred)", new ParserErrorInfo(78, "SQL46010", "DATABASE_NAME"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL DATA SOURCE eds1 WITH (TYPE = RDBMS, LOCATION = 'someServer', DATABASE_NAME = 'someDatabase', CREDENTIAL = 'someCred')", new ParserErrorInfo(110, "SQL46010", "CREDENTIAL"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void AlterExternalDataSourceNegativeTest()
        {
            // Alter external data source keyword typos
            //
            ParserTestUtils.ErrorTest130("ALTR EXTERNAL DATA SOURCE eds1 SET LOCATION = 'someServerLocation'", new ParserErrorInfo(0, "SQL46010", "ALTR"));
            ParserTestUtils.ErrorTest130("ALTER EXTRNAL DATA SOURCE eds1 SET LOCATION = 'someServerLocation'", new ParserErrorInfo(6, "SQL46010", "EXTRNAL"));
            ParserTestUtils.ErrorTest130("ALTER EXTERNALD ATA SOURCE eds1 SET LOCATION = 'someServerLocation'", new ParserErrorInfo(6, "SQL46010", "EXTERNALD"));
            ParserTestUtils.ErrorTest130("ALTER EXTERNAL DTA SOURCE eds1 SET LOCATION = 'someServerLocation'", new ParserErrorInfo(15, "SQL46010", "DTA"));
            ParserTestUtils.ErrorTest130("ALTER EXTERNAL DATA SROUCE eds1 SET LOCATION = 'someServerLocation'", new ParserErrorInfo(20, "SQL46005", "SOURCE", "SROUCE"));
            ParserTestUtils.ErrorTest130("ALTER EXTERNAL DATAS ROUCE eds1 SET LOCATION = 'someServerLocation'", new ParserErrorInfo(15, "SQL46010", "DATAS"));

            ParserTestUtils.ErrorTest130("ALTER EXTERNAL DATA SOURCE eds1 SIT LOCATION = 'someServerLocation'", new ParserErrorInfo(32, "SQL46010", "SIT"));
            ParserTestUtils.ErrorTest130("ALTER EXTERNAL DATA SOURCE eds1 SETL OCATION = 'someServerLocation'", new ParserErrorInfo(32, "SQL46010", "SETL"));
            ParserTestUtils.ErrorTest130("ALTER EXTERNAL DATA SOURCE eds1 SET LOCTION = 'someServerLocation'", new ParserErrorInfo(36, "SQL46010", "LOCTION"));
            ParserTestUtils.ErrorTest130("ALTER EXTERNAL DATA SOURCE eds1 SET LOCATION 'someServerLocation'", new ParserErrorInfo(36, "SQL46010", "LOCATION"));

            ParserTestUtils.ErrorTest130("ALTER EXTERNAL DATA SOURCE eds1 SET LOCATION = 'someServerLocation' RESOURCE_MANAGER_LOCATION = 'someResourceManagerLocation'", new ParserErrorInfo(68, "SQL46010", "RESOURCE_MANAGER_LOCATION"));
            ParserTestUtils.ErrorTest130("ALTER EXTERNAL DATA SOURCE eds1 SET LOCATION = 'someServerLocation', RESURCE_MANAGER_LOCATION = 'someResourceManagerLocation'", new ParserErrorInfo(69, "SQL46010", "RESURCE_MANAGER_LOCATION"));
            ParserTestUtils.ErrorTest130("ALTER EXTERNAL DATA SOURCE eds1 SET LOCATION = 'someServerLocation', RESOURCE_MANAGER_LOCATION 'someResourceManagerLocation'", new ParserErrorInfo(69, "SQL46010", "RESOURCE_MANAGER_LOCATION"));

            ParserTestUtils.ErrorTest130("ALTER EXTERNAL DATA SOURCE eds1 SET LOCATION = 'someServerLocation', RESOURCE_MANAGER_LOCATION = 'someResourceManagerLocation' CREDENTIAL = cred1", new ParserErrorInfo(127, "SQL46010", "CREDENTIAL"));
            ParserTestUtils.ErrorTest130("ALTER EXTERNAL DATA SOURCE eds1 SET LOCATION = 'someServerLocation', RESOURCE_MANAGER_LOCATION = 'someResourceManagerLocation', CREDNTIAL = cred1", new ParserErrorInfo(128, "SQL46010", "CREDNTIAL"));
            ParserTestUtils.ErrorTest130("ALTER EXTERNAL DATA SOURCE eds1 SET LOCATION = 'someServerLocation', RESOURCE_MANAGER_LOCATION = 'someResourceManagerLocation', CREDENTIAL cred1", new ParserErrorInfo(128, "SQL46010", "CREDENTIAL"));

            ParserTestUtils.ErrorTest130("ALTER EXTERNAL DATA SOURCE eds1 SET LOCATION = 'someServerLocation' DATABASE_NAME = 'someDatabase'", new ParserErrorInfo(68, "SQL46010", "DATABASE_NAME"));
            ParserTestUtils.ErrorTest130("ALTER EXTERNAL DATA SOURCE eds1 SET LOCATION = 'someServerLocation', DATABASE_NAMES = 'someDatabase'", new ParserErrorInfo(69, "SQL46010", "DATABASE_NAMES"));
            ParserTestUtils.ErrorTest130("ALTER EXTERNAL DATA SOURCE eds1 SET LOCATION = 'someServerLocation', DATABSE_NAME = 'someDatabase'", new ParserErrorInfo(69, "SQL46010", "DATABSE_NAME"));
            ParserTestUtils.ErrorTest130("ALTER EXTERNAL DATA SOURCE eds1 SET LOCATION = 'someServerLocation', DATABASE_NAME 'someDatabase'", new ParserErrorInfo(69, "SQL46010", "DATABASE_NAME"));

            ParserTestUtils.ErrorTest130("ALTER EXTERNAL DATA SOURCE eds1 SET LOCATION = 'someServerLocation', DATABASE_NAME = 'someDatabase' SHARD_MAP_NAME = 'somShardMap'", new ParserErrorInfo(100, "SQL46010", "SHARD_MAP_NAME"));
            ParserTestUtils.ErrorTest130("ALTER EXTERNAL DATA SOURCE eds1 SET LOCATION = 'someServerLocation', DATABASE_NAME = 'someDatabase', SHARDS_MAP_NAME = 'someShardMap'", new ParserErrorInfo(101, "SQL46010", "SHARDS_MAP_NAME"));
            ParserTestUtils.ErrorTest130("ALTER EXTERNAL DATA SOURCE eds1 SET LOCATION = 'someServerLocation', DATABASE_NAME = 'someDatabase', SHRD_MAP_NAME = 'someShardMap'", new ParserErrorInfo(101, "SQL46010", "SHRD_MAP_NAME"));
            ParserTestUtils.ErrorTest130("ALTER EXTERNAL DATA SOURCE eds1 SET LOCATION = 'someServerLocation', DATABASE_NAME = 'someDatabase', SHARD_MAP_NAME 'someShardMap'", new ParserErrorInfo(101, "SQL46010", "SHARD_MAP_NAME"));

            // Missing required properties
            //
            ParserTestUtils.ErrorTest130("ALTER EXTERNAL DATA SOURCE", new ParserErrorInfo(26, "SQL46029"));
            ParserTestUtils.ErrorTest130("ALTER EXTERNAL DATA SOURCE eds1", new ParserErrorInfo(31, "SQL46029"));

            // Alter external data source with incorrect data source location value
            //
            ParserTestUtils.ErrorTest130("ALTER EXTERNAL DATA SOURCE eds1 SET LOCATION = someServerLocation, RESOURCE_MANAGER_LOCATION = 'someResourceManagerLocation', CREDENTIAL = cred1", new ParserErrorInfo(47, "SQL46010", "someServerLocation"));

            // Alter external data source with incorrect resource manager location value
            //
            ParserTestUtils.ErrorTest130("ALTER EXTERNAL DATA SOURCE eds1 SET LOCATION = 'someServerLocation', RESOURCE_MANAGER_LOCATION = someResourceManagerLocation, CREDENTIAL = cred1", new ParserErrorInfo(69, "SQL46010", "RESOURCE_MANAGER_LOCATION"));

            // Alter external data source with database name, shard map and credential incorrect values
            //
            ParserTestUtils.ErrorTest130("ALTER EXTERNAL DATA SOURCE eds1 SET LOCATION = someServer, DATABASE_NAME = 'someDatabase', SHARD_MAP_NAME = 'someShardMap', CREDENTIAL = someCred", new ParserErrorInfo(47, "SQL46010", "someServer"));
            ParserTestUtils.ErrorTest130("ALTER EXTERNAL DATA SOURCE eds1 SET LOCATION = 'someServer', DATABASE_NAME = someDatabase, SHARD_MAP_NAME = 'someShardMap', CREDENTIAL = someCred", new ParserErrorInfo(61, "SQL46010", "DATABASE_NAME"));
            ParserTestUtils.ErrorTest130("ALTER EXTERNAL DATA SOURCE eds1 SET LOCATION = 'someServer', DATABASE_NAME = 'someDatabase', SHARD_MAP_NAME = someShardMap, CREDENTIAL = someCred", new ParserErrorInfo(93, "SQL46010", "SHARD_MAP_NAME"));
            ParserTestUtils.ErrorTest130("ALTER EXTERNAL DATA SOURCE eds1 SET LOCATION = 'someServer', DATABASE_NAME = 'someDatabase', SHARD_MAP_NAME = 'someShardMap', CREDENTIAL = 'someCred'", new ParserErrorInfo(126, "SQL46010", "CREDENTIAL"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void DropExternalDataSourceNegativeTest()
        {
            // Drop external data source keyword typos
            //
            ParserTestUtils.ErrorTest130("DRP EXTERNAL DATA SOURCE eds1", new ParserErrorInfo(0, "SQL46010", "DRP"));
            ParserTestUtils.ErrorTest130("DROP EXTRNAL DATA SOURCE eds1", new ParserErrorInfo(5, "SQL46005", "SERVER", "EXTRNAL"));
            ParserTestUtils.ErrorTest130("DROP EXTERNALD ATA SOURCE eds1", new ParserErrorInfo(5, "SQL46005", "SERVER", "EXTERNALD"));
            ParserTestUtils.ErrorTest130("DROP EXTERNAL DTA SOURCE eds1", new ParserErrorInfo(14, "SQL46010", "DTA"));
            ParserTestUtils.ErrorTest130("DROP EXTERNAL DATA SROUCE eds1", new ParserErrorInfo(19, "SQL46005", "SOURCE", "SROUCE"));
            ParserTestUtils.ErrorTest130("DROP EXTERNAL DATAS ROUCE eds1", new ParserErrorInfo(14, "SQL46010", "DATAS"));
            // Missing required properties
            //
            ParserTestUtils.ErrorTest130("DROP EXTERNAL DATA SOURCE", new ParserErrorInfo(25, "SQL46029"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void CreateExternalFileFormatNegativeTest()
        {
            // Create external file format keyword typos
            //
            ParserTestUtils.ErrorTest130("CREAT EXTERNAL FILE FORMAT eff1 WITH (FORMAT_TYPE = DELIMITEDTEXT)", new ParserErrorInfo(0, "SQL46010", "CREAT"));
            ParserTestUtils.ErrorTest130("CREATE EXTRNAL FILE FORMAT eff1 WITH (FORMAT_TYPE = DELIMITEDTEXT)", new ParserErrorInfo(7, "SQL46010", "EXTRNAL"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNALF ILE FORMAT eff1 WITH (FORMAT_TYPE = DELIMITEDTEXT)", new ParserErrorInfo(7, "SQL46010", "EXTERNALF"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL FLE FORMAT eff1 WITH (FORMAT_TYPE = DELIMITEDTEXT)", new ParserErrorInfo(16, "SQL46010", "FLE"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL FILE FROMAT eff1 WITH (FORMAT_TYPE = DELIMITEDTEXT)", new ParserErrorInfo(21, "SQL46005", "FORMAT", "FROMAT"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL FILEF ORMAT eff1 WITH (FORMAT_TYPE = DELIMITEDTEXT)", new ParserErrorInfo(16, "SQL46010", "FILEF"));

            ParserTestUtils.ErrorTest130("CREATE EXTERNAL FILE FORMAT eff1 WIT (FORMAT_TYPE = DELIMITEDTEXT)", new ParserErrorInfo(33, "SQL46010", "WIT"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL FILE FORMAT eff1 WITH FORMAT_TYPE = DELIMITEDTEXT)", new ParserErrorInfo(38, "SQL46010", "FORMAT_TYPE"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL FILE FORMAT eff1 WITH F(ORMAT_TYPE = DELIMITEDTEXT)", new ParserErrorInfo(38, "SQL46010", "F"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL FILE FORMAT eff1 WITH (ORMAT_TYPE = DELIMITEDTEXT)", new ParserErrorInfo(39, "SQL46005", "FORMAT_TYPE", "ORMAT_TYPE"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL FILE FORMAT eff1 WITH (FORMAT_TYPE DELIMITEDTEXT)", new ParserErrorInfo(51, "SQL46010", "DELIMITEDTEXT"));

            ParserTestUtils.ErrorTest130("CREATE EXTERNAL FILE FORMAT eff1 WITH (FORMAT_TYPE = DELIMITEDTEXT SERDE_METHOD = 'org.apache.hadoop.hive.serde2.columnar.ColumnarSerDe')", new ParserErrorInfo(67, "SQL46010", "SERDE_METHOD"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL FILE FORMAT eff1 WITH (FORMAT_TYPE = DELIMITEDTEXT, SRDE_METHOD = 'org.apache.hadoop.hive.serde2.columnar.ColumnarSerDe')", new ParserErrorInfo(68, "SQL46010", "SRDE_METHOD"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL FILE FORMAT eff1 WITH (FORMAT_TYPE = DELIMITEDTEXT, SERDE_METHOD 'org.apache.hadoop.hive.serde2.columnar.ColumnarSerDe')", new ParserErrorInfo(68, "SQL46010", "SERDE_METHOD"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL FILE FORMAT eff1 WITH (FORMAT_TYPE = DELIMITEDTEXT, SERDE_METHOD = 'org.apache.hadoop.hive.serde2.columnar.ColumnarSerDe' FORMAT_OPTIONS (FIELD_TERMINATOR = '|', STRING_DELIMITER = ';', DATE_FORMAT = 'MM-dd-yyyy', USE_TYPE_DEFAULT = FALSE))",
                new ParserErrorInfo(138, "SQL46010", "FORMAT_OPTIONS"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL FILE FORMAT eff1 WITH (FORMAT_TYPE = DELIMITEDTEXT, SERDE_METHOD = 'org.apache.hadoop.hive.serde2.columnar.ColumnarSerDe', FROMAT_OPTIONS (FIELD_TERMINATOR = '|', STRING_DELIMITER = ';', DATE_FORMAT = 'MM-dd-yyyy', USE_TYPE_DEFAULT = FALSE))",
                new ParserErrorInfo(139, "SQL46010", "FROMAT_OPTIONS"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL FILE FORMAT eff1 WITH (FORMAT_TYPE = DELIMITEDTEXT, SERDE_METHOD = 'org.apache.hadoop.hive.serde2.columnar.ColumnarSerDe', FORMAT_OPTIONS FIELD_TERMINATOR = '|', STRING_DELIMITER = ';', DATE_FORMAT = 'MM-dd-yyyy', USE_TYPE_DEFAULT = FALSE))",
                new ParserErrorInfo(139, "SQL46010", "FORMAT_OPTIONS"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL FILE FORMAT eff1 WITH (FORMAT_TYPE = DELIMITEDTEXT, SERDE_METHOD = 'org.apache.hadoop.hive.serde2.columnar.ColumnarSerDe', FORMAT_OPTIONS F(IELD_TERMINATOR = '|', STRING_DELIMITER = ';', DATE_FORMAT = 'MM-dd-yyyy', USE_TYPE_DEFAULT = FALSE))",
                new ParserErrorInfo(139, "SQL46010", "FORMAT_OPTIONS"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL FILE FORMAT eff1 WITH (FORMAT_TYPE = DELIMITEDTEXT, SERDE_METHOD = 'org.apache.hadoop.hive.serde2.columnar.ColumnarSerDe', FORMAT_OPTIONS (FEILD_TERMINATOR = '|', STRING_DELIMITER = ';', DATE_FORMAT = 'MM-dd-yyyy', USE_TYPE_DEFAULT = FALSE))",
                new ParserErrorInfo(155, "SQL46010", "FEILD_TERMINATOR"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL FILE FORMAT eff1 WITH (FORMAT_TYPE = DELIMITEDTEXT, SERDE_METHOD = 'org.apache.hadoop.hive.serde2.columnar.ColumnarSerDe', FORMAT_OPTIONS (FIELD_TERMINATOR '|', STRING_DELIMITER = ';', DATE_FORMAT = 'MM-dd-yyyy', USE_TYPE_DEFAULT = FALSE))",
                new ParserErrorInfo(155, "SQL46010", "FIELD_TERMINATOR"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL FILE FORMAT eff1 WITH (FORMAT_TYPE = DELIMITEDTEXT, SERDE_METHOD = 'org.apache.hadoop.hive.serde2.columnar.ColumnarSerDe', FORMAT_OPTIONS (FIELD_TERMINATOR = '|' STRING_DELIMITER = ';', DATE_FORMAT = 'MM-dd-yyyy', USE_TYPE_DEFAULT = FALSE))",
                new ParserErrorInfo(178, "SQL46010", "STRING_DELIMITER"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL FILE FORMAT eff1 WITH (FORMAT_TYPE = DELIMITEDTEXT, SERDE_METHOD = 'org.apache.hadoop.hive.serde2.columnar.ColumnarSerDe', FORMAT_OPTIONS (FIELD_TERMINATOR = '|', STRING_DELIMTER = ';', DATE_FORMAT = 'MM-dd-yyyy', USE_TYPE_DEFAULT = FALSE))",
                new ParserErrorInfo(179, "SQL46010", "STRING_DELIMTER"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL FILE FORMAT eff1 WITH (FORMAT_TYPE = DELIMITEDTEXT, SERDE_METHOD = 'org.apache.hadoop.hive.serde2.columnar.ColumnarSerDe', FORMAT_OPTIONS (FIELD_TERMINATOR = '|', STRING_DELIMITER ';', DATE_FORMAT = 'MM-dd-yyyy', USE_TYPE_DEFAULT = FALSE))",
                new ParserErrorInfo(179, "SQL46010", "STRING_DELIMITER"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL FILE FORMAT eff1 WITH (FORMAT_TYPE = DELIMITEDTEXT, SERDE_METHOD = 'org.apache.hadoop.hive.serde2.columnar.ColumnarSerDe', FORMAT_OPTIONS (FIELD_TERMINATOR = '|', STRING_DELIMITER = ';' DATE_FORMAT = 'MM-dd-yyyy', USE_TYPE_DEFAULT = FALSE))",
                new ParserErrorInfo(202, "SQL46010", "DATE_FORMAT"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL FILE FORMAT eff1 WITH (FORMAT_TYPE = DELIMITEDTEXT, SERDE_METHOD = 'org.apache.hadoop.hive.serde2.columnar.ColumnarSerDe', FORMAT_OPTIONS (FIELD_TERMINATOR = '|', STRING_DELIMITER = ';', DATE_FROMAT = 'MM-dd-yyyy', USE_TYPE_DEFAULT = FALSE))",
                new ParserErrorInfo(203, "SQL46010", "DATE_FROMAT"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL FILE FORMAT eff1 WITH (FORMAT_TYPE = DELIMITEDTEXT, SERDE_METHOD = 'org.apache.hadoop.hive.serde2.columnar.ColumnarSerDe', FORMAT_OPTIONS (FIELD_TERMINATOR = '|', STRING_DELIMITER = ';', DATE_FORMAT 'MM-dd-yyyy', USE_TYPE_DEFAULT = FALSE))",
                new ParserErrorInfo(203, "SQL46010", "DATE_FORMAT"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL FILE FORMAT eff1 WITH (FORMAT_TYPE = DELIMITEDTEXT, SERDE_METHOD = 'org.apache.hadoop.hive.serde2.columnar.ColumnarSerDe', FORMAT_OPTIONS (FIELD_TERMINATOR = '|', STRING_DELIMITER = ';', DATE_FORMAT = 'MM-dd-yyyy' USE_TYPE_DEFAULT = FALSE))",
                new ParserErrorInfo(230, "SQL46010", "USE_TYPE_DEFAULT"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL FILE FORMAT eff1 WITH (FORMAT_TYPE = DELIMITEDTEXT, SERDE_METHOD = 'org.apache.hadoop.hive.serde2.columnar.ColumnarSerDe', FORMAT_OPTIONS (FIELD_TERMINATOR = '|', STRING_DELIMITER = ';', DATE_FORMAT = 'MM-dd-yyyy', USE_TYPE_DEFALT = FALSE))",
                new ParserErrorInfo(249, "SQL46010", "FALSE"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL FILE FORMAT eff1 WITH (FORMAT_TYPE = DELIMITEDTEXT, SERDE_METHOD = 'org.apache.hadoop.hive.serde2.columnar.ColumnarSerDe', FORMAT_OPTIONS (FIELD_TERMINATOR = '|', STRING_DELIMITER = ';', DATE_FORMAT = 'MM-dd-yyyy', USE_TYPE_DEFAULT FALSE))",
                new ParserErrorInfo(231, "SQL46010", "USE_TYPE_DEFAULT"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL FILE FORMAT eff1 WITH (FORMAT_TYPE = DELIMITEDTEXT, SERDE_METHOD = 'org.apache.hadoop.hive.serde2.columnar.ColumnarSerDe', FORMAT_OPTIONS (FIELD_TERMINATOR = '|', STRING_DELIMITER = ';', DATE_FORMAT = 'MM-dd-yyyy', USE_TYPE_DEFAULT = FALSE)",
                new ParserErrorInfo(256, "SQL46029"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL FILE FORMAT eff1 WITH (FORMAT_TYPE = DELIMITEDTEXT, SERDE_METHOD = 'org.apache.hadoop.hive.serde2.columnar.ColumnarSerDe', FORMAT_OPTIONS (FIELD_TERMINATOR = '|', STRING_DELIMITER = ';', DATE_FORMAT = 'MM-dd-yyyy', USE_TYPE_DEFAULT = FALSE) DATA_COMPRESSION = 'org.apache.hadoop.io.compress.GzipCodec')",
                new ParserErrorInfo(257, "SQL46010", "DATA_COMPRESSION"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL FILE FORMAT eff1 WITH (FORMAT_TYPE = DELIMITEDTEXT, SERDE_METHOD = 'org.apache.hadoop.hive.serde2.columnar.ColumnarSerDe', FORMAT_OPTIONS (FIELD_TERMINATOR = '|', STRING_DELIMITER = ';', DATE_FORMAT = 'MM-dd-yyyy', USE_TYPE_DEFAULT = FALSE), DATA_COMPRESION = 'org.apache.hadoop.io.compress.GzipCodec')",
                new ParserErrorInfo(258, "SQL46010", "DATA_COMPRESION"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL FILE FORMAT eff1 WITH (FORMAT_TYPE = DELIMITEDTEXT, SERDE_METHOD = 'org.apache.hadoop.hive.serde2.columnar.ColumnarSerDe', FORMAT_OPTIONS (FIELD_TERMINATOR = '|', STRING_DELIMITER = ';', DATE_FORMAT = 'MM-dd-yyyy', USE_TYPE_DEFAULT = FALSE), DATA_COMPRESSION 'org.apache.hadoop.io.compress.GzipCodec')",
                new ParserErrorInfo(258, "SQL46010", "DATA_COMPRESSION"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL FILE FORMAT eff1 WITH (FORMAT_TYPE = DELIMITEDTEXT, SERDE_METHOD = 'org.apache.hadoop.hive.serde2.columnar.ColumnarSerDe', FORMAT_OPTIONS (FIELD_TERMINATOR = '|', STRING_DELIMITER = ';', DATE_FORMAT = 'MM-dd-yyyy', USE_TYPE_DEFAULT = FALSE), DATA_COMPRESSION = 'org.apache.hadoop.io.compress.GzipCodec'",
                new ParserErrorInfo(318, "SQL46029"));
            // Missing required properties
            //
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL FILE FORMAT", new ParserErrorInfo(27, "SQL46029"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL FILE FORMAT eff1", new ParserErrorInfo(32, "SQL46029"));
            // Create external file format with incorrect format type value
            //
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL FILE FORMAT eff1 WITH (FORMAT_TYPE = DELIMITEDTXT, FORMAT_OPTIONS (FIELD_TERMINATOR = '|', STRING_DELIMITER = ';', DATE_FORMAT = 'MM-dd-yyyy', USE_TYPE_DEFAULT = FALSE), SERDE_METHOD = 'org.apache.hadoop.hive.serde2.columnar.ColumnarSerDe', DATA_COMPRESSION = 'org.apache.hadoop.io.compress.GzipCodec')", new ParserErrorInfo(53, "SQL46010", "DELIMITEDTXT"));
            // Create external file format with incorrect field terminatior value
            //
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL FILE FORMAT eff1 WITH (FORMAT_TYPE = DELIMITEDTEXT, FORMAT_OPTIONS (FIELD_TERMINATOR = |, STRING_DELIMITER = ';', DATE_FORMAT = 'MM-dd-yyyy', USE_TYPE_DEFAULT = FALSE), SERDE_METHOD = 'org.apache.hadoop.hive.serde2.columnar.ColumnarSerDe', DATA_COMPRESSION = 'org.apache.hadoop.io.compress.GzipCodec')", new ParserErrorInfo(103, "SQL46010", "|"));
            // Create external file format with incorrect string delimiter value
            //
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL FILE FORMAT eff1 WITH (FORMAT_TYPE = DELIMITEDTEXT, FORMAT_OPTIONS (FIELD_TERMINATOR = '|', STRING_DELIMITER = :, DATE_FORMAT = 'MM-dd-yyyy', USE_TYPE_DEFAULT = FALSE), SERDE_METHOD = 'org.apache.hadoop.hive.serde2.columnar.ColumnarSerDe', DATA_COMPRESSION = 'org.apache.hadoop.io.compress.GzipCodec')", new ParserErrorInfo(127, "SQL46010", ":"));
            // Create external file format with incorrect date format value
            //
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL FILE FORMAT eff1 WITH (FORMAT_TYPE = DELIMITEDTEXT, FORMAT_OPTIONS (FIELD_TERMINATOR = '|', STRING_DELIMITER = ';', DATE_FORMAT = MM-dd-yyyy, USE_TYPE_DEFAULT = FALSE), SERDE_METHOD = 'org.apache.hadoop.hive.serde2.columnar.ColumnarSerDe', DATA_COMPRESSION = 'org.apache.hadoop.io.compress.GzipCodec')", new ParserErrorInfo(146, "SQL46010", "MM"));
            // Create external file format with incorrect use type default value
            //
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL FILE FORMAT eff1 WITH (FORMAT_TYPE = DELIMITEDTEXT, FORMAT_OPTIONS (FIELD_TERMINATOR = '|', STRING_DELIMITER = ';', DATE_FORMAT = 'MM-dd-yyyy', USE_TYPE_DEFAULT = 'FALSE'), SERDE_METHOD = 'org.apache.hadoop.hive.serde2.columnar.ColumnarSerDe', DATA_COMPRESSION = 'org.apache.hadoop.io.compress.GzipCodec')", new ParserErrorInfo(179, "SQL46010", "'FALSE'"));
            // Create external file format with incorrect serialize/deserialize method value
            //
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL FILE FORMAT eff1 WITH (FORMAT_TYPE = DELIMITEDTEXT, FORMAT_OPTIONS (FIELD_TERMINATOR = '|', STRING_DELIMITER = ';', DATE_FORMAT = 'MM-dd-yyyy', USE_TYPE_DEFAULT = FALSE), SERDE_METHOD = org.apache.hadoop.hive.serde2.columnar.ColumnarSerDe, DATA_COMPRESSION = 'org.apache.hadoop.io.compress.GzipCodec')", new ParserErrorInfo(202, "SQL46010", "org"));
            // Create external file format with incorrect data compression value
            //
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL FILE FORMAT eff1 WITH (FORMAT_TYPE = DELIMITEDTEXT, FORMAT_OPTIONS (FIELD_TERMINATOR = '|', STRING_DELIMITER = ';', DATE_FORMAT = 'MM-dd-yyyy', USE_TYPE_DEFAULT = FALSE), SERDE_METHOD = 'org.apache.hadoop.hive.serde2.columnar.ColumnarSerDe', DATA_COMPRESSION = org.apache.hadoop.io.compress.GzipCodec)", new ParserErrorInfo(277, "SQL46010", "org"));
            // Create external file format with incorrect first row value
            //
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL FILE FORMAT eff1 WITH (FORMAT_TYPE = DELIMITEDTEXT, FORMAT_OPTIONS (FIRST_ROW = -1))", new ParserErrorInfo(96, "SQL46010", "-"));
            // Create external file format with two instances of the first row option
            //
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL FILE FORMAT eff1 WITH (FORMAT_TYPE = DELIMITEDTEXT, FORMAT_OPTIONS (FIRST_ROW = 1, FIRST_ROW = 2))", new ParserErrorInfo(111, "SQL46049", "2"));
            // Create external file format with first passed in as a string
            //
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL FILE FORMAT eff1 WITH (FORMAT_TYPE = DELIMITEDTEXT, FORMAT_OPTIONS (FIRST_ROW = '1'))", new ParserErrorInfo(84, "SQL46010", "FIRST_ROW"));
            // Create external file format with FIRST_ROW misspelled as 'FRST_ROW'
            //
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL FILE FORMAT eff1 WITH (FORMAT_TYPE = DELIMITEDTEXT, FORMAT_OPTIONS (FRST_ROW = 1))", new ParserErrorInfo(84, "SQL46010", "FRST_ROW"));
            // Create external file format with two instances of the encoding option
            //
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL FILE FORMAT eff1 WITH (FORMAT_TYPE = DELIMITEDTEXT, FORMAT_OPTIONS (ENCODING = 'UTF8', ENCODING = 'UTF16'))", new ParserErrorInfo(114, "SQL46049", "'UTF16'"));
            // Create external file format with ENCODING misspelled as ENCODNG
            //
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL FILE FORMAT eff1 WITH (FORMAT_TYPE = DELIMITEDTEXT, FORMAT_OPTIONS (ENCODNG = 'UTF8'))", new ParserErrorInfo(84, "SQL46010", "ENCODNG"));
            // Create external file format with incorrect encoding value
            //
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL FILE FORMAT eff1 WITH (FORMAT_TYPE = DELIMITEDTEXT, FORMAT_OPTIONS (ENCODING = 16))", new ParserErrorInfo(84, "SQL46010", "ENCODING"));
            // Create external file format with two instances of parser version option
            //
            ParserTestUtils.ErrorTest160("CREATE EXTERNAL FILE FORMAT eff1 WITH (FORMAT_TYPE = DELIMITEDTEXT, FORMAT_OPTIONS (PARSER_VERSION = '1.0', PARSER_VERSION = '2.0'))", new ParserErrorInfo(125, "SQL46049", "'2.0'"));
            // Create external file format with PARSER_VERSION misspelled as PARSR_VERSION
            //
            ParserTestUtils.ErrorTest160("CREATE EXTERNAL FILE FORMAT eff1 WITH (FORMAT_TYPE = DELIMITEDTEXT, FORMAT_OPTIONS (PARSR_VERSION = '2.0'))", new ParserErrorInfo(84, "SQL46010", "PARSR_VERSION"));
            // Create external file format with incorrect parser version value
            //
            ParserTestUtils.ErrorTest160("CREATE EXTERNAL FILE FORMAT eff1 WITH (FORMAT_TYPE = DELIMITEDTEXT, FORMAT_OPTIONS (PARSER_VERSION = 16))", new ParserErrorInfo(84, "SQL46010", "PARSER_VERSION"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void DropExternalFileFormatNegativeTest()
        {
            // Drop external file format keyword typos
            //
            ParserTestUtils.ErrorTest130("DRP EXTERNAL FILE FORMAT eff1", new ParserErrorInfo(0, "SQL46010", "DRP"));
            ParserTestUtils.ErrorTest130("DROP EXTRNAL FILE FORMAT eff1", new ParserErrorInfo(5, "SQL46010", "EXTRNAL"));
            ParserTestUtils.ErrorTest130("DROP EXTERNALF ILE FORMAT eff1", new ParserErrorInfo(5, "SQL46005", "SERVER", "EXTERNALF"));
            ParserTestUtils.ErrorTest130("DROP EXTERNAL ILE FORMAT eff1", new ParserErrorInfo(14, "SQL46010", "ILE"));
            ParserTestUtils.ErrorTest130("DROP EXTERNAL FILE FROMAT eff1", new ParserErrorInfo(19, "SQL46005", "FORMAT", "FROMAT"));
            ParserTestUtils.ErrorTest130("DROP EXTERNAL FILEF OFMAT eff1", new ParserErrorInfo(14, "SQL46010", "FILEF"));
            // Missing required properties
            //
            ParserTestUtils.ErrorTest130("DROP EXTERNAL FILE FORMAT", new ParserErrorInfo(25, "SQL46029"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void CreateExternalTableNegativeTest()
        {
            // Create external table keyword typos
            //
            ParserTestUtils.ErrorTest130("CREAT EXTERNAL TABLE t1 (c1 INT) WITH (LOCATION = '/test/test.txt', DATA_SOURCE = eds1, FILE_FORMAT = eff1)", new ParserErrorInfo(0, "SQL46010", "CREAT"));
            ParserTestUtils.ErrorTest130("CREATE EXTRNAL TABLE t1 (c1 INT) WITH (LOCATION = '/test/test.txt', DATA_SOURCE = eds1, FILE_FORMAT = eff1)", new ParserErrorInfo(7, "SQL46010", "EXTRNAL"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNALT ABLE t1 (c1 INT) WITH (LOCATION = '/test/test.txt', DATA_SOURCE = eds1, FILE_FORMAT = eff1)", new ParserErrorInfo(7, "SQL46010", "EXTERNALT"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL TABEL t1 (c1 INT) WITH (LOCATION = '/test/test.txt', DATA_SOURCE = eds1, FILE_FORMAT = eff1)", new ParserErrorInfo(16, "SQL46010", "TABEL"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL TABLE t1 c1 INT) WITH (LOCATION = '/test/test.txt', DATA_SOURCE = eds1, FILE_FORMAT = eff1)", new ParserErrorInfo(25, "SQL46010", "c1"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL TABLE t1 (c1 INT WITH (LOCATION = '/test/test.txt', DATA_SOURCE = eds1, FILE_FORMAT = eff1)", new ParserErrorInfo(33, "SQL46010", "WITH"));

            ParserTestUtils.ErrorTest130("CREATE EXTERNAL TABLE t1 (c1 INT) WIT (LOCATION = '/test/test.txt', DATA_SOURCE = eds1, FILE_FORMAT = eff1)", new ParserErrorInfo(34, "SQL46010", "WIT"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL TABLE t1 (c1 INT) WITH LOCATION = '/test/test.txt', DATA_SOURCE = eds1, FILE_FORMAT = eff1)", new ParserErrorInfo(39, "SQL46010", "LOCATION"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL TABLE t1 (c1 INT) WITH L(OCATION = '/test/test.txt', DATA_SOURCE = eds1, FILE_FORMAT = eff1)", new ParserErrorInfo(39, "SQL46010", "L"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL TABLE t1 (c1 INT) WITH (OCATION = '/test/test.txt', DATA_SOURCE = eds1, FILE_FORMAT = eff1)", new ParserErrorInfo(40, "SQL46010", "OCATION"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL TABLE t1 (c1 INT) WITH (LOCATION '/test/test.txt', DATA_SOURCE = eds1, FILE_FORMAT = eff1)", new ParserErrorInfo(40, "SQL46010", "LOCATION"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL TABLE t1 (c1 INT) WITH (LOCATION = '/test/test.txt' DATA_SOURCE = eds1, FILE_FORMAT = eff1)", new ParserErrorInfo(68, "SQL46010", "DATA_SOURCE"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL TABLE t1 (c1 INT) WITH (LOCATION = '/test/test.txt', DATA_SUORCE = eds1, FILE_FORMAT = eff1)", new ParserErrorInfo(69, "SQL46010", "DATA_SUORCE"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL TABLE t1 (c1 INT) WITH (LOCATION = '/test/test.txt', DATA_SOURCE eds1, FILE_FORMAT = eff1)", new ParserErrorInfo(69, "SQL46010", "DATA_SOURCE"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL TABLE t1 (c1 INT) WITH (LOCATION = '/test/test.txt', DATA_SOURCE = eds1 FILE_FORMAT = eff1)", new ParserErrorInfo(88, "SQL46010", "FILE_FORMAT"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL TABLE t1 (c1 INT) WITH (LOCATION = '/test/test.txt', DATA_SOURCE = eds1, FILE_FORMAT eff1)", new ParserErrorInfo(89, "SQL46010", "FILE_FORMAT"));

            ParserTestUtils.ErrorTest130("CREATE EXTERNAL TABLE t1 (c1 INT) WITH (LOCATION = '/test/test.txt', DATA_SOURCE = eds1, FILE_FORMAT = eff1 REJECT_TYPE = VALUE)", new ParserErrorInfo(108, "SQL46010", "REJECT_TYPE"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL TABLE t1 (c1 INT) WITH (LOCATION = '/test/test.txt', DATA_SOURCE = eds1, FILE_FORMAT = eff1, REJCT_TYPE = VALUE)", new ParserErrorInfo(109, "SQL46010", "REJCT_TYPE"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL TABLE t1 (c1 INT) WITH (LOCATION = '/test/test.txt', DATA_SOURCE = eds1, FILE_FORMAT = eff1, REJECT_TYPE VALUE)", new ParserErrorInfo(109, "SQL46010", "REJECT_TYPE"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL TABLE t1 (c1 INT) WITH (LOCATION = '/test/test.txt', DATA_SOURCE = eds1, FILE_FORMAT = eff1, REJECT_TYPE = VALUE REJECT_VALUE = 0.0)", new ParserErrorInfo(129, "SQL46010", "REJECT_VALUE"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL TABLE t1 (c1 INT) WITH (LOCATION = '/test/test.txt', DATA_SOURCE = eds1, FILE_FORMAT = eff1, REJECT_TYPE = VALUE, REJCT_VALUE = 0.0)", new ParserErrorInfo(130, "SQL46010", "REJCT_VALUE"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL TABLE t1 (c1 INT) WITH (LOCATION = '/test/test.txt', DATA_SOURCE = eds1, FILE_FORMAT = eff1, REJECT_TYPE = VALUE, REJECT_VALUE 0.0)", new ParserErrorInfo(130, "SQL46010", "REJECT_VALUE"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL TABLE t1 (c1 INT) WITH (LOCATION = '/test/test.txt', DATA_SOURCE = eds1, FILE_FORMAT = eff1, REJECT_TYPE = VALUE, REJECT_VALUE = 0.0 REJECT_SAMPLE_VALUE = 0.0)", new ParserErrorInfo(149, "SQL46010", "REJECT_SAMPLE_VALUE"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL TABLE t1 (c1 INT) WITH (LOCATION = '/test/test.txt', DATA_SOURCE = eds1, FILE_FORMAT = eff1, REJECT_TYPE = VALUE, REJECT_VALUE = 0.0, REJECT_SAMPEL_VALUE = 0.0)", new ParserErrorInfo(150, "SQL46010", "REJECT_SAMPEL_VALUE"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL TABLE t1 (c1 INT) WITH (LOCATION = '/test/test.txt', DATA_SOURCE = eds1, FILE_FORMAT = eff1, REJECT_TYPE = VALUE, REJECT_VALUE = 0.0, REJECT_SAMPLE_VALUE 0.0)", new ParserErrorInfo(150, "SQL46010", "REJECT_SAMPLE_VALUE"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL TABLE t1 (c1 INT) WITH (LOCATION = '/test/test.txt', DATA_SOURCE = eds1, FILE_FORMAT = eff1, REJECT_TYPE = VALUE, REJECT_VALUE = 0.0, REJECT_SAMPLE_VALUE = 0.0", new ParserErrorInfo(175, "SQL46029"));

            ParserTestUtils.ErrorTest130("CREATE EXTERNAL TABLE t1 (c1 INT) WITH (DATA_SOURCE = eds1, DISTRIBUTON = REPLICATED)", new ParserErrorInfo(60, "SQL46010", "DISTRIBUTON"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL TABLE t1 (c1 INT) WITH (DATA_SOURCE = eds1, DISTRIBUTION REPLICATED)", new ParserErrorInfo(60, "SQL46010", "DISTRIBUTION"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL TABLE t1 (c1 INT) WITH (DATA_SOURCE = eds1, DISTRIBUTION ROUND_ROBIN)", new ParserErrorInfo(60, "SQL46010", "DISTRIBUTION"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL TABLE t1 (c1 INT) WITH (DATA_SOURCE = eds1, DISTRIBUTION SHARDED(c1))", new ParserErrorInfo(60, "SQL46010", "DISTRIBUTION"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL TABLE t1 (c1 INT) WITH (DATA_SOURCE = eds1, DISTRIBUTION = SHARDED(c1), SCHMA_NAME = 'sys')", new ParserErrorInfo(88, "SQL46010", "SCHMA_NAME"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL TABLE t1 (c1 INT) WITH (DATA_SOURCE = eds1, DISTRIBUTION = REPLICATED, SCHEMA_NAME = 'sys', OBJ_NAME = 'tables')", new ParserErrorInfo(108, "SQL46010", "OBJ_NAME"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL TABLE t1 (c1 INT) WITH (DATA_SOURCE = eds1, DISTRIBUTION = SHARDED(c1), SCHEMA_NAME 'sys')", new ParserErrorInfo(88, "SQL46010", "SCHEMA_NAME"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL TABLE t1 (c1 INT) WITH (DATA_SOURCE = eds1, DISTRIBUTION = REPLICATED, SCHEMA_NAME = 'sys', OBJECT_NAME 'tables')", new ParserErrorInfo(108, "SQL46010", "OBJECT_NAME"));
            ParserTestUtils.ErrorTest160("CREATE EXTERNAL TABLE t1 (c1 INT) WITH (LOCATION = '/test/test.txt', DATA_SOURCE = eds1 TABLE_OPTIONS = N'{\"READ_OPTIONS\":[\"ALLOW_INCONSISTENT_READS\"]}')", new ParserErrorInfo(88, "SQL46010", "TABLE_OPTIONS"));
            ParserTestUtils.ErrorTest160("CREATE EXTERNAL TABLE t1 (c1 INT) WITH (LOCATION = '/test/test.txt', DATA_SOURCE = eds1, TBL_OPTIONS = N'{\"READ_OPTIONS\":[\"ALLOW_INCONSISTENT_READS\"]}')", new ParserErrorInfo(89, "SQL46010", "TBL_OPTIONS"));
            ParserTestUtils.ErrorTest160("CREATE EXTERNAL TABLE t1 (c1 INT) WITH (LOCATION = '/test/test.txt', DATA_SOURCE = eds1, TABLE_OPTIONS N'{\"READ_OPTIONS\":[\"ALLOW_INCONSISTENT_READS\"]}')", new ParserErrorInfo(89, "SQL46010", "TABLE_OPTIONS"));
            ParserTestUtils.ErrorTest160("CREATE EXTERNAL TABLE t1 (c1 INT) WITH (LOCATION = '/test/test.txt', DATA_SOURCE = eds1, TABLE_OPTIONS = N'{\"READ_OPTIONS\":[\"ALLOW_INCONSISTENT_READS\"]}'", new ParserErrorInfo(153, "SQL46029"));

            // Missing required properties
            //
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL TABLE", new ParserErrorInfo(21, "SQL46029"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL TABLE t1", new ParserErrorInfo(24, "SQL46029"));
            // Create external table with incorrect location value
            //
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL TABLE t1 (c1 INT) WITH (LOCATION = /test/test.txt)", new ParserErrorInfo(51, "SQL46010", "/"));
            // Create external table with incorrect reject type value
            //
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL TABLE t1 (c1 INT) WITH (LOCATION = '/test/test.txt', REJECT_TYPE = VALU)", new ParserErrorInfo(83, "SQL46010", "VALU"));
            // Create external table with incorrect reject value
            //
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL TABLE t1 (c1 INT) WITH (LOCATION = '/test/test.txt', REJECT_TYPE = VALUE, REJECT_VALUE = zero)", new ParserErrorInfo(105, "SQL46010", "zero"));
            // Create external table with incorrect reject sample value
            //
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL TABLE t1 (c1 INT) WITH (LOCATION = '/test/test.txt', REJECT_TYPE = PERCENTAGE, REJECT_SAMPLE_VALUE = ten)", new ParserErrorInfo(117, "SQL46010", "ten"));

            // Create external table with incorrect DISTRIBUTION value
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL TABLE t1 (c1 INT) WITH (DATA_SOURCE = eds1, DISTRIBUTION = EPLICATED)", new ParserErrorInfo(75, "SQL46010", "EPLICATED"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL TABLE t1 (c1 INT) WITH (DATA_SOURCE = eds1, DISTRIBUTION = REPLICATED(c1))", new ParserErrorInfo(75, "SQL46010", "REPLICATED"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL TABLE t1 (c1 INT) WITH (DATA_SOURCE = eds1, DISTRIBUTION = ROUNDROBIN)", new ParserErrorInfo(75, "SQL46010", "ROUNDROBIN"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL TABLE t1 (c1 INT) WITH (DATA_SOURCE = eds1, DISTRIBUTION = ROUND_ROBIN(c1))", new ParserErrorInfo(75, "SQL46010", "ROUND_ROBIN"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL TABLE t1 (c1 INT) WITH (DATA_SOURCE = eds1, DISTRIBUTION = SHARDED)", new ParserErrorInfo(75, "SQL46010", "SHARDED"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL TABLE t1 (c1 INT) WITH (DATA_SOURCE = eds1, DISTRIBUTION = SHARDED c1)", new ParserErrorInfo(75, "SQL46010", "SHARDED"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL TABLE t1 (c1 INT) WITH (DATA_SOURCE = eds1, DISTRIBUTION = SHARDED c1))", new ParserErrorInfo(75, "SQL46010", "SHARDED"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL TABLE t1 (c1 INT) WITH (DATA_SOURCE = eds1, DISTRIBUTION = SHARDED(c1)", new ParserErrorInfo(86, "SQL46029"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL TABLE t1 (c1 INT) WITH (DATA_SOURCE = eds1, DISTRIBUTION = SHARDED('c1'))", new ParserErrorInfo(83, "SQL46010", "'c1'"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL TABLE t1 (c1 INT) WITH (DATA_SOURCE = eds1, DISTRIBUTION = SHARDED(57))", new ParserErrorInfo(83, "SQL46010", "57"));

            // Create external table with incorrectly quoted SCHEMA_NAME and OBJECT_NAME
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL TABLE t1 (c1 INT) WITH (DATA_SOURCE = eds1, DISTRIBUTION = SHARDED(c1), SCHEMA_NAME = 'sys)", new ParserErrorInfo(102, "SQL46030", "'sys)"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL TABLE t1 (c1 INT) WITH (DATA_SOURCE = eds1, DISTRIBUTION = REPLICATED, SCHEMA_NAME = sys')", new ParserErrorInfo(104, "SQL46030", "')"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL TABLE t1 (c1 INT) WITH (DATA_SOURCE = eds1, DISTRIBUTION = ROUND_ROBIN, SCHEMA_NAME = 'sys', OBJECT_NAME = 'tables)", new ParserErrorInfo(123, "SQL46030", "'tables)"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL TABLE t1 (c1 INT) WITH (DATA_SOURCE = eds1, DISTRIBUTION = SHARDED(c1), SCHEMA_NAME = 'sys', OBJECT_NAME = tables')", new ParserErrorInfo(129, "SQL46030", "')"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL TABLE t1 (c1 INT) WITH (DATA_SOURCE = eds1, DISTRIBUTION = SHARDED(c1), SCHEMA_NAME = sys)", new ParserErrorInfo(88, "SQL46010", "SCHEMA_NAME"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL TABLE t1 (c1 INT) WITH (DATA_SOURCE = eds1, DISTRIBUTION = SHARDED(c1), SCHEMA_NAME = 'sys', OBJECT_NAME = tables)", new ParserErrorInfo(109, "SQL46010", "OBJECT_NAME"));

            // Create external table with unsupported TABLE_OPTIONS for SQL 150
            ParserTestUtils.ErrorTest150("CREATE EXTERNAL TABLE t1 (c1 INT) WITH (LOCATION = '/test/test.txt', DATA_SOURCE = eds1, TABLE_OPTIONS =  N'{\"READ_OPTIONS\":[\"ALLOW_INCONSISTENT_READS\"]}')", new ParserErrorInfo(89, "SQL46010", "TABLE_OPTIONS"));
        }

        /// <summary>
        /// Tests Sql DW REJECTED_ROW_LOCATION
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void CreateExternalTableRejectedRowLocationErrorTests()
        {
            var testScript1 = "CREATE EXTERNAL TABLE t1 (c1 INT) WITH (LOCATION = '/test/test.txt', DATA_SOURCE = eds1, FILE_FORMAT = eff1, REJECT_TYPE = VALUE, REJECT_VALUE = 0.0, REJECT_SAMPLE_VALUE = 0.0 REJECTED_ROW_LOCATION = '/REJECT_Directory')";
            var testScript2 = "CREATE EXTERNAL TABLE t1 (c1 INT) WITH (LOCATION = '/test/test.txt', DATA_SOURCE = eds1, FILE_FORMAT = eff1, REJECT_TYPE = VALUE, REJECT_VALUE = 0.0, REJECT_SAMPLE_VALUE = 0.0 REJECTED_ROW_LOCATION '/REJECT_Directory')";
            var testScript3 = "CREATE EXTERNAL TABLE t1 (c1 INT) WITH (LOCATION = '/test/test.txt', DATA_SOURCE = eds1, FILE_FORMAT = eff1, REJECT_TYPE = VALUE, REJECT_VALUE = 0.0, REJECT_SAMPLE_VALUE = 0.0 REJECTED_RWO_LOCATION = '/REJECT_Directory')";
            var testScript4 = "CREATE EXTERNAL TABLE t1 (c1 INT) WITH (LOCATION = '/test/test.txt', DATA_SOURCE = eds1, FILE_FORMAT = eff1, REJECT_TYPE = VALUE, REJECT_VALUE = 0.0, REJECT_SAMPLE_VALUE = 0.0 REJECT_ROW_LOCATION = '/REJECT_Directory')";
            var testScript5 = "CREATE EXTERNAL TABLE t1 (c1 INT) WITH (LOCATION = '/test/test.txt', DATA_SOURCE = eds1, FILE_FORMAT = eff1, REJECT_TYPE = VALUE, REJECT_VALUE = 0.0, REJECT_SAMPLE_VALUE = 0.0 REJECTED_ROW_LOCATION = 0.0)";

            ParserTestUtils.ErrorTest130(testScript1, new ParserErrorInfo(176, "SQL46010", "REJECTED_ROW_LOCATION"));
            ParserTestUtils.ErrorTest130(testScript2, new ParserErrorInfo(176, "SQL46010", "REJECTED_ROW_LOCATION"));
            ParserTestUtils.ErrorTest130(testScript3, new ParserErrorInfo(176, "SQL46010", "REJECTED_RWO_LOCATION"));
            ParserTestUtils.ErrorTest130(testScript4, new ParserErrorInfo(176, "SQL46010", "REJECT_ROW_LOCATION"));
            ParserTestUtils.ErrorTest130(testScript5, new ParserErrorInfo(176, "SQL46010", "REJECTED_ROW_LOCATION"));

            ParserTestUtils.ErrorTest140(testScript1, new ParserErrorInfo(176, "SQL46010", "REJECTED_ROW_LOCATION"));
            ParserTestUtils.ErrorTest140(testScript2, new ParserErrorInfo(176, "SQL46010", "REJECTED_ROW_LOCATION"));
            ParserTestUtils.ErrorTest140(testScript3, new ParserErrorInfo(176, "SQL46010", "REJECTED_RWO_LOCATION"));
            ParserTestUtils.ErrorTest140(testScript4, new ParserErrorInfo(176, "SQL46010", "REJECT_ROW_LOCATION"));
            ParserTestUtils.ErrorTest140(testScript5, new ParserErrorInfo(176, "SQL46010", "REJECTED_ROW_LOCATION"));

            ParserTestUtils.ErrorTest150(testScript1, new ParserErrorInfo(176, "SQL46010", "REJECTED_ROW_LOCATION"));
            ParserTestUtils.ErrorTest150(testScript2, new ParserErrorInfo(176, "SQL46010", "REJECTED_ROW_LOCATION"));
            ParserTestUtils.ErrorTest150(testScript3, new ParserErrorInfo(176, "SQL46010", "REJECTED_RWO_LOCATION"));
            ParserTestUtils.ErrorTest150(testScript4, new ParserErrorInfo(176, "SQL46010", "REJECT_ROW_LOCATION"));
            ParserTestUtils.ErrorTest150(testScript5, new ParserErrorInfo(176, "SQL46010", "REJECTED_ROW_LOCATION"));
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void CreateExternalTableCtasStatementErrorTest()
        {
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL TABLE t1 AS SELECT 1; ", new ParserErrorInfo(25, "SQL46010", "AS"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL TABLE t1 WITH (LOCATION = '/test/test.txt', DATA_SOURCE = eds1, FILE_FORMAT = eff1)) AS SELECT 1; ", new ParserErrorInfo(99, "SQL46010", ")"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL TABLE t1 WITH (LOCATION = '/test/test.txt', DATA_SOURCE = eds1, FILE_FORMAT = eff1) SELECT 1; ", new ParserErrorInfo(100, "SQL46010", "SELECT"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL TABLE t1 WITH (LOCATION = '/test/test.txt', DATA_SOURCE = eds1, FILE_FORMAT = eff1) AS 1; ", new ParserErrorInfo(103, "SQL46010", "1"));
            ParserTestUtils.ErrorTest130("CREATE PROCEDURE dwsyntaxforsqldom AS BEGIN CREATE EXTERNAL TABLE t1 WITH (LOCATION = '/test/test.txt', DATA_SOURCE = eds1, FILE_FORMAT = eff1) AS SELECT * FROM[dbo].[DimSalesTerritory]",
                new ParserErrorInfo(185, "SQL46029"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL TABLE t1 WITH (LOCATION = '/test/test.txt', DATA_SOURCE = eds1, FILE_FORMAT = eff1) AS SELECT p.ProductKey FROM dbo.DimProduct p RIGHT JOIN dbo.stg_DimProduct s; ", new ParserErrorInfo(176, "SQL46010", ";"));
            ParserTestUtils.ErrorTest130("CREATE EXTERNAL TABLE t1 WITH (LOCATION = '/test/test.txt', DATA_SOURCE = eds1, FILE_FORMAT = eff1, REJECT_TYPE = VALUE, REJECT_VALUE = 0.0, REJECT_SAMPLE_VALUE = 0.0, REJECTED_ROW_LOCATION = '/REJECT_Directory') AS SELECT 1; ", new ParserErrorInfo(22, "SQL46128"));
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void DropExternalTableNegativeTest()
        {
            // Drop external table keyword typos
            //
            ParserTestUtils.ErrorTest130("DRP EXTERNAL TABLE t1", new ParserErrorInfo(0, "SQL46010", "DRP"));
            ParserTestUtils.ErrorTest130("DROP EXTRNAL TABLE t1", new ParserErrorInfo(5, "SQL46010", "EXTRNAL"));
            ParserTestUtils.ErrorTest130("DROP EXTERNALT ABLE t1", new ParserErrorInfo(5, "SQL46005", "SERVER", "EXTERNALT"));
            ParserTestUtils.ErrorTest130("DROP EXTERNAL TBLE t1", new ParserErrorInfo(14, "SQL46010", "TBLE"));
            // Missing required properties
            //
            ParserTestUtils.ErrorTest130("DROP EXTERNAL TABLE", new ParserErrorInfo(19, "SQL46029"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void CreateUserFromExternalProviderNegativeTest()
        {
            // Drop external data source keyword typos
            //
            ParserTestUtils.ErrorTest130("CREAT USER user1 FROM EXTERNAL PROVIDER", new ParserErrorInfo(0, "SQL46010", "CREAT"));
            ParserTestUtils.ErrorTest130("CREATE USER user1 FROM EXTERNALD PROVIDER", new ParserErrorInfo(23, "SQL46010", "EXTERNALD"));
            ParserTestUtils.ErrorTest130("CREATE USER user1 FROM EXTERNAL PROVIDER WITH SID", new ParserErrorInfo(49, "SQL46029"));
            ParserTestUtils.ErrorTest130("CREATE USER user1 WITH TYPO = X, PASSWORD = 'PLACEHOLDER1'", new ParserErrorInfo(23, "SQL46005", "TYPE", "TYPO"));
            // Missing required properties
            //
            ParserTestUtils.ErrorTest130("CREATE USER FROM EXTERNAL PROVIDER", new ParserErrorInfo(12, "SQL46010", "FROM"));

        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void AlterTableAlterColumnOnlineTestsNegativeTest()
        {
            ParserTestUtils.ErrorTest130("ALTER TABLE t1 ALTER COLUMN c1 VARCHAR (20) WITH (ONLINE2 = ON);",
                new ParserErrorInfo(50, "SQL46010", "ONLINE2", ""));
            ParserTestUtils.ErrorTest130("ALTER TABLE t1 ALTER COLUMN c1 VARCHAR (20) WITH (ONLINE = ONN);",
                new ParserErrorInfo(59, "SQL46010", "ONN", ""));
            ParserTestUtils.ErrorTest130("ALTER TABLE t1 ALTER COLUMN c1 VARCHAR (20) (ONLINE = ON);",
                new ParserErrorInfo(45, "SQL46010", "ONLINE", ""));
            ParserTestUtils.ErrorTest130("ALTER TABLE t1 ALTER COLUMN c1 VARCHAR (20) WITH ONLINE = ON);",
                new ParserErrorInfo(49, "SQL46010", "ONLINE", ""));
            ParserTestUtils.ErrorTest130("ALTER TABLE t1 ALTER COLUMN c1 VARCHAR (20) WITH (ONLINE = ON",
                new ParserErrorInfo(61, "SQL46029", "", ""));
            ParserTestUtils.ErrorTest130("ALTER TABLE t1 ALTER COLUMN c1 VARCHAR (20) WITH (ONLINE ON);",
                new ParserErrorInfo(50, "SQL46010", "ONLINE", ""));
            ParserTestUtils.ErrorTest130("ALTER TABLE t1 ALTER COLUMN c1 VARCHAR (20) WITH (SORT_IN_TEMPDB = ON);",
                new ParserErrorInfo(50, "SQL46057", "SORT_IN_TEMPDB", "ALTER TABLE ALTER COLUMN"));
            ParserTestUtils.ErrorTest130("ALTER TABLE t1 ALTER COLUMN c1 VARCHAR (20) WITH (ONLINE = ON (WAIT_AT_LOW_PRIORITY ( MAX_DURATION = 3, ABORT_AFTER_WAIT = NONE)));",
                new ParserErrorInfo(63, "SQL46057", "WAIT_AT_LOW_PRIORITY", "ALTER TABLE ALTER COLUMN"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest, Feature.AlwaysEncrypted)]
        public void AlwaysEncryptedNegativeTest()
        {

            // Create column master key definition keyword typo should be CREATE
            //
            ParserTestUtils.ErrorTest130(
@"CREAT COLUMN MASTER KEY
WITH (
     KEY_STORE_PROVIDER_NAME = 'MSSQL_CERTIFICATE_STORE',
     KEY_PATH = 'Current User/Personal/f2260f28d909d21c642a3d8e0b45a830e79a1420'
   );", new ParserErrorInfo(0, "SQL46010", "CREAT"));

            // Drop column master key missing master key name
            //
            ParserTestUtils.ErrorTest130(@"DROP COLUMN MASTER KEY", new ParserErrorInfo(22, "SQL46029"));

            // Create column encryption key typo, should be ENCRYPTED_VALUE
            //
            string cekTypoSyntax = @"
CREATE COLUMN ENCRYPTION KEY OneValueCEK
WITH VALUES
(
    COLUMN_MASTER_KEY = CMK1,
    ALGORITHM = 'RSA_OAEP',
    ENCRYPTED VALUE = 0x01700000016C006F00630061006C006D0061006300680069006E0065002F006D0079002F003200660061006600640038003100320031003400340034006500620031006100320065003000360039003300340038006100350064003400300032003300380065006600620063006300610031006300284FC4316518CF3328A6D9304F65DD2CE387B79D95D077B4156E9ED8683FC0E09FA848275C685373228762B02DF2522AFF6D661782607B4A2275F2F922A5324B392C9D498E4ECFC61B79F0553EE8FB2E5A8635C4DBC0224D5A7F1B136C182DCDE32A00451F1A7AC6B4492067FD0FAC7D3D6F4AB7FC0E86614455DBB2AB37013E0A5B8B5089B180CA36D8B06CDB15E95A7D06E25AACB645D42C85B0B7EA2962BD3080B9A7CDB805C6279FE7DD6941E7EA4C2139E0D4101D8D7891076E70D433A214E82D9030CF1F40C503103075DEEB3D64537D15D244F503C2750CF940B71967F51095BFA51A85D2F764C78704CAB6F015EA87753355367C5C9F66E465C0C66BADEDFDF76FB7E5C21A0D89A2FCCA8595471F8918B1387E055FA0B816E74201CD5C50129D29C015895CD073925B6EA87CAF4A4FAF018C06A3856F5DFB724F42807543F777D82B809232B465D983E6F19DFB572BEA7B61C50154605452A891190FB5A0C4E464862CF5EFAD5E7D91F7D65AA1A78F688E69A1EB098AB42E95C674E234173CD7E0925541AD5AE7CED9A3D12FDFE6EB8EA4F8AAD2629D4F5A18BA3DDCC9CF7F352A892D4BEBDC4A1303F9C683DACD51A237E34B045EBE579A381E26B40DCFBF49EFFA6F65D17F37C6DBA54AA99A65D5573D4EB5BA038E024910A4D36B79A1D4E3C70349DADFF08FD8B4DEE77FDB57F01CB276ED5E676F1EC973154F86
);";
            ParserTestUtils.ErrorTest130(cekTypoSyntax, new ParserErrorInfo(cekTypoSyntax.IndexOf(@"ENCRYPTED VALUE"), "SQL46010", "ENCRYPTED"));

            // Alter column encryption key missing parameters
            //
            string cekMissingParametersSyntax = @"
ALTER COLUMN ENCRYPTION KEY OneValueCEK
ADD VALUE
(
    COLUMN_MASTER_KEY = CMK2
);";
            ParserTestUtils.ErrorTest130(cekMissingParametersSyntax, new ParserErrorInfo(cekMissingParametersSyntax.IndexOf(@");"), "SQL46010", ")"));

            // Alter column encryption key with extra parameters
            //
            string cekExtraParametersSyntax = @"ALTER COLUMN ENCRYPTION KEY OneValueCEK
DROP VALUE
(
    COLUMN_MASTER_KEY = CMK2,
    ALGORITHM = 'RSA_OAEP',
    ENCRYPTED_VALUE = 0x016E000001630075007200720065006E00740075007300650072002F006D0079002F0064006500650063006200660034006100340031003000380034006200350033003200360066003200630062006200350030003600380065003900620061003000320030003600610037003800310066001DDA6134C3B73A90D349C8905782DD819B428162CF5B051639BA46EC69A7C8C8F81591A92C395711493B25DCBCCC57836E5B9F17A0713E840721D098F3F8E023ABCDFE2F6D8CC4339FC8F88630ED9EBADA5CA8EEAFA84164C1095B12AE161EABC1DF778C07F07D413AF1ED900F578FC00894BEE705EAC60F4A5090BBE09885D2EFE1C915F7B4C581D9CE3FDAB78ACF4829F85752E9FC985DEB8773889EE4A1945BD554724803A6F5DC0A2CD5EFE001ABED8D61E8449E4FAA9E4DD392DA8D292ECC6EB149E843E395CDE0F98D04940A28C4B05F747149B34A0BAEC04FFF3E304C84AF1FF81225E615B5F94E334378A0A888EF88F4E79F66CB377E3C21964AACB5049C08435FE84EEEF39D20A665C17E04898914A85B3DE23D56575EBC682D154F4F15C37723E04974DB370180A9A579BC84F6BC9B5E7C223E5CBEE721E57EE07EFDCC0A3257BBEBF9ADFFB00DBF7EF682EC1C4C47451438F90B4CF8DA709940F72CFDC91C6EB4E37B4ED7E2385B1FF71B28A1D2669FBEB18EA89F9D391D2FDDEA0ED362E6A591AC64EF4AE31CA8766C259ECB77D01A7F5C36B8418F91C1BEADDD4491C80F0016B66421B4B788C55127135DA2FA625FB7FD195FB40D90A6C67328602ECAF3EC4F5894BFD84A99EB4753BE0D22E0D4DE6A0ADFEDC80EB1B556749B4A8AD00E73B329C95827AB91C0256347E85E3C5FD6726D0E1FE82C925D3DF4A9
);";
            ParserTestUtils.ErrorTest130(cekExtraParametersSyntax, new ParserErrorInfo(cekExtraParametersSyntax.IndexOf(@"CMK2,") + 4, "SQL46010", ","));

            // Drop column encryption key missing keyworkd
            //
            ParserTestUtils.ErrorTest130(@"DROP COLUMN ENCRYPTION OneValueCEK", new ParserErrorInfo(12, "SQL46010", "ENCRYPTION"));

            // Enable column encryption with invalid encryption type
            //
            string enableColumnEncryptionSyntax = @"CREATE TABLE Customers (
    CustName nvarchar(60),
    SSN varchar(11)
        COLLATE  Latin1_General_BIN2 ENCRYPTED WITH (COLUMN_ENCRYPTION_KEY = TwoValueCEK,
        ENCRYPTION_TYPE = DETERMINISTIC ,
        ALGORITHM = 'AEAD_AES_256_CBC_HMAC_SHA_256'),
    Age int NULL,
    ACTNO varchar(11)
    ENCRYPTED WITH (COLUMN_ENCRYPTION_KEY = TwoValueCEK,
        ENCRYPTION_TYPE = RANDOM,
        ALGORITHM = 'AEAD_AES_256_CBC_HMAC_SHA_256')
);";
            ParserTestUtils.ErrorTest130(enableColumnEncryptionSyntax, new ParserErrorInfo(enableColumnEncryptionSyntax.IndexOf(@"RANDOM"), "SQL46010", "RANDOM"));

            // Duplicate parameters in Create column encryption key statement
            //
            string duplicateAlgorithmParamSyntax = @"
CREATE COLUMN ENCRYPTION KEY OneValueCEK
WITH VALUES
(
    COLUMN_MASTER_KEY = CMK1,
    ALGORITHM = 'RSA_OAEP',
    ALGORITHM = 'RSA_OAEP'
);";
            ParserTestUtils.ErrorTest130(duplicateAlgorithmParamSyntax, new ParserErrorInfo(duplicateAlgorithmParamSyntax.IndexOf("ALGORITHM", 110), "SQL46049", "ALGORITHM"));

            // Create column master key definition statement missing a quote (')
            //
            string cmkMissingQuoteSyntax = @"CREATE COLUMN MASTER KEY
WITH (
     KEY_STORE_PROVIDER_NAME = 'MSSQL_CERTIFICATE_STORE,
     KEY_PATH = 'Current User/Personal/f2260f28d909d21c642a3d8e0b45a830e79a1420');";
            ParserTestUtils.ErrorTest130(cmkMissingQuoteSyntax, new ParserErrorInfo(cmkMissingQuoteSyntax.IndexOf(@"');"), "SQL46030", "');"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void AlterDatabaseScopedCredentialNegativeTest()
        {

            // Keyword typos
            ParserTestUtils.ErrorTest130("ALTR DATABASE SCOPED CREDENTIAL MyCred WITH IDENTITY = 'myIdentity', SECRET = 'mySecret'", new ParserErrorInfo(0, "SQL46010", "ALTR"));
            ParserTestUtils.ErrorTest130("ALTER DATABAS SCOPED CREDENTIAL MyCred WITH IDENTITY = 'myIdentity', SECRET = 'mySecret'", new ParserErrorInfo(6, "SQL46010", "DATABAS"));
            ParserTestUtils.ErrorTest130("ALTER DATABASE SCOPE CREDENTIAL MyCred WITH IDENTITY = 'myIdentity', SECRET = 'mySecret'", new ParserErrorInfo(21, "SQL46005", "REBUILD", "CREDENTIAL"));
            ParserTestUtils.ErrorTest130("ALTER DATABASE SCOPED CREDNTIAL MyCred WITH IDENTITY = 'myIdentity', SECRET = 'mySecret'", new ParserErrorInfo(22, "SQL46005", "REBUILD", "CREDNTIAL"));
            ParserTestUtils.ErrorTest130("ALTER DATABASE SCOPED CREDENTIAL MyCred ITH IDENTITY = 'myIdentity', SECRET = 'mySecret'", new ParserErrorInfo(40, "SQL46010", "ITH"));
            ParserTestUtils.ErrorTest130("ALTER DATABASE SCOPED CREDENTIAL MyCred WITH IDENTY = 'myIdentity', SECRET = 'mySecret'", new ParserErrorInfo(45, "SQL46010", "IDENTY"));
            ParserTestUtils.ErrorTest130("ALTER DATABASE SCOPED CREDENTIAL MyCred WITH IDENTITY = 'myIdentity', SCRET = 'mySecret'", new ParserErrorInfo(70, "SQL46005", "SECRET", "SCRET"));

            // Missing equals sign
            ParserTestUtils.ErrorTest130("ALTER DATABASE SCOPED CREDENTIAL MyCred WITH IDENTITY  'myIdentity', SECRET = 'mySecret'", new ParserErrorInfo(55, "SQL46010", "'myIdentity'"));
            ParserTestUtils.ErrorTest130("ALTER DATABASE SCOPED CREDENTIAL MyCred WITH IDENTITY = 'myIdentity', SECRET 'mySecret'", new ParserErrorInfo(77, "SQL46010", "'mySecret'"));

            // Two equals signs
            ParserTestUtils.ErrorTest130("ALTER DATABASE SCOPED CREDENTIAL MyCred WITH IDENTITY == 'myIdentity', SECRET = 'mySecret'", new ParserErrorInfo(55, "SQL46010", "="));
            ParserTestUtils.ErrorTest130("ALTER DATABASE SCOPED CREDENTIAL MyCred WITH IDENTITY = 'myIdentity', SECRET == 'mySecret'", new ParserErrorInfo(78, "SQL46010", "="));

            // Missing comma and two commas
            ParserTestUtils.ErrorTest130("ALTER DATABASE SCOPED CREDENTIAL MyCred WITH IDENTITY = 'myIdentity' SECRET = 'mySecret'", new ParserErrorInfo(69, "SQL46010", "SECRET"));
            ParserTestUtils.ErrorTest130("ALTER DATABASE SCOPED CREDENTIAL MyCred WITH IDENTITY = 'myIdentity', , SECRET = 'mySecret'", new ParserErrorInfo(70, "SQL46010", ","));

            // Improper quotes
            ParserTestUtils.ErrorTest130("ALTER DATABASE SCOPED CREDENTIAL MyCred WITH IDENTITY = myIdentity', SECRET = 'mySecret'", new ParserErrorInfo(87, "SQL46030", "'"));
            ParserTestUtils.ErrorTest130("ALTER DATABASE SCOPED CREDENTIAL MyCred WITH IDENTITY = 'myIdentity, SECRET = 'mySecret'", new ParserErrorInfo(87, "SQL46030", "'"));
            ParserTestUtils.ErrorTest130("ALTER DATABASE SCOPED CREDENTIAL MyCred WITH IDENTITY = 'myIdentity', SECRET = mySecret'", new ParserErrorInfo(87, "SQL46030", "'"));
            ParserTestUtils.ErrorTest130("ALTER DATABASE SCOPED CREDENTIAL MyCred WITH IDENTITY = 'myIdentity', SECRET = 'mySecret", new ParserErrorInfo(79, "SQL46030", "'mySecret"));
            ParserTestUtils.ErrorTest130("ALTER DATABASE SCOPED CREDENTIAL MyCred WITH IDENTITY = myIdentity, SECRET = 'mySecret'", new ParserErrorInfo(56, "SQL46010", "myIdentity"));
            ParserTestUtils.ErrorTest130("ALTER DATABASE SCOPED CREDENTIAL MyCred WITH IDENTITY = myIdentity', SECRET = mySecret'", new ParserErrorInfo(56, "SQL46010", "myIdentity"));
            ParserTestUtils.ErrorTest130("ALTER DATABASE SCOPED CREDENTIAL MyCred WITH IDENTITY = myIdentity', SECRET = 'mySecret", new ParserErrorInfo(56, "SQL46010", "myIdentity"));
            ParserTestUtils.ErrorTest130("ALTER DATABASE SCOPED CREDENTIAL MyCred WITH IDENTITY = 'myIdentity, SECRET = 'mySecret", new ParserErrorInfo(79, "SQL46010", "mySecret"));
            ParserTestUtils.ErrorTest130("ALTER DATABASE SCOPED CREDENTIAL MyCred WITH IDENTITY = 'myIdentity', SECRET = mySecret", new ParserErrorInfo(79, "SQL46010", "mySecret"));
            ParserTestUtils.ErrorTest130("ALTER DATABASE SCOPED CREDENTIAL MyCred WITH IDENTITY = myIdentity, SECRET = mySecret'", new ParserErrorInfo(85, "SQL46030", "'"));
            ParserTestUtils.ErrorTest130("ALTER DATABASE SCOPED CREDENTIAL MyCred WITH IDENTITY = myIdentity, SECRET = 'mySecret", new ParserErrorInfo(77, "SQL46030", "'mySecret"));
            ParserTestUtils.ErrorTest130("ALTER DATABASE SCOPED CREDENTIAL MyCred WITH IDENTITY = myIdentity', SECRET = mySecret", new ParserErrorInfo(66, "SQL46030", "', SECRET = mySecret"));
            ParserTestUtils.ErrorTest130("ALTER DATABASE SCOPED CREDENTIAL MyCred WITH IDENTITY = 'myIdentity, SECRET = mySecret", new ParserErrorInfo(56, "SQL46030", "'myIdentity, SECRET = mySecret"));
            ParserTestUtils.ErrorTest130("ALTER DATABASE SCOPED CREDENTIAL MyCred WITH IDENTITY = myIdentity, SECRET = mySecret", new ParserErrorInfo(56, "SQL46010", "myIdentity"));

            // Missing keyword
            ParserTestUtils.ErrorTest130("DATABASE SCOPED CREDENTIAL MyCred WITH IDENTITY = 'myIdentity', SECRET = 'mySecret'", new ParserErrorInfo(0, "SQL46010", "DATABASE"));
            ParserTestUtils.ErrorTest130("ALTER SCOPED CREDENTIAL MyCred WITH IDENTITY = 'myIdentity', SECRET = 'mySecret'", new ParserErrorInfo(6, "SQL46010", "SCOPED"));
            ParserTestUtils.ErrorTest130("ALTER DATABASE CREDENTIAL MyCred WITH IDENTITY = 'myIdentity', SECRET = 'mySecret'", new ParserErrorInfo(26, "SQL46010", "MyCred"));
            ParserTestUtils.ErrorTest130("ALTER DATABASE SCOPED MyCred WITH IDENTITY = 'myIdentity', SECRET = 'mySecret'", new ParserErrorInfo(22, "SQL46010", "MyCred"));
            ParserTestUtils.ErrorTest130("ALTER DATABASE SCOPED CREDENTIAL MyCred IDENTITY = 'myIdentity', SECRET = 'mySecret'", new ParserErrorInfo(40, "SQL46010", "IDENTITY"));
            ParserTestUtils.ErrorTest130("ALTER DATABASE SCOPED CREDENTIAL MyCred WITH = 'myIdentity', SECRET = 'mySecret'", new ParserErrorInfo(45, "SQL46010", "="));
            ParserTestUtils.ErrorTest130("ALTER DATABASE SCOPED CREDENTIAL MyCred WITH IDENTITY = 'myIdentity', = 'mySecret'", new ParserErrorInfo(70, "SQL46010", "="));

            // Missing cred name
            ParserTestUtils.ErrorTest130("ALTER DATABASE SCOPED CREDENTIAL WITH IDENTITY = 'myIdentity', SECRET = 'mySecret'", new ParserErrorInfo(33, "SQL46010", "WITH"));

            // Improperly quoted cred name (should not have any quotes)
            ParserTestUtils.ErrorTest130("ALTER DATABASE SCOPED CREDENTIAL 'MyCred WITH IDENTITY = 'myIdentity', SECRET = 'mySecret'", new ParserErrorInfo(89, "SQL46030", "'"));
            ParserTestUtils.ErrorTest130("ALTER DATABASE SCOPED CREDENTIAL MyCred' WITH IDENTITY = 'myIdentity', SECRET = 'mySecret'", new ParserErrorInfo(89, "SQL46030", "'"));
            ParserTestUtils.ErrorTest130("ALTER DATABASE SCOPED CREDENTIAL 'MyCred' WITH IDENTITY = 'myIdentity', SECRET = 'mySecret'", new ParserErrorInfo(33, "SQL46010", "'MyCred'"));

            // Missing RHS
            ParserTestUtils.ErrorTest130("ALTER DATABASE SCOPED CREDENTIAL MyCred WITH IDENTITY = , SECRET = 'mySecret'", new ParserErrorInfo(56, "SQL46010", ","));
            ParserTestUtils.ErrorTest130("ALTER DATABASE SCOPED CREDENTIAL MyCred WITH IDENTITY = 'myIdentity', SECRET = ", new ParserErrorInfo(79, "SQL46029"));

            // Extra stuff after statement
            ParserTestUtils.ErrorTest130("ALTER DATABASE SCOPED CREDENTIAL MyCred WITH IDENTITY = 'myIdentity', SECRET = 'mySecret' foobar", new ParserErrorInfo(90, "SQL46010", "foobar"));

            // FOR CRYPTOGRAPHIC PROVIDER CLAUSE
            ParserTestUtils.ErrorTest130("ALTER DATABASE SCOPED CREDENTIAL MyCred WITH IDENTITY = 'myIdentity', SECRET = 'mySecret' FOR CRYPTOGRAPHIC PROVIDER AzureKeyVault_EKM_Prov", new ParserErrorInfo(90, "SQL46010", "FOR"));

            // Switch IDENTITY and SECRET
            ParserTestUtils.ErrorTest130("ALTER DATABASE SCOPED CREDENTIAL MyCred WITH SECRET = 'mySecret', IDENTITY = 'myIdentity'", new ParserErrorInfo(45, "SQL46010", "SECRET"));

            // No IDENTITY
            ParserTestUtils.ErrorTest130("ALTER DATABASE SCOPED CREDENTIAL MyCred WITH SECRET = 'mySecret'", new ParserErrorInfo(45, "SQL46010", "SECRET"));

        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void CreateDatabaseScopedCredentialNegativeTest()
        {

            // Keyword typos
            ParserTestUtils.ErrorTest130("CREAT DATABASE SCOPED CREDENTIAL MyCred WITH IDENTITY = 'myIdentity', SECRET = 'mySecret'", new ParserErrorInfo(0, "SQL46010", "CREAT"));
            ParserTestUtils.ErrorTest130("CREATE DATABAS SCOPED CREDENTIAL MyCred WITH IDENTITY = 'myIdentity', SECRET = 'mySecret'", new ParserErrorInfo(7, "SQL46010", "DATABAS"));
            ParserTestUtils.ErrorTest130("CREATE DATABASE SCOPE CREDENTIAL MyCred WITH IDENTITY = 'myIdentity', SECRET = 'mySecret'", new ParserErrorInfo(22, "SQL46010", "CREDENTIAL"));
            ParserTestUtils.ErrorTest130("CREATE DATABASE SCOPED CREDNTIAL MyCred WITH IDENTITY = 'myIdentity', SECRET = 'mySecret'", new ParserErrorInfo(23, "SQL46010", "CREDNTIAL"));
            ParserTestUtils.ErrorTest130("CREATE DATABASE SCOPED CREDENTIAL MyCred ITH IDENTITY = 'myIdentity', SECRET = 'mySecret'", new ParserErrorInfo(41, "SQL46010", "ITH"));
            ParserTestUtils.ErrorTest130("CREATE DATABASE SCOPED CREDENTIAL MyCred WITH IDENTY = 'myIdentity', SECRET = 'mySecret'", new ParserErrorInfo(46, "SQL46010", "IDENTY"));
            ParserTestUtils.ErrorTest130("CREATE DATABASE SCOPED CREDENTIAL MyCred WITH IDENTITY = 'myIdentity', SCRET = 'mySecret'", new ParserErrorInfo(71, "SQL46005", "SECRET", "SCRET"));

            // Missing equals sign
            ParserTestUtils.ErrorTest130("CREATE DATABASE SCOPED CREDENTIAL MyCred WITH IDENTITY  'myIdentity', SECRET = 'mySecret'", new ParserErrorInfo(56, "SQL46010", "'myIdentity'"));
            ParserTestUtils.ErrorTest130("CREATE DATABASE SCOPED CREDENTIAL MyCred WITH IDENTITY = 'myIdentity', SECRET 'mySecret'", new ParserErrorInfo(78, "SQL46010", "'mySecret'"));

            // Two equals signs
            ParserTestUtils.ErrorTest130("CREATE DATABASE SCOPED CREDENTIAL MyCred WITH IDENTITY == 'myIdentity', SECRET = 'mySecret'", new ParserErrorInfo(56, "SQL46010", "="));
            ParserTestUtils.ErrorTest130("CREATE DATABASE SCOPED CREDENTIAL MyCred WITH IDENTITY = 'myIdentity', SECRET == 'mySecret'", new ParserErrorInfo(79, "SQL46010", "="));

            // Missing comma and two commas
            ParserTestUtils.ErrorTest130("CREATE DATABASE SCOPED CREDENTIAL MyCred WITH IDENTITY = 'myIdentity' SECRET = 'mySecret'", new ParserErrorInfo(70, "SQL46010", "SECRET"));
            ParserTestUtils.ErrorTest130("CREATE DATABASE SCOPED CREDENTIAL MyCred WITH IDENTITY = 'myIdentity', , SECRET = 'mySecret'", new ParserErrorInfo(71, "SQL46010", ","));

            // Improper quotes
            ParserTestUtils.ErrorTest130("CREATE DATABASE SCOPED CREDENTIAL MyCred WITH IDENTITY = myIdentity', SECRET = 'mySecret'", new ParserErrorInfo(88, "SQL46030", "'"));
            ParserTestUtils.ErrorTest130("CREATE DATABASE SCOPED CREDENTIAL MyCred WITH IDENTITY = 'myIdentity, SECRET = 'mySecret'", new ParserErrorInfo(88, "SQL46030", "'"));
            ParserTestUtils.ErrorTest130("CREATE DATABASE SCOPED CREDENTIAL MyCred WITH IDENTITY = 'myIdentity', SECRET = mySecret'", new ParserErrorInfo(88, "SQL46030", "'"));
            ParserTestUtils.ErrorTest130("CREATE DATABASE SCOPED CREDENTIAL MyCred WITH IDENTITY = 'myIdentity', SECRET = 'mySecret", new ParserErrorInfo(80, "SQL46030", "'mySecret"));
            ParserTestUtils.ErrorTest130("CREATE DATABASE SCOPED CREDENTIAL MyCred WITH IDENTITY = myIdentity, SECRET = 'mySecret'", new ParserErrorInfo(57, "SQL46010", "myIdentity"));
            ParserTestUtils.ErrorTest130("CREATE DATABASE SCOPED CREDENTIAL MyCred WITH IDENTITY = myIdentity', SECRET = mySecret'", new ParserErrorInfo(57, "SQL46010", "myIdentity"));
            ParserTestUtils.ErrorTest130("CREATE DATABASE SCOPED CREDENTIAL MyCred WITH IDENTITY = myIdentity', SECRET = 'mySecret", new ParserErrorInfo(57, "SQL46010", "myIdentity"));
            ParserTestUtils.ErrorTest130("CREATE DATABASE SCOPED CREDENTIAL MyCred WITH IDENTITY = 'myIdentity, SECRET = 'mySecret", new ParserErrorInfo(80, "SQL46010", "mySecret"));
            ParserTestUtils.ErrorTest130("CREATE DATABASE SCOPED CREDENTIAL MyCred WITH IDENTITY = 'myIdentity', SECRET = mySecret", new ParserErrorInfo(80, "SQL46010", "mySecret"));
            ParserTestUtils.ErrorTest130("CREATE DATABASE SCOPED CREDENTIAL MyCred WITH IDENTITY = myIdentity, SECRET = mySecret'", new ParserErrorInfo(86, "SQL46030", "'"));
            ParserTestUtils.ErrorTest130("CREATE DATABASE SCOPED CREDENTIAL MyCred WITH IDENTITY = myIdentity, SECRET = 'mySecret", new ParserErrorInfo(78, "SQL46030", "'mySecret"));
            ParserTestUtils.ErrorTest130("CREATE DATABASE SCOPED CREDENTIAL MyCred WITH IDENTITY = myIdentity', SECRET = mySecret", new ParserErrorInfo(67, "SQL46030", "', SECRET = mySecret"));
            ParserTestUtils.ErrorTest130("CREATE DATABASE SCOPED CREDENTIAL MyCred WITH IDENTITY = 'myIdentity, SECRET = mySecret", new ParserErrorInfo(57, "SQL46030", "'myIdentity, SECRET = mySecret"));
            ParserTestUtils.ErrorTest130("CREATE DATABASE SCOPED CREDENTIAL MyCred WITH IDENTITY = myIdentity, SECRET = mySecret", new ParserErrorInfo(57, "SQL46010", "myIdentity"));

            // Missing keyword
            ParserTestUtils.ErrorTest130("DATABASE SCOPED CREDENTIAL MyCred WITH IDENTITY = 'myIdentity', SECRET = 'mySecret'", new ParserErrorInfo(0, "SQL46010", "DATABASE"));
            ParserTestUtils.ErrorTest130("CREATE SCOPED CREDENTIAL MyCred WITH IDENTITY = 'myIdentity', SECRET = 'mySecret'", new ParserErrorInfo(7, "SQL46010", "SCOPED"));
            ParserTestUtils.ErrorTest130("CREATE DATABASE CREDENTIAL MyCred WITH IDENTITY = 'myIdentity', SECRET = 'mySecret'", new ParserErrorInfo(27, "SQL46010", "MyCred"));
            ParserTestUtils.ErrorTest130("CREATE DATABASE SCOPED MyCred WITH IDENTITY = 'myIdentity', SECRET = 'mySecret'", new ParserErrorInfo(23, "SQL46010", "MyCred"));
            ParserTestUtils.ErrorTest130("CREATE DATABASE SCOPED CREDENTIAL MyCred IDENTITY = 'myIdentity', SECRET = 'mySecret'", new ParserErrorInfo(41, "SQL46010", "IDENTITY"));
            ParserTestUtils.ErrorTest130("CREATE DATABASE SCOPED CREDENTIAL MyCred WITH = 'myIdentity', SECRET = 'mySecret'", new ParserErrorInfo(46, "SQL46010", "="));
            ParserTestUtils.ErrorTest130("CREATE DATABASE SCOPED CREDENTIAL MyCred WITH IDENTITY = 'myIdentity', = 'mySecret'", new ParserErrorInfo(71, "SQL46010", "="));

            // Missing cred name
            ParserTestUtils.ErrorTest130("CREATE DATABASE SCOPED CREDENTIAL WITH IDENTITY = 'myIdentity', SECRET = 'mySecret'", new ParserErrorInfo(34, "SQL46010", "WITH"));

            // Improperly quoted cred name (should not have any quotes)
            ParserTestUtils.ErrorTest130("CREATE DATABASE SCOPED CREDENTIAL 'MyCred WITH IDENTITY = 'myIdentity', SECRET = 'mySecret'", new ParserErrorInfo(90, "SQL46030", "'"));
            ParserTestUtils.ErrorTest130("CREATE DATABASE SCOPED CREDENTIAL MyCred' WITH IDENTITY = 'myIdentity', SECRET = 'mySecret'", new ParserErrorInfo(90, "SQL46030", "'"));
            ParserTestUtils.ErrorTest130("CREATE DATABASE SCOPED CREDENTIAL 'MyCred' WITH IDENTITY = 'myIdentity', SECRET = 'mySecret'", new ParserErrorInfo(34, "SQL46010", "'MyCred'"));

            // Missing RHS
            ParserTestUtils.ErrorTest130("CREATE DATABASE SCOPED CREDENTIAL MyCred WITH IDENTITY = , SECRET = 'mySecret'", new ParserErrorInfo(57, "SQL46010", ","));
            ParserTestUtils.ErrorTest130("CREATE DATABASE SCOPED CREDENTIAL MyCred WITH IDENTITY = 'myIdentity', SECRET = ", new ParserErrorInfo(80, "SQL46029"));

            // Extra stuff after statement
            ParserTestUtils.ErrorTest130("CREATE DATABASE SCOPED CREDENTIAL MyCred WITH IDENTITY = 'myIdentity', SECRET = 'mySecret' foobar", new ParserErrorInfo(91, "SQL46010", "foobar"));

            // FOR CRYPTOGRAPHIC PROVIDER CLAUSE
            ParserTestUtils.ErrorTest130("CREATE DATABASE SCOPED CREDENTIAL MyCred WITH IDENTITY = 'myIdentity', SECRET = 'mySecret' FOR CRYPTOGRAPHIC PROVIDER AzureKeyVault_EKM_Prov", new ParserErrorInfo(91, "SQL46010", "FOR"));

            // Switch IDENTITY and SECRET
            ParserTestUtils.ErrorTest130("CREATE DATABASE SCOPED CREDENTIAL MyCred WITH SECRET = 'mySecret', IDENTITY = 'myIdentity'", new ParserErrorInfo(46, "SQL46010", "SECRET"));

            // No IDENTITY
            ParserTestUtils.ErrorTest130("CREATE DATABASE SCOPED CREDENTIAL MyCred WITH SECRET = 'mySecret'", new ParserErrorInfo(46, "SQL46010", "SECRET"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void DropDatabaseScopedCredentialNegativeTest()
        {

            // Keyword typos
            ParserTestUtils.ErrorTest130("DRO DATABASE SCOPED CREDENTIAL MyCred", new ParserErrorInfo(0, "SQL46010", "DRO"));
            ParserTestUtils.ErrorTest130("DROP DATABAS SCOPED CREDENTIAL MyCred", new ParserErrorInfo(5, "SQL46005", "SERVER", "DATABAS"));
            ParserTestUtils.ErrorTest130("DROP DATABASE SCOPE CREDENTIAL MyCred", new ParserErrorInfo(20, "SQL46010", "CREDENTIAL"));
            ParserTestUtils.ErrorTest130("DROP DATABASE SCOPED CREDNTIAL MyCred", new ParserErrorInfo(21, "SQL46010", "CREDNTIAL"));

            // Missing keyword
            ParserTestUtils.ErrorTest130("DATABASE SCOPED CREDENTIAL MyCred", new ParserErrorInfo(0, "SQL46010", "DATABASE"));
            ParserTestUtils.ErrorTest130("DROP SCOPED CREDENTIAL MyCred", new ParserErrorInfo(5, "SQL46005", "SERVER", "SCOPED"));
            ParserTestUtils.ErrorTest130("DROP DATABASE CREDENTIAL MyCred", new ParserErrorInfo(25, "SQL46010", "MyCred"));
            ParserTestUtils.ErrorTest130("DROP DATABASE SCOPED MyCred", new ParserErrorInfo(21, "SQL46010", "MyCred"));

            // Missing cred name
            ParserTestUtils.ErrorTest130("DROP DATABASE SCOPED CREDENTIAL", new ParserErrorInfo(31, "SQL46029"));

            // Improperly quoted cred name (should not have any quotes)
            ParserTestUtils.ErrorTest130("DROP DATABASE SCOPED CREDENTIAL 'MyCred", new ParserErrorInfo(32, "SQL46030", "'MyCred"));
            ParserTestUtils.ErrorTest130("DROP DATABASE SCOPED CREDENTIAL MyCred'", new ParserErrorInfo(38, "SQL46030", "'"));
            ParserTestUtils.ErrorTest130("DROP DATABASE SCOPED CREDENTIAL 'MyCred'", new ParserErrorInfo(32, "SQL46010", "'MyCred'"));

            // Extra stuff after statement
            ParserTestUtils.ErrorTest130("DROP DATABASE SCOPED CREDENTIAL MyCred foobar", new ParserErrorInfo(39, "SQL46010", "foobar"));
        }


        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void CreateMasterKeyNegativeTest()
        {
            // Keyword typos
            ParserTestUtils.ErrorTest130("CREAT MASTER KEY", new ParserErrorInfo(6, "SQL46010", "MASTER"));
            ParserTestUtils.ErrorTest130("CREATE MASTE KEY", new ParserErrorInfo(7, "SQL46010", "MASTE"));
            ParserTestUtils.ErrorTest130("CREATE MASTER KY", new ParserErrorInfo(7, "SQL46010", "MASTER"));
            ParserTestUtils.ErrorTest130("CREATE MASTER KEY BY PASSWD = N'Placeholder'", new ParserErrorInfo(18, "SQL46010", "BY"));

            // Missing keyword
            ParserTestUtils.ErrorTest130("CREATE KEY ENCRYPTION BY PASSWORD = N'Placeholder'", new ParserErrorInfo(0, "SQL46010", "CREATE"), new ParserErrorInfo(7, "SQL46010", "KEY"));
            ParserTestUtils.ErrorTest130("CREATE MASTER KEY ENCRYPTION  = N'Placeholder'", new ParserErrorInfo(18, "SQL46010", "ENCRYPTION"));
            ParserTestUtils.ErrorTest130("CREATE MASTER KEY ENCRYPTION PASSWORD = N'Placeholder'", new ParserErrorInfo(18, "SQL46010", "ENCRYPTION"));

            // Missing password
            // [SuppressMessage("Microsoft.Security", "CS002:SecretInNextLine", Justification="No password in next line")]
            ParserTestUtils.ErrorTest130("CREATE MASTER KEY ENCRYPTION BY PASSWORD = ", new ParserErrorInfo(43, "SQL46029", "="));

            // Missing equal sign, 2 equal signs
            ParserTestUtils.ErrorTest130("CREATE MASTER KEY ENCRYPTION BY PASSWORD N'Placeholder'", new ParserErrorInfo(41, "SQL46010", "N'Placeholder'"));
            ParserTestUtils.ErrorTest130("CREATE MASTER KEY ENCRYPTION BY PASSWORD == 'Placeholder'", new ParserErrorInfo(42, "SQL46010", "="));

            // Improperly quoted password
            ParserTestUtils.ErrorTest130("CREATE MASTER KEY ENCRYPTION BY PASSWORD N'Placeholder", new ParserErrorInfo(41, "SQL46030", "N'Placeholder"));
            ParserTestUtils.ErrorTest130("CREATE MASTER KEY ENCRYPTION BY PASSWORD Placeholder'", new ParserErrorInfo(52, "SQL46030", "'"));

            // Extra stuff after statement
            ParserTestUtils.ErrorTest130("CREATE MASTER KEY CREATE", new ParserErrorInfo(18, "SQL46010", "CREATE"));
            ParserTestUtils.ErrorTest130("CREATE MASTER KEY foobar", new ParserErrorInfo(18, "SQL46010", "foobar"));
            ParserTestUtils.ErrorTest130("CREATE MASTER KEY ENCRYPTION BY PASSWORD = N'Placeholder' KEY", new ParserErrorInfo(58, "SQL46010", "KEY"));
        }

        /// <summary>
        /// Negative tests for CREATE OR ALTER statements
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void CreateOrAlterNegativeTest()
        {
            // CREATE OR ALTER FUNCTIONS
            //
            ParserTestUtils.ErrorTest130("CREATE OR FUNCTION func_test() RETURNS INT AS BEGIN return (0); END;", new ParserErrorInfo(0, "SQL46010", "CREATE"), new ParserErrorInfo(7, "SQL46010", "OR"));
            ParserTestUtils.ErrorTest130("CREATE ALTER FUNCTION func_test() RETURNS INT AS BEGIN return (0); END;", new ParserErrorInfo(0, "SQL46010", "CREATE"), new ParserErrorInfo(7, "SQL46010", "ALTER"), new ParserErrorInfo(13, "SQL46010", "FUNCTION"));
            ParserTestUtils.ErrorTest130("OR ALTER FUNCTION func_test() RETURNS INT AS BEGIN return (0); END;", new ParserErrorInfo(0, "SQL46010", "OR"));
            ParserTestUtils.ErrorTest130("OR FUNCTION func_test() RETURNS INT AS BEGIN return (0); END;", new ParserErrorInfo(0, "SQL46010", "OR"));

            // CREATE OR ALTER PROCEDURE
            //
            ParserTestUtils.ErrorTest130("CREATE OR PROC sp_test AS BEGIN RETURN(0); END;", new ParserErrorInfo(0, "SQL46010", "CREATE"), new ParserErrorInfo(7, "SQL46010", "OR"));
            ParserTestUtils.ErrorTest130("CREATE ALTER PROC sp_test AS BEGIN RETURN(0); END;", new ParserErrorInfo(0, "SQL46010", "CREATE"), new ParserErrorInfo(7, "SQL46010", "ALTER"), new ParserErrorInfo(13, "SQL46010", "PROC"));
            ParserTestUtils.ErrorTest130("OR ALTER PROC sp_test AS BEGIN RETURN(0); END;", new ParserErrorInfo(0, "SQL46010", "OR"));
            ParserTestUtils.ErrorTest130("OR PROC sp_test AS BEGIN RETURN(0); END;", new ParserErrorInfo(0, "SQL46010", "OR"));
            ParserTestUtils.ErrorTest130("CREATE OR PROCEDURE sp_test AS BEGIN RETURN(0); END;", new ParserErrorInfo(0, "SQL46010", "CREATE"), new ParserErrorInfo(7, "SQL46010", "OR"));
            ParserTestUtils.ErrorTest130("CREATE ALTER PROCEDURE sp_test AS BEGIN RETURN(0); END;", new ParserErrorInfo(0, "SQL46010", "CREATE"), new ParserErrorInfo(7, "SQL46010", "ALTER"), new ParserErrorInfo(13, "SQL46010", "PROCEDURE"));
            ParserTestUtils.ErrorTest130("OR ALTER PROCEDURE sp_test AS BEGIN RETURN(0); END;", new ParserErrorInfo(0, "SQL46010", "OR"));
            ParserTestUtils.ErrorTest130("OR PROCEDURE sp_test AS BEGIN RETURN(0); END;", new ParserErrorInfo(0, "SQL46010", "OR"));

            // CREATE OR ALTER TRIGGER
            //
            ParserTestUtils.ErrorTest130("CREATE OR TRIGGER trg_test ON testTable FOR INSERT AS SELECT SUM(col1 + col2) FROM INSERTED;", new ParserErrorInfo(0, "SQL46010", "CREATE"), new ParserErrorInfo(7, "SQL46010", "OR"));
            ParserTestUtils.ErrorTest130("CREATE ALTER TRIGGER trg_test ON testTable FOR INSERT AS SELECT SUM(col1 + col2) FROM INSERTED;", new ParserErrorInfo(0, "SQL46010", "CREATE"), new ParserErrorInfo(7, "SQL46010", "ALTER"), new ParserErrorInfo(13, "SQL46010", "TRIGGER"));
            ParserTestUtils.ErrorTest130("OR ALTER TRIGGER trg_test ON testTable FOR INSERT AS SELECT SUM(col1 + col2) FROM INSERTED;", new ParserErrorInfo(0, "SQL46010", "OR"));
            ParserTestUtils.ErrorTest130("OR TRIGGER trg_test ON testTable FOR INSERT AS SELECT SUM(col1 + col2) FROM INSERTED;", new ParserErrorInfo(0, "SQL46010", "OR"));

            // CREATE OR ALTER VIEW
            //
            ParserTestUtils.ErrorTest130("CREATE OR VIEW view_test AS SELECT col1 FROM testTable;", new ParserErrorInfo(0, "SQL46010", "CREATE"), new ParserErrorInfo(7, "SQL46010", "OR"));
            ParserTestUtils.ErrorTest130("CREATE ALTER VIEW view_test AS SELECT col1 FROM testTable;", new ParserErrorInfo(0, "SQL46010", "CREATE"), new ParserErrorInfo(7, "SQL46010", "ALTER"), new ParserErrorInfo(13, "SQL46010", "VIEW"));
            ParserTestUtils.ErrorTest130("OR ALTER VIEW view_test AS SELECT col1 FROM testTable;", new ParserErrorInfo(0, "SQL46010", "OR"));
            ParserTestUtils.ErrorTest130("OR VIEW view_test AS SELECT col1 FROM testTable;", new ParserErrorInfo(0, "SQL46010", "OR"));
        }

        /// <summary>
        /// Trim built-in negative tests.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TrimBuiltInNegativeTest()
        {
            ParserTestUtils.ErrorTest140("SELECT TRIM(' TestString ';", new ParserErrorInfo(26, "SQL46010", ";"));
            ParserTestUtils.ErrorTest140("SELECT TRIM( TestString ');", new ParserErrorInfo(24, "SQL46030", "');"));
            ParserTestUtils.ErrorTest140("SELECT TRIM('Teg' FROM 'TestString';", new ParserErrorInfo(35, "SQL46010", ";"));
            ParserTestUtils.ErrorTest140("SELECT TRIM(SELECT FROM 'TestString');", new ParserErrorInfo(12, "SQL46010", "SELECT"));
            ParserTestUtils.ErrorTest140("SELECT TRIM('Teg' 'TestString');", new ParserErrorInfo(18, "SQL46010", "'TestString'"));
            ParserTestUtils.ErrorTest140("SELECT TRIM('Teg' FROM 'TestString' 'SomeString');", new ParserErrorInfo(36, "SQL46010", "'SomeString'"));
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void BulkInsertFormatFieldquoteEscapeCharOptionsNegativeTest()
        {
            ParserTestUtils.ErrorTest140("BULK INSERT t1 FROM 'f1' WITH (FORMAT = 'CSB', FIELDQUOTE = '\"');", new ParserErrorInfo(40, "SQL46010", "'CSB'"));
            ParserTestUtils.ErrorTest140("BULK INSERT t1 FROM 'f1' WITH (FORMA = 'CSV', FIELDQUOTE = '\"');", new ParserErrorInfo(31, "SQL46010", "FORMA"));
            ParserTestUtils.ErrorTest140("BULK INSERT t1 FROM 'f1' WITH ('CSV' = FORMAT, FIELDQUOTE = '\"');", new ParserErrorInfo(31, "SQL46010", "'CSV'"));
            ParserTestUtils.ErrorTest140("BULK INSERT t1 FROM 'f1' WITH (FORMAT = 'CSV', '\"' = FIELDQUOTE);", new ParserErrorInfo(47, "SQL46010", "'\"'"));
            ParserTestUtils.ErrorTest140("BULK INSERT t1 FROM 'f1' WITH (FORMAT = 'CSV', QUOTE = '\"');", new ParserErrorInfo(47, "SQL46010", "QUOTE"));
            ParserTestUtils.ErrorTest140("BULK INSERT t1 FROM 'f1' WITH (FORMAT 'CSV', FIELDQUOTE = '\"');", new ParserErrorInfo(31, "SQL46010", "FORMAT"));
            ParserTestUtils.ErrorTest140("BULK INSERT t1 FROM 'f1' WITH FORMAT = 'CSV', FIELDQUOTE = '\"';", new ParserErrorInfo(30, "SQL46010", "FORMAT"));
            ParserTestUtils.ErrorTest140("BULK INSERT t1 FROM 'f1' WITH (FORMAT = CSV, FIELDQUOTE = '\"');", new ParserErrorInfo(40, "SQL46010", "CSV"));
            ParserTestUtils.ErrorTest140("BULK INSERT t1 FROM 'f1' WITH (FORMAT = 'CSV', FIELDQUOTE = '\"', DATAFILETYPE = 'native');", new ParserErrorInfo(0, "SQL46118"));
            ParserTestUtils.ErrorTest140("BULK INSERT t1 FROM 'f1' WITH (FORMAT = 'CSV', FIELDQUOTE = '\"', DATAFILETYPE = 'widenative');", new ParserErrorInfo(0, "SQL46118"));

            // Escape char is supported starting from 150 syntax.
            //
            ParserTestUtils.ErrorTest140("BULK INSERT t1 FROM 'f1' WITH (FORMAT = 'CSV', ESCAPECHAR = '#');", new ParserErrorInfo(47, "SQL46010", "ESCAPECHAR"));
            ParserTestUtils.ErrorTest150("BULK INSERT t1 FROM 'f1' WITH (FORMAT = 'CSV', ESCAPECHAR = '\\', '\"' = FIELDQUOTE);", new ParserErrorInfo(65, "SQL46010", "'\"'"));
            ParserTestUtils.ErrorTest150("BULK INSERT t1 FROM 'f1' WITH (FORMAT = 'CSV', ESCAPE = '\\');", new ParserErrorInfo(47, "SQL46010", "ESCAPE"));
            ParserTestUtils.ErrorTest150("BULK INSERT t1 FROM 'f1' WITH (FORMAT = 'CSV', ESCAPE_CHAR = '\\');", new ParserErrorInfo(47, "SQL46010", "ESCAPE_CHAR"));
            ParserTestUtils.ErrorTest150("BULK INSERT t1 FROM 'f1' WITH (FORMAT = 'CSV', ESCAPECHAR = 20);", new ParserErrorInfo(47, "SQL46010", "ESCAPECHAR"));
            ParserTestUtils.ErrorTest150("DECLARE @var AS varchar(5); SET @var = '#'; BULK INSERT t1 FROM 'f1' WITH (FORMAT = 'CSV', ESCAPECHAR = @var);", new ParserErrorInfo(104, "SQL46010", "@var"));
            ParserTestUtils.ErrorTest150("BULK INSERT t1 FROM 'f1' WITH (FORMAT = 'CSV', '\\' = ESCAPECHAR);", new ParserErrorInfo(47, "SQL46010", "'\\'"));
            ParserTestUtils.ErrorTest150("BULK INSERT t1 FROM 'f1' WITH (FORMAT = 'CSV', ESCAPECHAR);", new ParserErrorInfo(47, "SQL46010", "ESCAPECHAR"));
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void OpenrowsetFormatFieldquoteEscapeCharOptionsNegativeTest()
        {
            ParserTestUtils.ErrorTest140("SELECT * FROM OPENROWSET (BULK 'f1', FORMATFILE = 'ff1', FORMAT = 'CSB', FIELDQUOTE = '\"') AS a;", new ParserErrorInfo(66, "SQL46010", "'CSB'"));
            ParserTestUtils.ErrorTest140("SELECT * FROM OPENROWSET (BULK 'f1', FORMATFILE = 'ff1', FORMA = 'CSV', FIELDQUOTE = '\"') AS a;", new ParserErrorInfo(57, "SQL46010", "FORMA"));
            ParserTestUtils.ErrorTest140("SELECT * FROM OPENROWSET (BULK 'f1', FORMATFILE = 'ff1', 'CSV' = FORMAT, FIELDQUOTE = '\"') AS a;", new ParserErrorInfo(57, "SQL46010", "'CSV'"));
            ParserTestUtils.ErrorTest140("SELECT * FROM OPENROWSET (BULK 'f1', FORMATFILE = 'ff1', FORMAT = 'CSV', '\"' = FIELDQUOTE) AS a;", new ParserErrorInfo(73, "SQL46010", "'\"'"));
            ParserTestUtils.ErrorTest140("SELECT * FROM OPENROWSET (BULK 'f1', FORMATFILE = 'ff1', FORMAT = 'CSV', QUOTE = '\"') AS a;", new ParserErrorInfo(73, "SQL46010", "QUOTE"));
            ParserTestUtils.ErrorTest140("SELECT * FROM OPENROWSET (BULK 'f1', FORMATFILE = 'ff1', FORMAT 'CSV', FIELDQUOTE = '\"') AS a;", new ParserErrorInfo(57, "SQL46010", "FORMAT"));
            ParserTestUtils.ErrorTest140("SELECT * FROM OPENROWSET BULK 'f1', FORMATFILE = 'ff1', FIELDQUOTE = '\"' AS a;", new ParserErrorInfo(25, "SQL46010", "BULK"));
            ParserTestUtils.ErrorTest140("SELECT * FROM OPENROWSET (BULK 'f1', FORMATFILE = 'ff1', FORMAT = CSV, FIELDQUOTE = '\"') AS a;", new ParserErrorInfo(66, "SQL46010", "CSV"));
            ParserTestUtils.ErrorTest140("SELECT * FROM OPENROWSET (BULK 'f1', SINGLE_BLOB, FORMAT = 'CSV', FIELDQUOTE = '\"') AS a;", new ParserErrorInfo(31, "SQL46119"));
            ParserTestUtils.ErrorTest140("SELECT * FROM OPENROWSET (BULK 'f1', SINGLE_CLOB, FORMAT = 'CSV', FIELDQUOTE = '\"') AS a;", new ParserErrorInfo(31, "SQL46119"));
            ParserTestUtils.ErrorTest140("SELECT * FROM OPENROWSET (BULK 'f1', SINGLE_NCLOB, FORMAT = 'CSV', FIELDQUOTE = '\"') AS a;", new ParserErrorInfo(31, "SQL46119"));

            // Escape char is supported starting from 150 syntax.
            //
            ParserTestUtils.ErrorTest140("SELECT * FROM OPENROWSET (BULK 'f1', FORMATFILE = 'ff1', FORMAT = 'CSV', ESCAPECHAR = '\\') AS a;", new ParserErrorInfo(73, "SQL46010", "ESCAPECHAR"));
            ParserTestUtils.ErrorTest150("SELECT * FROM OPENROWSET (BULK 'f1', FORMATFILE = 'ff1', FORMAT = 'CSV', ESCAPECHAR = '\\', '\"' = FIELDQUOTE) AS a;", new ParserErrorInfo(91, "SQL46010", "'\"'"));
            ParserTestUtils.ErrorTest150("SELECT * FROM OPENROWSET (BULK 'f1', FORMATFILE = 'ff1', FORMAT = 'CSV', ESCAPE = '\\') AS a;", new ParserErrorInfo(73, "SQL46010", "ESCAPE"));
            ParserTestUtils.ErrorTest150("SELECT * FROM OPENROWSET (BULK 'f1', FORMATFILE = 'ff1', FORMAT = 'CSV', ESCAPE_CHAR = '\\') AS a;", new ParserErrorInfo(73, "SQL46010", "ESCAPE_CHAR"));
            ParserTestUtils.ErrorTest150("SELECT * FROM OPENROWSET (BULK 'f1', FORMATFILE = 'ff1', FORMAT = 'CSV', ESCAPECHAR = 20);", new ParserErrorInfo(73, "SQL46010", "ESCAPECHAR"));
            ParserTestUtils.ErrorTest150("DECLARE @var AS varchar(5); SET @var = '#'; SELECT * FROM OPENROWSET (BULK 'f1', FORMATFILE = 'ff1', FORMAT = 'CSV', ESCAPECHAR = @var);", new ParserErrorInfo(130, "SQL46010", "@var"));
            ParserTestUtils.ErrorTest150("SELECT * FROM OPENROWSET (BULK 'f1', FORMATFILE = 'ff1', FORMAT = 'CSV', '\\' = ESCAPECHAR);", new ParserErrorInfo(73, "SQL46010", "'\\'"));
            ParserTestUtils.ErrorTest150("SELECT * FROM OPENROWSET (BULK 'f1', FORMATFILE = 'ff1', FORMAT = 'CSV', ESCAPECHAR);", new ParserErrorInfo(73, "SQL46010", "ESCAPECHAR"));
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void OpenrowsetSynapseSqlServerlessOptionsNegativeTest()
        {
            ParserTestUtils.ErrorTest150("SELECT * FROM OPENROWSET (BULK 'f1', FORMAT = 'CSV', HEADER_ROW = TRUE, HEADER_ROW = TRUE) WITH ([region] varchar(100)) AS a;", new ParserErrorInfo(72, "SQL46049", "HEADER_ROW"));
            ParserTestUtils.ErrorTest150("SELECT * FROM OPENROWSET (BULK 'f1', FORMAT = 'CSV', HEADER_ROW = TRUE, PARSER_VERSION = '80.0') AS a;", new ParserErrorInfo(89, "SQL46010", "'80.0'"));
            ParserTestUtils.ErrorTest150("SELECT * FROM OPENROWSET (BULK 'f1', FORMAT = 'CSV') WITH ([region]) AS a;", new ParserErrorInfo(67, "SQL46010", ")"));
            ParserTestUtils.ErrorTest150("SELECT * FROM OPENROWSET (BULK 'f1', FORMAT = 'CSV') WITH ([region] 1) AS a;", new ParserErrorInfo(68, "SQL46010", "1"));
            ParserTestUtils.ErrorTest150("SELECT * FROM OPENROWSET (BULK 'f1', FORMAT = 'CSV') WITH ([region] COLLATE Latin1_General_BIN2) AS a;", new ParserErrorInfo(68, "SQL46010", "COLLATE"));
            ParserTestUtils.ErrorTest150("SELECT * FROM OPENROWSET (BULK 'f1', FORMAT = 'PARQUET', PARSER_VERSION = '1.0') AS a;", new ParserErrorInfo(31, "SQL46142"));
            ParserTestUtils.ErrorTest150("SELECT * FROM OPENROWSET (BULK 'f1', FORMAT = 'PARQUET', ROWSET_OPTIONS = '{\"READ_OPTIONS\":[\"ALLOW_INCONSISTENT_READS\"]}')) AS a;", new ParserErrorInfo(31, "SQL46142"));
            ParserTestUtils.ErrorTest150("SELECT * FROM OPENROWSET (BULK 'f1', FORMAT = 'PARQUET', ROWSET_OPTIONS = '')) AS a;", new ParserErrorInfo(74, "SQL46010", "''"));
            ParserTestUtils.ErrorTest150("SELECT * FROM OPENROWSET (BULK 'f1', FORMAT = 'PARQUET', ROWSET_OPTIONS = 'READ_OPTIONS = ALLOW_INCONSISTENT_READS')) AS a;", new ParserErrorInfo(74, "SQL46010", "'READ_OPTIONS = ALLOW_INCONSISTENT_READS'"));
            ParserTestUtils.ErrorTest150("SELECT * FROM OPENROWSET (BULK 'f1', FORMAT = 'PARQUET', ROWSET_OPTIONS = '\"READ_OPTIONS\" : allow_INCONSISTENT_READS')) AS x;", new ParserErrorInfo(74, "SQL46010", "'\"READ_OPTIONS\" : allow_INCONSISTENT_READS'"));
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void GraphSyntaxNegativeTest()
        {
            ParserTestUtils.ErrorTest140("CREATE TABLE T1 AS NODE", new ParserErrorInfo(19, "SQL46005", "EDGE", "NODE"));
            ParserTestUtils.ErrorTest140("CREATE TABLE T1 (C INT) AS NODE AS EDGE", new ParserErrorInfo(32, "SQL46010", "AS"));
            ParserTestUtils.ErrorTest140("CREATE TABLE T1 AS EDGE AS FILETABLE", new ParserErrorInfo(24, "SQL46010", "AS"));
            ParserTestUtils.ErrorTest140("CREATE TABLE T1 () AS EDGE", new ParserErrorInfo(16, "SQL46010", "("));

            ParserTestUtils.ErrorTest140("SELECT * FROM NODETABLE WHERE MATCH( A-B->C )", new ParserErrorInfo(39, "SQL46010", "B"));
            ParserTestUtils.ErrorTest140("SELECT * FROM NODETABLE WHERE MATCH( A<-(B)->C )", new ParserErrorInfo(44, "SQL46010", ">"));
            ParserTestUtils.ErrorTest140("SELECT * FROM NODETABLE WHERE MATCH( A-(B)-C )", new ParserErrorInfo(43, "SQL46010", "C"));
            ParserTestUtils.ErrorTest140("SELECT * FROM NODETABLE WHERE MATCH( A-(B)->(C)->D)", new ParserErrorInfo(44, "SQL46010", "("));
            ParserTestUtils.ErrorTest140("SELECT * FROM NODETABLE WHERE MATCH( A-(B)->C OR A<-(B)-C)", new ParserErrorInfo(46, "SQL46010", "OR"));
            ParserTestUtils.ErrorTest140("SELECT * FROM NODETABLE HAVING MATCH( A-(B)->C)", new ParserErrorInfo(31, "SQL46010", "MATCH"));

            ParserTestUtils.ErrorTest140("CREATE INDEX IX1 ON T1 ($foo)", new ParserErrorInfo(24, "SQL46010", "$foo"));
            ParserTestUtils.ErrorTest140("CREATE INDEX IX1 ON T1 ($rowid)", new ParserErrorInfo(24, "SQL46010", "$rowid"));
            ParserTestUtils.ErrorTest140("CREATE INDEX IX1 ON T1 ($identity)", new ParserErrorInfo(24, "SQL46010", "$identity"));
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void ResumableIndexNegativeTest()
        {
            ParserTestUtils.ErrorTest140("ALTER INDEX ind ON t1 REBUILD WITH (RESUMABLED = ON, ONLINE = ON)", new ParserErrorInfo(36, "SQL46010", "RESUMABLED"));
            ParserTestUtils.ErrorTest140("ALTER INDEX idx1 ON t1 REBUILD WITH (online=on, resumable = 5)", new ParserErrorInfo(60, "SQL46010", "5"));
            ParserTestUtils.ErrorTest140("ALTER INDEX ind ON t1 REBUILD WITH (ONLINE = ON, MAX_DURATION = randomstring, RESUMABLE = ON)",
                new ParserErrorInfo(64, "SQL46010", "randomstring"));

            ParserTestUtils.ErrorTest140("ALTER INDEX idx1 ON t1 ABORTZ", new ParserErrorInfo(23, "SQL46010", "ABORTZ"));

            ParserTestUtils.ErrorTest140("ALTER INDEX idx1 ON t1 PAUSEZ", new ParserErrorInfo(23, "SQL46010", "PAUSEZ"));

            ParserTestUtils.ErrorTest140("ALTER INDEX idx1 ON t1 RESUMEZ", new ParserErrorInfo(23, "SQL46010", "RESUMEZ"));
            ParserTestUtils.ErrorTest140("ALTER INDEX ind ON t1 RESUME WITH ( MAXDOP = P )", new ParserErrorInfo(45, "SQL46010", "P"));
            ParserTestUtils.ErrorTest140("ALTER INDEX ind ON t1 RESUME WITH (MAX_DURATION = randomstring, RESUMABLE = ON)",
                new ParserErrorInfo(50, "SQL46010", "randomstring"));

            ParserTestUtils.ErrorTest140("ALTER INDEX idx1 ON t1 RESUME WITH (WAIT_AT_LOW_PRIORITY (MAX_DURATION = -1, ABORT_AFTER_WAIT = NONE))",
                new ParserErrorInfo(73, "SQL46010", "-"));
            ParserTestUtils.ErrorTest140("ALTER INDEX idx1 ON t1 RESUME WITH (WAIT_AT_LOW_PRIORITY (MAX_DURATION = 1, ABORT_AFTER_WAIT = NO))",
                new ParserErrorInfo(95, "SQL46010", "NO"));
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void CatalogCollationNegativeTests()
        {
            // Typo in the keyword
            //
            ParserTestUtils.ErrorTest140("CREATE DATABASE TESTDB WITH CATALOG_COLLATIO = DATABASE_DEFAULT", new ParserErrorInfo(28, "SQL46005", "CATALOG_COLLATION", "CATALOG_COLLATIO"));

            // Typo in the catalog collation option.
            //
            ParserTestUtils.ErrorTest140("CREATE DATABASE TESTDB WITH CATALOG_COLLATION = DATABASE_DEFAULD", new ParserErrorInfo(48, "SQL46010", "DATABASE_DEFAULD"));

            // Valid Collation, invalid CATALOG_COLLATION.
            //
            ParserTestUtils.ErrorTest140("CREATE DATABASE TESTDB WITH CATALOG_COLLATION = SQL_Latin1_General_CP1_CS_AS", new ParserErrorInfo(48, "SQL46010", "SQL_Latin1_General_CP1_CS_AS"));

            // Option out of order.
            //
            ParserTestUtils.ErrorTest140("CREATE DATABASE TESTDB WITH CATALOG_COLLATION = DATABASE_DEFAULT COLLATE SQL_Latin1_General_CP1_CI_AS", new ParserErrorInfo(65, "SQL46010", "COLLATE"));

            // Duplicate option
            //
            ParserTestUtils.ErrorTest140("CREATE DATABASE TESTDB WITH CATALOG_COLLATION = DATABASE_DEFAULT CATALOG_COLLATION = SQL_Latin1_General_CP1_CI_AS", new ParserErrorInfo(65, "SQL46010", "CATALOG_COLLATION"));

            // Incomplete option
            //
            ParserTestUtils.ErrorTest140("CREATE DATABASE TESTDB WITH CATALOG_COLLATION = ", new ParserErrorInfo(48, "SQL46029", "CATALOG_COLLATION"));
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void AutomaticTuningNegativeTests()
        {
            // Typo in the keyword
            //
            ParserTestUtils.ErrorTest140("ALTER DATABASE TESTDB SET AUTOMATIC_TUNIN = INHERIT", new ParserErrorInfo(26, "SQL46010", "AUTOMATIC_TUNIN"));

            // Invalid Automatic Tuning value
            //
            ParserTestUtils.ErrorTest140("ALTER DATABASE TESTDB SET AUTOMATIC_TUNING = ON", new ParserErrorInfo(45, "SQL46010", "ON"));

            // Automatic Tuning value and Automatic Tuning option in one statement
            //
            ParserTestUtils.ErrorTest140("ALTER DATABASE TESTDB SET AUTOMATIC_TUNING = INHERIT (FORCE_LAST_GOOD_PLAN = ON)", new ParserErrorInfo(54, "SQL46010", "FORCE_LAST_GOOD_PLAN"));

            // Bracket is missing
            //
            ParserTestUtils.ErrorTest140("ALTER DATABASE TESTDB SET AUTOMATIC_TUNING (FORCE_LAST_GOOD_PLAN = ON", new ParserErrorInfo(69, "SQL46029", "ON"));

            // Comma is missing
            //
            ParserTestUtils.ErrorTest140("ALTER DATABASE TESTDB SET AUTOMATIC_TUNING (FORCE_LAST_GOOD_PLAN = ON CREATE_INDEX = OFF)", new ParserErrorInfo(70, "SQL46010", "CREATE_INDEX"));

            // Two same regular options inside brackets
            //
            ParserTestUtils.ErrorTest140("ALTER DATABASE TESTDB SET AUTOMATIC_TUNING (FORCE_LAST_GOOD_PLAN = ON, FORCE_LAST_GOOD_PLAN = OFF)", new ParserErrorInfo(71, "SQL46049", "FORCE_LAST_GOOD_PLAN"));
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void ResumableCreateNegativeTests()
        {
            ParserTestUtils.ErrorTest140("CREATE INDEX ind ON t1(c1) WITH (RESUMABLED = ON, ONLINE = ON)", new ParserErrorInfo(33, "SQL46010", "RESUMABLED"));
            ParserTestUtils.ErrorTest140("CREATE INDEX ind ON t1(c1) WITH (online=on, resumable = 5)", new ParserErrorInfo(56, "SQL46010", "5"));
            ParserTestUtils.ErrorTest140("CREATE INDEX ind ON t1(c1) WITH (ONLINE = ON, MAX_DURATION = randomstring, RESUMABLE = ON)",
                new ParserErrorInfo(61, "SQL46010", "randomstring"));
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void ScalarUDFInlineNegativeTests()
        {
            string query1 = @"CREATE OR ALTER FUNCTION [dbo].[test_udf2]
( )
RETURNS INT
WITH ILINE = ON
AS
BEGIN
    DECLARE @v AS INT = 10;
    RETURN @v * @v;
END";
            // Typo in the keyword
            //
            ParserTestUtils.ErrorTest150(query1, new ParserErrorInfo(query1.IndexOf(@"ILINE"), "SQL46010", "ILINE"));

            string query2 = @"CREATE OR ALTER FUNCTION [dbo].[test_udf2]
( )
RETURNS INT
WITH INLINE = ONE
AS
BEGIN
    DECLARE @v AS INT = 10;
    RETURN @v * @v;
END";
            // Typo in the keyword
            //
            ParserTestUtils.ErrorTest150(query2, new ParserErrorInfo(query2.IndexOf(@"ONE"), "SQL46010", "ONE"));

            string query3 = @"CREATE OR ALTER FUNCTION [dbo].[test_udf2]
( )
RETURNS INT
WITH INLINE = OF
AS
BEGIN
    DECLARE @v AS INT = 10;
    RETURN @v * @v;
END";
            // Typo in the keyword
            //
            ParserTestUtils.ErrorTest150(query3, new ParserErrorInfo(query3.IndexOf("OF"), "SQL46010", "OF"));
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void GraphConnectionConstraintNegativeTests()
        {
            // Typo in word CONNECTION.
            //
            ParserTestUtils.ErrorTest150("ALTER TABLE edge add constraint myConstraint CONNECTIN (node3 TO node4)", new ParserErrorInfo(45, "SQL46005", "CONNECTION", "CONNECTIN"));

            // Typo in keyword TO.
            //
            ParserTestUtils.ErrorTest150("ALTER TABLE edge add constraint myConstraint CONNECTION (node3 TOO node4)", new ParserErrorInfo(63, "SQL46010", "TOO"));

            // Incomplete statement.
            //
            ParserTestUtils.ErrorTest150("ALTER TABLE edge add constraint myConstraint CONNECTION (node3 TO node4", new ParserErrorInfo(71, "SQL46029", "node4"));

            // Wrong token at the end. No new list of FromNode to ToNode after comma.
            //
            ParserTestUtils.ErrorTest150("ALTER TABLE edge add constraint myConstraint CONNECTION (node3 TO node4,)", new ParserErrorInfo(72, "SQL46010", ")"));

            // Empty FromNode to ToNode list.
            //
            ParserTestUtils.ErrorTest150("ALTER TABLE edge add constraint myConstraint CONNECTION ()", new ParserErrorInfo(57, "SQL46010", ")"));

            // Wrong syntax due to missing comma.
            //
            ParserTestUtils.ErrorTest150("ALTER TABLE edge add constraint myConstraint CONNECTION (node3 TO node4 node1 TO node2", new ParserErrorInfo(72, "SQL46010", "node1"));
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void GraphMergeNegativeTests()
        {
            string query1 = @"MERGE INTO Owns

USING (Source
       INNER JOIN
       Person
       ON Source.FirstName = Person.FirstName
       INNER JOIN
       Dog
       ON Source.DogName = Dog.DogName) ON MATCH(Person<-(Owns)->Dog)
WHEN NOT MATCHED BY SOURCE THEN DELETE OUTPUT inserted.*, deleted.*;";
            // Typo in the Match Clause.
            //
            ParserTestUtils.ErrorTest150(query1, new ParserErrorInfo(query1.IndexOf(@">Dog"), "SQL46010", ">"));

            string query2 = @"MERGE INTO Owns

USING (Source
       INNER JOIN
       Person
       ON Source.FirstName = Person.FirstName
       INNER JOIN
       Dog
       ON Source.DogName = Dog.DogName) ON MATCH(Person-(Owns)-Dog)
WHEN NOT MATCHED BY SOURCE THEN DELETE OUTPUT inserted.*, deleted.*;";
            // Typo in the Match Clause.
            //
            ParserTestUtils.ErrorTest150(query2, new ParserErrorInfo(query2.IndexOf("-Dog") + 1, "SQL46010", "Dog"));
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void DBScopedConfigClearProcCachePlanHandleNegativeTests()
        {
            // Plan handle passed as a string.
            //
            ParserTestUtils.ErrorTest150("ALTER DATABASE SCOPED CONFIGURATION CLEAR PROCEDURE_CACHE '0x06000600D5629633A06F79AC3302000001000000000000000000000000000000000000000000000000000000';",
                 new ParserErrorInfo(58, "SQL46010", "'0x06000600D5629633A06F79AC3302000001000000000000000000000000000000000000000000000000000000'"));

            // Plan Handle is a not a Hex
            //
            ParserTestUtils.ErrorTest150("ALTER DATABASE SCOPED CONFIGURATION CLEAR PROCEDURE_CACHE 06000600D5629633A06F79AC3302000001000000000000000000000000000000000000000000000000000000;",
                 new ParserErrorInfo(58, "SQL46010", "06000600"));
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void GraphMatchShortestPathSyntaxNegativeTest()
        {
            // Graph Match Shortest Path Syntax tests.
            //
            ParserTestUtils.ErrorTest150("SELECT * FROM NODETABLE WHERE MATCH(SHORTEST_PATH(A(-B->C)+ )", new ParserErrorInfo(53, "SQL46010", "B"));
            ParserTestUtils.ErrorTest150("SELECT * FROM NODETABLE WHERE MATCH(SHORTEST_PATH(A(-(B)->C) )", new ParserErrorInfo(61, "SQL46010", ")"));
            ParserTestUtils.ErrorTest150("SELECT * FROM NODETABLE WHERE MATCH( SHORTEST_PATH((A-(B)->C)+ )", new ParserErrorInfo(59, "SQL46010", "C"));
            ParserTestUtils.ErrorTest150("SELECT * FROM NODETABLE WHERE MATCH( SHRTEST_PATH(A(-(B)->C)+))", new ParserErrorInfo(49, "SQL46010", "("));
            ParserTestUtils.ErrorTest150("SELECT * FROM NODETABLE WHERE MATCH( SHORTEST_PATH((A-(B)->)C)+)", new ParserErrorInfo(60, "SQL46010", "C"));
            ParserTestUtils.ErrorTest150("SELECT * FROM NODETABLE WHERE MATCH( SHORTEST_PATH((A-(B)-)+C))", new ParserErrorInfo(58, "SQL46010", ")"));
            ParserTestUtils.ErrorTest150("SELECT * FROM NODETABLE WHERE MATCH( SHORTEST_PATH((A<-(B)->)+C))", new ParserErrorInfo(59, "SQL46010", ">"));

            // Graph Match Last Node Syntax tests.
            //
            ParserTestUtils.ErrorTest150("SELECT * FROM NODETABLE WHERE MATCH( LAST_NODE(node) = LAST_NOE(node2))", new ParserErrorInfo(55, "SQL46010", "LAST_NOE"));
            ParserTestUtils.ErrorTest150("SELECT * FROM NODETABLE WHERE MATCH( LAST_NODE(node) > LAST_NODE(node2))", new ParserErrorInfo(53, "SQL46010", ">"));
            ParserTestUtils.ErrorTest150("SELECT * FROM NODETABLE WHERE MATCH( LAST_NODE(node) = LAST_NODE{node2})", new ParserErrorInfo(55, "SQL46010", "LAST_NODE"));
            ParserTestUtils.ErrorTest150("SELECT * FROM NODETABLE WHERE MATCH( LAST_NODE(node) = node2)", new ParserErrorInfo(55, "SQL46010", "node2"));

            // Graph Match ShortestPath and LastNode Mix syntax tests.
            //
            ParserTestUtils.ErrorTest150("SELECT * FROM NODETABLE WHERE LAST_NODE( MATCH((A-(B)->)+C))", new ParserErrorInfo(54, "SQL46010", ">"));
            ParserTestUtils.ErrorTest150("SELECT * FROM NODETABLE WHERE MATCH( LAST_NODE(SHORTEST_PATH((A<-(B)->)+C)))", new ParserErrorInfo(60, "SQL46010", "("));
            ParserTestUtils.ErrorTest150("SELECT * FROM NODETABLE WHERE MATCH( SHORTEST_PATH((A-LAST_NODE(B)->)+C))", new ParserErrorInfo(54, "SQL46010", "LAST_NODE"));
            ParserTestUtils.ErrorTest150("SELECT * FROM NODETABLE WHERE MATCH( LAST_NODE())", new ParserErrorInfo(47, "SQL46010", ")"));
            ParserTestUtils.ErrorTest150("SELECT * FROM NODETABLE WHERE MATCH( LAST_NODE(n1))", new ParserErrorInfo(50, "SQL46010", ")"));
            ParserTestUtils.ErrorTest150("SELECT * FROM NODETABLE WHERE MATCH( A-(B)->LAST_NODE())", new ParserErrorInfo(54, "SQL46010", ")"));
            ParserTestUtils.ErrorTest150("SELECT * FROM NODETABLE WHERE MATCH( LAST_NODE(n1)-(LAST_NODE(n1) = LAST_NODE(n2))->LAST_NODE(n3))", new ParserErrorInfo(61, "SQL46010", "("));
            ParserTestUtils.ErrorTest150("SELECT * FROM NODETABLE WHERE MATCH(SHORTEST_PATH(n1(-(e1)->LAST_NODE(n2)-(e2)->n3)+))", new ParserErrorInfo(60, "SQL46010", "LAST_NODE"));
            ParserTestUtils.ErrorTest150("SELECT * FROM NODETABLE WHERE MATCH( LAST_NODE(node) = LAST_NODE(node2) OR SHORTEST_PATH(A(-(B)->C)+))", new ParserErrorInfo(72, "SQL46010", "OR"));

            // Graph Match Shortest Path Quantifier tests.
            //
            ParserTestUtils.ErrorTest150("SELECT * FROM NODETABLE WHERE MATCH(SHORTEST_PATH((A-(B)->){1, -1} C )", new ParserErrorInfo(63, "SQL46010", "-"));
            ParserTestUtils.ErrorTest150("SELECT * FROM NODETABLE WHERE MATCH(SHORTEST_PATH((A-(B)->){-1,  2} C )", new ParserErrorInfo(60, "SQL46010", "-"));
            ParserTestUtils.ErrorTest150("SELECT * FROM NODETABLE WHERE MATCH(SHORTEST_PATH((A-(B)->)[1, 2} C )", new ParserErrorInfo(59, "SQL46031", "[1, 2} C )"));
            ParserTestUtils.ErrorTest150("SELECT * FROM NODETABLE WHERE MATCH(SHORTEST_PATH((A-(B)->){1, 3] C )", new ParserErrorInfo(64, "SQL46010", "]"));

            // Graph 'FOR PATH' tests.
            //
            ParserTestUtils.ErrorTest150("SELECT * FROM node1, node2 FOR PAT as foo", new ParserErrorInfo(31, "SQL46010", "PAT"));
            ParserTestUtils.ErrorTest150("SELECT * FROM node1, node2 FORPATH as foo", new ParserErrorInfo(27, "SQL46010", "FORPATH"));

            // Graph 'GRAPH PATH' tests.
            //
            ParserTestUtils.ErrorTest150("SELECT MIN(node2.column1) WITHIN GROUP (GRAH PATH) FROM NODETABLE", new ParserErrorInfo(40, "SQL46010", "GRAH"));
            ParserTestUtils.ErrorTest150("SELECT MAX(node2.column1) WITHIN GROUP (GRAPH PAT) FROM NODETABLE", new ParserErrorInfo(40, "SQL46010", "GRAPH"));
            ParserTestUtils.ErrorTest150("SELECT AVG(node2.column1) WITHIN GROUP (GRAPHPATH) FROM NODETABLE", new ParserErrorInfo(40, "SQL46010", "GRAPHPATH"));
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void CreateExternalLoginSyntaxNegativeTest()
        {
            ParserTestUtils.ErrorTest150("CREATE LOGIN Bane Car FROM EXTERNAL PROVIDER", new ParserErrorInfo(18, "SQL46010", "Car"));
            ParserTestUtils.ErrorTest150("CREATE LOGIN [l1] FROM EXTERNAL PROVIDER WITH TYPE = E", new ParserErrorInfo(23, "SQL46121"));
            ParserTestUtils.ErrorTest150("CREATE LOGIN [l1] FROM EXTERNAL PROVIDER WITH SID = 0x6A8BD717B5ABA147B5D811E3D268FB3D", new ParserErrorInfo(23, "SQL46121"));
            ParserTestUtils.ErrorTest150("CREATE LOGIN [l1] FROM EXTERNAL PROVIDER WITH SID = 0x6A8BD717B5ABA147B5D811E3D268FB3D, TYPE = 'X'", new ParserErrorInfo(95, "SQL46010", "'X'"));
            ParserTestUtils.ErrorTest150("CREATE LOGIN [l1] FROM EXTERNAL PROVIDER WITH TYPE = X, SID = 0x6A8BD717B5ABA147B5D811E3D268FB3D, TYPE = E", new ParserErrorInfo(105, "SQL46049", "E"));
            ParserTestUtils.ErrorTest150("CREATE LOGIN [l1] FROM EXTERNAL PROVIDER WITH PASSWORD='PLACEHOLDER'", new ParserErrorInfo(46, "SQL46120", "PASSWORD"));
            ParserTestUtils.ErrorTest150("CREATE LOGIN [l1] FROM EXTERNAL PROVIDER WITH CHECK_POLICY = ON", new ParserErrorInfo(46, "SQL46120", "CHECK_POLICY"));
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void DataClassificationErrorTest_130()
        {
            DataClassificationErrorTest<TSql130Parser>();
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void DataClassificationErrorTest_140()
        {
            DataClassificationErrorTest<TSql140Parser>();
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void DataClassificationErrorTest_150()
        {
            DataClassificationErrorTest<TSql150Parser>();
        }

        private void DataClassificationErrorTest<T>() where T : TSqlParser
        {
            // Missing columns
            ParserTestUtils.ErrorTest<T>("ADD SENSITIVITY CLASSIFICATION TO WITH (label = 'Highly Confidential', INFORMATION_TYPE = 'Financial');",
                                         new ParserErrorInfo(34, "SQL46010", "WITH"));

            ParserTestUtils.ErrorTest<T>("DROP SENSITIVITY CLASSIFICATION FROM;",
                                         new ParserErrorInfo(36, "SQL46010", ";"));

            // Asterisk columns
            ParserTestUtils.ErrorTest<T>("ADD SENSITIVITY CLASSIFICATION TO T1.* WITH (LABEL = 'Highly Confidential', information_type = 'Financial');",
                                         new ParserErrorInfo(37, "SQL46010", "*"));

            ParserTestUtils.ErrorTest<T>("DROP SENSITIVITY CLASSIFICATION FROM T1.*;",
                                         new ParserErrorInfo(40, "SQL46010", "*"));

            // Table name not provided for columns
            ParserTestUtils.ErrorTest<T>("ADD SENSITIVITY CLASSIFICATION TO C1 WITH (LABEL = 'Highly Confidential', INFORMATION_TYPE = 'Financial');",
                                         new ParserErrorInfo(34, "SQL46016", "C1"));

            ParserTestUtils.ErrorTest<T>("ADD SENSITIVITY CLASSIFICATION TO dbo.T.C1, T.C2, C3 WITH (LABEL = 'Highly Confidential', INFORMATION_TYPE = 'Financial');",
                                         new ParserErrorInfo(50, "SQL46016", "C3"));

            ParserTestUtils.ErrorTest<T>("DROP SENSITIVITY FROM C1;",
                                         new ParserErrorInfo(5, "SQL46010", "SENSITIVITY"));

            ParserTestUtils.ErrorTest<T>("DROP SENSITIVITY FROM dbo.T.C1, T.C1, C2;",
                                         new ParserErrorInfo(5, "SQL46010", "SENSITIVITY"));

            // Missing classified options
            ParserTestUtils.ErrorTest<T>("ADD SENSITIVITY CLASSIFICATION TO T.C1 WITH ();",
                                         new ParserErrorInfo(45, "SQL46010", ")"));

            // No 'CLASSIFICATION'
            ParserTestUtils.ErrorTest<T>("ADD SENSITIVITY TO T.C1 (LABEL = 'Highly Confidential', INFORMATION_TYPE = 'Financial');",
                                         new ParserErrorInfo(4, "SQL46005", "SIGNATURE", "SENSITIVITY"));

            ParserTestUtils.ErrorTest<T>("DROP SENSITIVITY FROM T1.C1;",
                                         new ParserErrorInfo(5, "SQL46010", "SENSITIVITY"));

            // No 'WITH'
            ParserTestUtils.ErrorTest<T>("ADD SENSITIVITY CLASSIFICATION TO T.C1 (LABEL = 'Highly Confidential', INFORMATION_TYPE = 'Financial');",
                                         new ParserErrorInfo(39, "SQL46010", "("));

            // No 'FROM'
            ParserTestUtils.ErrorTest<T>("DROP SENSITIVITY CLASSIFICATION T1.C1;",
                                         new ParserErrorInfo(32, "SQL46010", "T1"));

            // Empty classified option
            ParserTestUtils.ErrorTest<T>("ADD SENSITIVITY CLASSIFICATION TO T.C1 WITH (LABEL = '', INFORMATION_TYPE = 'Financial');",
                                         new ParserErrorInfo(53, "SQL46063", "LABEL"));

            ParserTestUtils.ErrorTest<T>("ADD SENSITIVITY CLASSIFICATION TO T.C1 WITH (LABEL = 'Highly Confidential', INFORMATION_TYPE_ID = '');",
                                         new ParserErrorInfo(98, "SQL46063", "INFORMATION_TYPE_ID"));

            // Duplicated classified option
            ParserTestUtils.ErrorTest<T>("ADD SENSITIVITY CLASSIFICATION TO T.C1 WITH (LABEL = 'Highly Confidential', LABEL = 'Another label', INFORMATION_TYPE = 'Financial');",
                                         new ParserErrorInfo(76, "SQL46049", "LABEL"));

            ParserTestUtils.ErrorTest<T>("ADD SENSITIVITY CLASSIFICATION TO T.C1 WITH (LABEL = 'Highly Confidential', INFORMATION_TYPE_ID = '643f7acd-776a-438d-890c-79c3f2a520d6', INFORMATION_TYPE_ID = '643f7acd-776a-438d-890c-79c3f2a520d6');",
                                         new ParserErrorInfo(138, "SQL46049", "INFORMATION_TYPE_ID"));

            // Label value provided as identifier
            ParserTestUtils.ErrorTest<T>("ADD SENSITIVITY CLASSIFICATION TO T.C1 WITH (LABEL = Test, INFORMATION_TYPE_ID = '643f7acd-776a-438d-890c-79c3f2a520d6');",
                                         new ParserErrorInfo(45, "SQL46123", "LABEL"));

            // Illegal sensitivity option
            ParserTestUtils.ErrorTest<T>("ADD SENSITIVITY CLASSIFICATION TO T.C1 WITH (LABEL2 = 'Test');",
                                         new ParserErrorInfo(45, "SQL46125", "LABEL2"));

            ParserTestUtils.ErrorTest<T>("ADD SENSITIVITY CLASSIFICATION TO T.C1 WITH (RANK2 = Low);",
                                         new ParserErrorInfo(45, "SQL46125", "RANK2"));

            // Duplicated rank option
            ParserTestUtils.ErrorTest<T>("ADD SENSITIVITY CLASSIFICATION TO T.C1 WITH (LABEL = 'Highly Confidential', INFORMATION_TYPE_ID = '643f7acd-776a-438d-890c-79c3f2a520d6', RANK=Low, RANK=High);",
                                         new ParserErrorInfo(148, "SQL46049", "RANK"));

            // Rank value provided as text
            ParserTestUtils.ErrorTest<T>("ADD SENSITIVITY CLASSIFICATION TO T.C1 WITH (LABEL_ID = '643f7acd-776a-438d-890c-79c3f2a520d6', INFORMATION_TYPE = 'Financial', RANK='Low');",
                                         new ParserErrorInfo(128, "SQL46122"));

            // Illegal rank value
            ParserTestUtils.ErrorTest<T>("ADD SENSITIVITY CLASSIFICATION TO T.C1 WITH (LABEL = 'Highly Confidential', INFORMATION_TYPE_ID = '643f7acd-776a-438d-890c-79c3f2a520d6', RANK = Test);",
                                         new ParserErrorInfo(145, "SQL46124"));

            // Illegal rank value - underlying enum integer
            ParserTestUtils.ErrorTest<T>("ADD SENSITIVITY CLASSIFICATION TO T.C1 WITH (LABEL_ID = '643f7acd-776a-438d-890c-79c3f2a520d6', INFORMATION_TYPE = 'Financial', RANK = 20);",
                                         new ParserErrorInfo(135, "SQL46010", "20"));
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void StringAggErrorTest()
        {
            var invalidStringAgg = "SELECT STRING_AGG(col2,)";
            var invalidStringAgg1 = "SELECT STRING_AGG(col3, N',') WITHIN (ORDER BY col4, col5)";
            var invalidStringAgg2 = "SELECT STRING_AGG(col3, N',') WITHIN GROUP (ORDER BY)";
            var invalidStringAgg3 = "CREATE PROCEDURE sp1 AS BEGIN SELECT STRING_AGG(col3, N',') GROUP (ORDER BY col4, col5) END";

            ParserTestUtils.ErrorTest130(invalidStringAgg, new ParserErrorInfo(23, "SQL46010", ")"));
            ParserTestUtils.ErrorTest140(invalidStringAgg, new ParserErrorInfo(23, "SQL46010", ")"));
            ParserTestUtils.ErrorTest150(invalidStringAgg, new ParserErrorInfo(23, "SQL46010", ")"));

            ParserTestUtils.ErrorTest130(invalidStringAgg1, new ParserErrorInfo(37, "SQL46010", "("));
            ParserTestUtils.ErrorTest140(invalidStringAgg1, new ParserErrorInfo(37, "SQL46010", "("));
            ParserTestUtils.ErrorTest150(invalidStringAgg1, new ParserErrorInfo(37, "SQL46010", "("));

            ParserTestUtils.ErrorTest130(invalidStringAgg2, new ParserErrorInfo(52, "SQL46010", ")"));
            ParserTestUtils.ErrorTest140(invalidStringAgg2, new ParserErrorInfo(52, "SQL46010", ")"));
            ParserTestUtils.ErrorTest150(invalidStringAgg2, new ParserErrorInfo(52, "SQL46010", ")"));

            ParserTestUtils.ErrorTest130(invalidStringAgg3, new ParserErrorInfo(66, "SQL46010", "("));
            ParserTestUtils.ErrorTest140(invalidStringAgg3, new ParserErrorInfo(66, "SQL46010", "("));
            ParserTestUtils.ErrorTest150(invalidStringAgg3, new ParserErrorInfo(66, "SQL46010", "("));
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void CreateMaterializedViewNegativeTest()
        {
            var invalidMaterializedView = "CREATE MATERIALIZED View1 WITH (DISTRIBUTION = HASH(Col5), FOR_APPEND) AS SELECT Col1, Col2 FROM dbo.Table1 GROUP BY Col1, Col2";
            var invalidMaterializedView1 = "CREATE VIEW View1 WITH (DISTRIBUTION = HASH(Col5), FOR_APPEND) AS SELECT Col1, Col2 FROM dbo.Table1 GROUP BY Col1, Col2";
            var invalidMaterializedView2 = "CREATE MATERIALIZED VIEW View1 WITH DISTRIBUTION = HASH(Col5), FOR_APPEND AS SELECT Col1, Col2 FROM dbo.Table1 GROUP BY Col1, Col2";
            var invalidMaterializedView3 = "CREATE MATERIALIZED VIEW View1 WITH (DISTRIBUTION = HASH(Col5) FOR_APPEND) AS SELECT Col1, Col2 FROM dbo.Table1 GROUP BY Col1, Col2";
            var invalidMaterializedView4 = "CREATE MATERIALIZED VIEW View1 WITH (DISTRIBUTION = HASH(Col5)), FOR_APPEND AS SELECT Col1, Col2 FROM dbo.Table1 GROUP BY Col1, Col2";
            var invalidMaterializedView5 = "CREATE MATERIALIZED VIEW View1 WITH (DISTRIBUTION = REPLICATE,) AS SELECT Col1, Col2 FROM dbo.Table1 GROUP BY Col1, Col2";
            var invalidMaterializedView6 = "CREATE MATERIALIZED VIEW View1 WITH (FOR_APPEND, FOR_APPEND) AS SELECT Col1, Col2 FROM dbo.Table1 GROUP BY Col1, Col2";
            var invalidMaterializedView7 = "CREATE MATERIALIZED VIEW View1 WITH (DISTRIBUTION = HASH(), FOR_APPEND) AS SELECT Col1, Col2 FROM dbo.Table1 GROUP BY Col1, Col2";
            var invalidMaterializedView8 = "CREATE MATERIALIZED VIEW View1 WITH (DISTRIBUTION = ROUND_ROBIN) AS SELECT Col1, Col2 FROM";
            var invalidMaterializedView9 = "CREATE MATERIALIZED VIEW View1 WITH FOR_APPEND AS SELECT Col1, Col2 FROM dbo.Table1 GROUP BY Col1, Col2";
            var invalidMaterializedView10 = "CREATE MATERIALIZED VIEW View1 WITH (DISTRIBUTION = REPLICATE) AS SELECT Col1, Col2 FROM dbo.Table1 GROUP BY Col1, Col2";
            var invalidMaterializedView11 = "CREATE MATERIALIZED VIEW View1 (DISTRIBUTION = HASH(Col5), FOR_APPEND) AS SELECT Col1, Col2 FROM dbo.Table1 GROUP BY Col1, Col2";
            var invalidMaterializedView12 = "CREATE MATERIALIZED VIEW View1 WITH (DISTRIBTION = HASH(Col5)) AS SELECT Col1, Col2 FROM dbo.Table1 GROUP BY Col1, Col2";
            var invalidMaterializedView13 = "CREATE MATERIALIZED VIEW View1 WITH (DISTRIBUTION = HASH(Col5), FOR_APPND) AS SELECT Col1, Col2 FROM dbo.Table1 GROUP BY Col1, Col2";
            var invalidMaterializedView14 = "CREATE MATERIALIZED VIEW View1 WITH (DISTRIBUTION = HSH(Col5), FOR_APPEND) AS SELECT Col1, Col2 FROM dbo.Table1 GROUP BY Col1, Col2";
            var invalidMaterializedView15 = "CREATE MATERIALIZED VIEW View1 WITH (DISTRIBUTION = HASH(Col4), DISTRIBUTION = HASH(Col5)) AS SELECT Col1, Col2 FROM dbo.Table1 GROUP BY Col1, Col2";
            var invalidMaterializedView16 = "CREATE MATERIALIZED VIEW View1 WITH (DISTRIBUTION = HASH(Col1), ENCRYPTION) AS SELECT Col1, Col2 FROM dbo.Table1 GROUP BY Col1, Col2";
            var invalidMaterializedView17 = "CREATE MATERIALIZED VIEW View1 WITH (DISTRIBUTION = HASH(Col3, Col4,, Col5)) AS SELECT Col3, Col4, Col5 FROM dbo.Table1 GROUP BY Col3, Col4, Col5;";

            ParserTestUtils.ErrorTest130(invalidMaterializedView, new ParserErrorInfo(7, "SQL46010", "MATERIALIZED"));
            ParserTestUtils.ErrorTest140(invalidMaterializedView, new ParserErrorInfo(7, "SQL46010", "MATERIALIZED"));
            ParserTestUtils.ErrorTest150(invalidMaterializedView, new ParserErrorInfo(7, "SQL46010", "MATERIALIZED"));

            ParserTestUtils.ErrorTest130(invalidMaterializedView1, new ParserErrorInfo(23, "SQL46010", "("));
            ParserTestUtils.ErrorTest140(invalidMaterializedView1, new ParserErrorInfo(23, "SQL46010", "("));
            ParserTestUtils.ErrorTest150(invalidMaterializedView1, new ParserErrorInfo(23, "SQL46010", "("));

            ParserTestUtils.ErrorTest130(invalidMaterializedView2, new ParserErrorInfo(36, "SQL46010", "DISTRIBUTION"));
            ParserTestUtils.ErrorTest140(invalidMaterializedView2, new ParserErrorInfo(36, "SQL46010", "DISTRIBUTION"));
            ParserTestUtils.ErrorTest150(invalidMaterializedView2, new ParserErrorInfo(36, "SQL46010", "DISTRIBUTION"));

            ParserTestUtils.ErrorTest130(invalidMaterializedView3, new ParserErrorInfo(63, "SQL46010", "FOR_APPEND"));
            ParserTestUtils.ErrorTest140(invalidMaterializedView3, new ParserErrorInfo(63, "SQL46010", "FOR_APPEND"));
            ParserTestUtils.ErrorTest150(invalidMaterializedView3, new ParserErrorInfo(63, "SQL46010", "FOR_APPEND"));

            ParserTestUtils.ErrorTest130(invalidMaterializedView4, new ParserErrorInfo(63, "SQL46010", ","));
            ParserTestUtils.ErrorTest140(invalidMaterializedView4, new ParserErrorInfo(63, "SQL46010", ","));
            ParserTestUtils.ErrorTest150(invalidMaterializedView4, new ParserErrorInfo(63, "SQL46010", ","));

            ParserTestUtils.ErrorTest130(invalidMaterializedView5, new ParserErrorInfo(52, "SQL46010", "REPLICATE"));
            ParserTestUtils.ErrorTest140(invalidMaterializedView5, new ParserErrorInfo(52, "SQL46010", "REPLICATE"));
            ParserTestUtils.ErrorTest150(invalidMaterializedView5, new ParserErrorInfo(52, "SQL46010", "REPLICATE"));

            ParserTestUtils.ErrorTest130(invalidMaterializedView6, new ParserErrorInfo(49, "SQL46049", "FOR_APPEND"));
            ParserTestUtils.ErrorTest140(invalidMaterializedView6, new ParserErrorInfo(49, "SQL46049", "FOR_APPEND"));
            ParserTestUtils.ErrorTest150(invalidMaterializedView6, new ParserErrorInfo(49, "SQL46049", "FOR_APPEND"));

            ParserTestUtils.ErrorTest130(invalidMaterializedView7, new ParserErrorInfo(57, "SQL46010", ")"));
            ParserTestUtils.ErrorTest140(invalidMaterializedView7, new ParserErrorInfo(57, "SQL46010", ")"));
            ParserTestUtils.ErrorTest150(invalidMaterializedView7, new ParserErrorInfo(57, "SQL46010", ")"));

            ParserTestUtils.ErrorTest130(invalidMaterializedView8, new ParserErrorInfo(90, "SQL46029", ""));
            ParserTestUtils.ErrorTest140(invalidMaterializedView8, new ParserErrorInfo(90, "SQL46029", ""));
            ParserTestUtils.ErrorTest150(invalidMaterializedView8, new ParserErrorInfo(90, "SQL46029", ""));

            ParserTestUtils.ErrorTest130(invalidMaterializedView9, new ParserErrorInfo(36, "SQL46010", "FOR_APPEND"));
            ParserTestUtils.ErrorTest140(invalidMaterializedView9, new ParserErrorInfo(36, "SQL46010", "FOR_APPEND"));
            ParserTestUtils.ErrorTest150(invalidMaterializedView9, new ParserErrorInfo(36, "SQL46010", "FOR_APPEND"));

            ParserTestUtils.ErrorTest130(invalidMaterializedView9, new ParserErrorInfo(36, "SQL46010", "FOR_APPEND"));
            ParserTestUtils.ErrorTest140(invalidMaterializedView9, new ParserErrorInfo(36, "SQL46010", "FOR_APPEND"));
            ParserTestUtils.ErrorTest150(invalidMaterializedView9, new ParserErrorInfo(36, "SQL46010", "FOR_APPEND"));

            ParserTestUtils.ErrorTest130(invalidMaterializedView10, new ParserErrorInfo(52, "SQL46010", "REPLICATE"));
            ParserTestUtils.ErrorTest140(invalidMaterializedView10, new ParserErrorInfo(52, "SQL46010", "REPLICATE"));
            ParserTestUtils.ErrorTest150(invalidMaterializedView10, new ParserErrorInfo(52, "SQL46010", "REPLICATE"));

            ParserTestUtils.ErrorTest130(invalidMaterializedView11, new ParserErrorInfo(31, "SQL46010", "("));
            ParserTestUtils.ErrorTest140(invalidMaterializedView11, new ParserErrorInfo(31, "SQL46010", "("));
            ParserTestUtils.ErrorTest150(invalidMaterializedView11, new ParserErrorInfo(31, "SQL46010", "("));

            ParserTestUtils.ErrorTest130(invalidMaterializedView12, new ParserErrorInfo(37, "SQL46010", "DISTRIBTION"));
            ParserTestUtils.ErrorTest140(invalidMaterializedView12, new ParserErrorInfo(37, "SQL46010", "DISTRIBTION"));
            ParserTestUtils.ErrorTest150(invalidMaterializedView12, new ParserErrorInfo(37, "SQL46010", "DISTRIBTION"));

            ParserTestUtils.ErrorTest130(invalidMaterializedView13, new ParserErrorInfo(64, "SQL46010", "FOR_APPND"));
            ParserTestUtils.ErrorTest140(invalidMaterializedView13, new ParserErrorInfo(64, "SQL46010", "FOR_APPND"));
            ParserTestUtils.ErrorTest150(invalidMaterializedView13, new ParserErrorInfo(64, "SQL46010", "FOR_APPND"));

            ParserTestUtils.ErrorTest130(invalidMaterializedView14, new ParserErrorInfo(52, "SQL46010", "HSH"));
            ParserTestUtils.ErrorTest140(invalidMaterializedView14, new ParserErrorInfo(52, "SQL46010", "HSH"));
            ParserTestUtils.ErrorTest150(invalidMaterializedView14, new ParserErrorInfo(52, "SQL46010", "HSH"));

            ParserTestUtils.ErrorTest130(invalidMaterializedView15, new ParserErrorInfo(64, "SQL46049", "DISTRIBUTION"));
            ParserTestUtils.ErrorTest140(invalidMaterializedView15, new ParserErrorInfo(64, "SQL46049", "DISTRIBUTION"));
            ParserTestUtils.ErrorTest150(invalidMaterializedView15, new ParserErrorInfo(64, "SQL46049", "DISTRIBUTION"));

            ParserTestUtils.ErrorTest130(invalidMaterializedView16, new ParserErrorInfo(64, "SQL46010", "ENCRYPTION"));
            ParserTestUtils.ErrorTest140(invalidMaterializedView16, new ParserErrorInfo(64, "SQL46010", "ENCRYPTION"));
            ParserTestUtils.ErrorTest150(invalidMaterializedView16, new ParserErrorInfo(64, "SQL46010", "ENCRYPTION"));

            ParserTestUtils.ErrorTest130(invalidMaterializedView17, new ParserErrorInfo(68, "SQL46010", ","));
            ParserTestUtils.ErrorTest140(invalidMaterializedView17, new ParserErrorInfo(68, "SQL46010", ","));
            ParserTestUtils.ErrorTest150(invalidMaterializedView17, new ParserErrorInfo(68, "SQL46010", ","));
            ParserTestUtils.ErrorTest160(invalidMaterializedView17, new ParserErrorInfo(68, "SQL46010", ","));
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void AlterMaterializedViewNegativeTest()
        {
            var invalidMaterializedView = "ALTER MATERIALIZED VIEW";
            var invalidMaterializedView1 = "ALTER MATERIALIZED VIEW View1";
            var invalidMaterializedView2 = "ALTER MATERIALIZED VIEW View1 DISABLED";
            var invalidMaterializedView3 = "ALTER MATERIALIZED VIEW View1 DISABLE,REBUILD";
            var invalidMaterializedView4 = "ALTER MATERIALIZED VIEW View1 DISABLE REBUILD";
            var invalidMaterializedView5 = "ALTER MATERIALIZED VIEW View1 WITH (DISTRIBUTION = REPLICATE) AS SELECT Col1, Col2 FROM dbo.Table1 GROUP BY Col1, Col2";
            var invalidMaterializedView6 = "ALTER MATERIALIZED VIEW View1 AS SELECT Col1, Col2 FROM dbo.Table1 GROUP BY Col1, Col2";
            var invalidMaterializedView7 = "ALTER MATERIALIZED VIEW View1 ENCRYPTION";

            ParserTestUtils.ErrorTest130(invalidMaterializedView, new ParserErrorInfo(23, "SQL46029", ""));
            ParserTestUtils.ErrorTest140(invalidMaterializedView, new ParserErrorInfo(23, "SQL46029", ""));
            ParserTestUtils.ErrorTest150(invalidMaterializedView, new ParserErrorInfo(23, "SQL46029", ""));

            ParserTestUtils.ErrorTest130(invalidMaterializedView1, new ParserErrorInfo(29, "SQL46029", ""));
            ParserTestUtils.ErrorTest140(invalidMaterializedView1, new ParserErrorInfo(29, "SQL46029", ""));
            ParserTestUtils.ErrorTest150(invalidMaterializedView1, new ParserErrorInfo(29, "SQL46029", ""));

            ParserTestUtils.ErrorTest130(invalidMaterializedView2, new ParserErrorInfo(30, "SQL46010", "DISABLED"));
            ParserTestUtils.ErrorTest140(invalidMaterializedView2, new ParserErrorInfo(30, "SQL46010", "DISABLED"));
            ParserTestUtils.ErrorTest150(invalidMaterializedView2, new ParserErrorInfo(30, "SQL46010", "DISABLED"));

            ParserTestUtils.ErrorTest130(invalidMaterializedView3, new ParserErrorInfo(30, "SQL46010", "DISABLE"));
            ParserTestUtils.ErrorTest140(invalidMaterializedView3, new ParserErrorInfo(30, "SQL46010", "DISABLE"));
            ParserTestUtils.ErrorTest150(invalidMaterializedView3, new ParserErrorInfo(30, "SQL46010", "DISABLE"));

            ParserTestUtils.ErrorTest130(invalidMaterializedView4, new ParserErrorInfo(38, "SQL46010", "REBUILD"));
            ParserTestUtils.ErrorTest140(invalidMaterializedView4, new ParserErrorInfo(38, "SQL46010", "REBUILD"));
            ParserTestUtils.ErrorTest150(invalidMaterializedView4, new ParserErrorInfo(38, "SQL46010", "REBUILD"));

            ParserTestUtils.ErrorTest130(invalidMaterializedView5, new ParserErrorInfo(30, "SQL46010", "WITH"));
            ParserTestUtils.ErrorTest140(invalidMaterializedView5, new ParserErrorInfo(30, "SQL46010", "WITH"));
            ParserTestUtils.ErrorTest150(invalidMaterializedView5, new ParserErrorInfo(30, "SQL46010", "WITH"));

            ParserTestUtils.ErrorTest130(invalidMaterializedView6, new ParserErrorInfo(30, "SQL46010", "AS"));
            ParserTestUtils.ErrorTest140(invalidMaterializedView6, new ParserErrorInfo(30, "SQL46010", "AS"));
            ParserTestUtils.ErrorTest150(invalidMaterializedView6, new ParserErrorInfo(30, "SQL46010", "AS"));

            ParserTestUtils.ErrorTest130(invalidMaterializedView7, new ParserErrorInfo(30, "SQL46010", "ENCRYPTION"));
            ParserTestUtils.ErrorTest140(invalidMaterializedView7, new ParserErrorInfo(30, "SQL46010", "ENCRYPTION"));
            ParserTestUtils.ErrorTest150(invalidMaterializedView7, new ParserErrorInfo(30, "SQL46010", "ENCRYPTION"));
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void CopyCommandInvalidSyntaxTest()
        {
            var invalidFromSyntax = @"COPY INTO FactOnlineSales FROM ''
                                    WITH(
                                       FIELDTERMINATOR = '|',
                                       DATEFORMAT = 'ymd',
                                       ROWTERMINATOR = '0x0A'
                                    )";

            var invalidToSyntax = @"COPY INTO FROM 'https://loadingtest.blob.core.windows.net/customerdatasets/sastest/test_1.txt'
                                    WITH(
                                        FIELDTERMINATOR = '|',
                                        DATEFORMAT = 'ymd',
                                        ROWTERMINATOR = '0x0A'
                                    )";

            var invalidOptionSyntax = @"COPY INTO FactOnlineSales FROM 'https://loadingtest.blob.core.windows.net/customerdatasets/sastest/test_1.txt'
                                        WITH(
                                           FIELDTERMINATOR,
                                           DATEFORMAT = 'ymd',
                                           ROWTERMINATOR = '0x0A'
                                        )";

            var invalidIdentityOptionSyntax = @"COPY INTO FactOnlineSales
                                                FROM 'https://loadingtest.blob.core.windows.net/customerdatasets/sastest/test_2.txt'
                                                WITH(
                                                    FILE_TYPE = 'CSV',
                                                    IDENTITY_INSERT = 'TRUE',
                                                    CREDENTIAL = (IDENTITY = 'Shared Access Signature', SECRET = '<Your_SAS_Token>'),
                                                    FIELDQUOTE = '',
                                                    FIRSTROW = 2,
                                                    FIELDTERMINATOR = ';',
                                                    ROWTERMINATOR = '0X0A',
                                                    ENCODING = 'UTF8',
                                                    DATEFORMAT = 'ymd',
                                                    MAXERRORS = 10,
                                                    ERRORFILE = '/sastest/errorsfolder/'
                                                    )";

            var invalidOptions1Syntax = @"COPY INTO FactOnlineSales
                                        FROM 'https://loadingtest.blob.core.windows.net/customerdatasets/sastest/test_2.txt'
                                        WITH(
                                            FILE_TYPE = 'CSV',
                                            IDENTITY_INSERT = 'ON',
                                            CREDENTIAL = (IDENTITY = 'Shared Access Signature', SECRET = '<Your_SAS_Token>'),
                                            FIELDQUOTE = ';',
                                            FIRSTROW = 3,
                                            FIELDTERMINATOR = ';',
                                            ROWTERMINATOR = '0X0A',
                                            ENCODING = 'UTF66',
                                            DATEFORMAT = 'ymdd',
                                            MAXERRORS = 10,
                                            ERRORFILE = '/sastest/errorsfolder/'
                                            )";

            var invalidOptions2Syntax = @"COPY INTO FactOnlineSales
                                        FROM 'https://loadingtest.blob.core.windows.net/customerdatasets/sastest/test_2.txt'
                                        WITH(
                                            FILE_TYPE = 'CSV',
                                            DATEFORMAT = 'ymdd',
                                            MAXERRORS = 10,
                                            ERRORFILE = '/sastest/errorsfolder/'
                                            )";

            var invalidOptions3Syntax = @"COPY INTO FactOnlineSales
                                        FROM 'https://loadingtest.blob.core.windows.net/customerdatasets/sastest/test_2.txt'
                                        WITH(
                                            COMPRESSION = '',
                                            DATEFORMAT = 'ymdd',
                                            MAXERRORS = 10,
                                            ERRORFILE = '/sastest/errorsfolder/'
                                            )";
            ParserTestUtils.ErrorTest130(invalidFromSyntax, new ParserErrorInfo(31, "SQL46063", "FROM"));
            ParserTestUtils.ErrorTest140(invalidFromSyntax, new ParserErrorInfo(31, "SQL46063", "FROM"));
            ParserTestUtils.ErrorTest150(invalidFromSyntax, new ParserErrorInfo(31, "SQL46063", "FROM"));

            ParserTestUtils.ErrorTest130(invalidToSyntax, new ParserErrorInfo(10, "SQL46010", "FROM"));
            ParserTestUtils.ErrorTest140(invalidToSyntax, new ParserErrorInfo(10, "SQL46010", "FROM"));
            ParserTestUtils.ErrorTest150(invalidToSyntax, new ParserErrorInfo(10, "SQL46010", "FROM"));

            ParserTestUtils.ErrorTest130(invalidOptionSyntax, new ParserErrorInfo(invalidOptionSyntax.IndexOf(@"FIELDTERMINATOR,") + 15, "SQL46010", ","));
            ParserTestUtils.ErrorTest140(invalidOptionSyntax, new ParserErrorInfo(invalidOptionSyntax.IndexOf(@"FIELDTERMINATOR,") + 15, "SQL46010", ","));
            ParserTestUtils.ErrorTest150(invalidOptionSyntax, new ParserErrorInfo(invalidOptionSyntax.IndexOf(@"FIELDTERMINATOR,") + 15, "SQL46010", ","));

            ParserTestUtils.ErrorTest130(invalidIdentityOptionSyntax, new ParserErrorInfo(1,
                "SQL46130", "IDENTITY_INSERT"));
            ParserTestUtils.ErrorTest140(invalidIdentityOptionSyntax, new ParserErrorInfo(1,
                "SQL46130", "IDENTITY_INSERT"));
            ParserTestUtils.ErrorTest150(invalidIdentityOptionSyntax, new ParserErrorInfo(1,
                "SQL46130", "IDENTITY_INSERT"));

            ParserTestUtils.ErrorTest130(invalidOptions1Syntax, new ParserErrorInfo(1, "SQL46130", "ENCODING"));
            ParserTestUtils.ErrorTest140(invalidOptions1Syntax, new ParserErrorInfo(1, "SQL46130", "ENCODING"));
            ParserTestUtils.ErrorTest150(invalidOptions1Syntax, new ParserErrorInfo(1, "SQL46130", "ENCODING"));

            ParserTestUtils.ErrorTest130(invalidOptions2Syntax, new ParserErrorInfo(1, "SQL46130", "DATEFORMAT"));
            ParserTestUtils.ErrorTest140(invalidOptions2Syntax, new ParserErrorInfo(1, "SQL46130", "DATEFORMAT"));
            ParserTestUtils.ErrorTest150(invalidOptions2Syntax, new ParserErrorInfo(1, "SQL46130", "DATEFORMAT"));

            ParserTestUtils.ErrorTest130(invalidOptions3Syntax, new ParserErrorInfo(1, "SQL46131", "COMPRESSION"));
            ParserTestUtils.ErrorTest140(invalidOptions3Syntax, new ParserErrorInfo(1, "SQL46131", "COMPRESSION"));
            ParserTestUtils.ErrorTest150(invalidOptions3Syntax, new ParserErrorInfo(1, "SQL46131", "COMPRESSION"));
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void CreateExternalLibraryNegativeTest()
        {
            // Library keyword typos
            ParserTestUtils.ErrorTest140("CREAT EXTERNAL LIBRARY xts from (content = 0x504B03040A00000000001815104B00) WITH (LANGUAGE = 'r')", new ParserErrorInfo(0, "SQL46010", "CREAT"));
            ParserTestUtils.ErrorTest140("CREATE EXTENAL LIBRARY xts from (content = 0x504B03040A00000000001815104B00) WITH (LANGUAGE = 'r')", new ParserErrorInfo(7, "SQL46010", "EXTENAL"));
            ParserTestUtils.ErrorTest140("CREATE EXTERNAL LIBARY xts from (content = 0x504B03040A00000000001815104B00) WITH (LANGUAGE = 'r')", new ParserErrorInfo(16, "SQL46010", "LIBARY"));
            ParserTestUtils.ErrorTest140("CREATE EXTERNAL LIBRARY xts fom (content = 0x504B03040A00000000001815104B00) WITH (LANGUAGE = 'r')", new ParserErrorInfo(28, "SQL46010", "fom"));
            ParserTestUtils.ErrorTest140("CREATE EXTERNAL LIBRARY xts from (contnt = 0x504B03040A00000000001815104B00) WITH (LANGUAGE = 'r')", new ParserErrorInfo(34, "SQL46005", "CONTENT", "contnt"));
            ParserTestUtils.ErrorTest140("CREATE EXTERNAL LIBRARY xts from (content = 0x504B03040A00000000001815104B00) WIT (LANGUAGE = 'r')", new ParserErrorInfo(78, "SQL46010", "WIT"));
            ParserTestUtils.ErrorTest140("CREATE EXTERNAL LIBRARY xts from (content = 0x504B03040A00000000001815104B00) WITH (LANGUAG = 'r')", new ParserErrorInfo(84, "SQL46005", "LANGUAGE", "LANGUAG"));

            // Missing library name
            ParserTestUtils.ErrorTest140("CREATE EXTERNAL LIBRARY (content = 0x504B03040A00000000001815104B00) WITH (LANGUAGE = 'r')", new ParserErrorInfo(16, "SQL46010", "LIBRARY"));

            // Empty library name
            ParserTestUtils.ErrorTest140("CREATE EXTERNAL LIBRARY [] (content = 0x504B03040A00000000001815104B00) WITH (LANGUAGE = 'r')", new ParserErrorInfo(25, "SQL46010", "]"));

            // Missing FROM
            ParserTestUtils.ErrorTest140("CREATE EXTERNAL LIBRARY xts (content = 0x504B03040A00000000001815104B00) WITH (LANGUAGE = 'r')", new ParserErrorInfo(28, "SQL46010", "("));

            // Missing CONTENT keyword
            ParserTestUtils.ErrorTest140("CREATE EXTERNAL LIBRARY xts from (0x504B03040A00000000001815104B00) WITH (LANGUAGE = 'r')", new ParserErrorInfo(34, "SQL46010", "0x504B03040A00000000001815104B00"));
            ParserTestUtils.ErrorTest140("CREATE EXTERNAL LIBRARY xts from (0x504B03040A00000000001815104B00)", new ParserErrorInfo(34, "SQL46010", "0x504B03040A00000000001815104B00"));

            // Missing parenthesis around content
            ParserTestUtils.ErrorTest140("CREATE EXTERNAL LIBRARY xts from 0x504B03040A00000000001815104B00 WITH (LANGUAGE = 'r')", new ParserErrorInfo(33, "SQL46010", "0x504B03040A00000000001815104B00"));

            // Missing LANGUAGE part
            ParserTestUtils.ErrorTest140("CREATE EXTERNAL LIBRARY xts from (content=0x504B03040A00000000001815104B00)", new ParserErrorInfo(75, "SQL46029"));

            // Missing missing parenthesis around LANGUAGE
            ParserTestUtils.ErrorTest140("CREATE EXTERNAL LIBRARY xts from (content=0x504B03040A00000000001815104B00) WITH LANGUAGE = 'r'", new ParserErrorInfo(81, "SQL46010", "LANGUAGE"));

            // The language is not a string literal
            ParserTestUtils.ErrorTest140("CREATE EXTERNAL LIBRARY xts from (content=0x504B03040A00000000001815104B00) WITH LANGUAGE = r", new ParserErrorInfo(81, "SQL46010", "LANGUAGE"));

            // The language is an empty string literal
            ParserTestUtils.ErrorTest140("CREATE EXTERNAL LIBRARY xts from (content=0x504B03040A00000000001815104B00) WITH (LANGUAGE = '')", new ParserErrorInfo(93, "SQL46063"));

            // The language is inside the content
            ParserTestUtils.ErrorTest140("CREATE EXTERNAL LIBRARY xts from (content = 0x504B03040A00000000001815104B00, LANGUAGE = 'r')", new ParserErrorInfo(76, "SQL46010", ","));

            // Using PLATFORM in SQL 140 (only available 150+)
            ParserTestUtils.ErrorTest140("CREATE EXTERNAL LIBRARY xts from (content = 0x504B03040A00000000001815104B00, platform = linux) WITH (LANGUAGE = 'r')", new ParserErrorInfo(76, "SQL46010", ","));

            // PLATFORM should not have quotes around it
            ParserTestUtils.ErrorTest150("CREATE EXTERNAL LIBRARY xts from (content = 0x504B03040A00000000001815104B00, platform = 'linux') WITH (LANGUAGE = 'r')", new ParserErrorInfo(89, "SQL46010", "'linux'"));
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void LedgerCreateDatabaseNegativeTests()
        {
            // Typo in the keyword
            //
            ParserTestUtils.ErrorTest150("CREATE DATABASE db WITH LEDGER = OF", new ParserErrorInfo(33, "SQL46010", "OF"));

            // ALTER Database Statement is not supported for Ledger. We allow only for CREATE Database Statement 
            //
            ParserTestUtils.ErrorTest150("ALTER DATABASE dbname SET LEDGER = ON", new ParserErrorInfo(26, "SQL46010", "LEDGER"));
        }

        /// <summary>
        /// Tests OPENROWSET (PROVIDER = 'CosmosDB' ....) - this is specific to Serverless SQL Pools
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void OpenRowset160_CosmosDb()
        {
            // OBJECT parameter missing
            //
            ParserTestUtils.ErrorTest160(
                "SELECT * FROM OPENROWSET(PROVIDER = 'CosmosDB', CONNECTION = 'x', CREDENTIAL = 'y') as      abcd_efgh;",
                new ParserErrorInfo(25, "SQL46144", "Object"));

            // CONNECTION parameter missing
            //
            ParserTestUtils.ErrorTest160(
                "SELECT * FROM OPENROWSET(PROVIDER = 'CosmosDB', CREDENTIAL = 'x', OBJECT = 'y')",
                new ParserErrorInfo(25, "SQL46144", "Connection"));

            // PROVIDER parameter missing
            //
            ParserTestUtils.ErrorTest160(
                "SELECT * FROM OPENROWSET(CONNECTION = 'CosmosDB', CREDENTIAL = 'x', OBJECT = 'y')",
                new ParserErrorInfo(25, "SQL46144", "Provider"));

            // parameters declared more than once
            //
            string multipleParameterDeclarationSynax = @"SELECT * FROM OPENROWSET(PROVIDER = 'CosmosDB',
                    SERVER_CREDENTIAL = 'x',
                    OBJECT = 'y', CONNECTION = 'a', CONNECTION = 'b')
                    with (a varchar(10)) as cols";
            ParserTestUtils.ErrorTest160(multipleParameterDeclarationSynax,
                new ParserErrorInfo(multipleParameterDeclarationSynax.IndexOf(@"CONNECTION = 'a', CONNECTION") + 18, "SQL46049", "CONNECTION"));

            // Not all parameters provided
            //
            ParserTestUtils.ErrorTest160("select * from openrowset  (  provideR = 'CosmosDB') with (a varchar(10)) as cols",
                new ParserErrorInfo(29, "SQL46144", "Connection"));

            // Random strings
            //
            ParserTestUtils.ErrorTest160(
                "SELECT * FROM OPENROWSET(provider = 'a', SOMETHING = 'Wrong') with (a varchar(10)) as cols",
                new ParserErrorInfo(41, "SQL46010", "SOMETHING"));

            ParserTestUtils.ErrorTest160(
                "SELECT * FROM OPENROWSET(PROVIDER = 'CosmosDB', my_parameter = 'my value',  CREDENTIAL = 'abc', OBJECT = 'y', CONNECTION = 'a')",
                new ParserErrorInfo(48, "SQL46010", "my_parameter"));

            ParserTestUtils.ErrorTest160(
                "SELECT * FROM OPENROWSET(PROVIDER = 3, CREDENTIAL = 'abc', OBJECT = 'y', CONNECTION = 'a')",
                new ParserErrorInfo(36, "SQL46010", "3"));

            ParserTestUtils.ErrorTest160(
                "SELECT * FROM OPENROWSET(PROVIDER = '', CREDENTIAL = 'abc', OBJECT = 'y', SERVER_CONNECTION = 'a')",
                new ParserErrorInfo(36, "SQL46063", ""));

            ParserTestUtils.ErrorTest160(
                "SELECT * FROM OPENROWSET(PROVIDER = 'd', CREDENTIAL = '', OBJECT = 'y', SERVER_CONNECTION = 'a')",
                new ParserErrorInfo(54, "SQL46063"));

            ParserTestUtils.ErrorTest160(
                "SELECT * FROM OPENROWSET(PROVIDER = 'dd', CREDENTIAL = 'abc', OBJECT = '', SERVER_CONNECTION = 'a')",
                new ParserErrorInfo(71, "SQL46063"));

            ParserTestUtils.ErrorTest160(
                "SELECT * FROM OPENROWSET(PROVIDER = 'abd def', CREDENTIAL = 'abc', OBJECT = 'y', SERVER_CONNECTION = '')",
                new ParserErrorInfo(101, "SQL46063"));

            ParserTestUtils.ErrorTest160(
                "SELECT * FROM OPENROWSET(PROVIDER = 'abd def', CREDENTIAL = 'abc', OBJECT = 'y', CONNECTION = '')",
                new ParserErrorInfo(94, "SQL46063"));

            ParserTestUtils.ErrorTest160(
                "SELECT * FROM OPENROWSET(PROVIDER = my_cosmos_provider, CREDENTIAL = 'abc', OBJECT = 'y', CONNECTION = 'connection') with (a int, b int) as cs",
                new ParserErrorInfo(36, "SQL46010", "my_cosmos_provider"));

            ParserTestUtils.ErrorTest160(
                "SELECT * FROM OPENROWSET(PROVIDER = '3', CREDENTIAL = , OBJECT = 'y', CONNECTION = 'a')",
                new ParserErrorInfo(54, "SQL46010", ","));

            ParserTestUtils.ErrorTest160(
                "SELECT * FROM OPENROWSET() as x",
                new ParserErrorInfo(25, "SQL46010", ")"));

            // Conflicting parameters
            //
            ParserTestUtils.ErrorTest160(
                "SELECT * FROM OPENROWSET(OBJECT = 'X', PROVIDER = 'CosmosDB', SERVER_CREDENTIAL = 'x',  CREDENTIAL = 'abc', CONNECTION = 'a') with (a varchar(10)) as cols",
                new ParserErrorInfo(25, "SQL46143", "Object"));

            // Invalid syntax around 'SStream' parameter
            //
            ParserTestUtils.ErrorTest160(
                "SELECT TOP 100 UserID FROM OPENROWSET(BULK 'path', FORMAT = ' SStream', PARSER_VERSION = '2.0') AS a;",
                new ParserErrorInfo(60, "SQL46010", "' SStream'"));

            ParserTestUtils.ErrorTest160(
                "SELECT TOP 100 UserID FROM OPENROWSET(BULK 'path', FORMAT = 'SStream ', PARSER_VERSION = '2.0')",
                new ParserErrorInfo(60, "SQL46010", "'SStream '"));

            ParserTestUtils.ErrorTest160(
               "SELECT TOP 100 UserID FROM OPENROWSET(BULK 'path', FORMAT = 'stream', PARSER_VERSION = '1.0')",
               new ParserErrorInfo(60, "SQL46010", "'stream'"));
        }

        /// <summary>
        /// Negative tests for Trim 3 arguments syntax.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void Trim3ArgsNegativeTest()
        {
            ParserTestUtils.ErrorTest160("select TRIM ( INVALIDKW 'X' FROM 'XstringX'))",
                new ParserErrorInfo(24, "SQL46010", "'X'"));
        }

        /// <summary>
        /// Tests OPENROWSET (PROVIDER = 'CosmosDB' ....) - this is specific to Serverless SQL Pools
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void OpenRowset160_WithClause()
        {
            // Empty WITH clause
            //
            string emptyWithClauseSyntax = @"SELECT *
                 FROM OPENROWSET ('a', 'b', [dbo].[tbl])
                 WITH () AS a;";
            ParserTestUtils.ErrorTest160(emptyWithClauseSyntax, new ParserErrorInfo(emptyWithClauseSyntax.IndexOf(@"() AS a") + 1, "SQL46010", ")"));

            // Invalid WITH clause
            //
            string invalidWithClauseSyntax = @"SELECT *
                 FROM OPENROWSET ('a', 'b', c)
                 WITH (A = 5) AS a;";
            ParserTestUtils.ErrorTest160(invalidWithClauseSyntax, new ParserErrorInfo(invalidWithClauseSyntax.IndexOf(@"(A = 5)") + 3, "SQL46010", "="));

            string invalidWithClause2Syntax = @"SELECT *
                 FROM OPENROWSET ('a', 'b', [mytable])
                 WITH (a) AS a;";
            ParserTestUtils.ErrorTest160(invalidWithClause2Syntax, new ParserErrorInfo(invalidWithClause2Syntax.IndexOf(@"(a)") + 2, "SQL46010", ")"));

            string invalidWithClause3Syntax = @"SELECT *
                FROM OPENROWSET ('a', 'b', [ab cd])
                WITH a;";
            ParserTestUtils.ErrorTest160(invalidWithClause3Syntax, new ParserErrorInfo(invalidWithClause3Syntax.IndexOf(@"a;"), "SQL46010", "a"));
        }

        /// <summary>
        /// Negative tests for Scalar Functions in Fabric DW.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void ScalarFunctionNegativeTestsFabricDW()
        {
            string scalarFunctionSyntax = @"CREATE OR ALTER FUNCTION dbo.FormatPrice
                                            (
                                                @amount AS DECIMAL(10,2) = 0.0
                                            )
                                            RETURNS VARCHAR(20)
                                            WITH ENCRYPTION, SCHEMABINDING, RETURNS NULL ON NULL INPUT, INLINE = ON
                                            AS
                                            BEGIN
                                                RETURN '$' + CAST(@amount AS VARCHAR)
                                            END;
                                            ";
            ParserTestUtils.ErrorTestFabricDW(scalarFunctionSyntax, new ParserErrorInfo(scalarFunctionSyntax.IndexOf("ENCRYPTION"), "SQL46026", "ENCRYPTION"));

            string scalarFunctionSyntax2 = @"CREATE OR ALTER FUNCTION dbo.ConcatNames
                                            (
                                                @first AS NVARCHAR(50),
                                                @last  AS NVARCHAR(50)
                                            )
                                            RETURNS NVARCHAR(101)
                                            WITH RETURNS NULL ON NULL INPUT, INLINE = OFF, EXECUTE AS CALLER
                                            AS
                                            BEGIN
                                                RETURN @first + ' ' + @last
                                            END;";
            ParserTestUtils.ErrorTestFabricDW(scalarFunctionSyntax2, new ParserErrorInfo(scalarFunctionSyntax2.IndexOf("INLINE"), "SQL46010", "INLINE"));
            string scalarFunctionSyntax3 = @"CREATE OR ALTER FUNCTION dbo.CountProducts
                                            (
                                                @ProductTable AS dbo.ProductType READONLY
                                            )
                                            RETURNS INT
                                            WITH SCHEMABINDING
                                            AS
                                            BEGIN
                                                RETURN (SELECT COUNT(*) FROM @ProductTable)
                                            END;";
            ParserTestUtils.ErrorTestFabricDW(scalarFunctionSyntax3, new ParserErrorInfo(scalarFunctionSyntax3.IndexOf("READONLY"), "SQL46026", "READONLY"));

            string scalarFunctionSyntax4 = @"CREATE OR ALTER FUNCTION sales.TotalSalesForRegion
                                            (
                                                @RegionId    AS sys.INT NULL,
                                                @SalesData   AS sales.SalesTableType READONLY
                                            )
                                            RETURNS MONEY
                                            WITH RETURNS NULL ON NULL INPUT
                                            AS
                                            BEGIN
                                                RETURN (
                                                    SELECT SUM(Amount)
                                                    FROM @SalesData
                                                    WHERE RegionId = @RegionId
                                                )
                                            END;";
            ParserTestUtils.ErrorTestFabricDW(scalarFunctionSyntax4, new ParserErrorInfo(scalarFunctionSyntax4.IndexOf("NULL"), "SQL46010", "NULL"));
        }

        /// <summary>
        /// Negative tests for Scalar Functions in Fabric DW.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void IdentityColumnNegativeTestsFabricDW()
        {
            string identityColumnSyntax = @"CREATE TABLE TestTable1 (
                                                ID INT IDENTITY(1,1),
                                                Name VARCHAR(50)
                                            );
                                            ";
            ParserTestUtils.ErrorTestFabricDW(identityColumnSyntax, new ParserErrorInfo(identityColumnSyntax.IndexOf("IDENTITY(") + 8, "SQL46010", "("));

            string identityColumnSyntax2 = @"CREATE TABLE TestTable2 (
                                                RecordID BIGINT IDENTITY(100,5),
                                                Description NVARCHAR(200)
                                            );
                                            ";
            ParserTestUtils.ErrorTestFabricDW(identityColumnSyntax2, new ParserErrorInfo(identityColumnSyntax2.IndexOf("IDENTITY(") + 8, "SQL46010", "("));
        }

        /// <summary>
        /// Negative tests for VECTOR INDEX syntax
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void VectorIndexNegativeTests()
        {
            // Missing INDEX keyword
            ParserTestUtils.ErrorTest170("CREATE VECTOR IX_Test ON dbo.Documents (VectorData)",
                new ParserErrorInfo(7, "SQL46010", "VECTOR"));

            // Missing table name
            ParserTestUtils.ErrorTest170("CREATE VECTOR INDEX IX_Test ON (VectorData)",
                new ParserErrorInfo(31, "SQL46010", "("));

            // Missing column specification
            ParserTestUtils.ErrorTest170("CREATE VECTOR INDEX IX_Test ON dbo.Documents",
                new ParserErrorInfo(44, "SQL46029", ""));

            // Empty column list
            ParserTestUtils.ErrorTest170("CREATE VECTOR INDEX IX_Test ON dbo.Documents ()",
                new ParserErrorInfo(46, "SQL46010", ")"));

            // Multiple columns (not supported for vector indexes)
            ParserTestUtils.ErrorTest170("CREATE VECTOR INDEX IX_Test ON dbo.Documents (VectorData, OtherColumn)",
                new ParserErrorInfo(56, "SQL46010", ","));

            // Invalid metric value
            ParserTestUtils.ErrorTest170("CREATE VECTOR INDEX IX_Test ON dbo.Documents (VectorData) WITH (METRIC = 'invalid')",
                new ParserErrorInfo(73, "SQL46010", "'invalid'"));

            // Invalid type value
            ParserTestUtils.ErrorTest170("CREATE VECTOR INDEX IX_Test ON dbo.Documents (VectorData) WITH (TYPE = 'invalid')",
                new ParserErrorInfo(71, "SQL46010", "'invalid'"));

            // Missing option value
            ParserTestUtils.ErrorTest170("CREATE VECTOR INDEX IX_Test ON dbo.Documents (VectorData) WITH (METRIC = )",
                new ParserErrorInfo(73, "SQL46010", ")"));

            // Empty option value
            ParserTestUtils.ErrorTest170("CREATE VECTOR INDEX IX_Test ON dbo.Documents (VectorData) WITH (METRIC = '')",
                new ParserErrorInfo(73, "SQL46010", "''"));

            // Missing WITH keyword when options are present
            ParserTestUtils.ErrorTest170("CREATE VECTOR INDEX IX_Test ON dbo.Documents (VectorData) (METRIC = 'cosine')",
                new ParserErrorInfo(59, "SQL46010", "METRIC"));

            // Missing parentheses around options
            ParserTestUtils.ErrorTest170("CREATE VECTOR INDEX IX_Test ON dbo.Documents (VectorData) WITH METRIC = 'cosine'",
                new ParserErrorInfo(63, "SQL46010", "METRIC"));

            // Invalid option name
            ParserTestUtils.ErrorTest170("CREATE VECTOR INDEX IX_Test ON dbo.Documents (VectorData) WITH (INVALID_OPTION = 'value')",
                new ParserErrorInfo(64, "SQL46010", "INVALID_OPTION"));

            // Metric value without quotes
            ParserTestUtils.ErrorTest170("CREATE VECTOR INDEX IX_Test ON dbo.Documents (VectorData) WITH (METRIC = cosine)",
                new ParserErrorInfo(73, "SQL46010", "cosine"));

            // Type value without quotes
            ParserTestUtils.ErrorTest170("CREATE VECTOR INDEX IX_Test ON dbo.Documents (VectorData) WITH (TYPE = DiskANN)",
                new ParserErrorInfo(71, "SQL46010", "DiskANN"));

            // MAXDOP with invalid value
            ParserTestUtils.ErrorTest170("CREATE VECTOR INDEX IX_Test ON dbo.Documents (VectorData) WITH (MAXDOP = 'invalid')",
                new ParserErrorInfo(73, "SQL46010", "'invalid'"));

            // MAXDOP with negative value
            ParserTestUtils.ErrorTest170("CREATE VECTOR INDEX IX_Test ON dbo.Documents (VectorData) WITH (MAXDOP = -1)",
                new ParserErrorInfo(73, "SQL46010", "-"));

            // Missing equals sign in option
            ParserTestUtils.ErrorTest170("CREATE VECTOR INDEX IX_Test ON dbo.Documents (VectorData) WITH (METRIC 'cosine')",
                new ParserErrorInfo(64, "SQL46010", "METRIC"));

            // Incomplete WITH clause
            ParserTestUtils.ErrorTest170("CREATE VECTOR INDEX IX_Test ON dbo.Documents (VectorData) WITH",
                new ParserErrorInfo(58, "SQL46010", "WITH"));
        }

        /// <summary>
        /// Negative tests for AI_GENERATE_CHUNKS syntax
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void GenerateChunksNegativeTest170()
        {
            // Missing required parameters
            ParserTestUtils.ErrorTest170(
                "SELECT * FROM AI_GENERATE_CHUNKS(source = 'text')",
                new ParserErrorInfo(48, "SQL46010", ")"));

            // Missing CHUNK_SIZE
            ParserTestUtils.ErrorTest170(
                "SELECT * FROM AI_GENERATE_CHUNKS(source = 'text', CHUNK_TYPE = fixed)",
                new ParserErrorInfo(68, "SQL46010", ")"));

            // Invalid order: CHUNK_SIZE before CHUNK_TYPE
            ParserTestUtils.ErrorTest170(
                "SELECT * FROM AI_GENERATE_CHUNKS(source = 'text', CHUNK_SIZE = 5, CHUNK_TYPE = fixed)",
                new ParserErrorInfo(50, "SQL46005", "CHUNK_TYPE", "CHUNK_SIZE"));

            // Invalid order: enable_chunk_set_id before overlap
            ParserTestUtils.ErrorTest170(
                "SELECT * FROM AI_GENERATE_CHUNKS(source = 'text', CHUNK_TYPE = fixed, CHUNK_SIZE = 5, enable_chunk_set_id = 1, overlap = 10)",
                new ParserErrorInfo(86, "SQL46010", "enable_chunk_set_id"));

            // Invalid order: enable_chunk_set_id before CHUNK_SIZE
            ParserTestUtils.ErrorTest170(
                "SELECT * FROM AI_GENERATE_CHUNKS(source = 'text', CHUNK_TYPE = fixed, enable_chunk_set_id = 1, CHUNK_SIZE = 5, overlap = 10)",
                new ParserErrorInfo(70, "SQL46010", "enable_chunk_set_id"));

            // Invalid value: CHUNK_TYPE = 'fixed' (should be keyword, not string)
            ParserTestUtils.ErrorTest170(
                "SELECT * FROM AI_GENERATE_CHUNKS(source = 'text', CHUNK_TYPE = 'fixed', CHUNK_SIZE = 5)",
                new ParserErrorInfo(63, "SQL46010", "'fixed'"));

            // Invalid expression: CHUNK_TYPE = @CHUNK_TYPE (should not be variable)
            ParserTestUtils.ErrorTest170(
                "SELECT * FROM AI_GENERATE_CHUNKS(source = 'text', CHUNK_TYPE = @CHUNK_TYPE, CHUNK_SIZE = 5)",
                new ParserErrorInfo(63, "SQL46010", "@CHUNK_TYPE"));

            // Invalid parameter: CHUNK_TYPE = t1.c1 (should not be column reference)
            ParserTestUtils.ErrorTest170(
                "SELECT * FROM t1 CROSS APPLY AI_GENERATE_CHUNKS(source = 'text', CHUNK_TYPE = t1.c1, CHUNK_SIZE = 5)",
                new ParserErrorInfo(80, "SQL46010", "."));

            // Missing value after equals
            ParserTestUtils.ErrorTest170(
                "SELECT * FROM AI_GENERATE_CHUNKS(source = , CHUNK_TYPE = fixed, CHUNK_SIZE = 5)",
                new ParserErrorInfo(42, "SQL46010", ","));

            // Missing value for CHUNK_SIZE
            ParserTestUtils.ErrorTest170(
                "SELECT * FROM AI_GENERATE_CHUNKS(source = 'text', CHUNK_TYPE = fixed, CHUNK_SIZE = )",
                new ParserErrorInfo(83, "SQL46010", ")"));

            // Unknown parameter
            ParserTestUtils.ErrorTest170(
                "SELECT * FROM AI_GENERATE_CHUNKS(source = 'text', CHUNK_TYPE = fixed, CHUNK_SIZE = 5, invalid_param = 123)",
                new ParserErrorInfo(86, "SQL46010", "invalid_param"));

            // Extra comma at end
            ParserTestUtils.ErrorTest170(
                "SELECT * FROM AI_GENERATE_CHUNKS(source = 'text', CHUNK_TYPE = fixed, CHUNK_SIZE = 5,)",
                new ParserErrorInfo(85, "SQL46010", ")"));

            // Too many parameters
            ParserTestUtils.ErrorTest170(
                "SELECT * FROM AI_GENERATE_CHUNKS(source = 'text', CHUNK_TYPE = fixed, CHUNK_SIZE = 5, overlap = 1, enable_chunk_set_id = 2, extra = 3)",
                new ParserErrorInfo(122, "SQL46010", ","));

            // Function call with constant input, not keyword params
            ParserTestUtils.ErrorTest170(
                "SELECT * FROM ai_generate_chunks(3)",
                new ParserErrorInfo(33, "SQL46010", "3"));

            // Missing paramter "source"
            ParserTestUtils.ErrorTest170(
                "SELECT source, target FROM userTable cross apply ai_generate_chunks(target)",
                new ParserErrorInfo(68, "SQL46005", "SOURCE", "target"));

            // Misuse of parameter "source"
            ParserTestUtils.ErrorTest170(
                "SELECT source, target FROM userTable cross apply ai_generate_chunks(source)",
                new ParserErrorInfo(74, "SQL46010", ")"));

            // Misuse of bracketed function name
            ParserTestUtils.ErrorTest170(
                "SELECT * FROM [ai_generate_chunks](source = 'something to chunk', chunk_type = fixed, chunk_size = 5)",
                new ParserErrorInfo(42, "SQL46010", "="));

            // Misuse of 2-part identifier
            ParserTestUtils.ErrorTest170(
                "SELECT * FROM dbo.ai_generate_chunks(source = 'something to chunk', chunk_type = fixed, chunk_size = 5)",
                new ParserErrorInfo(36, "SQL46010", "("));

            // 2-part identifier misuse in CROSS APPLY
            ParserTestUtils.ErrorTest170(
                "SELECT * FROM source CROSS APPLY dbo.ai_generate_chunks(source.c1)",
                new ParserErrorInfo(55, "SQL46010", "("));

            // 2-part identifier misuse in CROSS APPLY
            ParserTestUtils.ErrorTest170(
                "SELECT source, target FROM userTable cross apply dbo.ai_generate_chunks(source)",
                new ParserErrorInfo(71, "SQL46010", "("));

            // Invalid CHUNK_TYPE
            ParserTestUtils.ErrorTest170(
                "SELECT * FROM AI_GENERATE_CHUNKS (source = 'some text', chunk_type = other, chunk_size = 5)",
                new ParserErrorInfo(69, "SQL46010", "other"));
        }

        
        /// <summary>
        /// Negative tests for AI_GENERATE_EMBEDDINGS syntax
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void GenerateEmbeddingsNegativeTest170()
        {
            // Missing required USE MODEL clause
            ParserTestUtils.ErrorTest170(
                "SELECT AI_GENERATE_EMBEDDINGS('My Default Input Text')",
                new ParserErrorInfo(53, "SQL46010", ")"));

            // Missing model name after USE MODEL
            ParserTestUtils.ErrorTest170(
                "SELECT AI_GENERATE_EMBEDDINGS('My Default Input Text' USE MODEL)",
                new ParserErrorInfo(63, "SQL46010", ")"));

            // Missing USE keyword before MODEL
            ParserTestUtils.ErrorTest170(
                "SELECT AI_GENERATE_EMBEDDINGS('My Default Input Text' MODEL MyModel)",
                new ParserErrorInfo(54, "SQL46010", "MODEL"));

            // USE keyword misplaced before input expression
            ParserTestUtils.ErrorTest170(
                "SELECT AI_GENERATE_EMBEDDINGS(USE MODEL MyModel 'My Default Input Text')",
                new ParserErrorInfo(30, "SQL46010", "USE"));

            // PARAMETERS specified without USE MODEL
            ParserTestUtils.ErrorTest170(
                "SELECT AI_GENERATE_EMBEDDINGS('My Default Input Text' PARAMETERS (TRY_CONVERT(JSON, N'{}')))",
                new ParserErrorInfo(54, "SQL46010", "PARAMETERS"));

            // PARAMETERS missing parentheses
            ParserTestUtils.ErrorTest170(
                "SELECT AI_GENERATE_EMBEDDINGS('My Default Input Text' USE MODEL MyModel PARAMETERS TRY_CONVERT(JSON, N'{}'))",
                new ParserErrorInfo(83, "SQL46010", "TRY_CONVERT"));

            // Invalid expression inside PARAMETERS (missing closing parenthesis)
            ParserTestUtils.ErrorTest170(
                "SELECT AI_GENERATE_EMBEDDINGS('My Default Input Text' USE MODEL MyModel PARAMETERS (TRY_CONVERT(JSON, N'{}')",
                new ParserErrorInfo(108, "SQL46029", "EOF"));

            // Extra comma at end inside PARAMETERS
            ParserTestUtils.ErrorTest170(
                "SELECT AI_GENERATE_EMBEDDINGS('My Default Input Text' USE MODEL MyModel PARAMETERS (TRY_CONVERT(JSON, N'{}'),))",
                new ParserErrorInfo(108, "SQL46010", ","));

            // PARAMETERS misplaced before input
            ParserTestUtils.ErrorTest170(
                "SELECT AI_GENERATE_EMBEDDINGS(PARAMETERS (TRY_CONVERT(JSON, N'{}') 'My Default Input Text'))",
                new ParserErrorInfo(67, "SQL46010", "'My Default Input Text'"));

            // Missing MODEL keyword after USE
            ParserTestUtils.ErrorTest170(
                "SELECT AI_GENERATE_EMBEDDINGS('My Default Input Text' USE MyModel)",
                new ParserErrorInfo(58, "SQL46005", "MODEL", "MyModel"));

            // NULL model name after USE MODEL
            ParserTestUtils.ErrorTest170(
                "SELECT AI_GENERATE_EMBEDDINGS('My Default Input Text' USE MODEL NULL)",
                new ParserErrorInfo(64, "SQL46010", "NULL"));
        }
    }
}
