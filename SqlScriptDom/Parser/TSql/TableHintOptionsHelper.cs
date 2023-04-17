//------------------------------------------------------------------------------
// <copyright file="TableHintOptionsHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    
    internal class TableHintOptionsHelper : OptionsHelper<TableHintKind>
    {
        private TableHintOptionsHelper()
        {
            AddOptionMapping(TableHintKind.FastFirstRow, CodeGenerationSupporter.FastFirstRow, SqlVersionFlags.TSqlUnder110);
            AddOptionMapping(TableHintKind.HoldLock, TSqlTokenType.HoldLock, SqlVersionFlags.TSqlAll);
            AddOptionMapping(TableHintKind.NoLock, CodeGenerationSupporter.NoLock, SqlVersionFlags.TSqlAll);
            AddOptionMapping(TableHintKind.PagLock, CodeGenerationSupporter.PagLock, SqlVersionFlags.TSqlAll);
            AddOptionMapping(TableHintKind.ReadCommitted, CodeGenerationSupporter.ReadCommitted, SqlVersionFlags.TSqlAll);
            AddOptionMapping(TableHintKind.ReadPast, CodeGenerationSupporter.ReadPast, SqlVersionFlags.TSqlAll);
            AddOptionMapping(TableHintKind.ReadUncommitted, CodeGenerationSupporter.ReadUncommitted, SqlVersionFlags.TSqlAll);
            AddOptionMapping(TableHintKind.RepeatableRead, CodeGenerationSupporter.RepeatableRead, SqlVersionFlags.TSqlAll);
            AddOptionMapping(TableHintKind.Rowlock, CodeGenerationSupporter.RowLock, SqlVersionFlags.TSqlAll);
            AddOptionMapping(TableHintKind.Serializable, CodeGenerationSupporter.Serializable, SqlVersionFlags.TSqlAll);
            AddOptionMapping(TableHintKind.TabLock, CodeGenerationSupporter.TabLock, SqlVersionFlags.TSqlAll);
            AddOptionMapping(TableHintKind.TabLockX, CodeGenerationSupporter.TabLockX, SqlVersionFlags.TSqlAll);
            AddOptionMapping(TableHintKind.UpdLock, CodeGenerationSupporter.UpdLock, SqlVersionFlags.TSqlAll);
            AddOptionMapping(TableHintKind.XLock, CodeGenerationSupporter.XLock, SqlVersionFlags.TSqlAll);
            AddOptionMapping(TableHintKind.NoExpand, CodeGenerationSupporter.NoExpand, SqlVersionFlags.TSqlAll);
            AddOptionMapping(TableHintKind.NoWait, CodeGenerationSupporter.NoWait, SqlVersionFlags.TSqlAll);

            AddOptionMapping(TableHintKind.ReadCommittedLock, CodeGenerationSupporter.ReadCommittedLock, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(TableHintKind.KeepIdentity, CodeGenerationSupporter.KeepIdentity, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(TableHintKind.KeepDefaults, CodeGenerationSupporter.KeepDefaults, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(TableHintKind.IgnoreConstraints, CodeGenerationSupporter.IgnoreConstraints, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(TableHintKind.IgnoreTriggers, CodeGenerationSupporter.IgnoreTriggers, SqlVersionFlags.TSql90AndAbove);

            AddOptionMapping(TableHintKind.ForceSeek, CodeGenerationSupporter.ForceSeek, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(TableHintKind.ForceScan, CodeGenerationSupporter.ForceScan, SqlVersionFlags.TSql100AndAbove);

            AddOptionMapping(TableHintKind.Snapshot, CodeGenerationSupporter.Snapshot, SqlVersionFlags.TSql120AndAbove);
        }

        internal static readonly TableHintOptionsHelper Instance = new TableHintOptionsHelper();

        protected override TSqlParseErrorException GetMatchingException(antlr.IToken token)
        {
            return new TSqlParseErrorException(TSql80ParserBaseInternal.CreateParseError(
                "SQL46022", token, TSqlParserResource.SQL46022Message, token.getText()));
        }
    }
}
