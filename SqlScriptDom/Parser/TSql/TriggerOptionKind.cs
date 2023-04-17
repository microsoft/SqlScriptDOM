//------------------------------------------------------------------------------
// <copyright file="TriggerOptionKind.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591
    /// <summary>
    /// The possible Trigger options.
    /// </summary>
    [Serializable]
    public enum TriggerOptionKind
    {
        Encryption      = 0,
        ExecuteAsClause = 1,
        NativeCompile   = 2,
        SchemaBinding   = 3,
    }

#pragma warning restore 1591
}
