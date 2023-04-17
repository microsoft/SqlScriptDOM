//------------------------------------------------------------------------------
// <copyright file="EndpointTypesHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    
    internal class EndpointTypesHelper : OptionsHelper<EndpointType>
    {
        private EndpointTypesHelper()
        {
            AddOptionMapping(EndpointType.DatabaseMirroring, CodeGenerationSupporter.DatabaseMirroring);
            AddOptionMapping(EndpointType.Soap, CodeGenerationSupporter.Soap);
            AddOptionMapping(EndpointType.ServiceBroker, CodeGenerationSupporter.ServiceBroker);
            AddOptionMapping(EndpointType.TSql, CodeGenerationSupporter.TSql);
        }
        internal static readonly EndpointTypesHelper Instance = new EndpointTypesHelper();
    }
}
