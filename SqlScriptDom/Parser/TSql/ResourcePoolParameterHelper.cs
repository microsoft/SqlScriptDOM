//------------------------------------------------------------------------------
// <copyright file="ResourcePoolParameterHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    
    [Serializable]
    internal class ResourcePoolParameterHelper : OptionsHelper<ResourcePoolParameterType>
    {
        private ResourcePoolParameterHelper()
        {
            AddOptionMapping(ResourcePoolParameterType.MaxCpuPercent, CodeGenerationSupporter.MaxCpuPercent);
            AddOptionMapping(ResourcePoolParameterType.MaxMemoryPercent, CodeGenerationSupporter.MaxMemoryPercent);
            AddOptionMapping(ResourcePoolParameterType.MinCpuPercent, CodeGenerationSupporter.MinCpuPercent);
            AddOptionMapping(ResourcePoolParameterType.MinMemoryPercent, CodeGenerationSupporter.MinMemoryPercent);
            AddOptionMapping(ResourcePoolParameterType.CapCpuPercent, CodeGenerationSupporter.CapCpuPercent, SqlVersionFlags.TSql110AndAbove);
            AddOptionMapping(ResourcePoolParameterType.TargetMemoryPercent, CodeGenerationSupporter.TargetMemoryPercent, SqlVersionFlags.TSql110AndAbove);
            AddOptionMapping(ResourcePoolParameterType.MinIoPercent, CodeGenerationSupporter.MinIoPercent, SqlVersionFlags.TSql110AndAbove);
            AddOptionMapping(ResourcePoolParameterType.MaxIoPercent, CodeGenerationSupporter.MaxIoPercent, SqlVersionFlags.TSql110AndAbove);
            AddOptionMapping(ResourcePoolParameterType.CapIoPercent, CodeGenerationSupporter.CapIoPercent, SqlVersionFlags.TSql110AndAbove);
            AddOptionMapping(ResourcePoolParameterType.Affinity, CodeGenerationSupporter.Affinity, SqlVersionFlags.TSql110AndAbove);
            AddOptionMapping(ResourcePoolParameterType.MinIopsPerVolume, CodeGenerationSupporter.MinIopsPerVolume, SqlVersionFlags.TSql120AndAbove);
            AddOptionMapping(ResourcePoolParameterType.MaxIopsPerVolume, CodeGenerationSupporter.MaxIopsPerVolume, SqlVersionFlags.TSql120AndAbove);
        }

        internal static readonly ResourcePoolParameterHelper Instance = new ResourcePoolParameterHelper();
    }
}
