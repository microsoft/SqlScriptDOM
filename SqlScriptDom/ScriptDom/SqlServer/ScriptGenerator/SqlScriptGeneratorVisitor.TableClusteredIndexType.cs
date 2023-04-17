//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.TableClusteredIndexType.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(TableClusteredIndexType node)
        {
            GenerateIdentifier(CodeGenerationSupporter.Clustered);
            if (node.ColumnStore == true)
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.ColumnStore);
            GenerateSpaceAndKeyword(TSqlTokenType.Index);

            if (node.ColumnStore == false)
               GenerateParenthesisedCommaSeparatedList(node.Columns);

            if(node.ColumnStore && node.OrderedColumns != null && node.OrderedColumns.Count > 0)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.Order);
                GenerateParenthesisedCommaSeparatedList(node.OrderedColumns);
            }
        }
    }
}