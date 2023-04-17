//------------------------------------------------------------------------------
// <copyright file="ExternalFileFormatType.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The enumeration specifies the external file format types
    /// DELIMITEDTEXT, RCFILE, ORC, PARQUET.
    /// </summary>
    public enum ExternalFileFormatType
    {
        /// <summary>
        /// Delimited text file format.
        /// </summary>
        DelimitedText = 0,

        /// <summary>
        /// RCFILE file format.
        /// </summary>
        RcFile = 1,

        /// <summary>
        /// ORC file format.
        /// </summary>
        Orc = 2,

        /// <summary>
        /// Parquet file format.
        /// </summary>
        Parquet = 3,

        /// <summary>
        /// Json file format.
        /// </summary>
        JSON = 4,

        /// <summary>
        /// Delta file format.
        /// </summary>
        Delta = 5,
    }

#pragma warning restore 1591
}
