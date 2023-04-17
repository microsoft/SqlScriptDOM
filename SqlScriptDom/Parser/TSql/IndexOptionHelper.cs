//------------------------------------------------------------------------------
// <copyright file="IndexOptionHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{

    internal class IndexOptionHelper : OptionsHelper<IndexOptionKind>
    {
        private IndexOptionHelper()
        {
            AddOptionMapping(IndexOptionKind.PadIndex, CodeGenerationSupporter.PadIndex, SqlVersionFlags.TSqlAll);
            AddOptionMapping(IndexOptionKind.FillFactor, TSqlTokenType.FillFactor, SqlVersionFlags.TSqlAll);
            AddOptionMapping(IndexOptionKind.SortInTempDB, CodeGenerationSupporter.SortInTempDb, SqlVersionFlags.TSqlAll);
            AddOptionMapping(IndexOptionKind.IgnoreDupKey, CodeGenerationSupporter.IgnoreDupKey, SqlVersionFlags.TSqlAll);
            AddOptionMapping(IndexOptionKind.StatisticsNoRecompute, CodeGenerationSupporter.StatisticsNoRecompute, SqlVersionFlags.TSqlAll);
            AddOptionMapping(IndexOptionKind.DropExisting, CodeGenerationSupporter.DropExisting, SqlVersionFlags.TSqlAll);

            AddOptionMapping(IndexOptionKind.Online, CodeGenerationSupporter.Online, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(IndexOptionKind.AllowPageLocks, CodeGenerationSupporter.AllowPageLocks, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(IndexOptionKind.AllowRowLocks, CodeGenerationSupporter.AllowRowLocks, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(IndexOptionKind.MaxDop, CodeGenerationSupporter.MaxDop, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(IndexOptionKind.LobCompaction, CodeGenerationSupporter.LobCompaction, SqlVersionFlags.TSql90AndAbove);

            AddOptionMapping(IndexOptionKind.FileStreamOn, CodeGenerationSupporter.FileStreamOn, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(IndexOptionKind.DataCompression, CodeGenerationSupporter.DataCompression, SqlVersionFlags.TSql100AndAbove);

            AddOptionMapping(IndexOptionKind.BucketCount, CodeGenerationSupporter.BucketCount, SqlVersionFlags.TSql120AndAbove);
            AddOptionMapping(IndexOptionKind.StatisticsIncremental, CodeGenerationSupporter.StatisticsIncremental, SqlVersionFlags.TSql120AndAbove);

            AddOptionMapping(IndexOptionKind.Order, CodeGenerationSupporter.Order, SqlVersionFlags.TSql130AndAbove);
            AddOptionMapping(IndexOptionKind.CompressAllRowGroups, CodeGenerationSupporter.CompressAllRowGroups, SqlVersionFlags.TSql130AndAbove);
            AddOptionMapping(IndexOptionKind.CompressionDelay, CodeGenerationSupporter.CompressionDelay, SqlVersionFlags.TSql130AndAbove);

            AddOptionMapping(IndexOptionKind.Resumable, CodeGenerationSupporter.Resumable, SqlVersionFlags.TSql140AndAbove);
            AddOptionMapping(IndexOptionKind.MaxDuration, CodeGenerationSupporter.MaxDuration, SqlVersionFlags.TSql140AndAbove);
            AddOptionMapping(IndexOptionKind.WaitAtLowPriority, CodeGenerationSupporter.WaitAtLowPriority, SqlVersionFlags.TSql140AndAbove);

            AddOptionMapping(IndexOptionKind.OptimizeForSequentialKey, CodeGenerationSupporter.OptimizeForSequentialKey, SqlVersionFlags.TSql150AndAbove);

            AddOptionMapping(IndexOptionKind.XmlCompression, CodeGenerationSupporter.XmlCompression, SqlVersionFlags.TSql160AndAbove);

        }

        internal static readonly IndexOptionHelper Instance = new IndexOptionHelper();
    }
}
