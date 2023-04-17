//------------------------------------------------------------------------------
// <copyright file="TriggerScope.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{

#pragma warning disable 1591

    /// <summary>
    /// This enum list possible trigger scopes 
    /// </summary>
    [Serializable]
    public enum TriggerScope
    {
        Normal = 0,
        Database = 1,
        AllServer = 2,
    }

#pragma warning restore 1591
}
