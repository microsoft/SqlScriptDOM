//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CreateVectorIndexStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CreateVectorIndexStatement node)
        {
            GenerateKeyword(TSqlTokenType.Create);

            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Vector);

            GenerateSpaceAndKeyword(TSqlTokenType.Index);

            // name
            GenerateSpaceAndFragmentIfNotNull(node.Name);

            NewLineAndIndent();
            GenerateKeyword(TSqlTokenType.On);
            GenerateSpaceAndFragmentIfNotNull(node.OnName);

            // Vector column
            if (node.VectorColumn != null)
            {
                GenerateSpace();
                GenerateSymbol(TSqlTokenType.LeftParenthesis);
                GenerateFragmentIfNotNull(node.VectorColumn);
                GenerateSymbol(TSqlTokenType.RightParenthesis);
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