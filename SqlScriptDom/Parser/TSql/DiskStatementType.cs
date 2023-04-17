//------------------------------------------------------------------------------
// <copyright file="DiskStatementType.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// The disk statement type.
    /// </summary>
    public enum DiskStatementType
    {
        /// <summary>
        /// Init.
        /// </summary>
        Init    = 0,
        /// <summary>
        /// Resize.
        /// </summary>
        Resize  = 1,
    }
}
