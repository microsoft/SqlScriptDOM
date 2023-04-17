//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.DbccNamedLiteral.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(DbccNamedLiteral node)
        {
            if (node.Name != null)
            {
                GenerateNameEqualsValue(node.Name, node.Value);
            }
            else
            {
                GenerateFragmentIfNotNull(node.Value);
            }
        }
    }
}
