//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AiSummarizeFunction.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        /// <summary>
        /// Emits an AI_SUMMARIZE function call like
        /// AI_SUMMARIZE(input),
        /// where input is an expression or identifier.
        /// </summary>
        /// <param name="node">Expression node to generate</param>
        public override void ExplicitVisit(AISummarizeFunctionCall node)
        {
            GenerateIdentifier(CodeGenerationSupporter.AISummarize);
            GenerateSymbol(TSqlTokenType.LeftParenthesis);
            GenerateFragmentIfNotNull(node.Input);
            GenerateSymbol(TSqlTokenType.RightParenthesis);
        }
    }
}
