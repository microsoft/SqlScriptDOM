//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.MessageTypeStatementBase.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Collections.Generic;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        private static Dictionary<MessageValidationMethod, String> _MessageValidationMethodNames =
            new Dictionary<MessageValidationMethod, String>()
        {
            {MessageValidationMethod.Empty, CodeGenerationSupporter.Empty},
            {MessageValidationMethod.None, CodeGenerationSupporter.None},
            {MessageValidationMethod.ValidXml, CodeGenerationSupporter.ValidXml},
            {MessageValidationMethod.WellFormedXml, CodeGenerationSupporter.WellFormedXml},
        };

        protected void GenerateValidationMethod(MessageTypeStatementBase node)
        {
            if (node.ValidationMethod != MessageValidationMethod.NotSpecified)
            {
                String optionName = GetValueForEnumKey(_MessageValidationMethodNames, node.ValidationMethod);
                NewLineAndIndent();
                GenerateNameEqualsValue(CodeGenerationSupporter.Validation, optionName);

                if (node.ValidationMethod == MessageValidationMethod.ValidXml && node.XmlSchemaCollectionName != null)
                {
                    GenerateSpaceAndKeyword(TSqlTokenType.With);
                    GenerateSpaceAndKeyword(TSqlTokenType.Schema); 
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Collection);
                    GenerateSpaceAndFragmentIfNotNull(node.XmlSchemaCollectionName);
                }
            }
        }
    }
}
