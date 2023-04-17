//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CompressionPartitionRange.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using Microsoft.SqlServer.TransactSql.ScriptDom;
namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CompressionPartitionRange node)
        {
            GenerateFragmentIfNotNull(node.From);

            if (node.To != null)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.To);
                GenerateSpaceAndFragmentIfNotNull(node.To);
            }
        }
    }
}
