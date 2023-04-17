//------------------------------------------------------------------------------
// <copyright file="AlterServerConfigurationHadrClusterOptionHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------



namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    internal class AlterServerConfigurationHadrClusterOptionHelper : OptionsHelper<AlterServerConfigurationHadrClusterOptionKind>
    {
        private AlterServerConfigurationHadrClusterOptionHelper()
        {
            AddOptionMapping(AlterServerConfigurationHadrClusterOptionKind.Context, CodeGenerationSupporter.Context);
        }

        internal static readonly AlterServerConfigurationHadrClusterOptionHelper Instance = new AlterServerConfigurationHadrClusterOptionHelper();
    }
}