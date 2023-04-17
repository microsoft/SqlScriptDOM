//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterPartitionSchemeStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AlterPartitionSchemeStatement node)
        {
            GenerateKeyword(TSqlTokenType.Alter);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Partition);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Scheme);

            // name
            GenerateSpaceAndFragmentIfNotNull(node.Name);

            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Next);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Used);

            GenerateSpaceAndFragmentIfNotNull(node.FileGroup);
        }
    }
}
