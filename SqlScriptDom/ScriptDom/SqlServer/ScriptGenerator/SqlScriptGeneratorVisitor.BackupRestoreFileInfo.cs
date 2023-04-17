//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.BackupRestoreFileInfo.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(BackupRestoreFileInfo node)
        {
            switch (node.ItemKind)
            {
                case BackupRestoreItemKind.ReadWriteFileGroups:
                    GenerateIdentifier(CodeGenerationSupporter.ReadWriteFilegroups); 
                    break;
                case BackupRestoreItemKind.Page:
                    if (node.Items.Count == 1)
                    {
                        GenerateIdentifier(CodeGenerationSupporter.Page);
                        GenerateSpace();
                        GenerateItems(node.Items);
                    }
                    break;
                case BackupRestoreItemKind.Files:
                    GenerateKeywordAndSpace(TSqlTokenType.File);
                    GenerateItems(node.Items);
                    break;
                case BackupRestoreItemKind.FileGroups:
                    GenerateIdentifier(CodeGenerationSupporter.Filegroup);
                    GenerateSpace();
                    GenerateItems(node.Items);
                    break;
                case BackupRestoreItemKind.None:
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false, "An unhandled option is encountered.");
                    break;
            }
        }

        private void GenerateItems(IList<ValueExpression> items)
        {
            if (items != null)
            {
                GenerateSymbolAndSpace(TSqlTokenType.EqualsSign);

                if (items.Count == 1)
                {
                    GenerateFragmentIfNotNull(items[0]);
                }
                else
                {
                    GenerateParenthesisedCommaSeparatedList(items);
                }
            }
        }
    }
}
