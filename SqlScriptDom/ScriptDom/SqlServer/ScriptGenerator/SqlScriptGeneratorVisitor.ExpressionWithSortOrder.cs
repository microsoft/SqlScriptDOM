//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.ExpressionWithSortOrder.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        protected static Dictionary<SortOrder, TokenGenerator> _sortOrderGenerators = new Dictionary<SortOrder, TokenGenerator>()
        {
            { SortOrder.Ascending, new KeywordGenerator(TSqlTokenType.Asc)},
            { SortOrder.Descending, new KeywordGenerator(TSqlTokenType.Desc)},
            { SortOrder.NotSpecified, new EmptyGenerator()},
        };
  
        public override void ExplicitVisit(ExpressionWithSortOrder node)
        {
            GenerateFragmentIfNotNull(node.Expression);

            TokenGenerator generator = GetValueForEnumKey(_sortOrderGenerators, node.SortOrder);
            if (generator != null && node.SortOrder != SortOrder.NotSpecified)
            {
                GenerateSpace();
                GenerateToken(generator);
            }
        }
    }
}
