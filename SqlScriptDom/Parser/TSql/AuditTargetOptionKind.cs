//------------------------------------------------------------------------------
// <copyright file="AuditTargetOptionKind.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    public enum AuditTargetOptionKind
    {
        MaxRolloverFiles    = 0,
        FilePath            = 1,
        ReserveDiskSpace    = 2,
        MaxSize             = 3,
        MaxFiles            = 4,
        Path                = 5,
        RetentionDays       = 6,
    }


#pragma warning restore 1591
}
