//------------------------------------------------------------------------------
// <copyright file="PartitionTableOptionRange.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The possible values for partition table option range
    /// </summary>            
    public enum PartitionTableOptionRange
    {
        NotSpecified = 0,
        Left = 1,
        Right = 2
    }

#pragma warning restore 1591
}
