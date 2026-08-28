//------------------------------------------------------------------------------
// <copyright file="FunctionCallFormattingTests.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using Microsoft.SqlServer.TransactSql.ScriptDom;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlStudio.Tests.AssemblyTools.TestCategory;
using static SqlStudio.Tests.UTSqlScriptDom.ScriptGeneratorTestHelper;

namespace SqlStudio.Tests.UTSqlScriptDom
{
    [TestClass]
    public class FunctionCallFormattingTests
    {
        private static SqlScriptGeneratorOptions MakeOptions()
        {
            return new SqlScriptGeneratorOptions
            {
                AlignClauseBodies = false,
                MultilineSelectElementsList = false,
                PreserveComments = true,
            };
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestFunctionCallFormattingDefaultsPreserveExistingOutput()
        {
            var defaults = new SqlScriptGeneratorOptions();
            Assert.IsFalse(defaults.MultilineNestedFunctionCalls);

            const string input = "SELECT REPLACE(value, 'x', 'y');";
            const string expected = "SELECT REPLACE(value, 'x', 'y');";

            AssertGenerated(input, MakeOptions(), expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestNestedFunctionCallsRemainCompactByDefault()
        {
            const string input = "SELECT REPLACE(LOWER(value), 'x', 'y');";
            const string expected = "SELECT REPLACE(LOWER(value), 'x', 'y');";

            AssertGenerated(input, MakeOptions(), expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineNestedFunctionCallsPreservesParameterComments()
        {
            const string input =
@"
SELECT TRIM(
REPLACE(
TRANSLATE(
city,
'0123456789', -- source characters
'~~~~~~~~~~' -- replacement characters
),
'~', ''
)
);";
            var options = MakeOptions();
            options.ClauseBodyAlignment = ClauseBodyAlignment.Indented;
            options.MultilineNestedFunctionCalls = true;

            const string expected =
@"
SELECT
    TRIM (
        REPLACE (
            TRANSLATE (
                city,
                '0123456789', -- source characters
                '~~~~~~~~~~' -- replacement characters
            ),
            '~',
            ''
        )
    );";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineNestedFunctionCallsAlignsLeadingParameterComment()
        {
            const string input =
@"
SELECT REPLACE(
-- normalize value
LOWER(value),
'x',
'y'
);";
            var options = MakeOptions();
            options.ClauseBodyAlignment = ClauseBodyAlignment.Indented;
            options.MultilineNestedFunctionCalls = true;

            const string expected =
@"
SELECT
    REPLACE (
        -- normalize value
        LOWER (value),
        'x',
        'y'
    );";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineNestedFunctionCallsWithAlignedClauseBodies()
        {
            const string input = "SELECT TRIM(REPLACE(city, 'x', 'y')) AS result FROM addresses WHERE city IS NOT NULL;";
            var options = new SqlScriptGeneratorOptions
            {
                ClauseBodyAlignment = ClauseBodyAlignment.Aligned,
                MultilineNestedFunctionCalls = true,
            };

            const string expected =
@"
SELECT TRIM (
           REPLACE (
               city,
               'x',
               'y'
           )
       ) AS result
FROM   addresses
WHERE  city IS NOT NULL;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineNestedFunctionCallsWithIndentedClauseBodies()
        {
            const string input = "SELECT TRIM(REPLACE(city, 'x', 'y')) AS result FROM addresses WHERE city IS NOT NULL;";
            var options = new SqlScriptGeneratorOptions
            {
                ClauseBodyAlignment = ClauseBodyAlignment.Indented,
                MultilineNestedFunctionCalls = true,
            };

            const string expected =
@"
SELECT
    TRIM (
        REPLACE (
            city,
            'x',
            'y'
        )
    ) AS result
FROM
    addresses
WHERE
    city IS NOT NULL;";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineNestedFunctionCallsNewLineBeforeOpenParenthesis()
        {
            const string input = "SELECT REPLACE(LOWER(value), 'x', 'y');";
            var options = MakeOptions();
            options.MultilineNestedFunctionCalls = true;
            options.NewLineBeforeOpenParenthesisInMultilineList = true;

            const string expected =
@"
SELECT REPLACE
       (
           LOWER
           (value),
           'x',
           'y'
       );";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineNestedFunctionCallsNoNewLineBeforeCloseParenthesis()
        {
            const string input = "SELECT REPLACE(LOWER(value), 'x', 'y');";
            var options = MakeOptions();
            options.MultilineNestedFunctionCalls = true;
            options.NewLineBeforeCloseParenthesisInMultilineList = false;

            const string expected =
@"
SELECT REPLACE (
           LOWER (value),
           'x',
           'y');";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestIsolatedFunctionCallRemainsCompact()
        {
            const string input = "SELECT REPLACE(value, 'x', 'y');";
            var options = MakeOptions();
            options.MultilineNestedFunctionCalls = true;
            const string expected = "SELECT REPLACE(value, 'x', 'y');";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestFunctionCallNestedInsideParenthesesUsesMultilineLayout()
        {
            const string input = "SELECT REPLACE((LOWER(value)), 'x', 'y');";
            var options = MakeOptions();
            options.MultilineNestedFunctionCalls = true;

            const string expected =
@"
SELECT REPLACE (
           (LOWER (value)),
           'x',
           'y'
       );";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestNestedParameterlessFunctionCallRemainsCompact()
        {
            const string input = "SELECT REPLACE(GETDATE(), 'x', 'y');";
            var options = MakeOptions();
            options.MultilineNestedFunctionCalls = true;

            const string expected =
@"
SELECT REPLACE (
           GETDATE(),
           'x',
           'y'
       );";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestTrailingParameterCommentForcesNewLineWithCompactFunctionCalls()
        {
            const string input =
@"
SELECT TRANSLATE(city,
'a', -- source characters
'b' -- replacement characters
);";
            var options = MakeOptions();

            const string expected =
@"
SELECT TRANSLATE(city, 'a', -- source characters
'b' -- replacement characters
);";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestLeftFunctionCallCommentBeforeCommaForcesNewLine()
        {
            const string input =
@"
SELECT LEFT(value -- input value
, 2);";
            var options = MakeOptions();

            const string expected =
@"
SELECT LEFT(value, -- input value
2);";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestRightFunctionCallCommentBeforeClosingParenthesisForcesNewLine()
        {
            const string input =
@"
SELECT RIGHT(value, 2 -- character count
);";
            var options = MakeOptions();

            const string expected =
@"
SELECT RIGHT(value, 2 -- character count
);";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestSyntaxSpecialFunctionsRemainValidWhenNestedFormattingIsEnabled()
        {
            const string input = "SELECT TRIM(BOTH 'x' FROM value), JSON_OBJECT('a':1);";
            var options = MakeOptions();
            options.MultilineNestedFunctionCalls = true;

            const string expected = "SELECT TRIM( BOTH 'x' FROM value), JSON_OBJECT('a':1);";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestTrimPreservesCommentBeforeFromClause()
        {
            const string input =
@"
SELECT TRIM(BOTH 'x' -- trim character
FROM LOWER(value));";
            var options = MakeOptions();
            options.MultilineNestedFunctionCalls = true;

            const string expected =
@"
SELECT TRIM( BOTH 'x' -- trim character
FROM LOWER(value));";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestLeftAndRightFunctionCallsUseNestedFormattingOnlyWhenNeeded()
        {
            var options = MakeOptions();
            options.MultilineNestedFunctionCalls = true;

            AssertGenerated(
                "SELECT LEFT(value, 2), RIGHT(value, 2);",
                options,
                "SELECT LEFT(value, 2), RIGHT(value, 2);");

            const string leftInput = "SELECT REPLACE(LEFT(LOWER(value), 2), 'x', 'y');";
            const string leftExpected =
@"
SELECT REPLACE (
           LEFT (
               LOWER (value),
               2
           ),
           'x',
           'y'
       );";
            AssertGenerated(leftInput, options, leftExpected);

            const string rightInput = "SELECT REPLACE(RIGHT(UPPER(value), 2), 'x', 'y');";
            const string rightExpected =
@"
SELECT REPLACE (
           RIGHT (
               UPPER (value),
               2
           ),
           'x',
           'y'
       );";
            AssertGenerated(rightInput, options, rightExpected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestJsonSpecialSyntaxRemainsValidWithNestedFunctionArguments()
        {
            const string input =
@"
SELECT
    JSON_OBJECTAGG('name':LOWER(value)),
    JSON_ARRAY(LOWER(value), 2),
    JSON_ARRAYAGG(LOWER(value) ORDER BY value),
    JSON_QUERY(LOWER(value), '$' WITH ARRAY WRAPPER),
    JSON_VALUE(LOWER(value), '$' RETURNING INT);";
            var options = MakeOptions();
            options.MultilineNestedFunctionCalls = true;
            options.MultilineSelectElementsList = true;

            const string expected =
@"
SELECT JSON_OBJECTAGG('name':LOWER(value)),
       JSON_ARRAY(LOWER(value), 2),
       JSON_ARRAYAGG(LOWER(value) ORDER BY value),
       JSON_QUERY(LOWER(value), '$' WITH ARRAY WRAPPER),
       JSON_VALUE(LOWER(value), '$' RETURNING INT);";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestDistinctAndAllFunctionCallsRemainCompact()
        {
            const string input = "SELECT COUNT(DISTINCT LOWER(value)), SUM(ALL ABS(value));";
            var options = MakeOptions();
            options.MultilineNestedFunctionCalls = true;
            const string expected = "SELECT COUNT(DISTINCT LOWER(value)), SUM(ALL ABS(value));";

            AssertGenerated(input, options, expected);
        }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void TestMultilineFunctionCallPreservesNullHandlingAndOverClause()
        {
            const string input = "SELECT FIRST_VALUE(LOWER(Measure)) IGNORE NULLS OVER ();";
            var options = MakeOptions();
            options.MultilineNestedFunctionCalls = true;

            const string expected =
@"
SELECT FIRST_VALUE (
           LOWER (Measure)
       ) IGNORE NULLS OVER ();";

            AssertGenerated(input, options, expected);
        }
    }
}
