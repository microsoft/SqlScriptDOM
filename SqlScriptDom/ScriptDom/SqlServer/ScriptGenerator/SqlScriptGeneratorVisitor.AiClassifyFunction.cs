//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AiClassifyFunction.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        /// <summary>
        /// Emits an AI_CLASSIFY function call like
        /// AI_CLASSIFY(input, label1, label2 [, ...]),
        /// where input is an expression or identifier,
        /// and labels are expressions or identifiers representing classification labels.
        /// </summary>
        /// <param name="node">Expression node to generate</param>
        public override void ExplicitVisit(AIClassifyFunctionCall node)
        {
            GenerateIdentifier(CodeGenerationSupporter.AIClassify);
            GenerateSymbol(TSqlTokenType.LeftParenthesis);

            // input
            GenerateFragmentIfNotNull(node.Input);

            // labels (require at least two via grammar)
            if (node.Labels != null)
            {
                for (int i = 0; i < node.Labels.Count; i++)
                {
                    GenerateSymbol(TSqlTokenType.Comma);
                    GenerateSpace();
                    GenerateFragmentIfNotNull(node.Labels[i]);
                }
            }

            GenerateSymbol(TSqlTokenType.RightParenthesis);
        }
    }
}
