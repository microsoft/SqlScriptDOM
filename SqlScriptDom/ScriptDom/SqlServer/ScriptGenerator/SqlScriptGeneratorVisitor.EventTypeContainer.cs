//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.EventTypeContainer.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(EventTypeContainer node)
        {
            if (!AuditEventTypeHelper.Instance.TryGenerateSourceForOption(_writer, node.EventType))
            {
                TriggerEventTypeHelper.Instance.GenerateSourceForOption(_writer, node.EventType);
            }
        }
    }
}
