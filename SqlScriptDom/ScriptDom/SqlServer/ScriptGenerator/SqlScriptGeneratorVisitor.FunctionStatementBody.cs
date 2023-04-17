//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.FunctionStatementBody.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        protected static Dictionary<FunctionOptionKind, List<TokenGenerator>> _functionOptionsGenerators =
            new Dictionary<FunctionOptionKind, List<TokenGenerator>>()
        {
            { FunctionOptionKind.CalledOnNullInput, new List<TokenGenerator>() {
                new IdentifierGenerator(CodeGenerationSupporter.Called, true),
                new KeywordGenerator(TSqlTokenType.On, true),
                new KeywordGenerator(TSqlTokenType.Null, true),
                new IdentifierGenerator(CodeGenerationSupporter.Input),
                }},
            { FunctionOptionKind.Encryption, new List<TokenGenerator>() {
                new IdentifierGenerator(CodeGenerationSupporter.Encryption),
                }},
            // handled specially
            //{ FunctionOptions.None, new List<TokenGenerator>() {
            //    }},
            { FunctionOptionKind.ReturnsNullOnNullInput, new List<TokenGenerator>() {
                new IdentifierGenerator(CodeGenerationSupporter.Returns, true),
                new KeywordGenerator(TSqlTokenType.Null, true),
                new KeywordGenerator(TSqlTokenType.On, true),
                new KeywordGenerator(TSqlTokenType.Null, true),
                new IdentifierGenerator(CodeGenerationSupporter.Input),
                }},
            { FunctionOptionKind.SchemaBinding, new List<TokenGenerator>() {
                new IdentifierGenerator(CodeGenerationSupporter.SchemaBinding),
                }},
            { FunctionOptionKind.NativeCompilation, new List<TokenGenerator>() {
                new IdentifierGenerator(CodeGenerationSupporter.NativeCompilation),
                }},
            { FunctionOptionKind.Inline, new List<TokenGenerator>() {
                new IdentifierGenerator(CodeGenerationSupporter.Inline),
                }},
        };
  
        protected void GenerateFunctionStatementBody(FunctionStatementBody node)
        {
            GenerateKeyword(TSqlTokenType.Function);

            // name
            GenerateSpaceAndFragmentIfNotNull(node.Name);

            // parameters
            NewLine();
            GenerateParenthesisedCommaSeparatedList(node.Parameters);
            if (node.Parameters == null || node.Parameters.Count == 0)
            {
                GenerateSymbol(TSqlTokenType.LeftParenthesis);
                GenerateSpaceAndSymbol(TSqlTokenType.RightParenthesis);
            }

            // RETURNS
            NewLine();
            GenerateIdentifier(CodeGenerationSupporter.Returns);
            GenerateSpace();

            // SelectFunctionReturn also holds function body (which is single select expression)
            SelectFunctionReturnType selectReturn = node.ReturnType as SelectFunctionReturnType;
            if (selectReturn != null)
            {
                GenerateKeywordAndSpace(TSqlTokenType.Table);
            }
            else
            {
                // could be
                //      ScalarFunctionReturnType
                //      SelectFunctionReturnType
                //      TableValuedFunctionReturnType
                GenerateFragmentIfNotNull(node.ReturnType);
            }
            GenerateCommaSeparatedWithClause(node.Options, false, false);
            
            if (node.OrderHint != null)
            {
                NewLine();
                GenerateFragmentIfNotNull(node.OrderHint);
            }
            
            NewLine();
            GenerateKeyword(TSqlTokenType.As);
            NewLine();

            if (selectReturn != null)
            {
                GenerateKeywordAndSpace(TSqlTokenType.Return);
                GenerateFragmentIfNotNull(selectReturn);
            }
            else
            {
                if (node.MethodSpecifier != null)
                    GenerateSpaceAndFragmentIfNotNull(node.MethodSpecifier);
                else if (node.StatementList != null)
                    GenerateFragmentIfNotNull(node.StatementList);
            }
        }

        public override void ExplicitVisit(FunctionOption node)
        {
            List<TokenGenerator> generators = GetValueForEnumKey(_functionOptionsGenerators, node.OptionKind);
            if (generators != null)
            {
                GenerateTokenList(generators);
            }
        }

        public override void ExplicitVisit(ExecuteAsFunctionOption node)
        {
            GenerateFragmentIfNotNull(node.ExecuteAs);
        }
    }
}
