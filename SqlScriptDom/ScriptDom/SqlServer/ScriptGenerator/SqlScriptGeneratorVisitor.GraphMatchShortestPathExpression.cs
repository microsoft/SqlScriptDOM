//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.GraphMatchShortestPathExpression.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;
using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(GraphMatchRecursivePredicate node)
        {
            switch (node.Function)
            {
                case GraphMatchRecursivePredicateKind.ShortestPath:
                    GenerateIdentifier(CodeGenerationSupporter.ShortestPath);
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false, "unexpected option encountered");
                    break;
            }

            GenerateSymbol(TSqlTokenType.LeftParenthesis);
            if (node.AnchorOnLeft)
            {
                GenerateFragmentIfNotNull(node.OuterNodeExpression);
                GenerateSymbol(TSqlTokenType.LeftParenthesis);
                GenerateSpaceSeparatedList(node.Expression);
                GenerateSymbol(TSqlTokenType.RightParenthesis);
                GenerateFragmentIfNotNull(node.RecursiveQuantifier);
            }
            else
            {
                
                GenerateSymbol(TSqlTokenType.LeftParenthesis);
                GenerateSpaceSeparatedList(node.Expression);
                GenerateSymbol(TSqlTokenType.RightParenthesis);
                GenerateFragmentIfNotNull(node.RecursiveQuantifier);
                GenerateSpaceAndFragmentIfNotNull(node.OuterNodeExpression);
            }

            GenerateSymbol(TSqlTokenType.RightParenthesis);
        }

        public override void ExplicitVisit(GraphRecursiveMatchQuantifier node)
        {
            if (node.IsPlusSign)
            {
                GenerateSymbol(TSqlTokenType.Plus);
            }
            else
            {
                GenerateSymbol(TSqlTokenType.LeftCurly);
                GenerateFragmentIfNotNull(node.LowerLimit);
                GenerateSymbol(TSqlTokenType.Comma);
                GenerateSpaceAndFragmentIfNotNull(node.UpperLimit);
                GenerateSymbol(TSqlTokenType.RightCurly);
            }

        }
    }
}
