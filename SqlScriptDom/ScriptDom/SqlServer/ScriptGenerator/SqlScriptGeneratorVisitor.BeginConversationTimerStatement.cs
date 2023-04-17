//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.BeginConversationTimerStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(BeginConversationTimerStatement node)
        {
            GenerateKeyword(TSqlTokenType.Begin);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Conversation);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Timer);

            GenerateSpace();
            GenerateParenthesisedFragmentIfNotNull(node.Handle);

            GenerateSpace();
            GenerateNameEqualsValue(CodeGenerationSupporter.Timeout, node.Timeout);
        }
    }
}
