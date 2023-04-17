//------------------------------------------------------------------------------
// <copyright file="ChangeTrackingOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The possible change tracking options.
    /// </summary>
    public enum ChangeTrackingOption
    {
        NotSpecified = 0,
        Auto = 1,
        Manual = 2,
        Off = 3,
        OffNoPopulation = 4,
    }


#pragma warning restore 1591
}
