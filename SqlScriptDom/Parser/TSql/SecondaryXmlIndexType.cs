//------------------------------------------------------------------------------
// <copyright file="SecondaryXmlIndexType.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{

#pragma warning disable 1591

    /// <summary>
    /// The secondary xml index types.
    /// </summary>
    [Serializable]
    public enum SecondaryXmlIndexType
    {
        NotSpecified = 0,
        Path = 1,
        Property = 2,
        Value = 3,
    }

#pragma warning restore 1591
}
