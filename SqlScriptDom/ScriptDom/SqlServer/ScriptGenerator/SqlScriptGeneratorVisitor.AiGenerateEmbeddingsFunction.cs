//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AIGenerateEmbeddingsFunction.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AIGenerateEmbeddingsFunctionCall node)
        {
            if (node.Input == null || node.ModelName == null)
            {
                return;
            }

            // Emit function name without extra space before '('
            GenerateIdentifierWithoutCasing(CodeGenerationSupporter.AIGenerateEmbeddings);
            GenerateSymbol(TSqlTokenType.LeftParenthesis);

            // Emit input expression
            GenerateFragmentIfNotNull(node.Input);

            // Emit " USE MODEL" with explicit spaces
            GenerateSpace();
            GenerateKeyword(TSqlTokenType.Use);
            GenerateSpace();
            GenerateIdentifierWithoutCasing(CodeGenerationSupporter.Model);
            GenerateSpaceAndFragmentIfNotNull(node.ModelName);

            // Emit optional PARAMETERS block
            if (node.OptionalParameters != null)
            {
                GenerateSpace();
                GenerateIdentifierWithoutCasing(CodeGenerationSupporter.Parameters);
                GenerateSpaceAndSymbol(TSqlTokenType.LeftParenthesis);
                GenerateFragmentIfNotNull(node.OptionalParameters);
                GenerateSymbol(TSqlTokenType.RightParenthesis);
            }

            // Emit closing parenthesis
            GenerateSymbol(TSqlTokenType.RightParenthesis);
        }
    }
}
