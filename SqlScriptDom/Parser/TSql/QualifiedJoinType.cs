//------------------------------------------------------------------------------
// <copyright file="QualifiedJoinType.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The types of qualified joins.
    /// </summary>
    [Serializable]
    public enum QualifiedJoinType
    {
        /// <summary>
        /// INNER, or just JOIN was used.
        /// </summary>
        Inner = 0,
        /// <summary>
        /// LEFT was used.
        /// </summary>
        LeftOuter = 1,
        /// <summary>
        /// RIGHT was used.
        /// </summary>
        RightOuter = 2,
        /// <summary>
        /// FULL was used.
        /// </summary>
        FullOuter = 3
    }

#pragma warning restore 1591
}
