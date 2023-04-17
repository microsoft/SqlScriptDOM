//------------------------------------------------------------------------------
// <copyright file="ColumnEncryptionDefinitionParameterKind.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{

    /// <summary>
    /// Parameter types for column encryption definition
    /// </summary>        
    public enum ColumnEncryptionDefinitionParameterKind
    {
        /// <summary>
        /// Column encryption key used to encrypt column
        /// </summary>
        ColumnEncryptionKey = 0,
        /// <summary>
        /// Encryption type
        /// </summary>
        EncryptionType = 1,
        /// <summary>
        /// Algorithm used to encrypt column
        /// </summary>
        Algorithm = 2,
    }

}
