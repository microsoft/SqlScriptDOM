//------------------------------------------------------------------------------
// <copyright file="ColumnMasterKeyParameterKind.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------



namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// Parameter types for column master key definition
    /// </summary>
    public enum  ColumnMasterKeyParameterKind
    {
        /// <summary>
        /// key store provider name where the column master key is stored
        /// </summary>
        KeyStoreProviderName = 0,
        /// <summary>
        /// key path of the column master key in the key store provider
        /// </summary>
        KeyPath = 1,
        /// <summary>
        /// True if key can be send to enclave for computations
        /// </summary>
        AllowEnclaveComputations = 2,
        /// <summary>
        /// Signature of the column master key
        /// </summary>
        Signature = 3,
    }
}
