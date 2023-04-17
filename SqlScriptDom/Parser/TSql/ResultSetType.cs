//------------------------------------------------------------------------------
// <copyright file="ResultSetType.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// Indicates the mechanism used for defining the Result Set.
    /// </summary>
    public enum ResultSetType
    {
        /// <summary>
        /// Result Set Columns are defined Inline
        /// </summary>
        Inline = 0,
        /// <summary>
        /// Result Set is defined by a Scbema Object Table, View, or Table-Valued Function
        /// </summary>
        Object = 1,
        /// <summary>
        /// Result Set is defined by a Table Type
        /// </summary>
        Type = 2,
        /// <summary>
        /// Result Set is defined by FOR XML
        /// </summary>
        ForXml = 3,
    }
}
