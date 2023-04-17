//------------------------------------------------------------------------------
// <copyright file="DelayedDurabilityOptionKind.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// The possible delayed durability settings.
    /// </summary>
    public enum DelayedDurabilityOptionKind
    {
        /// <summary>
        /// Delayed durability specified as DISABLED
        /// </summary>
        Disabled,
        /// <summary>
        /// Delayed durability specified as ALLOWED
        /// </summary>
        Allowed,
        /// <summary>
        /// Delayed durability specified as FORCED
        /// </summary>
        Forced,
    }
}