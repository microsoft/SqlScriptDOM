//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.ColumnWithSortOrder.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(ColumnWithSortOrder node)
        {
            GenerateFragmentIfNotNull(node.Column);

            switch (node.SortOrder)
            {
                case SortOrder.NotSpecified:
                    break;
                case SortOrder.Ascending:
                    GenerateSpaceAndKeyword(TSqlTokenType.Asc); 
                    break;
                case SortOrder.Descending:
                    GenerateSpaceAndKeyword(TSqlTokenType.Desc); 
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false, "An unhandled option is encountered.");
                    break;
            }
        }
    }
}
