//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.OrderIndexOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(OrderIndexOption node)
        {
            IndexOptionHelper.Instance.GenerateSourceForOption(_writer, node.OptionKind);
            GenerateSpace();
            GenerateSymbol(TSqlTokenType.LeftParenthesis);
            GenerateCommaSeparatedList(node.Columns);
            GenerateSymbol(TSqlTokenType.RightParenthesis);
        }
    }
}
