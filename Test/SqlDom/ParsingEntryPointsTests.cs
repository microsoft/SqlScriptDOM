//------------------------------------------------------------------------------
// <copyright file="ParsingEntryPointsTests.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlStudio.Tests.AssemblyTools.TestCategory;

namespace SqlStudio.Tests.UTSqlScriptDom
{
    /// <summary>
    /// This class is used in testing entry points which do some parsing
    /// </summary>
    public partial class SqlDomTests
    {
        private static readonly string[] ChildObjectNameTestScripts = { "t1", "[t1]", "[t1.i1]", "t1.i1", "..[t1].i1", "[db].dbo.t1.i1", "db..t1.[i1]" };
        private static readonly string[] SchemaObjectNameTestScripts = { "t1", "[t1]", "[t1.i1]", "t1.i1", "..[t1].i1", "[db].dbo.t1.i1", "db..t1.[i1]" };
        private static readonly string[] ScalarDataTypeTestScripts = { "NVARCHAR (50)", "INT", "MONEY" };
        private static readonly string[] ExpressionTestScripts = { "2 + 2", "someFunction(10)", "'zzz'" };
        private static readonly string[] BooleanExpressionTestScripts = { "1 < 3", "[birthdate] IN (2, 3)", "C1 IS NOT NULL" };
        // A bit strange formatting due to whitespace stripping for comparisons...
        private static readonly string[] StatementListTestScripts = { "CREATE TABLE t1 (      c1 INT  );  CREATE TABLE t2 (      c1 INT  );",
                "BEGIN TRANSACTION ttt1;  CREATE TABLE t1 (      c1 INT  );  COMMIT TRANSACTION;"};
        private static readonly string[] SubqueryExpresion80TestScripts = { "SELECT *  FROM schema1.table2" };
        private static readonly string[] SubqueryExpresionWithCTEListTestScripts = { "SELECT *  FROM schema1.table2", 
                "WITH XMLNAMESPACES ('u' AS n1)  SELECT c1  FROM t1" };

        private static readonly string[] IPv4TestScripts = { "10.20.30.40", "192.168.0.1" };
        private static readonly string[] ConstantOrIdentifierTestScripts = { "-10", "SomeIdentifier", "'qqq'", "[a]]b]" };
        private static readonly string[] ConstantOrIdentifierWithDefaultTestScripts = { "-25", "SomeIdentifier", "'abc'", "DEFAULT", "[a]]b]" };

        private string GenerateSource(SqlScriptGenerator scriptGen, TSqlFragment fragment)
        {
            Assert.IsNotNull(fragment);
            StringWriter writer = new StringWriter();
            scriptGen.GenerateScript(fragment, writer);

            StringBuilder result = new StringBuilder();
            foreach (Char c in writer.ToString())
            {
                if (c == '\n' || c == '\r' || c == '\t')
                    result.Append(' ');
                else
                    result.Append(c);
            }

            return result.ToString();
        }

        delegate T EntryPoint<T>(TextReader input, out IList<ParseError> errors, int startOffset, int startLine, int startColumn)
            where T : TSqlFragment;

        void TestEntryPoint<T>(EntryPoint<T> entryPoint, SqlScriptGenerator scriptGen, string[] scripts)
            where T : TSqlFragment
        {
            foreach (string script in scripts)
            {
                IList<ParseError> errors;
                StringReader reader = new StringReader(script);
                T actual = entryPoint(reader, out errors, 0, 1, 1);
                reader.Close();
                Assert.IsNotNull(actual);
                Assert.AreEqual<int>(0, errors.Count);
                Assert.AreEqual<string>(script, GenerateSource(scriptGen, actual));
            }
        }

        public void ParsingCommonEntryPointsTestImpl(TSqlParser parser, SqlScriptGenerator scriptGen)
        {
            TestEntryPoint<ChildObjectName>(parser.ParseChildObjectName, scriptGen, ChildObjectNameTestScripts);
            TestEntryPoint<SchemaObjectName>(parser.ParseSchemaObjectName, scriptGen, SchemaObjectNameTestScripts);
            TestEntryPoint<DataTypeReference>(parser.ParseScalarDataType, scriptGen, ScalarDataTypeTestScripts);
            TestEntryPoint<ScalarExpression>(parser.ParseExpression, scriptGen, ExpressionTestScripts);
            TestEntryPoint<BooleanExpression>(parser.ParseBooleanExpression, scriptGen, BooleanExpressionTestScripts);
            TestEntryPoint<StatementList>(parser.ParseStatementList, scriptGen, StatementListTestScripts);
            TestEntryPoint<TSqlFragment>(parser.ParseConstantOrIdentifier, scriptGen, ConstantOrIdentifierTestScripts);
            TestEntryPoint<TSqlFragment>(parser.ParseConstantOrIdentifierWithDefault, scriptGen, ConstantOrIdentifierWithDefaultTestScripts);
        }

 
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void ParsingCommonEntryPointsTest()
        {
            ParsingCommonEntryPointsTestImpl(new TSql80Parser(true), ParserTestUtils.CreateScriptGen(SqlVersion.Sql80));
            ParsingCommonEntryPointsTestImpl(new TSql90Parser(true), ParserTestUtils.CreateScriptGen(SqlVersion.Sql90));
            ParsingCommonEntryPointsTestImpl(new TSql100Parser(true), ParserTestUtils.CreateScriptGen(SqlVersion.Sql100));
        }

 
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void ParsingSubqueryExpressionEntryPointTest()
        {
            TSql80Parser parser80 = new TSql80Parser(true);
            TestEntryPoint<SelectStatement>(parser80.ParseSubQueryExpressionWithOptionalCTE, 
                ParserTestUtils.CreateScriptGen(SqlVersion.Sql80), SubqueryExpresion80TestScripts);

            TSql90Parser parser90 = new TSql90Parser(true);
            TestEntryPoint<SelectStatement>(parser90.ParseSubQueryExpressionWithOptionalCTE,
                ParserTestUtils.CreateScriptGen(SqlVersion.Sql90), SubqueryExpresionWithCTEListTestScripts);

            TSql100Parser parser100 = new TSql100Parser(true);
            TestEntryPoint<SelectStatement>(parser100.ParseSubQueryExpressionWithOptionalCTE,
                ParserTestUtils.CreateScriptGen(SqlVersion.Sql100), SubqueryExpresionWithCTEListTestScripts);
        }

 
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void ParsingIpV4EntryPointTest()
        {
            TSql90Parser parser90 = new TSql90Parser(true);
            TestEntryPoint<IPv4>(parser90.ParseIPv4,
                ParserTestUtils.CreateScriptGen(SqlVersion.Sql90), IPv4TestScripts);

            TSql100Parser parser100 = new TSql100Parser(true);
            TestEntryPoint<IPv4>(parser100.ParseIPv4,
                ParserTestUtils.CreateScriptGen(SqlVersion.Sql100), IPv4TestScripts);
        }
    }
}
