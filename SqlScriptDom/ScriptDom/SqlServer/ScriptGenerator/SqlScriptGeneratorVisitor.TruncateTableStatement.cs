//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.TruncateTableStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(TruncateTableStatement node)
        {
            GenerateKeyword(TSqlTokenType.Truncate);
            GenerateSpaceAndKeyword(TSqlTokenType.Table);
            GenerateSpaceAndFragmentIfNotNull(node.TableName);
            if (node.PartitionRanges != null && node.PartitionRanges.Count > 0)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.With);
                GenerateSpaceAndKeyword(TSqlTokenType.LeftParenthesis);
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Partitions);
                GenerateSpace();
                GenerateParenthesisedCommaSeparatedList(node.PartitionRanges);
                GenerateSpaceAndKeyword(TSqlTokenType.RightParenthesis);
            }
        }
    }
}
