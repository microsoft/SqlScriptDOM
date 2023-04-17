//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.BackupStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        protected void GenerateDeviceAndOption(BackupStatement node)
        {
            if (node.Devices.Count > 0)
            {
                NewLineAndIndent();
                GenerateKeyword(TSqlTokenType.To);
                GenerateSpace();
                GenerateCommaSeparatedList(node.Devices);

                if (node.MirrorToClauses != null)
                {
                    foreach (MirrorToClause mirrorTo in node.MirrorToClauses)
                    {
                        NewLineAndIndent();
                        GenerateFragmentIfNotNull(mirrorTo);
                    }
                }
            }

            if (node.Options.Count > 0)
            {
                NewLineAndIndent();
                GenerateKeyword(TSqlTokenType.With);
                GenerateSpace();
                GenerateCommaSeparatedList(node.Options);
            }
        }
    }
}
