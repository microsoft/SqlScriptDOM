//------------------------------------------------------------------------------
// <copyright file="AlterCertificateStatementKind.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


using System;
namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The types of Alter Certificate
    /// </summary>   
    [Serializable]
    public enum AlterCertificateStatementKind
    {
        None                    = 0,
        RemovePrivateKey        = 1,
        RemoveAttestedOption    = 2,
        WithPrivateKey          = 3,
        WithActiveForBeginDialog= 4,
        AttestedBy              = 5
    }

#pragma warning restore 1591
}
