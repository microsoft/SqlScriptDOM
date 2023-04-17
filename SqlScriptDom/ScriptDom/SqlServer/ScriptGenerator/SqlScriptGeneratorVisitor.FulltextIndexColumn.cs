//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.FulltextIndexColumn.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(FullTextIndexColumn node)
        {
            GenerateFragmentIfNotNull(node.Name);

            if (node.TypeColumn != null)
            {
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Type);
                GenerateSpaceAndKeyword(TSqlTokenType.Column);
                GenerateSpaceAndFragmentIfNotNull(node.TypeColumn);
            }

            if (node.LanguageTerm != null)
            {
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Language);
                GenerateSpaceAndFragmentIfNotNull(node.LanguageTerm);
            }

            if (node.StatisticalSemantics)
            {
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.StatisticalSemantics);
            }
        }
    }
}