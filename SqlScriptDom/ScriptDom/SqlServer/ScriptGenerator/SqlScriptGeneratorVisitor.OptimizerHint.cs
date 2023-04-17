//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.SimpleOptimizerHint.cs" company="Microsoft">
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

        protected static Dictionary<OptimizerHintKind, List<TokenGenerator>> _optimizerHintKindsGenerators =
            new Dictionary<OptimizerHintKind, List<TokenGenerator>>()
        {
               { OptimizerHintKind.AlterColumnPlan, new List<TokenGenerator>( ) {
                    new IdentifierGenerator(CodeGenerationSupporter.AlterColumn, true),
                    new KeywordGenerator(TSqlTokenType.Plan), }},
               { OptimizerHintKind.BypassOptimizerQueue, new List<TokenGenerator>( ) {
                    new IdentifierGenerator(CodeGenerationSupporter.Bypass, true),
                    new IdentifierGenerator(CodeGenerationSupporter.OptimizerQueue), }},
               { OptimizerHintKind.ConcatUnion, new List<TokenGenerator>( ) {
                    new IdentifierGenerator(CodeGenerationSupporter.Concat, true),
                    new KeywordGenerator(TSqlTokenType.Union), }},
               { OptimizerHintKind.ExpandViews, new List<TokenGenerator>( ) {
                    new IdentifierGenerator(CodeGenerationSupporter.Expand, true),
                    new IdentifierGenerator(CodeGenerationSupporter.Views), }},
               { OptimizerHintKind.ForceOrder, new List<TokenGenerator>( ) {
                    new IdentifierGenerator(CodeGenerationSupporter.Force, true),
                    new KeywordGenerator(TSqlTokenType.Order), }},
               { OptimizerHintKind.HashGroup, new List<TokenGenerator>( ) {
                    new IdentifierGenerator(CodeGenerationSupporter.Hash, true),
                    new KeywordGenerator(TSqlTokenType.Group), }},
               { OptimizerHintKind.HashJoin, new List<TokenGenerator>( ) {
                    new IdentifierGenerator(CodeGenerationSupporter.Hash, true),
                    new KeywordGenerator(TSqlTokenType.Join) }},
               { OptimizerHintKind.HashUnion, new List<TokenGenerator>( ) {
                    new IdentifierGenerator(CodeGenerationSupporter.Hash, true),
                    new KeywordGenerator(TSqlTokenType.Union) }},
               { OptimizerHintKind.KeepFixedPlan, new List<TokenGenerator>( ) {
                    new IdentifierGenerator(CodeGenerationSupporter.KeepFixed, true),
                    new KeywordGenerator(TSqlTokenType.Plan), }},
               { OptimizerHintKind.KeepPlan, new List<TokenGenerator>( ) {
                    new IdentifierGenerator(CodeGenerationSupporter.Keep, true),
                    new KeywordGenerator(TSqlTokenType.Plan), }},
               { OptimizerHintKind.KeepUnion, new List<TokenGenerator>( ) {
                    new IdentifierGenerator(CodeGenerationSupporter.Keep, true),
                    new KeywordGenerator(TSqlTokenType.Union) }},
               { OptimizerHintKind.LoopJoin, new List<TokenGenerator>( ) {
                    new IdentifierGenerator(CodeGenerationSupporter.Loop, true),
                    new KeywordGenerator(TSqlTokenType.Join) }},
               { OptimizerHintKind.MergeJoin, new List<TokenGenerator>( ) {
                    new IdentifierGenerator(CodeGenerationSupporter.Merge, true),
                    new KeywordGenerator(TSqlTokenType.Join) }},
               { OptimizerHintKind.MergeUnion, new List<TokenGenerator>( ) {
                    new IdentifierGenerator(CodeGenerationSupporter.Merge, true),
                    new KeywordGenerator(TSqlTokenType.Union) }},
               { OptimizerHintKind.OrderGroup, new List<TokenGenerator>( ) {
                    new KeywordGenerator(TSqlTokenType.Order, true),
                    new KeywordGenerator(TSqlTokenType.Group) }},
               { OptimizerHintKind.RobustPlan, new List<TokenGenerator>( ) {
                    new IdentifierGenerator(CodeGenerationSupporter.Robust, true),
                    new KeywordGenerator(TSqlTokenType.Plan), }},
               { OptimizerHintKind.ShrinkDBPlan, new List<TokenGenerator>( ) {
                    new IdentifierGenerator(CodeGenerationSupporter.ShrinkDb, true),
                    new KeywordGenerator(TSqlTokenType.Plan), }},
               // handled specially
               //{ SimpleOptimizerHintKind.UsePlan, new List<TokenGenerator>( ) {}},
               { OptimizerHintKind.ParameterizationSimple, new List<TokenGenerator>( ) {
                    new IdentifierGenerator(CodeGenerationSupporter.Parameterization, true),
                    new IdentifierGenerator(CodeGenerationSupporter.Simple) }},
               { OptimizerHintKind.ParameterizationForced, new List<TokenGenerator>( ) {
                    new IdentifierGenerator(CodeGenerationSupporter.Parameterization, true),
                    new IdentifierGenerator(CodeGenerationSupporter.Forced) }},
               { OptimizerHintKind.OptimizeCorrelatedUnionAll, new List<TokenGenerator>( ) {
                    new IdentifierGenerator(CodeGenerationSupporter.Optimize, true),
                    new IdentifierGenerator(CodeGenerationSupporter.Correlated, true),
                    new KeywordGenerator(TSqlTokenType.Union, true),
                    new KeywordGenerator(TSqlTokenType.All), }},
               { OptimizerHintKind.Recompile, new List<TokenGenerator>( ) {
                    new IdentifierGenerator(CodeGenerationSupporter.Recompile), }},
               { OptimizerHintKind.Fast, new List<TokenGenerator>( ) {
                    new IdentifierGenerator(CodeGenerationSupporter.Fast), }},
               { OptimizerHintKind.CheckConstraintsPlan, new List<TokenGenerator>( ) {
                    new IdentifierGenerator(CodeGenerationSupporter.CheckConstraintsHint, true),
                    new KeywordGenerator(TSqlTokenType.Plan), }},
               { OptimizerHintKind.MaxRecursion, new List<TokenGenerator>( ) {
                    new IdentifierGenerator(CodeGenerationSupporter.MaxRecursion), }},
               { OptimizerHintKind.MaxDop, new List<TokenGenerator>( ) {
                    new IdentifierGenerator(CodeGenerationSupporter.MaxDop), }},
               { OptimizerHintKind.IgnoreNonClusteredColumnStoreIndex, new List<TokenGenerator>( ) {
                    new IdentifierGenerator(CodeGenerationSupporter.IgnoreNonClusteredColumnStoreIndex), }},
               { OptimizerHintKind.QueryTraceOn, new List<TokenGenerator>( ) {
                    new IdentifierGenerator(CodeGenerationSupporter.QueryTraceOn), }},
               { OptimizerHintKind.MaxGrantPercent, new List<TokenGenerator>() {
                    new IdentifierGenerator(CodeGenerationSupporter.MaxGrantPercent, true),
				    new KeywordGenerator(TSqlTokenType.EqualsSign), }},
               { OptimizerHintKind.MinGrantPercent, new List<TokenGenerator>() {
                    new IdentifierGenerator(CodeGenerationSupporter.MinGrantPercent, true),
                    new KeywordGenerator(TSqlTokenType.EqualsSign), }},
               { OptimizerHintKind.NoPerformanceSpool, new List<TokenGenerator>( ) {
                    new IdentifierGenerator(CodeGenerationSupporter.NoPerformanceSpool), }},
               { OptimizerHintKind.Label, new List<TokenGenerator>() {
                    new IdentifierGenerator(CodeGenerationSupporter.Label, true),
                    new KeywordGenerator(TSqlTokenType.EqualsSign) }}
       };

        public override void ExplicitVisit(LiteralOptimizerHint node)
        {
            if (node.HintKind == OptimizerHintKind.UsePlan)
            {
                if (node.Value != null && node.Value.LiteralType == LiteralType.Integer)
                {
                    GenerateIdentifier(CodeGenerationSupporter.UsePlan);
                }
                else
                {
                    GenerateKeyword(TSqlTokenType.Use);
                    GenerateSpaceAndKeyword(TSqlTokenType.Plan);
                }
            }
            else
            {
                List<TokenGenerator> generators = GetValueForEnumKey(_optimizerHintKindsGenerators, node.HintKind);
                if (generators != null)
                {
                    GenerateTokenList(generators);
                }
            }

            GenerateSpaceAndFragmentIfNotNull(node.Value);
        }

        public override void ExplicitVisit(OptimizerHint node)
        {
            List<TokenGenerator> generators = GetValueForEnumKey(_optimizerHintKindsGenerators, node.HintKind);
            if (generators != null)
            {
                GenerateTokenList(generators);
            }
        }
    }
}
