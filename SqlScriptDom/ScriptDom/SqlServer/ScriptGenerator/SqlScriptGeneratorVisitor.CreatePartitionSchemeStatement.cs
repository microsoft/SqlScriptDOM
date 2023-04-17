//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CreatePartitionSchemeStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CreatePartitionSchemeStatement node)
        {
            GenerateKeyword(TSqlTokenType.Create);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Partition);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Scheme); 

            // name
            GenerateSpaceAndFragmentIfNotNull(node.Name);

            // AS PARTITION
            NewLineAndIndent();
            GenerateKeyword(TSqlTokenType.As);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Partition);
            GenerateSpaceAndFragmentIfNotNull(node.PartitionFunction);

            // TO filegroup
            NewLineAndIndent();
            if (node.IsAll)
            {
                GenerateKeyword(TSqlTokenType.All);
                GenerateSpace();
            }
            GenerateKeyword(TSqlTokenType.To);

            GenerateSpace();
            GenerateParenthesisedCommaSeparatedList(node.FileGroups);
        }
    }
}
