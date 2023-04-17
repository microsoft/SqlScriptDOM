//------------------------------------------------------------------------------
// <copyright file="BrokerPriorityParameterSpecialType.cs" company="Microsoft">
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
    /// The types of broker priority parameters
    /// </summary>           
    [Serializable]
    public enum BrokerPriorityParameterSpecialType
    {
        None = 0,
        Any = 1,
        Default = 2,
    }


#pragma warning restore 1591
}
