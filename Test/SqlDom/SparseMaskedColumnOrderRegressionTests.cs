//------------------------------------------------------------------------------
// <copyright file="SparseMaskedColumnOrderRegressionTests.cs" company="Microsoft">
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
        /// Regression test for https://github.com/microsoft/SqlScriptDOM/issues/216
        /// The Microsoft-documented column-definition order (SPARSE before MASKED WITH)
        /// must parse cleanly, matching the already-working reversed order.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SparseThenMaskedColumnOrderParses()
        {
            ParserTestUtils.ExecuteTestForParsers(parser =>
            {
                string script = @"CREATE TABLE t (c varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS SPARSE MASKED WITH (FUNCTION = 'default()') NULL);";
                using (var scriptReader = new StringReader(script))
                {
                    parser.Parse(scriptReader, out IList<ParseError> errors);
                    Assert.AreEqual(0, errors.Count);
                }
            }, new TSql130Parser(true), new TSql140Parser(true), new TSql150Parser(true), new TSql160Parser(true), new TSql170Parser(true), new TSql180Parser(true), new TSqlFabricDWParser(true));
        }

        /// <summary>
        /// Regression test for https://github.com/microsoft/SqlScriptDOM/issues/216
        /// Minimal reduction (no COLLATE) of the documented SPARSE-before-MASKED order.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SparseThenMaskedColumnOrderWithoutCollateParses()
        {
            ParserTestUtils.ExecuteTestForParsers(parser =>
            {
                string script = @"CREATE TABLE t (c varchar(100) SPARSE MASKED WITH (FUNCTION = 'default()') NULL);";
                using (var scriptReader = new StringReader(script))
                {
                    parser.Parse(scriptReader, out IList<ParseError> errors);
                    Assert.AreEqual(0, errors.Count);
                }
            }, new TSql130Parser(true), new TSql140Parser(true), new TSql150Parser(true), new TSql160Parser(true), new TSql170Parser(true), new TSql180Parser(true), new TSqlFabricDWParser(true));
        }

        /// <summary>
        /// Regression guard for https://github.com/microsoft/SqlScriptDOM/issues/216
        /// The reversed (non-standard) MASKED-before-SPARSE order already parses today and
        /// must keep parsing after the fix.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void MaskedThenSparseColumnOrderParses()
        {
            ParserTestUtils.ExecuteTestForParsers(parser =>
            {
                string script = @"CREATE TABLE t (c varchar(100) MASKED WITH (FUNCTION = 'default()') SPARSE NULL);";
                using (var scriptReader = new StringReader(script))
                {
                    parser.Parse(scriptReader, out IList<ParseError> errors);
                    Assert.AreEqual(0, errors.Count);
                }
            }, new TSql130Parser(true), new TSql140Parser(true), new TSql150Parser(true), new TSql160Parser(true), new TSql170Parser(true), new TSql180Parser(true), new TSqlFabricDWParser(true));
        }

        /// <summary>
        /// Round-trip guard for https://github.com/microsoft/SqlScriptDOM/issues/216
        /// Both orderings normalize to the documented SPARSE-before-MASKED order through the
        /// script generator, and the generated script re-parses cleanly.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SparseMaskedColumnOrderRoundTrips()
        {
            foreach (string script in new[]
            {
                @"CREATE TABLE t (c varchar(100) SPARSE MASKED WITH (FUNCTION = 'default()') NULL);",
                @"CREATE TABLE t (c varchar(100) MASKED WITH (FUNCTION = 'default()') SPARSE NULL);"
            })
            {
                var parser = new TSql160Parser(true);
                TSqlFragment fragment;
                using (var scriptReader = new StringReader(script))
                {
                    fragment = parser.Parse(scriptReader, out IList<ParseError> errors);
                    Assert.AreEqual(0, errors.Count);
                }

                SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql160);
                scriptGen.GenerateScript(fragment, out string generated);

                // Generator always emits storage options (SPARSE) before MASKED WITH.
                Assert.IsTrue(generated.IndexOf("SPARSE", System.StringComparison.Ordinal) <
                    generated.IndexOf("MASKED", System.StringComparison.Ordinal));

                using (var generatedReader = new StringReader(generated))
                {
                    new TSql160Parser(true).Parse(generatedReader, out IList<ParseError> reparseErrors);
                    Assert.AreEqual(0, reparseErrors.Count);
                }
            }
        }

        /// <summary>
        /// Negative guard for https://github.com/microsoft/SqlScriptDOM/issues/216
        /// SPARSE predates SQL Server 2008, so TSql80/TSql90 must still reject it — the fix
        /// must not accidentally enable SPARSE (or MASKED) on those versions.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void SparseMaskedColumnOrderRejectedBefore100()
        {
            ParserTestUtils.ExecuteTestForParsers(parser =>
            {
                string script = @"CREATE TABLE t (c varchar(100) SPARSE MASKED WITH (FUNCTION = 'default()') NULL);";
                using (var scriptReader = new StringReader(script))
                {
                    parser.Parse(scriptReader, out IList<ParseError> errors);
                    Assert.IsTrue(errors.Count > 0);
                }
            }, new TSql80Parser(true), new TSql90Parser(true));
        }
    }
}
