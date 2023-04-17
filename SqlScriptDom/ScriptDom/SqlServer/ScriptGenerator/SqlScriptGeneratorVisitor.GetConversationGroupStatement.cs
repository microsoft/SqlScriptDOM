//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.GetConversationGroupStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(GetConversationGroupStatement node)
        {
            GenerateIdentifier(CodeGenerationSupporter.Get);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Conversation);
            GenerateSpaceAndKeyword(TSqlTokenType.Group);
            GenerateSpaceAndFragmentIfNotNull(node.GroupId);
            GenerateSpaceAndKeyword(TSqlTokenType.From);
            GenerateSpaceAndFragmentIfNotNull(node.Queue);
        }
    }
}
