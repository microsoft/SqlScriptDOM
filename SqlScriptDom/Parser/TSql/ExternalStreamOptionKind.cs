//------------------------------------------------------------------------------
// <copyright file="ExternalStreamOptionKind.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// External Stream options
    /// </summary>
    public enum ExternalStreamOptionKind
    {
        /// <summary>
        /// Data source for External Stream
        /// </summary>
        DataSource = 0,
        /// <summary>
        /// File format for External Stream
        /// </summary>
        FileFormat = 1
    }
}
