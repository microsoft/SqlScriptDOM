//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.StopRestoreOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(StopRestoreOption node)
        {
            System.Diagnostics.Debug.Assert(node.OptionKind == RestoreOptionKind.Stop || node.OptionKind == RestoreOptionKind.StopAt);
            GenerateNameEqualsValue(
                node.IsStopAt 
                    ? CodeGenerationSupporter.StopAtMark 
                    : CodeGenerationSupporter.StopBeforeMark,
                node.Mark);

            if (node.After != null)
            {
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.After);
                GenerateSpaceAndFragmentIfNotNull(node.After);
            }
        }
    }
}
