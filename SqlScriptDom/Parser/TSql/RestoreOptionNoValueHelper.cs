//------------------------------------------------------------------------------
// <copyright file="RestoreOptionNoValueHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    
    [Serializable]
    internal class RestoreOptionNoValueHelper : OptionsHelper<RestoreOptionKind>
    {
        private RestoreOptionNoValueHelper()
        {
            AddOptionMapping(RestoreOptionKind.NoLog, CodeGenerationSupporter.NoLog);
            AddOptionMapping(RestoreOptionKind.Checksum, CodeGenerationSupporter.Checksum);
            AddOptionMapping(RestoreOptionKind.NoChecksum, CodeGenerationSupporter.NoChecksum);
            AddOptionMapping(RestoreOptionKind.ContinueAfterError, CodeGenerationSupporter.ContinueAfterError);
            AddOptionMapping(RestoreOptionKind.StopOnError, CodeGenerationSupporter.StopOnError);
            AddOptionMapping(RestoreOptionKind.Unload, CodeGenerationSupporter.Unload);
            AddOptionMapping(RestoreOptionKind.NoUnload, CodeGenerationSupporter.NoUnload);
            AddOptionMapping(RestoreOptionKind.Rewind, CodeGenerationSupporter.Rewind);
            AddOptionMapping(RestoreOptionKind.NoRewind, CodeGenerationSupporter.NoRewind);
            AddOptionMapping(RestoreOptionKind.Stats, CodeGenerationSupporter.Stats);
            AddOptionMapping(RestoreOptionKind.NoRecovery, CodeGenerationSupporter.NoRecovery);
            AddOptionMapping(RestoreOptionKind.Recovery, CodeGenerationSupporter.Recovery);
            AddOptionMapping(RestoreOptionKind.Replace, CodeGenerationSupporter.Replace);
            AddOptionMapping(RestoreOptionKind.Restart, CodeGenerationSupporter.Restart);
            AddOptionMapping(RestoreOptionKind.Verbose, CodeGenerationSupporter.Verbose);
            AddOptionMapping(RestoreOptionKind.LoadHistory, CodeGenerationSupporter.LoadHistory);
            AddOptionMapping(RestoreOptionKind.DboOnly, CodeGenerationSupporter.DboOnly, SqlVersionFlags.TSqlUnder110);
            AddOptionMapping(RestoreOptionKind.RestrictedUser, CodeGenerationSupporter.RestrictedUser);
            AddOptionMapping(RestoreOptionKind.Partial, CodeGenerationSupporter.Partial);
            AddOptionMapping(RestoreOptionKind.Snapshot, CodeGenerationSupporter.Snapshot);
            AddOptionMapping(RestoreOptionKind.KeepReplication, CodeGenerationSupporter.KeepReplication);
            AddOptionMapping(RestoreOptionKind.KeepTemporalRetention, CodeGenerationSupporter.KeepTemporalRetention, SqlVersionFlags.TSql140AndAbove);
            AddOptionMapping(RestoreOptionKind.Online, CodeGenerationSupporter.Online);
            AddOptionMapping(RestoreOptionKind.CommitDifferentialBase, CodeGenerationSupporter.CommitDifferentialBase);
            AddOptionMapping(RestoreOptionKind.SnapshotImport, CodeGenerationSupporter.SnapshotImport);
            AddOptionMapping(RestoreOptionKind.NewBroker, CodeGenerationSupporter.NewBroker);
            AddOptionMapping(RestoreOptionKind.EnableBroker, CodeGenerationSupporter.EnableBroker);
            AddOptionMapping(RestoreOptionKind.ErrorBrokerConversations, CodeGenerationSupporter.ErrorBrokerConversations);
        }

        internal static readonly RestoreOptionNoValueHelper Instance = new RestoreOptionNoValueHelper();
    }
}
