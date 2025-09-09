//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.OptimizedLockingAlterDatabaseOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(OptimizedLockingDatabaseOption node)
        {
            System.Diagnostics.Debug.Assert(node.OptionKind == DatabaseOptionKind.OptimizedLocking);
            GenerateIdentifier(CodeGenerationSupporter.OptimizedLocking);
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
                    break;
            }
        }
    }
}
