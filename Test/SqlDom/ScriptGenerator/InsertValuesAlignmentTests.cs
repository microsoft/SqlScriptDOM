//------------------------------------------------------------------------------
// <copyright file="InsertValuesAlignmentTests.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using Microsoft.SqlServer.TransactSql.ScriptDom;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlStudio.Tests.AssemblyTools.TestCategory;
using static SqlStudio.Tests.UTSqlScriptDom.ScriptGeneratorTestHelper;
namespace SqlStudio.Tests.UTSqlScriptDom
{
    // Tests for the "river" that lines up an INSERT statement's VALUES row constructors under its
    // column list (e.g. "INSERT INTO t (a, b)" / "VALUES      (1, 2)"). The opening parenthesis of
    // the VALUES row constructor lines up exactly with the opening parenthesis of the target's
    // column list, regardless of how long the target name or column list is.
    // Continuation rows (the 2nd, 3rd, ... row constructors) always align under the first row's
    // column - either under the river's column (default Aligned behavior), or at the fixed indent
    // used when the row constructors are moved to their own line (Indented + AlignClauseBodies =
    // false).
    [TestClass]
    public class InsertValuesAlignmentTests
    {
        // -----------------------------------------------------------------------------------------
        // Default behavior: continuation rows align under the first row's column
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestDefaultAlignsValuesRowUnderColumnList()
        {
            // Default options: the VALUES row's opening parenthesis lines up exactly under the
            // target column list's opening parenthesis, even for a long target/column list.
            // Continuation rows align under the first row's column.
            const string input =
                "INSERT INTO dbo.Nurses ([NurseID], [FNme], [LNme], [Spclty], [Crt], [CrtDt], [DtCrtd], [DtLst]) " +
                "VALUES (1, 'Susie', 'Derkins', 'Cardiology', 1, GETDATE() - 23139, GETDATE() - 2319, GETDATE()), " +
                "(2, 'Jo', 'Harding', 'Radiology', 1, GETDATE() - 23139, GETDATE() - 2319, GETDATE());";
            var options = new SqlScriptGeneratorOptions();
            const string expected =
@"
INSERT  INTO dbo.Nurses ([NurseID], [FNme], [LNme], [Spclty], [Crt], [CrtDt], [DtCrtd], [DtLst])
VALUES                  (1, 'Susie', 'Derkins', 'Cardiology', 1, GETDATE() - 23139, GETDATE() - 2319, GETDATE()),
                        (2, 'Jo', 'Harding', 'Radiology', 1, GETDATE() - 23139, GETDATE() - 2319, GETDATE());";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestDefaultAlignsShortColumnListToo()
        {
            const string input = "INSERT INTO t1 (c1, c2) VALUES (1, 2), (3, 4);";
            var options = new SqlScriptGeneratorOptions();
            const string expected =
@"
INSERT  INTO t1 (c1, c2)
VALUES          (1, 2),
                (3, 4);";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestDefaultAlignsSingleColumnList()
        {
            // Edge case: a single-column target list still lines up its VALUES row constructors
            // exactly under the target list's opening parenthesis.
            const string input = "INSERT INTO t (a) VALUES (1), (2);";
            var options = new SqlScriptGeneratorOptions();
            const string expected =
@"
INSERT  INTO t (a)
VALUES         (1),
               (2);";

            AssertGenerated(input, options, expected);
        }

        // -----------------------------------------------------------------------------------------
        // AlignClauseBodies = false alone: does not move the first VALUES row (only Indented +
        // AlignClauseBodies = false does that - see below), but continuation rows still align under
        // the first row's column, same as the default.
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestAlignClauseBodiesDisabledAloneAlignsValuesRowUnderColumnList()
        {
            const string input =
                "INSERT INTO dbo.Nurses ([NurseID], [FNme], [LNme], [Spclty], [Crt], [CrtDt], [DtCrtd], [DtLst]) " +
                "VALUES (1, 'Susie', 'Derkins', 'Cardiology', 1, GETDATE() - 23139, GETDATE() - 2319, GETDATE()), " +
                "(2, 'Jo', 'Harding', 'Radiology', 1, GETDATE() - 23139, GETDATE() - 2319, GETDATE());";
            var options = new SqlScriptGeneratorOptions { AlignClauseBodies = false };
            const string expected =
@"
INSERT INTO dbo.Nurses ([NurseID], [FNme], [LNme], [Spclty], [Crt], [CrtDt], [DtCrtd], [DtLst])
VALUES                 (1, 'Susie', 'Derkins', 'Cardiology', 1, GETDATE() - 23139, GETDATE() - 2319, GETDATE()),
                       (2, 'Jo', 'Harding', 'Radiology', 1, GETDATE() - 23139, GETDATE() - 2319, GETDATE());";

            AssertGenerated(input, options, expected);
        }

        // -----------------------------------------------------------------------------------------
        // ClauseBodyAlignment = Indented alone: also does not move the first VALUES row, since
        // AlignClauseBodies is still true (the default). Continuation rows still align under the
        // first row's column, same as the default.
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIndentedAloneAlignsValuesRowUnderColumnList()
        {
            const string input =
                "INSERT INTO dbo.Nurses ([NurseID], [FNme], [LNme], [Spclty], [Crt], [CrtDt], [DtCrtd], [DtLst]) " +
                "VALUES (1, 'Susie', 'Derkins', 'Cardiology', 1, GETDATE() - 23139, GETDATE() - 2319, GETDATE()), " +
                "(2, 'Jo', 'Harding', 'Radiology', 1, GETDATE() - 23139, GETDATE() - 2319, GETDATE());";
            var options = new SqlScriptGeneratorOptions { ClauseBodyAlignment = ClauseBodyAlignment.Indented };
            const string expected =
@"
INSERT INTO dbo.Nurses ([NurseID], [FNme], [LNme], [Spclty], [Crt], [CrtDt], [DtCrtd], [DtLst])
VALUES                 (1, 'Susie', 'Derkins', 'Cardiology', 1, GETDATE() - 23139, GETDATE() - 2319, GETDATE()),
                       (2, 'Jo', 'Harding', 'Radiology', 1, GETDATE() - 23139, GETDATE() - 2319, GETDATE());";

            AssertGenerated(input, options, expected);
        }

        // -----------------------------------------------------------------------------------------
        // ClauseBodyAlignment = Indented and AlignClauseBodies = false together: the VALUES row
        // constructors move to their own indented line instead of being padded onto the VALUES line,
        // and every row constructor (not just the first) is indented consistently.
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIndentedWithAlignClauseBodiesDisabledMovesValuesRowToNewLine()
        {
            const string input =
                "INSERT INTO dbo.Nurses ([NurseID], [FNme], [LNme], [Spclty], [Crt], [CrtDt], [DtCrtd], [DtLst]) " +
                "VALUES (1, 'Susie', 'Derkins', 'Cardiology', 1, GETDATE() - 23139, GETDATE() - 2319, GETDATE()), " +
                "(2, 'Jo', 'Harding', 'Radiology', 1, GETDATE() - 23139, GETDATE() - 2319, GETDATE());";
            var options = new SqlScriptGeneratorOptions
            {
                ClauseBodyAlignment = ClauseBodyAlignment.Indented,
                AlignClauseBodies = false,
            };
            const string expected =
@"
INSERT INTO dbo.Nurses ([NurseID], [FNme], [LNme], [Spclty], [Crt], [CrtDt], [DtCrtd], [DtLst])
VALUES
    (1, 'Susie', 'Derkins', 'Cardiology', 1, GETDATE() - 23139, GETDATE() - 2319, GETDATE()),
    (2, 'Jo', 'Harding', 'Radiology', 1, GETDATE() - 23139, GETDATE() - 2319, GETDATE());";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIndentedWithAlignClauseBodiesDisabledMovesShortValuesRowToNewLine()
        {
            const string input = "INSERT INTO t1 (c1, c2) VALUES (1, 2), (3, 4);";
            var options = new SqlScriptGeneratorOptions
            {
                ClauseBodyAlignment = ClauseBodyAlignment.Indented,
                AlignClauseBodies = false,
            };
            const string expected =
@"
INSERT INTO t1 (c1, c2)
VALUES
    (1, 2),
    (3, 4);";

            AssertGenerated(input, options, expected);
        }

        // -----------------------------------------------------------------------------------------
        // CommaPlacement = Leading: continuation rows carry a right-aligned leading comma so that
        // every row constructor's opening parenthesis still lines up under the first row's column.
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestLeadingCommaAlignsValuesRowUnderColumnList()
        {
            const string input = "INSERT INTO t1 (c1, c2) VALUES (1, 2), (3, 4);";
            var options = new SqlScriptGeneratorOptions { CommaPlacement = CommaPlacement.Leading };
            const string expected =
@"
INSERT  INTO t1 (c1, c2)
VALUES          (1, 2)
              , (3, 4);";

            AssertGenerated(input, options, expected);
        }

        // -----------------------------------------------------------------------------------------
        // MERGE ... WHEN NOT MATCHED THEN INSERT (...) VALUES (...) routes through the same
        // ValuesInsertSource path. MERGE permits only a single VALUES row, so it exercises the
        // path where the column-list alignment point is registered but never re-marked.
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMergeInsertActionValuesGenerates()
        {
            const string input =
                "MERGE INTO t AS tgt USING s AS src ON tgt.c1 = src.c1 " +
                "WHEN NOT MATCHED THEN INSERT (c1, c2) VALUES (src.c1, src.c2);";
            var options = new SqlScriptGeneratorOptions();
            const string expected =
@"
MERGE INTO t
 AS tgt
USING s AS src ON tgt.c1 = src.c1
WHEN NOT MATCHED THEN INSERT (c1, c2) VALUES (src.c1, src.c2);";

            AssertGenerated(input, options, expected);
        }

        // -----------------------------------------------------------------------------------------
        // A ValuesInsertSource scripted on its own has no column-list alignment point (that point is
        // registered by the enclosing INSERT/MERGE). Generation must still succeed and emit every
        // row instead of dereferencing the missing point.
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestBareValuesInsertSourceGeneratesWithoutColumnAlignmentPoint()
        {
            var source = new ValuesInsertSource();
            source.RowValues.Add(new RowValue { ColumnValues = { new IntegerLiteral { Value = "1" } } });
            source.RowValues.Add(new RowValue { ColumnValues = { new IntegerLiteral { Value = "2" } } });

            var options = new SqlScriptGeneratorOptions
            {
                ClauseBodyAlignment = ClauseBodyAlignment.Indented,
                AlignClauseBodies = false,
                CommaPlacement = CommaPlacement.Leading,
            };
            var generator = new Sql170ScriptGenerator(options);
            generator.GenerateScript(source, out string generated);

            const string expected =
@"
VALUES
    (1)
  , (2)";

            Assert.AreEqual(Normalize(expected).Trim(), Normalize(generated).Trim());
        }

        // -----------------------------------------------------------------------------------------
        // INSERT ... DEFAULT VALUES takes the IsDefaultValues branch of the VALUES source (no row
        // constructors, so no river alignment).
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestDefaultValuesSource()
        {
            const string input = "INSERT INTO t DEFAULT VALUES;";
            var options = new SqlScriptGeneratorOptions();
            const string expected =
@"
INSERT  INTO t
DEFAULT VALUES;";

            AssertGenerated(input, options, expected);
        }
    }
}
