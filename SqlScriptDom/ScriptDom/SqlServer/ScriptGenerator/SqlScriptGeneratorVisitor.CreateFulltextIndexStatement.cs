//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CreateFulltextIndexStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Diagnostics;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CreateFullTextIndexStatement node)
        {
            GenerateSpaceSeparatedTokens(
                TSqlTokenType.Create, 
                CodeGenerationSupporter.Fulltext);
            GenerateSpace();
            GenerateSpaceSeparatedTokens(
                TSqlTokenType.Index, 
                TSqlTokenType.On); 

            GenerateSpaceAndFragmentIfNotNull(node.OnName);

            if (node.FullTextIndexColumns != null && node.FullTextIndexColumns.Count > 0)
            {
                NewLineAndIndent();
                GenerateParenthesisedCommaSeparatedList(node.FullTextIndexColumns);
            }

            NewLineAndIndent();
            GenerateKeyword(TSqlTokenType.Key);
            GenerateSpaceAndKeyword(TSqlTokenType.Index);

            GenerateSpaceAndFragmentIfNotNull(node.KeyIndexName);

            GenerateFragmentIfNotNull(node.CatalogAndFileGroup);

            if (node.Options.Count > 0)
            {
                NewLineAndIndent();
                GenerateKeywordAndSpace(TSqlTokenType.With);
                GenerateCommaSeparatedList(node.Options);
            }
        }

        public override void ExplicitVisit(FullTextCatalogAndFileGroup node)
        {
            NewLineAndIndent();
            GenerateKeywordAndSpace(TSqlTokenType.On);
            if (node.FileGroupIsFirst)
            {
                GenerateSymbol(TSqlTokenType.LeftParenthesis);
                GenerateIdentifier(CodeGenerationSupporter.Filegroup);
                GenerateSpaceAndFragmentIfNotNull(node.FileGroupName);

                if (node.CatalogName != null)
                {
                    GenerateSymbolAndSpace(TSqlTokenType.Comma);
                    GenerateFragmentIfNotNull(node.CatalogName);
                }
                GenerateSymbol(TSqlTokenType.RightParenthesis);
            }
            else
            {
                if (node.FileGroupName != null)
                    GenerateSymbol(TSqlTokenType.LeftParenthesis);

                GenerateFragmentIfNotNull(node.CatalogName);

                if (node.FileGroupName != null)
                {
                    GenerateSymbolAndSpace(TSqlTokenType.Comma);
                    GenerateIdentifier(CodeGenerationSupporter.Filegroup);
                    GenerateSpaceAndFragmentIfNotNull(node.FileGroupName);
                    GenerateSymbol(TSqlTokenType.RightParenthesis);
                }
            }
        }

        public override void ExplicitVisit(ChangeTrackingFullTextIndexOption node)
        {
            System.Diagnostics.Debug.Assert(node.OptionKind == FullTextIndexOptionKind.ChangeTracking);
            GenerateIdentifier(CodeGenerationSupporter.ChangeTracking);
            GenerateSpace();
            switch (node.Value)
            {
                case ChangeTrackingOption.Auto:
                    GenerateIdentifier(CodeGenerationSupporter.Auto);
                    break;
                case ChangeTrackingOption.Manual:
                    GenerateIdentifier(CodeGenerationSupporter.Manual);
                    break;
                case ChangeTrackingOption.Off:
                    GenerateKeyword(TSqlTokenType.Off);
                    break;
                case ChangeTrackingOption.OffNoPopulation:
                    GenerateKeyword(TSqlTokenType.Off);
                    GenerateSymbol(TSqlTokenType.Comma);
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.No);
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Population);
                    break;
                default:
                    Debug.Assert(false); // Unknown option
                    break;
            }
        }

        public override void ExplicitVisit(StopListFullTextIndexOption node)
        {
            System.Diagnostics.Debug.Assert(node.OptionKind == FullTextIndexOptionKind.StopList);
            GenerateKeywordAndSpace(TSqlTokenType.StopList);
            if (node.IsOff)
                GenerateKeyword(TSqlTokenType.Off);
            else
                GenerateFragmentIfNotNull(node.StopListName);
        }

        public override void ExplicitVisit(SearchPropertyListFullTextIndexOption node)
        {
            System.Diagnostics.Debug.Assert(node.OptionKind == FullTextIndexOptionKind.SearchPropertyList);
            GenerateIdentifier(CodeGenerationSupporter.Search);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Property);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.List);
            GenerateSpaceAndKeyword(TSqlTokenType.EqualsSign);
            if (node.IsOff)
                GenerateSpaceAndKeyword(TSqlTokenType.Off);
            else
                GenerateSpaceAndFragmentIfNotNull(node.PropertyListName);
        }


    }
}
