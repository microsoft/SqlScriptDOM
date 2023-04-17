//------------------------------------------------------------------------------
// <copyright file="PrivilegeType80.cs" company="Microsoft">
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
    /// The privilege types that can be used in security statements.
    /// </summary>
    [Serializable]
    public enum PrivilegeType80
    {
        All = 0,
        Select = 1,
        Insert = 2,
        Delete = 3,
        Update = 4,
        Execute = 5,
        References = 6
    }

#pragma warning restore 1591
}
