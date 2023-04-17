//------------------------------------------------------------------------------
// <copyright file="SubqueryComparisonPredicateType.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
 
using System;
using System.Collections.Generic;
using System.Text;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// The types of comparison predicates that involve a subquery.
    /// </summary>
    [Serializable]
    public enum SubqueryComparisonPredicateType
    {
        /// <summary>
        /// Nothing was defined.
        /// </summary>
        None = 0,
        /// <summary>
        /// ALL was used.
        /// </summary>
        All = 1,
        /// <summary>
        /// ANY was used..
        /// </summary>
        Any = 2
    }
}
