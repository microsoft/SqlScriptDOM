//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.DropFederationStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(DropFederationStatement node)
        {
            GenerateKeywordAndSpace(TSqlTokenType.Drop);
            GenerateIdentifier(CodeGenerationSupporter.Federation);

            GenerateSpaceAndFragmentIfNotNull(node.Name);
        }
    }
}
