//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.BeginTransactionStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(BeginTransactionStatement node)
        {
            GenerateKeyword(TSqlTokenType.Begin);

            if (node.Distributed)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.Distributed); 
            }

            GenerateSpaceAndKeyword(TSqlTokenType.Transaction); 

            if (node.Name != null)
            {
                GenerateSpace();
                // TODO, yangg: why node.Name is an object? rather than TSqlFragment?
                GenerateTransactionName(node.Name);
            }

            if (node.MarkDefined)
            {
                NewLineAndIndent();
                GenerateKeyword(TSqlTokenType.With);
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Mark);
                GenerateSpaceAndFragmentIfNotNull(node.MarkDescription);
            }
        }
    }
}
