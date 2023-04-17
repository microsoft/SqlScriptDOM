//------------------------------------------------------------------------------
// <copyright file="PageVerifyDatabaseOptionKind.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------



namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The types of page verify alter database options
    /// </summary>            
    public enum PageVerifyDatabaseOptionKind
    {
        None                = 0,
        Checksum            = 1,
        TornPageDetection   = 2,
    }

#pragma warning restore 1591
}
