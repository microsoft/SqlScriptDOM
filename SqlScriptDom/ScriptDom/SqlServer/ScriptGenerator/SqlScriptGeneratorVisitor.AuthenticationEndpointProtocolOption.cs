//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AuthenticationEndpointProtocolOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        protected static Dictionary<AuthenticationTypes, TokenGenerator> _authenticationTypesGenerators = new Dictionary<AuthenticationTypes, TokenGenerator>()
        {
            {AuthenticationTypes.Basic, new IdentifierGenerator(CodeGenerationSupporter.Basic)},
            {AuthenticationTypes.Digest, new IdentifierGenerator(CodeGenerationSupporter.Digest)},
            {AuthenticationTypes.Integrated, new IdentifierGenerator(CodeGenerationSupporter.Integrated)},
            {AuthenticationTypes.Kerberos, new IdentifierGenerator(CodeGenerationSupporter.Kerberos)},
            {AuthenticationTypes.Ntlm, new IdentifierGenerator(CodeGenerationSupporter.Ntlm)},
        };

        // AUTHENTICATION = ( { BASIC | DIGEST | NTLM | KERBEROS | INTEGRATED } [ ,...n ] )
        public override void ExplicitVisit(AuthenticationEndpointProtocolOption node)
        {
            GenerateIdentifier(CodeGenerationSupporter.Authentication);
            GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
            GenerateSpaceAndSymbol(TSqlTokenType.LeftParenthesis); 
            GenerateCommaSeparatedFlagOpitons(_authenticationTypesGenerators, node.AuthenticationTypes);
            GenerateSymbol(TSqlTokenType.RightParenthesis); 
        }
    }
}
