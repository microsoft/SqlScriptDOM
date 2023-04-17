//------------------------------------------------------------------------------
// <copyright file="EndpointProtocols.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The types of endpoint protocol
    /// </summary>    
    public enum EndpointProtocol
    {
        None    = 0,
        Http    = 1,
        Tcp     = 2
    }

#pragma warning restore 1591
}
