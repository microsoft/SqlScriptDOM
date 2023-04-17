//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.UnaryExpression.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        protected static Dictionary<UnaryExpressionType, List<TokenGenerator>> _unaryExpressionTypeGenerators = new Dictionary<UnaryExpressionType, List<TokenGenerator>>()
        {
            { UnaryExpressionType.BitwiseNot, new List<TokenGenerator>() {
                new KeywordGenerator(TSqlTokenType.Tilde)}},

            { UnaryExpressionType.Negative, new List<TokenGenerator>() {
                new KeywordGenerator(TSqlTokenType.Minus) }},

            { UnaryExpressionType.Positive, new List<TokenGenerator>() {
                new KeywordGenerator(TSqlTokenType.Plus) }},
        };

        public override void ExplicitVisit(UnaryExpression node)
        {
            List<TokenGenerator> generators = GetValueForEnumKey(_unaryExpressionTypeGenerators, node.UnaryExpressionType);
            if (generators != null)
            {
                GenerateTokenList(generators);
            }

            // Separate multiple consecutive '-' with a space to avoid generating a T-SQL comment
            if (node.Expression is UnaryExpression &&
                node.UnaryExpressionType == UnaryExpressionType.Negative && (node.Expression as UnaryExpression).UnaryExpressionType == UnaryExpressionType.Negative)
            {
                GenerateSpace();
            }
            GenerateFragmentIfNotNull(node.Expression);
        }
    }
}
