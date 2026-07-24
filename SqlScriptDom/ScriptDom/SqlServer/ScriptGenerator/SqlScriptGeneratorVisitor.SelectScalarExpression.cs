//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.SelectColumn.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(SelectScalarExpression node)
        {
            if (node.ColumnName != null && UseEqualsSignForColumnAlias(node))
            {
                GenerateFragmentIfNotNull(node.ColumnName);
                MarkColumnAliasEqualSignAlignmentWhenNecessary();
                GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
                GenerateSpaceAndFragmentIfNotNull(node.Expression);
            }
            else
            {
                GenerateFragmentIfNotNull(node.Expression);

                if (node.ColumnName != null)
                {
                    GenerateSpaceAndKeyword(TSqlTokenType.As);
                    GenerateSpaceAndFragmentIfNotNull(node.ColumnName);
                }
            }
        }

        // Determines whether a column alias should be rendered as "alias = expression"
        // (as opposed to "expression AS alias") based on the ColumnAliasStyle option.
        private bool UseEqualsSignForColumnAlias(SelectScalarExpression node)
        {
            // The "alias = expression" form is only valid in a SELECT projection list.
            // OUTPUT, OUTPUT INTO and RECEIVE reuse SelectScalarExpression but only accept
            // "expression AS alias", so the equals-sign form must never be emitted there.
            if (!_inSelectProjection)
            {
                return false;
            }

            switch (_options.ColumnAliasStyle)
            {
                case ColumnAliasStyle.EqualsSign:
                    return true;
                case ColumnAliasStyle.Preserve:
                    return WasColumnAliasWrittenAsEqualsSign(node);
                case ColumnAliasStyle.AsKeyword:
                default:
                    return false;
            }
        }

        // In the equals-sign form ("alias = expression") the alias appears before the
        // expression in the source, so its token index is smaller. In the AS form
        // ("expression AS alias" or "expression alias") the alias appears after the
        // expression. Fragments that were not parsed (built programmatically) have no
        // token positions and are treated as AS-keyword style.
        private static bool WasColumnAliasWrittenAsEqualsSign(SelectScalarExpression node)
        {
            // The caller only invokes this when ColumnName is set; a missing Expression is only
            // possible for a malformed, non-parsed fragment and has no equals-sign form to preserve.
            if (node?.Expression == null)
            {
                return false;
            }

            int? aliasIndex = node?.ColumnName?.FirstTokenIndex;
            int expressionIndex = node.Expression.FirstTokenIndex;

            return aliasIndex >= 0 && expressionIndex >= 0 && aliasIndex < expressionIndex;
        }
    }
}
