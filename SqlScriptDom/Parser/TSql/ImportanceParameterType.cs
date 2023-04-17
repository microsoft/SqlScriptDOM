//------------------------------------------------------------------------------
// <copyright file="ImportanceParameterType.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The possible values for importance parameter in CREATE/ALTER WORKLOAD GROUP and CREATE SPATIAL INDEX statements
    /// </summary>
    [Serializable]
    public enum ImportanceParameterType
    {
        Unknown = 0,
        Low = 1,
        Medium = 2,
        High = 3,
        Normal = 4,
        Above_Normal = 5,
        Below_Normal = 6,
    }

#pragma warning restore 1591
}
