//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.JoinParenthesis.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(JoinParenthesisTableReference node)
        {
            GenerateSymbol(TSqlTokenType.LeftParenthesis);

            AlignmentPoint joinBody = new AlignmentPoint();
            MarkAndPushAlignmentPoint(joinBody);
            GenerateFragmentIfNotNull(node.Join);
            PopAlignmentPoint();

            GenerateSymbol(TSqlTokenType.RightParenthesis);
        }
    }
}
