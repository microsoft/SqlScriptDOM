//------------------------------------------------------------------------------
// <copyright file="QuoteType.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The possible quote type.
    /// </summary>
    [Serializable]
    public enum QuoteType
    {
        NotQuoted = 0,
        SquareBracket = 1,
        DoubleQuote = 2,
    }

#pragma warning restore 1591
}
