//------------------------------------------------------------------------------
// <copyright file="AlterFulltextCatalogActions.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The types of alter fulltext catalog action
    /// </summary>   
    public enum AlterFullTextCatalogAction
    {
        None        = 0,
        Rebuild     = 1,
        Reorganize  = 2,
        AsDefault   = 3
    }

#pragma warning restore 1591
}
