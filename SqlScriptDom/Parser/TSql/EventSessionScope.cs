//------------------------------------------------------------------------------
// <copyright file="EventSessionEventRetentionModeType.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The allowed scopes of event sessions
    /// </summary>        
    [Serializable]
    public enum EventSessionScope
    {
        Server = 0,
        Database = 1
    }

#pragma warning restore 1591
}
