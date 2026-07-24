//------------------------------------------------------------------------------
// <copyright file="IdentifierFormattingTests.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlStudio.Tests.AssemblyTools.TestCategory;
using static SqlStudio.Tests.UTSqlScriptDom.ScriptGeneratorTestHelper;

namespace SqlStudio.Tests.UTSqlScriptDom
{
    // Tests for the IdentifierCasing and IdentifierBracketing script-generation options.
    // Kept in a dedicated file to avoid churn in ScriptGeneratorTests.cs.
    //
    // The default option values (IdentifierCasing.Preserve and IdentifierBracketing.Preserve) leave
    // generated script unchanged versus the previous formatter behavior. The two "DefaultIs..." tests
    // pin the defaults, and TestDefaultOptionsArePureIdentity checks the default path is a no-op
    // across every construct the change touches.
    //
    // Expected values follow the JoinClauseFormattingTests pattern: verbatim @"..." literals compared
    // via the shared AssertGenerated helper (which normalizes line endings and trims).
    //
    // Work item: Formatter option: Identifier bracketing and casing
    [TestClass]
    public class IdentifierFormattingTests
    {
        // Builds options that toggle the identifier casing/bracketing under test while disabling
        // unrelated multi-line/alignment formatting, so the expected literals stay focused on the
        // identifier transformation itself.
        private static SqlScriptGeneratorOptions MakeOptions(IdentifierCasing casing, IdentifierBracketing bracketing)
        {
            return new SqlScriptGeneratorOptions
            {
                IdentifierCasing = casing,
                IdentifierBracketing = bracketing,
                AlignColumnDefinitionFields = false,
                AlignClauseBodies = false,
                NewLineBeforeFromClause = false,
                MultilineSelectElementsList = false,
            };
        }

        // Parses with the given (version-specific) parser and generates with the matching generator,
        // so the ExcludeBrackets reserved-word probe runs against that SqlVersion. Used to demonstrate
        // version-specific bracketing behavior.
        private static void AssertGeneratedWith(
            TSqlParser parser,
            SqlScriptGenerator generator,
            string sql,
            string expected)
        {
            TSqlFragment fragment = parser.Parse(new StringReader(sql), out IList<ParseError> errors);
            Assert.AreEqual(0, errors.Count, "Input must parse without errors.");

            generator.GenerateScript(fragment, out string generated);
            Assert.AreEqual(Normalize(expected).Trim(), Normalize(generated).Trim());
        }

        // -----------------------------------------------------------------------------------------
        // Defaults
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIdentifierCasingDefaultIsPreserve()
        {
            Assert.AreEqual(IdentifierCasing.Preserve, new SqlScriptGeneratorOptions().IdentifierCasing);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIdentifierBracketingDefaultIsPreserve()
        {
            Assert.AreEqual(IdentifierBracketing.Preserve, new SqlScriptGeneratorOptions().IdentifierBracketing);
        }

        // With the default (Preserve/Preserve) options the identifier code path is a pure identity
        // across every delimiter style and every construct the change touches (pre-bracketed,
        // double-quoted, plain, @variable, schema-qualified + built-in function names, and a label).
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestDefaultOptionsArePureIdentity()
        {
            const string input = @"
GOTO MyLabel;
MyLabel:
SELECT [Bracketed], ""Quoted"", Plain, @Var, dbo.MyFunc(Col), LEN(Col2) FROM [My Tbl];";
            var options = MakeOptions(IdentifierCasing.Preserve, IdentifierBracketing.Preserve);
            const string expected = @"
GOTO MyLabel;

MyLabel:

SELECT [Bracketed], ""Quoted"", Plain, @Var, dbo.MyFunc(Col), LEN(Col2) FROM [My Tbl];";

            AssertGenerated(input, options, expected);
        }

        // -----------------------------------------------------------------------------------------
        // IdentifierCasing
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIdentifierCasingPreserveLeavesIdentifiersUnchanged()
        {
            const string input = "SELECT MyCol FROM MyTbl;";
            var options = MakeOptions(IdentifierCasing.Preserve, IdentifierBracketing.Preserve);
            const string expected = @"SELECT MyCol FROM MyTbl;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIdentifierCasingLowercase()
        {
            // Identifiers lowered; keywords stay uppercase (governed by KeywordCasing).
            const string input = "SELECT MyCol FROM MyTbl;";
            var options = MakeOptions(IdentifierCasing.Lowercase, IdentifierBracketing.Preserve);
            const string expected = @"SELECT mycol FROM mytbl;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIdentifierCasingUppercase()
        {
            const string input = "SELECT MyCol FROM MyTbl;";
            var options = MakeOptions(IdentifierCasing.Uppercase, IdentifierBracketing.Preserve);
            const string expected = @"SELECT MYCOL FROM MYTBL;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIdentifierCasingDoesNotAffectStringLiteralsOrVariables()
        {
            // Interaction rule: casing must not touch @variables or string literals.
            const string input = "SELECT @MyVar, 'StrLit' FROM MyTbl;";
            var options = MakeOptions(IdentifierCasing.Lowercase, IdentifierBracketing.Preserve);
            const string expected = @"SELECT @MyVar, 'StrLit' FROM mytbl;";

            AssertGenerated(input, options, expected);
        }

        // -----------------------------------------------------------------------------------------
        // IdentifierBracketing
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIdentifierBracketingIncludeBracketsWrapsAllIdentifiers()
        {
            const string input = "SELECT MyCol FROM MyTbl;";
            var options = MakeOptions(IdentifierCasing.Preserve, IdentifierBracketing.IncludeBrackets);
            const string expected = @"SELECT [MyCol] FROM [MyTbl];";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIdentifierBracketingIncludeBracketsWrapsMultiPartName()
        {
            const string input = "SELECT MySchema.MyTbl.MyCol FROM MySchema.MyTbl;";
            var options = MakeOptions(IdentifierCasing.Preserve, IdentifierBracketing.IncludeBrackets);
            const string expected = @"SELECT [MySchema].[MyTbl].[MyCol] FROM [MySchema].[MyTbl];";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIdentifierBracketingExcludeBracketsRemovesUnnecessaryBrackets()
        {
            const string input = "SELECT [MyCol] FROM [MyTbl];";
            var options = MakeOptions(IdentifierCasing.Preserve, IdentifierBracketing.ExcludeBrackets);
            const string expected = @"SELECT MyCol FROM MyTbl;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIdentifierBracketingExcludeBracketsRetainsReservedWord()
        {
            // A reserved word (for the configured SqlVersion) must keep its brackets.
            const string input = "SELECT [SELECT] FROM [MyTbl];";
            var options = MakeOptions(IdentifierCasing.Preserve, IdentifierBracketing.ExcludeBrackets);
            const string expected = @"SELECT [SELECT] FROM MyTbl;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIdentifierBracketingExcludeBracketsRetainsSpecialCharacters()
        {
            // An identifier containing a space must keep its brackets.
            const string input = "SELECT [My Col] FROM [MyTbl];";
            var options = MakeOptions(IdentifierCasing.Preserve, IdentifierBracketing.ExcludeBrackets);
            const string expected = @"SELECT [My Col] FROM MyTbl;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIdentifierBracketingIncludeBracketsWrapsAliasDeclarationsAndReferences()
        {
            // The work item requires IncludeBrackets to wrap alias declarations and all references:
            // the table alias (x), the column alias (a), and the references (t, c) are all bracketed.
            const string input = "SELECT c AS a FROM t AS x;";
            var options = MakeOptions(IdentifierCasing.Preserve, IdentifierBracketing.IncludeBrackets);
            const string expected = @"SELECT [c] AS [a] FROM [t] AS [x];";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIdentifierBracketingIncludeBracketsDoesNotDoubleBracket()
        {
            // Already-bracketed identifiers stay singly bracketed (no [[MyCol]]).
            const string input = "SELECT [MyCol] FROM [MyTbl];";
            var options = MakeOptions(IdentifierCasing.Preserve, IdentifierBracketing.IncludeBrackets);
            const string expected = @"SELECT [MyCol] FROM [MyTbl];";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIdentifierBracketingIncludeBracketsEscapesClosingBracket()
        {
            // A closing bracket inside the identifier value must be escaped as ]] when bracketing.
            const string input = "SELECT [a]]b] FROM MyTbl;";
            var options = MakeOptions(IdentifierCasing.Preserve, IdentifierBracketing.IncludeBrackets);
            const string expected = @"SELECT [a]]b] FROM [MyTbl];";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIdentifierBracketingExcludeBracketsRetainsEscapedClosingBracket()
        {
            // An identifier containing a closing bracket must keep (and re-escape) its brackets.
            const string input = "SELECT [a]]b] FROM [MyTbl];";
            var options = MakeOptions(IdentifierCasing.Preserve, IdentifierBracketing.ExcludeBrackets);
            const string expected = @"SELECT [a]]b] FROM MyTbl;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIdentifierBracketingExcludeBracketsPreservesDoubleQuotedIdentifiers()
        {
            // ExcludeBrackets controls square brackets only, so double-quoted identifiers keep their
            // double quotes - both the reserved word and the regular name are preserved.
            const string input = "SELECT \"SELECT\" FROM \"MyTbl\";";
            var options = MakeOptions(IdentifierCasing.Preserve, IdentifierBracketing.ExcludeBrackets);
            const string expected = @"SELECT ""SELECT"" FROM ""MyTbl"";";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIdentifierBracketingExcludeBracketsReservedWordIsVersionSpecific()
        {
            // The ExcludeBrackets reserved-word check honors the configured SqlVersion. PIVOT is a
            // keyword starting with SQL Server 2005 but a regular identifier in SQL Server 2000 (80),
            // so the same input keeps its brackets under 170 and drops them under 80.
            const string input = "SELECT [PIVOT] FROM [MyTbl];";

            // SQL Server 2000 (80): PIVOT is a regular identifier, so its brackets are removed.
            AssertGeneratedWith(
                new TSql80Parser(true),
                new Sql80ScriptGenerator(MakeOptions(IdentifierCasing.Preserve, IdentifierBracketing.ExcludeBrackets)),
                input,
                @"SELECT PIVOT FROM MyTbl;");

            // SQL Server 2025 (170): PIVOT is a keyword, so it retains its brackets.
            AssertGeneratedWith(
                new TSql170Parser(true),
                new Sql170ScriptGenerator(MakeOptions(IdentifierCasing.Preserve, IdentifierBracketing.ExcludeBrackets)),
                input,
                @"SELECT [PIVOT] FROM MyTbl;");
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIdentifierBracketingExcludeBracketsLeavesUnquotedIdentifiersUntouched()
        {
            // Coverage: ExcludeBrackets on already-unquoted identifiers is a no-op (the probe is
            // skipped for QuoteType.NotQuoted); the identifiers pass through unchanged.
            const string input = "SELECT MyCol FROM MyTbl;";
            var options = MakeOptions(IdentifierCasing.Preserve, IdentifierBracketing.ExcludeBrackets);
            const string expected = @"SELECT MyCol FROM MyTbl;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIdentifierBracketingExcludeBracketsRetainsOpeningBracketSpecialChar()
        {
            // Coverage: a value containing an unescaped '[' cannot be re-lexed as a single identifier
            // (it produces a lex error), so the probe reports it as needing brackets and they are kept.
            const string input = "SELECT [a[b] FROM [MyTbl];";
            var options = MakeOptions(IdentifierCasing.Preserve, IdentifierBracketing.ExcludeBrackets);
            const string expected = @"SELECT [a[b] FROM MyTbl;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestSuppressedFunctionNamePreservesOriginalBracketingAndCasing()
        {
            // Coverage: a bracketed function name exercises the suppressed + quoted emit path. Under
            // IncludeBrackets + Uppercase the function name keeps its exact original form ([MyFunc]),
            // while the schema, argument, and table are still bracketed and uppercased.
            const string input = "SELECT dbo.[MyFunc](Col) FROM MyTbl;";
            var options = MakeOptions(IdentifierCasing.Uppercase, IdentifierBracketing.IncludeBrackets);
            const string expected = @"SELECT [DBO].[MyFunc]([COL]) FROM [MYTBL];";

            AssertGenerated(input, options, expected);
        }

        // -----------------------------------------------------------------------------------------
        // Interaction: casing is applied after bracketing
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIdentifierBracketingAndCasingCombined()
        {
            // ExcludeBrackets removes safe brackets, then Lowercase recases the value; the
            // reserved word retains its brackets but is still recased.
            const string input = "SELECT [SELECT], [MyCol] FROM [MyTbl];";
            var options = MakeOptions(IdentifierCasing.Lowercase, IdentifierBracketing.ExcludeBrackets);
            const string expected = @"SELECT [select], mycol FROM mytbl;";

            AssertGenerated(input, options, expected);
        }

        // =========================================================================================
        // GAP documentation
        //
        // The identifier options are applied centrally in ExplicitVisit(Identifier). The tests below
        // document intentional boundaries and safe-choice behaviors that are NOT spelled out in the
        // work item but are required to avoid generating invalid T-SQL or to keep the scope sane.
        // =========================================================================================

        // GAP-LABEL: GoToStatement.LabelName is an Identifier fragment, but T-SQL labels cannot be
        // delimited (GOTO [x] is invalid) and the reference must keep matching the never-recased
        // label declaration emitted by LabelStatement. Therefore identifier formatting is suppressed
        // for label references: neither IncludeBrackets nor casing is applied.
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestGapLabelIncludeBracketsDoesNotBracketLabel()
        {
            // The label declaration (MyLabel:) and the GOTO reference both stay unbracketed, while a
            // regular identifier (MyCol) in the same batch is still wrapped in brackets.
            const string input = @"
GOTO MyLabel;
MyLabel:
SELECT MyCol;";
            var options = MakeOptions(IdentifierCasing.Preserve, IdentifierBracketing.IncludeBrackets);
            const string expected = @"
GOTO MyLabel;

MyLabel:

SELECT [MyCol];";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestGapLabelCasingDoesNotRecaseLabel()
        {
            const string input = "GOTO MyLabel;";
            var options = MakeOptions(IdentifierCasing.Lowercase, IdentifierBracketing.Preserve);
            const string expected = @"GOTO MyLabel;";

            AssertGenerated(input, options, expected);
        }

        // GAP-FUNC-SCOPE: Only the function's own name is excluded from formatting. A schema
        // qualifier on a user-defined function (dbo in dbo.MyFunc) IS still transformed, because
        // bracketing/recasing a schema is valid T-SQL. This documents the boundary of the
        // "function names" exclusion (built-in/function-name casing is a separate future option).
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestGapFunctionNamePreservedButSchemaQualifierTransformed()
        {
            const string input = "SELECT dbo.MyFunc(MyCol) FROM MyTbl;";
            var options = MakeOptions(IdentifierCasing.Lowercase, IdentifierBracketing.Preserve);
            const string expected = @"SELECT dbo.MyFunc(mycol) FROM mytbl;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestGapBuiltInFunctionNamePreserved()
        {
            // A built-in function name (LEN) is left untouched while its argument identifier is recased.
            const string input = "SELECT LEN(MyCol) FROM MyTbl;";
            var options = MakeOptions(IdentifierCasing.Lowercase, IdentifierBracketing.Preserve);
            const string expected = @"SELECT LEN(mycol) FROM mytbl;";

            AssertGenerated(input, options, expected);
        }

        // GAP-PASCAL: PascalCase reuses the existing GetPascalCase helper, which only capitalizes the
        // first character of the whole value (not per word). A quoted multi-word identifier therefore
        // becomes "[My col]", not "[My Col]".
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestGapPascalCaseOnlyCapitalizesFirstCharacter()
        {
            const string input = "SELECT [my col] FROM MYTBL;";
            var options = MakeOptions(IdentifierCasing.PascalCase, IdentifierBracketing.Preserve);
            const string expected = @"SELECT [My col] FROM Mytbl;";

            AssertGenerated(input, options, expected);
        }

        // GAP-DQUOTE: IdentifierBracketing controls square brackets only. IncludeBrackets normalizes an
        // identifier to square brackets (a double-quoted "MyCol" becomes [MyCol]); ExcludeBrackets
        // removes square brackets only and preserves other quote types (a double-quoted "MyCol" stays
        // "MyCol").
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestGapDoubleQuotedIdentifierIncludeBracketsConvertsToSquareBrackets()
        {
            const string input = "SELECT \"MyCol\" FROM \"MyTbl\";";
            var options = MakeOptions(IdentifierCasing.Preserve, IdentifierBracketing.IncludeBrackets);
            const string expected = @"SELECT [MyCol] FROM [MyTbl];";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestGapDoubleQuotedIdentifierExcludeBracketsPreservesQuoting()
        {
            const string input = "SELECT \"MyCol\" FROM \"MyTbl\";";
            var options = MakeOptions(IdentifierCasing.Preserve, IdentifierBracketing.ExcludeBrackets);
            const string expected = @"SELECT ""MyCol"" FROM ""MyTbl"";";

            AssertGenerated(input, options, expected);
        }

        // GAP-VARIABLE: variable and parameter names are Identifier fragments whose value starts with
        // '@'. They must never be bracketed ('[@x]' is invalid T-SQL) or recased (the interaction
        // rules exclude @variables), so the options leave them untouched while surrounding object
        // identifiers are still transformed.
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestGapVariableDeclarationIncludeBracketsDoesNotBracketVariable()
        {
            const string input = "DECLARE @MyVar INT;";
            var options = MakeOptions(IdentifierCasing.Preserve, IdentifierBracketing.IncludeBrackets);
            const string expected = @"DECLARE @MyVar AS INT;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestGapVariableDeclarationCasingDoesNotRecaseVariable()
        {
            const string input = "DECLARE @MyVar INT;";
            var options = MakeOptions(IdentifierCasing.Uppercase, IdentifierBracketing.Preserve);
            const string expected = @"DECLARE @MyVar AS INT;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestGapDelimitedAtNameIsTransformedAsObjectIdentifier()
        {
            // A delimited [@Name] is an object identifier (QuoteType.SquareBracket), not a variable,
            // so - unlike an unquoted @variable - it still participates in the identifier options.
            const string input = "SELECT [@Name] FROM MyTbl;";
            var options = MakeOptions(IdentifierCasing.Uppercase, IdentifierBracketing.Preserve);
            const string expected = @"SELECT [@NAME] FROM MYTBL;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestGapTableVariableIncludeBracketsBracketsColumnsButNotVariable()
        {
            const string input = "DECLARE @MyTable TABLE (MyCol INT);";
            var options = MakeOptions(IdentifierCasing.Preserve, IdentifierBracketing.IncludeBrackets);
            const string expected = @"
DECLARE @MyTable TABLE (
    [MyCol] INT);";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestGapProcedureParameterIncludeBracketsBracketsProcNameButNotParameter()
        {
            const string input = "CREATE PROCEDURE MyProc @MyParam INT AS RETURN;";
            var options = MakeOptions(IdentifierCasing.Preserve, IdentifierBracketing.IncludeBrackets);
            const string expected = @"
CREATE PROCEDURE [MyProc]
@MyParam INT
AS
RETURN;";

            AssertGenerated(input, options, expected);
        }

        // GAP-TEMPTABLE: temp table names are Identifier fragments whose value starts with '#' (local)
        // or '##' (global). Unlike @variables, bracketing them is valid T-SQL ([#Temp] is legal), so
        // temp tables are intentionally NOT excluded: the options transform them like any other object
        // identifier.
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestGapLocalTempTableIncludeBracketsWrapsName()
        {
            const string input = "SELECT * FROM #Temp;";
            var options = MakeOptions(IdentifierCasing.Preserve, IdentifierBracketing.IncludeBrackets);
            const string expected = @"SELECT * FROM [#Temp];";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestGapLocalTempTableExcludeBracketsRemovesBrackets()
        {
            const string input = "SELECT * FROM [#Temp];";
            var options = MakeOptions(IdentifierCasing.Preserve, IdentifierBracketing.ExcludeBrackets);
            const string expected = @"SELECT * FROM #Temp;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestGapGlobalTempTableIncludeBracketsWrapsName()
        {
            const string input = "SELECT * FROM ##Temp;";
            var options = MakeOptions(IdentifierCasing.Preserve, IdentifierBracketing.IncludeBrackets);
            const string expected = @"SELECT * FROM [##Temp];";

            AssertGenerated(input, options, expected);
        }

        // GAP-SYNTAX-KEYWORD: some syntax options are stored in the AST as Identifier fragments even
        // though they are keywords (JSON ABSENT ON NULL, TRIM LEADING/TRAILING/BOTH, window
        // IGNORE/RESPECT NULLS). They must never be bracketed or recased - e.g. "[ABSENT] ON NULL" is
        // invalid T-SQL - so the options leave them untouched while real object identifiers around
        // them are still transformed.
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestGapJsonAbsentOnNullNotBracketed()
        {
            const string input = "SELECT JSON_OBJECT('a':1 ABSENT ON NULL);";
            var options = MakeOptions(IdentifierCasing.Preserve, IdentifierBracketing.IncludeBrackets);
            const string expected = @"SELECT JSON_OBJECT('a':1 ABSENT ON NULL);";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestGapTrimOptionNotBracketed()
        {
            const string input = "SELECT TRIM(LEADING ' ' FROM MyCol) FROM MyTbl;";
            var options = MakeOptions(IdentifierCasing.Preserve, IdentifierBracketing.IncludeBrackets);
            const string expected = @"SELECT TRIM( LEADING ' ' FROM [MyCol]) FROM [MyTbl];";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestGapIgnoreNullsNotBracketed()
        {
            const string input = "SELECT LAST_VALUE(MyCol) IGNORE NULLS OVER (ORDER BY MyId) FROM MyTbl;";
            var options = MakeOptions(IdentifierCasing.Preserve, IdentifierBracketing.IncludeBrackets);
            const string expected = @"SELECT LAST_VALUE([MyCol]) IGNORE NULLS OVER (ORDER BY [MyId]) FROM [MyTbl];";

            AssertGenerated(input, options, expected);
        }

        // GAP-COLLATE: a collation name is stored as an Identifier fragment but cannot be delimited
        // (COLLATE [name] is invalid T-SQL) or recased, so the options leave it untouched while the
        // surrounding column/table identifiers are still transformed.
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestGapCollationNameNotBracketed()
        {
            const string input = "SELECT MyCol COLLATE SQL_Latin1_General_CP1_CI_AS FROM MyTbl;";
            var options = MakeOptions(IdentifierCasing.Preserve, IdentifierBracketing.IncludeBrackets);
            const string expected = @"SELECT [MyCol] COLLATE SQL_Latin1_General_CP1_CI_AS FROM [MyTbl];";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestGapCollationNameNotRecased()
        {
            const string input = "SELECT MyCol COLLATE SQL_Latin1_General_CP1_CI_AS FROM MyTbl;";
            var options = MakeOptions(IdentifierCasing.Lowercase, IdentifierBracketing.Preserve);
            const string expected = @"SELECT mycol COLLATE SQL_Latin1_General_CP1_CI_AS FROM mytbl;";

            AssertGenerated(input, options, expected);
        }

        // GAP-EMPTY-VALUE: an empty Identifier.Value must be returned unchanged for every casing.
        // PascalCase would otherwise index str[0] and throw during script generation.
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestEmptyIdentifierValueIsNotRecased()
        {
            Assert.AreEqual(string.Empty, ScriptGeneratorSupporter.GetCasedString(string.Empty, IdentifierCasing.PascalCase));
            Assert.AreEqual(string.Empty, ScriptGeneratorSupporter.GetCasedString(string.Empty, IdentifierCasing.Uppercase));
            Assert.AreEqual(string.Empty, ScriptGeneratorSupporter.GetCasedString(string.Empty, IdentifierCasing.Lowercase));
            Assert.AreEqual(string.Empty, ScriptGeneratorSupporter.GetCasedString(string.Empty, IdentifierCasing.Preserve));
        }

        // GAP-MULTIPART: an omitted component of a multi-part name (e.g. the missing schema in
        // "db..t") is an Identifier fragment with an empty Value. IncludeBrackets must preserve it as
        // an empty, unquoted component - bracketing it to "[]" produces invalid T-SQL (SQL46010).
        // Present parts are still bracketed.
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestGapMultipartOmittedComponentNotBracketed()
        {
            const string input = "SELECT * FROM db..t;";
            var options = MakeOptions(IdentifierCasing.Preserve, IdentifierBracketing.IncludeBrackets);
            const string expected = @"SELECT * FROM [db]..[t];";

            AssertGenerated(input, options, expected);
        }

        // GAP-MULTIPART (four-part): omitted parts in a server-qualified name are likewise preserved,
        // whether the omitted part is in the middle or leading interior position.
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestGapFourPartOmittedComponentsNotBracketed()
        {
            var options = MakeOptions(IdentifierCasing.Preserve, IdentifierBracketing.IncludeBrackets);

            AssertGenerated("SELECT * FROM srv.db..t;", options, @"SELECT * FROM [srv].[db]..[t];");
            AssertGenerated("SELECT * FROM srv..sch.t;", options, @"SELECT * FROM [srv]..[sch].[t];");
        }

        // -----------------------------------------------------------------------------------------
        // GAP-KEYWORD-POSITION: further Identifier fragments that hold keywords / syntax words rather
        // than object names. Bracketing or recasing them produces invalid T-SQL, so the emit sites
        // suppress identifier formatting for them while real object identifiers around them are still
        // transformed. Each test asserts the complete generated script (AssertGenerated also reparses
        // the output, which is the round-trip invariant each of these fixes restores).
        // -----------------------------------------------------------------------------------------

        // GAP-PERMISSION: permission names (ALTER, ANY, SCHEMA, ...) are Identifier fragments but are
        // keywords - "GRANT [ALTER] [ANY] [SCHEMA]" is invalid. The grantee is still bracketed.
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestGapPermissionNamesNotBracketed()
        {
            const string input = "GRANT ALTER ANY SCHEMA TO user1;";
            var options = MakeOptions(IdentifierCasing.Preserve, IdentifierBracketing.IncludeBrackets);
            const string expected = @"GRANT ALTER ANY SCHEMA TO [user1];";

            AssertGenerated(input, options, expected);
        }

        // GAP-PERMISSION (GRANT): Permission.Identifiers holds the syntax keyword SELECT, not an object
        // name; "GRANT [SELECT]" is rejected by the parser. The securable and principal are bracketed.
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestGapPermissionGrantSelectNotBracketed()
        {
            const string input = "GRANT SELECT ON OBJECT::dbo.t TO u;";
            var options = MakeOptions(IdentifierCasing.Preserve, IdentifierBracketing.IncludeBrackets);
            const string expected =
@"GRANT SELECT
    ON OBJECT::[dbo].[t] TO [u];";

            AssertGenerated(input, options, expected);
        }

        // GAP-PERMISSION (DENY): same rule for the DENY variant.
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestGapPermissionDenySelectNotBracketed()
        {
            const string input = "DENY SELECT ON OBJECT::dbo.t TO u;";
            var options = MakeOptions(IdentifierCasing.Preserve, IdentifierBracketing.IncludeBrackets);
            const string expected =
@"DENY SELECT
    ON OBJECT::[dbo].[t] TO [u];";

            AssertGenerated(input, options, expected);
        }

        // GAP-PERMISSION (REVOKE): same rule for the REVOKE variant (the generator normalizes REVOKE
        // ... FROM to REVOKE ... TO, which round-trips).
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestGapPermissionRevokeSelectNotBracketed()
        {
            const string input = "REVOKE SELECT ON OBJECT::dbo.t FROM u;";
            var options = MakeOptions(IdentifierCasing.Preserve, IdentifierBracketing.IncludeBrackets);
            const string expected =
@"REVOKE SELECT
    ON OBJECT::[dbo].[t] TO [u];";

            AssertGenerated(input, options, expected);
        }

        // GAP-ODBC: the ODBC escape-function name and the ODBC data-type name are Identifier fragments
        // holding keywords; "{ fn [convert] (...) }" / "[SQL_INTEGER]" are invalid.
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestGapOdbcConvertNotBracketed()
        {
            const string input = "SELECT { fn convert(MyCol, SQL_INTEGER) } FROM MyTbl;";
            var options = MakeOptions(IdentifierCasing.Preserve, IdentifierBracketing.IncludeBrackets);
            const string expected = @"SELECT { FN convert ([MyCol], SQL_INTEGER) } FROM [MyTbl];";

            AssertGenerated(input, options, expected);
        }

        // GAP-FEDERATION: the federation name must be a plain identifier ("USE FEDERATION [f1]" is
        // invalid), but the distribution name is a normal identifier and is still bracketed.
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestGapFederationNameNotBracketed()
        {
            const string input = "use federation f1 (d1 = 20) with filtering=on, reset";
            var options = MakeOptions(IdentifierCasing.Preserve, IdentifierBracketing.IncludeBrackets);
            const string expected = @"USE FEDERATION f1 ([d1] = 20) WITH FILTERING = ON, RESET;";

            AssertGenerated(input, options, expected);
        }

        // GAP-WINDOW: an unparenthesized "OVER window_name" reference must be a plain identifier
        // ("OVER [Win1]" is invalid), while the WINDOW clause definition name may be bracketed.
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestGapWindowOverReferenceNotBracketed()
        {
            const string input = "SELECT Sum(c1) OVER Win1 FROM t1 WINDOW Win1 AS (PARTITION BY c1)";
            var options = MakeOptions(IdentifierCasing.Preserve, IdentifierBracketing.IncludeBrackets);
            const string expected =
@"SELECT Sum([c1]) OVER Win1 FROM [t1]
WINDOW [Win1] AS (PARTITION BY [c1]);";

            AssertGenerated(input, options, expected);
        }

        // GAP-TIMESTAMP: the INSERT BULK timestamp/rowversion shorthand column has no data type and its
        // name is the TIMESTAMP keyword ("([c2] CHAR, [TIMESTAMP])" is invalid); regular columns are
        // still bracketed.
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestGapInsertBulkTimestampColumnNotBracketed()
        {
            const string input = "insert bulk dbo.t1 (c1 int not null, c2 char, timestamp)";
            var options = MakeOptions(IdentifierCasing.Preserve, IdentifierBracketing.IncludeBrackets);
            const string expected = @"INSERT BULK [dbo].[t1] ([c1] INT NOT NULL, [c2] CHAR, TIMESTAMP);";

            AssertGenerated(input, options, expected);
        }

        // GAP-PARTITION-COLLATE: the partition-function parameter collation is a collation name and
        // cannot be delimited; the partition function name is still bracketed.
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestGapPartitionFunctionCollationNotBracketed()
        {
            const string input = "CREATE PARTITION FUNCTION myRangePF1 (char(10) COLLATE Estonian_CS_AS) AS RANGE RIGHT FOR VALUES (1)";
            var options = MakeOptions(IdentifierCasing.Preserve, IdentifierBracketing.IncludeBrackets);
            const string expected =
@"CREATE PARTITION FUNCTION [myRangePF1](CHAR (10) COLLATE Estonian_CS_AS)
    AS RANGE RIGHT
    FOR VALUES (1);";

            AssertGenerated(input, options, expected);
        }

        // GAP-OPENROWSET-COLLATE: an OPENROWSET WITH column collation cannot be delimited; the column
        // name is still bracketed.
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestGapOpenRowsetColumnCollationNotBracketed()
        {
            const string input = "select * from openrowset (bulk 'f1', format = 'CSV') with ([continent] varchar(100) collate latin1_general_bin2 2) as a;";
            var options = MakeOptions(IdentifierCasing.Preserve, IdentifierBracketing.IncludeBrackets);
            const string expected = @"SELECT * FROM OPENROWSET (BULK 'f1', FORMAT = 'CSV') WITH ([continent] VARCHAR (100) COLLATE latin1_general_bin2 2) AS [a];";

            AssertGenerated(input, options, expected);
        }

        // GAP-FILETABLE-COLLATE: the FILETABLE_COLLATE_FILENAME option value is a collation name and
        // cannot be delimited; the table name is still bracketed.
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestGapFileTableCollationValueNotBracketed()
        {
            const string input = "create table t1 as filetable with(filetable_collate_filename=Latin1_General_bin, filetable_directory=null)";
            var options = MakeOptions(IdentifierCasing.Preserve, IdentifierBracketing.IncludeBrackets);
            const string expected =
@"CREATE TABLE [t1] AS FILETABLE
WITH (FILETABLE_COLLATE_FILENAME = Latin1_General_bin, FILETABLE_DIRECTORY = NULL);";

            AssertGenerated(input, options, expected);
        }

        // GAP-STOPLIST: "STOPLIST = SYSTEM" is a keyword option, not a stoplist name. The value is
        // stored as an unquoted Identifier "SYSTEM"; bracketing it ("STOPLIST [SYSTEM]") makes the
        // parser read it as a user-defined stoplist named SYSTEM, silently changing the meaning. The
        // SYSTEM keyword must be preserved unbracketed.
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestGapFulltextStoplistSystemNotBracketed()
        {
            const string input = "ALTER FULLTEXT INDEX ON t SET STOPLIST = SYSTEM;";
            var options = MakeOptions(IdentifierCasing.Preserve, IdentifierBracketing.IncludeBrackets);
            const string expected = @"ALTER FULLTEXT INDEX ON [t] SET STOPLIST SYSTEM;";

            AssertGenerated(input, options, expected);
        }

        // GAP-STOPLIST (user name): a real, user-defined stoplist name IS a normal identifier and is
        // still bracketed under IncludeBrackets.
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestGapFulltextStoplistUserNameBracketed()
        {
            const string input = "ALTER FULLTEXT INDEX ON t SET STOPLIST = MyStopList;";
            var options = MakeOptions(IdentifierCasing.Preserve, IdentifierBracketing.IncludeBrackets);
            const string expected = @"ALTER FULLTEXT INDEX ON [t] SET STOPLIST [MyStopList];";

            AssertGenerated(input, options, expected);
        }

        // GAP-STOPLIST (CREATE): same rule on the CREATE FULLTEXT INDEX path (both paths share the
        // StopListFullTextIndexOption emit site).
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestGapCreateFulltextStoplistSystemNotBracketed()
        {
            const string input = "CREATE FULLTEXT INDEX ON t (c) KEY INDEX i WITH STOPLIST = SYSTEM;";
            var options = MakeOptions(IdentifierCasing.Preserve, IdentifierBracketing.IncludeBrackets);
            const string expected =
@"CREATE FULLTEXT INDEX ON [t]
    ([c])
    KEY INDEX [i]
    WITH STOPLIST SYSTEM;";

            AssertGenerated(input, options, expected);
        }

        // GAP-TVF: built-in / global table-valued function names (STRING_SPLIT, OPENJSON,
        // GENERATE_SERIES, ...) are stored on GlobalFunctionTableReference as Identifier names, but
        // they are function names, not object names. Bracketing them ("[STRING_SPLIT]") makes the
        // parser read them as user-defined functions, so they must stay unbracketed while a table
        // alias is still bracketed.
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestGapGlobalTableFunctionNameNotBracketed()
        {
            const string input = "SELECT * FROM STRING_SPLIT('a,b', ',') AS s;";
            var options = MakeOptions(IdentifierCasing.Preserve, IdentifierBracketing.IncludeBrackets);
            const string expected = @"SELECT * FROM STRING_SPLIT ('a,b', ',') AS [s];";

            AssertGenerated(input, options, expected);
        }

        // GAP-TVF (:: built-in): the "::function(...)" built-in table-function name
        // (BuiltInFunctionTableReference) is likewise a function name, not an object name.
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestGapBuiltInTableFunctionNameNotBracketed()
        {
            const string input = "SELECT * FROM ::fn_helpcollations();";
            var options = MakeOptions(IdentifierCasing.Preserve, IdentifierBracketing.IncludeBrackets);
            const string expected = @"SELECT * FROM ::fn_helpcollations ();";

            AssertGenerated(input, options, expected);
        }

        // GAP-EXCLUDE-KEYWORD: ExcludeBrackets must keep brackets around a value that lexes to a
        // keyword token (the reserved word KEY). Stripping them yields invalid T-SQL, so
        // CanOmitBrackets only removes brackets from genuine Identifier tokens. The non-reserved
        // table name loses its brackets as expected.
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestExcludeBracketsKeepsBracketsAroundReservedKeyword()
        {
            const string input = "SELECT [key] FROM [t1];";
            var options = MakeOptions(IdentifierCasing.Preserve, IdentifierBracketing.ExcludeBrackets);
            const string expected = @"SELECT [key] FROM t1;";

            AssertGenerated(input, options, expected);
        }

        // KNOWN LIMITATION (documented on IdentifierBracketing.ExcludeBrackets and CanOmitBrackets):
        // AI_GENERATE_CHUNKS lexes as an ordinary identifier, so ExcludeBrackets removes the brackets
        // that were forcing a regular table-valued-function parse. That changes semantics and the
        // output does not round-trip, so this test uses AssertGeneratedWith (which does NOT reparse)
        // to pin the accepted behavior. If ExcludeBrackets ever becomes context aware, update the
        // expected value (it should then keep the brackets and reparse cleanly).
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestExcludeBracketsSpecialOperatorNameIsKnownLimitation()
        {
            const string input = "SELECT SOURCE FROM userTable CROSS APPLY dbo.[AI_GENERATE_CHUNKS](SOURCE);";
            var options = MakeOptions(IdentifierCasing.Preserve, IdentifierBracketing.ExcludeBrackets);
            const string expected = @"SELECT SOURCE FROM userTable CROSS APPLY dbo.AI_GENERATE_CHUNKS(SOURCE);";

            AssertGeneratedWith(new TSql170Parser(true), new Sql170ScriptGenerator(options), input, expected);
        }
    }
}
