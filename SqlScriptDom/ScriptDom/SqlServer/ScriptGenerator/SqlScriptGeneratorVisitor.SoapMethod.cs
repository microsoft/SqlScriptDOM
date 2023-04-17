//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.SoapMethod.cs" company="Microsoft">
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
        protected static Dictionary<SoapMethodAction, TokenGenerator> _soapMethodActionGenerators = new Dictionary<SoapMethodAction, TokenGenerator>()
        {
            {SoapMethodAction.None, new EmptyGenerator()},
            {SoapMethodAction.Add, new KeywordGenerator(TSqlTokenType.Add)},
            {SoapMethodAction.Alter, new KeywordGenerator(TSqlTokenType.Alter)},
            {SoapMethodAction.Drop, new KeywordGenerator(TSqlTokenType.Drop)},
        };
  
        protected static Dictionary<SoapMethodSchemas, TokenGenerator> _soapMethodSchemasGenerators = new Dictionary<SoapMethodSchemas, TokenGenerator>()
        {
            {SoapMethodSchemas.Default, new KeywordGenerator(TSqlTokenType.Default)},
            {SoapMethodSchemas.None, new IdentifierGenerator(CodeGenerationSupporter.None)},
            {SoapMethodSchemas.Standard, new IdentifierGenerator(CodeGenerationSupporter.Standard)},
        };
  
        
        protected static Dictionary<SoapMethodFormat, String> _soapMethodFormatNames = new Dictionary<SoapMethodFormat, String>()
        {
            {SoapMethodFormat.AllResults, CodeGenerationSupporter.AllResults},
            {SoapMethodFormat.None, CodeGenerationSupporter.None},
            {SoapMethodFormat.RowsetsOnly, CodeGenerationSupporter.RowsetsOnly},
        };
  
        

        //[ { ADD WEBMETHOD [ 'namespace' .] 'method_alias' 
        //(   NAME = 'database.owner.name'
        //    [ , SCHEMA = {NONE | STANDARD | DEFAULT } ]
        //    [ , FORMAT = { ALL_RESULTS | ROWSETS_ONLY } ]
        //)  
        //} [ ,...n ] ]
        
        //[ { ALTER WEBMETHOD [ 'namespace' .] 'method_alias' 
        //(   NAME = 'database.owner.name'
        //    [ , SCHEMA = {NONE | STANDARD | DEFAULT} ]
        //    [ , FORMAT = { ALL_RESULTS | ROWSETS_ONLY } ]
        //  )  
        //} [ ,...n] ]

        //[ { DROP WEBMETHOD [ 'namespace' .] 'method_alias' } [ ,...n ] ]

        public override void ExplicitVisit(SoapMethod node)
        {
            // ADD | ALTER | DROP
            TokenGenerator generator = GetValueForEnumKey(_soapMethodActionGenerators, node.Action);
            if (generator != null)
            {
                GenerateToken(generator);
            }

            // WEBMETHOD
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.WebMethod); 

            // namespace.
            if (node.Namespace != null)
            {
                GenerateSpaceAndFragmentIfNotNull(node.Namespace);
                GenerateSymbol(TSqlTokenType.Dot);
            }
            else
            {
                //  Needed to separate WEBMETHOD from method name.
                GenerateSpace();
            }

            // method_alias
            GenerateFragmentIfNotNull(node.Alias);

            if (node.Action != SoapMethodAction.Drop)
            {
                GenerateSymbol(TSqlTokenType.LeftParenthesis);

                // NAME = 'database.owner.name'
                if (node.Name != null)
                {
                    GenerateNameEqualsValue(CodeGenerationSupporter.Name, node.Name);
                }

                // SCHEMA = {NONE | STANDARD | DEFAULT} 
                if (node.Schema != SoapMethodSchemas.NotSpecified)
                {
                    GenerateSymbolAndSpace(TSqlTokenType.Comma);

                    GenerateTokenAndEqualSign(TSqlTokenType.Schema);

                    TokenGenerator schemaGenerator = GetValueForEnumKey(_soapMethodSchemasGenerators, node.Schema);
                    if (schemaGenerator != null)
                    {
                        GenerateSpace();
                        GenerateToken(schemaGenerator);
                    }
                }

                // FORMAT = { ALL_RESULTS | ROWSETS_ONLY }
                if (node.Format != SoapMethodFormat.NotSpecified)
                {
                    GenerateSymbolAndSpace(TSqlTokenType.Comma);

                    String optionName = GetValueForEnumKey(_soapMethodFormatNames, node.Format);
                    if (optionName != null)
                    {
                        GenerateNameEqualsValue(CodeGenerationSupporter.Format, optionName);
                    }
                }

                GenerateSymbol(TSqlTokenType.RightParenthesis); 
            }
        }
    }
}
