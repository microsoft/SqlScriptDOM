//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.DataCompressionOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using Microsoft.SqlServer.TransactSql.ScriptDom;
namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(DataCompressionOption node)
        {
            GenerateTokenAndEqualSign(CodeGenerationSupporter.DataCompression);
            GenerateSpace();
            DataCompressionLevelHelper.Instance.GenerateSourceForOption(_writer, node.CompressionLevel);

            if (node.PartitionRanges.Count > 0)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.On);
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Partitions);
                GenerateSpace();
                GenerateParenthesisedCommaSeparatedList(node.PartitionRanges);
            }
        }
    }
}
