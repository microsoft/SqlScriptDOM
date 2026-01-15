//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AiGenerateResponseFunction.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        /// <summary>
        /// Emits an AI_GENERATE_RESPONSE function call like
        /// AI_GENERATE_RESPONSE(promptPart1, promptPart2),
        /// where promptPart1 and promptPart2 are expressions or identifiers
        /// representing the parts of the prompt.
        /// </summary>
        /// <param name="node">Expression node to generate</param>
        public override void ExplicitVisit(AIGenerateResponseFunctionCall node)
        {
            GenerateIdentifier(CodeGenerationSupporter.AIGenerateResponse);
            GenerateSymbol(TSqlTokenType.LeftParenthesis);

            GenerateFragmentIfNotNull(node.PromptPart1);

            if (node.PromptPart2 != null)
            {
                GenerateSymbol(TSqlTokenType.Comma);
                GenerateSpace();
                GenerateFragmentIfNotNull(node.PromptPart2);
            }

            GenerateSymbol(TSqlTokenType.RightParenthesis);
        }
    }
}
