//------------------------------------------------------------------------------
// <copyright file="DatabaseConfigurationOptionState.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// State of some database configuration options
    /// </summary>
    public enum DatabaseConfigurationOptionState
    {
        NotSet = 0,
        On = 1,
        Off = 2,
        Primary = 3
    }

#pragma warning restore 1591
}
