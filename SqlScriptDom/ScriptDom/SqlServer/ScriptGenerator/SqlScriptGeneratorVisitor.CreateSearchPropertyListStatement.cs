//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CreateSearchPropertyListStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CreateSearchPropertyListStatement node)
        {
            GenerateSpaceSeparatedTokens(TSqlTokenType.Create,
                CodeGenerationSupporter.Search,
                CodeGenerationSupporter.Property,
                CodeGenerationSupporter.List);
            GenerateSpaceAndFragmentIfNotNull(node.Name);
            if (node.SourceSearchPropertyList != null)
            {
                NewLine();
                GenerateKeyword(TSqlTokenType.From);
                GenerateSpaceAndFragmentIfNotNull(node.SourceSearchPropertyList);
            }
            GenerateOwnerIfNotNull(node.Owner);
        }
    }
}
