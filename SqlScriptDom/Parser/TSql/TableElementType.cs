//------------------------------------------------------------------------------
// <copyright file="TableElementType.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
 
using System;
using System.Collections.Generic;
using System.Text;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// Table element type.
    /// </summary>
    [Serializable]
    public enum TableElementType
    {
        /// <summary>
        /// Nothing was specified.
        /// </summary>
        NotSpecified = 0,
        /// <summary>
        /// The element is a constraint.
        /// </summary>
        Constraint = 1,
        /// <summary>
        /// The element is a column.
        /// </summary>
        Column = 2,
        /// <summary>
        /// The element is an index.
        /// </summary>
        Index = 3,
        /// <summary>
        /// The element is a period.
        /// </summary>
        Period = 4,
    }
}
