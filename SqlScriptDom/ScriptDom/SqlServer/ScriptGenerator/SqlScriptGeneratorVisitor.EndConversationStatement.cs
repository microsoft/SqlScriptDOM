//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.EndConversationStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(EndConversationStatement node)
        {
            GenerateKeyword(TSqlTokenType.End);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Conversation);

            // name
            GenerateSpaceAndFragmentIfNotNull(node.Conversation);

            if (node.WithCleanup)
            {
                NewLineAndIndent();
                GenerateKeyword(TSqlTokenType.With);
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Cleanup); 
            }
            else if (node.ErrorCode != null && node.ErrorDescription != null)
            {
                NewLineAndIndent();
                GenerateKeyword(TSqlTokenType.With);
                GenerateSpace();
                GenerateNameEqualsValue(CodeGenerationSupporter.Error, node.ErrorCode);
                GenerateSpace();
                GenerateNameEqualsValue(CodeGenerationSupporter.Description, node.ErrorDescription); 
            }
        }
    }
}
