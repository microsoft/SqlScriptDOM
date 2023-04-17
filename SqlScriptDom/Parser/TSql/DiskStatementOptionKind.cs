//------------------------------------------------------------------------------
// <copyright file="DiskStatementOptionKind.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// Different disk statement options
    /// </summary>
    public enum DiskStatementOptionKind
    {
        /// <summary>
        /// The name of the database.
        /// </summary>
        Name        = 0,
        /// <summary>
        /// The physical name of the device.
        /// </summary>
        PhysName    = 1,
        /// <summary>
        /// The virtual device number.
        /// </summary>
        VDevNo      = 2,
        /// <summary>
        /// The amount of space.
        /// </summary>
        Size        = 3,
        /// <summary>
        /// The starting virtual address.
        /// </summary>
        VStart      = 4,
    }
}
