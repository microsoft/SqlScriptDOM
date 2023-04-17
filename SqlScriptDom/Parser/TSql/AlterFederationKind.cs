//------------------------------------------------------------------------------
// <copyright file="AlterFederationKind.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The types of alter federation statement
    /// </summary>   
    [Serializable]
    public enum AlterFederationKind
    {
        Unknown = 0,
        Split = 1,
        DropLow = 2,
        DropHigh = 3,
    }

#pragma warning restore 1591
}
