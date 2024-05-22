//------------------------------------------------------------------------------
// <copyright file="TSqlParser.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// The abstract base class for TSql Parsers.
    /// </summary>
    [Serializable]
    public abstract class TSqlParser
    {

        #region Constructor and private variables

        internal TSqlParser(bool quotedIdentifiersOn)
        {
            _quotedIdentifier = quotedIdentifiersOn;
        }

        private readonly bool _quotedIdentifier;

        /// <summary>
        /// Creates a TSqlParser for the specified version.
        /// </summary>
        /// <param name="tsqlParserVersion">The parser version to use.</param>
        /// <param name="initialQuotedIdentifiers">if set to <c>true</c> quoted identifiers will be on.</param>
        public static TSqlParser CreateParser(SqlVersion tsqlParserVersion, bool initialQuotedIdentifiers)
        {
            switch (tsqlParserVersion)
            {
                case SqlVersion.Sql80:
                    return new TSql80Parser(initialQuotedIdentifiers);
                case SqlVersion.Sql90:
                    return new TSql90Parser(initialQuotedIdentifiers);
                case SqlVersion.Sql100:
                    return new TSql100Parser(initialQuotedIdentifiers);
                case SqlVersion.Sql110:
                    return new TSql110Parser(initialQuotedIdentifiers);
                case SqlVersion.Sql120:
                    return new TSql120Parser(initialQuotedIdentifiers);
                case SqlVersion.Sql130:
                    return new TSql130Parser(initialQuotedIdentifiers);
                case SqlVersion.Sql140:
                    return new TSql140Parser(initialQuotedIdentifiers);
                case SqlVersion.Sql150:
                    return new TSql150Parser(initialQuotedIdentifiers);
                case SqlVersion.Sql160:
                    return new TSql160Parser(initialQuotedIdentifiers);
                default:
                    throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, SqlScriptGeneratorResource.UnknownEnumValue, tsqlParserVersion, "TSqlParserVersion"), "tsqlParserVersion");
            }
        }

        /// <summary>
        /// Creates a TSqlParser for the specified version.
        /// </summary>
        /// <param name="tsqlParserVersion">The parser version to use.</param>
        /// <param name="initialQuotedIdentifiers">if set to <c>true</c> quoted identifiers will be on.</param>
        public TSqlParser Create(SqlVersion tsqlParserVersion, bool initialQuotedIdentifiers)
        {
            return TSqlParser.CreateParser(tsqlParserVersion, initialQuotedIdentifiers);
        }

        #endregion

        #region Parser entry points

            /// <summary>
            /// The blocking parse method.
            /// </summary>
            /// <param name="input">The script that will be parsed.</param>
            /// <param name="errors">The parse errors.</param>
            /// <returns>The fragment that is created.</returns>        
            public TSqlFragment Parse(TextReader input, out IList<ParseError> errors)
        {
            return Parse(input, out errors, 0, 1, 1);
        }

		/// <summary>
        /// The blocking parse method.
        /// </summary>
        /// <param name="input">The script that will be parsed.</param>
        /// <param name="errors">The parse errors.</param>
		/// <param name="startOffset">The starting offset of input.</param>        
		/// <param name="startLine">The starting line number for the input.</param>
		/// <param name="startColumn">The starting column position for the input.</param>	
        /// <returns>The fragment that is created.</returns>        
        public TSqlFragment Parse(TextReader input, out IList<ParseError> errors, 
            int startOffset, int startLine, int startColumn)
        {
            IList<TSqlParserToken> tokens = GetTokenStream(input, out errors, startOffset, startLine, startColumn);
            if (errors.Count > 0)
                return new TSqlScript();
            else
                return Parse(tokens, out errors);            
        }

        /// <summary>
        /// Parses the specified tokens into a TSqlFragment.
        /// </summary>
        /// <param name="tokens">The input tokens.</param>
        /// <param name="errors">The IList that the parse errors will be added to.</param>
        /// <returns></returns>
        public abstract TSqlFragment Parse(IList<TSqlParserToken> tokens, out IList<ParseError> errors);

        /// <summary>
        /// Parses an input string to get a ChildObjectName.  This will return null, if there were any errors.
        /// </summary>
        public abstract ChildObjectName ParseChildObjectName(TextReader input, out IList<ParseError> errors, int startOffset, int startLine, int startColumn);

        /// <summary>
        /// Parses an input string to get a ChildObjectName.  This will return null, if there were any errors.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="errors">The collection of errors.</param>
        /// <returns></returns>
        public ChildObjectName ParseChildObjectName(TextReader input, out IList<ParseError> errors)
        {
            return ParseChildObjectName(input, out errors, 0, 1, 1);
        }

        /// <summary>
        /// Parses an input string to get a SchemaObjectName.  This will return null, if there were any errors.
        /// </summary>
        public abstract SchemaObjectName ParseSchemaObjectName(TextReader input, out IList<ParseError> errors, int startOffset, int startLine, int startColumn);

        /// <summary>
        /// Parses an input string to get a SchemaObjectName.  This will return null, if there were any errors.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="errors">The collection of errors.</param>
        /// <returns></returns>
        public SchemaObjectName ParseSchemaObjectName(TextReader input, out IList<ParseError> errors)
        {
            return ParseSchemaObjectName(input, out errors, 0, 1, 1);
        }

        /// <summary>
        /// Parses an input string to get a data type. This will return null, if there were any errors.
        /// </summary>
        public abstract DataTypeReference ParseScalarDataType(TextReader input, out IList<ParseError> errors, int startOffset, int startLine, int startColumn);

        /// <summary>
        /// Parses an input string to get a data type. This will return null, if there were any errors.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="errors">The collection of errors.</param>
        /// <returns></returns>
        public DataTypeReference ParseScalarDataType(TextReader input, out IList<ParseError> errors)
        {
            return ParseScalarDataType(input, out errors, 0, 1, 1);
        }

        /// <summary>
        /// Parses an input string to get a constant or identifier. This will return null, if there were any errors.
        /// </summary>
        public abstract TSqlFragment ParseConstantOrIdentifier(TextReader input, out IList<ParseError> errors, int startOffset, int startLine, int startColumn);

        /// <summary>
        /// Parses an input string to get a constant or identifier. This will return null, if there were any errors.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="errors">The collection of errors.</param>
        /// <returns></returns>
        public TSqlFragment ParseConstantOrIdentifier(TextReader input, out IList<ParseError> errors)
        {
            return ParseConstantOrIdentifier(input, out errors, 0, 1, 1);
        }

        /// <summary>
        /// Parses an input string to get a constant or identifier or default literal(literal with value "DEFAULT"). This will return null, if there were any errors.
        /// </summary>
        public abstract TSqlFragment ParseConstantOrIdentifierWithDefault(TextReader input, out IList<ParseError> errors, int startOffset, int startLine, int startColumn);

        /// <summary>
        /// Parses an input string to get a constant or identifier or default literal(literal with value "DEFAULT"). This will return null, if there were any errors.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="errors">The collection of errors.</param>
        /// <returns></returns>
        public TSqlFragment ParseConstantOrIdentifierWithDefault(TextReader input, out IList<ParseError> errors)
        {
            return ParseConstantOrIdentifierWithDefault(input, out errors, 0, 1, 1);
        }

        /// <summary>
        /// Parses an input string to get a scalar expression. This will return null, if there were any errors.
        /// </summary>
        public abstract ScalarExpression ParseExpression(TextReader input, out IList<ParseError> errors, int startOffset, int startLine, int startColumn);

        /// <summary>
        /// Parses an input string to get a scalar expression. This will return null, if there were any errors.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="errors">The collection of errors.</param>
        /// <returns></returns>
        public ScalarExpression ParseExpression(TextReader input, out IList<ParseError> errors)
        {
            return ParseExpression(input, out errors, 0, 1, 1);
        }

        /// <summary>
        /// Parses an input string to get a boolean expression. This will return null, if there were any errors.
        /// </summary>
        public abstract BooleanExpression ParseBooleanExpression(TextReader input, out IList<ParseError> errors, int startOffset, int startLine, int startColumn);

        /// <summary>
        /// Parses an input string to get a boolean expression. This will return null, if there were any errors.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="errors">The collection of errors.</param>
        /// <returns></returns>
        public BooleanExpression ParseBooleanExpression(TextReader input, out IList<ParseError> errors)
        {
            return ParseBooleanExpression(input, out errors, 0, 1, 1);
        }

        /// <summary>
        /// Parses an input string to get a statement list. This will return null, if there were any errors.
        /// </summary>
        public abstract StatementList ParseStatementList(TextReader input, out IList<ParseError> errors, int startOffset, int startLine, int startColumn);

        /// <summary>
        /// Parses an input string to get a statement list. This will return null, if there were any errors.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="errors">The collection of errors.</param>
        /// <returns></returns>
        public StatementList ParseStatementList(TextReader input, out IList<ParseError> errors)
        {
            return ParseStatementList(input, out errors, 0, 1, 1);
        }

        /// <summary>
        /// Parses an input string to get a subquery expression with optional common table expression
        /// and xml namespaces. This will return null, if there were any errors.
        /// </summary>
        public abstract SelectStatement ParseSubQueryExpressionWithOptionalCTE(TextReader input, out IList<ParseError> errors, int startOffset, int startLine, int startColumn);

        /// <summary>
        /// Parses an input string to get a subquery expression with optional common table expression
        /// and xml namespaces. This will return null, if there were any errors.
        /// </summary>
        /// <param name="input">The input script.</param>
        /// <param name="errors">The errors encountered, if any.</param>
        /// <returns>The select statement, or null if errors were encountered.</returns>
        public SelectStatement ParseSubQueryExpressionWithOptionalCTE(TextReader input, out IList<ParseError> errors)
        {
            return ParseSubQueryExpressionWithOptionalCTE(input, out errors, 0, 1, 1);
        }

        /// <summary>
        /// Parses the input script to return the SchemaObjectName of the first create/alter sql_module statement, if found.
        /// </summary>
        /// <param name="input">The input script</param>
        /// <param name="result">The output schema object name.  Null if not found.</param>
        /// <returns>True if a result is returned, else false.</returns>
        public bool TryParseSqlModuleObjectName(TextReader input, out SchemaObjectName result)
        {
            TSqlStatement statement = PhaseOneParse(input);
            ExtractSchemaObjectNameVisitor extractSchemaObjectNameVisitor = new ExtractSchemaObjectNameVisitor();
            result = null;
            if (statement != null)
            {
                statement.Accept(extractSchemaObjectNameVisitor);
                result = extractSchemaObjectNameVisitor.SchemaObjectName;
            }
            return result != null;
        }

        /// <summary>
        /// Parses the input script to return the trigger name and trigger target name of the first create/alter trigger statement.  
        /// </summary>
        /// <param name="input">The input script</param>
        /// <param name="triggerName">The output trigger object name.  Null if not found.</param>
        /// <param name="targetName">The output trigger target object name.  Null if not found.</param>
        /// <returns>True if a trigger target is found, else false.</returns>
        public bool TryParseTriggerModule(TextReader input, out SchemaObjectName triggerName, out SchemaObjectName targetName)
        {
            TSqlStatement statement = PhaseOneParse(input);
            ExtractSchemaObjectNameVisitor extractSchemaObjectNameVisitor = new ExtractSchemaObjectNameVisitor();
            triggerName = null;
            targetName = null;
            if (statement != null)
            {
                statement.Accept(extractSchemaObjectNameVisitor);
                triggerName = extractSchemaObjectNameVisitor.SchemaObjectName;
                targetName = extractSchemaObjectNameVisitor.TriggerTargetName;
            }
            return targetName != null;
        }

        internal abstract TSqlStatement PhaseOneParse(TextReader input);

        #endregion

        #region Lexer entry points

        /// <summary>
        /// Parses the input into a Token Stream.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="errors">The collection of errors.</param>
        /// <returns></returns>
        public IList<TSqlParserToken> GetTokenStream(TextReader input, out IList<ParseError> errors)
        {
            return GetTokenStream(input, out errors, 0, 1, 1);
        }

        /// <summary>
        /// Parses the input into a Token Stream.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="errors">The collection of errors.</param>
        /// <param name="startOffset">The start offset.</param>
        /// <param name="startLine">The start line.</param>
        /// <param name="startColumn">The start column.</param>
        /// <returns></returns>
        public IList<TSqlParserToken> GetTokenStream(TextReader input, out IList<ParseError> errors, 
            int startOffset, int startLine, int startColumn)
        {
            errors = new List<ParseError>();
            Collection<TSqlParserToken> tokenStream = new Collection<TSqlParserToken>();

            TSqlLexerBaseInternal lexer = GetNewInternalLexer();
            lexer.InitializeForNewInput(startOffset, startLine, startColumn, input);

            TSqlParserToken token = null;
            do
            {
                try
                {
                    token = (TSqlParserToken)lexer.nextToken();
                    tokenStream.Add(token);
                }
                catch (antlr.TokenStreamRecognitionException e)
                {
                    ParseError error = TSql80ParserBaseInternal.ProcessTokenStreamRecognitionException(e, lexer.CurrentOffset);
                    errors.Add(error);
                    break;
                }
                catch (TSqlParseErrorException e)
                {
                    errors.Add(e.ParseError);
                    continue;
                }
            } while (token != null && token.TokenType != TSqlTokenType.EndOfFile);
            return tokenStream;
        }

        /// <summary>
        /// Check if a name is a valid sql identifier
        /// </summary>
        /// <param name="name">The string to be checked.</param>
        /// <returns>True if it is a valid identifier.</returns>
        public bool ValidateIdentifier(string name)
        {
            // Check if it's empty string
            if (string.IsNullOrEmpty(name))
                return false;

            using (StringReader input = new StringReader(name))
            {
                IList<ParseError> errors;
                IList<TSqlParserToken> tokens = GetTokenStream(input, out errors, 0, 1, 1);

                if (tokens.Count == 2 &&
                    tokens[1].TokenType == TSqlTokenType.EndOfFile &&
                    (tokens[0].TokenType == TSqlTokenType.Identifier ||
                     tokens[0].TokenType == TSqlTokenType.QuotedIdentifier ||
                     tokens[0].TokenType == TSqlTokenType.AsciiStringOrQuotedIdentifier) &&
                    String.Equals(name, tokens[0].Text, StringComparison.Ordinal))
                    return true;
            }
            return false;
        }

        #endregion

        /// <summary>
        /// Indicates whether quoted identifier is enabled for the parser.
        /// </summary>
        public bool QuotedIdentifier
        {
            get { return _quotedIdentifier; }
        }

        #region Shared internal utility methods

        internal abstract TSqlLexerBaseInternal GetNewInternalLexer();

        internal void InitializeInternalParserInput(TSql80ParserBaseInternal parser, TextReader input, out IList<ParseError> errors, 
            int startOffset, int startLine, int startColumn)
        {
            IList<TSqlParserToken> tokens = GetTokenStream(input, out errors, startOffset, startLine, startColumn);
            parser.InitializeForNewInput(tokens, errors, false);
        }

        internal TSqlStatement PhaseOneParseImpl(TSql80ParserBaseInternal parser, 
            TSql80ParserBaseInternal.ParserEntryPoint<TSqlScript> entryPoint, String entryPointName, TextReader input)
        {
            IList<ParseError> errors;
            IList<TSqlParserToken> tokens = GetTokenStream(input, out errors, 0, 1, 1);
            parser.InitializeForNewInput(tokens, errors, true);

            bool reenter = true;
            while (reenter == true)
            {
                reenter = false;
                try
                {
                    TSqlScript fragment = parser.ParseRuleWithStandardExceptionHandling<TSqlScript>(entryPoint, entryPointName);
                }
                catch (PhaseOnePartialAstException exception)
                {
                    return exception.Statement;
                }
                catch (PhaseOneBatchException)
                {
                    reenter = true;
                }
            }

            return null;
        }
        #endregion

        internal const String ScriptEntryMethod = "script";

        private class ExtractSchemaObjectNameVisitor : TSqlFragmentVisitor
        {
            public SchemaObjectName SchemaObjectName
            { get; private set; }

            public SchemaObjectName TriggerTargetName
            { get; private set; }

            public override void Visit(ProcedureStatementBody node)
            {
                this.SchemaObjectName = node.ProcedureReference.Name;
            }

            public override void Visit(ViewStatementBody node)
            {
                this.SchemaObjectName = node.SchemaObjectName;
            }

            public override void Visit(FunctionStatementBody node)
            {
                this.SchemaObjectName = node.Name;
            }

            public override void Visit(TriggerStatementBody node)
            {
                this.SchemaObjectName = node.Name;
                this.TriggerTargetName = node.TriggerObject.Name;
            }
        }
    }
}
