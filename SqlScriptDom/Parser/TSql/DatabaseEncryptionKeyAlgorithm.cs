//------------------------------------------------------------------------------
// <copyright file="DatabaseEncryptionKeyAlgorithm.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// Possible algorithms for database encryption key.
    /// </summary>
    
    public enum DatabaseEncryptionKeyAlgorithm
    {
        None            = 0,
        Aes128          = 1,
        Aes192          = 2,
        Aes256          = 3,
        TripleDes3Key   = 4,
    }

#pragma warning restore 1591
}

