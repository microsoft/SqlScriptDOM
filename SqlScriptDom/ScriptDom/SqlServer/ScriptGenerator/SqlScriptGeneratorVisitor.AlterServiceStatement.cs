//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterServiceStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AlterServiceStatement node)
        {
            GenerateSpaceSeparatedTokens(TSqlTokenType.Alter,
                CodeGenerationSupporter.Service);

            // name
            GenerateSpaceAndFragmentIfNotNull(node.Name);

            if (node.QueueName != null)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.On);
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Queue);
                GenerateSpaceAndFragmentIfNotNull(node.QueueName);
            }

            if (node.ServiceContracts.Count > 0)
            {
                GenerateSpace();
                GenerateParenthesisedCommaSeparatedList(node.ServiceContracts);
            }
        }
    }
}
