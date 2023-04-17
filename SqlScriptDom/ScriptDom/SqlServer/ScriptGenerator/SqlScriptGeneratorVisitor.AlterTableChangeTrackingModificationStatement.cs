//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterTableChangeTrackingModificationStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AlterTableChangeTrackingModificationStatement node)
        {
            GenerateAlterTableHead(node);
            GenerateSpaceAndIdentifier(node.IsEnable ? CodeGenerationSupporter.Enable : CodeGenerationSupporter.Disable);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.ChangeTracking);

            if (node.TrackColumnsUpdated != OptionState.NotSet)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.With);
                GenerateSpaceAndSymbol(TSqlTokenType.LeftParenthesis);
                GenerateOptionStateWithEqualSign(CodeGenerationSupporter.TrackColumnsUpdated, node.TrackColumnsUpdated);
                GenerateSymbol(TSqlTokenType.RightParenthesis);
            }
        }
    }
}
