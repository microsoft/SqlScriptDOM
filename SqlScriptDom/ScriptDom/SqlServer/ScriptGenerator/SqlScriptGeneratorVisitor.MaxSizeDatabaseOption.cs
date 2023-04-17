//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.MaxSizeDatabaseOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(MaxSizeDatabaseOption node)
        {
            System.Diagnostics.Debug.Assert(node.OptionKind == DatabaseOptionKind.MaxSize);
            GenerateNameEqualsValue(CodeGenerationSupporter.MaxSize, node.MaxSize);
            GenerateSpaceAndMemoryUnit(node.Units);
        }
    }
}
