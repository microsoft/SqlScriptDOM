//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.OptimizerHints.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Collections.Generic;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        protected void GenerateOptimizerHints(IList<OptimizerHint> hints)
        {
            if (hints != null && hints.Count > 0)
            {
                NewLine();
                AlignmentPoint start = new AlignmentPoint();
                MarkAndPushAlignmentPoint(start);

                GenerateKeywordAndSpace(TSqlTokenType.Option);
                GenerateParenthesisedCommaSeparatedList(hints);

                PopAlignmentPoint();
            }
        }

        public override void ExplicitVisit(OptimizeForOptimizerHint node)
        {
            System.Diagnostics.Debug.Assert(node.HintKind == OptimizerHintKind.OptimizeFor);
            GenerateIdentifier(CodeGenerationSupporter.Optimize);
            GenerateSpaceAndKeyword(TSqlTokenType.For);

            if (node.IsForUnknown)
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Unknown);
            else
            {
                GenerateSpace();
                GenerateParenthesisedCommaSeparatedList(node.Pairs);
            }
        }

        public override void ExplicitVisit(TableHintsOptimizerHint node)
        {
            System.Diagnostics.Debug.Assert(node.HintKind == OptimizerHintKind.TableHints);
            GenerateKeywordAndSpace(TSqlTokenType.Table);
            GenerateIdentifier(CodeGenerationSupporter.Hint);
            GenerateSymbol(TSqlTokenType.LeftParenthesis);
            GenerateFragmentIfNotNull(node.ObjectName);
            if (node.TableHints.Count > 0)
            {
                GenerateSymbolAndSpace(TSqlTokenType.Comma);
                GenerateCommaSeparatedList(node.TableHints);
            }

            GenerateSymbol(TSqlTokenType.RightParenthesis);
        }

        public override void ExplicitVisit(VariableValuePair node)
        {
            GenerateFragmentIfNotNull(node.Variable);
            if (node.IsForUnknown)
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Unknown);
            else
            {
                GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
                GenerateSpaceAndFragmentIfNotNull(node.Value);
            }
        }
    }
}
