//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterPartitionFunctionStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AlterPartitionFunctionStatement node)
        {
            GenerateKeyword(TSqlTokenType.Alter);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Partition);
            GenerateSpaceAndKeyword(TSqlTokenType.Function);

            // name
            GenerateSpaceAndFragmentIfNotNull(node.Name);
            GenerateSymbolAndSpace(TSqlTokenType.LeftParenthesis);
            GenerateSymbol(TSqlTokenType.RightParenthesis);

            NewLineAndIndent();
            GenerateIdentifier(node.IsSplit ? CodeGenerationSupporter.Split : CodeGenerationSupporter.Merge);

            if (node.Boundary != null)
            {
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Range); 
                GenerateSpace();
                GenerateParenthesisedFragmentIfNotNull(node.Boundary);
            }
        }
    }
}
