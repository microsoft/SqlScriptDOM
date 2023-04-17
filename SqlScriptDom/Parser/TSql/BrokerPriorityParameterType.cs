//------------------------------------------------------------------------------
// <copyright file="BrokerPriorityParameterType.cs" company="Microsoft">
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
    public enum BrokerPriorityParameterType
    {
        Unknown = 0,
        ContractName = 1,
        LocalServiceName = 2,
        RemoteServiceName = 3,
        PriorityLevel = 4,
    }


#pragma warning restore 1591
}
