//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.UpdateStatisticsStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(UpdateStatisticsStatement node)
        {
            GenerateKeyword(TSqlTokenType.Update);
            GenerateSpaceAndKeyword(TSqlTokenType.Statistics);
            GenerateSpaceAndFragmentIfNotNull(node.SchemaObjectName);

            if (node.SubElements.Count > 0)
            {
                GenerateSpace();
                GenerateParenthesisedCommaSeparatedList(node.SubElements);
            }

            if (node.StatisticsOptions.Count > 0)
            {
                NewLineAndIndent();
                GenerateKeyword(TSqlTokenType.With);
                GenerateSpace();
                GenerateCommaSeparatedList(node.StatisticsOptions);
            }
        }
    }
}
