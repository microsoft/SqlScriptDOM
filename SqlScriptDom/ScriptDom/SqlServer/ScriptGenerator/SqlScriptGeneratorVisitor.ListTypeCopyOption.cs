//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.ListTypeCopyOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(ListTypeCopyOption node)
        {
            GenerateCommaSeparatedList(node.Options);
        }
    }
}
