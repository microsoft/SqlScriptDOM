//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.DatabaseOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(DatabaseOption node)
        {
            SimpleDbOptionsHelper.Instance.GenerateSourceForOption(_writer, node.OptionKind);
        }
    }
}
