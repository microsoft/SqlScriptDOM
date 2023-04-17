//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CloseSymmetricKeyStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CloseSymmetricKeyStatement node)
        {
            GenerateKeyword(TSqlTokenType.Close); 

            if (node.All)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.All);
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Symmetric);
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Keys); 
            }
            else
            {
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Symmetric);
                GenerateSpaceAndKeyword(TSqlTokenType.Key);
                GenerateSpaceAndFragmentIfNotNull(node.Name);
            }
        }
    }
}
