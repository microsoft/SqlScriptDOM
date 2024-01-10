using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlStudio.Tests.AssemblyTools.TestCategory;

namespace SqlStudio.Tests.UTSqlScriptDom
{
	using System;
	using System.Data.Common;
	using System.IO;

	public partial class SqlDomTests
	{
		/// <summary>
		/// Regression test for VSTS 1627447: When IndexType is not set the ScripGenerator returns a NULL exception
		/// </summary>
 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]
		public void IndexTypeNullTest()
		{
			IndexDefinition indexDef = new IndexDefinition()
			{
				Name = new Identifier()
				{
					Value = "index",
					QuoteType = QuoteType.SquareBracket
				},
			};

			Assert.AreEqual("INDEX [index]", GenerateScript(indexDef));
		}

		/// <summary>
		/// Regression test for VSTS 1627447: When IdentifierAtomicBlockOption's Value is not set the ScripGenerator returns a NULL exception 
		/// </summary>
 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]
		public void IdentifierAtomicBlockOptionNoValueTest()
		{
			IdentifierAtomicBlockOption fragment = new IdentifierAtomicBlockOption();
			Assert.AreEqual(String.Empty, GenerateScript(fragment));
		}

		/// <summary>
		/// Regression test for VSTS 1632678: INDEX keyword is missing from the IndexDefinition token properties (StartColumn and FragmentLength)
		/// </summary>
 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]
		public void IndexTokenInfoTest()
		{
			string script = @"CREATE TABLE [dbo].[ShoppingCart4] (
    [ShoppingCartId] INT           NOT NULL,
    [UserId]         INT           NOT NULL,
    [CreatedDate]    DATETIME2 (7) NOT NULL,
    [TotalPrice]     MONEY         NULL,
    PRIMARY KEY NONCLUSTERED HASH ([ShoppingCartId]) WITH (BUCKET_COUNT = 2097152),
    INDEX [ix_UserId] NONCLUSTERED HASH ([UserId]) WITH (BUCKET_COUNT = 1048576)
)
WITH (MEMORY_OPTIMIZED = ON);";

			StringReader scriptReader = new StringReader(script);			
			Parse(scriptReader).Accept(new IndexDefinitionVisitor(5, 76, script.IndexOf(@"INDEX [ix_UserId] NONCLUSTERED HASH ([UserId]) WITH (BUCKET_COUNT = 1048576)")));

			script = @"CREATE TABLE [dbo].[tbl] (
    [ShoppingCartId] INT NOT NULL INDEX [range_index]
);";
			scriptReader = new StringReader(script);			
			Parse(scriptReader).Accept(new IndexDefinitionVisitor(35, 19, script.IndexOf(@"INDEX [range_index]")));
		}

        /// <summary>
        /// Regression test for null exception in nested built in function calls
        /// </summary>
	    [TestMethod]
	    [Priority(0)]
	    [SqlStudioTestCategory(Category.UnitTest)]
	    public void NestedBuiltInFunctionTest()
	    {
	        string script = @"CREATE FUNCTION testFunction()
                  RETURNS INT
                  AS
                  BEGIN
                    RETURN ( floor(cast(5 as nvarchar(5)) + N'+'));
                  END;";

            StringReader scriptReader = new StringReader(script);
	        Parse(scriptReader);
	    }

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest, Feature.DataWarehouseUnitTesting, TestPlatform = SqlTestPlatform.SQLDw)]
        public void RenameDwTest()
        {
            string script = @"RENAME OBJECT T2 TO T1";

            StringReader scriptReader = new StringReader(script);
            TSqlParser parser = new TSql130Parser(true);
            IList<ParseError> errors;

            TSqlFragment fragment = parser.Parse(scriptReader, out errors);

            Assert.AreEqual(0, errors.Count);
        }

        /// <summary>
        /// Regression test for VSTS11190721: MATCH predicate at end of T-SQL script causes LastTokenIndex to be set incorrectly.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void MatchClauseLastTokenTest()
        {
            // Test Sql140 and Sql150 parsers.
            //
            MatchClauseLastTokenTestHelper(new TSql140Parser(false));
            MatchClauseLastTokenTestHelper(new TSql150Parser(false));
        }

        /// <summary>
        /// Regression test to verify column named 'URL' can be created
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void CreateTableColumnUrlTest()
        {
            string script = @"CREATE TABLE [dbo].[ShoppingCart4] (
                                [ShoppingCartId] INT           NOT NULL,
                                [URL]            VARCHAR(1024) NOT NULL,
                                [CreatedDate]    DATETIME2 (7) NOT NULL)";

            StringReader scriptReader = new StringReader(script);
            Parse(scriptReader);
        }

        /// <summary>
        /// Regression test to verify stored procedure that uses column named 'URL' can be created
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void CreateProcedureUrlTest()
        {
            string script = @"CREATE Procedure [dbo].[urlTest]
                              AS
                              INSERT INTO [dbo].[UrlTable] (Col1, Url) VALUES ('1', '2')
                              GO";

            StringReader scriptReader = new StringReader(script);
            Parse(scriptReader);
        }

        /// <summary>
        /// Regression test to verify token info (start line, start column, etc.) are correctly populated for CursorOption
        /// See https://github.com/microsoft/SqlScriptDOM/issues/26 for details on the issue.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void CursorOptionTokenInfoTest()
        {
            ParserTestUtils.ExecuteTestForAllParsers(delegate (TSqlParser parser)
            {
                string script = @"-- Single line comment
        DECLARE MyCursor scroll CURSOR FOR
        SELECT * FROM ANYTHING";

                using (var scriptReader = new StringReader(script))
                {
                    var fragment = parser.Parse(scriptReader, out IList<ParseError> errors) as TSqlScript;
                    Assert.AreEqual(0, errors.Count);

                    Assert.IsTrue(fragment is TSqlScript);
                    Assert.IsTrue(fragment.Batches[0].Statements[0] is DeclareCursorStatement);

                    var declCursorStmt = fragment.Batches[0].Statements[0] as DeclareCursorStatement;

                    Assert.AreEqual(3, declCursorStmt.CursorDefinition.Select.StartLine);
                    Assert.AreEqual(9, declCursorStmt.CursorDefinition.Select.StartColumn);

                    Assert.AreEqual(2, declCursorStmt.CursorDefinition.Options[0].StartLine);
                    Assert.AreEqual(26, declCursorStmt.CursorDefinition.Options[0].StartColumn);
                }
            }, true);
        }

        #region Private Methods

        private static string GenerateScript(TSqlFragment fragment)
		{
			string actualResult;
			SqlScriptGenerator scriptGenerator140 = new Sql140ScriptGenerator();
			scriptGenerator140.GenerateScript(fragment, out actualResult);
			return actualResult;
		}

		private static TSqlFragment Parse(TextReader reader)
		{
			TSqlParser parser = new TSql140Parser(true);
            IList<ParseError> errors;

			TSqlFragment fragment = parser.Parse(reader, out errors);

            Assert.AreEqual(0, errors.Count);
		    return fragment;
		}

        public void MatchClauseLastTokenTestHelper(TSqlParser parser)
        {
            string tsql = @"SELECT * FROM YelpUser AS U, YelpReview AS Reviews, YelpBusiness AS B WHERE MATCH(U-(Reviews)->B)";

            IList<ParseError> parsingErrors = null;

            TSqlFragment tree = parser.Parse(new System.IO.StringReader(tsql), out parsingErrors);
            Assert.AreEqual(0, parsingErrors.Count);

            TSqlParserToken res1 = tree.ScriptTokenStream[(tree as TSqlScript).LastTokenIndex - 1]; // -1 to exclude EndOfFile
            TSqlParserToken res2 = tree.ScriptTokenStream[(tree as TSqlScript).Batches[0].LastTokenIndex];

            // Both statements should yield the same token (a right closing parenthesis).
            //
            Assert.AreEqual(res1, res2);
            Assert.AreEqual(res1.TokenType, TSqlTokenType.RightParenthesis);
        }

        #endregion

        #region Private Classes

        class IndexDefinitionVisitor : TSqlFragmentVisitor
		{
			private int _startColumn;
			private int _fragmentLength;
			private int _startOffset;

			public IndexDefinitionVisitor(int startColumn, int fragmentLength, int startOffset) : base()
			{
				_startColumn = startColumn;
				_fragmentLength = fragmentLength;
				_startOffset = startOffset;
			}

			public override void Visit(IndexDefinition node)
			{
				Assert.AreEqual(_startColumn, node.StartColumn);
				Assert.AreEqual(_fragmentLength, node.FragmentLength);
				Assert.AreEqual(_startOffset, node.StartOffset);				
				base.Visit(node);
			}
		}

		#endregion
	}
}
