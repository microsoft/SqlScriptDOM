//------------------------------------------------------------------------------
// <copyright file="VectorMetricTypeHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------



using static Microsoft.SqlServer.TransactSql.ScriptDom.SensitivityClassification;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    
    internal class VectorMetricTypeHelper : OptionsHelper<VectorMetricType>
    {
        private VectorMetricTypeHelper()
        {
            AddOptionMapping(VectorMetricType.Cosine, "'" + CodeGenerationSupporter.Cosine + "'");
            AddOptionMapping(VectorMetricType.Dot, "'" + CodeGenerationSupporter.Dot + "'");
            AddOptionMapping(VectorMetricType.Euclidean, "'" + CodeGenerationSupporter.Euclidean + "'");
        }

        public static readonly VectorMetricTypeHelper Instance = new VectorMetricTypeHelper();
    }
}