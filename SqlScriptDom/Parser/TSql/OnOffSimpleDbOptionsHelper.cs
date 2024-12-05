//------------------------------------------------------------------------------
// <copyright file="OnOffSimpleDbOptionsHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------



namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    
    internal class OnOffSimpleDbOptionsHelper : OptionsHelper<DatabaseOptionKind>
    {
        private OnOffSimpleDbOptionsHelper()
        {
            // 80 options
            AddOptionMapping(DatabaseOptionKind.CursorCloseOnCommit, CodeGenerationSupporter.CursorCloseOnCommit, SqlVersionFlags.TSqlAll);
            AddOptionMapping(DatabaseOptionKind.AutoClose, CodeGenerationSupporter.AutoClose, SqlVersionFlags.TSqlAll);
            AddOptionMapping(DatabaseOptionKind.AutoCreateStatistics, CodeGenerationSupporter.AutoCreateStatistics, SqlVersionFlags.TSqlAll);
            AddOptionMapping(DatabaseOptionKind.AutoShrink, CodeGenerationSupporter.AutoShrink, SqlVersionFlags.TSqlAll);
            AddOptionMapping(DatabaseOptionKind.AutoUpdateStatistics, CodeGenerationSupporter.AutoUpdateStatistics, SqlVersionFlags.TSqlAll);
            AddOptionMapping(DatabaseOptionKind.AnsiNullDefault, CodeGenerationSupporter.AnsiNullDefault, SqlVersionFlags.TSqlAll);
            AddOptionMapping(DatabaseOptionKind.AnsiNulls, CodeGenerationSupporter.AnsiNulls, SqlVersionFlags.TSqlAll);
            AddOptionMapping(DatabaseOptionKind.AnsiPadding, CodeGenerationSupporter.AnsiPadding, SqlVersionFlags.TSqlAll);
            AddOptionMapping(DatabaseOptionKind.AnsiWarnings, CodeGenerationSupporter.AnsiWarnings, SqlVersionFlags.TSqlAll);
            AddOptionMapping(DatabaseOptionKind.ArithAbort, CodeGenerationSupporter.ArithAbort, SqlVersionFlags.TSqlAll);
            AddOptionMapping(DatabaseOptionKind.ConcatNullYieldsNull, CodeGenerationSupporter.ConcatNullYieldsNull, SqlVersionFlags.TSqlAll);
            AddOptionMapping(DatabaseOptionKind.NumericRoundAbort, CodeGenerationSupporter.NumericRoundAbort, SqlVersionFlags.TSqlAll);
            AddOptionMapping(DatabaseOptionKind.QuotedIdentifier, CodeGenerationSupporter.QuotedIdentifier, SqlVersionFlags.TSqlAll);
            AddOptionMapping(DatabaseOptionKind.RecursiveTriggers, CodeGenerationSupporter.RecursiveTriggers, SqlVersionFlags.TSqlAll);
            AddOptionMapping(DatabaseOptionKind.TornPageDetection, CodeGenerationSupporter.TornPageDetection, SqlVersionFlags.TSqlAll);

            // 90 options
            AddOptionMapping(DatabaseOptionKind.DBChaining, CodeGenerationSupporter.DbChaining, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(DatabaseOptionKind.Trustworthy, CodeGenerationSupporter.Trustworthy, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(DatabaseOptionKind.AutoUpdateStatisticsAsync, CodeGenerationSupporter.AutoUpdateStatisticsAsync, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(DatabaseOptionKind.DateCorrelationOptimization, CodeGenerationSupporter.DateCorrelationOptimization, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(DatabaseOptionKind.AllowSnapshotIsolation, CodeGenerationSupporter.AllowSnapshotIsolation, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(DatabaseOptionKind.ReadCommittedSnapshot, CodeGenerationSupporter.ReadCommittedSnapshot, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(DatabaseOptionKind.SupplementalLogging, CodeGenerationSupporter.SupplementalLogging, SqlVersionFlags.TSql90AndAbove);

            // 100 options
            AddOptionMapping(DatabaseOptionKind.Encryption, CodeGenerationSupporter.Encryption, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(DatabaseOptionKind.HonorBrokerPriority, CodeGenerationSupporter.HonorBrokerPriority, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(DatabaseOptionKind.VarDecimalStorageFormat, CodeGenerationSupporter.VardecimalStorageFormat, SqlVersionFlags.TSql100AndAbove);

            // 110 options
            AddOptionMapping(DatabaseOptionKind.NestedTriggers, CodeGenerationSupporter.NestedTriggers, SqlVersionFlags.TSql110AndAbove);
            AddOptionMapping(DatabaseOptionKind.TransformNoiseWords, CodeGenerationSupporter.TransformNoiseWords, SqlVersionFlags.TSql110AndAbove);

            // 120 options
            AddOptionMapping(DatabaseOptionKind.MemoryOptimizedElevateToSnapshot, CodeGenerationSupporter.MemoryOptimizedElevateToSnapshot, SqlVersionFlags.TSql120AndAbove);

            // 130 options
            AddOptionMapping(DatabaseOptionKind.MixedPageAllocation, CodeGenerationSupporter.MixedPageAllocation, SqlVersionFlags.TSql130AndAbove);
           
            // 140 options
            AddOptionMapping(DatabaseOptionKind.TemporalHistoryRetention, CodeGenerationSupporter.TemporalHistoryRetention, SqlVersionFlags.TSql140AndAbove);

            // 150 options
            AddOptionMapping(DatabaseOptionKind.DataRetention, CodeGenerationSupporter.DataRetention, SqlVersionFlags.TSql150AndAbove);

            // 160 options
            AddOptionMapping(DatabaseOptionKind.Ledger, CodeGenerationSupporter.Ledger, SqlVersionFlags.TSql160AndAbove);

            // 170 options
            // TODO: add any new 170 options here

        }

        internal static readonly OnOffSimpleDbOptionsHelper Instance = new OnOffSimpleDbOptionsHelper();

        /// <summary>
        /// Indicates whether the DatabaseOption requires specifying an EqualsSign between the option and the value.
        /// </summary>
        /// <param name="optionKind">DatabaseOption kind</param>
        /// <returns></returns>
        internal bool RequiresEqualsSign(DatabaseOptionKind optionKind)
        {
            switch (optionKind)
            {
                case DatabaseOptionKind.MemoryOptimizedElevateToSnapshot:
                case DatabaseOptionKind.NestedTriggers:
                case DatabaseOptionKind.TransformNoiseWords:
                case DatabaseOptionKind.Ledger:
                    return true;
                default:
                    return false;
            }
        }
    }
}
