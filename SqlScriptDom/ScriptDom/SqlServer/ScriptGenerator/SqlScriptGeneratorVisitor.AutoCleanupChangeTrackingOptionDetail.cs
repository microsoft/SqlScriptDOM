//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AutoCleanupChangeTrackingOptionDetail.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AutoCleanupChangeTrackingOptionDetail node)
        {
            GenerateIdentifier(CodeGenerationSupporter.AutoCleanup);
            GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
            GenerateSpaceAndKeyword(node.IsOn ? TSqlTokenType.On : TSqlTokenType.Off);
        }
    }
}
