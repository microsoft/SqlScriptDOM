//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.DelayedDurabilityDatabaseOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(DelayedDurabilityDatabaseOption node)
        {
            GenerateIdentifier(CodeGenerationSupporter.DelayedDurability);
            GenerateSpace();
            GenerateSymbol(TSqlTokenType.EqualsSign);
            GenerateSpace();
            DelayedDurabilityOptionKindHelper.Instance.GenerateSourceForOption(_writer, node.Value);
        }
    }
}
