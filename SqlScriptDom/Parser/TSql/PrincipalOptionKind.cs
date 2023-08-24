//------------------------------------------------------------------------------
// <copyright file="PrincipalOptionKind.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The types of login options
    /// </summary>    
    public enum PrincipalOptionKind
    {
        CheckExpiration = 0,
        CheckPolicy     = 1,
        Sid             = 2,
        DefaultDatabase = 3,
        DefaultLanguage = 4,
        Credential      = 5,
        Name            = 6,
        NoCredential    = 7,
        DefaultSchema   = 8,
        Login           = 9,
        Password        = 10,
        Type            = 11,
        Object_ID       = 12,
    }

#pragma warning restore 1591
}
