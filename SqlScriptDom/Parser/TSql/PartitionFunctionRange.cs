//------------------------------------------------------------------------------
// <copyright file="PartitionFunctionRange.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The possible values for partition function range
    /// </summary>            
    public enum PartitionFunctionRange
    {
        NotSpecified    = 0,
        Left            = 1,
        Right           = 2
    }

#pragma warning restore 1591
}
