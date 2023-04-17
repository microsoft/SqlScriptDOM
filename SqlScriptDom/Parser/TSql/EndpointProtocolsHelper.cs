//------------------------------------------------------------------------------
// <copyright file="EndpointProtocolsHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    
    internal class EndpointProtocolsHelper : OptionsHelper<EndpointProtocol>
    {
        private EndpointProtocolsHelper()
        {
            AddOptionMapping(EndpointProtocol.Tcp, CodeGenerationSupporter.Tcp);
            AddOptionMapping(EndpointProtocol.Http, CodeGenerationSupporter.Http);
        }

        internal static readonly EndpointProtocolsHelper Instance = new EndpointProtocolsHelper();
    }
}
