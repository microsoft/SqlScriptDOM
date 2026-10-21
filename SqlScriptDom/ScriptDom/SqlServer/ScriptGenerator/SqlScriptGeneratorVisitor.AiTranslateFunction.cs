//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AiTranslateFunction.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
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
            if (!TryGenerateBuiltInFunctionName(CodeGenerationSupporter.AITranslate))
            {
                GenerateIdentifier(CodeGenerationSupporter.AITranslate);
            }
            GenerateSymbol(TSqlTokenType.LeftParenthesis);
            GenerateFragmentIfNotNull(node.Input);
            GenerateSymbol(TSqlTokenType.Comma);
            GenerateSpace();
            GenerateFragmentIfNotNull(node.Language);
            GenerateSpaceAndFragmentIfNotNull(node.OnErrorClause);
            GenerateSymbol(TSqlTokenType.RightParenthesis);
        }

        /// <summary>
        /// Emits ERROR ON ERROR behavior for AI functions.
        /// </summary>
        /// <param name="node">The error behavior clause to generate.</param>
        public override void ExplicitVisit(AIErrorOnErrorClause node)
        {
            GenerateIdentifier(CodeGenerationSupporter.Error);
            GenerateSpaceAndKeyword(TSqlTokenType.On);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Error);
        }

        /// <summary>
        /// Emits NULL ON ERROR behavior for AI functions.
        /// </summary>
        /// <param name="node">The null behavior clause to generate.</param>
        public override void ExplicitVisit(AINullOnErrorClause node)
        {
            GenerateKeyword(TSqlTokenType.Null);
            GenerateSpaceAndKeyword(TSqlTokenType.On);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Error);
        }

        /// <summary>
        /// Emits DEFAULT 'literal' ON ERROR behavior for AI functions.
        /// </summary>
        /// <param name="node">The default behavior clause to generate.</param>
        public override void ExplicitVisit(AIDefaultOnErrorClause node)
        {
            if (node.Value == null)
            {
                throw new InvalidOperationException("AIDefaultOnErrorClause.Value is required.");
            }

            GenerateKeyword(TSqlTokenType.Default);
            GenerateSpaceAndFragmentIfNotNull(node.Value);
            GenerateSpaceAndKeyword(TSqlTokenType.On);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Error);
        }
    }
}
