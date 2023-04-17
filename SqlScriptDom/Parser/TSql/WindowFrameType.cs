//------------------------------------------------------------------------------
// <copyright file="WindowFrameType.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// Type of the window frame. Specifies whether ROWS or RANGE units are used.
    /// </summary>
    [Serializable]
    public enum WindowFrameType
    {
        /// <summary>
        /// Rows is used
        /// </summary>
        Rows    = 0,
        /// <summary>
        /// Range is used
        /// </summary>
        Range   = 1,
    }
}
