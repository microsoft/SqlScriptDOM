//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.DropExternalResourcePoolStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(DropExternalResourcePoolStatement node)
        {
            GenerateKeyword(TSqlTokenType.Drop);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.External);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Resource);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Pool);
            GenerateSpaceAndFragmentIfNotNull(node.Name);
        }
    }
}
