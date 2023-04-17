//------------------------------------------------------------------------------
// <copyright file="servicebrokeroptionshelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    internal class ServiceBrokerOptionsHelper : OptionsHelper<ServiceBrokerOption>
    {
        private ServiceBrokerOptionsHelper()
        {
            AddOptionMapping(ServiceBrokerOption.NewBroker, CodeGenerationSupporter.NewBroker);
            AddOptionMapping(ServiceBrokerOption.EnableBroker, CodeGenerationSupporter.EnableBroker);
            AddOptionMapping(ServiceBrokerOption.ErrorBrokerConversations, CodeGenerationSupporter.ErrorBrokerConversations);
        }

        internal static readonly ServiceBrokerOptionsHelper Instance = new ServiceBrokerOptionsHelper();
    }
}
