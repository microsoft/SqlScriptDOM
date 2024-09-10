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

            if (_options.NewLineAfterHavingKeyword)
            {
                NewLineAndIndent();
            }

            AlignmentPoint clauseBody = GetAlignmentPointForFragment(node, ClauseBody);
            MarkClauseBodyAlignmentWhenNecessary(_options.NewLineBeforeOrderByClause, clauseBody);

            GenerateSpace();
            GenerateCommaSeparatedList(node.OrderByElements);

            PopAlignmentPoint();
        }
    }
}
