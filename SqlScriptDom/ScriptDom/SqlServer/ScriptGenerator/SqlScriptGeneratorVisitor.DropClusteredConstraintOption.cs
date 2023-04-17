//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.DropClusteredConstraintOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        protected static Dictionary<DropClusteredConstraintOptionKind, List<TokenGenerator>> _dropClusteredConstraintOptionTypeGenerators = 
            new Dictionary<DropClusteredConstraintOptionKind, List<TokenGenerator>>()
        {
            { DropClusteredConstraintOptionKind.MaxDop, new List<TokenGenerator>() {
                new IdentifierGenerator(CodeGenerationSupporter.MaxDop, true),
                new KeywordGenerator(TSqlTokenType.EqualsSign), }},
            { DropClusteredConstraintOptionKind.MoveTo, new List<TokenGenerator>() {
                new IdentifierGenerator(CodeGenerationSupporter.Move, true),
                new KeywordGenerator(TSqlTokenType.To), }},
            { DropClusteredConstraintOptionKind.Online,  new List<TokenGenerator>() {
                new IdentifierGenerator(CodeGenerationSupporter.Online, true),
                new KeywordGenerator(TSqlTokenType.EqualsSign), }},
            { DropClusteredConstraintOptionKind.WaitAtLowPriority, new List<TokenGenerator>() {
                new IdentifierGenerator(CodeGenerationSupporter.WaitAtLowPriority, true) }
            }
        };

        protected void GenerateOptionHeader(DropClusteredConstraintOption node)
        {
            List<TokenGenerator> generators = GetValueForEnumKey(_dropClusteredConstraintOptionTypeGenerators, node.OptionKind);
            if (generators != null)
            {
                GenerateTokenList(generators);
            }
        }

        public override void ExplicitVisit(DropClusteredConstraintValueOption node)
        {
            GenerateOptionHeader(node);

            GenerateSpaceAndFragmentIfNotNull(node.OptionValue);
        }

        public override void ExplicitVisit(DropClusteredConstraintMoveOption node)
        {
            GenerateOptionHeader(node);

            // FilegroupOrPartitionScheme 
            GenerateSpaceAndFragmentIfNotNull(node.OptionValue);
        }

        public override void ExplicitVisit(DropClusteredConstraintWaitAtLowPriorityLockOption node)
        {
            GenerateLowPriorityWaitOptions(node.Options);
        }
    }
}
