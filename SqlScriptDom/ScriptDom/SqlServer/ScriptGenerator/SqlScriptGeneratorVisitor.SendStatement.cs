//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.SendStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(SendStatement node)
        {
            GenerateIdentifier(CodeGenerationSupporter.Send);
            GenerateSpaceAndKeyword(TSqlTokenType.On);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Conversation);
            GenerateSpace();
            if (_options.SqlVersion >= SqlVersion.Sql110)
            {
                //Parentheses are only valid in Sql110
                //
                GenerateParenthesisedCommaSeparatedList(node.ConversationHandles);
            }
            else
            {
                GenerateCommaSeparatedList(node.ConversationHandles);
            }

            if (node.MessageTypeName != null)
            {
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Message);
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Type);
                // could be
                //      SchemaObjectName
                //      Literal
                GenerateSpaceAndFragmentIfNotNull(node.MessageTypeName);
            }

            if (node.MessageBody != null)
            {
                GenerateSpace();
                GenerateParenthesisedFragmentIfNotNull(node.MessageBody);
            }
        }
    }
}
