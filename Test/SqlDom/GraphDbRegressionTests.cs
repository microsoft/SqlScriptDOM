//------------------------------------------------------------------------------
// <copyright file="BackupRestoreRegressionTests.cs" company="Microsoft">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using Microsoft.SqlServer.TransactSql.ScriptDom;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlStudio.Tests.AssemblyTools.TestCategory;
using System.Collections.Generic;
using System.IO;

namespace SqlStudio.Tests.UTSqlScriptDom
{
    public partial class SqlDomTests
    {
        /// <summary>
        /// Test for graph pseudo column as included column in a CREATE INDEX statement
        /// See https://github.com/microsoft/SqlScriptDOM/issues/25 for details on the issue.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void GraphPseudoColumnsInCreateIndexIncludedColumns()
        {
            ParserTestUtils.ExecuteTestForParsers(parser =>
            {
                string script = @"create index NC_node_4	ON node_4 (c1) INCLUDE ($node_id);";
                using (var scriptReader = new StringReader(script))
                {
                    var fragment = parser.Parse(scriptReader, out IList<ParseError> errors) as TSqlScript;
                    Assert.AreEqual(0, errors.Count);
                    Assert.IsTrue(fragment is TSqlScript);
                    Assert.IsTrue(fragment.Batches[0].Statements[0] is CreateIndexStatement);
                    Assert.AreEqual(ColumnType.PseudoColumnGraphNodeId, (fragment.Batches[0].Statements[0] as CreateIndexStatement).IncludeColumns[0].ColumnType);
                    Assert.AreEqual(ColumnType.Regular, (fragment.Batches[0].Statements[0] as CreateIndexStatement).Columns[0].Column.ColumnType);
                    Assert.AreEqual("c1", (fragment.Batches[0].Statements[0] as CreateIndexStatement).Columns[0].Column.MultiPartIdentifier.Identifiers[0].Value);
                }
            }, new TSql140Parser(true), new TSql150Parser(true), new TSql160Parser(true));
        }

        /// <summary>
        /// Test for graph pseudo column as included column in an inline index definition within a CREATE TABLE statement (starting SQL 2019)
        /// See https://github.com/microsoft/SqlScriptDOM/issues/25 for details on the issue.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void GraphPseudoColumnsInInlineIndexIncludedColumns()
        {
            ParserTestUtils.ExecuteTestForParsers(parser =>
            {
                string script = @"create table [dbo].[node_5] (c1 int, index idx (c1) include ($node_id)) As Node";
                using (var scriptReader = new StringReader(script))
                {
                    var fragment = parser.Parse(scriptReader, out IList<ParseError> errors) as TSqlScript;
                    Assert.AreEqual(0, errors.Count);
                    Assert.IsTrue(fragment is TSqlScript);
                    Assert.IsTrue(fragment.Batches[0].Statements[0] is CreateTableStatement);
                    Assert.AreEqual(ColumnType.PseudoColumnGraphNodeId, (fragment.Batches[0].Statements[0] as CreateTableStatement).Definition.Indexes[0].IncludeColumns[0].ColumnType);
                    Assert.AreEqual(ColumnType.Regular, (fragment.Batches[0].Statements[0] as CreateTableStatement).Definition.Indexes[0].Columns[0].Column.ColumnType);
                    Assert.AreEqual("c1", (fragment.Batches[0].Statements[0] as CreateTableStatement).Definition.Indexes[0].Columns[0].Column.MultiPartIdentifier.Identifiers[0].Value);
                }
            }, new TSql150Parser(true), new TSql160Parser(true));
        }

        /// <summary>
        /// Currently, graph pseudo columns are not supported in the ORDER clause for a clustered columnstore index
        /// This was added as regression test when fixing https://github.com/microsoft/SqlScriptDOM/issues/25.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void DisallowGraphPseudoColumnsInOrderedClusteredColumnstore()
        {
            ParserTestUtils.ExecuteTestForParsers(parser =>
            {
                string script = @"create clustered columnstore index occi1 on n1 ORDER (c1, $node_id, c2)";
                using (var scriptReader = new StringReader(script))
                {
                    var fragment = parser.Parse(scriptReader, out IList<ParseError> errors) as TSqlScript;
                    Assert.AreEqual(1, errors.Count);
                    Assert.AreEqual("Incorrect syntax near $node_id.", errors[0].Message);
                }
            }, new TSql130Parser(true), new TSql140Parser(true), new TSql150Parser(true), new TSql160Parser(true));
        }
    }
}
