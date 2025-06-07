//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterTableAddClusterByStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AlterTableAddClusterByStatement node)
        {
            AlignmentPoint start = new AlignmentPoint();
            MarkAndPushAlignmentPoint(start);

            GenerateAlterTableHead(node);

            NewLineAndIndent();

            GenerateKeyword(TSqlTokenType.Add);
            GenerateSpace();
            if (node.ClusterByOption != null)
            {
                GenerateSymbol(TSqlTokenType.LeftParenthesis);
                GenerateFragmentIfNotNull(node.ClusterByOption);
                GenerateSymbol(TSqlTokenType.RightParenthesis);
            }

            PopAlignmentPoint();
        }
    }
}
