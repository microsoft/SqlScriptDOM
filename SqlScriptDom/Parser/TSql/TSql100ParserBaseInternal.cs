//------------------------------------------------------------------------------
// <copyright file="TSql100ParserBaseInternals.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using antlr;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{

    internal abstract class TSql100ParserBaseInternal : TSql90ParserBaseInternal
    {
        #region Constructors

        // Not really needed, here only because ANTLR generates call to this one in derived classes
        protected TSql100ParserBaseInternal(TokenBuffer tokenBuf, int k)
            : base(tokenBuf, k)
        {
        }

        // Not really needed, here only because ANTLR generates call to this one in derived classes
        protected TSql100ParserBaseInternal(ParserSharedInputState state, int k)
            : base(state, k)
        {
        }

        // Not really needed, here only because ANTLR generates call to this one in derived classes
        protected TSql100ParserBaseInternal(TokenStream lexer, int k)
            : base(lexer, k)
        {
        }

        /// <summary>
        /// Real constructor (the one which is used)
        /// </summary>
        /// <param name="initialQuotedIdentifiersOn">if set to <c>true</c> initial quoted identifiers will be on.</param>
        public TSql100ParserBaseInternal(bool initialQuotedIdentifiersOn)
            : base(initialQuotedIdentifiersOn)
        {
        }

        #endregion

        protected AutoCleanupChangeTrackingOptionDetail CreateAutoCleanupDetail(IToken firstToken,
            IToken lastToken, ref bool autoCleanupEncountered)
        {
            Match(firstToken, CodeGenerationSupporter.AutoCleanup);
            if (autoCleanupEncountered)
            {
                ThrowParseErrorException("SQL46050", firstToken,
                    TSqlParserResource.SQL46050Message, firstToken.getText());
            }
            autoCleanupEncountered = true;
            AutoCleanupChangeTrackingOptionDetail autoCleanup = FragmentFactory.CreateFragment<AutoCleanupChangeTrackingOptionDetail>();
            UpdateTokenInfo(autoCleanup, firstToken);
            UpdateTokenInfo(autoCleanup, lastToken);
            autoCleanup.IsOn = (lastToken.Type == TSql80ParserInternal.On);
            return autoCleanup;
        }

        protected static SqlDataTypeOption ParseDataType100(string token)
        {
            switch (token.ToUpperInvariant())
            {
                case "DATE":
                    return SqlDataTypeOption.Date;
                case "TIME":
                    return SqlDataTypeOption.Time;
                case "DATETIME2":
                    return SqlDataTypeOption.DateTime2;
                case "DATETIMEOFFSET":
                    return SqlDataTypeOption.DateTimeOffset;
                default:
                    return ParseDataType(token);
            }
        }

        /// <summary>
        /// Checks broker priority parameter duplication
        /// </summary>
        /// <param name="current">The enum that the newOption will be added.</param>
        /// <param name="newOption">The new option to be added.</param>
        /// <param name="token">The token that was parsed for the new option.</param>
        /// <returns>The aggregated value for options.</returns>
        protected static void CheckBrokerPriorityParameterDuplication(int current, BrokerPriorityParameterType newOption, antlr.IToken token)
        {
            if ((current & (1 << (int)(newOption))) != 0)
                ThrowIncorrectSyntaxErrorException(token);
        }

        // Updates the running variable that keeps track of the options present in the input
        protected static void UpdateBrokerPriorityEncounteredOptions(ref int encountered, BrokerPriorityParameter vBrokerPriorityParameter)
        {
            encountered = encountered | (1 << (int)(vBrokerPriorityParameter.ParameterType));
        }

        /// <summary>
        /// Checks spatial index bounding box parameter duplication
        /// </summary>
        /// <param name="current">The enum that the newOption will be added.</param>
        /// <param name="newOption">The new option to be added.</param>
        /// <param name="token">The token that was parsed for the new option.</param>
        /// <returns>The aggregated value for options.</returns>
        protected static void CheckBoundingBoxParameterDuplication(int current, BoundingBoxParameterType newOption, antlr.IToken token)
        {
            if ((current & (1 << (int)(newOption))) != 0)
                ThrowIncorrectSyntaxErrorException(token);
        }

        // Updates the running variable that keeps track of the options present in the input
        protected static void UpdateBoundingBoxParameterEncounteredOptions(ref int encountered, BoundingBoxParameter vBoundingBoxParameter)
        {
            encountered = encountered | (1 << (int)(vBoundingBoxParameter.Parameter));
        }

        protected static void CheckIfValidSpatialIndexOptionValue(IndexAffectingStatement statement, IndexOption option)
        {
            IndexStateOption indexStateOption = option as IndexStateOption;
            if (indexStateOption != null)
            {
                if (indexStateOption.OptionKind == IndexOptionKind.IgnoreDupKey)
                {
                    if (indexStateOption.OptionState == OptionState.On)
                        ThrowWrongIndexOptionError(statement, indexStateOption);
                }
            }
        }

        protected static void SetFileStreamStorageOption(ColumnStorageOptions storageOptions, IToken fileStreamToken, DataTypeReference columnType, IndexAffectingStatement statementType)
        {
            if (statementType == IndexAffectingStatement.AlterTableAddElement ||
                statementType == IndexAffectingStatement.CreateTable)
            {
                // Filestream is only allowed on VARBINARY(MAX) columns
                SqlDataTypeReference sqlDataType = columnType as SqlDataTypeReference;
                if (sqlDataType != null &&
                    sqlDataType.SqlDataTypeOption == SqlDataTypeOption.VarBinary &&
                    sqlDataType.Parameters.Count == 1 &&
                    sqlDataType.Parameters[0].LiteralType == LiteralType.Max)
                {
                    storageOptions.IsFileStream = true;
                }
                else
                    ThrowParseErrorException("SQL46051", fileStreamToken, TSqlParserResource.SQL46051Message);
            }
            else
                ThrowIncorrectSyntaxErrorException(fileStreamToken);
        }

        protected static void SetSparseStorageOption(ColumnStorageOptions columnStorage, SparseColumnOption option, IToken token, IndexAffectingStatement statementType)
        {
            switch (statementType)
            {
                case IndexAffectingStatement.AlterTableAddElement:
                case IndexAffectingStatement.CreateTable:
                case IndexAffectingStatement.DeclareTableVariable:
                case IndexAffectingStatement.CreateOrAlterFunction:
                    {
                        // Disallow create/alter function with COLUMN_SET FOR ALL_SPARSE_COLUMNS syntax to match engine behavior
                        if (statementType == IndexAffectingStatement.CreateOrAlterFunction && option == SparseColumnOption.ColumnSetForAllSparseColumns)
                            ThrowIncorrectSyntaxErrorException(token);

                        columnStorage.SparseOption = option;
                        break;
                    }
                default:
                    ThrowIncorrectSyntaxErrorException(token);
                    break;
            }
        }

        protected static void CheckComparisonOperandForIndexFilter(ScalarExpression rightOperand, bool convertAllowed)
        {
            UnaryExpression unary = rightOperand as UnaryExpression;
            if (unary != null)
            {
                CheckComparisonOperandForIndexFilter(unary.Expression, convertAllowed);
                return;
            }

            Literal literal = rightOperand as Literal;
            if (literal != null &&
                literal.LiteralType != LiteralType.Max)
                return;

            ParenthesisExpression parenthesis = rightOperand as ParenthesisExpression;
            if (parenthesis != null)
            {
                CheckComparisonOperandForIndexFilter(parenthesis.Expression, convertAllowed);
                return;
            }

            if (convertAllowed)
            {
                ConvertCall convertCall = rightOperand as ConvertCall;
                if (convertCall != null)
                {
                    CheckComparisonOperandForIndexFilter(convertCall.Parameter, false);
                    return;
                }

                CastCall castCall = rightOperand as CastCall;
                if (castCall != null)
                {
                    CheckComparisonOperandForIndexFilter(castCall.Parameter, false);
                    return;
                }
            }

            ThrowParseErrorException("SQL46059", rightOperand, TSqlParserResource.SQL46059Message);
        }

        // The PARTITION=ALL or PARTITION=[partition_number] option must be used if any partitions are specified in the DATA_COMPRESSION or XML_COMPRESSION clause.
        // This makes it explicit that selected partitions of the table/index will be rebuilt if they are specified in the DATA_COMPRESSION or XML_COMPRESSION clause.
        protected static void CheckPartitionAllSpecifiedForIndexRebuild(PartitionSpecifier partitionSpecifier, IList<IndexOption> indexOptions)
        {
            if (partitionSpecifier == null)
            {
                foreach (IndexOption option in indexOptions)
                {
                    if (option as DataCompressionOption != null && (option as DataCompressionOption).PartitionRanges.Count > 0)
                    {
                        ThrowParseErrorException("SQL46061",
                            option, TSqlParserResource.SQL46061Message);
                    }
                    else if (option as XmlCompressionOption != null && (option as XmlCompressionOption).PartitionRanges.Count > 0)
                    {
                        ThrowParseErrorException("SQL46061",
                            option, TSqlParserResource.SQL46061Message);
                    }
                }
            }
        }

        protected static void ThrowIfWrongGuidFormat(Literal literal)
        {
            string guidRegex = "[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}";
            if (!Regex.IsMatch(literal.Value, guidRegex, RegexOptions.CultureInvariant))
            {
                ThrowParseErrorException("SQL46055",
                    literal, TSqlParserResource.SQL46055Message);
            }
        }

        protected static void ThrowIfTooLargeAuditFileSize(Literal size, int shift)
        {
            UInt64 convertedSize;
            if (!UInt64.TryParse(size.Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out convertedSize) ||
                convertedSize > (UInt64.MaxValue >> (20 + shift)))
            {
                ThrowParseErrorException("SQL46054", size, TSqlParserResource.SQL46054Message);
            }
        }

        protected static void CheckForCellsPerObjectValueRange(Literal value)
        {
            int convertedValue;
            if (!Int32.TryParse(value.Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out convertedValue) ||
                convertedValue < 1 || convertedValue > 8192)
            {
                ThrowParseErrorException("SQL46073", value, TSqlParserResource.SQL46073Message, value.Value);
            }
        }
    }
}
