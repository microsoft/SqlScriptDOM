//------------------------------------------------------------------------------
// <copyright file="AuditOptionKind.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    public enum AuditOptionKind
    {
        QueueDelay    = 0,
        AuditGuid     = 1,
        OnFailure     = 2,
        State         = 3,
        OperatorAudit = 4
    }


#pragma warning restore 1591
}
