//------------------------------------------------------------------------------
// <copyright file="Only140SyntaxTests.cs" company="Microsoft">
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
        private static readonly ParserTest[] Only140TestInfos = 
        {
            new ParserTest140("AlterIndexStatementTests140.sql", 2, 20, 20, 20, 20, 20),
            new ParserTest140("AlterDatabaseOptionsTests140.sql", 18, 18, 18, 18, 18, 18),
            new ParserTest140("CreateIndexStatementTests140.sql", 18, 14, 14, 14, 14, 14),
            new ParserTest140("CreateTableTests140.sql", 17, 15, 15, 15, 15, 11),
            new ParserTest140("OptimizerHintsTests140.sql", 6, 6, 6, 6, 6, 0),
            new ParserTest140("WithinGroupTests140.sql", 4, 4, 4, 2, 2, 0),
            new ParserTest140("TrimBuiltInTest140.sql", 10, 2, 10, 10, 10, 10),
            new ParserTest140("AlterTableAlterColumnStatementTests140.sql", 14, 14, 14, 14, 14, 12),
            new ParserTest140("BulkInsertStatementTests140.sql", 12, 12, 12, 12, 12, 12),
            new ParserTest140("OpenRowsetBulkStatementTests140.sql", 9, 9, 9, 9, 9, 9),
            new ParserTest140("GraphDbSyntaxTests140.sql", 28, 28, 28, 28, 28, 28),
            new ParserTest140("AlterDatabaseScopedGenericConfigurationTests140.sql", 33, 33, 33, 33, 33, 11),
            new ParserTest140("AlterTableDropTableElementStatementTests140.sql", 1, 5, 5, 5, 5, 5),
            new ParserTest140("DropIndexStatementTests140.sql", 4, 4, 4, 4, 4, 4),
            new ParserTest140("SelectStatementTests140.sql", 8, 8, 8, 8, 8, 1),
            new ParserTest140("CreateDatabaseTests140.sql", 3, 3, 3, 3, 3, 3),
            new ParserTest140("CreateExternalDataSource140.sql", 2, 2, 2, 2, 2, 4),
            new ParserTest140("GroupByClauseTests140.sql", 4, 4, 2, 2, 2, 0),
            new ParserTest140("DataClassificationTests140.sql", 1, 4, 4, 4, 4, 0),
            new ParserTest140("AlterExternalLibrary140.sql", 2, 2, 2, 2, 2, 3),
            new ParserTest140("CreateExternalLibrary140.sql", 2, 2, 2, 2, 2, 3),
            new ParserTest140("DropExternalLibrary140.sql", 2, 2, 2, 2, 2, 2),
            new ParserTest140("MaterializedViewTests140.sql", 6, 3, 3, 3, 3, 0),
        };

        private static readonly ParserTest[] SqlAzure_TestInfos = 
        {
            // Add parser tests that have Azure-specific syntax constructs
            // (those that have versioning visitor implemented)
            //
            new ParserTest140("AlterTableStatementTests140_Azure.sql", 2, 2, 2, 2, 2, 2),
            new ParserTest140("AlterDatabaseOptionsTests140_Azure.sql", 2, 2, 2, 2, 2, 2),
            new ParserTest140("CreateTableTests140_Azure.sql", 9, 9, 9, 9, 9, 9),
            new ParserTest140("RestoreStatementTests140_Azure.sql", 5, 5, 5, 5, 5, 5)
        };

        [TestMethod] 
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql140SyntaxIn140ParserTest()
        {
            // Parsing both for Azure and standalone flavors
            //
            TSql140Parser parser = new TSql140Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql140);
            foreach (ParserTest ti in Only140TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result140);
            }

            // Parsing both for Azure and standalone flavors - explicitly set
            // enum value to 'All'
            //
            parser = new TSql140Parser(true, SqlEngineType.All);
            scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql140);
            scriptGen.Options.SqlEngineType = SqlEngineType.All;
            foreach (ParserTest ti in Only140TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result140);
            }

            // Explicitly ask for 'standalone' and parse
            //
            parser = new TSql140Parser(true, SqlEngineType.Standalone);
            scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql140);
            scriptGen.Options.SqlEngineType = SqlEngineType.Standalone;
            foreach (ParserTest ti in Only140TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result140);
            }
        }

        [TestMethod] 
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql140Azure_SyntaxIn140ParserTest()
        {
            // This test checks that Azure-specific syntax constructs are successfully parsed
            // with Sql140 parser pointed to Sql Azure.
            //
            TSql140Parser parser = new TSql140Parser(true, SqlEngineType.SqlAzure);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql140);
            scriptGen.Options.SqlEngineType = SqlEngineType.SqlAzure;
            foreach (ParserTest ti in SqlAzure_TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result140);
            }
        }

        [TestMethod] 
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql140SyntaxIn130ParserTest()
        {
            TSql130Parser parser = new TSql130Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql130);
            System.Collections.Generic.List<ParserTest> all140ParserTests = new System.Collections.Generic.List<ParserTest>();
            all140ParserTests.AddRange(Only140TestInfos);
            all140ParserTests.AddRange(SqlAzure_TestInfos);

            foreach (ParserTest ti in all140ParserTests)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result130);
            }
        }

        [TestMethod] 
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql140SyntaxIn120ParserTest()
        {
            TSql120Parser parser = new TSql120Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql120);
            System.Collections.Generic.List<ParserTest> all140ParserTests = new System.Collections.Generic.List<ParserTest>();
            all140ParserTests.AddRange(Only140TestInfos);
            all140ParserTests.AddRange(SqlAzure_TestInfos);

            foreach (ParserTest ti in all140ParserTests)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result120);
            }
        }


        [TestMethod] 
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql140SyntaxIn110ParserTest()
        {
            TSql110Parser parser = new TSql110Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql110);
            System.Collections.Generic.List<ParserTest> all140ParserTests = new System.Collections.Generic.List<ParserTest>();
            all140ParserTests.AddRange(Only140TestInfos);
            all140ParserTests.AddRange(SqlAzure_TestInfos);

            foreach (ParserTest ti in all140ParserTests)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result110);
            }
        }

        [TestMethod] 
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql140SyntaxIn100ParserTest()
        {
            TSql100Parser parser = new TSql100Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql100);
            System.Collections.Generic.List<ParserTest> all140ParserTests = new System.Collections.Generic.List<ParserTest>();
            all140ParserTests.AddRange(Only140TestInfos);
            all140ParserTests.AddRange(SqlAzure_TestInfos);

            foreach (ParserTest ti in all140ParserTests)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result100);
            }
        }

        [TestMethod] 
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql140SyntaxIn90ParserTest()
        {
            TSql90Parser parser = new TSql90Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql90);
            System.Collections.Generic.List<ParserTest> all140ParserTests = new System.Collections.Generic.List<ParserTest>();
            all140ParserTests.AddRange(Only140TestInfos);
            all140ParserTests.AddRange(SqlAzure_TestInfos);

            foreach (ParserTest ti in all140ParserTests)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result90);
            }
        }

        [TestMethod] 
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql140SyntaxIn80ParserTest()
        {
            TSql80Parser parser = new TSql80Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql80);
            System.Collections.Generic.List<ParserTest> all140ParserTests = new System.Collections.Generic.List<ParserTest>();
            all140ParserTests.AddRange(Only140TestInfos);
            all140ParserTests.AddRange(SqlAzure_TestInfos);

            foreach (ParserTest ti in all140ParserTests)
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
        public void TSql130SyntaxIn140ParserTest()
        {
            TSql140Parser parser = new TSql140Parser(true, SqlEngineType.Standalone);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql130);
            foreach (ParserTest ti in Only130TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result130);
            }
        }

        [TestMethod] 
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql120SyntaxIn140ParserTest()
        {
            TSql140Parser parser = new TSql140Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql120);
            foreach (ParserTest ti in Only120TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result120);
            }
        }

        [TestMethod] 
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql110SyntaxIn140ParserTest()
        {
            TSql140Parser parser = new TSql140Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql110);
            foreach (ParserTest ti in Only110TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result110);
            }
        }

        [TestMethod] 
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TSql100SyntaxIn140ParserTest()
        {
            TSql140Parser parser = new TSql140Parser(true);
            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql100);
            foreach (ParserTest ti in Only100TestInfos)
            {
                ParserTest.ParseAndVerify(parser, scriptGen, ti._scriptFilename, ti._result100);
            }
        }
    }
}
