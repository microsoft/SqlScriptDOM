//------------------------------------------------------------------------------
// <copyright file="EncryptionAlgorithmPreference.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// Possible values for encryption algorithm preference.
    /// </summary>    
    public enum EncryptionAlgorithmPreference
    {
        NotSpecified    = 0,
        Aes             = 1,
        Rc4             = 2
    }

#pragma warning restore 1591
}
