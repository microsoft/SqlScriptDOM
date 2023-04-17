//------------------------------------------------------------------------------
// <copyright file="EndpointProtocolOptionsHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    
    internal class EndpointProtocolOptionsHelper : OptionsHelper<EndpointProtocolOptions>
    {
        private EndpointProtocolOptionsHelper()
        {
            AddOptionMapping(EndpointProtocolOptions.HttpAuthenticationRealm, CodeGenerationSupporter.AuthRealm);
            AddOptionMapping(EndpointProtocolOptions.HttpAuthentication, CodeGenerationSupporter.Authentication);
            AddOptionMapping(EndpointProtocolOptions.HttpClearPort, CodeGenerationSupporter.ClearPort);
            AddOptionMapping(EndpointProtocolOptions.HttpCompression, CodeGenerationSupporter.Compression);
            AddOptionMapping(EndpointProtocolOptions.HttpDefaultLogonDomain, CodeGenerationSupporter.DefaultLogonDomain);
            AddOptionMapping(EndpointProtocolOptions.HttpPath, CodeGenerationSupporter.Path);
            AddOptionMapping(EndpointProtocolOptions.HttpPorts, CodeGenerationSupporter.Ports);
            AddOptionMapping(EndpointProtocolOptions.HttpSite, CodeGenerationSupporter.Site);
            AddOptionMapping(EndpointProtocolOptions.HttpSslPort, CodeGenerationSupporter.SslPort);
            AddOptionMapping(EndpointProtocolOptions.TcpListenerIP, CodeGenerationSupporter.ListenerIP);
            AddOptionMapping(EndpointProtocolOptions.TcpListenerPort, CodeGenerationSupporter.ListenerPort);
        }

        internal static readonly EndpointProtocolOptionsHelper Instance = new EndpointProtocolOptionsHelper();
    }
}
