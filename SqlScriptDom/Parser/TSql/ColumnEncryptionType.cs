//------------------------------------------------------------------------------
// <copyright file="ColumnEncryptionType.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{

    /// <summary>
    /// Types of encryption supported on a column.
    /// </summary>        
    [Serializable]
    public enum ColumnEncryptionType
    {
        /// <summary>
        /// Deterministic encryption.
        /// Always generates same encrypted value for a plain text.
        /// </summary>
        Deterministic = 0,
        /// <summary>
        /// Randomized encryption.
        /// Encrypts plain text in a less predictable manner.
        /// </summary>
        Randomized = 1,
    }

}
