//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.HavingClause.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(HavingClause node)
        {
            AlignmentPoint start = new AlignmentPoint();
            MarkAndPushAlignmentPoint(start);

            GenerateKeyword(TSqlTokenType.Having);

            if (_options.NewLineAfterHavingKeyword)
            {
                NewLineAndIndent();
            }

            AlignmentPoint clauseBody = GetAlignmentPointForFragment(node, ClauseBody);
            MarkClauseBodyAlignmentWhenNecessary(_options.NewLineBeforeHavingClause, clauseBody);

            GenerateSpaceAndFragmentIfNotNull(node.SearchCondition);

            PopAlignmentPoint();
        }
    }
}
