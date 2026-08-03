//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CommonPhrases.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        // generate memory size
        protected void GenerateSpaceAndMemoryUnit(MemoryUnit unit)
        {
            if (unit != MemoryUnit.Unspecified)
            {
                GenerateSpace();
                MemoryUnitsHelper.Instance.GenerateSourceForOption(_writer, unit);
            }
        }

        // generate AUTHORIZATION
        protected void GenerateOwnerIfNotNull(Identifier owner)
        {
            if (owner != null)
            {
                NewLineAndIndent();
                GenerateKeyword(TSqlTokenType.Authorization);
                GenerateSpace();
                owner.Accept(this);
            }
        }

        // generate WITH CREDENTIAL = identifier
        private void GenerateCredential(Identifier identifier)
        {
            if (identifier != null)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.With);
                GenerateSpace();
                GenerateNameEqualsValue(CodeGenerationSupporter.Credential, identifier);
            }
        }

        // generated REMOVE PRIVATE KEY, used by AlterAsymmetricKeyStatement and AlterCertificateStatement
        protected void GenerateRemovePrivateKey()
        {
            GenerateIdentifier(CodeGenerationSupporter.Remove);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Private);
            GenerateSpaceAndKeyword(TSqlTokenType.Key);
        }

        //  generated ATTESTED BY, used by AlterAsymmetricKeyStatement and AlterCertificateStatement
        protected void GenerateAttestedBy(Literal attestedBy)
        {
            GenerateIdentifier(CodeGenerationSupporter.Attested);
            GenerateSpaceAndKeyword(TSqlTokenType.By);
            GenerateSpaceAndFragmentIfNotNull(attestedBy);
        }

        // generated REMOVE ATTESTED OPTION, used by AlterAsymmetricKeyStatement and AlterCertificateStatement
        protected void GenerateRemoteAttestedOption()
        {
            GenerateIdentifier(CodeGenerationSupporter.Remove);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Attested);
            GenerateSpaceAndKeyword(TSqlTokenType.Option);
        }

        /// Generates WITH PRIVATE KEY (), used by CREATE/ALTER CERTIFICATE, and ALTER ASYMMETRIC KEY
        internal void GenerateWithPrivateKey(Literal privateKeyPath, Literal encryptionPassword, Literal decryptionPassword)
        {
            GenerateKeyword(TSqlTokenType.With);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Private);
            GenerateSpaceAndKeyword(TSqlTokenType.Key);

            GenerateSpaceAndSymbol(TSqlTokenType.LeftParenthesis);

            bool first = true;
            if (privateKeyPath != null)
            {
                first = false;
                GenerateNameEqualsValue(TSqlTokenType.File, privateKeyPath);
            }

            if (decryptionPassword != null)
            {
                if (first == false)
                {
                    GenerateSymbolAndSpace(TSqlTokenType.Comma);
                }
                else
                {
                    first = false;
                }

                GenerateIdentifier(CodeGenerationSupporter.Decryption);
                GenerateSpaceAndKeyword(TSqlTokenType.By);
                GenerateSpace();
                GenerateNameEqualsValue(CodeGenerationSupporter.Password, decryptionPassword);
            }

            if (encryptionPassword != null)
            {
                if (first == false)
                {
                    GenerateSymbolAndSpace(TSqlTokenType.Comma);
                }
                else
                {
                    first = false;
                }

                GenerateIdentifier(CodeGenerationSupporter.Encryption);
                GenerateSpaceAndKeyword(TSqlTokenType.By);
                GenerateSpace();
                GenerateNameEqualsValue(CodeGenerationSupporter.Password, encryptionPassword);
            }

            GenerateSymbol(TSqlTokenType.RightParenthesis);
        }

        protected void GenerateSpaceAndCollation(Identifier collation)
        {
            if (collation != null)
            {
                GenerateSpace();
                GenerateKeyword(TSqlTokenType.Collate);
                // Collation names are stored as Identifier fragments but cannot be delimited or recased
                // (COLLATE [name] is invalid T-SQL), so suppress identifier formatting for them.
                GenerateWithoutIdentifierFormatting(() => GenerateSpaceAndFragmentIfNotNull(collation));
            }
        }

        protected void GenerateTriggerEnforcement(TriggerEnforcement triggerEnforcement)
        {
            switch (triggerEnforcement)
            {
                case TriggerEnforcement.Disable:
                    GenerateIdentifier(CodeGenerationSupporter.Disable);
                    break;
                case TriggerEnforcement.Enable:
                    GenerateIdentifier(CodeGenerationSupporter.Enable);
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false, "An unhandled option is encountered.");
                    break;
            }
        }

        // generate NOT FOR REPLICATION
        protected void GenerateNotForReplication()
        {
            GenerateSpaceSeparatedTokens(
                TSqlTokenType.Not,
                TSqlTokenType.For,
                TSqlTokenType.Replication);
        }

        // generate ENCRIPTION BY PASSWORD = 'password'
        protected void GenerateDecryptionByPassword(Literal password)
        {
            GenerateIdentifier(CodeGenerationSupporter.Decryption);

            GenerateSpace();
            GenerateByPassword(password);
        }

        // generate DECRIPTION BY PASSWORD = 'password'
        protected void GenerateEncryptionByPassword(Literal password)
        {
            GenerateIdentifier(CodeGenerationSupporter.Encryption);

            GenerateSpace();
            GenerateByPassword(password);
        }

        // generate BY PASSWORD = 'password'
        protected void GenerateByPassword(Literal password)
        {
            GenerateKeywordAndSpace(TSqlTokenType.By);
            GenerateNameEqualsValue(CodeGenerationSupporter.Password, password);
        }

        protected static Dictionary<BinaryExpressionType, List<TokenGenerator>> _binaryExpressionTypeGenerators =
            new Dictionary<BinaryExpressionType, List<TokenGenerator>>()
        {
            { BinaryExpressionType.Add, new List<TokenGenerator>() {
                new KeywordGenerator(TSqlTokenType.Plus) }},
            { BinaryExpressionType.Subtract, new List<TokenGenerator>() {
                new KeywordGenerator(TSqlTokenType.Minus) }},
            { BinaryExpressionType.Multiply, new List<TokenGenerator>() {
                new KeywordGenerator(TSqlTokenType.Star) }},
            { BinaryExpressionType.Divide, new List<TokenGenerator>() {
                new KeywordGenerator(TSqlTokenType.Divide) }},
            { BinaryExpressionType.Modulo, new List<TokenGenerator>() {
                new KeywordGenerator(TSqlTokenType.PercentSign) }},
            { BinaryExpressionType.BitwiseAnd, new List<TokenGenerator>() {
                new KeywordGenerator(TSqlTokenType.Ampersand) }},
            { BinaryExpressionType.BitwiseOr, new List<TokenGenerator>() {
                new KeywordGenerator(TSqlTokenType.VerticalLine) }},
            { BinaryExpressionType.BitwiseXor, new List<TokenGenerator>() {
                new KeywordGenerator(TSqlTokenType.Circumflex) }},
            { BinaryExpressionType.LeftShift, new List<TokenGenerator>() {
                new KeywordGenerator(TSqlTokenType.LeftShift) }},
            { BinaryExpressionType.RightShift, new List<TokenGenerator>() {
                new KeywordGenerator(TSqlTokenType.RightShift) }},
            { BinaryExpressionType.Concat, new List<TokenGenerator>() {
                new KeywordGenerator(TSqlTokenType.Concat) }},
        };

        protected static Dictionary<BooleanComparisonType, List<TokenGenerator>> _booleanComparisonTypeGenerators =
            new Dictionary<BooleanComparisonType, List<TokenGenerator>>()
        {
            { BooleanComparisonType.Equals, new List<TokenGenerator>() {
                new KeywordGenerator(TSqlTokenType.EqualsSign) }},
            { BooleanComparisonType.GreaterThan, new List<TokenGenerator>() {
                new KeywordGenerator(TSqlTokenType.GreaterThan) }},
            { BooleanComparisonType.LessThan, new List<TokenGenerator>() {
                new KeywordGenerator(TSqlTokenType.LessThan) }},
            { BooleanComparisonType.GreaterThanOrEqualTo, new List<TokenGenerator>() {
                new KeywordGenerator(TSqlTokenType.GreaterThan),
                new KeywordGenerator(TSqlTokenType.EqualsSign) }},
            { BooleanComparisonType.LessThanOrEqualTo, new List<TokenGenerator>() {
                new KeywordGenerator(TSqlTokenType.LessThan),
                new KeywordGenerator(TSqlTokenType.EqualsSign) }},
            { BooleanComparisonType.NotEqualToBrackets, new List<TokenGenerator>() {
                new KeywordGenerator(TSqlTokenType.LessThan),
                new KeywordGenerator(TSqlTokenType.GreaterThan) }},
            { BooleanComparisonType.NotEqualToExclamation, new List<TokenGenerator>() {
                new KeywordGenerator(TSqlTokenType.Bang),
                new KeywordGenerator(TSqlTokenType.EqualsSign) }},
            { BooleanComparisonType.NotLessThan, new List<TokenGenerator>() {
                new KeywordGenerator(TSqlTokenType.Bang),
                new KeywordGenerator(TSqlTokenType.LessThan) }},
            { BooleanComparisonType.NotGreaterThan, new List<TokenGenerator>() {
                new KeywordGenerator(TSqlTokenType.Bang),
                new KeywordGenerator(TSqlTokenType.GreaterThan) }},
            { BooleanComparisonType.LeftOuterJoin, new List<TokenGenerator>() {
                new KeywordGenerator(TSqlTokenType.MultiplyEquals) }},
            { BooleanComparisonType.RightOuterJoin, new List<TokenGenerator>() {
                new KeywordGenerator(TSqlTokenType.RightOuterJoin) }},
            { BooleanComparisonType.NotLike, new List<TokenGenerator>() {
                new KeywordGenerator(TSqlTokenType.Not, true),
                new KeywordGenerator(TSqlTokenType.Like) }},
            { BooleanComparisonType.IsDistinctFrom, new List<TokenGenerator>() {
                new KeywordGenerator(TSqlTokenType.Is, true),
                new KeywordGenerator(TSqlTokenType.Distinct, true),
                new KeywordGenerator(TSqlTokenType.From) }},
            { BooleanComparisonType.IsNotDistinctFrom, new List<TokenGenerator>() {
                new KeywordGenerator(TSqlTokenType.Is, true),
                new KeywordGenerator(TSqlTokenType.Not, true),
                new KeywordGenerator(TSqlTokenType.Distinct, true),
                new KeywordGenerator(TSqlTokenType.From) }}
        };

        protected static Dictionary<BooleanBinaryExpressionType, List<TokenGenerator>> _booleanBinaryExpressionTypeGenerators =
            new Dictionary<BooleanBinaryExpressionType, List<TokenGenerator>>()
        {
            { BooleanBinaryExpressionType.And, new List<TokenGenerator>() {
                new KeywordGenerator(TSqlTokenType.And) }},
            { BooleanBinaryExpressionType.Or, new List<TokenGenerator>() {
                new KeywordGenerator(TSqlTokenType.Or) }},
        };

        // generate binary operator
        protected void GenerateBinaryOperator(BinaryExpressionType operatorType)
        {
            List<TokenGenerator> generators = GetValueForEnumKey(_binaryExpressionTypeGenerators, operatorType);
            GenerateTokenList(generators);
        }

        protected void GenerateBinaryOperator(BooleanComparisonType operatorType)
        {
            List<TokenGenerator> generators = GetValueForEnumKey(_booleanComparisonTypeGenerators, operatorType);
            GenerateTokenList(generators);
        }

        protected void GenerateBinaryOperator(BooleanBinaryExpressionType operatorType)
        {
            List<TokenGenerator> generators = GetValueForEnumKey(_booleanBinaryExpressionTypeGenerators, operatorType);
            GenerateTokenList(generators);
        }

        // generate UniqueRowFilter
        protected void GenerateUniqueRowFilter(UniqueRowFilter uniqueRowFilter, bool spaceBefore)
        {
            if (uniqueRowFilter != UniqueRowFilter.NotSpecified)
            {
				if (spaceBefore)
					GenerateSpace();

				if (uniqueRowFilter == UniqueRowFilter.All)
					GenerateKeyword(TSqlTokenType.All);
				else if (uniqueRowFilter == UniqueRowFilter.Distinct)
					GenerateKeyword(TSqlTokenType.Distinct);
				else
					Debug.Assert(false, "Unknown value in UniqueRowFilter enum");
            }
        }

        // generate a new line or a space
        protected void GenerateNewLineOrSpace(Boolean newline)
        {
            // Only write a newline if the newlineBeforeXXXClause option is set
            // (that's what is passed in)
            if (newline)
            {
                NewLine();
            }
            else
            {
                GenerateSpace();
            }
        }

        // Mark the shared cross-clause "river" alignment point for a clause body, when the current
        // options call for it.
        //
        // ClauseBodyAlignment.Indented never uses the river: a clause body either moves onto its own
        // indented line (see GenerateClauseBodyStart) or stays on the keyword line separated by a
        // single space. Marking in that mode would re-introduce the river padding the Indented layout
        // deliberately drops, so the mode is part of the guard below instead of being repeated by
        // every caller. Every other case - including the default (Aligned) - is unchanged.
        protected void MarkClauseBodyAlignmentWhenNecessary(Boolean newline, AlignmentPoint ap)
        {
            // If we didn't put a newline in, don't align, even if AlignClauseBodies is on.
            if (newline && _options.AlignClauseBodies && _options.ClauseBodyAlignment != ClauseBodyAlignment.Indented)
            {
#if !PIMODLANGUAGE
                Debug.Assert(ap != null, "Alignment point should not be null");
#endif
                if (ap != null)
                {
                    Mark(ap);
                }
            }
        }

        // Handle the transition from a clause keyword to its body when the body would start on a new
        // line (newline == true).
        //
        // Default (Aligned) mode: this is a pass-through - it marks the shared clause-body alignment
        // point exactly as before (via MarkClauseBodyAlignmentWhenNecessary) and returns false, so the
        // caller emits the usual separating space. The combination is identical to the original
        // "MarkClauseBodyAlignmentWhenNecessary(...); GenerateSpace();" pair, so the default output
        // does not change.
        //
        // Indented mode: the body is broken onto its own line, indented one level, and this returns
        // true so the caller skips the separating space.
        protected Boolean GenerateClauseBodyStart(Boolean newline, AlignmentPoint ap)
        {
            if (newline && _options.ClauseBodyAlignment == ClauseBodyAlignment.Indented)
            {
                NewLineAndIndent();
                return true;
            }

            MarkClauseBodyAlignmentWhenNecessary(newline, ap);
            return false;
        }

        protected void MarkInsertColumnsAlignmentPointWhenNecessary(AlignmentPoint ap)
        {
#if !PIMODLANGUAGE
            Debug.Assert(ap != null, "Alignment point should not be null");
#endif
            if (ap != null)
            {
                Mark(ap);
            }
        }

        protected void GenerateSeparatorForOrderBy()
        {
            GenerateNewLineOrSpace(_options.NewLineBeforeOrderByClause);
        }

        protected void GenerateSeparatorForFromClause()
        {
            GenerateNewLineOrSpace(_options.NewLineBeforeFromClause);
        }

        protected void GenerateSeparatorForWhereClause()
        {
            GenerateNewLineOrSpace(_options.NewLineBeforeWhereClause);
        }

        protected void GenerateSeparatorForGroupByClause()
        {
            GenerateNewLineOrSpace(_options.NewLineBeforeGroupByClause);
        }

        protected void GenerateSeparatorForHavingClause()
        {
            GenerateNewLineOrSpace(_options.NewLineBeforeHavingClause);
        }

        protected void GenerateSeparatorForWindowClause()
        {
            GenerateNewLineOrSpace(_options.NewLineBeforeWindowClause);
        }

        protected void GenerateSeparatorForOutputClause()
        {
            GenerateNewLineOrSpace(_options.NewLineBeforeOutputClause);
        }

        protected void GenerateSeparatorForOffsetClause()
        {
            GenerateNewLineOrSpace(_options.NewLineBeforeOffsetClause);
        }

        protected void GenerateQueryExpressionInParentheses(QueryExpression queryExpression)
        {
            GenerateSymbol(TSqlTokenType.LeftParenthesis);

            AlignmentPoint queryBody = new AlignmentPoint();
            MarkAndPushAlignmentPoint(queryBody);

            if (queryExpression != null)
            {
                AlignmentPoint clauseBody = new AlignmentPoint(ClauseBody);
                GenerateFragmentWithAlignmentPointIfNotNull(queryExpression, clauseBody);
            }

            PopAlignmentPoint();

            GenerateSymbol(TSqlTokenType.RightParenthesis);
        }

        // Emits the parameter list for a CREATE/ALTER PROCEDURE or FUNCTION statement.
        //
        // The default (MultilineProcedureParametersList == false) is intentionally the existing,
        // unchanged behavior: all parameters are written on a single line - function parameters in
        // parentheses, procedure parameters without. Multi-line output (one parameter per line,
        // indented one level from the procedure/function name) is strictly opt-in via the option.
        // CommaPlacement is honored by the underlying list generation when multi-line is enabled.
        protected void GenerateProcedureOrFunctionParameters(IList<ProcedureParameter> parameters, bool parenthesized)
        {
            bool hasParameters = parameters != null && parameters.Count > 0;

            // Default path: unchanged single-line behavior. Taken whenever the option is off (its
            // default) or there is nothing to spread across multiple lines.
            if (!_options.MultilineProcedureParametersList || !hasParameters)
            {
                if (parenthesized)
                {
                    NewLine();
                    GenerateParenthesisedCommaSeparatedList(parameters);
                    if (!hasParameters)
                    {
                        GenerateSymbol(TSqlTokenType.LeftParenthesis);
                        GenerateSpaceAndSymbol(TSqlTokenType.RightParenthesis);
                    }
                }
                else if (hasParameters)
                {
                    NewLine();
                    GenerateCommaSeparatedList(parameters);
                }

                return;
            }

            // Opt-in path: one parameter per line, indented one level.
            if (parenthesized)
            {
                ListGenerationOption option = ListGenerationOption.CreateOptionFromFormattingConfig(_options);
                GenerateFragmentList(parameters, option);
            }
            else
            {
                // The option produces its own leading new line before the first parameter.
                GenerateFragmentList(parameters, ListGenerationOption.MultipleLineProcedureParameterOption);
            }
        }

        // True while rendering a SELECT projection list (QuerySpecification.SelectElements).
        // Restricts the "alias = expression" ColumnAliasStyle form to real SELECT projections,
        // because OUTPUT, OUTPUT INTO and RECEIVE reuse SelectScalarExpression but do not
        // allow that form.
        private bool _inSelectProjection;

        private void GenerateSelectElementsList(IList<SelectElement> selectElements)
        {
            // SelectScalarExpression is shared with non-projection contexts (OUTPUT / OUTPUT INTO /
            // RECEIVE). Mark that we are rendering a real SELECT projection so that column aliases
            // may honor the ColumnAliasStyle option here (and only here). The previous value is
            // restored so nested scalar subqueries remain independent.
            bool previousInSelectProjection = _inSelectProjection;
            _inSelectProjection = true;
            try
            {
                if (_options.MultilineSelectElementsList == false)
                {
                    GenerateCommaSeparatedList(selectElements);
                }
                else if (_options.ColumnAliasStyle != ColumnAliasStyle.AsKeyword && _options.AlignClauseBodies)
                {
                    // Push a dedicated alignment scope so that "alias = expression" equals signs
                    // align only within this SELECT list. Nested subqueries push their own scope
                    // and therefore align independently.
                    AlignmentPoint selectItems = new AlignmentPoint();
                    MarkAndPushAlignmentPoint(selectItems);

                    // Hold the "=" alignment point in a field for the duration of this SELECT list.
                    // When PreserveComments is enabled, GenerateFragmentList wraps each select
                    // element in its own freshly pushed alignment scope (which resets the writer's
                    // alignment-point name map), so resolving the point by name would yield a
                    // distinct point per row and the "=" signs would not align across rows. Keeping
                    // the point in a field lets every row share it. The previous value is restored
                    // so nested SELECT projections align independently.
                    AlignmentPoint previousEqualSignAlignmentPoint = _selectColumnAliasEqualSignAlignmentPoint;
                    _selectColumnAliasEqualSignAlignmentPoint = new AlignmentPoint(SelectColumnAliasEqualSign);
                    try
                    {
                        GenerateFragmentList(selectElements, ListGenerationOption.MultipleLineSelectElementOption);
                    }
                    finally
                    {
                        _selectColumnAliasEqualSignAlignmentPoint = previousEqualSignAlignmentPoint;
                    }

                    PopAlignmentPoint();
                }
                else
                {
                    GenerateFragmentList(selectElements, ListGenerationOption.MultipleLineSelectElementOption);
                }
            }
            finally
            {
                _inSelectProjection = previousInSelectProjection;
            }
        }

        // Mark the alignment point used to vertically align the "=" signs when column aliases
        // are rendered as "alias = expression". Only applies when the SELECT list is written
        // on multiple lines and clause-body alignment is enabled.
        protected void MarkColumnAliasEqualSignAlignmentWhenNecessary()
        {
            if (_options.MultilineSelectElementsList && _options.AlignClauseBodies)
            {
                // Prefer the alignment point held for the current SELECT list so that the "=" signs
                // align across all rows even when PreserveComments pushes a per-row alignment scope.
                // Fall back to name resolution for safety if the field was not set.
                AlignmentPoint ap = _selectColumnAliasEqualSignAlignmentPoint
                    ?? FindOrCreateAlignmentPointByName(SelectColumnAliasEqualSign);
                if (ap != null)
                {
                    Mark(ap);
                }
            }
        }

        protected void GenerateParameters(ParameterizedDataTypeReference node)
        {
            if (node.Parameters.Count > 0)
            {
                if (_options.SpaceBetweenDataTypeAndParameters) {
                    GenerateSpace();
                }
                GenerateParenthesisedCommaSeparatedList(node.Parameters, false, _options.SpaceBetweenParametersInDataType);
            }
        }

        internal abstract HashSet<Type> StatementsThatCannotHaveSemiColon
        {
            get;
        }

        // some statements can be part of another statment, for example, SELECT statement can be 
        // part of CREATE VIEW statement, and we don't want to generate semicolon for the included statements
        protected Boolean _generateSemiColon = true;

        protected void GenerateSemiColonWhenNecessary(TSqlStatement node)
        {
            if (node != null &&
                _generateSemiColon &&
                StatementsThatCannotHaveSemiColon.Contains(node.GetType()) == false)
            {
                GenerateSymbol(TSqlTokenType.Semicolon);
            }
        }

        // Some statements must be preceded by a semicolon terminator to be valid in SQL Server:
        // statements that begin with a WITH clause (common table expression / XMLNAMESPACES) and
        // the THROW statement. When such a statement follows a statement that was not already
        // terminated with a semicolon (for example an IF / BEGIN...END / WHILE / TRY...CATCH block,
        // whose generated form ends with END and no terminator), the required separating semicolon
        // is emitted so the generated script is valid for SQL Server, which enforces the terminator
        // even though ScriptDom's own parser is lenient.
        protected void GenerateSeparatingSemiColonWhenNecessary(TSqlStatement previous, TSqlStatement next)
        {
            if (previous != null &&
                next != null &&
                _generateSemiColon &&
                StatementRequiresPrecedingSemiColon(next) &&
                _writer.LastMeaningfulTokenIsSemicolon() == false)
            {
                GenerateSymbol(TSqlTokenType.Semicolon);
            }
        }

        private static Boolean StatementRequiresPrecedingSemiColon(TSqlStatement statement)
        {
            if (statement is ThrowStatement)
            {
                return true;
            }

            StatementWithCtesAndXmlNamespaces statementWithCtes = statement as StatementWithCtesAndXmlNamespaces;
            return statementWithCtes != null && statementWithCtes.WithCtesAndXmlNamespaces != null;
        }

        /// <summary>
        /// Generates a statement fragment with semicolon placed before any trailing comments.
        /// This prevents semicolons from being appended after single-line comments (-- style),
        /// which would make them part of the comment text.
        /// </summary>
        protected void GenerateStatementWithSemiColon(TSqlStatement statement)
        {
            if (statement == null)
            {
                return;
            }

            // Handle comments before
            HandleCommentsBeforeFragment(statement);

            // Suppress trailing comment emission during statement body generation
            // so that the semicolon can be placed before trailing comments.
            // Only suppress for fragments at the statement boundary (LastTokenIndex).
            bool previousSuppressState = _suppressTrailingComments;
            int previousSuppressIndex = _suppressTrailingCommentsAfterIndex;
            if (_options.PreserveComments && _generateSemiColon && !StatementsThatCannotHaveSemiColon.Contains(statement.GetType()))
            {
                _suppressTrailingComments = true;
                _suppressTrailingCommentsAfterIndex = statement.LastTokenIndex;
            }

            // Generate the statement body
            statement.Accept(this);

            // Restore suppression state and emit semicolon before trailing comments
            _suppressTrailingComments = previousSuppressState;
            _suppressTrailingCommentsAfterIndex = previousSuppressIndex;

            // Sweep any comments inside the statement's token range that no
            // inner-fragment scan emitted (e.g. comments between an absorbed
            // ';' separator and the statement's last token).
            EmitUnemittedCommentsThroughStatementEnd(statement);

            // Semicolon BEFORE trailing comments
            GenerateSemiColonWhenNecessary(statement);

            // Only same-line trailing comments belong after the semicolon; a
            // comment on a later line is a leading comment of the next statement.
            if (_options.PreserveComments && _currentTokenStream != null)
            {
                EmitSameLineTrailingComments(statement);
                UpdateLastProcessedIndex(statement);
            }
            else
            {
                HandleCommentsAfterFragment(statement);
            }
        }
		
        protected void GenerateCommaSeparatedWithClause<T>(IList<T> fragments, bool indent, bool includeParentheses) where T : TSqlFragment
        {
            if (fragments != null && fragments.Count > 0)
            {
                NewLine();
                if (indent)
                    Indent();
                GenerateKeywordAndSpace(TSqlTokenType.With);
                if (includeParentheses)
                    GenerateSymbol(TSqlTokenType.LeftParenthesis);
                GenerateCommaSeparatedList(fragments);
                if (includeParentheses)
                    GenerateSymbol(TSqlTokenType.RightParenthesis);

            }
        }
    }
}
