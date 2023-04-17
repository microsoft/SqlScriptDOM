//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.OpenMasterKeyStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(OpenMasterKeyStatement node)
        {
            GenerateKeyword(TSqlTokenType.Open);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Master);
            GenerateSpaceAndKeyword(TSqlTokenType.Key);

            GenerateSpace();
            GenerateDecryptionByPassword(node.Password);
        }
    }
}
