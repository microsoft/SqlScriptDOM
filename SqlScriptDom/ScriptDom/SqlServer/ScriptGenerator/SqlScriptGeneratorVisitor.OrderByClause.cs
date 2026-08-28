//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.OrderByClause.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(OrderByClause node)
        {
            AlignmentPoint start = new AlignmentPoint();
            MarkAndPushAlignmentPoint(start);

            GenerateKeyword(TSqlTokenType.Order);
            GenerateSpaceAndKeyword(TSqlTokenType.By);

            AlignmentPoint clauseBody = GetAlignmentPointForFragment(node, ClauseBody);

            if (node.All)
            {
                // ORDER BY ALL shorthand: orders by every column in the select list.
                // ALL is a keyword modifier of the clause (like GROUP BY ALL), so keep it on
                // the same line as ORDER BY. The NewLineBeforeOrderByClause option only applies
                // to an explicit column list, not to the ALL shorthand.
                GenerateSpace();
                GenerateKeyword(TSqlTokenType.All);

                TokenGenerator sortOrderGenerator = GetValueForEnumKey(_sortOrderGenerators, node.AllSortOrder);
                if (sortOrderGenerator != null && node.AllSortOrder != SortOrder.NotSpecified)
                {
                    GenerateSpace();
                    GenerateToken(sortOrderGenerator);
                }
            }
            else
            {
                if (!GenerateClauseBodyStart(_options.NewLineBeforeOrderByClause, clauseBody))
                {
                    GenerateSpace();
                }

                if (_options.MultilineOrderByElementsList)
                {
                    GenerateAlignedMultilineCommaSeparatedList(node.OrderByElements);
                }
                else
                {
                    GenerateCommaSeparatedList(node.OrderByElements);
                }
            }

            PopAlignmentPoint();
        }
    }
}
