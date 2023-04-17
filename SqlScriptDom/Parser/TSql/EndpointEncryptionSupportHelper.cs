//------------------------------------------------------------------------------
// <copyright file="EndpointEncryptionSupportHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    
    internal class EndpointEncryptionSupportHelper : OptionsHelper<EndpointEncryptionSupport>
    {
        private EndpointEncryptionSupportHelper()
        {
            AddOptionMapping(EndpointEncryptionSupport.Disabled, CodeGenerationSupporter.Disabled);
            AddOptionMapping(EndpointEncryptionSupport.Required, CodeGenerationSupporter.Required);
            AddOptionMapping(EndpointEncryptionSupport.Supported, CodeGenerationSupporter.Supported);
        }

        internal static readonly EndpointEncryptionSupportHelper Instance = new EndpointEncryptionSupportHelper();
    }
}
