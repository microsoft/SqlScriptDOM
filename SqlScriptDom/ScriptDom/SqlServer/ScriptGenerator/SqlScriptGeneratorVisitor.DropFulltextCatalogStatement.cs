//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.DropFulltextCatalogStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(DropFullTextCatalogStatement node)
        {
            GenerateKeyword(TSqlTokenType.Drop);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Fulltext);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Catalog);

            GenerateSpaceAndFragmentIfNotNull(node.Name);
        }
    }
}
