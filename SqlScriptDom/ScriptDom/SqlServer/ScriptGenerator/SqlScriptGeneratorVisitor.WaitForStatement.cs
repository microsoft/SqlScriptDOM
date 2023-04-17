//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.WaitForStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        protected static Dictionary<WaitForOption, TokenGenerator> _waitForOptionGenerators = new Dictionary<WaitForOption, TokenGenerator>()
        {
            {WaitForOption.Delay, new IdentifierGenerator(CodeGenerationSupporter.Delay)},
            // handled specially
            //{WaitForOption.Statement, new KeywordGenerator(TSqlTokenTypes)},
            {WaitForOption.Time, new IdentifierGenerator(CodeGenerationSupporter.Time)},
        };

        public override void ExplicitVisit(WaitForStatement node)
        {
            GenerateKeyword(TSqlTokenType.WaitFor); 

            if (node.WaitForOption == WaitForOption.Statement)
            {
                GenerateSpace();

                // to avoid generate semicolon for the included statement
                Boolean originalValue = _generateSemiColon;
                _generateSemiColon = false;

                GenerateParenthesisedFragmentIfNotNull(node.Statement);

                // restore the original value
                _generateSemiColon = originalValue;

                if (node.Timeout != null)
                {
                    GenerateSymbolAndSpace(TSqlTokenType.Comma);
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Timeout);
                    GenerateSpaceAndFragmentIfNotNull(node.Timeout);
                }
            }
            else
            {
                TokenGenerator generator = GetValueForEnumKey(_waitForOptionGenerators, node.WaitForOption);
                if (generator != null)
                {
                    GenerateSpace();
                    GenerateToken(generator);
                }

                GenerateSpaceAndFragmentIfNotNull(node.Parameter);
            }
        }
    }
}
