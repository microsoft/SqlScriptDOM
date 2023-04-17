//------------------------------------------------------------------------------
// <copyright file="BooleanComparisonType.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// The types of comparison expressions
    /// </summary>
    [Serializable]
    public enum BooleanComparisonType
    {
        /// <summary>
        /// The '=' character, equal to.
        /// </summary>
        Equals = 0,
        /// <summary>
        /// The '>' character, greater than.
        /// </summary>
        GreaterThan = 1,
        /// <summary>
        /// The '&lt;' character, less than.
        /// </summary>
        LessThan = 2,
        /// <summary>
        /// The '>' '=' characters, greater than or equal to.
        /// </summary>
        GreaterThanOrEqualTo = 3,
        /// <summary>
        /// The '&lt;' '=' characters, less than or equal to.
        /// </summary>
        LessThanOrEqualTo = 4,
        /// <summary>
        /// The '&lt;' '&gt;' characters, not equal to.
        /// </summary>
        NotEqualToBrackets = 5,
        /// <summary>
        /// The '!' '=' characters, not equal to.
        /// </summary>
        NotEqualToExclamation = 6,
        /// <summary>
        /// The '!' '&lt;' characters, not less than.
        /// </summary>
        NotLessThan = 7,
        /// <summary>
        /// The '!' '&gt;' characters, not greater than.
        /// </summary>
        NotGreaterThan = 8,
        /// <summary>
        /// The '*' '=' characters, left outer join.
        /// </summary>
        LeftOuterJoin = 9,
        /// <summary>
        /// The '=' '*' characters, right outer join.
        /// </summary>
        RightOuterJoin = 10,
        /// <summary>
        /// The distinct predicate, IS DISTINCT FROM.
        /// </summary>
        IsDistinctFrom = 11,
        /// <summary>
        /// The distinct predicate, IS NOT DISTINCT FROM.
        /// </summary>
        IsNotDistinctFrom = 12,
    }
}
