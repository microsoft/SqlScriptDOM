//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AiTranslateFunction.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        /// <summary>
        /// Emits an AI_TRANSLATE function call like
        /// AI_TRANSLATE(input, language),
        /// where input is an expression or identifier,
        /// and language is an expression or identifier
        /// representing the target language.
        /// </summary>
        /// <param name="node">Expression node to generate</param>
        public override void ExplicitVisit(AITranslateFunctionCall node)
        {
            GenerateIdentifier(CodeGenerationSupporter.AITranslate);
            GenerateSymbol(TSqlTokenType.LeftParenthesis);
            GenerateFragmentIfNotNull(node.Input);
            GenerateSymbol(TSqlTokenType.Comma);
            GenerateSpace();
            GenerateFragmentIfNotNull(node.Language);
            GenerateSymbol(TSqlTokenType.RightParenthesis);
        }
    }
}
