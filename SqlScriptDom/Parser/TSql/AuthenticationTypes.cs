//------------------------------------------------------------------------------
// <copyright file="AuthenticationTypes.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


using System;
namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// Possible authentication types
    /// </summary>   
    [Flags]
    public enum AuthenticationTypes
    {
        None        = 0x0000,
        Basic       = 0x0001,
        Digest      = 0x0002,
        Integrated  = 0x0004,
        Ntlm        = 0x0008,
        Kerberos    = 0x0010
    }


#pragma warning restore 1591
}
