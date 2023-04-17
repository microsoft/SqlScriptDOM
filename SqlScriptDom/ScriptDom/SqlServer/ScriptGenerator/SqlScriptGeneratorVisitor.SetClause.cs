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

            GenerateKeyword(TSqlTokenType.Set);

            MarkClauseBodyAlignmentWhenNecessary(true, alignmentPoint);

            GenerateSpace();

            AlignmentPoint setItems = new AlignmentPoint();
            MarkAndPushAlignmentPoint(setItems);
            GenerateCommaSeparatedList(setClauses, _options.MultilineSetClauseItems);
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
