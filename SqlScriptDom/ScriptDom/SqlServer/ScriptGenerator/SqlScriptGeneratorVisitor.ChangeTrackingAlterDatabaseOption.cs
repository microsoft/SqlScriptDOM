//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.ChangeTrackingDatabaseOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(ChangeTrackingDatabaseOption node)
        {
            System.Diagnostics.Debug.Assert(node.OptionKind == DatabaseOptionKind.ChangeTracking);
            GenerateIdentifier(CodeGenerationSupporter.ChangeTracking);
            GenerateSpace();

            switch (node.OptionState)
            {
                case OptionState.Off:
                    GenerateSymbolAndSpace(TSqlTokenType.EqualsSign);
                    GenerateKeyword(TSqlTokenType.Off);
                    break;
                case OptionState.On:
                    GenerateSymbolAndSpace(TSqlTokenType.EqualsSign);
                    GenerateKeyword(TSqlTokenType.On);
                    GenerateParenthesisedCommaSeparatedList(node.Details);
                    break;
                case OptionState.NotSet:
                default:
                    GenerateParenthesisedCommaSeparatedList(node.Details);
                    break;
            }
        }
    }
}
