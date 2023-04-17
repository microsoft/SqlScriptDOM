//------------------------------------------------------------------------------
// <copyright file="EncryptionAlgorithms.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------



namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// Possible algorithms for encryption
    /// </summary>
    public enum EncryptionAlgorithm
    {
        None        = 0,
        RC2         = 1,
        RC4         = 2,
        RC4_128     = 3,
        Des         = 4,
        TripleDes   = 5,
        DesX        = 6,
        Aes128      = 7,
        Aes192      = 8,
        Aes256      = 9,
        Rsa512      = 10,
        Rsa1024     = 11,
        Rsa2048     = 12,
        TripleDes3Key = 13,
        Rsa3072       = 14,
        Rsa4096       = 15
    }

#pragma warning restore 1591
}
