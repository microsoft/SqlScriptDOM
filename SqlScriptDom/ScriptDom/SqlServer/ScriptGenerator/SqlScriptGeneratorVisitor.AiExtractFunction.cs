//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AiExtractFunction.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        /// <summary>
        /// Emits an AI_EXTRACT function call like
        /// AI_EXTRACT(input, label1 [, ...]),
        /// where input is an expression or identifier,
        /// and labels are expressions or identifiers
        /// representing extractible entities.
        /// </summary>
        /// <param name="node">Expression node to generate</param>
        public override void ExplicitVisit(AIExtractFunctionCall node)
        {
            GenerateIdentifier(CodeGenerationSupporter.AIExtract);
            GenerateSymbol(TSqlTokenType.LeftParenthesis);

            // input
            GenerateFragmentIfNotNull(node.Input);

            // labels
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
