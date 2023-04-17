//------------------------------------------------------------------------------
// <copyright file="DataCompressionLevel.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------



namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The possible values for data compression level
    /// </summary>              
    public enum DataCompressionLevel
    {
        None = 0,
        Row  = 1,
        Page = 2,
        ColumnStore = 3,
        ColumnStoreArchive = 4
    }

#pragma warning restore 1591
}
     
