//------------------------------------------------------------------------------
// <copyright file="BrokerPriorityParameterHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    
    [Serializable]
    internal class BrokerPriorityParameterHelper : OptionsHelper<BrokerPriorityParameterType>
    {
        private BrokerPriorityParameterHelper()
        {
            AddOptionMapping(BrokerPriorityParameterType.ContractName, CodeGenerationSupporter.ContractName);
            AddOptionMapping(BrokerPriorityParameterType.LocalServiceName, CodeGenerationSupporter.LocalServiceName);
            AddOptionMapping(BrokerPriorityParameterType.PriorityLevel, CodeGenerationSupporter.PriorityLevel);
            AddOptionMapping(BrokerPriorityParameterType.RemoteServiceName, CodeGenerationSupporter.RemoteServiceName);
        }

        internal static readonly BrokerPriorityParameterHelper Instance = new BrokerPriorityParameterHelper();
    }
}
