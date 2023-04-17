//------------------------------------------------------------------------------
// <copyright file="AlterServerConfigurationExternalAuthenticationOptionKind.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------



namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The types of external authentication options.
    /// </summary>
    public enum AlterServerConfigurationExternalAuthenticationOptionKind
    {
        None = 0,
        OnOff = 1,
        CredentialName = 2,
        UseIdentity = 3,
    }

#pragma warning restore 1591
}
