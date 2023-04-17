//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CreateStatisticsStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CreateStatisticsStatement node)
        {
            GenerateKeyword(TSqlTokenType.Create);
            GenerateSpaceAndKeyword(TSqlTokenType.Statistics);

            // name
            GenerateSpaceAndFragmentIfNotNull(node.Name);

            // ON
            NewLineAndIndent();
            GenerateKeyword(TSqlTokenType.On);
            GenerateSpaceAndFragmentIfNotNull(node.OnName);

            GenerateParenthesisedCommaSeparatedList(node.Columns);

            if (node.FilterPredicate != null)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.Where);
                GenerateSpaceAndFragmentIfNotNull(node.FilterPredicate);
            }

            if (node.StatisticsOptions != null && node.StatisticsOptions.Count > 0)
            {
                NewLineAndIndent();
                GenerateKeyword(TSqlTokenType.With);
                GenerateSpace();

                GenerateCommaSeparatedList(node.StatisticsOptions);
            }
        }
    }
}
