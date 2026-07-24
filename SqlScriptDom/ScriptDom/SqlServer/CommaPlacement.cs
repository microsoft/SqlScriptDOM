//------------------------------------------------------------------------------
// <copyright file="CommaPlacement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// Represents the placement of commas when a comma-separated list is written on multiple lines.
    /// </summary>
    public enum CommaPlacement
    {
        /// <summary>
        /// The comma is placed at the end of the line, after the item that precedes it.
        /// </summary>
        Trailing,

        /// <summary>
        /// The comma is placed at the start of the line, before the item that follows it.
        /// When this value is used, the <see cref="IndentationMode"/> setting is ignored and
        /// indentation always uses spaces.
        /// </summary>
        Leading
    }
}
