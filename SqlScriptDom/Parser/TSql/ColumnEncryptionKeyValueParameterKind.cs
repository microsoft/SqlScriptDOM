//------------------------------------------------------------------------------
// <copyright file="ColumnEncryptionKeyValueParameterKind.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------



namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// Column encryption key value parameters
    /// </summary>
    public enum ColumnEncryptionKeyValueParameterKind
    {
        /// <summary>
        /// Column master key name used to encrypt the column encryption key value
        /// </summary>
        ColumnMasterKeyName = 0,
        /// <summary>
        /// Encryption algorithm used to encrypt the column encryption key value
        /// </summary>
        EncryptionAlgorithmName = 1,
        /// <summary>
        /// Encrypted value of the column encryption key
        /// </summary>
        EncryptedValue = 2,
    }
}
