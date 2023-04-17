//------------------------------------------------------------------------------
// <copyright file="ResourcePoolAffinityHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    
    [Serializable]
    internal class ResourcePoolAffinityHelper : OptionsHelper<ResourcePoolAffinityType>
    {
        private ResourcePoolAffinityHelper()
        {
            AddOptionMapping(ResourcePoolAffinityType.Scheduler, CodeGenerationSupporter.Scheduler);
            AddOptionMapping(ResourcePoolAffinityType.NumaNode, CodeGenerationSupporter.NumaNode);
        }

        internal static readonly ResourcePoolAffinityHelper Instance = new ResourcePoolAffinityHelper();
    }
}
