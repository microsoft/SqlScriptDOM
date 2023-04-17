//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.FileTableConstraintNameTableOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(FileTableConstraintNameTableOption node)
        {
            switch (node.OptionKind)
            {
                case TableOptionKind.FileTableFullPathUniqueConstraintName:
                    GenerateNameEqualsValue(CodeGenerationSupporter.FileTableFullPathUniqueConstraintName, node.Value);
                    break;
                case TableOptionKind.FileTablePrimaryKeyConstraintName:
                    GenerateNameEqualsValue(CodeGenerationSupporter.FileTablePrimaryKeyConstraintName, node.Value);
                    break;
                case TableOptionKind.FileTableStreamIdUniqueConstraintName:
                    GenerateNameEqualsValue(CodeGenerationSupporter.FileTableStreamIdUniqueConstraintName, node.Value);
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false, "TableOption does not match");
                    break;
            }
        }
    }
}