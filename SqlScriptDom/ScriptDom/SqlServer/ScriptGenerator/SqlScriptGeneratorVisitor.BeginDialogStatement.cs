//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.BeginDialogStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        protected static Dictionary<DialogOptionKind, String> _dialogOptionNames = new Dictionary<DialogOptionKind, String>()
        {
            {DialogOptionKind.Lifetime, CodeGenerationSupporter.LifeTime},
            {DialogOptionKind.RelatedConversation, CodeGenerationSupporter.RelatedConversation},
            {DialogOptionKind.RelatedConversationGroup, CodeGenerationSupporter.RelatedConversationGroup},
        };

        public override void ExplicitVisit(BeginDialogStatement node)
        {
            GenerateKeyword(TSqlTokenType.Begin);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Dialog);
            if (node.IsConversation)
            {
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Conversation);
            }

            GenerateSpaceAndFragmentIfNotNull(node.Handle);

            NewLineAndIndent();
            GenerateKeyword(TSqlTokenType.From);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Service);
            GenerateSpaceAndFragmentIfNotNull(node.InitiatorServiceName);

            NewLineAndIndent();
            GenerateKeyword(TSqlTokenType.To);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Service);
            GenerateSpaceAndFragmentIfNotNull(node.TargetServiceName);

            if (node.InstanceSpec != null)
            {
                GenerateSymbol(TSqlTokenType.Comma);
                GenerateSpaceAndFragmentIfNotNull(node.InstanceSpec);
            }

            if (node.ContractName != null)
            {
                NewLineAndIndent();
                GenerateKeyword(TSqlTokenType.On);
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Contract);
                GenerateSpaceAndFragmentIfNotNull(node.ContractName);
            }

            GenerateDialogOptions(node.Options);
        }

        private void GenerateDialogOptions(IList<DialogOption> options)
        {
            if (options != null && options.Count > 0)
            {
                NewLineAndIndent();
                GenerateKeywordAndSpace(TSqlTokenType.With);
                GenerateCommaSeparatedList(options);
            }
        }

        public override void ExplicitVisit(OnOffDialogOption node)
        {
            System.Diagnostics.Debug.Assert(node.OptionKind == DialogOptionKind.Encryption);
            GenerateOptionStateWithEqualSign(CodeGenerationSupporter.Encryption, node.OptionState);
        }

        public override void ExplicitVisit(ScalarExpressionDialogOption node)
        {
            string optionName = GetValueForEnumKey(_dialogOptionNames, node.OptionKind);
            GenerateNameEqualsValue(optionName, node.Value);
        }
    }
}
