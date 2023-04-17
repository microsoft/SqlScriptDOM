//------------------------------------------------------------------------------
// <copyright file="FipsComplianceLevel.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// Fips 127-2 compliance level
    /// </summary>
    public enum FipsComplianceLevel
    {
        Off         = 0,
        Entry       = 1,
        Intermediate= 2,
        Full        = 3,
    }

#pragma warning restore 1591
}
