//------------------------------------------------------------------------------
// <copyright file="SpatialIndexingSchemeTypeHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    internal class SpatialIndexingSchemeTypeHelper : OptionsHelper<SpatialIndexingSchemeType>
    {
        private SpatialIndexingSchemeTypeHelper()
        {
            AddOptionMapping(SpatialIndexingSchemeType.GeographyGrid, CodeGenerationSupporter.GeographyGrid);
            AddOptionMapping(SpatialIndexingSchemeType.GeometryGrid, CodeGenerationSupporter.GeometryGrid);
            AddOptionMapping(SpatialIndexingSchemeType.GeographyAutoGrid, CodeGenerationSupporter.GeographyAutoGrid, SqlVersionFlags.TSql110AndAbove);
            AddOptionMapping(SpatialIndexingSchemeType.GeometryAutoGrid, CodeGenerationSupporter.GeometryAutoGrid, SqlVersionFlags.TSql110AndAbove);
        }
         
        internal static readonly SpatialIndexingSchemeTypeHelper Instance = new SpatialIndexingSchemeTypeHelper();
    }
}
