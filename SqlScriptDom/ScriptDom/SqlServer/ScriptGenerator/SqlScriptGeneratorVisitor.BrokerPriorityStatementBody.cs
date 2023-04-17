//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.BrokerPriorityStatementBody.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        protected void GenerateBrokerPriorityStatementBody(BrokerPriorityStatement node)
        {
            AlignmentPoint ap = new AlignmentPoint();

            GenerateIdentifier(CodeGenerationSupporter.Broker);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Priority);

            // name
            GenerateSpaceAndFragmentIfNotNull(node.Name);

            GenerateSpaceAndKeyword(TSqlTokenType.For);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Conversation);

            if ((node.BrokerPriorityParameters != null) && (node.BrokerPriorityParameters.Count > 0))
            {
                NewLineAndIndent();
                MarkAndPushAlignmentPoint(ap);

                GenerateKeyword(TSqlTokenType.Set);
                GenerateSpace();
                GenerateAlignedParenthesizedOptionsWithMultipleIndent(node.BrokerPriorityParameters, 2);
                PopAlignmentPoint();
            }
        }
    }
}
