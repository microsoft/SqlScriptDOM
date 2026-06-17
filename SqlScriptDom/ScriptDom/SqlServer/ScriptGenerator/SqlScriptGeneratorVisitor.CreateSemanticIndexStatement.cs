//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CreateSemanticIndexStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CreateSemanticIndexStatement node)
        {
            GenerateKeyword(TSqlTokenType.Create);

            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Semantic);

            GenerateSpaceAndKeyword(TSqlTokenType.Index);

            // name
            GenerateSpaceAndFragmentIfNotNull(node.Name);

            NewLineAndIndent();
            GenerateKeyword(TSqlTokenType.On);
            GenerateSpaceAndFragmentIfNotNull(node.OnName);

            // Column definitions
            GenerateSpace();
            GenerateSymbol(TSqlTokenType.LeftParenthesis);

            for (int i = 0; i < node.Columns.Count; i++)
            {
                if (i > 0)
                {
                    GenerateSymbol(TSqlTokenType.Comma);
                    GenerateSpace();
                }
                GenerateFragmentIfNotNull(node.Columns[i]);
            }

            GenerateSymbol(TSqlTokenType.RightParenthesis);

            // WITH clause
            bool hasWithOption = node.ExternalModelName != null ||
                                 (node.VectorIndexOptions != null && node.VectorIndexOptions.Count > 0) ||
                                 node.FulltextStoplistOption != null ||
                                 (node.IndexOptions != null && node.IndexOptions.Count > 0);

            if (hasWithOption)
            {
                NewLineAndIndent();
                GenerateKeyword(TSqlTokenType.With);
                GenerateSpace();
                GenerateSymbol(TSqlTokenType.LeftParenthesis);

                bool first = true;

                if (node.ExternalModelName != null)
                {
                    GenerateIdentifier(CodeGenerationSupporter.ExternalModel);
                    GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
                    GenerateSpace();
                    GenerateFragmentIfNotNull(node.ExternalModelName);
                    if (node.ExternalModelParameters != null)
                    {
                        GenerateSpace();
                        GenerateSymbol(TSqlTokenType.LeftParenthesis);
                        GenerateIdentifier(CodeGenerationSupporter.Parameters);
                        GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
                        GenerateSpace();
                        GenerateFragmentIfNotNull(node.ExternalModelParameters);
                        GenerateSymbol(TSqlTokenType.RightParenthesis);
                    }
                    first = false;
                }

                if (node.VectorIndexOptions != null && node.VectorIndexOptions.Count > 0)
                {
                    if (!first)
                    {
                        GenerateSymbol(TSqlTokenType.Comma);
                        GenerateSpace();
                    }
                    GenerateIdentifier(CodeGenerationSupporter.VectorIndex);
                    GenerateSpace();
                    GenerateSymbol(TSqlTokenType.LeftParenthesis);
                    GenerateCommaSeparatedList(node.VectorIndexOptions);
                    GenerateSymbol(TSqlTokenType.RightParenthesis);
                    first = false;
                }

                if (node.FulltextStoplistOption != null)
                {
                    if (!first)
                    {
                        GenerateSymbol(TSqlTokenType.Comma);
                        GenerateSpace();
                    }
                    GenerateIdentifier(CodeGenerationSupporter.FulltextStopList);
                    GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
                    GenerateSpace();
                    if (node.FulltextStoplistOption.IsOff)
                    {
                        GenerateKeyword(TSqlTokenType.Off);
                    }
                    else
                    {
                        GenerateFragmentIfNotNull(node.FulltextStoplistOption.StopListName);
                    }
                    first = false;
                }

                if (node.IndexOptions != null && node.IndexOptions.Count > 0)
                {
                    foreach (var option in node.IndexOptions)
                    {
                        if (!first)
                        {
                            GenerateSymbol(TSqlTokenType.Comma);
                            GenerateSpace();
                        }
                        GenerateFragmentIfNotNull(option);
                        first = false;
                    }
                }

                GenerateSymbol(TSqlTokenType.RightParenthesis);
            }

            if (node.OnFileGroupOrPartitionScheme != null)
            {
                NewLineAndIndent();
                GenerateKeyword(TSqlTokenType.On);

                GenerateSpaceAndFragmentIfNotNull(node.OnFileGroupOrPartitionScheme);
            }
        }

        public override void ExplicitVisit(SemanticIndexColumn node)
        {
            GenerateFragmentIfNotNull(node.ColumnName);

            if (node.SearchType != SemanticIndexSearchType.NotSpecified)
            {
                GenerateSpace();
                GenerateIdentifier(CodeGenerationSupporter.SearchType);
                GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
                GenerateSpace();
                switch (node.SearchType)
                {
                    case SemanticIndexSearchType.Vector:
                        GenerateIdentifier(CodeGenerationSupporter.Vector);
                        break;
                    case SemanticIndexSearchType.Fulltext:
                        GenerateIdentifier(CodeGenerationSupporter.Fulltext);
                        break;
                    case SemanticIndexSearchType.Hybrid:
                        GenerateIdentifier(CodeGenerationSupporter.Hybrid);
                        break;
                }
            }

            if (node.TypeColumnName != null)
            {
                GenerateSpace();
                GenerateIdentifier(CodeGenerationSupporter.Type);
                GenerateSpaceAndKeyword(TSqlTokenType.Column);
                GenerateSpaceAndFragmentIfNotNull(node.TypeColumnName);
            }

            if (node.Language != null)
            {
                GenerateSpace();
                GenerateIdentifier(CodeGenerationSupporter.Language);
                GenerateSpaceAndFragmentIfNotNull(node.Language);
            }

            if (node.ChunkOptions != null && node.ChunkOptions.Count > 0)
            {
                GenerateSpace();
                GenerateIdentifier(CodeGenerationSupporter.ChunkUsing);
                GenerateSymbol(TSqlTokenType.LeftParenthesis);
                GenerateCommaSeparatedList(node.ChunkOptions);
                GenerateSymbol(TSqlTokenType.RightParenthesis);
            }
        }

        public override void ExplicitVisit(SemanticIndexChunkOption node)
        {
            switch (node.OptionKind)
            {
                case SemanticIndexChunkOptionKind.Type:
                    GenerateIdentifier(CodeGenerationSupporter.Type);
                    break;
                case SemanticIndexChunkOptionKind.Size:
                    GenerateIdentifier(CodeGenerationSupporter.Size);
                    break;
                case SemanticIndexChunkOptionKind.Overlap:
                    GenerateIdentifier(CodeGenerationSupporter.Overlap);
                    break;
            }
            GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
            GenerateSpace();
            GenerateFragmentIfNotNull(node.Value);
        }
    }
}
