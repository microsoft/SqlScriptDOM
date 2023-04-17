//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CreateQueueStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CreateQueueStatement node)
        {
            GenerateKeyword(TSqlTokenType.Create);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Queue);

            // name
            GenerateSpaceAndFragmentIfNotNull(node.Name);

            // WITH
            if (node.QueueOptions != null && node.QueueOptions.Count > 0)
            {
                NewLineAndIndent();
                GenerateKeyword(TSqlTokenType.With);
                GenerateSpace();

                GenerateQueueOptions(node.QueueOptions);
            }

            // file group
            if (node.OnFileGroup != null)
            {
                NewLineAndIndent();
                GenerateKeyword(TSqlTokenType.On);

                GenerateSpaceAndFragmentIfNotNull(node.OnFileGroup);
            }
        }
    }
}
