//------------------------------------------------------------------------------
// <copyright file="EnableDisableOptionType.cs" company="Microsoft">
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
    /// The types of enable disable options
    /// </summary>               
    [Serializable]
    public enum EnableDisableOptionType
    {
        None = 0,
        Enable = 1,
        Disable = 2,
    }

#pragma warning restore 1591
}
