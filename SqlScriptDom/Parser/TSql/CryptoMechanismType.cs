//------------------------------------------------------------------------------
// <copyright file="CryptoMechanismType.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The types of crypto mechanism
    /// </summary>               
    public enum CryptoMechanismType
    {
        Certificate = 0,
        AsymmetricKey = 1,
        SymmetricKey = 2,
        Password = 3,
    }

#pragma warning restore 1591
}
