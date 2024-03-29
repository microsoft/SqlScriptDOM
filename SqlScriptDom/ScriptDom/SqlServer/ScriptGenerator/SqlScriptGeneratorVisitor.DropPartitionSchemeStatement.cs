//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.DropPartitionSchemeStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(DropPartitionSchemeStatement node)
        {
            GenerateKeyword(TSqlTokenType.Drop);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Partition);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Scheme); 

            GenerateSpaceAndFragmentIfNotNull(node.Name);
        }
    }
}
