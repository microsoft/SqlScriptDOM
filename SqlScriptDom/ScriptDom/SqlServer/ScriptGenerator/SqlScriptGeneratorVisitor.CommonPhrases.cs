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
                GenerateSpaceAndFragmentIfNotNull(collation);
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

        // mark an alignment point for clause body if it's configured so at a new line
        protected void MarkClauseBodyAlignmentWhenNecessary(Boolean newline, AlignmentPoint ap)
        {
            // If we didn't put a newline in, don't align, even if AlignClauseBodies is on
            if (newline && _options.AlignClauseBodies)
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

        private void GenerateSelectElementsList(IList<SelectElement> selectElements)
        {
            if (_options.MultilineSelectElementsList == false)
            {
                GenerateCommaSeparatedList(selectElements);
            }
            else
            {
                GenerateFragmentList(selectElements, ListGenerationOption.MultipleLineSelectElementOption);
            }
        }

        protected void GenerateParameters(ParameterizedDataTypeReference node)
        {
            if (node.Parameters.Count > 0)
            {
                GenerateParenthesisedCommaSeparatedList(node.Parameters);
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
