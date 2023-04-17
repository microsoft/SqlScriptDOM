//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.ChangeRetentionChangeTrackingOptionDetail.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(ChangeRetentionChangeTrackingOptionDetail node)
        {
            GenerateIdentifier(CodeGenerationSupporter.ChangeRetention);
            GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
            GenerateSpace();
            GenerateFragmentIfNotNull(node.RetentionPeriod);
            GenerateSpace();
            RetentionUnitHelper.Instance.GenerateSourceForOption(_writer, node.Unit);
        }
    }
}
