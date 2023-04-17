//------------------------------------------------------------------------------
// <copyright file="SignableElementKind.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


using System;
namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The types of signable elements
    /// </summary>        
    [Serializable]
    public enum SignableElementKind
    {
        NotSpecified        = 0,
        Object              = 1,
        Assembly            = 2,
        Database            = 3,
    }

#pragma warning restore 1591
}
