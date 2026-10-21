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
                GenerateKeyword(TSqlTokenType.Values);

                AlignmentPoint insertColumns = GetAlignmentPointForFragment(node, InsertColumns);
                bool moveToNewLine = ShouldMoveInsertValuesToNewLine();

                if (moveToNewLine)
                {
                    NewLineAndIndent();
                }
                else
                {
                    GenerateSpace();
                    MarkClauseBodyAlignmentWhenNecessary(true, clauseBody);
                    MarkInsertColumnsAlignmentPointWhenNecessary(insertColumns);
                }

                // insertColumns is null for a ValuesInsertSource scripted on its own (no enclosing
                // INSERT/MERGE registered the column-list point); skip all re-anchoring in that case.
                bool alignLeadingCommaRows = !moveToNewLine
                    && _options.CommaPlacement == CommaPlacement.Leading
                    && insertColumns != null;

                if (alignLeadingCommaRows)
                {
                    // A plain leading comma after NewLine() would land 2 columns past the aligned "(".
                    // Restore each continuation row to a neutral anchor, then re-mark insertColumns
                    // after a right-aligned comma so every row's "(" lines up with the first row's.
                    AlignmentPoint rowAnchor = new AlignmentPoint();
                    PushAlignmentPoint(rowAnchor);

                    bool firstRow = true;
                    foreach (RowValue rowValue in node.RowValues)
                    {
                        if (!firstRow)
                        {
                            NewLine();
                            GenerateRightAlignedCommaSeparator();
                            // insertColumns is non-null here (guaranteed by alignLeadingCommaRows).
                            Mark(insertColumns);
                        }

                        GenerateFragmentIfNotNull(rowValue);
                        firstRow = false;
                    }

                    PopAlignmentPoint();
                }
                else
                {
                    // Align continuation rows under the first row when an enclosing INSERT/MERGE
                    // registered insertColumns. For a bare source (null) we intentionally skip the
                    // push: rows fall back to the statement's start anchor, which is null-safe and
                    // avoids an unreachable branch (the non-moved bare-source path can't run in debug
                    // builds because the null clauseBody/insertColumns marks above assert first).
                    if (insertColumns != null)
                    {
                        PushAlignmentPoint(insertColumns);
                    }

                    GenerateCommaSeparatedList(node.RowValues, true, moveToNewLine);

                    if (insertColumns != null)
                    {
                        PopAlignmentPoint();
                    }
                }
            }

            PopAlignmentPoint();
        }

        public override void ExplicitVisit(RowValue node)
        {
            GenerateParenthesisedCommaSeparatedList(node.ColumnValues);
        }
    }
}
