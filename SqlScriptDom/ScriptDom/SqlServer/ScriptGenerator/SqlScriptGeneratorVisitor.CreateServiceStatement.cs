//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CreateServiceStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CreateServiceStatement node)
        {
            GenerateKeyword(TSqlTokenType.Create);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Service);

            // name
            GenerateSpaceAndFragmentIfNotNull(node.Name);

            // owner
            GenerateOwnerIfNotNull(node.Owner);

            // ON QUEUE
            NewLineAndIndent();
            GenerateKeyword(TSqlTokenType.On);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Queue);
            GenerateSpaceAndFragmentIfNotNull(node.QueueName);

            // contract name
            if (node.ServiceContracts != null && node.ServiceContracts.Count > 0)
            {
                NewLineAndIndent();
                GenerateParenthesisedCommaSeparatedList(node.ServiceContracts);
            }
        }
    }
}
