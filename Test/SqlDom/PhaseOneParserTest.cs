//------------------------------------------------------------------------------
// <copyright file="PhaseOneParserTest.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using SqlStudio.Tests.AssemblyTools.TestCategory;

namespace SqlStudio.Tests.UTSqlScriptDom
{
    /// <summary>
    /// This class is used in testing the TSqlParser.
    /// </summary>
    public partial class SqlDomTests
    {
        private void CheckCommonAlterTablePart(AlterTableStatement statement)
        {
            Assert.AreEqual<string>("master", statement.SchemaObjectName.DatabaseIdentifier.Value);
            Assert.AreEqual<string>("dbo", statement.SchemaObjectName.SchemaIdentifier.Value);
            Assert.AreEqual<string>("t1", statement.SchemaObjectName.BaseIdentifier.Value);
        }

        void CheckIdentifier(Identifier identifier, string expected)
        {
            if (expected == null)
                Assert.IsNull(identifier);
            else
                Assert.AreEqual<string>(expected, identifier.Value);
        }

        private void CheckSchemaObjectName(TSqlFragment tSqlFragment, string schemaIdentifier, string baseIdentifier)
        {
            CheckSchemaObjectName(tSqlFragment, schemaIdentifier, baseIdentifier, null, null);
        }

        private void CheckSchemaObjectName(TSqlFragment tSqlFragment, string schemaIdentifier, string baseIdentifier, string databaseIdentifier, string serverIdentifier)
        {
            SchemaObjectName objName = tSqlFragment as SchemaObjectName;
            Assert.IsNotNull(objName);
            CheckIdentifier(objName.BaseIdentifier, baseIdentifier);
            CheckIdentifier(objName.SchemaIdentifier, schemaIdentifier);
            CheckIdentifier(objName.DatabaseIdentifier, databaseIdentifier);
            CheckIdentifier(objName.ServerIdentifier, serverIdentifier);
        }

        delegate void PhaseOneResultVerifier<T>(T statement)
            where T : TSqlStatement;

        private T PhaseOneParse<T>(TSqlParser parser, string resourceName)
            where T : TSqlStatement
        {
            using (StreamReader reader = ParserTestUtils.GetStreamReaderFromManifestResource(GlobalConstants.PhaseOneTestScriptsFilesNameSpace + "." + resourceName))
            {
                TSqlStatement statement = parser.PhaseOneParse(reader);
                return (statement as T);
            }
        }

        const string InvalidPrefix = "This is not a valid TSql Statement.\r\n\r\n";

        private T PhaseOneParseString<T>(TSqlParser parser, string statementStart)
            where T : TSqlStatement
        {
            using (StringReader reader = new StringReader(InvalidPrefix + statementStart))
            {
                TSqlStatement statement = parser.PhaseOneParse(reader);
                return (statement as T);
            }
        }

        private void PhaseOneAllParserTest<T>(PhaseOneResultVerifier<T> verifier, string resourceName)
            where T : TSqlStatement
        {
            ParserTestUtils.ExecuteTestForAllParsers(delegate(TSqlParser parser)
            {
                T statement = PhaseOneParse<T>(parser, resourceName);
                Assert.IsNotNull(statement);
                verifier(statement);
            }, true);
        }

        private void PhaseOne90Test<T>(PhaseOneResultVerifier<T> verifier, string resourceName)
            where T : TSqlStatement
        {
            TSql80Parser parser80 = new TSql80Parser(true);
            T statement = PhaseOneParse<T>(parser80, resourceName);
            Assert.IsNull(statement);

            ParserTestUtils.ExecuteTestForParsers(delegate(TSqlParser parser)
            {
                statement = PhaseOneParse<T>(parser, resourceName);
                Assert.IsNotNull(statement);
                verifier(statement);
            }, new TSql90Parser(true), new TSql100Parser(true));
        }

        private void PhaseOne100Test<T>(PhaseOneResultVerifier<T> verifier, string resourceName)
            where T : TSqlStatement
        {
            TSql100Parser parser100 = new TSql100Parser(true);
            T statement = PhaseOneParse<T>(parser100, resourceName);
            Assert.IsNotNull(statement);
            verifier(statement);
       }

        /// <summary>
        /// Executes a phase one, 150 test for the statement type.
        /// </summary>
        /// <typeparam name="T">The statement type.</typeparam>
        /// <param name="verifier">The phase one verifier to use.</param>
        /// <param name="resourceName">The resource name.</param>
        private void PhaseOne150Test<T>(PhaseOneResultVerifier<T> verifier, string resourceName)
            where T : TSqlStatement
        {
            TSql150Parser parser150 = new TSql150Parser(true);
            T statement = PhaseOneParse<T>(parser150, resourceName);
            Assert.IsNotNull(statement);
            verifier(statement);
        }

        private void PhaseOne130Test<T>(PhaseOneResultVerifier<T> verifier, string resourceName)
            where T : TSqlStatement
        {
            TSql130Parser parser130 = new TSql130Parser(true);
            T statement = PhaseOneParse<T>(parser130, resourceName);
            Assert.IsNotNull(statement);
            verifier(statement);
        }

        private void PhaseOne100TestString<T>(PhaseOneResultVerifier<T> verifier, string resourceName)
            where T : TSqlStatement
        {
            TSql100Parser parser100 = new TSql100Parser(true);
            T statement = PhaseOneParseString<T>(parser100, resourceName);
            Assert.IsNotNull(statement);
            verifier(statement);
        }

        private void SqlModuleObjectNameAllTest(string schemaIdentifier, string baseIdentifier, string resourceName)
        {
            ParserTestUtils.ExecuteTestForAllParsers(delegate(TSqlParser parser)
            {
                using (StreamReader reader = ParserTestUtils.GetStreamReaderFromManifestResource(GlobalConstants.PhaseOneTestScriptsFilesNameSpace + "." + resourceName))
                {
                    SchemaObjectName result;
                    Assert.IsTrue(parser.TryParseSqlModuleObjectName(reader, out result));
                    CheckSchemaObjectName(result, schemaIdentifier, baseIdentifier);
                }
            }, true);
        }

        private void SqlModuleObjectName90PlusTest(string schemaIdentifier, string baseIdentifier, string resourceName)
        {
            ParserTestUtils.ExecuteTestForParsers(delegate(TSqlParser parser)
            {
                using (StreamReader reader = ParserTestUtils.GetStreamReaderFromManifestResource(GlobalConstants.PhaseOneTestScriptsFilesNameSpace + "." + resourceName))
                {
                    SchemaObjectName result;
                    Assert.IsTrue(parser.TryParseSqlModuleObjectName(reader, out result));
                    CheckSchemaObjectName(result, schemaIdentifier, baseIdentifier);
                }
            }, new TSql90Parser(true), new TSql100Parser(true), new TSql110Parser(true));
        }

        private void TriggerTargetObjectNameAllTest(string schemaIdentifier, string baseIdentifier, string targetSchemaIdentifier, string targetBaseIdentifier, string resourceName)
        {
            ParserTestUtils.ExecuteTestForAllParsers(delegate(TSqlParser parser)
            {
                using (StreamReader reader = ParserTestUtils.GetStreamReaderFromManifestResource(GlobalConstants.PhaseOneTestScriptsFilesNameSpace + "." + resourceName))
                {
                    SchemaObjectName result;
                    SchemaObjectName triggerTarget;
                    Assert.IsTrue(parser.TryParseTriggerModule(reader, out result, out triggerTarget));
                    CheckSchemaObjectName(result, schemaIdentifier, baseIdentifier);
                    CheckSchemaObjectName(triggerTarget, targetSchemaIdentifier, targetBaseIdentifier);
                }
            }, true);
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]
        public void AlterApplicationRoleTest()
        {
            PhaseOne90Test(delegate(AlterApplicationRoleStatement statement)
                {
                    Assert.AreEqual<string>("r1", statement.Name.Value);
                }, "AlterApplicationRoleTest.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void AlterAssemblyTest()
        {
            PhaseOne90Test(delegate(AlterAssemblyStatement st)
                {
                    Assert.AreEqual<string>("a1", st.Name.Value);
                }, "AlterAssemblyTest.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void AlterAsymmetricKeyTest()
        {
            PhaseOne90Test(delegate(AlterAsymmetricKeyStatement st)
                {
                    Assert.AreEqual<string>("ask1", st.Name.Value);
                }, "AlterAssymetricKeyTest.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void AlterRemoteServiceBindingTest()
        {
            PhaseOne90Test(delegate(AlterRemoteServiceBindingStatement st)
                {
                    Assert.AreEqual<string>("b1", st.Name.Value);
                }, "AlterRemoteServiceBindingTest.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void AlterCertificateTest()
        {
            PhaseOne90Test(delegate(AlterCertificateStatement st)
                {
                    Assert.AreEqual<string>("cert1", st.Name.Value);
                }, "AlterCertificateTest.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void AlterCredentialTest()
        {
            PhaseOne90Test(delegate(AlterCredentialStatement st)
                {
                    Assert.AreEqual<string>("cred1", st.Name.Value);
                }, "AlterCredentialTest.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void AlterEndpointTest()
        {
            PhaseOne90Test(delegate(AlterEndpointStatement st)
                {
                    Assert.AreEqual<string>("ep1", st.Name.Value);
                }, "AlterEndpointTest.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void AlterFulltextIndexTest()
        {
            PhaseOne90Test(delegate(AlterFullTextIndexStatement st)
                {
                    CheckSchemaObjectName(st.OnName, "HumanResources", "JobCandidate");
                }, "AlterFulltextIndexTest.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void AlterFulltextCatalogTest()
        {
            PhaseOne90Test(delegate(AlterFullTextCatalogStatement st)
                {
                    Assert.AreEqual<string>("somecatalog", st.Name.Value);
                }, "AlterFulltextCatalogTest.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void AlterLoginTest()
        {
            PhaseOne90Test(delegate(AlterLoginStatement st)
                {
                    Assert.AreEqual<string>("l1", st.Name.Value);
                }, "AlterLoginTest.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void AlterMessageTypeTest()
        {
            PhaseOne90Test(delegate(AlterMessageTypeStatement st)
                {
                    Assert.AreEqual<string>("//Adventure-Works.com/Expenses/SubmitExpense", st.Name.Value);
                }, "AlterMessageTypeTest.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void AlterPartitionFunctionTest()
        {
            PhaseOne90Test(delegate(AlterPartitionFunctionStatement st)
                {
                    Assert.AreEqual<string>("func1", st.Name.Value);
                }, "AlterPartitionFunctionTest.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void AlterPartitionSchemeTest()
        {
            PhaseOne90Test(delegate(AlterPartitionSchemeStatement st)
                {
                    Assert.AreEqual<string>("scm1", st.Name.Value);
                }, "AlterPartitionSchemeTest.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void AlterQueueTest()
        {
            PhaseOne90Test(delegate(AlterQueueStatement st)
                {
                    CheckSchemaObjectName(st.Name, null, "ExpenseQueue");
                }, "AlterQueueTest.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void AlterRoleTest()
        {
            PhaseOne90Test(delegate(AlterRoleStatement st)
                {
                    Assert.AreEqual<string>("somerole", st.Name.Value);
                }, "AlterRoleTest.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void AlterRouteTest()
        {
            PhaseOne90Test(delegate(AlterRouteStatement st)
                {
                    Assert.AreEqual<string>("r1", st.Name.Value);
                }, "AlterRouteTest.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void AlterServiceTest()
        {
            PhaseOne90Test(delegate(AlterServiceStatement st)
                {
                    Assert.AreEqual<string>("s1", st.Name.Value);
                }, "AlterServiceTest.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void AlterServiceMasterKeyTest()
        {
            PhaseOne90Test(delegate(AlterServiceMasterKeyStatement st)
                {
                    Assert.IsNotNull(st);
                }, "AlterServiceMasterKeyTest.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void AlterSymmetricKeyTest()
        {
            PhaseOne90Test(delegate(AlterSymmetricKeyStatement st)
                {
                    Assert.AreEqual<string>("JanainaKey043", st.Name.Value);
                }, "AlterSymmetricKeyTest.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void AlterUserTest()
        {
            PhaseOne90Test(delegate(AlterUserStatement st)
                {
                    Assert.AreEqual<string>("u1", st.Name.Value);
                }, "AlterUserTest.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void AlterXmlSchemaCollectionTest()
        {
            PhaseOne90Test(delegate(AlterXmlSchemaCollectionStatement st)
                {
                    CheckSchemaObjectName(st.Name, null, "MyColl");
                }, "AlterXmlSchemaCollectionTest.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void AlterDatabaseWithSqlCommandIdentifier()
        {
            PhaseOneAllParserTest(delegate(AlterDatabaseAddFileGroupStatement statement)
                {
                    Assert.AreEqual<string>("$(tempdb)", statement.DatabaseName.Value);
                    Assert.AreEqual<string>("fg1", statement.FileGroup.Value);
                }, "AlterDatabaseWithSqlCommandIdentifier.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void AlterDatabaseAddFilegroupStatementTest()
        {
            PhaseOneAllParserTest(delegate(AlterDatabaseAddFileGroupStatement statement)
                {
                    Assert.AreEqual<string>("tempdb", statement.DatabaseName.Value);
                    Assert.AreEqual<string>("fg1", statement.FileGroup.Value);
                }, "AlterDatabaseAddFilegroupStatementTest.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void AlterDatabaseAddFileStatementTest()
        {
            PhaseOneAllParserTest(delegate(AlterDatabaseAddFileStatement statement)
                {
                    Assert.AreEqual<string>("tempdb", statement.DatabaseName.Value);
                    Assert.IsFalse(statement.IsLog);
                    Assert.AreEqual<int>(0, statement.FileDeclarations.Count);
                }, "AlterDatabaseAddFileStatementTest.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void AlterDatabaseAddLogFileStatementTest()
        {
            PhaseOneAllParserTest(delegate(AlterDatabaseAddFileStatement statement)
                {
                    Assert.AreEqual<string>("tempdb", statement.DatabaseName.Value);
                    Assert.IsTrue(statement.IsLog);
                    Assert.AreEqual<int>(0, statement.FileDeclarations.Count);
                }, "AlterDatabaseAddLogFileStatementTest.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void AlterDatabaseModifyFilegroup1StatementTest()
        {
            PhaseOneAllParserTest(delegate(AlterDatabaseModifyFileGroupStatement statement)
                {
                    Assert.IsNotNull(statement);
                    Assert.AreEqual("tempdb", statement.DatabaseName.Value);
                    Assert.AreEqual("fg1", statement.FileGroup.Value);
                }, "AlterDatabaseModifyFilegroup1StatementTest.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void AlterDatabaseModifyFilegroup2StatementTest()
        {
            PhaseOneAllParserTest(delegate(AlterDatabaseModifyFileGroupStatement statement)
                {
                    Assert.AreEqual("tempdb", statement.DatabaseName.Value);
                    Assert.AreEqual("fg1", statement.FileGroup.Value);
                    Assert.AreEqual("SomeIdent", statement.NewFileGroupName.Value);
                }, "AlterDatabaseModifyFilegroup2StatementTest.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void AlterDatabaseModifyFilegroup3StatementTest()
        {
            PhaseOneAllParserTest(delegate(AlterDatabaseModifyFileGroupStatement statement)
                {
                    Assert.AreEqual("tempdb", statement.DatabaseName.Value);
                    Assert.AreEqual("fg1", statement.FileGroup.Value);
                }, "AlterDatabaseModifyFilegroup3StatementTest.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void AlterDatabaseModifyFileStatementTest()
        {
            PhaseOneAllParserTest(delegate(AlterDatabaseModifyFileStatement statement)
                {
                    Assert.AreEqual("tempdb", statement.DatabaseName.Value);
                }, "AlterDatabaseModifyFileStatementTest.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void AlterDatabaseModifyNameStatementTest()
        {
            PhaseOneAllParserTest(delegate(AlterDatabaseModifyNameStatement statement)
                {
                    Assert.AreEqual<string>("tempdb", statement.DatabaseName.Value);
                    Assert.AreEqual<string>("newName", statement.NewDatabaseName.Value);
                }, "AlterDatabaseModifyNameStatementTest.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void AlterDatabaseRemoveFilegroupStatementTest()
        {
            PhaseOneAllParserTest(delegate(AlterDatabaseRemoveFileGroupStatement statement)
                {
                    Assert.AreEqual<string>("tempdb", statement.DatabaseName.Value);
                    Assert.AreEqual<string>("fg1", statement.FileGroup.Value);
                }, "AlterDatabaseRemoveFilegroupStatementTest.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void AlterDatabaseRemoveFileStatementTest()
        {
            PhaseOneAllParserTest(delegate(AlterDatabaseRemoveFileStatement statement)
                {
                    Assert.AreEqual<string>("tempdb", statement.DatabaseName.Value);
                    Assert.AreEqual<string>("file1", statement.File.Value);
                }, "AlterDatabaseRemoveFileStatementTest.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void AlterTableAddCheckConstraintTest()
        {
            PhaseOneAllParserTest(delegate(AlterTableAddTableElementStatement alterTable)
                {
                    CheckCommonAlterTablePart(alterTable);
                    Assert.AreEqual<int>(1, alterTable.Definition.TableConstraints.Count);
                    CheckConstraintDefinition constraint = alterTable.Definition.TableConstraints[0] as CheckConstraintDefinition;
                    Assert.IsNotNull(constraint);
                    Assert.AreEqual<string>("check1", constraint.ConstraintIdentifier.Value);
                }, "AlterTableAddCheckConstraintTest.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void AlterTableAddColumnDefinitionTest()
        {
            PhaseOneAllParserTest(delegate(AlterTableAddTableElementStatement alterTable)
                {
                    CheckCommonAlterTablePart(alterTable);
                    Assert.AreEqual<int>(1, alterTable.Definition.ColumnDefinitions.Count);
                    ColumnDefinition columnDefinition = alterTable.Definition.ColumnDefinitions[0];
                    Assert.AreEqual<string>("c1", columnDefinition.ColumnIdentifier.Value);
                }, "AlterTableAddColumnDefinitionTest.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void AlterTableAddDefaultConstraintTest()
        {
            PhaseOneAllParserTest(delegate(AlterTableAddTableElementStatement alterTable)
                {
                    CheckCommonAlterTablePart(alterTable);
                    Assert.AreEqual<int>(1, alterTable.Definition.TableConstraints.Count);
                    DefaultConstraintDefinition constraint = alterTable.Definition.TableConstraints[0] as DefaultConstraintDefinition;
                    Assert.IsNotNull(constraint);
                    Assert.AreEqual<string>("default1", constraint.ConstraintIdentifier.Value);
                }, "AlterTableAddDefaultConstraintTest.sql");
        }

 
        [TestMethod] 
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void AlterTableAddForeignKeyConstraintTest()
        {
            PhaseOneAllParserTest(delegate(AlterTableAddTableElementStatement alterTable)
                {
                    CheckCommonAlterTablePart(alterTable);
                    Assert.AreEqual<int>(1, alterTable.Definition.TableConstraints.Count);
                    ForeignKeyConstraintDefinition constraint = alterTable.Definition.TableConstraints[0] as ForeignKeyConstraintDefinition;
                    Assert.IsNotNull(constraint);
                    Assert.AreEqual<string>("fk1", constraint.ConstraintIdentifier.Value);
                }, "AlterTableAddForeignKeyConstraintTest.sql");
        }

        /// <summary>
        /// Verifies phase one for the alter table statement with edge constraints.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void AlterTableAddEdgeConstraintTest()
        {
            PhaseOne150Test(delegate(AlterTableAddTableElementStatement alterTable)
            {
                CheckCommonAlterTablePart(alterTable);
            },
            "AlterTableAddEdgeConstraintTest.sql");
        }

        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void AlterTableAddPrimaryKeyConstraintTest()
        {
            PhaseOneAllParserTest(delegate(AlterTableAddTableElementStatement alterTable)
                {
                    CheckCommonAlterTablePart(alterTable);
                    Assert.AreEqual<int>(1, alterTable.Definition.TableConstraints.Count);
                    UniqueConstraintDefinition constraint = alterTable.Definition.TableConstraints[0] as UniqueConstraintDefinition;
                    Assert.IsNotNull(constraint);
                    Assert.IsTrue(constraint.IsPrimaryKey);
                    Assert.AreEqual<string>("pk1", constraint.ConstraintIdentifier.Value);
                }, "AlterTableAddPrimaryKeyConstraintTest.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void AlterTableAddUniqueConstraintTest()
        {
            PhaseOneAllParserTest(delegate(AlterTableAddTableElementStatement alterTable)
                {
                    CheckCommonAlterTablePart(alterTable);
                    Assert.AreEqual<int>(1, alterTable.Definition.TableConstraints.Count);
                    UniqueConstraintDefinition constraint = alterTable.Definition.TableConstraints[0] as UniqueConstraintDefinition;
                    Assert.IsNotNull(constraint);
                    Assert.IsFalse(constraint.IsPrimaryKey);
                    Assert.AreEqual<string>("unique1", constraint.ConstraintIdentifier.Value);
                }, "AlterTableAddUniqueConstraintTest.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void AlterTableAlterColumnStatementTest()
        {
            PhaseOneAllParserTest(delegate(AlterTableAlterColumnStatement alterTable)
                {
                    CheckCommonAlterTablePart(alterTable);
                    Assert.AreEqual<string>("c1", alterTable.ColumnIdentifier.Value);
                }, "AlterTableAlterColumnStatementTest.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void AlterTableConstraintModificationStatementTest()
        {
            PhaseOneAllParserTest(delegate(AlterTableConstraintModificationStatement alterTable)
                {
                    CheckCommonAlterTablePart(alterTable);
                    Assert.AreEqual<ConstraintEnforcement>(ConstraintEnforcement.Check, alterTable.ConstraintEnforcement);
                    Assert.AreEqual<ConstraintEnforcement>(ConstraintEnforcement.Check, alterTable.ExistingRowsCheckEnforcement);
                    Assert.AreEqual<int>(1, alterTable.ConstraintNames.Count);
                    Assert.AreEqual<string>("fk1", alterTable.ConstraintNames[0].Value);
                }, "AlterTableConstraintModificationStatementTest.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void AlterTableDropTableElementStatementTest()
        {
            PhaseOneAllParserTest(delegate(AlterTableDropTableElementStatement alterTable)
                {
                    CheckCommonAlterTablePart(alterTable);
                    Assert.AreEqual<int>(1, alterTable.AlterTableDropTableElements.Count);
                    Assert.AreEqual<TableElementType>(TableElementType.Column, alterTable.AlterTableDropTableElements[0].TableElementType);
                    Assert.AreEqual<string>("c1", alterTable.AlterTableDropTableElements[0].Name.Value);
                }, "AlterTableDropTableElementStatementTest.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void AlterTableTriggerModificationStatementTest()
        {
            PhaseOneAllParserTest(delegate(AlterTableTriggerModificationStatement alterTable)
                {
                    CheckCommonAlterTablePart(alterTable);
                    Assert.AreEqual<TriggerEnforcement>(TriggerEnforcement.Enable, alterTable.TriggerEnforcement);
                    Assert.AreEqual<int>(1, alterTable.TriggerNames.Count);
                    Assert.AreEqual<string>("trigger1", alterTable.TriggerNames[0].Value);
                }, "AlterTableTriggerModificationStatementTest.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void AvoidingInfiniteLoopAtStatementLevelTest()
        {
            PhaseOneAllParserTest(delegate(TSqlStatement st)
                {
                }, "AvoidingInfiniteLoopAtStatementLevel.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void AvoidingInfiniteRecursionAtBatchLevelTest()
        {
            PhaseOneAllParserTest(delegate(TSqlStatement statement)
                {
                }, "AvoidingInfiniteRecursionAtBatchLevel.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void BackupStatementTest()
        {
            PhaseOneAllParserTest(delegate(BackupDatabaseStatement statement)
                {
                    Assert.AreEqual("d1", statement.DatabaseName.Value);
                }, "BackupStatementTest.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void BulkInsertStatementTest()
        {
            PhaseOneAllParserTest(delegate(BulkInsertStatement statement)
                {
                    CheckSchemaObjectName(statement.To, "db1", "t1");
                }, "BulkInsertTest.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]
        

        public void InsertBulkStatementTest()
        {
            PhaseOneAllParserTest(delegate(InsertBulkStatement statement)
                {
                    CheckSchemaObjectName(statement.To, null, "t1");
                }, "InsertBulkTest.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void RestoreStatementTest()
        {
            PhaseOneAllParserTest(delegate(RestoreStatement statement)
                {
                    Assert.AreEqual("d1", statement.DatabaseName.Value);
                    Assert.AreEqual(true, statement.Kind == RestoreStatementKind.Database);
                }, "RestoreStatementTest.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void CreateAggregate()
        {
            PhaseOne90Test(delegate(CreateAggregateStatement statement)
                {
                    CheckSchemaObjectName(statement.Name, "s1", "a1");
                }, "CreateAggregate.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]
        

        public void CreateApplicationRole()
        {
            PhaseOne90Test(delegate(CreateApplicationRoleStatement statement)
                {
                    Assert.AreEqual<string>("a1", statement.Name.Value);
                }, "CreateApplicationRole.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void CreateAssembly()
        {
            PhaseOne90Test(delegate(CreateAssemblyStatement statement)
                {
                    Assert.AreEqual<string>("a1", statement.Name.Value);
                }, "CreateAssembly.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void CreateAsymmetricKey()
        {
            PhaseOne90Test(delegate(CreateAsymmetricKeyStatement statement)
                {
                    Assert.AreEqual<string>("a1", statement.Name.Value);
                }, "CreateAsymmetricKey.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void CreateCertificate()
        {
            PhaseOne90Test(delegate(CreateCertificateStatement statement)
                {
                    Assert.AreEqual<string>("c1", statement.Name.Value);
                }, "CreateCertificate.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void CreateContract()
        {
            PhaseOne90Test(delegate(CreateContractStatement statement)
                {
                    Assert.AreEqual<string>("c1", statement.Name.Value);
                }, "CreateContract.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void CreateCredential()
        {
            PhaseOne90Test(delegate(CreateCredentialStatement statement)
                {
                    Assert.AreEqual<string>("c1", statement.Name.Value);
                }, "CreateCredential.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void CreateDatabase()
        {
            PhaseOneAllParserTest(delegate(CreateDatabaseStatement statement)
                {
                    Assert.AreEqual<string>("d1", statement.DatabaseName.Value);
                }, "CreateDatabase.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

		

		public void AlterDatabaseScopedCredential()
		{
			PhaseOne130Test(delegate(AlterCredentialStatement statement)
			{
				Assert.AreEqual<string>("a1", statement.Name.Value);
			}, "AlterDatabaseScopedCredential.sql");
		}

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

		

		public void CreateDatabaseScopedCredential()
		{
			PhaseOne130Test(delegate(CreateCredentialStatement statement)
			{
				Assert.AreEqual<string>("c1", statement.Name.Value);
			}, "CreateDatabaseScopedCredential.sql");
		}

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void CreateDefaultStatementTest()
        {
            PhaseOneAllParserTest(delegate(CreateDefaultStatement createDefault)
                {
                    Assert.AreEqual<string>("default1", createDefault.Name.BaseIdentifier.Value);
                }, "CreateDefaultStatementTest.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void CreateEndpoint()
        {
            PhaseOne90Test(delegate(CreateEndpointStatement statement)
                {
                    Assert.AreEqual<string>("e1", statement.Name.Value);
                }, "CreateEndpoint.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void CreateEventNotification()
        {
            PhaseOne90Test(delegate(CreateEventNotificationStatement statement)
                {
                    Assert.AreEqual<string>("e1", statement.Name.Value);
                }, "CreateEventNotification.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void CreateFulltextCatalog()
        {
            PhaseOne90Test(delegate(CreateFullTextCatalogStatement statement)
                {
                    Assert.AreEqual<string>("c1", statement.Name.Value);
                }, "CreateFulltextCatalog.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void CreateFulltextIndex()
        {
            PhaseOne90Test(delegate(CreateFullTextIndexStatement statement)
                {
                    CheckSchemaObjectName(statement.OnName, "dbo", "t1");
                }, "CreateFulltextIndex.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void CreateFunctionStatementTest()
        {
            PhaseOneAllParserTest(delegate(CreateFunctionStatement createFunction)
                {
                    Assert.AreEqual<string>("f1", createFunction.Name.BaseIdentifier.Value);
                }, "CreateFunctionStatementTest.sql");
            SqlModuleObjectNameAllTest("dbo", "f1", "CreateFunctionStatementTest.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void CreateIndexStatementTest()
        {
            PhaseOneAllParserTest(delegate(CreateIndexStatement createIndex)
                {
                    Assert.AreEqual<string>("index1", createIndex.Name.Value);
                    Assert.AreEqual<string>("t1", createIndex.OnName.BaseIdentifier.Value);
                }, "CreateIndexStatementTest.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void CreateLogin()
        {
            PhaseOne90Test(delegate(CreateLoginStatement statement)
                {
                    Assert.AreEqual<string>("l1", statement.Name.Value);
                }, "CreateLogin.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void CreateMessageType()
        {
            PhaseOne90Test(delegate(CreateMessageTypeStatement statement)
                {
                    Assert.AreEqual<string>("t1", statement.Name.Value);
                }, "CreateMessageType.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void CreatePartitionFunction()
        {
            PhaseOne90Test(delegate(CreatePartitionFunctionStatement statement)
                {
                    Assert.AreEqual<string>("f1", statement.Name.Value);
                }, "CreatePartitionFunction.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void CreatePartitionScheme()
        {
            PhaseOne90Test(delegate(CreatePartitionSchemeStatement statement)
                {
                    Assert.AreEqual<string>("s1", statement.Name.Value);
                }, "CreatePartitionScheme.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void CreatePrimaryXmlIndex()
        {
            PhaseOne90Test(delegate(CreateXmlIndexStatement statement)
                {
                    Assert.AreEqual<bool>(true, statement.Primary);
                    Assert.AreEqual<string>("i1", statement.Name.Value);
                    Assert.AreEqual<string>("t1", statement.OnName.BaseIdentifier.Value);
                }, "CreatePrimaryXmlIndex.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void CreateProcedureStatementTest()
        {
            PhaseOneAllParserTest(delegate(CreateProcedureStatement createProc)
                {
                    Assert.AreEqual<string>("p1", createProc.ProcedureReference.Name.BaseIdentifier.Value);
                }, "CreateProcedureStatementTest.sql");
            SqlModuleObjectNameAllTest("dbo", "p1", "CreateProcedureStatementTest.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void CreateQueue()
        {
            PhaseOne90Test(delegate(CreateQueueStatement statement)
                {
                    CheckSchemaObjectName(statement.Name, "sc", "t1");
                }, "CreateQueue.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void CreateRemoteServiceBinding()
        {
            PhaseOne90Test(delegate(CreateRemoteServiceBindingStatement statement)
                {
                    Assert.AreEqual<string>("r1", statement.Name.Value);
                }, "CreateRemoteServiceBinding.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void CreateRole()
        {
            PhaseOne90Test(delegate(CreateRoleStatement statement)
                {
                    Assert.AreEqual<string>("r1", statement.Name.Value);
                }, "CreateRole.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void CreateRoute()
        {
            PhaseOne90Test(delegate(CreateRouteStatement statement)
                {
                    Assert.AreEqual<string>("r1", statement.Name.Value);
                }, "CreateRoute.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void CreateRuleStatementTest()
        {
            PhaseOneAllParserTest(delegate(CreateRuleStatement createRule)
                {
                    Assert.AreEqual<string>("rule1", createRule.Name.BaseIdentifier.Value);
                }, "CreateRuleStatementTest.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void CreateSchemaStatementTest()
        {
            PhaseOneAllParserTest(delegate(CreateSchemaStatement createSchema)
                {
                    Assert.AreEqual<string>("sc1", createSchema.Name.Value);
                    Assert.AreEqual<string>("dbo", createSchema.Owner.Value);
                }, "CreateSchemaStatementTest.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void CreateService()
        {
            PhaseOne90Test(delegate(CreateServiceStatement statement)
                {
                    Assert.AreEqual<string>("s1", statement.Name.Value);
                }, "CreateService.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void CreateStatisticsStatementTest()
        {
            PhaseOneAllParserTest(delegate(CreateStatisticsStatement createStat)
                {
                    Assert.AreEqual<string>("stat1", createStat.Name.Value);
                    Assert.AreEqual<string>("t1", createStat.OnName.BaseIdentifier.Value);
                }, "CreateStatisticsStatementTest.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void CreateSymmetricKey()
        {
            PhaseOne90Test(delegate(CreateSymmetricKeyStatement statement)
                {
                    Assert.AreEqual<string>("s1", statement.Name.Value);
                }, "CreateSymmetricKey.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void CreateSynonym()
        {
            PhaseOne90Test(delegate(CreateSynonymStatement statement)
                {
                    CheckSchemaObjectName(statement.Name, "dbo", "s1");
                }, "CreateSynonym.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void CreateTableStatementTest()
        {
            PhaseOneAllParserTest(delegate(CreateTableStatement createTable)
                {
                    Assert.AreEqual<string>("t1", createTable.SchemaObjectName.BaseIdentifier.Value);
                }, "CreateTableStatementTest.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void CreateTriggerStatementTest()
        {
            PhaseOneAllParserTest(delegate(CreateTriggerStatement createTrigger)
                {
                    Assert.AreEqual<string>("trig1", createTrigger.Name.BaseIdentifier.Value);
                    Assert.AreEqual<string>("t1", createTrigger.TriggerObject.Name.BaseIdentifier.Value);
                }, "CreateTriggerStatementTest.sql");
            SqlModuleObjectNameAllTest(null, "trig1", "CreateTriggerStatementTest.sql");
            TriggerTargetObjectNameAllTest(null, "trig1", "dbo", "t1", "CreateTriggerStatementTest.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void CreateTypeUddt()
        {
            PhaseOne90Test(delegate(CreateTypeUddtStatement statement)
                {
                    CheckSchemaObjectName(statement.Name, "dbo", "t1");
                }, "CreateTypeUddt.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void CreateTypeUdt()
        {
            PhaseOne90Test(delegate(CreateTypeUdtStatement statement)
                {
                    CheckSchemaObjectName(statement.Name, "dbo", "t1");
                },"CreateTypeUdt.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void CreateUser()
        {
            PhaseOne90Test(delegate(CreateUserStatement statement)
                {
                    Assert.AreEqual<string>("u1", statement.Name.Value);
                }, "CreateUser.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void CreateViewStatementTest()
        {
            PhaseOneAllParserTest(delegate(CreateViewStatement createView)
                {
                    Assert.AreEqual<string>("v1", createView.SchemaObjectName.BaseIdentifier.Value);
                }, "CreateViewStatementTest.sql");
            SqlModuleObjectNameAllTest("dbo", "v1", "CreateViewStatementTest.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void AlterTriggerAllServerTest()
        {
            PhaseOne90Test(delegate(AlterTriggerStatement alterTrigger)
                {
                    Assert.AreEqual<TriggerScope>(TriggerScope.AllServer, alterTrigger.TriggerObject.TriggerScope);
                    Assert.IsNull(alterTrigger.TriggerObject.Name);
                    CheckSchemaObjectName(alterTrigger.Name, null, "t1");
                }, "AlterTriggerAllServerTest.sql");
            SqlModuleObjectName90PlusTest(null, "t1", "AlterTriggerAllServerTest.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void AlterTriggerDatabaseTest()
        {
            PhaseOne90Test(delegate(AlterTriggerStatement alterTrigger)
                {
                    Assert.AreEqual<TriggerScope>(TriggerScope.Database, alterTrigger.TriggerObject.TriggerScope);
                    Assert.IsNull(alterTrigger.TriggerObject.Name);
                    CheckSchemaObjectName(alterTrigger.Name, null, "t1");
                }, "AlterTriggerDatabaseTest.sql");
            SqlModuleObjectName90PlusTest(null, "t1", "AlterTriggerDatabaseTest.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void CreateXmlIndexTest()
        {
            PhaseOne90Test(delegate(CreateXmlIndexStatement statement)
                {
                    Assert.AreEqual<bool>(false, statement.Primary);
                    Assert.AreEqual<string>("i1", statement.Name.Value);
                    Assert.AreEqual<string>("t1", statement.OnName.BaseIdentifier.Value);
                }, "CreateXmlIndex.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void CreateXmlSchemaCollection()
        {
            PhaseOne90Test(delegate(CreateXmlSchemaCollectionStatement statement)
                {
                    CheckSchemaObjectName(statement.Name, "sc", "c1");
                }, "CreateXmlSchemaCollection.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void ColumnDefinitionNotInAlterTable()
        {
            ParserTestUtils.ExecuteTestForAllParsers(delegate(TSqlParser parser)
                {
                    StringReader reader = new StringReader("declare @v1 as table (c1 int)");
                    TSqlStatement statement = parser.PhaseOneParse(reader);
                    // P1 parse should return null, since this is not DDL, but shouldn't assert or throw - VS Whidbey 604310
                    Assert.IsNull(statement);
                }, true);
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void ResourcePoolStatementTests()
        {
            PhaseOne100TestString(delegate(CreateResourcePoolStatement statement)
            {
                Assert.AreEqual<string>("rp1", statement.Name.Value);
            }, "create resource pool rp1");

            PhaseOne100TestString(delegate(AlterResourcePoolStatement statement)
            {
                Assert.AreEqual<string>("rp1", statement.Name.Value);
            }, "alter resource pool rp1");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void WorkloadGroupStatementTests()
        {
            PhaseOne100TestString(delegate(CreateWorkloadGroupStatement statement)
            {
                Assert.AreEqual<string>("wg1", statement.Name.Value);
            }, "create workload group wg1");

            PhaseOne100TestString(delegate(AlterWorkloadGroupStatement statement)
            {
                Assert.AreEqual<string>("wg1", statement.Name.Value);
            }, "alter workload group wg1");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void BrokerPriorityStatementTests()
        {
            PhaseOne100TestString(delegate(CreateBrokerPriorityStatement statement)
            {
                Assert.AreEqual<string>("bp1", statement.Name.Value);
            }, "create broker priority bp1");

            PhaseOne100TestString(delegate(AlterBrokerPriorityStatement statement)
            {
                Assert.AreEqual<string>("bp1", statement.Name.Value);
            }, "alter broker priority bp1");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void FulltextStoplistStatementTests()
        {
            PhaseOne100TestString(delegate(CreateFullTextStopListStatement statement)
            {
                Assert.AreEqual<string>("fs1", statement.Name.Value);
            }, "create fulltext stoplist fs1");

            PhaseOne100TestString(delegate(AlterFullTextStopListStatement statement)
            {
                Assert.AreEqual<string>("fs1", statement.Name.Value);
            }, "alter fulltext stoplist fs1");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void CryptographicProviderStatementTests()
        {
            PhaseOne100TestString(delegate(CreateCryptographicProviderStatement statement)
            {
                Assert.AreEqual<string>("cp1", statement.Name.Value);
            }, "create cryptographic provider cp1");

            PhaseOne100TestString(delegate(AlterCryptographicProviderStatement statement)
            {
                Assert.AreEqual<string>("cp1", statement.Name.Value);
            }, "alter cryptographic provider cp1");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void EventSessionStatementTests()
        {
            PhaseOne100TestString(delegate(CreateEventSessionStatement statement)
            {
                Assert.AreEqual<string>("es1", statement.Name.Value);
            }, "create event session es1");

            PhaseOne100TestString(delegate(AlterEventSessionStatement statement)
            {
                Assert.AreEqual<string>("es1", statement.Name.Value);
            }, "alter event session es1");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void CreateSpatialIndexStatementTest()
        {
            PhaseOne100TestString(delegate(CreateSpatialIndexStatement statement)
            {
                Assert.AreEqual<string>("sp1", statement.Name.Value);
            }, "create spatial index sp1");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void AuditStatementsTest()
        {
            PhaseOne100TestString(delegate(CreateServerAuditStatement st)
            {
                Assert.AreEqual<string>("a1", st.AuditName.Value);
            }, "CREATE SERVER AUDIT a1 TO");

            PhaseOne100TestString(delegate(AlterServerAuditStatement st)
            {
                Assert.AreEqual<string>("a1", st.AuditName.Value);
            }, "ALTER SERVER AUDIT a1 TO");

            PhaseOne100TestString(delegate(CreateServerAuditSpecificationStatement st)
            {
                Assert.AreEqual<string>("sp1", st.SpecificationName.Value);
            }, "CREATE SERVER AUDIT SPECIFICATION sp1");

            PhaseOne100TestString(delegate(AlterServerAuditSpecificationStatement st)
            {
                Assert.AreEqual<string>("sp1", st.SpecificationName.Value);
            }, "ALTER SERVER AUDIT SPECIFICATION sp1");

            PhaseOne100TestString(delegate(CreateDatabaseAuditSpecificationStatement st)
            {
                Assert.AreEqual<string>("sp1", st.SpecificationName.Value);
            }, "CREATE DATABASE AUDIT SPECIFICATION sp1");

            PhaseOne100TestString(delegate(AlterDatabaseAuditSpecificationStatement st)
            {
                Assert.AreEqual<string>("sp1", st.SpecificationName.Value);
            }, "ALTER DATABASE AUDIT SPECIFICATION sp1");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void CreateExternalDataSource()
        {
            PhaseOne130Test(delegate(CreateExternalDataSourceStatement statement)
            {
                Assert.AreEqual<string>("eds1", statement.Name.Value);
            }, "CreateExternalDataSource.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void CreateExternalFileFormat()
        {
            PhaseOne130Test(delegate(CreateExternalFileFormatStatement statement)
            {
                Assert.AreEqual<string>("eff1", statement.Name.Value);
            }, "CreateExternalFileFormat.sql");
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]

        

        public void CreateExternalTable()
        {
            PhaseOne130Test(delegate(CreateExternalTableStatement statement)
            {
                Assert.AreEqual<string>("t1", statement.SchemaObjectName.BaseIdentifier.Value);
            }, "CreateExternalTable.sql");
        }
    }
}
