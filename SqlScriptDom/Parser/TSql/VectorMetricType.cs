//------------------------------------------------------------------------------
// <copyright file="VectorMetricType.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------



namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The possible values for vector index metric
    /// </summary>              
    public enum VectorMetricType
    {
        Cosine = 0,
        Dot = 1,
        Euclidean = 2
    }

#pragma warning restore 1591
}
     