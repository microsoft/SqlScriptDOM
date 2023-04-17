//------------------------------------------------------------------------------
// <copyright file="UniqueRowFilter.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// Used in Select statement.
    /// </summary>
    [Serializable]
    public enum UniqueRowFilter
    {
        /// <summary>
        /// Nothing was specified.
        /// </summary>
        NotSpecified = 0,
        /// <summary>
        /// ALL keyword was used.
        /// </summary>
        All = 1,
        /// <summary>
        /// DISTINCT keyword was used.
        /// </summary>
        Distinct = 2
    }
}
