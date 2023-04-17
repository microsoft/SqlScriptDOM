//------------------------------------------------------------------------------
// <copyright file="SequenceOptionKind.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------



namespace Microsoft.SqlServer.TransactSql.ScriptDom
{

#pragma warning disable 1591
    /// <summary>
    /// The types of Sequence options
    /// </summary>       
    public enum SequenceOptionKind
    {
        As = 0,
        MinValue = 1,
        MaxValue = 2,
        Cache = 3,
        Cycle = 4,
        Start = 5,
        Increment = 6,
        Restart = 7,
    }

#pragma warning restore 1591
}
