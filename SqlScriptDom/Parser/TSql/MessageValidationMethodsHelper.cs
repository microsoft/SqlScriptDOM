//------------------------------------------------------------------------------
// <copyright file="MessageValidationMethodsHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    
    internal class MessageValidationMethodsHelper : OptionsHelper<MessageValidationMethod>
    {
        private MessageValidationMethodsHelper()
        {
            AddOptionMapping(MessageValidationMethod.None, CodeGenerationSupporter.None);
            AddOptionMapping(MessageValidationMethod.Empty, CodeGenerationSupporter.Empty);
            AddOptionMapping(MessageValidationMethod.WellFormedXml, CodeGenerationSupporter.WellFormedXml);
            AddOptionMapping(MessageValidationMethod.ValidXml, CodeGenerationSupporter.ValidXml);
        }

        internal static readonly MessageValidationMethodsHelper Instance = new MessageValidationMethodsHelper();
    }
}
