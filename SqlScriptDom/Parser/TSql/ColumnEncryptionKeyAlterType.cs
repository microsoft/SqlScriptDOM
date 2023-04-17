//------------------------------------------------------------------------------
// <copyright file="ColumnEncryptionKeyAlterType.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{

    /// <summary>
    /// Column encryption key alter statement types
    /// </summary>        
    [Serializable]
    public enum ColumnEncryptionKeyAlterType
    {
        /// <summary>
        /// Add column encryption key value
        /// </summary>
        Add,
        /// <summary>
        /// Drop column encryption key value
        /// </summary>
        Drop
    }

}
