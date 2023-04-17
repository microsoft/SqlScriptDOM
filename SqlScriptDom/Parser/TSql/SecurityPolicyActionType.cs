//------------------------------------------------------------------------------
// <copyright file="SecurityPolicyActionType.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------



namespace Microsoft.SqlServer.TransactSql.ScriptDom
{

#pragma warning disable 1591
    /// <summary>
    /// The types of security policy statement actions
    /// </summary>
    public enum SecurityPolicyActionType
    {
        Create = 0,
        AlterPredicates = 1,
        AlterState = 2,
        AlterReplication = 3
    }

#pragma warning restore 1591
}
