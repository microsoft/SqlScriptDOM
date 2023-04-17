//------------------------------------------------------------------------------
// <copyright file="AuthenticationProtocols.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The types of authentication protocols
    /// </summary>   
    public enum AuthenticationProtocol
    {
        NotSpecified    = 0,
        Windows         = 1,
        WindowsNtlm     = 2,
        WindowsKerberos = 3,
        WindowsNegotiate= 4
    }


#pragma warning restore 1591
}
