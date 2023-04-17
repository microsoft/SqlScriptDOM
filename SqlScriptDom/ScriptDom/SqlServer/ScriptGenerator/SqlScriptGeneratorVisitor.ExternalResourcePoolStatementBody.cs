//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.ExternalResourcePoolStatementBody.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        protected void GenerateExternalResourcePoolStatementBody(ExternalResourcePoolStatement node)
        {
            AlignmentPoint ap = new AlignmentPoint();

            GenerateIdentifier(CodeGenerationSupporter.External);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Resource);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Pool);

            // Pool name
            GenerateSpaceAndFragmentIfNotNull(node.Name);

            if ((node.ExternalResourcePoolParameters != null) && (node.ExternalResourcePoolParameters.Count > 0))
            {
                NewLineAndIndent();
                MarkAndPushAlignmentPoint(ap);
                GenerateKeyword(TSqlTokenType.With);
                GenerateSpace();
                GenerateAlignedParenthesizedOptionsWithMultipleIndent(node.ExternalResourcePoolParameters, 2);
                PopAlignmentPoint();
            }
        }
    }
}
