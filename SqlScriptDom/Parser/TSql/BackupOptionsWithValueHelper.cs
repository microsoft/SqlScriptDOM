//------------------------------------------------------------------------------
// <copyright file="BackupOptionsWithValueHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------



namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    
    internal class BackupOptionsWithValueHelper : OptionsHelper<BackupOptionKind>
    {
        private BackupOptionsWithValueHelper()
        {
            AddOptionMapping(BackupOptionKind.Stats, CodeGenerationSupporter.Stats);
            AddOptionMapping(BackupOptionKind.Standby, CodeGenerationSupporter.Standby);
            AddOptionMapping(BackupOptionKind.ExpireDate, CodeGenerationSupporter.ExpireDate);
            AddOptionMapping(BackupOptionKind.RetainDays, CodeGenerationSupporter.RetainDays);
            AddOptionMapping(BackupOptionKind.Name, CodeGenerationSupporter.Name);
            AddOptionMapping(BackupOptionKind.Description, CodeGenerationSupporter.Description);
            AddOptionMapping(BackupOptionKind.Password, CodeGenerationSupporter.Password, SqlVersionFlags.TSqlUnder110);
            AddOptionMapping(BackupOptionKind.MediaName, CodeGenerationSupporter.MediaName);
            AddOptionMapping(BackupOptionKind.MediaDescription, CodeGenerationSupporter.MediaDescription);
            AddOptionMapping(BackupOptionKind.MediaPassword, CodeGenerationSupporter.MediaPassword, SqlVersionFlags.TSqlUnder110);
            AddOptionMapping(BackupOptionKind.BlockSize, CodeGenerationSupporter.BlockSize);
            AddOptionMapping(BackupOptionKind.BufferCount, CodeGenerationSupporter.BufferCount);
            AddOptionMapping(BackupOptionKind.MaxTransferSize, CodeGenerationSupporter.MaxTransferSize);
            AddOptionMapping(BackupOptionKind.EnhancedIntegrity, CodeGenerationSupporter.EnhancedIntegrity);
        }
        internal static readonly BackupOptionsWithValueHelper Instance = new BackupOptionsWithValueHelper();
    }
}
