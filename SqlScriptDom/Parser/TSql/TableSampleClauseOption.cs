//------------------------------------------------------------------------------
// <copyright file="TableSampleClauseOption.cs" company="Microsoft">
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
    /// The table sample clause options.
    /// </summary>
    public enum TableSampleClauseOption
    {
        NotSpecified = 0,
        Percent = 1,
        Rows = 2,
    }

#pragma warning restore 1591
}
