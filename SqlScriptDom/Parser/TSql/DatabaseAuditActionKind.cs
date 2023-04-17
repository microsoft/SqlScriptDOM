//------------------------------------------------------------------------------
// <copyright file="DatabaseAuditActionKind.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The types of database audit action
    /// </summary>               
    public enum DatabaseAuditActionKind
    {
        Select = 0,
        Update = 1,
        Insert = 2,
        Delete = 3,
        Execute = 4,
        Receive = 5,
        References = 6,
    }

#pragma warning restore 1591
}
