//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.QualifiedJoin.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        protected static Dictionary<QualifiedJoinType, List<TokenGenerator>> _qualifiedJoinTypeGenerators =
            new Dictionary<QualifiedJoinType, List<TokenGenerator>>()
        {
            { QualifiedJoinType.FullOuter, new List<TokenGenerator>() {
                new KeywordGenerator(TSqlTokenType.Full, true),
                new KeywordGenerator(TSqlTokenType.Outer),
                }},
            { QualifiedJoinType.Inner, new List<TokenGenerator>() {
                new KeywordGenerator(TSqlTokenType.Inner),
                }},
            { QualifiedJoinType.LeftOuter, new List<TokenGenerator>() {
                new KeywordGenerator(TSqlTokenType.Left, true),
                new KeywordGenerator(TSqlTokenType.Outer),
                }},
            { QualifiedJoinType.RightOuter, new List<TokenGenerator>() {
                new KeywordGenerator(TSqlTokenType.Right, true),
                new KeywordGenerator(TSqlTokenType.Outer),
                }},
        };

        public override void ExplicitVisit(QualifiedJoin node)
        {
            GenerateFragmentIfNotNull(node.FirstTableReference);

            GenerateNewLineOrSpace(_options.NewLineBeforeJoinClause);

            GenerateQualifiedJoinType(node.QualifiedJoinType);

            if (node.JoinHint != JoinHint.None)
            {
                GenerateSpace();
                JoinHintHelper.Instance.GenerateSourceForOption(_writer, node.JoinHint);
            }

            GenerateSpaceAndKeyword(TSqlTokenType.Join); 

            //MarkClauseBodyAlignmentWhenNecessary(_options.NewlineBeforeJoinClause);

            NewLine();
            GenerateFragmentIfNotNull(node.SecondTableReference);

            NewLine();
            GenerateKeyword(TSqlTokenType.On); 

            GenerateSpaceAndFragmentIfNotNull(node.SearchCondition);
        }

        private void GenerateQualifiedJoinType(QualifiedJoinType qualifiedJoinType)
        {
            List<TokenGenerator> generators = GetValueForEnumKey(_qualifiedJoinTypeGenerators, qualifiedJoinType);
            if (generators != null)
            {
                GenerateTokenList(generators);
            }
        }
    }
}
