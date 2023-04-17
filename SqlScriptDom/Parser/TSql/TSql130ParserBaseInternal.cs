//------------------------------------------------------------------------------
// <copyright file="TSql130ParserBaseInternals.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using antlr;
using System.Diagnostics;
using System.Security;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{

    internal abstract class TSql130ParserBaseInternal : TSql120ParserBaseInternal
    {

        /// <summary>
        /// String used to encrypt secure information with Asterisks.
        /// </summary>
        internal const string EncryptionWithAsterisks = "'***'";

        #region Constructors

        // Not really needed, here only because ANTLR generates call to this one in derived classes
        protected TSql130ParserBaseInternal(TokenBuffer tokenBuf, int k)
            : base(tokenBuf, k)
        {
        }

        // Not really needed, here only because ANTLR generates call to this one in derived classes
        protected TSql130ParserBaseInternal(ParserSharedInputState state, int k)
            : base(state, k)
        {
        }

        // Not really needed, here only because ANTLR generates call to this one in derived classes
        protected TSql130ParserBaseInternal(TokenStream lexer, int k)
            : base(lexer, k)
        {
        }

        /// <summary>
        /// Real constructor (the one which is used)
        /// </summary>
        /// <param name="initialQuotedIdentifiersOn">if set to <c>true</c> initial quoted identifiers will be set to on.</param>
        public TSql130ParserBaseInternal(bool initialQuotedIdentifiersOn)
            : base(initialQuotedIdentifiersOn)
        {
        }

        #endregion

        protected static void VerifyAllowedIndexOption130(IndexAffectingStatement statement, IndexOption option)
        {
            VerifyAllowedIndexOption(statement, option, SqlVersionFlags.TSql130);

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
        /// Checks if table definition contains 'generated always' columns that match period definition,
        /// if period definition exists.
        /// </summary>
        /// <param name="definition">Table definition to check.</param>
        /// <param name="isInAlterStatement">True if the definition used in ALTER statement (False for CREATE statement).</param>
        protected static void CheckTemporalPeriodInTableDefinition(TableDefinition definition, bool isInAlterStatement)
        {
            // The column check should be executed for every create statement and for alter/add statements that contain both
            // column and period definitions.
            //
            bool createOrNonEmptyAlter = !isInAlterStatement || (definition.ColumnDefinitions != null && definition.ColumnDefinitions.Count > 0);

            if (definition.SystemTimePeriod != null && createOrNonEmptyAlter)
            {
                bool startTimeColumnIsCorrect = false;
                bool endTimeColumnIsCorrect = false;

                foreach (ColumnDefinition column in definition.ColumnDefinitions)
                {
                    if (definition.SystemTimePeriod.StartTimeColumn.Value == column.ColumnIdentifier.Value)
                    {
                        if (column.GeneratedAlways != GeneratedAlwaysType.RowStart)
                        {
                            ThrowParseErrorException("SQL46103", definition, TSqlParserResource.SQL46103Message);
                        }
                        else
                        {
                            startTimeColumnIsCorrect = true;
                        }
                    }
                    else
                    {
                        if (definition.SystemTimePeriod.EndTimeColumn.Value == column.ColumnIdentifier.Value)
                        {
                            if (column.GeneratedAlways != GeneratedAlwaysType.RowEnd)
                            {
                                ThrowParseErrorException("SQL46104", definition, TSqlParserResource.SQL46104Message);
                            }
                            else
                            {
                                endTimeColumnIsCorrect = true;
                            }
                        }
                        else
                        {
                            if (column.GeneratedAlways != null)
                            {
                                if (column.GeneratedAlways == GeneratedAlwaysType.RowStart)
                                {
                                    ThrowParseErrorException("SQL46103", definition, TSqlParserResource.SQL46103Message);
                                }
                                else if (column.GeneratedAlways == GeneratedAlwaysType.RowEnd)
                                {
                                    ThrowParseErrorException("SQL46104", definition, TSqlParserResource.SQL46104Message);
                                }
                            }
                        }
                    }
                }

                // In case that ALTER statement is executed for:
                // 1. adding system time period
                // 2. adding columns that are not included in the period
                // we will have here both startTimeColumnIsCorrect and endTimeColumnIsCorrect set to false
                // and the alter statement is still valid.
                if (!(isInAlterStatement && !startTimeColumnIsCorrect && !endTimeColumnIsCorrect))
                {
                    if (!startTimeColumnIsCorrect)
                    {
                        ThrowParseErrorException("SQL46105", definition, TSqlParserResource.SQL46105Message);
                    }

                    if (!endTimeColumnIsCorrect)
                    {
                        ThrowParseErrorException("SQL46106", definition, TSqlParserResource.SQL46106Message);
                    }
                }
            }

            // Do the check for other GeneratedAlways columns
            CheckTemporalGeneratedAlwaysColumns(definition, isInAlterStatement);
        }

        /// <summary>
        /// Checks if generated always columns that are generated always as user id/name start/end are unique within table definition
        /// and if period definition exists when trying to create table with those columns
        /// </summary>
        /// <param name="definition">Table definition to check.</param>
        /// <param name="isInAlterStatement">True if the definition used in ALTER statement (False for CREATE statement).</param>
        protected static void CheckTemporalGeneratedAlwaysColumns(TableDefinition definition, bool isInAlterStatement)
        {
            bool userIdStartExists = false;
            bool userIdEndExists = false;
            bool userNameStartExists = false;
            bool userNameEndExists = false;
            bool startTimeExists = false;
            bool endTimeExists = false;

            int genAlwaysColumnCount = 0;

            foreach (ColumnDefinition column in definition.ColumnDefinitions)
            {
                if (column.GeneratedAlways == GeneratedAlwaysType.RowStart)
                {
                    genAlwaysColumnCount++;
                    if (startTimeExists)
                    {
                        ThrowParseErrorException("SQL46109", definition, TSqlParserResource.SQL46109Message);
                    }
                    else
                    {
                        startTimeExists = true;
                    }
                }
                else if (column.GeneratedAlways == GeneratedAlwaysType.RowEnd)
                {
                    genAlwaysColumnCount++;
                    if (endTimeExists)
                    {
                        ThrowParseErrorException("SQL46109", definition, TSqlParserResource.SQL46109Message);
                    }
                    else
                    {
                        endTimeExists = true;
                    }
                }
                else if (column.GeneratedAlways == GeneratedAlwaysType.UserIdStart)
                {
                    genAlwaysColumnCount++;
                    if (userIdStartExists)
                    {
                        ThrowParseErrorException("SQL46109", definition, TSqlParserResource.SQL46109Message);
                    }
                    else
                    {
                        userIdStartExists = true;
                    }
                }
                else if (column.GeneratedAlways == GeneratedAlwaysType.UserIdEnd)
                {
                    genAlwaysColumnCount++;
                    if (userIdEndExists)
                    {
                        ThrowParseErrorException("SQL46110", definition, TSqlParserResource.SQL46110Message);
                    }
                    else
                    {
                        userIdEndExists = true;
                    }
                }
                else if (column.GeneratedAlways == GeneratedAlwaysType.UserNameStart)
                {
                    genAlwaysColumnCount++;
                    if (userNameStartExists)
                    {
                        ThrowParseErrorException("SQL46111", definition, TSqlParserResource.SQL46111Message);
                    }
                    else
                    {
                        userNameStartExists = true;
                    }
                }
                else if (column.GeneratedAlways == GeneratedAlwaysType.UserNameEnd)
                {
                    genAlwaysColumnCount++;
                    if (userNameEndExists)
                    {
                        ThrowParseErrorException("SQL46112", definition, TSqlParserResource.SQL46112Message);
                    }
                    else
                    {
                        userNameEndExists = true;
                    }
                }
            }

            // In case of CREATE statement SystemTimePeriod needs to be defined
            // (table needs to be temporal in order to have GeneratedAlways as User Id/Name columns
            if (genAlwaysColumnCount > 0 && !isInAlterStatement && definition.SystemTimePeriod == null)
            {
                ThrowParseErrorException("SQL46113", definition, TSqlParserResource.SQL46113Message);
            }
        }

        /// <summary>
        /// Checks the retention period duration and throws an error if it is not positive number.
        /// </summary>
        /// <param name="duration">The retention period duration.</param>
        protected static void CheckRetentionPeriodDuration(ScalarExpression duration)
        {
            int retentionPeriodDuration;
            if (duration is UnaryExpression)
            {
                retentionPeriodDuration = -Int32.Parse(((duration as UnaryExpression).Expression as IntegerLiteral).Value, CultureInfo.InvariantCulture);
            }
            else
            {
                retentionPeriodDuration = Int32.Parse((duration as IntegerLiteral).Value, CultureInfo.InvariantCulture);
            }

            if (retentionPeriodDuration <= 0)
            {
                ThrowParseErrorException("SQL46116", duration, TSqlParserResource.SQL46116Message, retentionPeriodDuration.ToString(CultureInfo.CurrentCulture));
            }
        }

		/// <summary>
		/// This method checks a create table statement for being a hekaton table with an inline filtered index.
		/// This is currently not exposed and not allowed.
		/// </summary>
		/// <param name="statement">The create table statement.</param>
		protected static void CheckHekatonTableForInlineFilteredIndexes(CreateTableStatement statement)
		{
			if(!IsMemoryOptimized(statement))
			{
				return;
			}

			foreach(ColumnDefinition column in statement.Definition.ColumnDefinitions)
			{
				if(column.Index != null)
				{
					if(column.Index.FilterPredicate != null)
					{
						ThrowParseErrorException("SQL46107", statement.Definition, TSqlParserResource.SQL46107Message);
					}
				}
			}

			foreach(IndexDefinition index in statement.Definition.Indexes)
			{
				if(index.FilterPredicate != null)
				{
					ThrowParseErrorException("SQL46107", statement.Definition, TSqlParserResource.SQL46107Message);
				}
			}
		}

        /// <summary>
        /// This method checks hekaton tables for nonclustered column store indexes, which are not allowed.
        /// </summary>
        /// <param name="statement">The create table statement.</param>
        protected static void CheckHekatonTableForNonClusteredColumnStoreIndexes(CreateTableStatement statement)
		{
			if(!IsMemoryOptimized(statement))
			{
				return;
			}

			foreach(ColumnDefinition column in statement.Definition.ColumnDefinitions)
			{
				if(column.Index != null)
				{
					if(column.Index.IndexType.IndexTypeKind == IndexTypeKind.NonClusteredColumnStore)
					{
						ThrowParseErrorException("SQL46108", statement.Definition, TSqlParserResource.SQL46108Message);
					}
				}
			}

			foreach (IndexDefinition index in statement.Definition.Indexes)
			{
				if (index.IndexType.IndexTypeKind == IndexTypeKind.NonClusteredColumnStore)
				{
					ThrowParseErrorException("SQL46108", statement.Definition, TSqlParserResource.SQL46108Message);
				}
			}
		}

		/// <summary>
		/// Determines if the create talbe statement is memory optimized.
		/// </summary>
		/// <param name="statement">The create table statement.</param>
		private static bool IsMemoryOptimized(CreateTableStatement statement)
		{
			foreach (TableOption option in statement.Options)
			{
				if (option.OptionKind == TableOptionKind.MemoryOptimized)
				{
					return true;
				}
			}

			return false;
		}

		protected static void ThrowIfCompressionDelayValueOutOfRange(Literal value)
		{
			Int32 outValue;
			if (!Int32.TryParse(value.Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out outValue) || (outValue > 10080) || (outValue < 0))
			{
				ThrowParseErrorException("SQL46114", value, TSqlParserResource.SQL46114Message, value.Value);
			}
		}

        /// <summary>
        /// This method checks Ctas statements has distribution option, which is required.
        /// If no distribution option mentioned, this method throws parser error saying "The CTAS statement requires a distribution option"
        /// </summary>
        /// <param name="statement">The create table statement.</param>
        protected static void CheckCtasStatementHasDistributionOption(CreateTableStatement statement)
        {
            if (statement.SelectStatement != null)
            {
                int count = statement.Options.Where(o => o is TableDistributionOption).Count();
                if (count == 0)
                {
                    ThrowParseErrorException("SQL46127", statement, TSqlParserResource.SQL46127Message);
                }
            }
        }

        /// <summary>
        /// This method checks External Table Ctas statements has not Rejected Row Location option.
        /// If Rejected Row Location option mentioned, this method throws parser error saying "REJECTED_ROW_LOCATION cannot be specified in a CREATE EXTERNAL TABLE AS SELECT statement."
        /// </summary>
        /// <param name="statement">The create table statement.</param>
        protected static void CheckExternalTableCtasStatementHasNotRejectedRowLocationOption(CreateExternalTableStatement statement)
        {
            if (statement.SelectStatement != null)
            {
                if (statement.ExternalTableOptions != null && statement.ExternalTableOptions.Any(o => o.OptionKind == ExternalTableOptionKind.RejectedRowLocation)
                )
                {
                    ThrowParseErrorException("SQL46128", statement, TSqlParserResource.SQL46128Message);
                }
            }
        }

        new protected static FunctionOptionKind ParseAlterCreateFunctionWithOption(antlr.IToken token)
        {
            switch (token.getText().ToUpperInvariant())
            {
                case CodeGenerationSupporter.Encryption:
                    return FunctionOptionKind.Encryption;
                case CodeGenerationSupporter.SchemaBinding:
                    return FunctionOptionKind.SchemaBinding;
                case CodeGenerationSupporter.NativeCompilation:
                    return FunctionOptionKind.NativeCompilation;
                default:
                    throw new TSqlParseErrorException(
                        CreateParseError("SQL46026", token, TSqlParserResource.SQL46026Message, token.getText()));
            }
        }

        protected bool NextIdentifierMatchesOneOf(params string[] keywords)
        {
            if (LA(1) == TSql80ParserInternal.Identifier)
            {
                string text = LT(1).getText();
                foreach (string keyword in keywords)
                {
                    if (String.Equals(keyword, text, StringComparison.OrdinalIgnoreCase))
                        return true;
                }
            }
            else if (LA(1) == TSql80ParserInternal.QuotedIdentifier)
            {
                QuoteType quote;
                string text = Identifier.DecodeIdentifier(LT(1).getText(), out quote);
                foreach (string keyword in keywords)
                {
                    if (String.Equals(keyword, text, StringComparison.OrdinalIgnoreCase))
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks whether the column limit is reached and if not it increments the column count.
        /// </summary>
        /// <param name="columnCount">Current column count</param>
        /// <param name="token">Token to be passed in case of error.</param>
        protected static void CheckAndIncrementColumnCount(ref Int32 columnCount, antlr.IToken token)
        {
            if (columnCount == 1024)
            {
                ThrowParseErrorException("TSP0017", token, TSqlParserResource.SQL46016Message);
            }
            else
            {
                columnCount++;
            }
        }

        /// <summary>
        /// Checks, if option was already specified, and if it is the case, throws
        /// </summary>
        /// <param name="encountered">The encountered.</param>
        /// <param name="newOption">The new option to be added.</param>
        /// <param name="token">The token.</param>
        protected static void CheckCopyOptionDuplication(ref Int32 encountered,
                CopyOptionKind newOption, antlr.IToken token)
        {
            Int32 newOptionBit = (1 << ((Int32)newOption));
            if ((encountered & newOptionBit) == newOptionBit)
                throw GetUnexpectedTokenErrorException(token);
            encountered |= newOptionBit;
        }


        /// <summary>
        /// Checks, if TimeLiteral is valid and if not, throws
        /// </summary>
        /// <param name="timeStringToken">Time as string literal</param>
        protected static void CheckValidWlmTimeLiteral(StringLiteral timeStringToken)
        {
            TimeSpan classifierTimeSpan;
            if (!TimeSpan.TryParseExact(timeStringToken.Value, @"h\:mm", CultureInfo.CurrentCulture, out classifierTimeSpan))
            {
                ThrowParseErrorException("SQL46134", timeStringToken, TSqlParserResource.SQL46134Message);
            }
        }
    }
}
