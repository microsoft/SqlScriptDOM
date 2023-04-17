//------------------------------------------------------------------------------
// <copyright file="ContainmentOptionKind.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// The possible containment settings.
    /// </summary>
    public enum ContainmentOptionKind
    {
        /// <summary>
        /// Containment specified as NONE
        /// </summary>
        None,
        /// <summary>
        /// Containment specified as PARTIAL
        /// </summary>
        Partial,
    }
}
