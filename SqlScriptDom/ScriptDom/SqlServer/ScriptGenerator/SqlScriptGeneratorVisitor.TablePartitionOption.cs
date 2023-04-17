//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.TablePartitionOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(TablePartitionOption node)
        {
            TableOptionHelper.Instance.GenerateSourceForOption(_writer, node.OptionKind);
            GenerateKeyword(TSqlTokenType.LeftParenthesis);
            GenerateFragmentIfNotNull(node.PartitionColumn);
            GenerateFragmentIfNotNull(node.PartitionOptionSpecs);
            GenerateSymbol(TSqlTokenType.RightParenthesis);
        }

    }
}