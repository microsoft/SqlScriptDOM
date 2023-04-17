//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.ExternalTableShardedDistributionPolicy.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(ExternalTableShardedDistributionPolicy node)
        {
            GenerateIdentifier(CodeGenerationSupporter.Sharded);
            GenerateSymbol(TSqlTokenType.LeftParenthesis);
            GenerateFragmentIfNotNull(node.ShardingColumn);
            GenerateSymbol(TSqlTokenType.RightParenthesis);
        }
    }
}