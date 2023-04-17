//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CreateEndpointStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CreateEndpointStatement node)
        {
            GenerateKeyword(TSqlTokenType.Create);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Endpoint);

            // name
            GenerateSpaceAndFragmentIfNotNull(node.Name);

            // owner
            GenerateOwnerIfNotNull(node.Owner);

            GenerateSpace();
            GenerateEndpointBody(node);
        }
    }
}
