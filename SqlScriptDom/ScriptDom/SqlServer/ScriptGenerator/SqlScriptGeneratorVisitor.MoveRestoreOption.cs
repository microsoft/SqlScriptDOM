//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.MoveRestoreOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(MoveRestoreOption node)
        {
            System.Diagnostics.Debug.Assert(node.OptionKind == RestoreOptionKind.Move);
            GenerateIdentifier(CodeGenerationSupporter.Move);
            GenerateSpaceAndFragmentIfNotNull(node.LogicalFileName);
            GenerateSpaceAndKeyword(TSqlTokenType.To);
            GenerateSpaceAndFragmentIfNotNull(node.OSFileName);
        }
    }
}
