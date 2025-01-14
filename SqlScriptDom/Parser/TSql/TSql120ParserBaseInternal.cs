//------------------------------------------------------------------------------
// <copyright file="TSql120ParserBaseInternals.cs" company="Microsoft">
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

    internal abstract class TSql120ParserBaseInternal : TSql110ParserBaseInternal
    {
        #region Constructors

        // Not really needed, here only because ANTLR generates call to this one in derived classes
        protected TSql120ParserBaseInternal(TokenBuffer tokenBuf, int k)
            : base(tokenBuf, k)
        {
        }

        // Not really needed, here only because ANTLR generates call to this one in derived classes
        protected TSql120ParserBaseInternal(ParserSharedInputState state, int k)
            : base(state, k)
        {
        }

        // Not really needed, here only because ANTLR generates call to this one in derived classes
        protected TSql120ParserBaseInternal(TokenStream lexer, int k)
            : base(lexer, k)
        {
        }

        /// <summary>
        /// Real constructor (the one which is used)
        /// </summary>
        /// <param name="initialQuotedIdentifiersOn">if set to <c>true</c> initial quoted identifiers will be set to on.</param>
        public TSql120ParserBaseInternal(bool initialQuotedIdentifiersOn)
            : base(initialQuotedIdentifiersOn)
        {
        }

        #endregion

        protected static void CheckLowPriorityLockWaitValue(IntegerLiteral maxDuration, AbortAfterWaitType abortAfterWait)
        {
            Int32 outValue;
            if (!Int32.TryParse(maxDuration.Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out outValue) || (outValue > 71582) || (outValue < 0))
            {
                ThrowParseErrorException("SQL46101", maxDuration, TSqlParserResource.SQL46101Message, maxDuration.Value);
            }

            if (outValue == 0 && abortAfterWait == AbortAfterWaitType.Self)
            {
                ThrowParseErrorException("SQL46102", maxDuration, TSqlParserResource.SQL46102Message, maxDuration.Value, CodeGenerationSupporter.Self);
            }
        }

        protected static void VerifyAllowedIndexOption120(IndexAffectingStatement statement, IndexOption option)
        {
            VerifyAllowedIndexOption(statement, option, SqlVersionFlags.TSql120);
            VerifyAllowedOnlineIndexOptionLowPriorityLockWait(statement, option, SqlVersionFlags.TSql120);
        }

        protected static void VerifyAllowedOnlineIndexOptionLowPriorityLockWait(IndexAffectingStatement statement, IndexOption option, SqlVersionFlags versionFlags)
        {
            // for a low priority lock wait (MLP) option, check if it is allowed for the statement.
            //
            if (option is OnlineIndexOption)
            {
                OnlineIndexOption onlineIndexOption = option as OnlineIndexOption;
                if (onlineIndexOption.LowPriorityLockWaitOption != null)
                {
                    // This syntax for CREATE INDEX currently applies to SQL Server 2022 (16.x), Azure SQL Database, and Azure SQL Managed Instance only. For ALTER INDEX, this syntax applies to SQL Server (Starting with SQL Server 2014 (12.x)) and Azure SQL Database.
                    switch (statement)
                    {
                        case IndexAffectingStatement.AlterIndexRebuildOnePartition:
                        case IndexAffectingStatement.AlterTableRebuildOnePartition:
                        case IndexAffectingStatement.AlterIndexRebuildAllPartitions:
                        case IndexAffectingStatement.AlterTableRebuildAllPartitions:
                            // allowed
                            //
                            break;

                        case IndexAffectingStatement.CreateIndex:
                            // allowed in Sql160 and higher only
                            //
                            if (versionFlags > SqlVersionFlags.TSql150)
                            {
                                break;
                            }
                            else
                            {
                                ThrowWrongIndexOptionError(statement, onlineIndexOption.LowPriorityLockWaitOption);
                                break;
                            }

                        default:
                            // WAIT_AT_LOW_PRIORITY is not a valid index option in the statement
                            //
                            ThrowWrongIndexOptionError(statement, onlineIndexOption.LowPriorityLockWaitOption);
                            break;
                    }
                }
            }
        }

    }
}
