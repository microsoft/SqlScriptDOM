//------------------------------------------------------------------------------
// <copyright file="SortOrder.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
 
using System;
using System.Collections.Generic;
using System.Text;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// This enum is the possibilities for a sort order.
    /// </summary>
    [Serializable]
    public enum SortOrder
    {
        /// <summary>
        /// Nothing was specified.
        /// </summary>
        NotSpecified = 0,
        /// <summary>
        /// Ascending
        /// </summary>
        Ascending = 1,
        /// <summary>
        /// Descending
        /// </summary>
        Descending = 2
    }
}
