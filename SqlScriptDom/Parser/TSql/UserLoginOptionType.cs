//------------------------------------------------------------------------------
// <copyright file="UserLoginOptionType.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{

#pragma warning disable 1591

    /// <summary>
    /// The possible user login options.
    /// </summary>
    [Serializable]
    public enum UserLoginOptionType
    {
        Login = 0,
        Certificate = 1,
        AsymmetricKey = 2,
        WithoutLogin = 3,
        External = 4,
    }

#pragma warning disable 1591

}
