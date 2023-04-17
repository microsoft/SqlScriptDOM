//------------------------------------------------------------------------------
// <copyright file="AlterServerConfigurationFailoverClusterPropertyOptionHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------



namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    internal class AlterServerConfigurationFailoverClusterPropertyOptionHelper : OptionsHelper<AlterServerConfigurationFailoverClusterPropertyOptionKind>
    {
        private AlterServerConfigurationFailoverClusterPropertyOptionHelper()
        {
            AddOptionMapping(AlterServerConfigurationFailoverClusterPropertyOptionKind.VerboseLogging, CodeGenerationSupporter.VerboseLogging);
            AddOptionMapping(AlterServerConfigurationFailoverClusterPropertyOptionKind.SqlDumperDumpFlags, CodeGenerationSupporter.SqlDumperDumpFlags);
            AddOptionMapping(AlterServerConfigurationFailoverClusterPropertyOptionKind.SqlDumperDumpPath, CodeGenerationSupporter.SqlDumperDumpPath);
            AddOptionMapping(AlterServerConfigurationFailoverClusterPropertyOptionKind.SqlDumperDumpTimeout, CodeGenerationSupporter.SqlDumperDumpTimeout);
            AddOptionMapping(AlterServerConfigurationFailoverClusterPropertyOptionKind.FailureConditionLevel, CodeGenerationSupporter.FailureConditionLevel);
            AddOptionMapping(AlterServerConfigurationFailoverClusterPropertyOptionKind.HealthCheckTimeout, CodeGenerationSupporter.HealthCheckTimeout);
        }

        internal static readonly AlterServerConfigurationFailoverClusterPropertyOptionHelper Instance = new AlterServerConfigurationFailoverClusterPropertyOptionHelper();
    }
}