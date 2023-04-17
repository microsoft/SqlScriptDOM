//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.OnlineIndexOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(OnlineIndexOption node)
        {
            IndexOptionHelper.Instance.GenerateSourceForOption(_writer, node.OptionKind);
            GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
            GenerateSpace();
            GenerateOptionStateOnOff(node.OptionState);
            
            if (_options.SqlVersion >= SqlVersion.Sql120 &&
                node.OptionState == OptionState.On &&
                node.LowPriorityLockWaitOption != null)
            {
                GenerateSpace();
                GenerateSymbol(TSqlTokenType.LeftParenthesis);
                GenerateFragmentIfNotNull(node.LowPriorityLockWaitOption);
                GenerateSymbol(TSqlTokenType.RightParenthesis);
            }
        }
    }
}
