//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.FulltextPredicate.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        protected static Dictionary<FullTextFunctionType, TokenGenerator> _fulltextFunctionTypeGenerators = new Dictionary<FullTextFunctionType, TokenGenerator>()
        {
            // handled specially
            //{ FulltextFunctionType.None, new EmptyGenerator()},
            { FullTextFunctionType.Contains, new KeywordGenerator(TSqlTokenType.Contains)},
            { FullTextFunctionType.FreeText, new KeywordGenerator(TSqlTokenType.FreeText)},
        };

        public override void ExplicitVisit(FullTextPredicate node)
        {
            if (node.FullTextFunctionType != FullTextFunctionType.None)
            {
                TokenGenerator generator = GetValueForEnumKey(_fulltextFunctionTypeGenerators, node.FullTextFunctionType);
                if (generator != null)
                {
                    GenerateToken(generator);
                }
            }

            GenerateSpaceAndSymbol(TSqlTokenType.LeftParenthesis);
            if (node.PropertyName != null)
            {
                GenerateIdentifier(CodeGenerationSupporter.Property);
                GenerateSymbol(TSqlTokenType.LeftParenthesis);
                GenerateCommaSeparatedList(node.Columns);
                GenerateSymbol(TSqlTokenType.Comma);
                GenerateSpaceAndFragmentIfNotNull(node.PropertyName);
                GenerateSymbol(TSqlTokenType.RightParenthesis);
            }
            else
            {
                GenerateParenthesisedCommaSeparatedList(node.Columns);
            }
            GenerateSymbolAndSpace(TSqlTokenType.Comma);

            GenerateFragmentIfNotNull(node.Value);

            if (node.LanguageTerm != null)
            {
                GenerateSymbolAndSpace(TSqlTokenType.Comma);
                GenerateIdentifier(CodeGenerationSupporter.Language);
                GenerateSpaceAndFragmentIfNotNull(node.LanguageTerm);
            }

            GenerateSymbol(TSqlTokenType.RightParenthesis); 
        }
    }
}
