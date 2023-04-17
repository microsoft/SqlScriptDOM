//------------------------------------------------------------------------------
// <copyright file="RouteOptionKind.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{

#pragma warning disable 1591
    /// <summary>
    /// The possible route options.
    /// </summary>
    [Serializable]
    public enum RouteOptionKind
    {
        Address = 0,
        BrokerInstance = 1,
        Lifetime = 2,
        MirrorAddress = 3,
        ServiceName = 4,
    }

#pragma warning restore 1591
}
