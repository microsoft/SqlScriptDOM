//------------------------------------------------------------------------------
// <copyright file="DatabaseConfigSetOptionKind.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------



namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The types of database configuration options that can be set
    /// </summary>   
    public enum DatabaseConfigSetOptionKind
    {
        MaxDop = 0,
        LegacyCardinalityEstimate = 1,
        ParameterSniffing = 2,
        QueryOptimizerHotFixes = 3,
        DWCompatibilityLevel = 4,
    }

#pragma warning restore 1591
}
