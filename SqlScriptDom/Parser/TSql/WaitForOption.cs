//------------------------------------------------------------------------------
// <copyright file="WaitForOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{

#pragma warning disable 1591

    /// <summary>
    /// The possible waitfor options.
    /// </summary>
    [Serializable]
    public enum WaitForOption
    {
        Delay = 0,
        Time = 1,
        Statement = 2,
    }

#pragma warning restore 1591

}
