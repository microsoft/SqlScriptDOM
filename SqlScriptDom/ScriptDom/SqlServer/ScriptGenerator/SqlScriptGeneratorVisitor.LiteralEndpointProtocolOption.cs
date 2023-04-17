//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.LiteralEndpointProtocolOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        protected static Dictionary<EndpointProtocolOptions, String> _endpointProtocolOptionsNames = new Dictionary<EndpointProtocolOptions, String>()
        {
            // handled specially
            //{EndpointProtocolOptions.HttpAuthentication, CodeGenerationSupporter},
            {EndpointProtocolOptions.HttpAuthenticationRealm, CodeGenerationSupporter.AuthRealm},
            {EndpointProtocolOptions.HttpClearPort, CodeGenerationSupporter.ClearPort},
            // handled specially
            //{EndpointProtocolOptions.HttpCompression, CodeGenerationSupporter},
            {EndpointProtocolOptions.HttpDefaultLogonDomain, CodeGenerationSupporter.DefaultLogonDomain},
            // all HTTP options
            //{EndpointProtocolOptions.HttpOptions, CodeGenerationSupporter},
            {EndpointProtocolOptions.HttpPath, CodeGenerationSupporter.Path},
            // handled specially
            //{EndpointProtocolOptions.HttpPorts, CodeGenerationSupporter.Ports},
            {EndpointProtocolOptions.HttpSite, CodeGenerationSupporter.Site},
            {EndpointProtocolOptions.HttpSslPort, CodeGenerationSupporter.SslPort},

            // handled specially
            //{EndpointProtocolOptions.TcpListenerIp, CodeGenerationSupporter},
            {EndpointProtocolOptions.TcpListenerPort, CodeGenerationSupporter.ListenerPort},
            // all TCP options
            //{EndpointProtocolOptions.TcpOptions, CodeGenerationSupporter},
        };
  

        public override void ExplicitVisit(LiteralEndpointProtocolOption node)
        {
            String optionName = GetValueForEnumKey(_endpointProtocolOptionsNames, node.Kind);
            if (optionName != null)
            {
                if (node.Value != null)
                {
                    GenerateNameEqualsValue(optionName, node.Value);
                }
                else
                {
                    GenerateNameEqualsValue(optionName, CodeGenerationSupporter.None);
                }
            }
        }
    }
}
