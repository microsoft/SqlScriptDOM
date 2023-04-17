//------------------------------------------------------------------------------
// <copyright file="AlterServerConfigurationDiagnosticsLogOptionHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------



namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    internal class AlterServerConfigurationDiagnosticsLogOptionHelper : OptionsHelper<AlterServerConfigurationDiagnosticsLogOptionKind>
    {
        private AlterServerConfigurationDiagnosticsLogOptionHelper()
        {
            AddOptionMapping(AlterServerConfigurationDiagnosticsLogOptionKind.Path, CodeGenerationSupporter.Path);
            AddOptionMapping(AlterServerConfigurationDiagnosticsLogOptionKind.MaxSize, CodeGenerationSupporter.MaxUnderscoreSize);
            AddOptionMapping(AlterServerConfigurationDiagnosticsLogOptionKind.MaxFiles, CodeGenerationSupporter.MaxFiles);
        }

        internal static readonly AlterServerConfigurationDiagnosticsLogOptionHelper Instance = new AlterServerConfigurationDiagnosticsLogOptionHelper();
    }
}