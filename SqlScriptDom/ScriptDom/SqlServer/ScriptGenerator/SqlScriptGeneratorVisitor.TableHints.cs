//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.TableHints.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        protected void GenerateWithTableHints(IList<TableHint> tableHints)
        {
            if (tableHints.Count > 0)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.With);
                GenerateSpace();
                GenerateParenthesisedCommaSeparatedList(tableHints);
            }
        }

        public override void ExplicitVisit(TableHint node)
        {
            TableHintOptionsHelper.Instance.GenerateSourceForOption(_writer, node.HintKind);
        }

        public override void ExplicitVisit(IndexTableHint node)
        {
            System.Diagnostics.Debug.Assert(node.HintKind == TableHintKind.Index);
            GenerateKeywordAndSpace(TSqlTokenType.Index);
            GenerateParenthesisedCommaSeparatedList(node.IndexValues);
        }

        public override void ExplicitVisit(LiteralTableHint node)
        {
            System.Diagnostics.Debug.Assert(node.HintKind == TableHintKind.SpatialWindowMaxCells);
            GenerateNameEqualsValue(CodeGenerationSupporter.SpatialWindowMaxCells, node.Value);
        }
        public override void ExplicitVisit(ForceSeekTableHint node)
        {
            //Hint sample: FORCESEEK(idx1(col1, col2, col3))
            System.Diagnostics.Debug.Assert(node.HintKind == TableHintKind.ForceSeek);
            GenerateIdentifier(CodeGenerationSupporter.ForceSeek);
            if (node.IndexValue != null)
            {
                GenerateSpace();
            	GenerateSymbol(TSqlTokenType.LeftParenthesis);
            	GenerateFragmentIfNotNull(node.IndexValue);
                GenerateSpace();
            	GenerateParenthesisedCommaSeparatedList(node.ColumnValues);
            	GenerateSymbol(TSqlTokenType.RightParenthesis);
            }
        }
    }
}
