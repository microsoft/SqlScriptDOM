//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterFulltextIndexStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        protected static Dictionary<SimpleAlterFullTextIndexActionKind, List<TokenGenerator>> _simpleAlterFulltextIndexActionKindActions =
            new Dictionary<SimpleAlterFullTextIndexActionKind, List<TokenGenerator>>()
        {
    		{ SimpleAlterFullTextIndexActionKind.Disable, new List<TokenGenerator>() {
                    new IdentifierGenerator(CodeGenerationSupporter.Disable), }},
		    { SimpleAlterFullTextIndexActionKind.Enable, new List<TokenGenerator>() {
                    new IdentifierGenerator(CodeGenerationSupporter.Enable), }},
		    { SimpleAlterFullTextIndexActionKind.PausePopulation, new List<TokenGenerator>() {
                    new IdentifierGenerator(CodeGenerationSupporter.Pause, true),
                    new IdentifierGenerator(CodeGenerationSupporter.Population), }},
		    { SimpleAlterFullTextIndexActionKind.ResumePopulation, new List<TokenGenerator>() {
                    new IdentifierGenerator(CodeGenerationSupporter.Resume, true),
                    new IdentifierGenerator(CodeGenerationSupporter.Population), }},
    		{ SimpleAlterFullTextIndexActionKind.StopPopulation, new List<TokenGenerator>() {
                    new IdentifierGenerator(CodeGenerationSupporter.Stop, true),
                    new IdentifierGenerator(CodeGenerationSupporter.Population), }},
    		{ SimpleAlterFullTextIndexActionKind.SetChangeTrackingAuto, new List<TokenGenerator>() {
                    new KeywordGenerator(TSqlTokenType.Set, true),
                    new IdentifierGenerator(CodeGenerationSupporter.ChangeTracking, true),
                    new IdentifierGenerator(CodeGenerationSupporter.Auto), }},
		    { SimpleAlterFullTextIndexActionKind.SetChangeTrackingManual, new List<TokenGenerator>() {
                    new KeywordGenerator(TSqlTokenType.Set, true),
                    new IdentifierGenerator(CodeGenerationSupporter.ChangeTracking, true),
                    new IdentifierGenerator(CodeGenerationSupporter.Manual), }},
    		{ SimpleAlterFullTextIndexActionKind.SetChangeTrackingOff, new List<TokenGenerator>() {
                    new KeywordGenerator(TSqlTokenType.Set, true),
                    new IdentifierGenerator(CodeGenerationSupporter.ChangeTracking, true),
                    new KeywordGenerator(TSqlTokenType.Off), }},
    		{ SimpleAlterFullTextIndexActionKind.StartFullPopulation, new List<TokenGenerator>() {
                    new IdentifierGenerator(CodeGenerationSupporter.Start, true),
                    new KeywordGenerator(TSqlTokenType.Full, true),
                    new IdentifierGenerator(CodeGenerationSupporter.Population), }},
    		{ SimpleAlterFullTextIndexActionKind.StartIncrementalPopulation, new List<TokenGenerator>() {
                    new IdentifierGenerator(CodeGenerationSupporter.Start, true),
                    new IdentifierGenerator(CodeGenerationSupporter.Incremental, true),
                    new IdentifierGenerator(CodeGenerationSupporter.Population), }},
    		{ SimpleAlterFullTextIndexActionKind.StartUpdatePopulation, new List<TokenGenerator>() {
                    new IdentifierGenerator(CodeGenerationSupporter.Start, true),
                    new KeywordGenerator(TSqlTokenType.Update, true),
                    new IdentifierGenerator(CodeGenerationSupporter.Population), }}
        };

        public override void ExplicitVisit(AlterFullTextIndexStatement node)
        {
            GenerateKeyword(TSqlTokenType.Alter);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Fulltext);
            GenerateSpaceAndKeyword(TSqlTokenType.Index);
            GenerateSpaceAndKeyword(TSqlTokenType.On);

            // name
            GenerateSpaceAndFragmentIfNotNull(node.OnName);

            // could be
            //      AddAlterFulltextIndexAction
            //      DropAlterFulltextIndexAction
            //      SimpleAlterFulltextIndexAction
            GenerateSpaceAndFragmentIfNotNull(node.Action);
        }

        public override void ExplicitVisit(SimpleAlterFullTextIndexAction node)
        {
            List<TokenGenerator> generators = GetValueForEnumKey(_simpleAlterFulltextIndexActionKindActions, node.ActionKind);
            if (generators != null)
            {
                GenerateTokenList(generators);
            }
        }

        public override void ExplicitVisit(SetStopListAlterFullTextIndexAction node)
        {
            GenerateKeywordAndSpace(TSqlTokenType.Set);
            GenerateFragmentIfNotNull(node.StopListOption);
            GenerateWithNoPopulation(node.WithNoPopulation);
        }

        public override void ExplicitVisit(SetSearchPropertyListAlterFullTextIndexAction node)
        {
            GenerateKeywordAndSpace(TSqlTokenType.Set);
            GenerateFragmentIfNotNull(node.SearchPropertyListOption);
            GenerateWithNoPopulation(node.WithNoPopulation);
        }

        public override void ExplicitVisit(DropAlterFullTextIndexAction node)
        {
            GenerateKeywordAndSpace(TSqlTokenType.Drop);
            GenerateParenthesisedCommaSeparatedList(node.Columns);
            GenerateWithNoPopulation(node.WithNoPopulation);
        }

        public override void ExplicitVisit(AddAlterFullTextIndexAction node)
        {
            GenerateKeywordAndSpace(TSqlTokenType.Add);
            GenerateParenthesisedCommaSeparatedList(node.Columns);
            GenerateWithNoPopulation(node.WithNoPopulation);
        }

        public override void ExplicitVisit(AlterColumnAlterFullTextIndexAction node)
        {
            GenerateKeywordAndSpace(TSqlTokenType.Alter);
            GenerateKeywordAndSpace(TSqlTokenType.Column);
            GenerateFragmentIfNotNull(node.Column.Name);
            if (node.Column.StatisticalSemantics)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.Add);
            }
            else
            {
                GenerateSpaceAndKeyword(TSqlTokenType.Drop);
            }
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.StatisticalSemantics);
            GenerateWithNoPopulation(node.WithNoPopulation);
        }

        protected void GenerateWithNoPopulation(bool withNoPopulation)
        {
            if (withNoPopulation)
            {
                GenerateSpace();
                GenerateSpaceSeparatedTokens(TSqlTokenType.With, CodeGenerationSupporter.No, CodeGenerationSupporter.Population);
            }
        }
    }
}