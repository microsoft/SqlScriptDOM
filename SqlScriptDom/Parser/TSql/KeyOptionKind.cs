//------------------------------------------------------------------------------
// <copyright file="KeyOptionKind.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    public enum KeyOptionKind
    {
        KeySource           = 0,
        Algorithm           = 1,
        IdentityValue       = 2,
        ProviderKeyName     = 3,
        CreationDisposition = 4,
    }

#pragma warning restore 1591
}
