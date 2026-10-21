//------------------------------------------------------------------------------
// <copyright file="BlockStatementTerminatorTests.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlStudio.Tests.AssemblyTools.TestCategory;
using static SqlStudio.Tests.UTSqlScriptDom.ScriptGeneratorTestHelper;

namespace SqlStudio.Tests.UTSqlScriptDom
{
    // Tests for the TerminateBlockStatements script-generation option, which controls whether a
    // semicolon terminator is written after a BEGIN...END block and after the END CATCH of a
    // TRY...CATCH block. When false (the default) these blocks are written without a terminator,
    // preserving the previous behavior; when true a semicolon is appended so a terminator supplied
    // in the source survives a parse and generate round-trip.
    //
    // Reported at https://developercommunity.visualstudio.com/t/SSMS-SQL-Formatter-removes-semicolons-af/11126731
    [TestClass]
    public class BlockStatementTerminatorTests
    {
        // Options that opt in to block terminators, leaving everything else at default.
        private static SqlScriptGeneratorOptions Terminated()
        {
            return new SqlScriptGeneratorOptions { TerminateBlockStatements = true };
        }

        // -----------------------------------------------------------------------------------------
        // Default: the option is off, so the existing unterminated form is preserved.
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestTerminateBlockStatementsDefaultIsFalse()
        {
            Assert.AreEqual(false, new SqlScriptGeneratorOptions().TerminateBlockStatements);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestBlockDefaultDropsSuppliedTerminator()
        {
            // The reported behavior, retained as the default: a supplied semicolon is not preserved.
            const string input = "BEGIN SELECT 1; END;";
            const string expected =
@"
BEGIN
    SELECT 1;
END";
            AssertGenerated(input, new SqlScriptGeneratorOptions(), expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestTryCatchDefaultDropsSuppliedTerminator()
        {
            const string input = "BEGIN TRY SELECT 1; END TRY BEGIN CATCH SELECT ERROR_NUMBER(); END CATCH;";
            const string expected =
@"
BEGIN TRY
    SELECT 1;
END TRY
BEGIN CATCH
    SELECT ERROR_NUMBER();
END CATCH";
            AssertGenerated(input, new SqlScriptGeneratorOptions(), expected);
        }

        // -----------------------------------------------------------------------------------------
        // Opt in: blocks are terminated.
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestBlockIsTerminatedWhenOptionIsSet()
        {
            const string input = "BEGIN SELECT 1; END;";
            const string expected =
@"
BEGIN
    SELECT 1;
END;";
            AssertGenerated(input, Terminated(), expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestBlockIsTerminatedEvenWhenSourceOmitsTheTerminator()
        {
            // The option normalizes to the terminated form; it does not merely preserve what was supplied.
            const string input = "BEGIN SELECT 1; END";
            const string expected =
@"
BEGIN
    SELECT 1;
END;";
            AssertGenerated(input, Terminated(), expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestEndCatchIsTerminatedButEndTryIsNot()
        {
            // END TRY is internal to the TRY...CATCH block, so only END CATCH receives a terminator.
            const string input = "BEGIN TRY SELECT 1; END TRY BEGIN CATCH SELECT ERROR_NUMBER(); END CATCH;";
            const string expected =
@"
BEGIN TRY
    SELECT 1;
END TRY
BEGIN CATCH
    SELECT ERROR_NUMBER();
END CATCH;";
            AssertGenerated(input, Terminated(), expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestNestedBlocksInsideProcedureAreTerminated()
        {
            // The exact scenario from the customer report.
            const string input =
@"CREATE PROCEDURE dbo.SemicolonTerminatedBlocks
AS
BEGIN
    BEGIN TRY
        SELECT 1;
    END TRY
    BEGIN CATCH
        SELECT ERROR_NUMBER();
    END CATCH;
END;";
            const string expected =
@"
CREATE PROCEDURE dbo.SemicolonTerminatedBlocks
AS
BEGIN
    BEGIN TRY
        SELECT 1;
    END TRY
    BEGIN CATCH
        SELECT ERROR_NUMBER();
    END CATCH;
END;";
            AssertGenerated(input, Terminated(), expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestTerminatedBlockRoundTripsUnchanged()
        {
            // Generating the already-terminated output again must be a fixed point.
            const string expected =
@"
BEGIN
    SELECT 1;
END;";
            string once = Generate("BEGIN SELECT 1; END;", Terminated());
            Assert.AreEqual(Normalize(expected).Trim(), Normalize(once).Trim());
            AssertGenerated(once, Terminated(), expected);
        }

        // -----------------------------------------------------------------------------------------
        // Interaction with the separating semicolon injected before CTEs and THROW.
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCteAfterTerminatedBlockDoesNotGetASecondSemicolon()
        {
            // The block now supplies its own terminator, so the separating semicolon must not be added.
            const string input = "BEGIN SELECT 1; END WITH cte AS (SELECT 1 AS c) SELECT c FROM cte;";
            const string expected =
@"
BEGIN
    SELECT 1;
END;

WITH   cte
AS     (SELECT 1 AS c)
SELECT c
FROM   cte;";
            AssertGenerated(input, Terminated(), expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestThrowAfterTerminatedBlockDoesNotGetASecondSemicolon()
        {
            const string input = "BEGIN SELECT 1; END THROW 50000, 'x', 1;";
            const string expected =
@"
BEGIN
    SELECT 1;
END;

THROW 50000, 'x', 1;";
            AssertGenerated(input, Terminated(), expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIfStatementIsStillNotTerminated()
        {
            // The option opts in only BEGIN...END and TRY...CATCH; IF, WHILE and labels keep their
            // existing unterminated form, so the separating semicolon before a CTE is still required.
            const string input = "IF 1 = 1 SELECT 1 WITH cte AS (SELECT 1 AS c) SELECT c FROM cte;";
            const string expected =
@"
IF 1 = 1
    SELECT 1;

WITH   cte
AS     (SELECT 1 AS c)
SELECT c
FROM   cte;";
            AssertGenerated(input, Terminated(), expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestWhileStatementIsStillNotTerminated()
        {
            const string input = "WHILE 1 = 1 SELECT 1 WITH cte AS (SELECT 1 AS c) SELECT c FROM cte;";
            const string expected =
@"
WHILE 1 = 1
    SELECT 1;

WITH   cte
AS     (SELECT 1 AS c)
SELECT c
FROM   cte;";
            AssertGenerated(input, Terminated(), expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestLabelStatementIsStillNotTerminated()
        {
            const string input = "lbl: SELECT 1;";
            const string expected =
@"
lbl:

SELECT 1;";
            AssertGenerated(input, Terminated(), expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestStatementSnippetIsStillNotTerminated()
        {
            // Snippets are emitted verbatim, so a terminator here would corrupt the preserved text.
            // They are not produced by parsing, so the AST is built directly.
            var batch = new TSqlBatch();
            batch.Statements.Add(new TSqlStatementSnippet { Script = "EXEC dbo.SomeProc" });
            var script = new TSqlScript();
            script.Batches.Add(batch);

            var generator = new Sql170ScriptGenerator(Terminated());
            generator.GenerateScript(script, out string generated);
            Assert.AreEqual("EXEC dbo.SomeProc", Normalize(generated).Trim());
        }

        // -----------------------------------------------------------------------------------------
        // BeginEndAtomicBlockStatement derives from BeginEndBlockStatement, so these pin the
        // exact-runtime-type membership test: an atomic block must not follow the option.
        // -----------------------------------------------------------------------------------------

        private const string AtomicProcedure =
@"CREATE PROCEDURE dbo.NativeProc
WITH NATIVE_COMPILATION, SCHEMABINDING
AS
BEGIN ATOMIC WITH (TRANSACTION ISOLATION LEVEL = SNAPSHOT, LANGUAGE = N'us_english')
    SELECT 1;
END";

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestAtomicBlockIsTerminatedByDefault()
        {
            // Atomic blocks are not in StatementsThatCannotHaveSemiColon, so they already carry a
            // terminator before this option existed.
            const string expected =
@"
CREATE PROCEDURE dbo.NativeProc
WITH NATIVE_COMPILATION, SCHEMABINDING
AS
BEGIN ATOMIC
WITH (TRANSACTION ISOLATION LEVEL = SNAPSHOT, LANGUAGE = N'us_english')
    SELECT 1;
END;";
            AssertGenerated(AtomicProcedure, new SqlScriptGeneratorOptions(), expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestAtomicBlockOutputIsUnaffectedByTheOption()
        {
            const string expected =
@"
CREATE PROCEDURE dbo.NativeProc
WITH NATIVE_COMPILATION, SCHEMABINDING
AS
BEGIN ATOMIC
WITH (TRANSACTION ISOLATION LEVEL = SNAPSHOT, LANGUAGE = N'us_english')
    SELECT 1;
END;";
            AssertGenerated(AtomicProcedure, Terminated(), expected);
        }

        // -----------------------------------------------------------------------------------------
        // Comment placement: the terminator must land before a trailing comment, not inside it.
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestTerminatorIsPlacedBeforeTrailingSingleLineComment()
        {
            const string input = "BEGIN SELECT 1; END -- trailing";
            var options = new SqlScriptGeneratorOptions { TerminateBlockStatements = true, PreserveComments = true };
            const string expected =
@"
BEGIN
    SELECT 1;
END; -- trailing";
            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestTrailingCommentPlacementIsUnchangedByDefault()
        {
            const string input = "BEGIN SELECT 1; END -- trailing";
            var options = new SqlScriptGeneratorOptions { PreserveComments = true };
            const string expected =
@"
BEGIN
    SELECT 1;
END -- trailing";
            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestTerminatorIsPlacedBeforeTrailingBlockComment()
        {
            const string input = "BEGIN SELECT 1; END /* trailing */";
            var options = new SqlScriptGeneratorOptions { TerminateBlockStatements = true, PreserveComments = true };
            const string expected =
@"
BEGIN
    SELECT 1;
END; /* trailing */";
            AssertGenerated(input, options, expected);
        }

        // -----------------------------------------------------------------------------------------
        // The option is honored by every generator version, including Fabric DW and Serverless.
        // Each version supplies its own StatementsThatCannotHaveSemiColon, which the gate consults.
        // -----------------------------------------------------------------------------------------

        private static IEnumerable<SqlScriptGenerator> AllGenerators(SqlScriptGeneratorOptions options)
        {
            yield return new Sql80ScriptGenerator(options);
            yield return new Sql90ScriptGenerator(options);
            yield return new Sql100ScriptGenerator(options);
            yield return new Sql110ScriptGenerator(options);
            yield return new Sql120ScriptGenerator(options);
            yield return new Sql130ScriptGenerator(options);
            yield return new Sql140ScriptGenerator(options);
            yield return new Sql150ScriptGenerator(options);
            yield return new Sql160ScriptGenerator(options);
            yield return new Sql170ScriptGenerator(options);
            yield return new Sql180ScriptGenerator(options);
            yield return new SqlFabricDWScriptGenerator(options);
            yield return new SqlServerlessScriptGenerator(options);
        }

        private static TSqlFragment ParseBlock()
        {
            var parser = new TSql80Parser(true);
            TSqlFragment fragment = parser.Parse(new StringReader("BEGIN SELECT 1; END"), out IList<ParseError> errors);
            Assert.AreEqual(0, errors.Count, "Input must parse without errors.");
            return fragment;
        }

        private static void AssertGeneratedBy(SqlScriptGenerator generator, TSqlFragment fragment, string expected)
        {
            generator.GenerateScript(fragment, out string generated);
            Assert.AreEqual(Normalize(expected).Trim(), Normalize(generated).Trim(), generator.GetType().Name);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestEveryGeneratorVersionTerminatesWhenOptionIsSet()
        {
            // Every version supplies its own StatementsThatCannotHaveSemiColon, so assert the exact
            // output for each rather than trusting that the shared gate is reached.
            const string expected =
@"
BEGIN
    SELECT 1;
END;";
            TSqlFragment fragment = ParseBlock();
            foreach (SqlScriptGenerator generator in AllGenerators(Terminated()))
            {
                AssertGeneratedBy(generator, fragment, expected);
            }
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestEveryGeneratorVersionLeavesBlockUnterminatedByDefault()
        {
            const string expected =
@"
BEGIN
    SELECT 1;
END";
            TSqlFragment fragment = ParseBlock();
            foreach (SqlScriptGenerator generator in AllGenerators(new SqlScriptGeneratorOptions()))
            {
                AssertGeneratedBy(generator, fragment, expected);
            }
        }

        // -----------------------------------------------------------------------------------------
        // The oldest parser must accept the terminated form the generator now produces.
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestSql80GeneratorOutputReparsesWithSql80Parser()
        {
            const string expected =
@"
BEGIN
    SELECT 1;
END;";
            TSqlFragment fragment = ParseBlock();

            var generator = new Sql80ScriptGenerator(Terminated());
            generator.GenerateScript(fragment, out string generated);
            Assert.AreEqual(Normalize(expected).Trim(), Normalize(generated).Trim());

            var reparser = new TSql80Parser(true);
            reparser.Parse(new StringReader(generated), out IList<ParseError> reErrors);
            Assert.AreEqual(0, reErrors.Count, "Generated script must reparse without errors. Actual:\n" + generated);
        }
    }
}
