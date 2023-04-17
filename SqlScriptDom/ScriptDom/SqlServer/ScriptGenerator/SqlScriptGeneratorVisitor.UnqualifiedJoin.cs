//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.UnqualifiedJoin.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Collections.Generic;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        protected static Dictionary<UnqualifiedJoinType, List<TokenGenerator>> _unqualifiedJoinTypeGenerators = 
            new Dictionary<UnqualifiedJoinType, List<TokenGenerator>>()
        {
            { UnqualifiedJoinType.CrossApply, new List<TokenGenerator>() {
                new KeywordGenerator(TSqlTokenType.Cross, true),
                new IdentifierGenerator(CodeGenerationSupporter.Apply), }},
            { UnqualifiedJoinType.CrossJoin, new List<TokenGenerator>() {
                new KeywordGenerator(TSqlTokenType.Cross, true),
                new KeywordGenerator(TSqlTokenType.Join), }},
            { UnqualifiedJoinType.OuterApply, new List<TokenGenerator>() {
                new KeywordGenerator(TSqlTokenType.Outer, true),
                new IdentifierGenerator(CodeGenerationSupporter.Apply), }},
        };
  
        public override void ExplicitVisit(UnqualifiedJoin node)
        {
            GenerateFragmentIfNotNull(node.FirstTableReference);

            List<TokenGenerator> generators = GetValueForEnumKey(_unqualifiedJoinTypeGenerators, node.UnqualifiedJoinType);
            if (generators != null)
            {
                GenerateSpace();
                GenerateTokenList(generators);
            }

            GenerateSpaceAndFragmentIfNotNull(node.SecondTableReference);
        }
    }
}
