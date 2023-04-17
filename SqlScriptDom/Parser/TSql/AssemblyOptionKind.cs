//------------------------------------------------------------------------------
// <copyright file="AssemblyOptionKind.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591
    
    /// <summary>
    /// The possible Assembly Options.
    /// </summary>
    public enum AssemblyOptionKind
    {
        PermissionSet   = 0,
        Visibility      = 1,
        UncheckedData   = 2,
    }

#pragma warning restore 1591
}
