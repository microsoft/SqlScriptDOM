//------------------------------------------------------------------------------
// <copyright file="SubDmlFlags.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{

#pragma warning disable 1591
    /// <summary>
    /// Used to keep track of various Sub DML statement options inside Parser
    /// </summary>
    [Flags]
    internal enum SubDmlFlags
    {
        None                = 0x0,
        InsideSubDml        = 0x1,
        SelectNotForInsert  = 0x2,
        MergeUsing          = 0x4,
        UpdateDeleteFrom    = 0x8
    }

#pragma warning restore 1591
}
