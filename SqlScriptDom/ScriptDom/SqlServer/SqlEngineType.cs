//------------------------------------------------------------------------------
// <copyright file="SqlEngineType.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// This enum lists the engine type of SQL Server
    /// </summary>
    public enum SqlEngineType
    {
        /// <summary>
        /// All engine versions
        /// </summary>
        All = 0,

        /// <summary>
        /// SQL Server box
        /// </summary>
        Standalone = 1,

        /// <summary>
        /// SQL Azure Database
        /// </summary>
        SqlAzure = 2,
	}
}