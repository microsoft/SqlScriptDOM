//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.KillStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(KillStatement node)
        {
            GenerateKeyword(TSqlTokenType.Kill);
            GenerateSpaceAndFragmentIfNotNull(node.Parameter);

            if (node.WithStatusOnly)
            {
                NewLineAndIndent();
                GenerateKeyword(TSqlTokenType.With);
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.StatusOnly); 
            }
        }
    }
}
