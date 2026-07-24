//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.SetClause.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using System.Diagnostics;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        private static Dictionary<AssignmentKind, TSqlTokenType> _assignmentKindSymbols = new Dictionary<AssignmentKind, TSqlTokenType>()
        {
            {AssignmentKind.Equals, TSqlTokenType.EqualsSign},
            {AssignmentKind.AddEquals, TSqlTokenType.AddEquals},
            {AssignmentKind.SubtractEquals, TSqlTokenType.SubtractEquals},
            {AssignmentKind.MultiplyEquals, TSqlTokenType.MultiplyEquals},
            {AssignmentKind.DivideEquals, TSqlTokenType.DivideEquals},
            {AssignmentKind.ModEquals, TSqlTokenType.ModEquals},
            {AssignmentKind.BitwiseAndEquals, TSqlTokenType.BitwiseAndEquals},
            {AssignmentKind.BitwiseOrEquals, TSqlTokenType.BitwiseOrEquals},
            {AssignmentKind.BitwiseXorEquals, TSqlTokenType.BitwiseXorEquals},
            {AssignmentKind.ConcatEquals, TSqlTokenType.ConcatEquals},
        };

        protected void GenerateSetClauses(IList<SetClause> setClauses, AlignmentPoint alignmentPoint)
        {
            NewLine();

            if (_options.IndentSetClause)
            {
                Indent();
            }

            bool leadingComma = _options.MultilineSetClauseItems && _options.CommaPlacement == CommaPlacement.Leading;

            if (!leadingComma)
            {
                // Default / trailing behavior: unchanged from the original implementation so the
                // generated output is identical when CommaPlacement is Trailing (the default).
                GenerateKeyword(TSqlTokenType.Set);

                MarkClauseBodyAlignmentWhenNecessary(true, alignmentPoint);

                GenerateSpace();

                AlignmentPoint setItems = new AlignmentPoint();
                MarkAndPushAlignmentPoint(setItems);
                GenerateCommaSeparatedList(setClauses, _options.MultilineSetClauseItems);
                PopAlignmentPoint();
                return;
            }

            // Leading comma placement (opt-in). Anchor the newlines produced between SET items at
            // the start of the SET clause (rather than at the item column). This lets leading
            // commas be right-aligned so they end just before the aligned item column, mirroring
            // the SELECT list, while the item column itself is established by the setItems
            // alignment point marked on every item. Pushing a single alignment point also keeps
            // the '=' sign alignment points (resolved by name within this scope) shared across all
            // items.
            AlignmentPoint clauseStart = new AlignmentPoint();
            MarkAndPushAlignmentPoint(clauseStart);

            GenerateKeyword(TSqlTokenType.Set);

            MarkClauseBodyAlignmentWhenNecessary(true, alignmentPoint);

            GenerateSpace();

            AlignmentPoint items = new AlignmentPoint();
            bool firstItem = true;
            foreach (SetClause setClause in setClauses)
            {
                if (firstItem)
                {
                    Mark(items);
                    firstItem = false;
                }
                else
                {
                    NewLine();
                    GenerateRightAlignedCommaSeparator();

                    // Each multi-line item starts a new line, so re-mark the item alignment point
                    // to keep continuation items aligned under the first item.
                    Mark(items);
                }

                GenerateFragmentIfNotNull(setClause);
            }

            PopAlignmentPoint();
        }

        public override void ExplicitVisit(FunctionCallSetClause node)
        {
            AlignWhenNecessary(SetClauseItemFirstEqualSign);
            GenerateFragmentIfNotNull(node.MutatorFunction);
        }

        public override void ExplicitVisit(AssignmentSetClause node)
        {
            if (node.Variable != null)
            {
                GenerateFragmentIfNotNull(node.Variable);
                AlignWhenNecessary(SetClauseItemFirstEqualSign);
            }

            if (node.Column != null && node.Variable != null)
            {
                GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
                GenerateSpace();
            }

            GenerateFragmentIfNotNull(node.Column);

            if (node.Column != null || node.Variable != null)
            {
                AlignWhenNecessary(SetClauseItemSecondEqualSign);

                TSqlTokenType symbol = GetValueForEnumKey(_assignmentKindSymbols, node.AssignmentKind);
                GenerateSpaceAndSymbol(symbol);
            }

            GenerateSpaceAndFragmentIfNotNull(node.NewValue);
        }

        private void AlignWhenNecessary(string apName)
        {
            if (_options.MultilineSetClauseItems && _options.AlignSetClauseItem)
            {
                AlignmentPoint ap = FindOrCreateAlignmentPointByName(apName);
#if !PIMODLANGUAGE
                Debug.Assert(ap != null, "Cannot obtain alignment point");
#endif
                Mark(ap);
            }
        }
    }
}
