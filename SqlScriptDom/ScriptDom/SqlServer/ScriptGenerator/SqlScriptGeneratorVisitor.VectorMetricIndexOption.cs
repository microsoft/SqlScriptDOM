//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.VectorMetricIndexOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(VectorMetricIndexOption node)
        {
            IndexOptionHelper.Instance.GenerateSourceForOption(_writer, node.OptionKind);
            GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
            GenerateSpace();
            VectorMetricTypeHelper.Instance.GenerateSourceForOption(_writer, node.MetricType);
        }
    }
}