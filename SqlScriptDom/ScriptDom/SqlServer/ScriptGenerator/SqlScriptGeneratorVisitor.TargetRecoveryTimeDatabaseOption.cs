//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.TargetRecoveryTimeDatabaseOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(TargetRecoveryTimeDatabaseOption node)
        {
            System.Diagnostics.Debug.Assert(node.OptionKind == DatabaseOptionKind.TargetRecoveryTime);
            GenerateIdentifier(CodeGenerationSupporter.TargetRecoveryTime);
            GenerateSpace();
            GenerateSymbolAndSpace(TSqlTokenType.EqualsSign);
            GenerateFragmentIfNotNull(node.RecoveryTime);
            GenerateSpace();
            TargetRecoveryTimeUnitHelper.Instance.GenerateSourceForOption(_writer, node.Unit);
        }
    }
}
