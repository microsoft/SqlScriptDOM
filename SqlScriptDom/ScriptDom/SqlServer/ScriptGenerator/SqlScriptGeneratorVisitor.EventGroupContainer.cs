//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.EventGroupContainer.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(EventGroupContainer node)
        {
            if (!AuditEventGroupHelper.Instance.TryGenerateSourceForOption(_writer, node.EventGroup))
            {
                TriggerEventGroupHelper.Instance.GenerateSourceForOption(_writer, node.EventGroup);
            }
        }
    }
}
