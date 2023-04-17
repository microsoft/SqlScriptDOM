//------------------------------------------------------------------------------
// <copyright file="RecoveryDatabaseOptionKind.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The types of recovery alter database options
    /// </summary>            
    [Serializable]
    public enum RecoveryDatabaseOptionKind
    {
        None        = 0,
        Full        = 1,
        BulkLogged  = 2,
        Simple      = 3,
    }

#pragma warning restore 1591
}
