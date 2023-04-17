//------------------------------------------------------------------------------
// <copyright file="AlterResourceGovernorCommandType.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The types of alter resource governor command
    /// </summary>   
    [Serializable]
    public enum AlterResourceGovernorCommandType
    {
        Unknown = 0,
        Disable = 1,
        Reconfigure = 2,
        ClassifierFunction = 3,
        ResetStatistics = 4,
    }

#pragma warning restore 1591
}
