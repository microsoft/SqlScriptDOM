//------------------------------------------------------------------------------
// <copyright file="WindowDelimiterType.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// Type of a window boundary
    /// </summary>
    public enum WindowDelimiterType
    {
        /// <summary>
        /// All rows from the begining of a group
        /// </summary>
        UnboundedPreceding = 0,
        /// <summary>
        /// Assosiated expression determines the boundary before the current row
        /// </summary>
        ValuePreceding = 1,
        /// <summary>
        /// The current row is the boundary
        /// </summary>
        CurrentRow = 2,
        /// <summary>
        /// Assosiated expression determines the boundary after the current row
        /// </summary>
        ValueFollowing = 3,
        /// <summary>
        /// All rows to the end of a group
        /// </summary>
        UnboundedFollowing = 4,
    }
}
