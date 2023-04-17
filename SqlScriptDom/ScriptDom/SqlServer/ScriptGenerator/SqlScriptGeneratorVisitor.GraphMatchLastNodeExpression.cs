//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.GraphMatchLastNodeExpression.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;
using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(GraphMatchLastNodePredicate node)
        {
            GenerateFragmentIfNotNull(node.LeftExpression);
            GenerateSpace();
            GenerateSymbol(TSqlTokenType.EqualsSign);
            GenerateSpace();
            GenerateFragmentIfNotNull(node.RightExpression);
        }

        public override void ExplicitVisit(GraphMatchNodeExpression node)
        {
            if (node.UsesLastNode)
            {
                GenerateIdentifier(CodeGenerationSupporter.LastNode);
                GenerateSymbol(TSqlTokenType.LeftParenthesis);
                GenerateFragmentIfNotNull(node.Node);
                GenerateSymbol(TSqlTokenType.RightParenthesis);
            }
            else
            {
                GenerateFragmentIfNotNull(node.Node);
            }
        }

        public override void ExplicitVisit(GraphMatchCompositeExpression node)
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
