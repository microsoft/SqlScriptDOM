//------------------------------------------------------------------------------
// <copyright file="EndpointProtocolOptions.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


using System;
namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The possible endpoint protocol options
    /// </summary>   
    
    [Flags]    
    public enum EndpointProtocolOptions
    {
        None                    = 0x0000,
        HttpAuthenticationRealm = 0x0001,
        HttpAuthentication      = 0x0002,
        HttpClearPort           = 0x0004,
        HttpCompression         = 0x0008,
        HttpDefaultLogonDomain  = 0x0010,
        HttpPath                = 0x0020,
        HttpPorts               = 0x0040,
        HttpSite                = 0x0080,
        HttpSslPort             = 0x0100,

        HttpOptions             = HttpAuthenticationRealm | HttpAuthentication | HttpClearPort |
            HttpCompression | HttpDefaultLogonDomain | HttpPath | HttpPorts | HttpSite | HttpSslPort,

        TcpListenerIP           = 0x0200,
        TcpListenerPort         = 0x0400,

        TcpOptions              = TcpListenerIP | TcpListenerPort
    }

#pragma warning restore 1591
}
