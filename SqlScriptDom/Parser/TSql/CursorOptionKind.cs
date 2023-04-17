//------------------------------------------------------------------------------
// <copyright file="CursorOptionKind.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// Different cursor options
    /// </summary>
    public enum CursorOptionKind
    {
        Local           = 0,
        Global          = 1,
        Scroll          = 2,
        ForwardOnly     = 3,
        Insensitive     = 4,
        Keyset          = 5,
        Dynamic         = 6,
        FastForward     = 7,
        ScrollLocks     = 8,
        Optimistic      = 9,
        ReadOnly        = 10,
        Static          = 11,
        TypeWarning     = 12,
    }

#pragma warning restore 1591
}
