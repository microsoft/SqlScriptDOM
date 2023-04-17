//------------------------------------------------------------------------------
// <copyright file="ExternalResourcePoolAffinityHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    [Serializable]
    internal class ExternalResourcePoolAffinityHelper : OptionsHelper<ExternalResourcePoolAffinityType>
    {
        private ExternalResourcePoolAffinityHelper()
        {
            // 130 options
            AddOptionMapping(ExternalResourcePoolAffinityType.Cpu, CodeGenerationSupporter.Cpu, SqlVersionFlags.TSql130AndAbove);
            AddOptionMapping(ExternalResourcePoolAffinityType.NumaNode, CodeGenerationSupporter.NumaNode, SqlVersionFlags.TSql130AndAbove);
        }

        internal static readonly ExternalResourcePoolAffinityHelper Instance = new ExternalResourcePoolAffinityHelper();
    }
}
