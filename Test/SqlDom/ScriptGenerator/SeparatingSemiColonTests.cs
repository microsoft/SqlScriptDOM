//------------------------------------------------------------------------------
// <copyright file="SeparatingSemiColonTests.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using Microsoft.SqlServer.TransactSql.ScriptDom;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlStudio.Tests.AssemblyTools.TestCategory;
using static SqlStudio.Tests.UTSqlScriptDom.ScriptGeneratorTestHelper;

namespace SqlStudio.Tests.UTSqlScriptDom
{
    // Tests for the separating-semicolon behavior of the script generator. SQL Server requires the
    // statement that precedes a statement beginning with a WITH clause (a common table expression or
    // XMLNAMESPACES) or a THROW statement to be terminated with a semicolon. The generator does not
    // append a semicolon to block statements (IF / BEGIN...END / WHILE / TRY...CATCH), so when such a
    // block is followed by a CTE or THROW the generator injects the required separating semicolon so
    // the generated script is valid for SQL Server.
    [TestClass]
    public class SeparatingSemiColonTests
    {
        // -----------------------------------------------------------------------------------------
        // CTE following a block
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCteAfterBeginEndBlockGetsSeparatingSemicolon()
        {
            const string input =
@"DECLARE @x INT;
IF @x IS NULL BEGIN SELECT 1; END
;WITH cte AS (SELECT 1 AS c) SELECT * FROM cte";
            var options = new SqlScriptGeneratorOptions();
            const string expected =
@"
DECLARE @x AS INT;

IF @x IS NULL
    BEGIN
        SELECT 1;
    END;

WITH   cte
AS     (SELECT 1 AS c)
SELECT *
FROM   cte;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCteAfterIfStatementWithoutBlockHasNoDoubleSemicolon()
        {
            // The IF's then-statement already ends with its own semicolon, so no additional
            // separating semicolon is injected (no "SELECT 0;;").
            const string input =
@"DECLARE @x INT;
IF @x IS NULL SELECT 0
;WITH cte AS (SELECT 1 AS c) SELECT * FROM cte";
            var options = new SqlScriptGeneratorOptions();
            const string expected =
@"
DECLARE @x AS INT;

IF @x IS NULL
    SELECT 0;

WITH   cte
AS     (SELECT 1 AS c)
SELECT *
FROM   cte;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCteAfterWhileBlockGetsSeparatingSemicolon()
        {
            const string input =
@"DECLARE @x INT = 0;
WHILE @x < 1 BEGIN SET @x = @x + 1; END
;WITH cte AS (SELECT 1 AS c) SELECT * FROM cte";
            var options = new SqlScriptGeneratorOptions();
            const string expected =
@"
DECLARE @x AS INT = 0;

WHILE @x < 1
    BEGIN
        SET @x = @x + 1;
    END;

WITH   cte
AS     (SELECT 1 AS c)
SELECT *
FROM   cte;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCteAfterTryCatchBlockGetsSeparatingSemicolon()
        {
            const string input =
@"BEGIN TRY SELECT 1; END TRY BEGIN CATCH SELECT 2; END CATCH
;WITH cte AS (SELECT 1 AS c) SELECT * FROM cte";
            var options = new SqlScriptGeneratorOptions();
            const string expected =
@"
BEGIN TRY
    SELECT 1;
END TRY
BEGIN CATCH
    SELECT 2;
END CATCH;

WITH   cte
AS     (SELECT 1 AS c)
SELECT *
FROM   cte;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestXmlNamespacesAfterBeginEndBlockGetsSeparatingSemicolon()
        {
            // A WITH XMLNAMESPACES statement (the other kind of statement that begins with WITH)
            // requires the same preceding semicolon as a common table expression.
            const string input =
@"DECLARE @x INT;
IF @x IS NULL BEGIN SELECT 1; END
WITH XMLNAMESPACES ('uri' AS ns) SELECT 1 AS c";
            var options = new SqlScriptGeneratorOptions();
            const string expected =
@"
DECLARE @x AS INT;

IF @x IS NULL
    BEGIN
        SELECT 1;
    END;

WITH   XMLNAMESPACES ('uri' AS ns)
SELECT 1 AS c;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCteInsertAfterBeginEndBlockGetsSeparatingSemicolon()
        {
            // A common table expression can feed any DML statement, not just SELECT. Here the CTE
            // feeds an INSERT that follows a block, so the separating semicolon is still required.
            const string input =
@"DECLARE @x INT;
IF @x IS NULL BEGIN SELECT 1; END
WITH cte AS (SELECT 1 AS c) INSERT INTO t (c) SELECT c FROM cte";
            var options = new SqlScriptGeneratorOptions();
            const string expected =
@"
DECLARE @x AS INT;

IF @x IS NULL
    BEGIN
        SELECT 1;
    END;

WITH cte
AS   (SELECT 1 AS c)
INSERT INTO t (c)
SELECT c
FROM   cte;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCteAfterIfElseBlockGetsSeparatingSemicolon()
        {
            // The IF statement ends with the ELSE branch's END (no terminator), so a CTE that
            // follows the whole IF...ELSE still needs the separating semicolon.
            const string input =
@"DECLARE @x INT;
IF @x = 1 BEGIN SELECT 1; END ELSE BEGIN SELECT 2; END
WITH cte AS (SELECT 1 AS c) SELECT * FROM cte";
            var options = new SqlScriptGeneratorOptions();
            const string expected =
@"
DECLARE @x AS INT;

IF @x = 1
    BEGIN
        SELECT 1;
    END
ELSE
    BEGIN
        SELECT 2;
    END;

WITH   cte
AS     (SELECT 1 AS c)
SELECT *
FROM   cte;";

            AssertGenerated(input, options, expected);
        }

        // -----------------------------------------------------------------------------------------
        // THROW following a block
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestThrowAfterBeginEndBlockGetsSeparatingSemicolon()
        {
            const string input =
@"DECLARE @x INT;
IF @x IS NULL BEGIN SELECT 1; END
;THROW 50001, 'e', 1";
            var options = new SqlScriptGeneratorOptions();
            const string expected =
@"
DECLARE @x AS INT;

IF @x IS NULL
    BEGIN
        SELECT 1;
    END;

THROW 50001, 'e', 1;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestThrowAfterWhileBlockGetsSeparatingSemicolon()
        {
            const string input =
@"DECLARE @x INT = 0;
WHILE @x < 1 BEGIN SET @x = @x + 1; END
THROW 50001, 'e', 1";
            var options = new SqlScriptGeneratorOptions();
            const string expected =
@"
DECLARE @x AS INT = 0;

WHILE @x < 1
    BEGIN
        SET @x = @x + 1;
    END;

THROW 50001, 'e', 1;";

            AssertGenerated(input, options, expected);
        }

        // -----------------------------------------------------------------------------------------
        // Cases that must NOT get an extra semicolon
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCteAfterTerminatedStatementHasNoDoubleSemicolon()
        {
            // The preceding SELECT already ends with a semicolon, so no separating semicolon is added.
            const string input =
@"SELECT 1
;WITH cte AS (SELECT 1 AS c) SELECT * FROM cte";
            var options = new SqlScriptGeneratorOptions();
            const string expected =
@"
SELECT 1;

WITH   cte
AS     (SELECT 1 AS c)
SELECT *
FROM   cte;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestThrowAfterTerminatedStatementHasNoDoubleSemicolon()
        {
            // The preceding SELECT already ends with a semicolon, so no separating semicolon is
            // added before the THROW.
            const string input =
@"SELECT 1
;THROW 50001, 'e', 1";
            var options = new SqlScriptGeneratorOptions();
            const string expected =
@"
SELECT 1;

THROW 50001, 'e', 1;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCteAsFirstStatementHasNoLeadingSemicolon()
        {
            const string input = "WITH cte AS (SELECT 1 AS c) SELECT * FROM cte";
            var options = new SqlScriptGeneratorOptions();
            const string expected =
@"
WITH   cte
AS     (SELECT 1 AS c)
SELECT *
FROM   cte;";

            AssertGenerated(input, options, expected);
        }

        // -----------------------------------------------------------------------------------------
        // Nested statement list (inside a procedure body)
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCteAfterBlockInsideProcedureBodyGetsSeparatingSemicolon()
        {
            // The input intentionally omits the semicolon before WITH. ScriptDom's parser is lenient
            // and accepts this, and the block statement's AST does not carry a terminator, so the
            // generator is responsible for injecting the semicolon SQL Server requires. (A leading
            // semicolon in the input would be discarded during parsing, so its presence or absence in
            // the input does not affect the generated output.)
            const string input =
@"CREATE PROCEDURE p @x INT AS BEGIN
IF @x IS NULL BEGIN SELECT 1; END
WITH cte AS (SELECT 1 AS c) SELECT * FROM cte
END";
            var options = new SqlScriptGeneratorOptions();
            const string expected =
@"
CREATE PROCEDURE p
@x INT
AS
BEGIN
    IF @x IS NULL
        BEGIN
            SELECT 1;
        END;
    WITH   cte
    AS     (SELECT 1 AS c)
    SELECT *
    FROM   cte;
END";

            AssertGenerated(input, options, expected);
        }

        // -----------------------------------------------------------------------------------------
        // Original reported repro: THROW inside a BEGIN...END block, followed by a CTE after the
        // block. Without the injected separating semicolon, SQL Server rejects the generated script
        // with error 319 ("Incorrect syntax near the keyword 'WITH'").
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestOriginalReproThrowInBlockFollowedByCteGetsSeparatingSemicolon()
        {
            const string input =
@"declare @ClientID varchar(100) = ''
IF @ClientID IS NULL
BEGIN
;THROW 50001, 'Client with PolicyID ''0001'' not found.', 1;
END
;WITH cte
AS (SELECT 1 AS Column1)
select * from cte";
            var options = new SqlScriptGeneratorOptions();
            const string expected =
@"
DECLARE @ClientID AS VARCHAR (100) = '';

IF @ClientID IS NULL
    BEGIN
        THROW 50001, 'Client with PolicyID ''0001'' not found.', 1;
    END;

WITH   cte
AS     (SELECT 1 AS Column1)
SELECT *
FROM   cte;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCteAfterTerminatedStatementWithTrailingCommentHasNoDoubleSemicolon()
        {
            // With PreserveComments on, the previous statement's terminating semicolon is followed
            // by a trailing single-line comment. The separator scan must look past the comment and
            // detect the semicolon, so no redundant semicolon is written into the comment text.
            const string input =
@"SELECT 1; -- keep me
WITH cte AS (SELECT 1 AS c) SELECT * FROM cte";
            var options = new SqlScriptGeneratorOptions { PreserveComments = true };
            const string expected =
@"SELECT 1; -- keep me

WITH   cte
AS     (SELECT 1 AS c)
SELECT *
FROM   cte;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCteAfterTerminatedStatementWithTrailingBlockCommentHasNoDoubleSemicolon()
        {
            // Same as the single-line case but with a trailing multi-line comment, exercising the
            // MultilineComment branch of the separator scan.
            const string input =
@"SELECT 1; /* keep me */
WITH cte AS (SELECT 1 AS c) SELECT * FROM cte";
            var options = new SqlScriptGeneratorOptions { PreserveComments = true };
            const string expected =
@"SELECT 1; /* keep me */

WITH   cte
AS     (SELECT 1 AS c)
SELECT *
FROM   cte;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCteAfterBlockWithTrailingCommentGetsSeparatingSemicolon()
        {
            // A block ends with END (no terminator) and carries a trailing comment. The separator
            // scan must look past the comment, find END (not a semicolon), and inject the required
            // semicolon so the following CTE is valid, without corrupting the comment.
            const string input =
@"DECLARE @x INT;
IF @x IS NULL BEGIN SELECT 1; END -- trailing note
WITH cte AS (SELECT 1 AS c) SELECT * FROM cte";
            var options = new SqlScriptGeneratorOptions { PreserveComments = true };
            const string expected =
@"DECLARE @x AS INT;

IF @x IS NULL
    BEGIN
        SELECT 1;
    END; -- trailing note

WITH   cte
AS     (SELECT 1 AS c)
SELECT *
FROM   cte;";

            AssertGenerated(input, options, expected);
        }
    }
}
