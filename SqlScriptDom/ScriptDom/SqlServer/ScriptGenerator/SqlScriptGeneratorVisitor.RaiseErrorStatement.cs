//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.RaiseErrorStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        protected static Dictionary<RaiseErrorOptions, TokenGenerator> _raiseErrorOptionsGenerators = new Dictionary<RaiseErrorOptions, TokenGenerator>()
        {
            { RaiseErrorOptions.Log, new IdentifierGenerator(CodeGenerationSupporter.Log)},
            { RaiseErrorOptions.NoWait, new IdentifierGenerator(CodeGenerationSupporter.NoWait)},
            { RaiseErrorOptions.SetError, new IdentifierGenerator(CodeGenerationSupporter.SetError)},
        };
  
        public override void ExplicitVisit(RaiseErrorStatement node)
        {
            GenerateKeyword(TSqlTokenType.Raiserror);

            GenerateSpace();
            GenerateSymbol(TSqlTokenType.LeftParenthesis);

            GenerateFragmentIfNotNull(node.FirstParameter);
            GenerateSymbol(TSqlTokenType.Comma);  

            GenerateSpaceAndFragmentIfNotNull(node.SecondParameter);
            GenerateSymbol(TSqlTokenType.Comma);

            GenerateSpaceAndFragmentIfNotNull(node.ThirdParameter);

            if (node.OptionalParameters.Count > 0)
            {
                GenerateSymbolAndSpace(TSqlTokenType.Comma);
                GenerateCommaSeparatedList(node.OptionalParameters);
            }

            GenerateSymbol(TSqlTokenType.RightParenthesis); 
	
            if (node.RaiseErrorOptions != RaiseErrorOptions.None)
            {
                NewLineAndIndent();
                GenerateKeywordAndSpace(TSqlTokenType.With);

                GenerateCommaSeparatedFlagOpitons(_raiseErrorOptionsGenerators, node.RaiseErrorOptions);
            }
        }
    }
}
