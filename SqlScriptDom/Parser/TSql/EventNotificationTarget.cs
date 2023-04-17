//------------------------------------------------------------------------------
// <copyright file="EventNotificationTarget.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The possible event notification targets.
    /// </summary>
    public enum EventNotificationTarget
    {
        Unknown = 0,
        Server = 1,
        Database = 2,
        Queue = 3,
    }

#pragma warning restore 1591
}
