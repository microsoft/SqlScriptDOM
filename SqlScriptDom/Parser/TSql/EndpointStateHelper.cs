//------------------------------------------------------------------------------
// <copyright file="EndpointStateHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    
    internal class EndpointStateHelper : OptionsHelper<EndpointState>
    {
        private EndpointStateHelper()
        {
            AddOptionMapping(EndpointState.Disabled, CodeGenerationSupporter.Disabled);
            AddOptionMapping(EndpointState.Started, CodeGenerationSupporter.Started);
            AddOptionMapping(EndpointState.Stopped, CodeGenerationSupporter.Stopped);
        }

        internal static readonly EndpointStateHelper Instance = new EndpointStateHelper();
    }
}
