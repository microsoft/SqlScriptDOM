//------------------------------------------------------------------------------
// <copyright file="TSqlParserTest.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlStudio.Tests.AssemblyTools.TestCategory;

namespace SqlStudio.Tests.UTSqlScriptDom
{
    [TestClass]
    public partial class SqlDomTests
    {
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        [Timeout(GlobalConstants.DefaultTestTimeout)]
        [Description("This test verifies that every SqlVersion has a correct TSqlParser mapping.")]
        public void VerifySqlVersionToParserMapping()
        {
            Dictionary<SqlVersion, Type> knownVersionMapping = new Dictionary<SqlVersion, Type>()
            {
                { SqlVersion.Sql80, typeof(TSql80Parser) },
                { SqlVersion.Sql90, typeof(TSql90Parser) },
                { SqlVersion.Sql100, typeof(TSql100Parser) },
                { SqlVersion.Sql110, typeof(TSql110Parser) },
                { SqlVersion.Sql120, typeof(TSql120Parser) },
                { SqlVersion.Sql130, typeof(TSql130Parser) },
                { SqlVersion.Sql140, typeof(TSql140Parser) },
                { SqlVersion.Sql150, typeof(TSql150Parser) },
                { SqlVersion.Sql160, typeof(TSql160Parser) },
                { SqlVersion.Sql170, typeof(TSql170Parser) },
                { SqlVersion.SqlFabricDW, typeof(TSqlFabricDWParser) }
            };

            TSqlParser parser = new TSql80Parser(false);

            foreach (SqlVersion version in Enum.GetValues(typeof(SqlVersion)))
            {
                Type expectedParserType;
                if (!knownVersionMapping.TryGetValue(version, out expectedParserType))
                {
                    Assert.Fail("New SqlVersion {0} needs to be added to knownVersionMapping.", version.ToString());
                }

                TSqlParser versionedParser = TSqlParser.CreateParser(version, false);

                Assert.IsNotNull(versionedParser, "Create parser returned null.");
                Assert.IsInstanceOfType(versionedParser, expectedParserType, "Created parser is not of the expected type.");

                versionedParser = TSqlParser.CreateParser(version, false);

                Assert.IsNotNull(versionedParser, "Create parser returned null.");
                Assert.IsInstanceOfType(versionedParser, expectedParserType, "Created parser is not of the expected type.");
            }
        }

        [TestMethod] 
        [Priority(0)]
        [Timeout(GlobalConstants.DefaultTestTimeout)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void StringUnquotingTest()
        {
            ParserTestUtils.ExecuteTestForAllParsers(delegate(TSqlParser parser)
                {
                    TSqlScript script = (TSqlScript)ParserTestUtils.ParseStringNoErrors(parser, "EXEC MyFunc 'str1', N'str2', 'str''3''', N'str''4', 10, 0x10, abc, [ab]]c], \"a\"\"bc\"");
                    ExecuteStatement execStatement = script.Batches[0].Statements[0] as ExecuteStatement;
                    Assert.IsNotNull(execStatement);

                    string [] paramVals = { "str1", "str2", "str'3'", "str'4", "10", "0x10", "abc", "ab]c", "a\"bc" };
                    Assert.AreEqual<int>(paramVals.Length, execStatement.ExecuteSpecification.ExecutableEntity.Parameters.Count);
                    for (int i = 0; i < paramVals.Length; ++i)
                    {
                        Literal literal = execStatement.ExecuteSpecification.ExecutableEntity.Parameters[i].ParameterValue as Literal;
                        Assert.IsNotNull(literal);
                        Assert.AreEqual<string>(paramVals[i], literal.Value);
                    }
                }, true);
        }

 
        [TestMethod] 
		[Priority(0)]
        [Timeout(GlobalConstants.DefaultTestTimeout)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void LexerGoOffsetsTest()
        {
            string script = "/* c1 */GO\r\n-- c2 \r\nGO  \r\n/* c3 */  GO";
            TSqlTokenType[] tokenTypes = new TSqlTokenType[] { TSqlTokenType.MultilineComment,
                TSqlTokenType.Go, TSqlTokenType.WhiteSpace, TSqlTokenType.SingleLineComment,
                TSqlTokenType.WhiteSpace, TSqlTokenType.Go, TSqlTokenType.WhiteSpace,
                TSqlTokenType.WhiteSpace, TSqlTokenType.MultilineComment, 
                TSqlTokenType.WhiteSpace, TSqlTokenType.Go, TSqlTokenType.EndOfFile };
            int [] tokenOffsets = new int[] { 0, 8, 10, 12, 18, 20, 22, 24, 26, 34, 36, 38};

            ParserTestUtils.ExecuteTestForAllParsers(delegate(TSqlParser parser)
            {
                using (StringReader sr = new StringReader(script))
                {
                    IList<ParseError> errors;
                    IList<TSqlParserToken> tokens = parser.GetTokenStream(sr, out errors);

                    foreach (var z in tokens)
                    {
                        System.Console.WriteLine(z.TokenType.ToString() + "  " + z.Text);
                    }
                    Assert.AreEqual<int>(0, errors.Count);
                    VerifyTokenTypesAndOffsets(tokens, tokenTypes, tokenOffsets, 0);
                }
            }, true);
        }

 
        [TestMethod] 
		[Priority(0)]
        [Timeout(GlobalConstants.DefaultTestTimeout)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void BuiltInTypesTest()
        {
            // Cursor and Table datatypes are not tested here...
            // Note: string and SqlDataTypeOption should have same indicies in array for the code below to work...
            string[] builtInDatatypes = new string[] {"BIGINT", "INTEGER", "INT", "SMALLINT", "TINYINT", "BIT",
                "DEC", "DECIMAL", "NUMERIC", "MONEY", "SMALLMONEY", "FLOAT", "REAL", "DATETIME",
                "SMALLDATETIME", "CHARACTER", "CHAR", "VARCHAR","TEXT", "NCHAR","NVARCHAR","NTEXT",
                "BINARY","VARBINARY","IMAGE","SQL_VARIANT","ROWVERSION","TIMESTAMP",
                "UNIQUEIDENTIFIER" };
            SqlDataTypeOption[] options = new SqlDataTypeOption[] { SqlDataTypeOption.BigInt, SqlDataTypeOption.Int, SqlDataTypeOption.Int, 
                SqlDataTypeOption.SmallInt, SqlDataTypeOption.TinyInt, SqlDataTypeOption.Bit, SqlDataTypeOption.Decimal,
                SqlDataTypeOption.Decimal, SqlDataTypeOption.Numeric,SqlDataTypeOption.Money, SqlDataTypeOption.SmallMoney,
                SqlDataTypeOption.Float, SqlDataTypeOption.Real, SqlDataTypeOption.DateTime, SqlDataTypeOption.SmallDateTime,
                SqlDataTypeOption.Char, SqlDataTypeOption.Char, SqlDataTypeOption.VarChar,SqlDataTypeOption.Text,
                SqlDataTypeOption.NChar, SqlDataTypeOption.NVarChar, SqlDataTypeOption.NText,SqlDataTypeOption.Binary,
                SqlDataTypeOption.VarBinary,SqlDataTypeOption.Image,SqlDataTypeOption.Sql_Variant,
                SqlDataTypeOption.Rowversion, SqlDataTypeOption.Timestamp, SqlDataTypeOption.UniqueIdentifier };

            int len = builtInDatatypes.Length;

            ParserTestUtils.ExecuteTestForAllParsers(delegate(TSqlParser parser)
            {
                for (int i = 0; i < len; ++i)
                {
                    SqlDataTypeReference sqlDataType = (SqlDataTypeReference)GetParsedDataType(parser, builtInDatatypes[i], string.Empty);
                    Assert.AreEqual<SqlDataTypeOption>(options[i], sqlDataType.SqlDataTypeOption);
                }
            }, true);
        }

        const int nKatmaiDataTypes = 4;
        string[] KatmaiDataTypes = new string[nKatmaiDataTypes] { "Time", "DateTime2", "DateTimeOffset", "Date" };
        bool[] KatmaiDataTypeCanHaveParam = new bool[nKatmaiDataTypes] { true, true, true, false };
        SqlDataTypeOption[] KatmaiDataTypeOptions = new SqlDataTypeOption[nKatmaiDataTypes] { SqlDataTypeOption.Time, 
                SqlDataTypeOption.DateTime2, SqlDataTypeOption.DateTimeOffset, SqlDataTypeOption.Date };

 
        [TestMethod] 
		[Priority(0)]
        [Timeout(GlobalConstants.DefaultTestTimeout)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void KatmaiTypesIn100ParserTest()
        {
            // Check, that new data types work in SQL 100
            ParserTestUtils.ExecuteTestForParsers(delegate(TSqlParser parser)
            {
                // Parameterless case
                for (int i = 0; i < nKatmaiDataTypes; ++i)
                {
                    SqlDataTypeReference sqlDataType = (SqlDataTypeReference)GetParsedDataType(parser, KatmaiDataTypes[i], string.Empty);
                    Assert.AreEqual<SqlDataTypeOption>(KatmaiDataTypeOptions[i], sqlDataType.SqlDataTypeOption);
                    Assert.AreEqual<int>(0, sqlDataType.Parameters.Count);
                }

                // Correct parameters specified
                for (int i = 0; i < nKatmaiDataTypes; ++i)
                {
                    if (KatmaiDataTypeCanHaveParam[i])
                    {
                        SqlDataTypeReference sqlDataType = (SqlDataTypeReference)GetParsedDataType(parser, KatmaiDataTypes[i], "(1)");
                        Assert.AreEqual<SqlDataTypeOption>(KatmaiDataTypeOptions[i], sqlDataType.SqlDataTypeOption);
                        Assert.AreEqual<int>(1, sqlDataType.Parameters.Count);
                        Assert.AreEqual<string>("1", sqlDataType.Parameters[0].Value);
                    }
                }

                // Wrong parameters
                for (int i = 0; i < nKatmaiDataTypes; ++i)
                {
                    ParserTestUtils.ErrorTest(parser, 
                        "DECLARE @a as " + KatmaiDataTypes[i] + "(1,2)",
                        new ParserErrorInfo(14, "SQL46009", KatmaiDataTypes[i]));

                    if (!KatmaiDataTypeCanHaveParam[i])
                    {
                        ParserTestUtils.ErrorTest(parser,
                            "DECLARE @a as " + KatmaiDataTypes[i] + "(10)",
                            new ParserErrorInfo(14, "SQL46008", KatmaiDataTypes[i]));
                    }
                }

            }, new TSql100Parser(true));
        }

        static DataTypeReference GetParsedDataType(TSqlParser parser, string dataType, string suffix)
        {
            string input = "DECLARE @a as " + dataType + suffix;
            DeclareVariableStatement st = (DeclareVariableStatement)(((TSqlScript)ParserTestUtils.ParseStringNoErrors(parser, input)).Batches[0].Statements[0]);
            return st.Declarations[0].DataType;
        }

 
        [TestMethod] 
		[Priority(0)]
        [Timeout(GlobalConstants.DefaultTestTimeout)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void KatmaiTypesIn80And90ParserTest()
        {
            // Check, how 80/90 parsers react to new types - should treat them as user types
            ParserTestUtils.ExecuteTestForParsers(delegate(TSqlParser parser)
            {
                for (int i = 0; i < 4; ++i)
                {
                    UserDataTypeReference dataType = (UserDataTypeReference)GetParsedDataType(parser, KatmaiDataTypes[i], string.Empty);
                    Assert.AreEqual<string>(KatmaiDataTypeOptions[i].ToString(), dataType.Name.BaseIdentifier.Value);
                    Assert.AreEqual<int>(0, dataType.Parameters.Count);
                }
            }, new TSql80Parser(true), new TSql90Parser(true));
        }

        const int SemicolonsTestNumberOfScripts = 4;
        string[] SemicolonsTestScripts = new string[SemicolonsTestNumberOfScripts] { 
                "create view myview2 as select 1 as hello", 
                "create proc p1 as return 0;",
                "aaa;", 
                "create table t1(c1 int);" };
        int[] SemicolonsTestErrorPositions = new int[SemicolonsTestNumberOfScripts] { 40, 27, 4, 24 };

 
        [TestMethod] 
		[Priority(0)]
        [Timeout(GlobalConstants.DefaultTestTimeout)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SemicolonsAfterStatementsInDifferentModes()
        {
            TSql80Parser parser80 = new TSql80Parser(true);
            // Original script should work in all modes parsers

            for (int i = 0; i < SemicolonsTestNumberOfScripts; ++i)
            {
                ParserTestUtils.ErrorTestAllParsers(SemicolonsTestScripts[i]);

                    // Lets check, that adding one semicolon at the end renders script SQL 2005 only
                string scriptWithSemi = SemicolonsTestScripts[i] + ";";

                // Check for single error in 80
                ParserTestUtils.ErrorTest(parser80, scriptWithSemi,
                    new ParserErrorInfo(SemicolonsTestErrorPositions[i], "SQL46010", ";"));

                // Check for NO errors in 90+
                ParserTestUtils.ErrorTest90AndAbove(scriptWithSemi);
            }
        }

 
        [TestMethod] 
		[Priority(0)]
        [Timeout(GlobalConstants.DefaultTestTimeout)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SingleLineCommentEOFTest()
        {
            string testScript = @"create view x as select 1 as c1
--";
            ParserTestUtils.ExecuteTestForAllParsers(delegate(TSqlParser parser)
            {
                IList<TSqlParserToken> tokens;
                IList<ParseError> errors;

                using (StringReader sr = new StringReader(testScript))
                {
                    tokens = parser.GetTokenStream(sr, out errors, 0, 1, 1);
                }
                Assert.AreEqual<int>(0, errors.Count);
                int singleLineCommentToken = tokens.Count - 2;
                Assert.IsTrue(tokens[singleLineCommentToken].TokenType == TSqlTokenType.SingleLineComment);
                Assert.AreEqual<int>(2, tokens[singleLineCommentToken].Text.Length);
            }, true);

        }

 
        [TestMethod] 
		[Priority(0)]
        [Timeout(GlobalConstants.DefaultTestTimeout)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void CommentSkippingAndTokenIndexSmokeTest()
        {
            ParserTestUtils.ExecuteTestForAllParsers(delegate(TSqlParser parser)
            {
                TSqlScript script = (TSqlScript)ParserTestUtils.ParseStringNoErrors(parser, "");
                CheckTokenIndiciesForFragment(script, 0, 0);

                script = (TSqlScript)ParserTestUtils.ParseStringNoErrors(parser, 
                    @"-- Single line comment
CREATE TABLE /* c2 */ t1(c1 int)/* multiline
                                    comment*/"
                    );

                CheckTokenIndiciesForFragment(script, 0, 15);
                Assert.AreEqual<TSqlTokenType>(TSqlTokenType.EndOfFile, script.ScriptTokenStream[script.LastTokenIndex].TokenType);
                Assert.AreEqual<int>(1, script.Batches.Count);
                CheckFirstAndLastTokensForFragment(script.Batches[0], 2, TSqlTokenType.Create,
                    13, TSqlTokenType.RightParenthesis);

                Assert.AreEqual<int>(1, script.Batches[0].Statements.Count);
                CreateTableStatement createTable = (CreateTableStatement)script.Batches[0].Statements[0];
                CheckFirstAndLastTokensForFragment(createTable, 2, TSqlTokenType.Create,
                    13, TSqlTokenType.RightParenthesis);

                Assert.AreEqual<int>(1, createTable.Definition.ColumnDefinitions.Count);
                CheckFirstAndLastTokensForFragment(createTable.Definition.ColumnDefinitions[0], 10, TSqlTokenType.Identifier,
                    12, TSqlTokenType.Identifier);
            }, true);
        }

        [TestMethod] 
		[Priority(0)]
        [Timeout(GlobalConstants.DefaultTestTimeout)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void StartOffsetTest()
        {
            const string testScript = "create table t1(c1 int)";
            const int numberOfTokens = 11;
            const int offsetShift = 10;

            TSqlTokenType[] tokenTypes = new TSqlTokenType[numberOfTokens] { TSqlTokenType.Create, TSqlTokenType.WhiteSpace, 
                TSqlTokenType.Table, TSqlTokenType.WhiteSpace, TSqlTokenType.Identifier, 
                TSqlTokenType.LeftParenthesis, TSqlTokenType.Identifier, TSqlTokenType.WhiteSpace, 
                TSqlTokenType.Identifier, TSqlTokenType.RightParenthesis, TSqlTokenType.EndOfFile};
            int[] zeroBasedTokenOffsets = new int[numberOfTokens] { 0, 6, 7, 12, 13, 15, 16, 18, 19, 22, 23 };

            ParserTestUtils.ExecuteTestForAllParsers(delegate(TSqlParser parser)
            {
                IList<TSqlParserToken> tokens;
                IList<ParseError> errors;
                
                using (StringReader sr = new StringReader(testScript))
                {
                    tokens = parser.GetTokenStream(sr, out errors, 0, 1, 1);
                }
                Assert.AreEqual<int>(0, errors.Count);
                VerifyTokenTypesAndOffsets(tokens, tokenTypes, zeroBasedTokenOffsets, 0);

                using (StringReader sr = new StringReader(testScript))
                {
                    tokens = parser.GetTokenStream(sr, out errors, offsetShift, 1, offsetShift);
                }
                Assert.AreEqual<int>(0, errors.Count);
                VerifyTokenTypesAndOffsets(tokens, tokenTypes, zeroBasedTokenOffsets, offsetShift);

            }, true);
        }

 
        [TestMethod] 
		[Priority(0)]
        [Timeout(GlobalConstants.DefaultTestTimeout)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void UniqueConstraintDefinitionClusteredPropertyDepricated()
        {
            string input = @"CREATE TABLE TestTable( c1 [int], CONSTRAINT [PK_c1] PRIMARY KEY NONCLUSTERED (c1))";
            IList<ParseError> errors = null;
            TSqlScript script = (TSqlScript)new TSql140Parser(true).Parse(new StringReader(input), out errors);            
            TSqlBatch batch = script.Batches.First();
            CreateTableStatement statement = (CreateTableStatement) batch.Statements.First();
            UniqueConstraintDefinition primaryKey = (UniqueConstraintDefinition) statement.Definition.TableConstraints.First();
            Assert.AreEqual(false, primaryKey.Clustered);
            Assert.AreEqual(IndexTypeKind.NonClustered, primaryKey.IndexType.IndexTypeKind);
        }

        [TestMethod]
        [Priority(0)]
        [Timeout(GlobalConstants.DefaultTestTimeout)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SystemTimeLastTokenIndexTest()
        {
            ParserTestUtils.ExecuteTestForParsers(delegate (TSqlParser parser)
            {
                TSqlScript script = (TSqlScript)ParserTestUtils.ParseStringNoErrors(parser, "");
                CheckTokenIndiciesForFragment(script, 0, 0);

                script = (TSqlScript)ParserTestUtils.ParseStringNoErrors(parser,
                    @"ALTER TABLE t1 ADD
date1 DATETIME2 GENERATED ALWAYS AS ROW START HIDDEN NOT NULL DEFAULT CAST('20200101' AS DATETIME2),
date2 DATETIME2 GENERATED ALWAYS AS ROW END HIDDEN NOT NULL DEFAULT CAST('20200102' as DATETIME2),
PERIOD FOR SYSTEM_TIME(date1, date2)"
                    );

                CheckTokenIndiciesForFragment(script, 0, 83);
                Assert.AreEqual<TSqlTokenType>(TSqlTokenType.EndOfFile, script.ScriptTokenStream[script.LastTokenIndex].TokenType);
                Assert.AreEqual<int>(1, script.Batches.Count);

                Assert.AreEqual<int>(1, script.Batches[0].Statements.Count);
                AlterTableStatement alterTable = (AlterTableStatement)script.Batches[0].Statements[0];
                CheckFirstAndLastTokensForFragment(alterTable, 0, TSqlTokenType.Alter, 82, TSqlTokenType.RightParenthesis);
            }, new TSql130Parser(false), new TSql140Parser(false), new TSql150Parser(false));
        }

        [TestMethod]
        [Priority(0)]
        [Timeout(GlobalConstants.DefaultTestTimeout)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void OpenRowsetBulkWithThreeFiles()
        {
            string input = @"SELECT TOP 10 * FROM OPENROWSET (BULK ('https://azureopendatastorage.blob.core.windows.net/censusdatacontainer/release/us_population_county/year=2000/*.parquet', 'https://azureopendatastorage.blob.core.windows.net/censusdatacontainer/release/us_population_county/year=2010/*.parquet', 'https://azureopendatastorage.blob.core.windows.net/censusdatacontainer/release/us_population_county/year=2008/*.parquet'), FORMAT = 'PARQUET') AS [r9];";
            IList<ParseError> errors = null;
            TSqlScript script = (TSqlScript)new TSql160Parser(true).Parse(new StringReader(input), out errors);

            script.Accept(new GenericFragmentVisitor<BulkOpenRowset>(
                delegate (BulkOpenRowset bulkOpenRowset)
                {
                    return bulkOpenRowset.DataFiles.Count == 3;
                }
            ));
            script.Accept(new GenericFragmentVisitor<LiteralBulkInsertOption>(
                delegate(LiteralBulkInsertOption literalBulkInsertOption)
                {
                    return literalBulkInsertOption.Value.Value.Equals(CodeGenerationSupporter.Parquet);
                }
            ));
        }

        [TestMethod]
        [Priority(0)]
        [Timeout(GlobalConstants.DefaultTestTimeout)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void ParsingNativelyCompiledStoredProceduresWithAtomicBlock()
        {
            string input = "";
            IList<ParseError> errors = null;
            TSqlScript script = null;

            input = @"
CREATE PROCEDURE dbo.testproc @p1 INT, @p2 INT NOT NULL
WITH EXECUTE AS OWNER, SCHEMABINDING, NATIVE_COMPILATION
AS
BEGIN 
ATOMIC WITH (TRANSACTION ISOLATION LEVEL = SNAPSHOT, LANGUAGE = N'us_english')
UPDATE dbo.Departments
SET p3 = ISNULL(p3, 0) + @p2
WHERE ID = @p1
END;";
            errors = null;
            script = (TSqlScript)new TSql160Parser(true).Parse(new StringReader(input), out errors);

            script.Accept(new GenericFragmentVisitor<ProcedureStatementBody>(
                 delegate (ProcedureStatementBody procedureStatementBody)
                 {
                     return procedureStatementBody.StatementList.StartLine.Equals(5) && procedureStatementBody.StatementList.StartColumn.Equals(1);
                 }
            ));
        }

        [TestMethod]
        [Priority(0)]
        [Timeout(GlobalConstants.DefaultTestTimeout)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void ParsingSelectWithOptimizerHint()
        {
            string input = "";
            IList<ParseError> errors = null;
            TSqlScript script = null;

            input = @"Select 1 option(
HASH  GROUP, 
ORDER  GROUP, 
CONCAT UNION, 
HASH UNION,
MERGE UNION,
Keep UNION,
LOOP  JOIN, 
HASH  JOIN, 
MERGE  JOIN,  
PARAMETERIZATION Simple,
IGNORE_NONCLUSTERED_COLUMNSTORE_INDEX, 
KEEP PLAN, 
KEEPFIXED PLAN,
recompile, 
EXPAND VIEWS)";
            errors = null;
            script = (TSqlScript)new TSql160Parser(true).Parse(new StringReader(input), out errors);
            Assert.IsTrue(errors.Count == 0, "Unexpected parsing error");

            var selectStatement = (SelectStatement) script.Batches[0].Statements[0];
            Assert.IsNotNull(selectStatement);
            Assert.IsTrue(selectStatement.OptimizerHints.Any());

            SqlScriptGenerator scriptGenerator = new Sql160ScriptGenerator();
            scriptGenerator.GenerateScript(script, out string actualResult);
            Assert.IsTrue(actualResult.IndexOf("option", StringComparison.OrdinalIgnoreCase) > 0);

            foreach (var hint in selectStatement.OptimizerHints)
            {
                Assert.IsTrue(hint.FirstTokenIndex > 0 && hint.LastTokenIndex > 0);
            }

            selectStatement.Accept(new GenericFragmentVisitor<SelectStatement>(
                 delegate (SelectStatement select)
                 {
                     return select != null && select.OptimizerHints.Any();
                 }
            ));
        }

        [TestMethod]
        [Priority(0)]
        [Timeout(GlobalConstants.DefaultTestTimeout)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void OpenRowsetBulkWithTwoFiles()
        {
            string input = @"SELECT TOP 10 * FROM OPENROWSET (BULK ('https://azureopendatastorage.blob.core.windows.net/censusdatacontainer/release/us_population_county/year=2000/*.parquet', 'https://azureopendatastorage.blob.core.windows.net/censusdatacontainer/release/us_population_county/year=2010/*.parquet'), FORMAT = 'PARQUET') AS [r9];";
            IList<ParseError> errors = null;
            TSqlScript script = (TSqlScript)new TSql160Parser(true).Parse(new StringReader(input), out errors);

            script.Accept(new GenericFragmentVisitor<BulkOpenRowset>(
                delegate (BulkOpenRowset bulkOpenRowset)
                {
                    return bulkOpenRowset.DataFiles.Count == 2;
                }
            ));
            script.Accept(new GenericFragmentVisitor<LiteralBulkInsertOption>(
                delegate (LiteralBulkInsertOption literalBulkInsertOption)
                {
                    return literalBulkInsertOption.Value.Value.Equals(CodeGenerationSupporter.Parquet);
                }
            ));
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        [Timeout(GlobalConstants.DefaultTestTimeout)]
        public void OpenRowsetBulkWithOneFile()
        {
            string input = @"SELECT TOP 10 * FROM OPENROWSET (BULK 'https://azureopendatastorage.blob.core.windows.net/censusdatacontainer/release/us_population_county/year=2000/*.parquet', FORMAT = 'PARQUET') AS [r9];";
            IList<ParseError> errors = null;
            TSqlScript script = (TSqlScript)new TSql160Parser(true).Parse(new StringReader(input), out errors);

            script.Accept(new GenericFragmentVisitor<BulkOpenRowset>(
                delegate (BulkOpenRowset bulkOpenRowset)
                {
                    return bulkOpenRowset.DataFiles.Count == 1;
                }
            ));
            script.Accept(new GenericFragmentVisitor<LiteralBulkInsertOption>(
                delegate (LiteralBulkInsertOption literalBulkInsertOption)
                {
                    return literalBulkInsertOption.Value.Value.Equals(CodeGenerationSupporter.Parquet);
                }
            ));
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void FieldQuoteParsingIn160ParserTest()
        {
            TSql160Parser parser = new TSql160Parser(true); IList<ParseError> errors;
           
            string scriptString = @"create PROC [dbo].[test]  AS
                                    BEGIN
                                    CREATE TABLE #Test
                                    ([Col1] [NVARCHAR](266));
                                    COPY INTO #Test
                                    FROM 'https://xxxx.blob.core.windows.net/raw/'
                                    WITH (FIELDQUOTE='',FIRSTROW = 1)
                                    END;";
           
            var script = new StringReader(scriptString);
            parser.Parse(script, out errors);
            foreach (var error in errors)
            {
                Console.WriteLine($"Error {error.Number} Message {error.Message}");
            }
            Assert.AreEqual(0, errors.Count);
        }

        /// <summary>
        /// This test validates the generation of table and stored procedure that contains CTAS statements
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void CreateCTASParser160Test()
        {
            TSql160Parser parser = new TSql160Parser(true);
            StringReader reader = new StringReader(@"
                                                    -- CTAS
                                                    CREATE TABLE TestTable AS
                                                    SELECT customername, contactname
                                                    FROM customers
                                                    GO;

                                                    -- CTAS in stored procedure - 'create table As Select *'
                                                    CREATE PROCEDURE test_proc_withCreateSelect
                                                    AS
                                                    BEGIN
                                                       CREATE TABLE Test1
                                                       AS
                                                       SELECT * FROM Test
                                                    END
                                                    GO;

                                                    -- CTAS in stored procedure - 'create table With( ) As Select'
                                                    CREATE PROCEDURE test_proc_withCreateWith
                                                    AS
                                                    BEGIN
                                                       CREATE TABLE [dbo].[ReplicateToHash_ID]
                                                       WITH(DISTRIBUTION = Hash(id))  
                                                       AS SELECT ID FROM [dbo].[REPLICATE_TEST_UPDATE]
                                                    END
                                                    GO;

                                                    -- CTAS in stored procedure - 'create table as Select columnsNames'
                                                    CREATE PROCEDURE test_proc_withSelectColumns
                                                    AS
                                                    BEGIN
                                                       CREATE TABLE Test1
                                                       AS
                                                       SELECT Id AS ID,
	                                                    person AS Person
	                                                    FROM Test
                                                    END
                                                    GO;");

            var fragments = parser.Parse(reader, out IList<ParseError> errors);

            Assert.IsTrue(errors.Count == 0);
            Assert.AreEqual(4, ((TSqlScript)fragments).Batches.Count);
            foreach(TSqlBatch batch in ((TSqlScript)fragments).Batches)
            {
                TSqlStatement statement = batch.Statements[0];
                Assert.IsNotNull(statement);
                Assert.IsTrue(statement is CreateTableStatement || statement is CreateProcedureStatement);
            }
        }

        void VerifyTokenTypesAndOffsets(IList<TSqlParserToken> tokens, TSqlTokenType[] tokenTypes, int[] zeroBasedTokenOffsets, int offsetShift)
        {
            Assert.AreEqual<int>(tokenTypes.Length, tokens.Count);
            for (int i = 0; i < tokens.Count; ++i)
            {
                Assert.AreEqual<TSqlTokenType>(tokenTypes[i], tokens[i].TokenType);
                Assert.AreEqual<int>(zeroBasedTokenOffsets[i] + offsetShift, tokens[i].Offset);
            }
        }

        void CheckFirstAndLastTokensForFragment(TSqlFragment fragment, int first, TSqlTokenType firstType, int last, TSqlTokenType lastType)
        {
            CheckTokenIndiciesForFragment(fragment, first, last);
            if (first != TSqlFragment.Uninitialized)
                Assert.AreEqual<TSqlTokenType>(firstType, fragment.ScriptTokenStream[fragment.FirstTokenIndex].TokenType);
            if (last != TSqlFragment.Uninitialized)
                Assert.AreEqual<TSqlTokenType>(lastType, fragment.ScriptTokenStream[fragment.LastTokenIndex].TokenType);
        }

        void CheckTokenIndiciesForFragment(TSqlFragment fragment, int first, int last)
        {
            Assert.AreEqual<int>(first, fragment.FirstTokenIndex);
            Assert.AreEqual<int>(last, fragment.LastTokenIndex);
        }
    }
}
