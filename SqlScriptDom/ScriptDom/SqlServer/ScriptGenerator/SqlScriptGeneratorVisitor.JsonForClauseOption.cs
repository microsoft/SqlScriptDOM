//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.JsonForClauseOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Collections.Generic;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        protected static Dictionary<JsonForClauseOptions, TokenGenerator> _jsonForClauseOptionsGenerators =
            new Dictionary<JsonForClauseOptions, TokenGenerator>()
        {
            { JsonForClauseOptions.Auto, new IdentifierGenerator(CodeGenerationSupporter.Auto) },

            { JsonForClauseOptions.Path, new IdentifierGenerator(CodeGenerationSupporter.Path) },

            { JsonForClauseOptions.Root, new IdentifierGenerator(CodeGenerationSupporter.Root) },

            { JsonForClauseOptions.IncludeNullValues, new IdentifierGenerator(CodeGenerationSupporter.IncludeNullValues) },

            { JsonForClauseOptions.WithoutArrayWrapper, new IdentifierGenerator(CodeGenerationSupporter.WithoutArrayWrapper) },

        };

        public override void ExplicitVisit(JsonForClauseOption node)
        {
            TokenGenerator generator = GetValueForEnumKey(_jsonForClauseOptionsGenerators, node.OptionKind);
            if (generator != null)
            {
                GenerateToken(generator);
            }

            if (node.Value != null)
            {
                GenerateSpace();
                GenerateParenthesisedFragmentIfNotNull(node.Value);
            }
        }
    }
}
