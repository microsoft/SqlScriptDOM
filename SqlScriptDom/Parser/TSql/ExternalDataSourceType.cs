//------------------------------------------------------------------------------
// <copyright file="ExternalDataSourceType.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The enumeration specifies the external data source type for external tables
    /// Currently, we support HADOOP, RDBMS, and SHARD_MAP_MANAGER. EXTERNAL_GENERICS only works 
    /// for SQL150 and DW, in DW it is called Native external data source.
    /// </summary>
    public enum ExternalDataSourceType
    {
        /// <summary>
        /// HADOOP external data source type.
        /// </summary>
        HADOOP = 0,

        /// <summary>
        /// RDBMS external data source type.
        /// </summary>
        RDBMS = 1,

        /// <summary>
        /// SHARD_MAP_MANAGER external data source type.
        /// </summary>
        SHARD_MAP_MANAGER = 2,

        /// <summary>
        ///  BLOB_STORAGE external data source type.
        /// </summary>
        BLOB_STORAGE = 3,

        /// <summary>
        ///  EXTERNAL_GENERICS external data source type.
        /// </summary>
        EXTERNAL_GENERICS = 4
    }

#pragma warning restore 1591
}
