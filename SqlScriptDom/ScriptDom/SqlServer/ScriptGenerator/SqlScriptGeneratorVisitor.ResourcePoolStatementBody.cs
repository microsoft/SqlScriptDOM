//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.ResourcePoolStatementBody.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        protected void GenerateResourcePoolStatementBody(ResourcePoolStatement node)
        {
            AlignmentPoint ap = new AlignmentPoint();

            GenerateIdentifier(CodeGenerationSupporter.Resource);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Pool);

            // name
            GenerateSpaceAndFragmentIfNotNull(node.Name);

            if ((node.ResourcePoolParameters != null) && (node.ResourcePoolParameters.Count > 0))
            {
                NewLineAndIndent();
                MarkAndPushAlignmentPoint(ap);
                GenerateKeyword(TSqlTokenType.With);
                GenerateSpace();
                GenerateAlignedParenthesizedOptionsWithMultipleIndent(node.ResourcePoolParameters, 2);
                PopAlignmentPoint();
            }
        }
    }
}
