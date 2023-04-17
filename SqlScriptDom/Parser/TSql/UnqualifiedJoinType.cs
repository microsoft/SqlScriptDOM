//------------------------------------------------------------------------------
// <copyright file="UnqualifiedJoinType.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{

#pragma warning disable 1591

    /// <summary>
    /// The possible index options.
    /// </summary>
    
    public enum UnqualifiedJoinType
    {
        CrossJoin = 0,
        CrossApply = 1,
        OuterApply = 2,
    }

#pragma warning restore 1591

}
