//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CreateColumnStoreIndexStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CreateColumnStoreIndexStatement node)
        {
            GenerateKeyword(TSqlTokenType.Create);

            if (node.Clustered.HasValue)
            {
                GenerateSpaceAndKeyword(node.Clustered.Value ? TSqlTokenType.Clustered : TSqlTokenType.NonClustered);
            }
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.ColumnStore);

            GenerateSpaceAndKeyword(TSqlTokenType.Index);

            // name
            GenerateSpaceAndFragmentIfNotNull(node.Name);

            NewLineAndIndent();
            GenerateKeyword(TSqlTokenType.On);
            GenerateSpaceAndFragmentIfNotNull(node.OnName);

            // If clustered, ignore the columns, if any
            if (!node.Clustered.GetValueOrDefault() &&
                 node.Columns != null && node.Columns.Count > 0)
            {
                GenerateParenthesisedCommaSeparatedList(node.Columns);
            }

            // Generate ordered columns if any, for both clustered and non-clustered indexes
            // If node.Clustered is null or false, it indicates index type is non-clustered.
            // If node.Clustered is true, it indicates index type is clustered.
            if (node.OrderedColumns != null && node.OrderedColumns.Count > 0)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.Order);
                GenerateParenthesisedCommaSeparatedList(node.OrderedColumns);
            }

            if (node.FilterPredicate != null)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.Where);
                GenerateSpaceAndFragmentIfNotNull(node.FilterPredicate);
            }

            GenerateIndexOptions(node.IndexOptions);

            if (node.OnFileGroupOrPartitionScheme != null)
            {
                NewLineAndIndent();
                GenerateKeyword(TSqlTokenType.On);

                GenerateSpaceAndFragmentIfNotNull(node.OnFileGroupOrPartitionScheme);
            }
        }
    }
}
