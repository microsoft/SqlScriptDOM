//------------------------------------------------------------------------------
// <copyright file="FetchOrientation.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// Possible values for fetch orientation
    /// </summary>        
    public enum FetchOrientation
    {
        None    = 0,
        First   = 1,
        Next    = 2,
        Prior   = 3,
        Last    = 4,
        Relative= 5,
        Absolute= 6,
    }

#pragma warning restore 1591
}
