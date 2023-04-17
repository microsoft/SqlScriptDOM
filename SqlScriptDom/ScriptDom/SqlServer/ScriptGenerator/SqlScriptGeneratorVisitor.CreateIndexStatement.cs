//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CreateIndexStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CreateIndexStatement node)
        {
            GenerateKeyword(TSqlTokenType.Create);

            if (node.Unique)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.Unique);
            }

            if (node.Clustered.HasValue)
            {
                GenerateSpaceAndKeyword(node.Clustered.Value ? TSqlTokenType.Clustered : TSqlTokenType.NonClustered);
            }

            GenerateSpaceAndKeyword(TSqlTokenType.Index);

            // name
            GenerateSpaceAndFragmentIfNotNull(node.Name);

            NewLineAndIndent();
            GenerateKeyword(TSqlTokenType.On);
            GenerateSpaceAndFragmentIfNotNull(node.OnName);

            GenerateParenthesisedCommaSeparatedList(node.Columns);

            if (node.IncludeColumns != null && node.IncludeColumns.Count > 0)
            {
                NewLineAndIndent();
                GenerateIdentifier(CodeGenerationSupporter.Include);

                GenerateParenthesisedCommaSeparatedList(node.IncludeColumns);
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

            GenerateFileStreamOn(node);
        }

        // SQL80 has different format for the index option
        protected virtual void GenerateIndexOptions(IList<IndexOption> options)
        {
            if (options != null && options.Count > 0)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.With);

                GenerateSpace();
                GenerateParenthesisedCommaSeparatedList(options);
            }
        }

        private void GenerateFileStreamOn(IFileStreamSpecifier node)
        {
            if (node.FileStreamOn != null)
            {
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.FileStreamOn);
                GenerateSpaceAndFragmentIfNotNull(node.FileStreamOn);
            }
        }
    }
}
