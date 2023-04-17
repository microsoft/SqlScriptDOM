//------------------------------------------------------------------------------
// <copyright file="RestoreOptionWithValueHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    
    [Serializable]
    internal class RestoreOptionWithValueHelper : OptionsHelper<RestoreOptionKind>
    {
        private RestoreOptionWithValueHelper()
        {
            AddOptionMapping(RestoreOptionKind.File, TSqlTokenType.File);
            AddOptionMapping(RestoreOptionKind.Stats, CodeGenerationSupporter.Stats);
            AddOptionMapping(RestoreOptionKind.StopAt, CodeGenerationSupporter.StopAt);
            AddOptionMapping(RestoreOptionKind.MediaName, CodeGenerationSupporter.MediaName);
            AddOptionMapping(RestoreOptionKind.MediaPassword, CodeGenerationSupporter.MediaPassword);
            AddOptionMapping(RestoreOptionKind.Password, CodeGenerationSupporter.Password);
            AddOptionMapping(RestoreOptionKind.BlockSize, CodeGenerationSupporter.BlockSize);
            AddOptionMapping(RestoreOptionKind.BufferCount, CodeGenerationSupporter.BufferCount);
            AddOptionMapping(RestoreOptionKind.MaxTransferSize, CodeGenerationSupporter.MaxTransferSize);
            AddOptionMapping(RestoreOptionKind.Standby, CodeGenerationSupporter.Standby);
            AddOptionMapping(RestoreOptionKind.EnhancedIntegrity, CodeGenerationSupporter.EnhancedIntegrity);
            AddOptionMapping(RestoreOptionKind.SnapshotRestorePhase, CodeGenerationSupporter.SnapshotRestorePhase);
        }

        internal static readonly RestoreOptionWithValueHelper Instance = new RestoreOptionWithValueHelper();
    }
}
