//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.GraphMatchExpression.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;
using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(GraphMatchPredicate node)
        {
            GenerateIdentifier(CodeGenerationSupporter.GraphMatch);

            GenerateSymbol(TSqlTokenType.LeftParenthesis);

            GenerateFragmentIfNotNull(node.Expression);

            GenerateSymbol(TSqlTokenType.RightParenthesis);
        }

        public override void ExplicitVisit(GraphMatchExpression node)
        {
            GenerateFragmentIfNotNull(node.LeftNode);

            if (!node.ArrowOnRight)
            {
                GenerateSymbol(TSqlTokenType.LessThan);
            }
            GenerateSymbol(TSqlTokenType.Minus);
            GenerateSymbol(TSqlTokenType.LeftParenthesis);

            GenerateFragmentIfNotNull(node.Edge);

            GenerateSymbol(TSqlTokenType.RightParenthesis);
            GenerateSymbol(TSqlTokenType.Minus);
            if (node.ArrowOnRight)
            {
                GenerateSymbol(TSqlTokenType.GreaterThan);
            }

            GenerateFragmentIfNotNull(node.RightNode);
        }
    }
}
