//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AuthenticationPayloadOption.cs" company="Microsoft">
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
        protected static Dictionary<AuthenticationProtocol, String> _authenticationProtocolNames = new Dictionary<AuthenticationProtocol, String>()
        {
            {AuthenticationProtocol.Windows, CodeGenerationSupporter.Windows},
            {AuthenticationProtocol.WindowsKerberos, CodeGenerationSupporter.Kerberos},
            {AuthenticationProtocol.WindowsNegotiate, CodeGenerationSupporter.Negotiate},
            {AuthenticationProtocol.WindowsNtlm, CodeGenerationSupporter.Ntlm},
        };

        public override void ExplicitVisit(AuthenticationPayloadOption node)
        {
            // AUTHENTICATION = WINDOWS 
            GenerateTokenAndEqualSign(CodeGenerationSupporter.Authentication);

            // Order of CERTIFICATE and Windows Authentication options is important
            GenerateCertificateForAuthenticationPayloadOption(node.TryCertificateFirst, node.Certificate);

            //[ { NTLM | KERBEROS | NEGOTIATE } ]
            if (node.Protocol != AuthenticationProtocol.NotSpecified)
            {
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Windows);
                if (node.Protocol != AuthenticationProtocol.Windows)
                {
                    String optionName = GetValueForEnumKey(_authenticationProtocolNames, node.Protocol);
                    if (optionName != null)
                    {
                        GenerateSpaceAndIdentifier(optionName);
                    }
                }
            }

            GenerateCertificateForAuthenticationPayloadOption(!node.TryCertificateFirst, node.Certificate);
        }

        protected void GenerateCertificateForAuthenticationPayloadOption(bool generate, Identifier certificateName)
        {
            if (generate && certificateName != null)
            {
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Certificate);
                GenerateSpaceAndFragmentIfNotNull(certificateName);
            }
        }
    }
}
