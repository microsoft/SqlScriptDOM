//------------------------------------------------------------------------------
// <copyright file="CertificateOptionKinds.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The types of certificate options
    /// </summary>           
    [Flags]
    public enum CertificateOptionKinds
    {
        None        = 0x000,
        Subject     = 0x001,
        StartDate   = 0x002,
        ExpiryDate  = 0x004
    }


#pragma warning restore 1591
}
