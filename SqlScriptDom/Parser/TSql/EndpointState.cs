//------------------------------------------------------------------------------
// <copyright file="EndpointState.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The types of endpoint state
    /// </summary>        
    public enum EndpointState
    {
        NotSpecified    = 0,
        Disabled        = 1,
        Started         = 2,
        Stopped         = 3
    }

#pragma warning restore 1591
}
