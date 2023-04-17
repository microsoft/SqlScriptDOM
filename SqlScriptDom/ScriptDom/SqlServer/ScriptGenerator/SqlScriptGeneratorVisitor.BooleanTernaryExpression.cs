//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.TernaryExpression.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Collections.Generic;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{

    partial class SqlScriptGeneratorVisitor
    {

        protected static Dictionary<BooleanTernaryExpressionType, List<TokenGenerator>> _ternaryExpressionTypeGenerators =
            new Dictionary<BooleanTernaryExpressionType, List<TokenGenerator>>()
        {
            {BooleanTernaryExpressionType.Between, new List<TokenGenerator>() {
                new KeywordGenerator(TSqlTokenType.Between) }},
            {BooleanTernaryExpressionType.NotBetween, new List<TokenGenerator>() {
                new KeywordGenerator(TSqlTokenType.Not, true),
                new KeywordGenerator(TSqlTokenType.Between) }},
        };

        public override void ExplicitVisit(BooleanTernaryExpression node)
        {
            GenerateFragmentIfNotNull(node.FirstExpression);

            List<TokenGenerator> generators = GetValueForEnumKey(_ternaryExpressionTypeGenerators, node.TernaryExpressionType);
            if (generators != null)
            {
                GenerateSpace();
                GenerateTokenList(generators);
            }

            GenerateSpaceAndFragmentIfNotNull(node.SecondExpression);
            GenerateSpaceAndKeyword(TSqlTokenType.And);
            GenerateSpaceAndFragmentIfNotNull(node.ThirdExpression);
        }
    }
}
