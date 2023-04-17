//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.OpenSymmetricKeyStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(OpenSymmetricKeyStatement node)
        {
            GenerateKeyword(TSqlTokenType.Open);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Symmetric);
            GenerateSpaceAndKeyword(TSqlTokenType.Key);

            // name
            GenerateSpaceAndFragmentIfNotNull(node.Name);

            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Decryption);
            GenerateSpaceAndKeyword(TSqlTokenType.By);
            GenerateSpaceAndFragmentIfNotNull(node.DecryptionMechanism);
        }
    }
}
