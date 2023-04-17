//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CopyCredentialOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CopyCredentialOption node)
        {
            GenerateIdentifier(CodeGenerationSupporter.Identity);
            GenerateSymbol(TSqlTokenType.EqualsSign);
            GenerateIdentifier(node.Identity.Value);

            GenerateSymbol(TSqlTokenType.Comma);
            GenerateIdentifier(CodeGenerationSupporter.Secret);
            GenerateSymbol(TSqlTokenType.EqualsSign);
            GenerateIdentifier(node.Secret.Value);

        }
    }
}
