//------------------------------------------------------------------------------
// <copyright file="EventSessionMemoryPartitionModeTypeHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    internal class EventSessionMemoryPartitionModeTypeHelper : OptionsHelper<EventSessionMemoryPartitionModeType>
    {
        private EventSessionMemoryPartitionModeTypeHelper()
        {
            AddOptionMapping(EventSessionMemoryPartitionModeType.None, CodeGenerationSupporter.None);
            AddOptionMapping(EventSessionMemoryPartitionModeType.PerCpu, CodeGenerationSupporter.PerCpu);
            AddOptionMapping(EventSessionMemoryPartitionModeType.PerNode, CodeGenerationSupporter.PerNode);           
        }

        internal static readonly EventSessionMemoryPartitionModeTypeHelper Instance = new EventSessionMemoryPartitionModeTypeHelper();
    }
}
