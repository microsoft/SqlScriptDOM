//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.PortsEndpointProtocolOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        protected static Dictionary<PortTypes, TokenGenerator> _portTypesGenerators = new Dictionary<PortTypes, TokenGenerator>()
        {
            // this is a flagged enum; we should exclude None (0), otherwise, it will show up all the time
            //{PortTypes.None, new EmptyGenerator()},
            {PortTypes.Clear, new IdentifierGenerator(CodeGenerationSupporter.Clear)},
            {PortTypes.Ssl, new IdentifierGenerator(CodeGenerationSupporter.Ssl)},
        };

        public override void ExplicitVisit(PortsEndpointProtocolOption node)
        {
            GenerateIdentifier(CodeGenerationSupporter.Ports);
            GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
            GenerateSpaceAndSymbol(TSqlTokenType.LeftParenthesis);
            GenerateCommaSeparatedFlagOpitons(_portTypesGenerators, node.PortTypes);
            GenerateSymbol(TSqlTokenType.RightParenthesis); 
        }
    }
}
