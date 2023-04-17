//------------------------------------------------------------------------------
// <copyright file="JsonForClauseOptions.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Collections.Generic;

using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{

#pragma warning disable 1591

    /// <summary>
    /// Enum to store different JSON for clause options
    /// </summary>
    [Flags]

    [Serializable]

    public enum JsonForClauseOptions
    {
        None                = 0x0000,
            // Modes
        Auto                = 0x0001,
        Path                = 0x0002,
            // All other options
        Root                = 0x0004,
        IncludeNullValues   = 0x0008,
        WithoutArrayWrapper = 0x0010,
    }

#pragma warning restore 1591

}