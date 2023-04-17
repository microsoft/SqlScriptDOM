//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.FilegroupOrPartitionScheme.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(FileGroupOrPartitionScheme node)
        {
            GenerateFragmentIfNotNull(node.Name);

            if (node.PartitionSchemeColumns.Count > 0)
            {
                GenerateSpace();
                GenerateParenthesisedCommaSeparatedList(node.PartitionSchemeColumns);
            }
        }
    }
}
