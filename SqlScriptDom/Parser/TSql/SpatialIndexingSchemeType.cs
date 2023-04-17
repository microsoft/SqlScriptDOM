//------------------------------------------------------------------------------
// <copyright file="SpatialIndexingSchemeType.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The types of spatial indexing scheme
    /// </summary>        
    public enum SpatialIndexingSchemeType
    {
        None = 0,
        GeometryGrid = 1,
        GeographyGrid = 2,
        GeometryAutoGrid = 3,
        GeographyAutoGrid = 4,
    }

#pragma warning restore 1591
}
