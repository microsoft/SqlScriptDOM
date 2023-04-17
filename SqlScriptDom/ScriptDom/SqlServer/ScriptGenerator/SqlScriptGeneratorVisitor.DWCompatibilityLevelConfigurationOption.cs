//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.DWCompatibilityLevelConfigurationOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(DWCompatibilityLevelConfigurationOption node)
        {
            DatabaseConfigSetOptionKindHelper.Instance.GenerateSourceForOption(_writer, node.OptionKind);
            GenerateSpace();
            GenerateKeywordAndSpace(TSqlTokenType.EqualsSign);
            GenerateFragmentIfNotNull(node.Value);
        }
    }
}
