//------------------------------------------------------------------------------
// <copyright file="ExternalTableOptionKind.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The external table options.
    /// </summary>
    public enum ExternalTableOptionKind
    {
        /// <summary>
        /// The distribution policy of the external data.
        /// </summary>
        /// <remarks>
        /// Specific to Shard Map Manager based external tables.
        /// </remarks>
		Distribution = 0,

        /// <summary>
        /// The file format the external table is affiliated with.
        /// </summary>
        /// <remarks>
        /// Specific to Hadoop based external tables.
        /// </remarks>
		FileFormat = 1,

        /// <summary>
        /// The location (file name or path) of the external data.
        /// </summary>
        /// <remarks>
        /// Specific to Hadoop based external tables.
        /// </remarks>
		Location = 2,

        /// <summary>
        /// Number of rows to retrieve before recalculating the reject percentage.
        /// </summary>
        /// <remarks>
        /// Specific to Hadoop based external tables.
        /// </remarks>
        RejectSampleValue = 3,

        /// <summary>
        /// Reject policy to use when accessing external data.
        /// </summary>
        /// <remarks>
        /// Specific to Hadoop based external tables.
        /// </remarks>
        RejectType = 4,

        /// <summary>
        /// Value or percentage of rows that can be rejected before the query fails.
        /// </summary>
        /// <remarks>
        /// Specific to Hadoop based external tables.
        /// </remarks>
        RejectValue = 5,

        /// <summary>
        /// The schema of the remote object the external table is acting as a proxy for.
        /// </summary>
        /// <remarks>
        /// Specific to Shard Map Manager based external tables.
        /// </remarks>
        SchemaName = 6,

        /// <summary>
        /// The name of the remote object the external table is acting as a proxy for.
        /// </summary>
        /// <remarks>
        /// Specific to Shard Map Manager based external tables.
        /// </remarks>
        ObjectName = 7,

        /// <summary>
        /// The directory within the External Data Source where rejected rows and the corresponding error file are written.
        /// </summary>
        /// <remarks>
        /// Specific to Hadoop based external tables.
        /// </remarks>
        RejectedRowLocation = 8,
}

#pragma warning restore 1591
}
