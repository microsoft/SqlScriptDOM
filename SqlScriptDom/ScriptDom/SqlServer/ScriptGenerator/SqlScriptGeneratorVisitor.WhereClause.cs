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

            AlignmentPoint clauseBody = GetAlignmentPointForFragment(node, ClauseBody);

            bool indented = GenerateClauseBodyStart(_options.NewLineBeforeWhereClause, clauseBody);

            if (node.SearchCondition != null)
            {
                if (indented)
                {
                    GenerateFragmentIfNotNull(node.SearchCondition);
                }
                else
                {
                    GenerateSpaceAndFragmentIfNotNull(node.SearchCondition);
                }
            }
            else
            {
                if (indented)
                {
                    GenerateKeyword(TSqlTokenType.Current);
                }
                else
                {
                    GenerateSpaceAndKeyword(TSqlTokenType.Current);
                }
                GenerateSpaceAndKeyword(TSqlTokenType.Of);
                GenerateSpaceAndFragmentIfNotNull(node.Cursor);
            }

            PopAlignmentPoint();
        }
    }
}
