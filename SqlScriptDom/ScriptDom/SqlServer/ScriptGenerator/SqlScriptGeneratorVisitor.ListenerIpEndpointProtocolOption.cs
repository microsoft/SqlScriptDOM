//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.ListenerIpEndpointProtocolOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        // LISTENER_PORT = listenerPort [ , LISTENER_IP = ALL | (<4-part-ip> | <ip_address_v6> ) ]
        public override void ExplicitVisit(ListenerIPEndpointProtocolOption node)
        {
            GenerateIdentifier(CodeGenerationSupporter.ListenerIP);
            GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);

            GenerateSpace();
            if (node.IsAll)
            {
                GenerateKeyword(TSqlTokenType.All); 
            }
            else
            {
                GenerateSymbol(TSqlTokenType.LeftParenthesis);

                if (node.IPv6 != null)
                {
                    GenerateFragmentIfNotNull(node.IPv6);
                }
                else
                {
                    GenerateFragmentIfNotNull(node.IPv4PartOne);
                    if (node.IPv4PartTwo != null)
                    {
                        GenerateSymbol(TSqlTokenType.Colon);
                        GenerateFragmentIfNotNull(node.IPv4PartTwo);
                    }
                }

                GenerateSymbol(TSqlTokenType.RightParenthesis); 
            }
        }
    }
}
