//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.TruncateTargetTableSwitchOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(TruncateTargetTableSwitchOption node)
        {
            var optionState = OptionState.On;
            if (!node.TruncateTarget)
            {
                optionState = OptionState.Off;
            }

            GenerateOptionStateWithEqualSign(CodeGenerationSupporter.TruncateTarget, optionState);
        }
    }
}
