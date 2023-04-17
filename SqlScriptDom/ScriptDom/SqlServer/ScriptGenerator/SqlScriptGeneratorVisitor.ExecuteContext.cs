//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.ExecuteContext.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        private static Dictionary<ExecuteAsOption, TokenGenerator> _executeAsOptionGenerators = new Dictionary<ExecuteAsOption, TokenGenerator>()
        {
            {ExecuteAsOption.Caller, new IdentifierGenerator(CodeGenerationSupporter.Caller) },
            {ExecuteAsOption.Login, new IdentifierGenerator(CodeGenerationSupporter.Login) },
            {ExecuteAsOption.Owner, new IdentifierGenerator(CodeGenerationSupporter.Owner) },
            {ExecuteAsOption.Self, new IdentifierGenerator(CodeGenerationSupporter.Self) },
            // TODO, yangg: what should we do with this option?
            //{ExecuteAsOption.String, },
            {ExecuteAsOption.User, new KeywordGenerator(TSqlTokenType.User) },
        };
  
        public override void ExplicitVisit(ExecuteContext node)
        {
            GenerateKeywordAndSpace(TSqlTokenType.As);
            TokenGenerator generator = GetValueForEnumKey(_executeAsOptionGenerators, node.Kind);

            if (node.Principal != null)
            {
                GenerateNameEqualsValue(generator, node.Principal);
            }
            else
            {
                GenerateToken(generator);
            }
        }
    }
}
