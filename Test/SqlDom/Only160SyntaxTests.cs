//------------------------------------------------------------------------------
// <copyright file="Only160SyntaxTests.cs" company="Microsoft">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
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
        private static readonly ParserTest[] Only160TestInfos =
        {
            new ParserTest160("AlterTableStatementTests160.sql", nErrors80: 0, nErrors90: 0, nErrors100: 0, nErrors110: 0, nErrors120: 0, nErrors130: 0, nErrors140: 0, nErrors150: 0),
            new ParserTest160("AlterTableResumableTests160.sql", nErrors80: 4, nErrors90: 4, nErrors100: 4, nErrors110: 4, nErrors120: 4, nErrors130: 4, nErrors140: 4, nErrors150: 4),
            new ParserTest160("ExpressionTests160.sql", nErrors80: 1, nErrors90: 0, nErrors100: 0, nErrors110: 0, nErrors120: 0, nErrors130: 0, nErrors140: 0, nErrors150: 0),
            new ParserTest160("CreateUserFromExternalProvider160.sql", nErrors80: 2, nErrors90: 1, nErrors100: 1, nErrors110: 1, nErrors120: 1, nErrors130: 1, nErrors140: 1, nErrors150: 1),
            new ParserTest160("CreateExternalTableStatementTests160.sql", nErrors80: 2, nErrors90: 2, nErrors100: 2, nErrors110: 2, nErrors120: 2, nErrors130: 2, nErrors140: 2, nErrors150: 2),
            new ParserTest160("WindowClauseTests160.sql", nErrors80: 14, nErrors90: 13, nErrors100: 13, nErrors110: 13, nErrors120: 13, nErrors130: 13, nErrors140: 13, nErrors150: 13),
            new ParserTest160("CreateExternalDataSourceStatementTests160.sql", nErrors80: 2, nErrors90: 2, nErrors100: 2, nErrors110: 2, nErrors120: 2, nErrors130: 0, nErrors140: 0, nErrors150: 0),
            new ParserTest160("CreateDatabaseTests160.sql", nErrors80: 4, nErrors90: 4, nErrors100: 4, nErrors110: 4, nErrors120: 4, nErrors130: 4, nErrors140: 4, nErrors150: 4),
            new ParserTest160("CreateLedgerTableTests160.sql", nErrors80: 14, nErrors90: 14, nErrors100: 14, nErrors110: 14, nErrors120: 14, nErrors130: 14, nErrors140: 14, nErrors150: 14),
            new ParserTest160("CreateIndexStatementTests160.sql", nErrors80: 12, nErrors90: 17, nErrors100: 15, nErrors110: 15, nErrors120: 15, nErrors130: 15, nErrors140: 15, nErrors150: 15),
            new ParserTest160("MergeStatementTests160.sql", nErrors80: 1, nErrors90: 5, nErrors100: 5, nErrors110: 5, nErrors120: 5, nErrors130: 5, nErrors140: 5, nErrors150: 5),
            new ParserTest160("SelectStatementTests160.sql", nErrors80: 5, nErrors90: 5, nErrors100: 5, nErrors110: 5, nErrors120: 5, nErrors130: 5, nErrors140: 5, nErrors150: 5),
            new ParserTest160("CreateTableTests160.sql", nErrors80: 11, nErrors90: 11, nErrors100: 11, nErrors110: 11, nErrors120: 11, nErrors130: 7, nErrors140: 7, nErrors150: 7),
            new ParserTest160("CreateFunctionStatementTests160.sql", nErrors80: 0, nErrors90: 0, nErrors100: 0, nErrors110: 0, nErrors120: 0, nErrors130: 0, nErrors140: 0, nErrors150: 0),
            new ParserTest160("CreateProcedureStatementTests160.sql", nErrors80: 0, nErrors90: 0, nErrors100: 0, nErrors110: 0, nErrors120: 0, nErrors130: 0, nErrors140: 0, nErrors150: 0),
            new ParserTest160("OpenRowsetStatementTests160.sql", nErrors80: 9, nErrors90: 9, nErrors100: 9, nErrors110: 9, nErrors120: 9, nErrors130: 9, nErrors140: 9, nErrors150: 3),
            new ParserTest160("WithinGroupTests160.sql", nErrors80: 2, nErrors90: 2, nErrors100: 2, nErrors110: 2, nErrors120: 2, nErrors130: 0, nErrors140: 0, nErrors150: 0),
            // Built-in functions with valid opening and closing parantheses are recognized by default for all parser versions
            new ParserTest160("BitManipulationFunctionTests160.sql", nErrors80: 0, nErrors90: 0, nErrors100: 0, nErrors110: 0, nErrors120: 0, nErrors130: 0, nErrors140: 0, nErrors150: 0),
            new ParserTest160("ShiftOperatorTests160.sql", nErrors80: 3, nErrors90: 3, nErrors100: 3, nErrors110: 3, nErrors120: 3, nErrors130: 3, nErrors140: 3, nErrors150: 3),
            new ParserTest160("BuiltInFunctionTests160.sql", nErrors80: 1, nErrors90: 1, nErrors100: 2, nErrors110: 0, nErrors120: 0, nErrors130: 0, nErrors140: 0, nErrors150: 0),
            new ParserTest160("TrimFunctionTests160.sql", nErrors80: 7, nErrors90: 7, nErrors100: 7, nErrors110: 7, nErrors120: 7, nErrors130: 7, nErrors140: 4, nErrors150: 4),
            new ParserTest160("JsonFunctionTests160.sql", nErrors80: 9, nErrors90: 8, nErrors100: 14, nErrors110: 14, nErrors120: 14, nErrors130: 14, nErrors140: 14, nErrors150: 14),
            new ParserTest160(scriptFilename: "IgnoreRespectNullsSyntaxTests160.sql", nErrors80: 12, nErrors90: 8, nErrors100: 8, nErrors110: 8, nErrors120: 8, nErrors130: 8, nErrors140: 8, nErrors150: 8),
            new ParserTest160(scriptFilename: "FuzzyStringMatchingTests160.sql",  nErrors80: 0, nErrors90: 0, nErrors100: 0, nErrors110: 0, nErrors120: 0, nErrors130: 0, nErrors140: 0, nErrors150: 0),
            // OPENROWSET BULK statements specific to SQL Serverless Pools
            new ParserTest160(scriptFilename: "OpenRowsetBulkStatementTests160.sql", nErrors80: 8, nErrors90: 8, nErrors100: 8, nErrors110: 8, nErrors120: 8, nErrors130: 8, nErrors140: 8, nErrors150: 8),
            new ParserTest160(scriptFilename: "CreateStatisticsStatementTests160.sql", nErrors80: 4, nErrors90: 4, nErrors100: 4, nErrors110: 4, nErrors120: 4, nErrors130: 4, nErrors140: 4, nErrors150: 4),
            new ParserTest160(scriptFilename: "CreateExternalFileFormatStatementTests160.sql", nErrors80: 2, nErrors90: 2, nErrors100: 2, nErrors110: 2, nErrors120: 2, nErrors130: 2, nErrors140: 2, nErrors150: 2),
            new ParserTest160("StringConcatOperatorTests160.sql", nErrors80: 11, nErrors90: 11, nErrors100: 11, nErrors110: 11, nErrors120: 11, nErrors130: 11, nErrors140: 11, nErrors150: 11),
            new ParserTest160("InlineIndexColumnWithINCLUDEtest.sql", nErrors80: 3, nErrors90: 3, nErrors100: 3, nErrors110: 3, nErrors120: 2, nErrors130: 2, nErrors140: 2, nErrors150: 2),
            new ParserTest160("MaterializedViewTests160.sql", nErrors80: 6, nErrors90: 3, nErrors100: 3, nErrors110: 3, nErrors120: 3, nErrors130: 0, nErrors140: 0, nErrors150: 0),
            new ParserTest160("NotEnforcedConstraintTests160.sql", nErrors80: 3, nErrors90: 1, nErrors100: 1, nErrors110: 1, nErrors120: 1, nErrors130: 1, nErrors140: 1, nErrors150: 1),
            new ParserTest160("VectorFunctionTests160.sql", nErrors80: 0, nErrors90: 0, nErrors100: 0, nErrors110: 0, nErrors120: 0, nErrors130: 0, nErrors140: 0, nErrors150: 0),
            new ParserTest160("CreateEventSessionNotLikePredicate.sql", nErrors80: 2, nErrors90: 1,  nErrors100: 1, nErrors110: 1, nErrors120: 1, nErrors130: 0, nErrors140: 0, nErrors150: 0),
        };

        private static readonly ParserTest[] SqlAzure160_TestInfos = 
        {
            // Add parser tests that have Azure-specific syntax constructs
            // (those that have versioning visitor implemented)
            //
        };

        /// <summary>
        /// This test runs only the graph syntax tests.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void GraphTSql160SyntaxParserTest()
        {
            TSql160Parser parser = new TSql160Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql160);
            foreach (ParserTest ti in GraphTestsOnly)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result160);
            }

            // Parsing both for Azure and standalone flavors - explicitly set
            // enum value to 'All'
            //
            parser = new TSql160Parser(true, SqlEngineType.All);
            scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql160);
            scriptGen.Options.SqlEngineType = SqlEngineType.All;
            foreach (ParserTest ti in GraphTestsOnly)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result160);
            }

            // Explicitly ask for 'standalone' and parse
            //
            parser = new TSql160Parser(true, SqlEngineType.Standalone);
            scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql160);
            scriptGen.Options.SqlEngineType = SqlEngineType.Standalone;
            foreach (ParserTest ti in GraphTestsOnly)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result160);
            }
        }

        [TestMethod] 
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql160SyntaxIn160ParserTest()
        {
            // Parsing both for Azure and standalone flavors
            //
            TSql160Parser parser = new TSql160Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql160);
            foreach (ParserTest ti in Only160TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result160);
            }

            // Parsing both for Azure and standalone flavors - explicitly set
            // enum value to 'All'
            //
            parser = new TSql160Parser(true, SqlEngineType.All);
            scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql160);
            scriptGen.Options.SqlEngineType = SqlEngineType.All;
            foreach (ParserTest ti in Only160TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result160);
            }

            // Explicitly ask for 'standalone' and parse
            //
            parser = new TSql160Parser(true, SqlEngineType.Standalone);
            scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql160);
            scriptGen.Options.SqlEngineType = SqlEngineType.Standalone;
            foreach (ParserTest ti in Only160TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result160);
            }
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql160Azure_SyntaxIn160ParserTest()
        {
            // This test checks that Azure-specific syntax constructs are successfully parsed
            // with Sql160 parser pointed to Sql Azure.
            //
            TSql160Parser parser = new TSql160Parser(true, SqlEngineType.SqlAzure);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql160);
            scriptGen.Options.SqlEngineType = SqlEngineType.SqlAzure;
            foreach (ParserTest ti in SqlAzure160_TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result160);
            }
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql160SyntaxIn150ParserTest()
        {
            TSql150Parser parser = new TSql150Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql150);
            System.Collections.Generic.List<ParserTest> all160ParserTests = new System.Collections.Generic.List<ParserTest>();
            all160ParserTests.AddRange(Only160TestInfos);
            all160ParserTests.AddRange(SqlAzure160_TestInfos);

            foreach (ParserTest ti in all160ParserTests)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result150);
            }
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql160SyntaxIn140ParserTest()
        {
            TSql140Parser parser = new TSql140Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql140);
            System.Collections.Generic.List<ParserTest> all160ParserTests = new System.Collections.Generic.List<ParserTest>();
            all160ParserTests.AddRange(Only160TestInfos);
            all160ParserTests.AddRange(SqlAzure160_TestInfos);

            foreach (ParserTest ti in all160ParserTests)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result140);
            }
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql160SyntaxIn130ParserTest()
        {
            TSql130Parser parser = new TSql130Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql130);
            System.Collections.Generic.List<ParserTest> all160ParserTests = new System.Collections.Generic.List<ParserTest>();
            all160ParserTests.AddRange(Only160TestInfos);
            all160ParserTests.AddRange(SqlAzure160_TestInfos);

            foreach (ParserTest ti in all160ParserTests)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result130);
            }
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql160SyntaxIn120ParserTest()
        {
            TSql120Parser parser = new TSql120Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql120);
            System.Collections.Generic.List<ParserTest> all160ParserTests = new System.Collections.Generic.List<ParserTest>();
            all160ParserTests.AddRange(Only160TestInfos);
            all160ParserTests.AddRange(SqlAzure160_TestInfos);

            foreach (ParserTest ti in all160ParserTests)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result120);
            }
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql160SyntaxIn110ParserTest()
        {
            TSql110Parser parser = new TSql110Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql110);
            System.Collections.Generic.List<ParserTest> all160ParserTests = new System.Collections.Generic.List<ParserTest>();
            all160ParserTests.AddRange(Only160TestInfos);
            all160ParserTests.AddRange(SqlAzure160_TestInfos);

            foreach (ParserTest ti in all160ParserTests)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result110);
            }
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql160SyntaxIn100ParserTest()
        {
            TSql100Parser parser = new TSql100Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql100);
            System.Collections.Generic.List<ParserTest> all160ParserTests = new System.Collections.Generic.List<ParserTest>();
            all160ParserTests.AddRange(Only160TestInfos);
            all160ParserTests.AddRange(SqlAzure160_TestInfos);

            foreach (ParserTest ti in all160ParserTests)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result100);
            }
        }

        [TestMethod] 
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql160SyntaxIn90ParserTest()
        {
            TSql90Parser parser = new TSql90Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql90);
            System.Collections.Generic.List<ParserTest> all160ParserTests = new System.Collections.Generic.List<ParserTest>();
            all160ParserTests.AddRange(Only160TestInfos);
            all160ParserTests.AddRange(SqlAzure160_TestInfos);

            foreach (ParserTest ti in all160ParserTests)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result90);
            }
        }

        [TestMethod] 
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql160SyntaxIn80ParserTest()
        {
            TSql80Parser parser = new TSql80Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql80);
            System.Collections.Generic.List<ParserTest> all160ParserTests = new System.Collections.Generic.List<ParserTest>();
            all160ParserTests.AddRange(Only160TestInfos);
            all160ParserTests.AddRange(SqlAzure160_TestInfos);

            foreach (ParserTest ti in all160ParserTests)
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
        public void TSql150SyntaxIn160ParserTest()
        {
            TSql160Parser parser = new TSql160Parser(true, SqlEngineType.Standalone);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql150);
            foreach (ParserTest ti in Only140TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result150);
            }
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql140SyntaxIn160ParserTest()
        {
            TSql160Parser parser = new TSql160Parser(true, SqlEngineType.Standalone);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql140);
            foreach (ParserTest ti in Only140TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result140);
            }
        }

        /// <summary>
        /// Let's make sure the older syntax tests still pass in the new parser
        /// </summary>
        [TestMethod] 
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql130SyntaxIn160ParserTest()
        {
            TSql160Parser parser = new TSql160Parser(true, SqlEngineType.Standalone);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql130);
            foreach (ParserTest ti in Only130TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result130);
            }
        }

        [TestMethod] 
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql120SyntaxIn160ParserTest()
        {
            TSql160Parser parser = new TSql160Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql120);
            foreach (ParserTest ti in Only120TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result120);
            }
        }

        [TestMethod] 
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql110SyntaxIn160ParserTest()
        {
            TSql160Parser parser = new TSql160Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql110);
            foreach (ParserTest ti in Only110TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result110);
            }
        }

        [TestMethod] 
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql100SyntaxIn160ParserTest()
        {
            TSql160Parser parser = new TSql160Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql100);
            foreach (ParserTest ti in Only100TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result100);
            }
        }
    }
}
