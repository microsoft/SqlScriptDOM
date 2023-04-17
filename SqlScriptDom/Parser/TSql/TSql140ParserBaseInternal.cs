//------------------------------------------------------------------------------
// <copyright file="TSql140ParserBaseInternals.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;
using antlr;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    internal abstract class TSql140ParserBaseInternal : TSql130ParserBaseInternal
    {
        #region Constructors

        // Not really needed, here only because ANTLR generates call to this one in derived classes
        protected TSql140ParserBaseInternal(TokenBuffer tokenBuf, int k)
            : base(tokenBuf, k)
        {
        }

        // Not really needed, here only because ANTLR generates call to this one in derived classes
        protected TSql140ParserBaseInternal(ParserSharedInputState state, int k)
            : base(state, k)
        {
        }

        // Not really needed, here only because ANTLR generates call to this one in derived classes
        protected TSql140ParserBaseInternal(TokenStream lexer, int k)
            : base(lexer, k)
        {
        }

        /// <summary>
        /// Real constructor (the one which is used)
        /// </summary>
        /// <param name="initialQuotedIdentifiersOn">if set to <c>true</c> initial quoted identifiers will be set to on.</param>
        public TSql140ParserBaseInternal(bool initialQuotedIdentifiersOn)
            : base(initialQuotedIdentifiersOn)
        {
        }

        #endregion

        #region Context Sensitive Keyword Matchers

        /// <summary>
        /// Tries to match string literal with predefined string value.
        /// </summary>
        /// <param name="literal">The string litral.</param>
        /// <param name="keyword">Predefined string to match.</param>
        /// <returns>True if there was a match.</returns>
        protected static bool TryMatch(Literal literal, string keyword)
        {
            Debug.Assert(literal.LiteralType == LiteralType.String);
            return String.Equals(literal.Value, keyword, StringComparison.OrdinalIgnoreCase);
        }

        #endregion

        protected static void VerifyAllowedIndexOption140(IndexAffectingStatement statement, IndexOption option)
        {
            VerifyAllowedIndexOption(statement, option, SqlVersionFlags.TSql140);

            // for a low priority lock wait (MLP) option, check if it is allowed for the statement.
            //
            if (option is OnlineIndexOption)
            {
                OnlineIndexOption onlineIndexOption = option as OnlineIndexOption;
                if (onlineIndexOption.LowPriorityLockWaitOption != null)
                {
                    switch (statement)
                    {
                        case IndexAffectingStatement.AlterIndexRebuildOnePartition:
                        case IndexAffectingStatement.AlterTableRebuildOnePartition:
                        case IndexAffectingStatement.AlterIndexRebuildAllPartitions:
                        case IndexAffectingStatement.AlterTableRebuildAllPartitions:
                            // allowed
                            //
                            break;

                        default:
                            // WAIT_AT_LOW_PRIORITY is not a valid index option in the statement
                            //
                            ThrowWrongIndexOptionError(statement, onlineIndexOption.LowPriorityLockWaitOption);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Checks that supported data file type, i.e. char or widechar, is specified with data file format CSV.
        /// If data file type is not supported, parser error exception is thrown.
        /// </summary>
        /// <param name="encounteredOptions">Mask with bulk insert options that are specified.</param>
        /// <param name="statement">Bulk insert statement.</param>
        /// <exception cref="TSqlParseErrorException">Exception thrown if prohibited options are sprecified.</exception>
        protected static void CheckForDataFileFormatProhibitedOptionsBulkInsert(long encounteredOptions, BulkInsertStatement statement)
        {
            const long DataFileFormatProhibitedOptionsBulkInsertMask =
                (1L << (int)BulkInsertOptionKind.DataFileFormat) |
                (1L << (int)BulkInsertOptionKind.DataFileType);

            // Check if both data file type and data file format options are specified
            //
            if ((encounteredOptions & DataFileFormatProhibitedOptionsBulkInsertMask) == DataFileFormatProhibitedOptionsBulkInsertMask)
            {
                bool isDataFileFormatCSV = false;
                bool isDataFileTypeProhibited = false;

                // Check if unsupported values are specified
                //
                foreach (BulkInsertOption option in statement.Options)
                {
                    if (option.OptionKind == BulkInsertOptionKind.DataFileFormat)
                    {
                        LiteralBulkInsertOption dataFileFormatOption = option as LiteralBulkInsertOption;

                        isDataFileFormatCSV = TryMatch(dataFileFormatOption.Value, CodeGenerationSupporter.Csv);
                    }
                    else if (option.OptionKind == BulkInsertOptionKind.DataFileType)
                    {
                        LiteralBulkInsertOption dataFileTypeOption = option as LiteralBulkInsertOption;

                        isDataFileTypeProhibited = !(TryMatch(dataFileTypeOption.Value, CodeGenerationSupporter.Char) || TryMatch(dataFileTypeOption.Value, CodeGenerationSupporter.WideChar));
                    }
                }

                // Throw parser error exception if invalid combination is specified
                //
                if (isDataFileFormatCSV && isDataFileTypeProhibited)
                {
                    ThrowParseErrorException("SQL46118", statement, TSqlParserResource.SQL46118Message);
                }
            }
        }

        /// <summary>
        /// Checks that file format CSV is not specified with unsupported openrowset bulk options: SINGLE_BLOB, SINGLE_CLOB and SINGLE_NCLOB.
        /// If data file type is not supported, parser error exception is thrown.
        /// </summary>
        /// <param name="encounteredOptions">Mask with openrowset bulk options that are specified.</param>
        /// <param name="relatedFragment">Fragment related to error.</param>
        /// <exception cref="TSqlParseErrorException">Exception thrown if prohibited options are sprecified.</exception>
        protected static void CheckForDataFileFormatProhibitedOptionsInOpenRowsetBulk(long encounteredOptions, TSqlFragment relatedFragment)
        {
            const long DataFileFormatOpenRowsetBulkMask =
                (1L << (int)BulkInsertOptionKind.DataFileFormat);

            const long CheckForDataFileFormatProhibitedOptionsInOpenRowsetBulkMask =
                (1L << (int)BulkInsertOptionKind.SingleBlob) |
                (1L << (int)BulkInsertOptionKind.SingleClob) |
                (1L << (int)BulkInsertOptionKind.SingleNClob);

            if (((encounteredOptions & DataFileFormatOpenRowsetBulkMask) != 0) && 
                ((encounteredOptions & CheckForDataFileFormatProhibitedOptionsInOpenRowsetBulkMask) != 0))
            {
                ThrowParseErrorException("SQL46119", relatedFragment, TSqlParserResource.SQL46119Message);
            }
        }

        /// <summary>
        /// Checks whether the FORMAT='PARQUET' option has been specified and verifies whether the provided options
        /// are compatible with the PARQUET format.
        /// </summary>
        /// <param name="encounteredOptions">Mask with openrowset bulk options that are specified.</param>
        /// <param name="bulkOpenRowset">OPENROWSET(BULK ...) syntax tree element constructed by the parser.</param>
        protected static void CheckForParquetFormatProhibitedOptionsInOpenRowsetBulk(long encounteredOptions, BulkOpenRowset bulkOpenRowset)
        {
            const long DataFileFormatOpenRowsetBulkMask =
                (1L << (int)BulkInsertOptionKind.DataFileFormat);

            const long InvalidParquetOptionsInOpenRowsetBulkMask =
                (1L << (int)BulkInsertOptionKind.ParserVersion) |
                (1L << (int)BulkInsertOptionKind.RowsetOptions);

            if (((encounteredOptions & DataFileFormatOpenRowsetBulkMask) != 0) &&
                ((encounteredOptions & InvalidParquetOptionsInOpenRowsetBulkMask) != 0))
            {
                foreach (BulkInsertOption option in bulkOpenRowset.Options)
                {
                    if (option.OptionKind == BulkInsertOptionKind.DataFileFormat)
                    {
                        LiteralBulkInsertOption dataFileFormatOption = option as LiteralBulkInsertOption;

                        if (TryMatch(dataFileFormatOption.Value, CodeGenerationSupporter.Parquet))
                        {
                            ThrowParseErrorException("SQL46142", bulkOpenRowset, TSqlParserResource.SQL46142Message);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Creates an identifier from the passed in token.
        /// </summary>
        protected Identifier CreateIdentifierFromToken(antlr.IToken token)
        {
            return new Identifier { Value = token.getText(), QuoteType = QuoteType.NotQuoted };
        }

        /// <summary>
        /// Creates an IdentifierOrScalarExpression fragment from the passed in identifier.
        /// </summary>
        protected IdentifierOrScalarExpression CreateIdentifierOrScalarExpressionFromIdentifier(Identifier identifier)
        {
            IdentifierOrScalarExpression valExp = FragmentFactory.CreateFragment<IdentifierOrScalarExpression>();
            valExp.Identifier = identifier;
            return valExp;
        }
    }
}
