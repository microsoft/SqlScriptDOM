//------------------------------------------------------------------------------
// <copyright file="Sql150ExternalDataSourceParserTest.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using Microsoft.SqlServer.TransactSql.ScriptDom;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlStudio.Tests.AssemblyTools.TestCategory;
using System;
using System.Collections.Generic;
using System.IO;

namespace SqlStudio.Tests.UTSqlScriptDom
{

    /// <summary>
    /// Verify Supported Syntax for create external data source in SQL19
    /// </summary>
    [TestClass]
    public class Sql150ExternalDataSourceParserTest
    {
        [TestMethod]
        [Priority(1)]
        [Timeout(GlobalConstants.DefaultTestTimeout)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void CreateExternalGenericDataSourceStatementTest()
        {
            string createDataSourceScript = @"
                         CREATE EXTERNAL DATA SOURCE ExternalGenerics_DataPoolSource
                         WITH
                         (LOCATION = 'sqldatapool://controller-svc/default')";

            TSqlStatement statement = createTSqlStatement(createDataSourceScript);
            CreateExternalDataSourceStatement createExternalDataSourceStatement = (CreateExternalDataSourceStatement)statement;

            Assert.AreEqual(ExternalDataSourceType.EXTERNAL_GENERICS, createExternalDataSourceStatement.DataSourceType);
            Assert.IsNull(createExternalDataSourceStatement.PushdownOption);
        }

        [TestMethod]
        [Priority(1)]
        [Timeout(GlobalConstants.DefaultTestTimeout)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void CreateExternalGenericDataSourceStatementWithCredentialProvidedTest()
        {
            string credentialName = "testCredentialName";
            string createDataSourceScript = String.Format(@"
                         CREATE EXTERNAL DATA SOURCE ConnectionOption_DataSource
                         WITH (
                            LOCATION = N'sqlserver://WINSQL2019',
                            CREDENTIAL = {0}
                   )", credentialName);

            TSqlStatement statement = createTSqlStatement(createDataSourceScript);
            CreateExternalDataSourceStatement createExternalDataSourceStatement = (CreateExternalDataSourceStatement)statement;

            Assert.AreEqual(ExternalDataSourceType.EXTERNAL_GENERICS, createExternalDataSourceStatement.DataSourceType);
            Assert.IsNull(createExternalDataSourceStatement.PushdownOption);

            IList<ExternalDataSourceOption> externalDataSourceOptions = createExternalDataSourceStatement.ExternalDataSourceOptions;
            Assert.AreEqual(1, externalDataSourceOptions.Count);

            ExternalDataSourceLiteralOrIdentifierOption option = (ExternalDataSourceLiteralOrIdentifierOption)externalDataSourceOptions[0];
            Assert.AreEqual(ExternalDataSourceOptionKind.Credential, option.OptionKind);
            Assert.AreEqual(credentialName, option.Value.Value);
        }

        [TestMethod]
        [Priority(1)]
        [Timeout(GlobalConstants.DefaultTestTimeout)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void CreateHadoopDataSourceStatementTest()
        {
            string createDataSourceScript = @"
                         CREATE EXTERNAL DATA SOURCE Hadoop_DataPoolSource
                         WITH
                         (
                             TYPE = HADOOP,
                             LOCATION = 'hdfs://10.10.10.10:8050'
                         )";

            TSqlStatement statement = createTSqlStatement(createDataSourceScript);
            CreateExternalDataSourceStatement createExternalDataSourceStatement = (CreateExternalDataSourceStatement)statement;

            Assert.AreEqual(ExternalDataSourceType.HADOOP, createExternalDataSourceStatement.DataSourceType);
        }

        [TestMethod]
        [Priority(1)]
        [Timeout(GlobalConstants.DefaultTestTimeout)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void CreateDataSourceStatementConnectionOptionsTest()
        {
            string connectionOptions = @"Server=%s\SQL2019";
            string createDataSourceScript = String.Format(@"
                         CREATE EXTERNAL DATA SOURCE ConnectionOption_DataSource
                         WITH (
                            LOCATION = N'sqlserver://WINSQL2019',
                            CONNECTION_OPTIONS = N'{0}'
                   )", connectionOptions);

            TSqlStatement statement = createTSqlStatement(createDataSourceScript);
            CreateExternalDataSourceStatement createExternalDataSourceStatement = (CreateExternalDataSourceStatement)statement;

            Assert.AreEqual(ExternalDataSourceType.EXTERNAL_GENERICS, createExternalDataSourceStatement.DataSourceType);
            Assert.IsNull(createExternalDataSourceStatement.PushdownOption);

            IList<ExternalDataSourceOption> externalDataSourceOptions = createExternalDataSourceStatement.ExternalDataSourceOptions;
            Assert.AreEqual(1, externalDataSourceOptions.Count);

            ExternalDataSourceLiteralOrIdentifierOption option = (ExternalDataSourceLiteralOrIdentifierOption)externalDataSourceOptions[0];
            Assert.AreEqual(ExternalDataSourceOptionKind.ConnectionOptions, option.OptionKind);
            Assert.AreEqual(connectionOptions, option.Value.Value);
        }

        [TestMethod]
        [Priority(1)]
        [Timeout(GlobalConstants.DefaultTestTimeout)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void CreateDataSourceStatementPushdownOptionTest()
        {
            string createDataSourceScript = @"
                         CREATE EXTERNAL DATA SOURCE ConnectionOption_DataSource
                         WITH (
                            PUSHDOWN = OFF,
                            LOCATION = N'sqlserver://WINSQL2019')";

            TSqlStatement statement = createTSqlStatement(createDataSourceScript);
            CreateExternalDataSourceStatement createExternalDataSourceStatement = (CreateExternalDataSourceStatement)statement;

            Assert.AreEqual(ExternalDataSourceType.EXTERNAL_GENERICS, createExternalDataSourceStatement.DataSourceType);
            Assert.AreEqual(ExternalDataSourcePushdownOption.OFF, createExternalDataSourceStatement.PushdownOption);
        }

        [TestMethod]
        [Priority(1)]
        [Timeout(GlobalConstants.DefaultTestTimeout)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void CreateDataSourceStatementConnectionOptionsPushdownOptionOnTest()
        {
            string connectionOptions = @"Server=%s\SQL2019";
            string createDataSourceScript = String.Format(@"
                         CREATE EXTERNAL DATA SOURCE ConnectionOption_PushdownOn_DataSource
                         WITH (
                            LOCATION = N'sqlserver://WINSQL2019',
                            PUSHDOWN = ON,
                            CONNECTION_OPTIONS = N'{0}'
                   )", connectionOptions);

            TSqlStatement statement = createTSqlStatement(createDataSourceScript);
            CreateExternalDataSourceStatement createExternalDataSourceStatement = (CreateExternalDataSourceStatement)statement;

            Assert.AreEqual(ExternalDataSourceType.EXTERNAL_GENERICS, createExternalDataSourceStatement.DataSourceType);
            Assert.AreEqual(ExternalDataSourcePushdownOption.ON, createExternalDataSourceStatement.PushdownOption);

            IList<ExternalDataSourceOption> externalDataSourceOptions = createExternalDataSourceStatement.ExternalDataSourceOptions;
            Assert.AreEqual(1, externalDataSourceOptions.Count);

            ExternalDataSourceLiteralOrIdentifierOption option = (ExternalDataSourceLiteralOrIdentifierOption)externalDataSourceOptions[0];
            Assert.AreEqual(ExternalDataSourceOptionKind.ConnectionOptions, option.OptionKind);
            Assert.AreEqual(connectionOptions, option.Value.Value);
        }

        [TestMethod]
        [Priority(1)]
        [Timeout(GlobalConstants.DefaultTestTimeout)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void CreateDataSourceStatementConnectionOptionsPushdownOptionOffTest()
        {
            string connectionOptions = @"Server=%s\SQL2019";
            string createDataSourceScript = String.Format(@"
                         CREATE EXTERNAL DATA SOURCE ConnectionOption_PushdownOff_DataSource
                         WITH (
                            LOCATION = N'sqlserver://WINSQL2019',
                            PUSHDOWN = OFF,
                            CONNECTION_OPTIONS = N'{0}'
                   )", connectionOptions);

            TSqlStatement statement = createTSqlStatement(createDataSourceScript);
            CreateExternalDataSourceStatement createExternalDataSourceStatement = (CreateExternalDataSourceStatement)statement;

            Assert.AreEqual(ExternalDataSourceType.EXTERNAL_GENERICS, createExternalDataSourceStatement.DataSourceType);
            Assert.AreEqual(ExternalDataSourcePushdownOption.OFF, createExternalDataSourceStatement.PushdownOption);

            IList<ExternalDataSourceOption> externalDataSourceOptions = createExternalDataSourceStatement.ExternalDataSourceOptions;
            Assert.AreEqual(1, externalDataSourceOptions.Count);

            ExternalDataSourceLiteralOrIdentifierOption option = (ExternalDataSourceLiteralOrIdentifierOption)externalDataSourceOptions[0];
            Assert.AreEqual(ExternalDataSourceOptionKind.ConnectionOptions, option.OptionKind);
            Assert.AreEqual(connectionOptions, option.Value.Value);
        }

        [TestMethod]
        [Priority(1)]
        [Timeout(GlobalConstants.DefaultTestTimeout)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void AlterDataSourceStatementConnectionOptionsTest()
        {
            string connectionOptions = @"Server=%s\SQL2019";
            string alterDataSourceScript = String.Format(@"
                         ALTER EXTERNAL DATA SOURCE ConnectionOption_PushdownOff_DataSource
                         SET CONNECTION_OPTIONS = N'{0}'", connectionOptions);

            TSqlStatement statement = createTSqlStatement(alterDataSourceScript);
            AlterExternalDataSourceStatement alterExternalDataSourceStatement = (AlterExternalDataSourceStatement)statement;

            Assert.IsNull(alterExternalDataSourceStatement.PushdownOption);

            IList<ExternalDataSourceOption> externalDataSourceOptions = alterExternalDataSourceStatement.ExternalDataSourceOptions;
            Assert.AreEqual(1, externalDataSourceOptions.Count);

            ExternalDataSourceLiteralOrIdentifierOption option = (ExternalDataSourceLiteralOrIdentifierOption)externalDataSourceOptions[0];
            Assert.AreEqual(ExternalDataSourceOptionKind.ConnectionOptions, option.OptionKind);
            Assert.AreEqual(connectionOptions, option.Value.Value);
        }

        [TestMethod]
        [Priority(1)]
        [Timeout(GlobalConstants.DefaultTestTimeout)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void AlterDataSourceStatementPushdownOptionTest()
        {
            string alterDataSourceScript = @"
                         ALTER EXTERNAL DATA SOURCE ConnectionOption_PushdownOff_DataSource
                         SET PUSHDOWN = OFF";

            TSqlStatement statement = createTSqlStatement(alterDataSourceScript);
            AlterExternalDataSourceStatement alterExternalDataSourceStatement = (AlterExternalDataSourceStatement)statement;

            Assert.AreEqual(ExternalDataSourcePushdownOption.OFF, alterExternalDataSourceStatement.PushdownOption);
        }

        [TestMethod]
        [Priority(1)]
        [Timeout(GlobalConstants.DefaultTestTimeout)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void AlterDataSourceStatementConnectionOptionsPushdownOnTest()
        {
            string connectionOptions = @"Server=%s\SQL2019";
            string alterDataSourceScript = String.Format(@"
                         ALTER EXTERNAL DATA SOURCE ConnectionOption_PushdownOff_DataSource
                         SET CONNECTION_OPTIONS = N'{0}', PUSHDOWN = ON", connectionOptions);

            TSqlStatement statement = createTSqlStatement(alterDataSourceScript);
            AlterExternalDataSourceStatement alterExternalDataSourceStatement = (AlterExternalDataSourceStatement)statement;

            Assert.AreEqual(ExternalDataSourcePushdownOption.ON, alterExternalDataSourceStatement.PushdownOption);

            IList<ExternalDataSourceOption> externalDataSourceOptions = alterExternalDataSourceStatement.ExternalDataSourceOptions;
            Assert.AreEqual(1, externalDataSourceOptions.Count);

            ExternalDataSourceLiteralOrIdentifierOption option = (ExternalDataSourceLiteralOrIdentifierOption)externalDataSourceOptions[0];
            Assert.AreEqual(ExternalDataSourceOptionKind.ConnectionOptions, option.OptionKind);
            Assert.AreEqual(connectionOptions, option.Value.Value);
        }

        [TestMethod]
        [Priority(1)]
        [Timeout(GlobalConstants.DefaultTestTimeout)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void AlterDataSourceStatementConnectionOptionsPushdownOffTest()
        {
            string connectionOptions = @"Server=%s\SQL2019";
            string alterDataSourceScript = String.Format(@"
                         ALTER EXTERNAL DATA SOURCE ConnectionOption_PushdownOff_DataSource
                         SET CONNECTION_OPTIONS = N'{0}', PUSHDOWN = OFF", connectionOptions);

            TSqlStatement statement = createTSqlStatement(alterDataSourceScript);
            AlterExternalDataSourceStatement alterExternalDataSourceStatement = (AlterExternalDataSourceStatement)statement;

            Assert.AreEqual(ExternalDataSourcePushdownOption.OFF, alterExternalDataSourceStatement.PushdownOption);

            IList<ExternalDataSourceOption> externalDataSourceOptions = alterExternalDataSourceStatement.ExternalDataSourceOptions;
            Assert.AreEqual(1, externalDataSourceOptions.Count);

            ExternalDataSourceLiteralOrIdentifierOption option = (ExternalDataSourceLiteralOrIdentifierOption)externalDataSourceOptions[0];
            Assert.AreEqual(ExternalDataSourceOptionKind.ConnectionOptions, option.OptionKind);
            Assert.AreEqual(connectionOptions, option.Value.Value);
        }

        private TSqlStatement createTSqlStatement(string script)
        {
            TSql150Parser parser = new TSql150Parser(initialQuotedIdentifiers: true);

            IList<ParseError> parseErrors;
            TSqlScript tsqlScript = null;

            using (StringReader reader = new StringReader(script))
            {
                tsqlScript = (TSqlScript)parser.Parse(reader, out parseErrors);
            }

            if (parseErrors != null && parseErrors.Count > 0)
            {
                foreach (ParseError parseError in parseErrors)
                {
                    Console.WriteLine("PARSE ERROR: {0}, Line={0}, Column={1}, ErrorCode={2}", parseError.Message, parseError.Line, parseError.Column, parseError.Number);
                }

                Assert.Fail("One or more parser errors found.");
            }

            Assert.AreEqual(1, tsqlScript.Batches.Count);
            Assert.AreEqual(1, tsqlScript.Batches[0].Statements.Count);
            return tsqlScript.Batches[0].Statements[0];
        }

    }
}