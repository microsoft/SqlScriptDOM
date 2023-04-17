//------------------------------------------------------------------------------
// <copyright file="AuditTargetKind.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The possible audit target kinds
    /// </summary>
    
    public enum AuditTargetKind
    {
        File = 0,
        ApplicationLog = 1,
        SecurityLog = 2,

        //150 level
        Url = 3,
        ExternalMonitor = 4,
    }


#pragma warning restore 1591
}
