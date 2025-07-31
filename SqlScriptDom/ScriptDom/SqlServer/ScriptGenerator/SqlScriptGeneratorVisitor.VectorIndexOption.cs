//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.VectorIndexOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Diagnostics;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(VectorIndexOption node)
        {
            switch (node.OptionKind)
            {
                case IndexOptionKind.VectorMetric:
                    GenerateKeyword(TSqlTokenType.Identifier, CodeGenerationSupporter.Metric);
                    GenerateSpaceAndKeyword(TSqlTokenType.EqualsSign);
                    GenerateSpace();
                    VectorMetricTypeHelper.Instance.GenerateSourceFromEnum(this, node.MetricType);
                    break;
                case IndexOptionKind.VectorType:
                    GenerateKeyword(TSqlTokenType.Identifier, CodeGenerationSupporter.Type);
                    GenerateSpaceAndKeyword(TSqlTokenType.EqualsSign);
                    GenerateSpace();
                    VectorIndexTypeHelper.Instance.GenerateSourceFromEnum(this, node.VectorType);
                    break;
                default:
                    Debug.Assert(false, "An unhandled option is encountered.");
                    break;
            }
        }
    }
}