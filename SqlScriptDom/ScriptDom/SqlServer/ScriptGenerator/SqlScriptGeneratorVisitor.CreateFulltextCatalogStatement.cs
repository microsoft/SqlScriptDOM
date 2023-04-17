//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CreateFulltextCatalogStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CreateFullTextCatalogStatement node)
        {
            GenerateKeyword(TSqlTokenType.Create);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Fulltext);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Catalog);

            // name
            GenerateSpaceAndFragmentIfNotNull(node.Name);

            if (node.FileGroup != null)
            {
                NewLineAndIndent();
                GenerateKeyword(TSqlTokenType.On);
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Filegroup);

                GenerateSpaceAndFragmentIfNotNull(node.FileGroup);
            }

            if (node.Path != null)
            {
                NewLineAndIndent();
                GenerateKeyword(TSqlTokenType.In);
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Path);

                GenerateSpaceAndFragmentIfNotNull(node.Path);
            }

            GenerateCommaSeparatedWithClause(node.Options, true, false);

            if (node.IsDefault)
            {
                NewLineAndIndent();
                GenerateKeyword(TSqlTokenType.As);
                GenerateSpaceAndKeyword(TSqlTokenType.Default);
            }

            GenerateOwnerIfNotNull(node.Owner);
        }

        public override void ExplicitVisit(OnOffFullTextCatalogOption node)
        {
            System.Diagnostics.Debug.Assert(node.OptionKind == FullTextCatalogOptionKind.AccentSensitivity);
            GenerateOptionStateWithEqualSign(CodeGenerationSupporter.AccentSensitivity, node.OptionState);
        }

    }
}
