//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.XmlDataType.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        protected static Dictionary<XmlDataTypeOption, TokenGenerator> _xmlDataTypeOptionGenerators = 
            new Dictionary<XmlDataTypeOption, TokenGenerator>()
        {
            { XmlDataTypeOption.Content, new IdentifierGenerator(CodeGenerationSupporter.Content)},
            { XmlDataTypeOption.Document, new IdentifierGenerator(CodeGenerationSupporter.Document)},
            { XmlDataTypeOption.None, new EmptyGenerator()},
        };

        public override void ExplicitVisit(XmlDataTypeReference node)
        {
            GenerateIdentifier(CodeGenerationSupporter.Xml);

            if (node.XmlSchemaCollection != null)
            {
                GenerateSymbol(TSqlTokenType.LeftParenthesis);

                TokenGenerator generator = GetValueForEnumKey(_xmlDataTypeOptionGenerators, node.XmlDataTypeOption);
                if (generator != null)
                {
                    GenerateToken(generator);
                }

                GenerateSpaceAndFragmentIfNotNull(node.XmlSchemaCollection);
                GenerateSymbol(TSqlTokenType.RightParenthesis);
            }
        }
    }
}
