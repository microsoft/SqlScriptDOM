//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CreateJsonIndexStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CreateJsonIndexStatement node)
        {
            GenerateKeyword(TSqlTokenType.Create);

            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Json);

            GenerateSpaceAndKeyword(TSqlTokenType.Index);

            // name
            GenerateSpaceAndFragmentIfNotNull(node.Name);

            NewLineAndIndent();
            GenerateKeyword(TSqlTokenType.On);
            GenerateSpaceAndFragmentIfNotNull(node.OnName);

            // JSON column
            if (node.JsonColumn != null)
            {
                GenerateSpace();
                GenerateSymbol(TSqlTokenType.LeftParenthesis);
                GenerateFragmentIfNotNull(node.JsonColumn);
                GenerateSymbol(TSqlTokenType.RightParenthesis);
            }

            // FOR clause with JSON paths
            if (node.ForJsonPaths != null && node.ForJsonPaths.Count > 0)
            {
                NewLineAndIndent();
                GenerateKeyword(TSqlTokenType.For);
                GenerateSpace();
                GenerateParenthesisedCommaSeparatedList(node.ForJsonPaths);
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