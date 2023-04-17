//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.Permission.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(Permission node)
        {
            GenerateSpaceSeparatedList(node.Identifiers);

            if (node.Columns != null && node.Columns.Count > 0)
            {
                GenerateSpace();
                GenerateParenthesisedCommaSeparatedList(node.Columns);
            }
        }
    }
}
