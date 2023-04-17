//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.TopRowFilter.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(TopRowFilter node)
        {
            GenerateKeyword(TSqlTokenType.Top);
            GenerateSpaceAndFragmentIfNotNull(node.Expression);

            if (node.Percent)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.Percent); 
            }

            if (node.WithTies)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.With);
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Ties); 
            }
        }
    }
}
