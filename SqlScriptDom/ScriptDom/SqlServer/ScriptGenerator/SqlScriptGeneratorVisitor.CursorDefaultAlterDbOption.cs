//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CursorDefaultAlterDbOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CursorDefaultDatabaseOption node)
        {
            System.Diagnostics.Debug.Assert(node.OptionKind == DatabaseOptionKind.CursorDefault);
            GenerateIdentifier(CodeGenerationSupporter.CursorDefault); 
            GenerateSpaceAndIdentifier(node.IsLocal ? CodeGenerationSupporter.Local : CodeGenerationSupporter.Global);
        }
    }
}
