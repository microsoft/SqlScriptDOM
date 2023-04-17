//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.ContractMessage.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        protected static Dictionary<MessageSender, TokenGenerator> _messageSenderGenerators = new Dictionary<MessageSender, TokenGenerator>()
        {
            {MessageSender.None, new EmptyGenerator()},
            {MessageSender.Any, new KeywordGenerator(TSqlTokenType.Any)},
            {MessageSender.Initiator, new IdentifierGenerator(CodeGenerationSupporter.Initiator)},
            {MessageSender.Target, new IdentifierGenerator(CodeGenerationSupporter.Target)},
        };

        public override void ExplicitVisit(ContractMessage node)
        {
            // name
            GenerateFragmentIfNotNull(node.Name);

            // SENT BY
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Sent);
            GenerateSpaceAndKeyword(TSqlTokenType.By);

            TokenGenerator gen = GetValueForEnumKey(_messageSenderGenerators, node.SentBy);
            GenerateSpace();
            GenerateToken(gen);
        }
    }
}
