//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.DropPartitionFunctionStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(DropPartitionFunctionStatement node)
        {
            GenerateKeyword(TSqlTokenType.Drop);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Partition);
            GenerateSpaceAndKeyword(TSqlTokenType.Function);

            GenerateSpaceAndFragmentIfNotNull(node.Name);
        }
    }
}
