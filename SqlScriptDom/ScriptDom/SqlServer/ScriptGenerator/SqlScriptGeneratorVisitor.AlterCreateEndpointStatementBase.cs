//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterCreateEndpointStatementBase.cs" company="Microsoft">
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
        protected static Dictionary<EndpointState, String> _endpointStateNames = new Dictionary<EndpointState, String>()
        {
            {EndpointState.Disabled, CodeGenerationSupporter.Disabled},
            {EndpointState.Started, CodeGenerationSupporter.Started},
            {EndpointState.Stopped, CodeGenerationSupporter.Stopped},
        };
  
        protected static Dictionary<EndpointProtocol, String> _endpointProtocolNames = new Dictionary<EndpointProtocol, String>()
        {
            {EndpointProtocol.Http, CodeGenerationSupporter.Http},
            {EndpointProtocol.Tcp, CodeGenerationSupporter.Tcp},
        };

        protected static Dictionary<EndpointType, String> _endpointTypeNames = new Dictionary<EndpointType, String>()
        {
            {EndpointType.DatabaseMirroring, CodeGenerationSupporter.DatabaseMirroring},
            {EndpointType.ServiceBroker, CodeGenerationSupporter.ServiceBroker},
            {EndpointType.Soap, CodeGenerationSupporter.Soap},
            {EndpointType.TSql, CodeGenerationSupporter.TSql},
        };

        protected static Dictionary<PayloadOptionKinds, TokenGenerator> _payloadOptionKindsGenerators = new Dictionary<PayloadOptionKinds, TokenGenerator>()
        {
            { PayloadOptionKinds.Authentication, new IdentifierGenerator(CodeGenerationSupporter.Authentication) },
            { PayloadOptionKinds.Batches, new IdentifierGenerator(CodeGenerationSupporter.Batches) },
            { PayloadOptionKinds.CharacterSet, new IdentifierGenerator(CodeGenerationSupporter.CharacterSet) },
            { PayloadOptionKinds.Database, new KeywordGenerator(TSqlTokenType.Database) },
            { PayloadOptionKinds.Encryption, new IdentifierGenerator(CodeGenerationSupporter.Encryption) },
            { PayloadOptionKinds.HeaderLimit, new IdentifierGenerator(CodeGenerationSupporter.HeaderLimit) },
            { PayloadOptionKinds.LoginType, new IdentifierGenerator(CodeGenerationSupporter.LoginType) },
            { PayloadOptionKinds.MessageForwardSize, new IdentifierGenerator(CodeGenerationSupporter.MessageForwardSize) },
            { PayloadOptionKinds.MessageForwarding, new IdentifierGenerator(CodeGenerationSupporter.MessageForwarding) },
            { PayloadOptionKinds.Namespace, new IdentifierGenerator(CodeGenerationSupporter.Namespace) },
            { PayloadOptionKinds.None, new EmptyGenerator() },
            { PayloadOptionKinds.Role, new IdentifierGenerator(CodeGenerationSupporter.Role) },
            { PayloadOptionKinds.Schema, new KeywordGenerator(TSqlTokenType.Schema) },
            { PayloadOptionKinds.SessionTimeout, new IdentifierGenerator(CodeGenerationSupporter.SessionTimeout) },
            { PayloadOptionKinds.Sessions, new IdentifierGenerator(CodeGenerationSupporter.Sessions) },
            { PayloadOptionKinds.WebMethod, new IdentifierGenerator(CodeGenerationSupporter.WebMethod) },
            { PayloadOptionKinds.Wsdl, new IdentifierGenerator(CodeGenerationSupporter.Wsdl) },
            // option combinations of different types 
            //{ PayloadOptionKinds.SoapOptions, new IdentifierGenerator(CodeGenerationSupporter.SoapOptions) },
            //{ PayloadOptionKinds.ServiceBrokerOptions, new IdentifierGenerator(CodeGenerationSupporter.ServiceBrokerOptions) },
            //{ PayloadOptionKinds.DatabaseMirroringOptions, new IdentifierGenerator(CodeGenerationSupporter.DatabaseMirroringOptions) },
        };

        protected void GenerateEndpointBody(AlterCreateEndpointStatementBase node)
        {
            // STATE = { STARTED | STOPPED | DISABLED }
            if (node.State != EndpointState.NotSpecified)
            {
                String optionName = GetValueForEnumKey(_endpointStateNames, node.State);
                if (optionName != null)
                {
                    NewLineAndIndent();
                    GenerateNameEqualsValue(CodeGenerationSupporter.State, optionName); 
                }
            }

            // AFFINITY = { NONE | <64bit_integer> | ADMIN }
            if (node.Affinity != null)
            {
                if (node.State != EndpointState.NotSpecified)
                {
                    GenerateSymbolAndSpace(TSqlTokenType.Comma); 
                }
                NewLineAndIndent();
                GenerateFragmentIfNotNull(node.Affinity);
            }

            // AS { TCP | HTTP } (option1, option2, ...)
            if (node.Protocol != EndpointProtocol.None)
            {
                NewLineAndIndent();
                GenerateKeyword(TSqlTokenType.As); 

                String optionName = GetValueForEnumKey(_endpointProtocolNames, node.Protocol);
                if (optionName != null)
                {
                    GenerateSpaceAndIdentifier(optionName); 
                }

                // could be
                //      AuthenticationEndpointProtocolOption
                //      CompressionEndpointProtocolOption
                //      ListenerIpEndpointProtocolOption
                //      LiteralEndpointProtocolOption
                //      PortsEndpointProtocolOption

                GenerateAlignedParenthesizedOptionsWithMultipleIndent(node.ProtocolOptions, 3);
            }

            // FOR { SOAP | TSQL | SERVICE_BROKER | DATABASE_MIRRORING } (option1, option2, ...)
            if (node.EndpointType != EndpointType.NotSpecified)
            {
                NewLineAndIndent();
                GenerateKeyword(TSqlTokenType.For); 

                String optionName = GetValueForEnumKey(_endpointTypeNames, node.EndpointType);
                if (optionName != null)
                {
                    GenerateSpaceAndIdentifier(optionName); 
                }

                // could be
                //      AuthenticationPayloadOption
                //      CharacterSetPayloadOption
                //      EnabledDisabledPayloadOption
                //      EncryptionPayloadOption
                //      LiteralPayloadOption
                //      LoginTypePayloadOption
                //      RolePayloadOption
                //      SchemaPayloadOption
                //      SessionTimeoutPayloadOption
                //      SoapMethod
                //      WsdlPayloadOption

                GenerateAlignedParenthesizedOptionsWithMultipleIndent(node.PayloadOptions, 3);
            }
        }
    }
}
