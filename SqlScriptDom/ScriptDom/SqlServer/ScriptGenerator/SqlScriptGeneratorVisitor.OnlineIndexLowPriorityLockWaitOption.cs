//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.OnlineIndexLowPriorityLockWaitOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(OnlineIndexLowPriorityLockWaitOption node)
        {
            GenerateLowPriorityWaitOptions(node.Options);
        }
    }
}
