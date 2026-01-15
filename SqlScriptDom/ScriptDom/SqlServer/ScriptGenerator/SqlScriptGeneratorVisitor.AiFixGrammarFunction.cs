//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AiFixGrammarFunction.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        /// <summary>
        /// Emits an AI_ANALYZE_SENTIMENT function call like
        /// AI_ANALYZE_SENTIMENT(input), where input is an expression or identifier.
        /// </summary>
        /// <param name="node">Expression node to generate</param>
        public override void ExplicitVisit(AIFixGrammarFunctionCall node)
        {
            GenerateIdentifier(CodeGenerationSupporter.AIFixGrammar);
            GenerateSymbol(TSqlTokenType.LeftParenthesis);
            GenerateFragmentIfNotNull(node.Input);
            GenerateSymbol(TSqlTokenType.RightParenthesis);
        }
    }
}
