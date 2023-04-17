//------------------------------------------------------------------------------
// <copyright file="CatalogCollation.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// Values for the CATALOG_COLLATION database option
    /// </summary>
    public enum CatalogCollation
    {
        Database_Default = 0,
        Latin1_General_100_CI_AS_KS_WS_SC = 1,
        SQL_Latin1_General_CP1_CI_AS = 2
    }

#pragma warning restore 1591
}
