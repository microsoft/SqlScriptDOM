//------------------------------------------------------------------------------
// <copyright file="AlterServerConfigurationFailoverClusterPropertyOptionKind.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------



namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The types of failover cluster properties.
    /// </summary>       
    public enum AlterServerConfigurationFailoverClusterPropertyOptionKind
    {
        None = 0,
        VerboseLogging = 1,
        SqlDumperDumpFlags = 2,
        SqlDumperDumpPath = 3,
        SqlDumperDumpTimeout = 4,
        FailureConditionLevel = 5,
        HealthCheckTimeout = 6,
    }

#pragma warning restore 1591
}
