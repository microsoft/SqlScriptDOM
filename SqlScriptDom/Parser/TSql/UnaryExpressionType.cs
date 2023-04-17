//------------------------------------------------------------------------------
// <copyright file="UnaryExpressionType.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// The types of scalar expressions that have a single expression as a child.
    /// </summary>
    [Serializable]
    public enum UnaryExpressionType
    {
        /// <summary>
        /// Prefix, '+' character
        /// </summary>
        Positive = 0,
        /// <summary>
        /// Prefix, '-' charachter
        /// </summary>
        Negative = 1,
        /// <summary>
        /// The '~' character
        /// </summary>
        BitwiseNot = 2,
    }
}
