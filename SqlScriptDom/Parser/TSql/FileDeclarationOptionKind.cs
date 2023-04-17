//------------------------------------------------------------------------------
// <copyright file="FileDeclarationOptionKind.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The possible File Declaration Options
    /// </summary>
    public enum FileDeclarationOptionKind
    {
        Name        = 0,
        NewName     = 1,
        Offline     = 2,
        FileName    = 3,
        Size        = 4,
        MaxSize     = 5,
        FileGrowth  = 6,
    }

#pragma warning restore 1591
}
