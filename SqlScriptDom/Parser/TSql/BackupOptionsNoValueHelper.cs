//------------------------------------------------------------------------------
// <copyright file="BackupOptionsNoValueHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------



namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    
    internal class BackupOptionsNoValueHelper : OptionsHelper<BackupOptionKind>
    {
        private BackupOptionsNoValueHelper()
        {
            AddOptionMapping(BackupOptionKind.NoRecovery, CodeGenerationSupporter.NoRecovery);
            AddOptionMapping(BackupOptionKind.TruncateOnly, CodeGenerationSupporter.TruncateOnly);
            AddOptionMapping(BackupOptionKind.NoLog, CodeGenerationSupporter.NoLog);
            AddOptionMapping(BackupOptionKind.NoTruncate, CodeGenerationSupporter.NoTruncate);
            AddOptionMapping(BackupOptionKind.Unload, CodeGenerationSupporter.Unload);
            AddOptionMapping(BackupOptionKind.NoUnload, CodeGenerationSupporter.NoUnload);
            AddOptionMapping(BackupOptionKind.Rewind, CodeGenerationSupporter.Rewind);
            AddOptionMapping(BackupOptionKind.NoRewind, CodeGenerationSupporter.NoRewind);
            AddOptionMapping(BackupOptionKind.Format, CodeGenerationSupporter.Format);
            AddOptionMapping(BackupOptionKind.NoFormat, CodeGenerationSupporter.NoFormat);
            AddOptionMapping(BackupOptionKind.Init, CodeGenerationSupporter.Init);
            AddOptionMapping(BackupOptionKind.NoInit, CodeGenerationSupporter.NoInit);
            AddOptionMapping(BackupOptionKind.Skip, CodeGenerationSupporter.Skip);
            AddOptionMapping(BackupOptionKind.NoSkip, CodeGenerationSupporter.NoSkip);
            AddOptionMapping(BackupOptionKind.Restart, CodeGenerationSupporter.Restart);
            AddOptionMapping(BackupOptionKind.Stats, CodeGenerationSupporter.Stats);
            AddOptionMapping(BackupOptionKind.Differential, CodeGenerationSupporter.Differential);
            AddOptionMapping(BackupOptionKind.Snapshot, CodeGenerationSupporter.Snapshot);
            AddOptionMapping(BackupOptionKind.Checksum, CodeGenerationSupporter.Checksum);
            AddOptionMapping(BackupOptionKind.NoChecksum, CodeGenerationSupporter.NoChecksum);
            AddOptionMapping(BackupOptionKind.ContinueAfterError, CodeGenerationSupporter.ContinueAfterError);
            AddOptionMapping(BackupOptionKind.StopOnError, CodeGenerationSupporter.StopOnError);
            AddOptionMapping(BackupOptionKind.CopyOnly, CodeGenerationSupporter.CopyOnly);
            AddOptionMapping(BackupOptionKind.Compression, CodeGenerationSupporter.Compression, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(BackupOptionKind.NoCompression, CodeGenerationSupporter.NoCompression, SqlVersionFlags.TSql100AndAbove);
        }
        internal static readonly BackupOptionsNoValueHelper Instance = new BackupOptionsNoValueHelper();
    }
}
