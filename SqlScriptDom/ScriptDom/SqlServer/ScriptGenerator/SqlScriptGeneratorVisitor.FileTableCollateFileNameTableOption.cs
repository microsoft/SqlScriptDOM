//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.FileTableCollateFileNameTableOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(FileTableCollateFileNameTableOption node)
        {
            System.Diagnostics.Debug.Assert(node.OptionKind == TableOptionKind.FileTableCollateFileName, "TableOption does not match");
            // The collation value (e.g. database_default) is a keyword Identifier fragment; do not
            // bracket or recase it.
            GenerateWithoutIdentifierFormatting(() => GenerateNameEqualsValue(CodeGenerationSupporter.FileTableCollateFileName, node.Value));
        }
    }
}