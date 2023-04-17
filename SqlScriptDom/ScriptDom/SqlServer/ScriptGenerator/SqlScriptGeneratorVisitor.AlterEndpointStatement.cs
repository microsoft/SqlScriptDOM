//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterEndpointStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AlterEndpointStatement node)
        {
            GenerateKeyword(TSqlTokenType.Alter);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Endpoint);

            // name
            GenerateSpaceAndFragmentIfNotNull(node.Name);

            GenerateSpace();
            GenerateEndpointBody(node); ;
        }
    }
}
