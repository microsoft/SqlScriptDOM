//------------------------------------------------------------------------------
// <copyright file="ExternalDataSourceOptionKind.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The external data source options.
    /// </summary>
    public enum ExternalDataSourceOptionKind
    {
        /// <summary>
        /// The resource manager location.
        /// </summary>
        /// <remarks>
        /// Specific to Hadoop data sources.
        /// </remarks>
        ResourceManagerLocation = 0,

        /// <summary>
        /// Credential to use to connect to the data source.
        /// </summary>
        /// <remarks>
        /// Common to Hadoop and Shard Map Manager data sources.
        /// </remarks>
        Credential = 1,

        /// <summary>
        /// Name of the database to connect to.
        /// </summary>
        /// <remarks>
        /// Specific to Shard Map Manager data sources.
        /// </remarks>
        DatabaseName = 2,

        /// <summary>
        /// Name of the shard map to use.
        /// </summary>
        /// <remarks>
        /// Specific to Shard Map Manager data sources.
        /// </remarks>
        ShardMapName = 3,

        /// <summary>
        /// Additional option when connecting over ODBC to an external data source.
        /// </summary>
        /// <remarks>
        /// Specific to Polybase external data sources
        /// </remarks>
        ConnectionOptions = 4,
    }

#pragma warning restore 1591
}
