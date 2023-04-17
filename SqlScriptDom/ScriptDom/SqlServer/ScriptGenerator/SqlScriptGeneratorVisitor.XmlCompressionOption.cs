//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.XmlCompressionOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(XmlCompressionOption node)
        {
            GenerateTokenAndEqualSign(CodeGenerationSupporter.XmlCompression);
            GenerateSpace();
            XmlCompressionOptionHelper.Instance.GenerateSourceForOption(_writer, node.IsCompressed);

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
