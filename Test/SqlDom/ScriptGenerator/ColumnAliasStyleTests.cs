//------------------------------------------------------------------------------
// <copyright file="ColumnAliasStyleTests.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlStudio.Tests.AssemblyTools.TestCategory;
using static SqlStudio.Tests.UTSqlScriptDom.ScriptGeneratorTestHelper;

namespace SqlStudio.Tests.UTSqlScriptDom
{
    // Tests for the ColumnAliasStyle script-generation option (AsKeyword / EqualsSign / Preserve).
    // Kept in a dedicated file to avoid churn in ScriptGeneratorTests.cs.
    [TestClass]
    public class ColumnAliasStyleTests
    {
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestColumnAliasStyleDefaultIsAsKeyword()
        {
            Assert.AreEqual(ColumnAliasStyle.AsKeyword, new SqlScriptGeneratorOptions().ColumnAliasStyle);
        }

        // ---------------------------------------------------------------------------------------
        // SELECT-projection behavior: conversion between styles, '=' alignment, Preserve, and the
        // pass-through cases where the option must leave a projection element unchanged.
        // ---------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestColumnAliasStyleEqualsSignSelectProjectionRoundTrips()
        {
            string generated = Generate(
                "SELECT a AS x, b AS y FROM t;",
                new SqlScriptGeneratorOptions { ColumnAliasStyle = ColumnAliasStyle.EqualsSign });

            string expected =
                "SELECT x = a," + Environment.NewLine +
                "       y = b" + Environment.NewLine +
                "FROM   t;" + Environment.NewLine + Environment.NewLine;

            Assert.AreEqual(expected, generated);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestColumnAliasStyleEqualsSignSingleLineSelect()
        {
            // MultilineSelectElementsList == false exercises the single-line branch.
            string generated = Generate(
                "SELECT a AS x FROM t;",
                new SqlScriptGeneratorOptions
                {
                    ColumnAliasStyle = ColumnAliasStyle.EqualsSign,
                    MultilineSelectElementsList = false
                });

            string expected =
                "SELECT x = a" + Environment.NewLine +
                "FROM   t;" + Environment.NewLine + Environment.NewLine;

            Assert.AreEqual(expected, generated);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestColumnAliasStyleEqualsSignLeavesSelectStarAndUnaliasedColumns()
        {
            // '*' (SelectStarExpression) and unaliased columns have no alias to convert.
            string generated = Generate(
                "SELECT *, a, b AS y FROM t;",
                new SqlScriptGeneratorOptions { ColumnAliasStyle = ColumnAliasStyle.EqualsSign });

            string expected =
                "SELECT *," + Environment.NewLine +
                "       a," + Environment.NewLine +
                "       y = b" + Environment.NewLine +
                "FROM   t;" + Environment.NewLine + Environment.NewLine;

            Assert.AreEqual(expected, generated);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestColumnAliasStyleEqualsSignLeavesSelectSetVariable()
        {
            // 'SELECT @v = a' is a SelectSetVariable, not a SelectScalarExpression column alias,
            // so the option must not alter it.
            string generated = Generate(
                "SELECT @v = a FROM t;",
                new SqlScriptGeneratorOptions { ColumnAliasStyle = ColumnAliasStyle.EqualsSign });

            string expected =
                "SELECT @v = a" + Environment.NewLine +
                "FROM   t;" + Environment.NewLine + Environment.NewLine;

            Assert.AreEqual(expected, generated);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestColumnAliasStyleAsKeywordLeavesSelectSetVariable()
        {
            // 'SELECT @v = a' is a SelectSetVariable, not a SelectScalarExpression column alias,
            // so AsKeyword must not rewrite it into 'a AS @v'.
            string generated = Generate(
                "SELECT @v = a FROM t;",
                new SqlScriptGeneratorOptions { ColumnAliasStyle = ColumnAliasStyle.AsKeyword });

            string expected =
                "SELECT @v = a" + Environment.NewLine +
                "FROM   t;" + Environment.NewLine + Environment.NewLine;

            Assert.AreEqual(expected, generated);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestColumnAliasStyleAsKeywordConvertsFromEqualsSign()
        {
            // AsKeyword rewrites "alias = expression" into "expression AS alias".
            string generated = Generate(
                "SELECT x = a, y = bb FROM t;",
                new SqlScriptGeneratorOptions { ColumnAliasStyle = ColumnAliasStyle.AsKeyword });

            string expected =
                "SELECT a AS x," + Environment.NewLine +
                "       bb AS y" + Environment.NewLine +
                "FROM   t;" + Environment.NewLine + Environment.NewLine;

            Assert.AreEqual(expected, generated);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestColumnAliasStyleEqualsSignConvertsFromAsKeyword()
        {
            // EqualsSign rewrites "expression AS alias" into "alias = expression" and aligns the
            // '=' signs within the SELECT list when AlignClauseBodies is on (the default).
            string generated = Generate(
                "SELECT a AS col1, bb AS c FROM t;",
                new SqlScriptGeneratorOptions { ColumnAliasStyle = ColumnAliasStyle.EqualsSign });

            string expected =
                "SELECT col1 = a," + Environment.NewLine +
                "       c    = bb" + Environment.NewLine +
                "FROM   t;" + Environment.NewLine + Environment.NewLine;

            Assert.AreEqual(expected, generated);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestColumnAliasStyleEqualsSignConvertsDelimitedAndStringAliases()
        {
            // The alias (ColumnName) is an IdentifierOrValueExpression: it can be a delimited
            // identifier or a string literal, both of which are valid on the left of '='.
            string generated = Generate(
                "SELECT a AS [My Col], b AS 'Str Alias' FROM t;",
                new SqlScriptGeneratorOptions { ColumnAliasStyle = ColumnAliasStyle.EqualsSign });

            string expected =
                "SELECT [My Col]    = a," + Environment.NewLine +
                "       'Str Alias' = b" + Environment.NewLine +
                "FROM   t;" + Environment.NewLine + Environment.NewLine;

            Assert.AreEqual(expected, generated);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestColumnAliasStyleEqualsSignNoAlignment()
        {
            // With AlignClauseBodies off, EqualsSign still applies but the '=' signs are not padded.
            string generated = Generate(
                "SELECT a AS col1, bb AS c FROM t;",
                new SqlScriptGeneratorOptions
                {
                    ColumnAliasStyle = ColumnAliasStyle.EqualsSign,
                    AlignClauseBodies = false
                });

            string expected =
                "SELECT col1 = a," + Environment.NewLine +
                "       c = bb" + Environment.NewLine +
                "FROM t;" + Environment.NewLine + Environment.NewLine;

            Assert.AreEqual(expected, generated);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestColumnAliasStyleEqualsSignSingleLineList()
        {
            // With MultilineSelectElementsList off, the list stays on one line and no '=' alignment
            // is performed.
            string generated = Generate(
                "SELECT a AS col1, bb AS c FROM t;",
                new SqlScriptGeneratorOptions
                {
                    ColumnAliasStyle = ColumnAliasStyle.EqualsSign,
                    MultilineSelectElementsList = false
                });

            string expected =
                "SELECT col1 = a, c = bb" + Environment.NewLine +
                "FROM   t;" + Environment.NewLine + Environment.NewLine;

            Assert.AreEqual(expected, generated);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestColumnAliasStyleEqualsSignLeavesUnaliasedExpressionsUnchanged()
        {
            // Expressions without an explicit alias are not affected by the option.
            string generated = Generate(
                "SELECT a, b AS c FROM t;",
                new SqlScriptGeneratorOptions { ColumnAliasStyle = ColumnAliasStyle.EqualsSign });

            string expected =
                "SELECT a," + Environment.NewLine +
                "       c = b" + Environment.NewLine +
                "FROM   t;" + Environment.NewLine + Environment.NewLine;

            Assert.AreEqual(expected, generated);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestColumnAliasStyleEqualsSignLeavesTableAliasUnchanged()
        {
            // Table aliases must remain AS-style regardless of ColumnAliasStyle.
            string generated = Generate(
                "SELECT t.a AS c FROM t AS t;",
                new SqlScriptGeneratorOptions { ColumnAliasStyle = ColumnAliasStyle.EqualsSign });

            string expected =
                "SELECT c = t.a" + Environment.NewLine +
                "FROM   t AS t;" + Environment.NewLine + Environment.NewLine;

            Assert.AreEqual(expected, generated);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestColumnAliasStylePreserveKeepsEqualsSign()
        {
            // Preserve keeps the original equals-sign form.
            string generated = Generate(
                "SELECT col1 = a, c = bb FROM t;",
                new SqlScriptGeneratorOptions { ColumnAliasStyle = ColumnAliasStyle.Preserve });

            string expected =
                "SELECT col1 = a," + Environment.NewLine +
                "       c    = bb" + Environment.NewLine +
                "FROM   t;" + Environment.NewLine + Environment.NewLine;

            Assert.AreEqual(expected, generated);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestColumnAliasStylePreserveKeepsAsKeyword()
        {
            // Preserve keeps the original AS-keyword form.
            string generated = Generate(
                "SELECT a AS col1, bb AS c FROM t;",
                new SqlScriptGeneratorOptions { ColumnAliasStyle = ColumnAliasStyle.Preserve });

            string expected =
                "SELECT a AS col1," + Environment.NewLine +
                "       bb AS c" + Environment.NewLine +
                "FROM   t;" + Environment.NewLine + Environment.NewLine;

            Assert.AreEqual(expected, generated);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestColumnAliasStylePreserveMixedForms()
        {
            // Preserve keeps each column's original style independently.
            string generated = Generate(
                "SELECT col1 = a, bb AS c FROM t;",
                new SqlScriptGeneratorOptions { ColumnAliasStyle = ColumnAliasStyle.Preserve });

            string expected =
                "SELECT col1 = a," + Environment.NewLine +
                "       bb AS c" + Environment.NewLine +
                "FROM   t;" + Environment.NewLine + Environment.NewLine;

            Assert.AreEqual(expected, generated);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestColumnAliasStyleEqualsSignNestedSubqueryAlignsIndependently()
        {
            // The outer and inner SELECT lists align their '=' signs independently.
            string generated = Generate(
                "SELECT longName = (SELECT x = 1), y = 2 FROM t;",
                new SqlScriptGeneratorOptions { ColumnAliasStyle = ColumnAliasStyle.EqualsSign });

            string expected =
                "SELECT longName = (SELECT x = 1)," + Environment.NewLine +
                "       y        = 2" + Environment.NewLine +
                "FROM   t;" + Environment.NewLine + Environment.NewLine;

            Assert.AreEqual(expected, generated);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestColumnAliasStyleAsKeywordConvertsFromEqualsSignInCommonTableExpression()
        {
            // The option applies to the SELECT list inside a CTE body, while the CTE's own column
            // list "(alias1, alias2)" is a definition and must remain unchanged.
            string generated = Generate(
                "WITH cte (alias1, alias2) AS (SELECT alias1 = a, alias2 = bb FROM t) SELECT * FROM cte;",
                new SqlScriptGeneratorOptions { ColumnAliasStyle = ColumnAliasStyle.AsKeyword });

            string expected =
                "WITH   cte (alias1, alias2)" + Environment.NewLine +
                "AS     (SELECT a AS alias1," + Environment.NewLine +
                "               bb AS alias2" + Environment.NewLine +
                "        FROM   t)" + Environment.NewLine +
                "SELECT *" + Environment.NewLine +
                "FROM   cte;" + Environment.NewLine + Environment.NewLine;

            Assert.AreEqual(expected, generated);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestColumnAliasStyleEqualsSignConvertsFromAsKeywordWithSubqueries()
        {
            // Converting AS -> '=' across a scalar subquery in the projection and a derived-table
            // subquery in FROM. Each SELECT list aligns its '=' independently, and the derived
            // table alias ("AS o") remains AS-style.
            string generated = Generate(
                "SELECT o.total AS grandTotal, (SELECT COUNT(*) FROM t2) AS cnt " +
                "FROM (SELECT SUM(x) AS total FROM t1) AS o;",
                new SqlScriptGeneratorOptions { ColumnAliasStyle = ColumnAliasStyle.EqualsSign });

            string expected =
                "SELECT grandTotal = o.total," + Environment.NewLine +
                "       cnt        = (SELECT COUNT(*)" + Environment.NewLine +
                "                     FROM   t2)" + Environment.NewLine +
                "FROM   (SELECT total = SUM(x)" + Environment.NewLine +
                "        FROM   t1) AS o;" + Environment.NewLine + Environment.NewLine;

            Assert.AreEqual(expected, generated);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestColumnAliasStylePreserveOnProgrammaticallyBuiltFragmentRendersAsKeyword()
        {
            // A fragment built in code has no source token positions, so Preserve cannot detect an
            // original equals-sign form and falls back to AS-keyword rendering.
            var scalar = new SelectScalarExpression
            {
                Expression = new ColumnReferenceExpression
                {
                    MultiPartIdentifier = new MultiPartIdentifier { Identifiers = { new Identifier { Value = "a" } } }
                },
                ColumnName = new IdentifierOrValueExpression { Identifier = new Identifier { Value = "x" } }
            };

            var query = new QuerySpecification();
            query.SelectElements.Add(scalar);
            var select = new SelectStatement { QueryExpression = query };

            var generator = new Sql170ScriptGenerator(
                new SqlScriptGeneratorOptions { ColumnAliasStyle = ColumnAliasStyle.Preserve });
            generator.GenerateScript(select, out string generated);

            string expected = "SELECT a AS x";

            Assert.AreEqual(expected, generated);
        }

        // ---------------------------------------------------------------------------------------
        // Context-scoping tests: the "alias = expression" form is valid ONLY in a SELECT
        // projection list. OUTPUT, OUTPUT INTO and RECEIVE reuse SelectScalarExpression but their
        // grammar (outputClauseSelectColumn / receiveColumnSelectExpression) does NOT accept the
        // equals-sign form, so ColumnAliasStyle must never rewrite aliases in those contexts.
        // Generate() also re-parses the output, which is the key regression guard: before scoping,
        // the generated "alias = expression" output could not be re-parsed in these contexts.
        // ---------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestColumnAliasStyleEqualsSignDoesNotApplyToOutputClause()
        {
            string generated = Generate(
                "DELETE dbo.T OUTPUT deleted.Id AS OldId;",
                new SqlScriptGeneratorOptions { ColumnAliasStyle = ColumnAliasStyle.EqualsSign });

            string expected =
                "DELETE dbo.T" + Environment.NewLine +
                "OUTPUT deleted.Id AS OldId;" + Environment.NewLine + Environment.NewLine;

            Assert.AreEqual(expected, generated);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestColumnAliasStyleEqualsSignDoesNotApplyToOutputIntoClause()
        {
            string generated = Generate(
                "DELETE dbo.T OUTPUT deleted.Id AS OldId INTO dbo.Removed (OldId);",
                new SqlScriptGeneratorOptions { ColumnAliasStyle = ColumnAliasStyle.EqualsSign });

            string expected =
                "DELETE dbo.T" + Environment.NewLine +
                "OUTPUT deleted.Id AS OldId INTO dbo.Removed (OldId);" + Environment.NewLine + Environment.NewLine;

            Assert.AreEqual(expected, generated);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestColumnAliasStylePreserveDoesNotApplyToOutputClause()
        {
            // OUTPUT can only ever be authored in AS form, so Preserve must round-trip it unchanged.
            string generated = Generate(
                "DELETE dbo.T OUTPUT deleted.Id AS OldId;",
                new SqlScriptGeneratorOptions { ColumnAliasStyle = ColumnAliasStyle.Preserve });

            string expected =
                "DELETE dbo.T" + Environment.NewLine +
                "OUTPUT deleted.Id AS OldId;" + Environment.NewLine + Environment.NewLine;

            Assert.AreEqual(expected, generated);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestColumnAliasStyleEqualsSignDoesNotApplyToReceiveStatement()
        {
            string generated = Generate(
                "RECEIVE TOP (1) conversation_handle AS Handle FROM dbo.TargetQueue;",
                new SqlScriptGeneratorOptions { ColumnAliasStyle = ColumnAliasStyle.EqualsSign });

            string expected =
                "RECEIVE TOP (1) conversation_handle AS Handle FROM dbo.TargetQueue;" + Environment.NewLine + Environment.NewLine;

            Assert.AreEqual(expected, generated);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestColumnAliasStyleEqualsSignOutputAliasAndDerivedTableProjectionDoNotInterfere()
        {
            // A single statement mixes a non-projection OUTPUT alias (must stay AS) with a real
            // derived-table projection (must convert). This exercises the save/restore of the
            // in-projection flag: the derived-table SELECT sets it true and restores it, so the
            // sibling OUTPUT column is never rewritten.
            string generated = Generate(
                "UPDATE t SET a = 1 OUTPUT inserted.a AS x " +
                "FROM dbo.Target AS t INNER JOIN (SELECT v AS w FROM dbo.Src) AS d ON t.k = d.k;",
                new SqlScriptGeneratorOptions { ColumnAliasStyle = ColumnAliasStyle.EqualsSign });

            string expected =
                "UPDATE t" + Environment.NewLine +
                "SET    a = 1" + Environment.NewLine +
                "OUTPUT inserted.a AS x" + Environment.NewLine +
                "FROM   dbo.Target AS t" + Environment.NewLine +
                "       INNER JOIN" + Environment.NewLine +
                "       (SELECT w = v" + Environment.NewLine +
                "        FROM   dbo.Src) AS d" + Environment.NewLine +
                "       ON t.k = d.k;" + Environment.NewLine + Environment.NewLine;

            Assert.AreEqual(expected, generated);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestColumnAliasStyleEqualsSignWithPreserveCommentsRepositionsInlineComment()
        {
            // Known limitation: converting "expr AS alias" -> "alias = expr" reorders the alias
            // ahead of the expression. With PreserveComments on, a comment authored between the
            // expression and the AS keyword has no faithful home in the reordered text, so it is
            // preserved (never dropped) but repositioned to the start of the element. The output
            // still round-trips (Generate() asserts it re-parses).
            string generated = Generate(
                "SELECT a /* mid */ AS x, bb AS y FROM t;",
                new SqlScriptGeneratorOptions { ColumnAliasStyle = ColumnAliasStyle.EqualsSign, PreserveComments = true });

            string expected =
                "SELECT /* mid */" + Environment.NewLine +
                "       x = a," + Environment.NewLine +
                "       y = bb" + Environment.NewLine +
                "FROM   t;" + Environment.NewLine + Environment.NewLine;

            Assert.AreEqual(expected, generated);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestColumnAliasStyleEqualsSignWithPreserveCommentsAlignsEqualSignsAcrossRows()
        {
            // Regression: with PreserveComments on, GenerateFragmentList pushes a per-row alignment
            // scope for each select element. The '=' alignment point must still be shared across
            // rows so the '=' signs line up to the widest alias, exactly as when PreserveComments
            // is off. (Aliases of different widths are required to observe the alignment.)
            string generated = Generate(
                "SELECT a AS col1, bb AS c FROM t;",
                new SqlScriptGeneratorOptions
                {
                    ColumnAliasStyle = ColumnAliasStyle.EqualsSign,
                    PreserveComments = true
                });

            string expected =
                "SELECT col1 = a," + Environment.NewLine +
                "       c    = bb" + Environment.NewLine +
                "FROM   t;" + Environment.NewLine + Environment.NewLine;

            Assert.AreEqual(expected, generated);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestColumnAliasStylePreserveWithCommentsKeepsCommentInPlace()
        {
            // Preserve never reorders the alias and expression, so PreserveComments keeps inline
            // comments exactly where they were authored (contrast with the EqualsSign conversion,
            // which repositions them).
            string generated = Generate(
                "SELECT a /* mid */ AS x, bb AS y FROM t;",
                new SqlScriptGeneratorOptions { ColumnAliasStyle = ColumnAliasStyle.Preserve, PreserveComments = true });

            string expected =
                "SELECT a /* mid */ AS x," + Environment.NewLine +
                "       bb AS y" + Environment.NewLine +
                "FROM   t;" + Environment.NewLine + Environment.NewLine;

            Assert.AreEqual(expected, generated);
        }
    }
}
