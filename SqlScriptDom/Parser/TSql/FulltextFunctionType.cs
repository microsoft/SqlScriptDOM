//------------------------------------------------------------------------------
// <copyright file="FulltextFunctionType.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// The fulltext function types.
    /// </summary>
    public enum FullTextFunctionType
    {
        /// <summary>
        /// Nothing was specified.
        /// </summary>
        None = 0,
        /// <summary>
        /// CONTAINS keyword.
        /// </summary>
        Contains = 1,
        /// <summary>
        /// FREETEXT keyword.
        /// </summary>
        FreeText = 2
    }
}
