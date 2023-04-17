//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.ValuesInsertSource.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(ValuesInsertSource node)
        {
            AlignmentPoint start = new AlignmentPoint();
            MarkAndPushAlignmentPoint(start);

            AlignmentPoint clauseBody = GetAlignmentPointForFragment(node, ClauseBody);

            if (node.IsDefaultValues)
            {
                GenerateKeyword(TSqlTokenType.Default);

                MarkClauseBodyAlignmentWhenNecessary(true, clauseBody);
                GenerateSpaceAndKeyword(TSqlTokenType.Values); 
            }
            else
            {
                GenerateKeywordAndSpace(TSqlTokenType.Values);

                MarkClauseBodyAlignmentWhenNecessary(true, clauseBody);

                AlignmentPoint insertColumns = GetAlignmentPointForFragment(node, InsertColumns);

                MarkInsertColumnsAlignmentPointWhenNecessary(insertColumns);

                GenerateCommaSeparatedList(node.RowValues, true);
            }

            PopAlignmentPoint();
        }

        public override void ExplicitVisit(RowValue node)
        {
            GenerateParenthesisedCommaSeparatedList(node.ColumnValues);
        }
    }
}
