//------------------------------------------------------------------------------
// <copyright file="ComputeFunctionType.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// The possible types for compute function of compute clause.
    /// </summary>
    public enum ComputeFunctionType
    {
        /// <summary>
        /// Nothing was specified.
        /// </summary>
        NotSpecified = 0,
        /// <summary>
        /// AVG keyword.
        /// </summary>
        Avg = 1,
        /// <summary>
        /// COUNT keyword.
        /// </summary>
        Count = 2,
        /// <summary>
        /// MAX keyword.
        /// </summary>
        Max = 3,
        /// <summary>
        /// MIN keyword.
        /// </summary>
        Min = 4,
        /// <summary>
        /// STDEV keyword.
        /// </summary>
        Stdev = 5,
        /// <summary>
        /// STDEVP keyword.
        /// </summary>
        Stdevp = 6,
        /// <summary>
        /// VAR keyword.
        /// </summary>
        Var = 7,
        /// <summary>
        /// VARP keyword.
        /// </summary>
        Varp = 8,
        /// <summary>
        /// SUM keyword.
        /// </summary>
        Sum = 9,
        /// <summary>
        /// COUNT_BIG keyword.
        /// </summary>
        CountBig = 10,
        /// <summary>
        /// CHECKSUM_AGG keyword.
        /// </summary>
        ChecksumAgg = 11,
        /// <summary>
        /// MODULAR_SUM keyword.
        /// </summary>
        ModularSum = 12
    }
}
