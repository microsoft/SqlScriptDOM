//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.LedgerOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(LedgerOption node)
        {
            System.Diagnostics.Debug.Assert(node.OptionKind == DatabaseOptionKind.Ledger);

            if (node.OptionState == OptionState.On)
            {
                GenerateNameEqualsValue(CodeGenerationSupporter.Ledger, node.OptionState.ToString());
            }
        }
    }
}
