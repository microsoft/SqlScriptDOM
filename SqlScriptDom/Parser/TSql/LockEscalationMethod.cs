//------------------------------------------------------------------------------
// <copyright file="LockEscalationMethod.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The types of lock escalation methods
    /// </summary>        
    public enum LockEscalationMethod
    {
        Table        = 0, // SQL server default
        Auto         = 1,
        Disable      = 2
    }

#pragma warning restore 1591
}
