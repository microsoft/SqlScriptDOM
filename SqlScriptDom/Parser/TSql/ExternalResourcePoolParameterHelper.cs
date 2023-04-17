//------------------------------------------------------------------------------
// <copyright file="ExternalResourcePoolParameterHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    [Serializable]
    internal class ExternalResourcePoolParameterHelper : OptionsHelper<ExternalResourcePoolParameterType>
    {
        private ExternalResourcePoolParameterHelper()
        {
            AddOptionMapping(ExternalResourcePoolParameterType.MaxCpuPercent, CodeGenerationSupporter.MaxCpuPercent, SqlVersionFlags.TSql130AndAbove);
            AddOptionMapping(ExternalResourcePoolParameterType.MaxMemoryPercent, CodeGenerationSupporter.MaxMemoryPercent, SqlVersionFlags.TSql130AndAbove);
            AddOptionMapping(ExternalResourcePoolParameterType.MaxProcesses, CodeGenerationSupporter.MaxProcesses, SqlVersionFlags.TSql130AndAbove);
            AddOptionMapping(ExternalResourcePoolParameterType.Affinity, CodeGenerationSupporter.Affinity, SqlVersionFlags.TSql130AndAbove);
        }

        internal static readonly ExternalResourcePoolParameterHelper Instance = new ExternalResourcePoolParameterHelper();
    }
}
