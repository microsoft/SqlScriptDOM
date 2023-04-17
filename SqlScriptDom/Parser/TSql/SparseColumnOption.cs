//------------------------------------------------------------------------------
// <copyright file="SparseColumnOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The possible values for sparse column options
    /// </summary>            
    [Serializable]
    public enum SparseColumnOption
    {
        None = 0,
        Sparse = 1,
        ColumnSetForAllSparseColumns = 2
    }

#pragma warning restore 1591
}
