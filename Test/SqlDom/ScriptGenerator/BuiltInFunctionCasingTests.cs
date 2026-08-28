//------------------------------------------------------------------------------
// <copyright file="BuiltInFunctionCasingTests.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using Microsoft.SqlServer.TransactSql.ScriptDom;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlStudio.Tests.AssemblyTools.TestCategory;
using static SqlStudio.Tests.UTSqlScriptDom.ScriptGeneratorTestHelper;

namespace SqlStudio.Tests.UTSqlScriptDom
{
    // Tests for the BuiltInFunctionCasing script-generation option.
    //
    // Expected values follow the IdentifierFormattingTests pattern: verbatim @"..." literals compared
    // via the shared AssertGenerated helper (which normalizes line endings and trims). MakeOptions
    // sets the casing under test and disables unrelated multi-line/alignment formatting.
    //
    // Work item: Formatter option: Built-in function name casing
    [TestClass]
    public class BuiltInFunctionCasingTests
    {
        // Builds options that set the BuiltInFunctionCasing (and KeywordCasing) under test while
        // disabling unrelated multi-line/alignment formatting, so the expected literals stay focused
        // on the function-name transformation itself.
        private static SqlScriptGeneratorOptions MakeOptions(
            BuiltInFunctionCasing casing,
            KeywordCasing keywordCasing = KeywordCasing.Uppercase,
            bool preserveComments = false)
        {
            return new SqlScriptGeneratorOptions
            {
                BuiltInFunctionCasing = casing,
                KeywordCasing = keywordCasing,
                PreserveComments = preserveComments,
                AlignColumnDefinitionFields = false,
                AlignClauseBodies = false,
                NewLineBeforeFromClause = false,
                NewLineBeforeJoinClause = false,
                MultilineSelectElementsList = false,
            };
        }

        // -----------------------------------------------------------------------------------------
        // Default / backward compatibility
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestBuiltInFunctionCasingDefaultIsPreserve()
        {
            Assert.AreEqual(BuiltInFunctionCasing.Preserve, new SqlScriptGeneratorOptions().BuiltInFunctionCasing);
        }

        // Preserve must reproduce the pre-feature output exactly: generic function names keep their
        // original casing, while keyword-backed built-ins (CAST, CONVERT, COALESCE) continue to
        // follow KeywordCasing as they always have. This is the backward-compatibility sentinel.
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestPreserveHasZeroImpact()
        {
            const string input = "SELECT cast(x AS INT), Convert(INT, y), Coalesce(a, b), getdate(), IsNull(a, b);";
            var options = MakeOptions(BuiltInFunctionCasing.Preserve);
            const string expected = @"SELECT CAST (x AS INT), CONVERT (INT, y), COALESCE (a, b), getdate(), IsNull(a, b);";

            AssertGenerated(input, options, expected);
        }

        // -----------------------------------------------------------------------------------------
        // Generic FunctionCall names (GETDATE, ISNULL, COUNT, OBJECT_ID, ...)
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestGenericFunctionUppercase()
        {
            const string input = "SELECT getdate(), isnull(a, b), object_id('t');";
            var options = MakeOptions(BuiltInFunctionCasing.Uppercase);
            const string expected = @"SELECT GETDATE(), ISNULL(a, b), OBJECT_ID('t');";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestGenericFunctionLowercase()
        {
            const string input = "SELECT GETDATE(), ISNULL(A, B), OBJECT_ID('t');";
            var options = MakeOptions(BuiltInFunctionCasing.Lowercase);
            const string expected = @"SELECT getdate(), isnull(A, B), object_id('t');";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestGenericFunctionPascalCase()
        {
            // PascalCase capitalizes the first letter and lowercases the rest (matching the shared
            // PascalCase helper used by KeywordCasing/IdentifierCasing); underscores are preserved.
            const string input = "SELECT GETDATE(), object_id('t');";
            var options = MakeOptions(BuiltInFunctionCasing.PascalCase);
            const string expected = @"SELECT Getdate(), Object_id('t');";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMixedCaseInputIsNormalized()
        {
            const string input = "SELECT GetDate(), IsNull(a, b);";
            var options = MakeOptions(BuiltInFunctionCasing.Uppercase);
            const string expected = @"SELECT GETDATE(), ISNULL(a, b);";

            AssertGenerated(input, options, expected);
        }

        // -----------------------------------------------------------------------------------------
        // Aggregate functions
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestAggregateFunctions()
        {
            const string input = "SELECT count(*), sum(a), max(b), min(c), avg(d) FROM t;";
            var options = MakeOptions(BuiltInFunctionCasing.Uppercase);
            const string expected = @"SELECT COUNT(*), SUM(a), MAX(b), MIN(c), AVG(d) FROM t;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestWindowedAggregate()
        {
            const string input = "SELECT count(*) OVER (PARTITION BY a) FROM t;";
            var options = MakeOptions(BuiltInFunctionCasing.Lowercase);
            const string expected = @"SELECT count(*) OVER (PARTITION BY a) FROM t;";

            AssertGenerated(input, options, expected);
        }

        // -----------------------------------------------------------------------------------------
        // Dedicated built-in nodes (CAST, CONVERT, COALESCE, NULLIF, IIF, LEFT, RIGHT, TRY_*)
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCastLowercase()
        {
            const string input = "SELECT CAST(x AS INT);";
            var options = MakeOptions(BuiltInFunctionCasing.Lowercase);
            const string expected = @"SELECT cast (x AS INT);";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestConvertLowercase()
        {
            const string input = "SELECT CONVERT(INT, y, 1);";
            var options = MakeOptions(BuiltInFunctionCasing.Lowercase);
            const string expected = @"SELECT convert (INT, y, 1);";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCoalesceAndNullIfLowercase()
        {
            const string input = "SELECT COALESCE(a, b), NULLIF(a, b);";
            var options = MakeOptions(BuiltInFunctionCasing.Lowercase);
            const string expected = @"SELECT coalesce (a, b), nullif (a, b);";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIIfLowercase()
        {
            const string input = "SELECT IIF(a > b, 1, 0);";
            var options = MakeOptions(BuiltInFunctionCasing.Lowercase);
            const string expected = @"SELECT iif (a > b, 1, 0);";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestLeftAndRightLowercase()
        {
            const string input = "SELECT LEFT(s, 1), RIGHT(s, 1);";
            var options = MakeOptions(BuiltInFunctionCasing.Lowercase);
            const string expected = @"SELECT left(s, 1), right(s, 1);";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestTryCastAndTryConvertLowercase()
        {
            const string input = "SELECT TRY_CAST(x AS INT), TRY_CONVERT(INT, y);";
            var options = MakeOptions(BuiltInFunctionCasing.Lowercase);
            const string expected = @"SELECT try_cast (x AS INT), try_convert (INT, y);";

            AssertGenerated(input, options, expected);
        }

        // PascalCase on the dedicated (keyword/literal-backed) built-in nodes: the shared PascalCase
        // helper lowercases the whole name then capitalizes the first letter, so CAST -> Cast and
        // TRY_CONVERT -> Try_convert. LEFT/RIGHT emit without a space before '(' while the other
        // dedicated nodes keep the pre-existing space.
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestDedicatedNodesPascalCase()
        {
            const string input = "SELECT CAST(x AS INT), CONVERT(INT, y), COALESCE(a, b), NULLIF(a, b), IIF(a > b, 1, 0), LEFT(s, 1), RIGHT(s, 1), TRY_CAST(x AS INT), TRY_CONVERT(INT, y);";
            var options = MakeOptions(BuiltInFunctionCasing.PascalCase);
            const string expected = @"SELECT Cast (x AS INT), Convert (INT, y), Coalesce (a, b), Nullif (a, b), Iif (a > b, 1, 0), Left(s, 1), Right(s, 1), Try_cast (x AS INT), Try_convert (INT, y);";

            AssertGenerated(input, options, expected);
        }

        // -----------------------------------------------------------------------------------------
        // Interaction rule: takes precedence over KeywordCasing for function names, so the two can be
        // set to different casings.
        // -----------------------------------------------------------------------------------------

        // KeywordCasing lowercases keywords, but built-in function names follow their own option.
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIndependentOfKeywordCasing_KeywordsLowerFunctionsUpper()
        {
            const string input = "select getdate(), coalesce(a, b) from t;";
            var options = MakeOptions(BuiltInFunctionCasing.Uppercase, KeywordCasing.Lowercase);
            const string expected = @"select GETDATE(), COALESCE (a, b) from t;";

            AssertGenerated(input, options, expected);
        }

        // KeywordCasing uppercases keywords, but built-in function names follow their own option.
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIndependentOfKeywordCasing_KeywordsUpperFunctionsLower()
        {
            const string input = "select GETDATE(), COALESCE(a, b) from t;";
            var options = MakeOptions(BuiltInFunctionCasing.Lowercase, KeywordCasing.Uppercase);
            const string expected = @"SELECT getdate(), coalesce (a, b) FROM t;";

            AssertGenerated(input, options, expected);
        }

        // -----------------------------------------------------------------------------------------
        // Interaction rule: AS inside CAST follows KeywordCasing, not BuiltInFunctionCasing
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCastAsKeywordFollowsKeywordCasing()
        {
            // Function name CAST -> uppercase (BuiltInFunctionCasing); AS -> lowercase (KeywordCasing).
            const string input = "SELECT CAST(x AS INT);";
            var options = MakeOptions(BuiltInFunctionCasing.Uppercase, KeywordCasing.Lowercase);
            const string expected = @"select CAST (x as int);";

            AssertGenerated(input, options, expected);
        }

        // -----------------------------------------------------------------------------------------
        // Interaction rule: data type names are not affected
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestDataTypeNamesNotAffected()
        {
            // BuiltInFunctionCasing is Lowercase, but the INT / VARCHAR data type names follow
            // KeywordCasing (Uppercase here), not the function-casing option.
            const string input = "SELECT CAST(x AS INT), CONVERT(VARCHAR(10), y);";
            var options = MakeOptions(BuiltInFunctionCasing.Lowercase, KeywordCasing.Uppercase);
            const string expected = @"SELECT cast (x AS INT), convert (VARCHAR (10), y);";

            AssertGenerated(input, options, expected);
        }

        // -----------------------------------------------------------------------------------------
        // Interaction rule: only qualified / delimited names are preserved
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestSchemaQualifiedFunctionNotAffected()
        {
            // Schema-qualified => user-defined, even if the name matches a built-in.
            const string input = "SELECT dbo.GetDate(), dbo.Count(a);";
            var options = MakeOptions(BuiltInFunctionCasing.Uppercase);
            const string expected = @"SELECT dbo.GetDate(), dbo.Count(a);";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestUnqualifiedUnknownFunctionIsRecased()
        {
            // Unrecognized one-part names are re-cased too. That is safe rather than merely
            // unavoidable: SQL Server rejects a one-part scalar function call ("'MyCustomFunc' is not
            // a recognized built-in function name"), so no executable script reaches this path with a
            // UDF name. TestSchemaQualifiedFunctionNotAffected covers the callable two-part form.
            const string input = "SELECT MyCustomFunc(a), fn_MyHelper(b);";
            var options = MakeOptions(BuiltInFunctionCasing.Uppercase);
            const string expected = @"SELECT MYCUSTOMFUNC(a), FN_MYHELPER(b);";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestDelimitedFunctionNameNotAffected()
        {
            // A delimited (bracketed) name is treated as an identifier / UDF and left untouched.
            const string input = "SELECT [GETDATE]();";
            var options = MakeOptions(BuiltInFunctionCasing.Lowercase);
            const string expected = @"SELECT [GETDATE]();";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMethodCallOnVariableOrColumnNotAffected()
        {
            // CLR/spatial/XML method invocations parse as FunctionCall with a non-null CallTarget,
            // so they take the same qualified-call exit as dbo.MyFunc() and keep their casing.
            const string input = "SELECT @g.STArea(), x.value('(/a)[1]', 'int');";
            var options = MakeOptions(BuiltInFunctionCasing.Uppercase);
            const string expected = @"SELECT @g.STArea(), x.value('(/a)[1]', 'int');";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestTableValuedFunctionNameNotAffected()
        {
            // A TVF in a FROM clause is a SchemaObjectFunctionTableReference, not a FunctionCall, so
            // it never reaches the re-casing path - including the one-part form, which (unlike a
            // scalar UDF) T-SQL does resolve against the default schema.
            const string input = "SELECT * FROM MyTvf(1) AS t CROSS APPLY dbo.OtherTvf(t.a) AS u;";
            var options = MakeOptions(BuiltInFunctionCasing.Uppercase);
            const string expected = @"SELECT * FROM MyTvf(1) AS t CROSS APPLY dbo.OtherTvf(t.a) AS u;";

            AssertGenerated(input, options, expected);
        }

        // -----------------------------------------------------------------------------------------
        // Nested and mixed usage
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestNestedBuiltInFunctions()
        {
            const string input = "SELECT isnull(convert(varchar(20), getdate()), 'n/a');";
            var options = MakeOptions(BuiltInFunctionCasing.Uppercase);
            const string expected = @"SELECT ISNULL(CONVERT (VARCHAR (20), GETDATE()), 'n/a');";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestBuiltInMixedWithUdf()
        {
            // Built-ins re-cased, the qualified UDF left alone, in a single expression.
            const string input = "SELECT getdate(), dbo.MyFunc(datediff(day, a, b));";
            var options = MakeOptions(BuiltInFunctionCasing.Uppercase);
            const string expected = @"SELECT GETDATE(), dbo.MyFunc(DATEDIFF(day, a, b));";

            AssertGenerated(input, options, expected);
        }

        // -----------------------------------------------------------------------------------------
        // System / error functions
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestSystemFunctions()
        {
            const string input = "SELECT error_number(), error_message(), newid();";
            var options = MakeOptions(BuiltInFunctionCasing.Uppercase);
            const string expected = @"SELECT ERROR_NUMBER(), ERROR_MESSAGE(), NEWID();";

            AssertGenerated(input, options, expected);
        }

        // -----------------------------------------------------------------------------------------
        // Dedicated conversion nodes: PARSE / TRY_PARSE (mirror the CAST / CONVERT family)
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestParseAndTryParseLowercase()
        {
            // PARSE / TRY_PARSE are keyword-backed conversion built-ins like CAST/CONVERT: the name
            // follows BuiltInFunctionCasing (lower) while AS and the data type follow KeywordCasing.
            const string input = "SELECT PARSE('42' AS INT), TRY_PARSE('42' AS INT);";
            var options = MakeOptions(BuiltInFunctionCasing.Lowercase);
            const string expected = @"SELECT parse ('42' AS INT), try_parse ('42' AS INT);";

            AssertGenerated(input, options, expected);
        }

        // -----------------------------------------------------------------------------------------
        // Built-in AI_* scalar functions (SQL Server 2025 / 170) follow BuiltInFunctionCasing
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestAiFunctionNameUppercase()
        {
            // The AI_* function name is a built-in and is re-cased; USE / MODEL and the model name
            // keep their own rules (KeywordCasing / IdentifierCasing).
            const string input = "SELECT ai_generate_embeddings('t' USE MODEL MyModel);";
            var options = MakeOptions(BuiltInFunctionCasing.Uppercase);
            const string expected = @"SELECT AI_GENERATE_EMBEDDINGS('t' USE MODEL MyModel);";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestAiFunctionNameLowercase()
        {
            const string input = "SELECT AI_GENERATE_EMBEDDINGS('t' USE MODEL MyModel);";
            var options = MakeOptions(BuiltInFunctionCasing.Lowercase);
            const string expected = @"SELECT ai_generate_embeddings('t' USE MODEL MyModel);";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestInvokeExternalApiFunctionNameLowercase()
        {
            const string input = "SELECT INVOKE_EXTERNAL_API('MySet', 'MyFunc', 1);";
            var options = MakeOptions(BuiltInFunctionCasing.Lowercase);
            const string expected = @"SELECT invoke_external_api('MySet', 'MyFunc', 1);";

            AssertGenerated(input, options, expected);
        }

        // -----------------------------------------------------------------------------------------
        // Out-of-scope constructs: niladic system functions and built-in table-valued functions
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestParameterlessSystemFunctionsNotAffected()
        {
            // Keyword-form niladic functions are emitted as keywords and are NOT re-cased by
            // BuiltInFunctionCasing; they continue to follow KeywordCasing (Uppercase here).
            const string input = "SELECT current_timestamp, system_user, current_user;";
            var options = MakeOptions(BuiltInFunctionCasing.Lowercase, KeywordCasing.Uppercase);
            const string expected = @"SELECT CURRENT_TIMESTAMP, SYSTEM_USER, CURRENT_USER;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestBuiltInTableValuedFunctionNotAffected()
        {
            // Built-in table-valued functions flow through table-reference nodes, not FunctionCall,
            // so their names are governed by IdentifierCasing and are NOT re-cased.
            const string input = "SELECT * FROM STRING_SPLIT('a,b,c', ',');";
            var options = MakeOptions(BuiltInFunctionCasing.Lowercase);
            const string expected = @"SELECT * FROM STRING_SPLIT ('a,b,c', ',');";

            AssertGenerated(input, options, expected);
        }

        // -----------------------------------------------------------------------------------------
        // Out-of-scope constructs: keyword-operator forms. Each is its own AST node rather than a
        // FunctionCall, so none of them reaches the re-casing helper. These pin that boundary.
        // -----------------------------------------------------------------------------------------

        // Asserts the option leaves a construct untouched by comparing output with it off and on,
        // so the assertion does not depend on unrelated layout options. Names in sql must be
        // uppercase or mixed case, otherwise lowercasing would be invisible.
        private static void AssertNotAffectedByCasing(string sql)
        {
            Assert.AreEqual(
                Generate(sql, MakeOptions(BuiltInFunctionCasing.Preserve)),
                Generate(sql, MakeOptions(BuiltInFunctionCasing.Lowercase)));
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIdentityFunctionNotAffected()
        {
            AssertNotAffectedByCasing("SELECT IDENTITY(INT, 1, 1) AS id INTO t2 FROM t;");
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestAtTimeZoneNotAffected()
        {
            AssertNotAffectedByCasing("SELECT d AT TIME ZONE 'UTC' FROM t;");
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestNextValueForNotAffected()
        {
            AssertNotAffectedByCasing("SELECT NEXT VALUE FOR dbo.MySeq;");
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestPartitionFunctionNotAffected()
        {
            AssertNotAffectedByCasing("SELECT $PARTITION.MyRangeFn(1);");
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestOdbcFunctionCallNotAffected()
        {
            AssertNotAffectedByCasing("SELECT {fn NOW()};");
        }

        // -----------------------------------------------------------------------------------------
        // Remaining guard branches on the generic FunctionCall path
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestDoubleQuotedFunctionNameNotAffected()
        {
            // The guard is QuoteType != NotQuoted, so double-quoted names are preserved just like
            // bracketed ones.
            const string input = "SELECT \"GETDATE\"();";
            var options = MakeOptions(BuiltInFunctionCasing.Lowercase);
            const string expected = "SELECT \"GETDATE\"();";

            AssertGenerated(input, options, expected);
        }

        // -----------------------------------------------------------------------------------------
        // Generic FunctionCall nodes that branch on the function name to emit a custom body.
        // Re-casing the emitted name must not disturb that body generation, because the branches
        // dispatch on the unmodified AST value (node.FunctionName.Value).
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestJsonConstructorFunctionsAreRecased()
        {
            const string input = "SELECT JSON_OBJECT('a':'1'), JSON_ARRAY(1, 2);";
            var options = MakeOptions(BuiltInFunctionCasing.Lowercase);
            const string expected = @"SELECT json_object('a':'1'), json_array(1, 2);";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestTrimWithOptionsIsRecased()
        {
            // The two-argument TRIM form keeps its LEADING/TRAILING/BOTH keyword and FROM body.
            const string input = "SELECT TRIM(LEADING ' ' FROM s);";
            var options = MakeOptions(BuiltInFunctionCasing.Lowercase);
            const string expected = @"SELECT trim( LEADING ' ' FROM s);";

            AssertGenerated(input, options, expected);
        }

        // -----------------------------------------------------------------------------------------
        // Interaction with PreserveComments. Re-casing emits the function name as a raw token rather
        // than as a fragment, so the comment hooks have to be driven explicitly; without them a
        // comment trailing the name is absorbed into the argument list or dropped entirely.
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCommentAfterFunctionNameStaysWithName()
        {
            const string input = "SELECT LEN /*c*/ (x);";
            var options = MakeOptions(BuiltInFunctionCasing.Lowercase, preserveComments: true);
            const string expected = @"SELECT len /*c*/(x);";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCommentAfterParameterlessFunctionNameIsPreserved()
        {
            // A zero-argument call has no following fragment inside the parentheses, so the comment
            // has nowhere to migrate to and would be lost outright without the trailing-comment hook.
            const string input = "SELECT GETDATE /*c*/ ();";
            var options = MakeOptions(BuiltInFunctionCasing.Lowercase, preserveComments: true);
            const string expected = @"SELECT getdate /*c*/();";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestCommentBeforeFunctionNameIsPreserved()
        {
            // Exercises the leading hook; the trailing-comment tests above only cover the other side.
            const string input = "SELECT /*c*/ LEN(x);";
            var options = MakeOptions(BuiltInFunctionCasing.Lowercase, preserveComments: true);
            const string expected = @"SELECT /*c*/
len(x);";

            AssertGenerated(input, options, expected);
        }

        // -----------------------------------------------------------------------------------------
        // Fabric DW dialect. The helper lives on the shared SqlScriptGeneratorVisitor base, so every
        // derived generator inherits it; these also cover the seven AI_* functions that the SQL-170
        // grammar does not accept.
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestFabricDwGenericFunctionLowercase()
        {
            const string input = "SELECT GETDATE(), ISNULL(a, b);";
            var options = MakeOptions(BuiltInFunctionCasing.Lowercase);
            const string expected = @"SELECT getdate(), isnull(a, b);";

            AssertGeneratedFabric(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestFabricDwAiFunctionNamesLowercase()
        {
            const string input =
                "SELECT AI_ANALYZE_SENTIMENT('text'), AI_CLASSIFY('text', 'spam', 'ham'), " +
                "AI_EXTRACT('text', 'spam', 'ham'), AI_FIX_GRAMMAR('text');";
            var options = MakeOptions(BuiltInFunctionCasing.Lowercase);
            const string expected =
                @"SELECT ai_analyze_sentiment('text'), ai_classify('text', 'spam', 'ham'), ai_extract('text', 'spam', 'ham'), ai_fix_grammar('text');";

            AssertGeneratedFabric(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestFabricDwAiFunctionNamesUppercase()
        {
            const string input =
                "SELECT ai_generate_response('Hello'), ai_summarize('text'), ai_translate('text', 'es');";
            var options = MakeOptions(BuiltInFunctionCasing.Uppercase);
            const string expected =
                @"SELECT AI_GENERATE_RESPONSE('Hello'), AI_SUMMARIZE('text'), AI_TRANSLATE('text', 'es');";

            AssertGeneratedFabric(input, options, expected);
        }

        // -----------------------------------------------------------------------------------------
        // Dedicated built-in forms outside the scalar-expression path: the TSEQUAL predicate and the
        // aggregate name in a COMPUTE clause. Both name a built-in function, so both follow the
        // option rather than KeywordCasing or a fixed-case literal.
        // -----------------------------------------------------------------------------------------

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestTSEqualLowercase()
        {
            const string input = "SELECT * FROM t WHERE TSEQUAL(c1, c2);";
            var options = MakeOptions(BuiltInFunctionCasing.Lowercase, KeywordCasing.Uppercase);
            const string expected = @"SELECT * FROM t
WHERE tsequal (c1, c2);";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestTSEqualPreserveIsUnchanged()
        {
            const string input = "SELECT * FROM t WHERE tsequal(c1, c2);";
            var options = MakeOptions(BuiltInFunctionCasing.Preserve, KeywordCasing.Uppercase);
            const string expected = @"SELECT * FROM t
WHERE TSEQUAL (c1, c2);";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestComputeClauseAggregateLowercase()
        {
            const string input = "SELECT a FROM t COMPUTE SUM(a);";
            var options = MakeOptions(BuiltInFunctionCasing.Lowercase, KeywordCasing.Uppercase);
            const string expected = @"SELECT a FROM t
COMPUTE sum(a);";

            AssertGenerated100(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestComputeClauseAggregatePreserveIsUnchanged()
        {
            // Preserve must keep the historical fixed-case literal, not the input's casing.
            const string input = "SELECT a FROM t COMPUTE sum(a);";
            var options = MakeOptions(BuiltInFunctionCasing.Preserve, KeywordCasing.Uppercase);
            const string expected = @"SELECT a FROM t
COMPUTE SUM(a);";

            AssertGenerated100(input, options, expected);
        }
    }
}
