//------------------------------------------------------------------------------
// <copyright file="TSql160ParserBaseInternals.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using antlr;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    internal abstract class TSql160ParserBaseInternal : TSql150ParserBaseInternal
    {
        #region Constructors

        // Not really needed, here only because ANTLR generates call to this one in derived classes
        protected TSql160ParserBaseInternal(TokenBuffer tokenBuf, int k)
            : base(tokenBuf, k)
        {
        }

        // Not really needed, here only because ANTLR generates call to this one in derived classes
        protected TSql160ParserBaseInternal(ParserSharedInputState state, int k)
            : base(state, k)
        {
        }

        // Not really needed, here only because ANTLR generates call to this one in derived classes
        protected TSql160ParserBaseInternal(TokenStream lexer, int k)
            : base(lexer, k)
        {
        }

        /// <summary>
        /// Real constructor (the one which is used)
        /// </summary>
        /// <param name="initialQuotedIdentifiersOn">if set to <c>true</c> initial quoted identifiers will be set to on.</param>
        public TSql160ParserBaseInternal(bool initialQuotedIdentifiersOn)
            : base(initialQuotedIdentifiersOn)
        {
        }

        #endregion

        /// <summary>
        /// Verifies the option is valid for SQL 160 platform
        /// </summary>
        protected static void VerifyAllowedIndexOption160(IndexAffectingStatement statement, IndexOption option)
        {
            VerifyAllowedIndexOption(statement, option, SqlVersionFlags.TSql160);
            VerifyAllowedOnlineIndexOptionLowPriorityLockWait(statement, option);
        }

        /// <summary>
        /// Checks if table definition contains 'generated always' columns that match period definition,
        /// if period definition exists.
        /// </summary>
        /// <param name="definition">Table definition to check.</param>
        /// <param name="isInAlterStatement">True if the definition used in ALTER statement (False for CREATE statement).</param>
        /// <param name="isLedgerSupported">Ledger is only supported for version Sql160 or above (True for Sql160 version)</param>
        protected static void CheckTemporalPeriodInTableDefinition(TableDefinition definition, bool isInAlterStatement, bool isLedgerSupported)
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
            CheckTemporalGeneratedAlwaysColumns(definition, isInAlterStatement, isLedgerSupported);
        }

        /// <summary>
        /// Checks if generated always columns that are generated always as user id/name start/end are unique within table definition
        /// and if period definition exists when trying to create table with those columns
        /// </summary>
        /// <param name="definition">Table definition to check.</param>
        /// <param name="isInAlterStatement">True if the definition used in ALTER statement (False for CREATE statement).</param>
        /// <param name="isLedgerSupported">Ledger is only supported for version Sql160 or above (True for Sql160 version)</param>
        protected static void CheckTemporalGeneratedAlwaysColumns(TableDefinition definition, bool isInAlterStatement, bool isLedgerSupported)
        {
            bool userIdStartExists = false;
            bool userIdEndExists = false;
            bool userNameStartExists = false;
            bool userNameEndExists = false;
            bool startTimeExists = false;
            bool endTimeExists = false;
            bool transactionIdStartExists = false;
            bool transactionIdEndExists = false;
            bool sequenceNumberStartExists = false;
            bool sequenceNumberEndExists = false;

            int temporalGenAlwaysColumnCount = 0;

            foreach (ColumnDefinition column in definition.ColumnDefinitions)
            {
                if (column.GeneratedAlways == GeneratedAlwaysType.RowStart)
                {
                    temporalGenAlwaysColumnCount++;
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
                    temporalGenAlwaysColumnCount++;
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
                    temporalGenAlwaysColumnCount++;
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
                    temporalGenAlwaysColumnCount++;
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
                    temporalGenAlwaysColumnCount++;
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
                    temporalGenAlwaysColumnCount++;
                    if (userNameEndExists)
                    {
                        ThrowParseErrorException("SQL46112", definition, TSqlParserResource.SQL46112Message);
                    }
                    else
                    {
                        userNameEndExists = true;
                    }
                }
                else if (column.GeneratedAlways == GeneratedAlwaysType.TransactionIdStart && isLedgerSupported)
                {
                    if (transactionIdStartExists)
                    {
                        ThrowParseErrorException("SQL46136", definition, TSqlParserResource.SQL46136Message);
                    }
                    else
                    {
                        transactionIdStartExists = true;
                    }
                }
                else if (column.GeneratedAlways == GeneratedAlwaysType.TransactionIdEnd && isLedgerSupported)
                {
                    if (transactionIdEndExists)
                    {
                        ThrowParseErrorException("SQL46137", definition, TSqlParserResource.SQL46137Message);
                    }
                    else
                    {
                        transactionIdEndExists = true;
                    }
                }
                else if (column.GeneratedAlways == GeneratedAlwaysType.SequenceNumberStart && isLedgerSupported)
                {
                    if (sequenceNumberStartExists)
                    {
                        ThrowParseErrorException("SQL46138", definition, TSqlParserResource.SQL46138Message);
                    }
                    else
                    {
                        sequenceNumberStartExists = true;
                    }
                }
                else if (column.GeneratedAlways == GeneratedAlwaysType.SequenceNumberEnd && isLedgerSupported)
                {
                    if (sequenceNumberEndExists)
                    {
                        ThrowParseErrorException("SQL46139", definition, TSqlParserResource.SQL46139Message);
                    }
                    else
                    {
                        sequenceNumberEndExists = true;
                    }
                }
                // Ledger is only supported for version Sql160 - throwing an error if the version is not 160 or above.
                //
                else if (!isLedgerSupported && (column.GeneratedAlways == GeneratedAlwaysType.SequenceNumberStart || column.GeneratedAlways == GeneratedAlwaysType.SequenceNumberEnd
                    || column.GeneratedAlways == GeneratedAlwaysType.TransactionIdStart || column.GeneratedAlways == GeneratedAlwaysType.TransactionIdEnd))
                {
                    ThrowParseErrorException("SQL46141", definition, TSqlParserResource.SQL46141Message);
                }
            }

            // In case of CREATE statement SystemTimePeriod needs to be defined
            // (table needs to be temporal in order to have GeneratedAlways as User Id/Name columns
            if (temporalGenAlwaysColumnCount > 0 && !isInAlterStatement && definition.SystemTimePeriod == null)
            {
                ThrowParseErrorException("SQL46113", definition, TSqlParserResource.SQL46113Message);
            }
        }

        /// <summary>
        /// Checks if OPENROWSET (PROVIDER = 'CosmosDB' ...) contains all the expected parameters.
        /// Four parameters are available:
        /// 1. PROVIDER                          (required)
        /// 2. CONNECTION                        (required)
        /// 3. OBJECT                            (required)
        /// 4. CREDENTIAL | SERVER_CREDENTIAL    (optional)
        /// 
        /// This syntax is specific to Serverless SQL pools.
        /// </summary>
        /// <param name="openRowsetCosmos">OPENROWSET(PROVIDER = 'CosmosDB' ...) syntax tree element constructed by the parser.</param>
        protected static void CheckForConflictingOptionsInOpenRowsetBulkCosmos(OpenRowsetCosmos openRowsetCosmos)
        {
            // Required parameter 'Provider' not provided
            //
            if (!openRowsetCosmos.Options.Where(opt => opt.OptionKind == OpenRowsetCosmosOptionKind.Provider).Any())
            {
                ThrowParseErrorException("SQL46144", openRowsetCosmos, TSqlParserResource.SQL46144Message, nameof(OpenRowsetCosmosOptionKind.Provider));
            }

            // Required parameter 'Connection' not provided
            //
            if (!openRowsetCosmos.Options.Where(opt => opt.OptionKind == OpenRowsetCosmosOptionKind.Connection).Any())
            {
                ThrowParseErrorException("SQL46144", openRowsetCosmos, TSqlParserResource.SQL46144Message, nameof(OpenRowsetCosmosOptionKind.Connection));
            }

            // Required parameter 'Object' not provided
            //
            if (!openRowsetCosmos.Options.Where(opt => opt.OptionKind == OpenRowsetCosmosOptionKind.Object).Any())
            {
                ThrowParseErrorException("SQL46144", openRowsetCosmos, TSqlParserResource.SQL46144Message, nameof(OpenRowsetCosmosOptionKind.Object));
            }

            // Both optional parameters 'Credential' and 'Server_Credential' are provided. Only one (or none) are expected
            //
            if (openRowsetCosmos.Options.Where(opt => opt.OptionKind == OpenRowsetCosmosOptionKind.Credential).Any() && openRowsetCosmos.Options.Where(opt => opt.OptionKind == OpenRowsetCosmosOptionKind.Server_Credential).Any())
            {
                ThrowParseErrorException("SQL46143", openRowsetCosmos, TSqlParserResource.SQL46143Message);
            }
        }
    }
}
