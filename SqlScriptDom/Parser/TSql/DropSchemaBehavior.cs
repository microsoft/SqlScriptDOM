//------------------------------------------------------------------------------
// <copyright file="DropSchemaBehavior.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// DROP SCHEMA behavior (in case of SQL 2000)
    /// </summary>
    public enum DropSchemaBehavior
    {
        None = 0,
        Cascade = 1,
        Restrict = 2,
    }

#pragma warning restore 1591
}
