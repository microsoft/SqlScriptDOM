//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.ReceiveStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(ReceiveStatement node)
        {
            // RECEIVE
            GenerateIdentifier(CodeGenerationSupporter.Receive); 

            // TOP
            if (node.Top != null)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.Top); 
                GenerateSpace();
                GenerateParenthesisedFragmentIfNotNull(node.Top);
            }

            GenerateSpace();
            // could be
            //      SelectSetVariable
            //      SelectColumn
            GenerateCommaSeparatedList(node.SelectElements);

            // FROM
            GenerateSpaceAndKeyword(TSqlTokenType.From);
            GenerateSpaceAndFragmentIfNotNull(node.Queue);

            // INTO
            if (node.Into != null)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.Into);
                GenerateSpaceAndFragmentIfNotNull(node.Into);
            }

            // WHERE
            if (node.Where != null)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.Where);

                GenerateSpace();
                GenerateNameEqualsValue(
                    node.IsConversationGroupIdWhere 
                        ? CodeGenerationSupporter.ConversationGroupId 
                        : CodeGenerationSupporter.ConversationHandle, 
                    node.Where);
            }
        }
    }
}
