//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.ExternalTableDistributionOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(ExternalTableDistributionOption node)
        {
            System.Diagnostics.Debug.Assert(node.OptionKind == ExternalTableOptionKind.Distribution, "ExternalTableOption does not match");
            GenerateNameEqualsValue(CodeGenerationSupporter.Distribution, node.Value);
        }
    }
}