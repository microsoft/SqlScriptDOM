//------------------------------------------------------------------------------
// <copyright file="WorkloadGroupResourceParameterHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
 
    [Serializable]
    internal class WorkloadGroupResourceParameterHelper : OptionsHelper<WorkloadGroupParameterType>
    {
        private WorkloadGroupResourceParameterHelper()
        {
            AddOptionMapping(WorkloadGroupParameterType.RequestMaxMemoryGrantPercent, CodeGenerationSupporter.RequestMaxMemoryGrantPercent);
            AddOptionMapping(WorkloadGroupParameterType.RequestMaxCpuTimeSec, CodeGenerationSupporter.RequestMaxCpuTimeSec);
            AddOptionMapping(WorkloadGroupParameterType.RequestMemoryGrantTimeoutSec, CodeGenerationSupporter.RequestMemoryGrantTimeoutSec);
            AddOptionMapping(WorkloadGroupParameterType.MaxDop, CodeGenerationSupporter.Max_Dop);
            AddOptionMapping(WorkloadGroupParameterType.GroupMaxRequests, CodeGenerationSupporter.GroupMaxRequests);
            AddOptionMapping(WorkloadGroupParameterType.GroupMinMemoryPercent, CodeGenerationSupporter.GroupMinMemoryPercent, SqlVersionFlags.TSql110AndAbove);
            AddOptionMapping(WorkloadGroupParameterType.CapPercentageResource, CodeGenerationSupporter.CapPercentageResource);
            AddOptionMapping(WorkloadGroupParameterType.MinPercentageResource, CodeGenerationSupporter.MinPercentageResource);
            AddOptionMapping(WorkloadGroupParameterType.QueryExecutionTimeoutSec, CodeGenerationSupporter.QueryExecutionTimeoutSec);
            AddOptionMapping(WorkloadGroupParameterType.RequestMaxResourceGrantPercent, CodeGenerationSupporter.RequestMaxResourceGrantPercent);
            AddOptionMapping(WorkloadGroupParameterType.RequestMinResourceGrantPercent, CodeGenerationSupporter.RequestMinResourceGrantPercent);
        }

        internal static readonly WorkloadGroupResourceParameterHelper Instance = new WorkloadGroupResourceParameterHelper();
    }
}
