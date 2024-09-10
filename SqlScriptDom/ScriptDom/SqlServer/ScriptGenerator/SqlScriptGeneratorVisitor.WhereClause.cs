//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.WhereClause.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(WhereClause node)
        {
            AlignmentPoint start = new AlignmentPoint();
            MarkAndPushAlignmentPoint(start);

            GenerateKeyword(TSqlTokenType.Where);

            if (_options.NewLineAfterWhereKeyword)
            {
                NewLineAndIndent();
            }

            AlignmentPoint clauseBody = GetAlignmentPointForFragment(node, ClauseBody);

            MarkClauseBodyAlignmentWhenNecessary(_options.NewLineBeforeWhereClause, clauseBody);

            if (node.SearchCondition != null)
            {
                GenerateSpaceAndFragmentIfNotNull(node.SearchCondition);
            }
            else
            {
                GenerateSpaceAndKeyword(TSqlTokenType.Current);
                GenerateSpaceAndKeyword(TSqlTokenType.Of);
                GenerateSpaceAndFragmentIfNotNull(node.Cursor);
            }

            PopAlignmentPoint();
        }
    }
}
