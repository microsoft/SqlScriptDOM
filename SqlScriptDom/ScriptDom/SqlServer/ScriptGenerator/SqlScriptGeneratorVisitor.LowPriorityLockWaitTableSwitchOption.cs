//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.LowPriorityLockWaitTableSwitchOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(LowPriorityLockWaitTableSwitchOption node)
        {
            GenerateLowPriorityWaitOptions(node.Options);
        }
    }
}
