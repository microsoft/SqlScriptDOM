//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.ComputeClause.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(ComputeClause node)
        {
            AlignmentPoint start = new AlignmentPoint();
            MarkAndPushAlignmentPoint(start);

            GenerateKeyword(TSqlTokenType.Compute);

            AlignmentPoint clauseBody = GetAlignmentPointForFragment(node, ClauseBody);
            MarkClauseBodyAlignmentWhenNecessary(true, clauseBody);

            GenerateSpace();
            GenerateCommaSeparatedList(node.ComputeFunctions);

            if (node.ByExpressions.Count > 0)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.By); 

                GenerateSpace();
                GenerateCommaSeparatedList(node.ByExpressions);
            }

            PopAlignmentPoint();
        }
    }
}
