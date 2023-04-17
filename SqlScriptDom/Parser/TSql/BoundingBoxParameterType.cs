//------------------------------------------------------------------------------
// <copyright file="BoundingBoxParameterType.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The types of bounding box paramter
    /// </summary>       
    public enum BoundingBoxParameterType
    {
        None = 0,
        XMin = 1,
        YMin = 2,
        XMax = 3,
        YMax = 4,
    }


#pragma warning restore 1591
}
