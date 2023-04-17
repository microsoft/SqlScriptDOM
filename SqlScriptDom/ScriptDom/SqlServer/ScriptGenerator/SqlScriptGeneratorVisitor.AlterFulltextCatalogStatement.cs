//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterFulltextCatalogStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AlterFullTextCatalogStatement node)
        {
            GenerateSpaceSeparatedTokens(TSqlTokenType.Alter, CodeGenerationSupporter.Fulltext, CodeGenerationSupporter.Catalog);

            // name
            GenerateSpaceAndFragmentIfNotNull(node.Name);

            GenerateSpace();
            switch (node.Action)
            {
                case AlterFullTextCatalogAction.AsDefault:
                    GenerateSpaceSeparatedTokens(TSqlTokenType.As, TSqlTokenType.Default); 
                    break;
                case AlterFullTextCatalogAction.Rebuild:
                    GenerateIdentifier(CodeGenerationSupporter.Rebuild);
                    if (node.Options != null && node.Options.Count > 0)
                    {
                        GenerateSpace();
                        GenerateSymbolAndSpace(TSqlTokenType.With);
                        GenerateCommaSeparatedList(node.Options);
                    }
                    break;
                case AlterFullTextCatalogAction.Reorganize:
                    GenerateIdentifier(CodeGenerationSupporter.Reorganize); 
                    break;
                case AlterFullTextCatalogAction.None:
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false, "An unhandled option is encountered.");
                    break;
            }
        }
    }
}
