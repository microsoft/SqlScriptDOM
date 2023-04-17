//------------------------------------------------------------------------------
// <copyright file="GroupByOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// The options that group by clause might have.
    /// </summary>
    public enum GroupByOption
    {
        /// <summary>
        /// Nothing defined.
        /// </summary>
        None = 0,
        /// <summary>
        /// CUBE keyword.
        /// </summary>
        Cube = 1,
        /// <summary>
        /// ROLLUP keyword.
        /// </summary>
        Rollup = 2
    }
}
