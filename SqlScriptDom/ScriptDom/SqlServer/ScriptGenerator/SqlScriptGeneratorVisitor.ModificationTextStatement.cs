//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.ModificationTextStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        protected void GenerateBulkColumnTimestamp(TextModificationStatement node)
        {
            if (node.Bulk)
            {
                GenerateKeywordAndSpace(TSqlTokenType.Bulk);
            }

            GenerateFragmentIfNotNull(node.Column);
            GenerateSpaceAndFragmentIfNotNull(node.TextId);

            if (node.Timestamp != null)
            {
                GenerateSpace();
                GenerateNameEqualsValue(CodeGenerationSupporter.TimeStamp, node.Timestamp);
            }
        }

    }
}
