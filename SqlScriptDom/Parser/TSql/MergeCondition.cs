//------------------------------------------------------------------------------
// <copyright file="MergeCondition.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The possible values for merge condition
    /// </summary>        
	public enum MergeCondition
	{
        NotSpecified        = 0,
        Matched             = 1,
        NotMatched          = 2,
        NotMatchedByTarget  = 3,
        NotMatchedBySource  = 4,
    }

#pragma warning restore 1591
}

