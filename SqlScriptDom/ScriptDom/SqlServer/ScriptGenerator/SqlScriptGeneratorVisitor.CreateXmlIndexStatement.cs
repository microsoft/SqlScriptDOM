//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CreateXmlIndexStatement.cs" company="Microsoft">
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
        
        private static Dictionary<SecondaryXmlIndexType, String> _secondaryXmlIndexTypeNames = new Dictionary<SecondaryXmlIndexType, String>()
        {
            {SecondaryXmlIndexType.Path, CodeGenerationSupporter.Path},
            {SecondaryXmlIndexType.Property, CodeGenerationSupporter.Property},
            {SecondaryXmlIndexType.Value, CodeGenerationSupporter.Value},
        };
  
        public override void ExplicitVisit(CreateXmlIndexStatement node)
        {
            GenerateKeyword(TSqlTokenType.Create);
            if (node.Primary)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.Primary); 
            }
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Xml);
            GenerateSpaceAndKeyword(TSqlTokenType.Index);

            // name
            GenerateSpaceAndFragmentIfNotNull(node.Name);

            // ON
            NewLineAndIndent();
            GenerateKeyword(TSqlTokenType.On);
            GenerateSpaceAndFragmentIfNotNull(node.OnName);
            GenerateParenthesisedFragmentIfNotNull(node.XmlColumn);

            // USING XML INDEX
            if (node.SecondaryXmlIndexName != null)
            {
                NewLineAndIndent();
                GenerateIdentifier(CodeGenerationSupporter.Using);
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Xml);
                GenerateSpaceAndKeyword(TSqlTokenType.Index);
                GenerateSpaceAndFragmentIfNotNull(node.SecondaryXmlIndexName);

                if (node.SecondaryXmlIndexType != SecondaryXmlIndexType.NotSpecified)
                {
                    String optionName = GetValueForEnumKey(_secondaryXmlIndexTypeNames, node.SecondaryXmlIndexType);
                    GenerateSpaceAndKeyword(TSqlTokenType.For);
                    GenerateSpaceAndIdentifier(optionName); 
                }
            }

            // WITH
            if (node.IndexOptions != null && node.IndexOptions.Count > 0)
            {
                NewLineAndIndent();
                GenerateKeyword(TSqlTokenType.With);
                GenerateSpace();

                GenerateParenthesisedCommaSeparatedList(node.IndexOptions);
            }
        }
    }
}
