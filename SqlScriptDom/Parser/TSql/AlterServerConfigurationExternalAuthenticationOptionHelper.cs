//------------------------------------------------------------------------------
// <copyright file="AlterServerConfigurationExternalAuthenticationOptionHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{

    internal class AlterServerConfigurationExternalAuthenticationOptionHelper : OptionsHelper<AlterServerConfigurationExternalAuthenticationOptionKind>
    {
        private AlterServerConfigurationExternalAuthenticationOptionHelper()
        {
            AddOptionMapping(AlterServerConfigurationExternalAuthenticationOptionKind.CredentialName, CodeGenerationSupporter.CredentialName);
            AddOptionMapping(AlterServerConfigurationExternalAuthenticationOptionKind.UseIdentity, CodeGenerationSupporter.UseIdentity);
        }

        internal static readonly AlterServerConfigurationExternalAuthenticationOptionHelper Instance = new AlterServerConfigurationExternalAuthenticationOptionHelper();
    }
}
