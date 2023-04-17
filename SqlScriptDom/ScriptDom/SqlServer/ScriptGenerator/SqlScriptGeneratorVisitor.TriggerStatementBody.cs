//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.TriggerStatementBody.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        private static Dictionary<TriggerType, List<TokenGenerator>> _triggerTypeGenerators = new Dictionary<TriggerType, List<TokenGenerator>>()
        {
            {TriggerType.After, new List<TokenGenerator>() {
                new IdentifierGenerator(CodeGenerationSupporter.After)}},
            {TriggerType.For, new List<TokenGenerator>() {
                new KeywordGenerator(TSqlTokenType.For)}},
            {TriggerType.InsteadOf, new List<TokenGenerator>() {
                new IdentifierGenerator(CodeGenerationSupporter.Instead, true),
                new KeywordGenerator(TSqlTokenType.Of)}},
            {TriggerType.Unknown, new List<TokenGenerator>() {
                new EmptyGenerator()}},
        };

        protected void GenerateTriggerStatementBody(TriggerStatementBody node)
        {
            GenerateKeyword(TSqlTokenType.Trigger);

            // name
            GenerateSpaceAndFragmentIfNotNull(node.Name);

            // ON
            NewLineAndIndent();
            GenerateKeyword(TSqlTokenType.On);
            GenerateSpaceAndFragmentIfNotNull(node.TriggerObject);

            // WITH
            GenerateCommaSeparatedWithClause(node.Options, true, false);

            // trigger type: AFTER, FOR, INSTEAD OF
            List<TokenGenerator> generators = GetValueForEnumKey(_triggerTypeGenerators, node.TriggerType);
            if (generators != null)
            {
                NewLineAndIndent();
                GenerateTokenList(generators);
            }

            // trigger action
            if (node.TriggerActions != null && node.TriggerActions.Count > 0)
            {
                GenerateSpace();
                GenerateCommaSeparatedList(node.TriggerActions);
            }

            // WITH APPEND
            if (node.WithAppend)
            {
                NewLineAndIndent();
                GenerateKeyword(TSqlTokenType.With);
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Append);
            }

            // NOT FOR REPLICATION
            if (node.IsNotForReplication)
            {
                NewLineAndIndent();
                GenerateKeyword(TSqlTokenType.Not);
                GenerateSpaceAndKeyword(TSqlTokenType.For);
                GenerateSpaceAndKeyword(TSqlTokenType.Replication);
            }

            // AS
            NewLineAndIndent();
            GenerateKeyword(TSqlTokenType.As);

            GenerateSpace();
            AlignmentPoint ap = new AlignmentPoint();
            MarkAndPushAlignmentPoint(ap);
            GenerateFragmentIfNotNull(node.StatementList);
            PopAlignmentPoint();

            GenerateSpaceAndFragmentIfNotNull(node.MethodSpecifier);
        }
    }
}
