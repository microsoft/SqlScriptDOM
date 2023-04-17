//------------------------------------------------------------------------------
// <copyright file="EndpointEncryptionSupport.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The types of endpoint encryption support
    /// </summary>
    public enum EndpointEncryptionSupport
    {
        NotSpecified    = 0,
        Disabled        = 1,
        Supported       = 2,
        Required        = 3
    }

#pragma warning restore 1591
}
