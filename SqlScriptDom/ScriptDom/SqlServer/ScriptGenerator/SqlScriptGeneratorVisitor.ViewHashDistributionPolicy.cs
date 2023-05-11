//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.ViewHashDistributionPolicy.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(ViewHashDistributionPolicy node)
        {
            GenerateIdentifier(CodeGenerationSupporter.Hash);
            GenerateSymbol(TSqlTokenType.LeftParenthesis);

            if (node.DistributionColumns?.Count > 0)
            {
                GenerateCommaSeparatedList(node.DistributionColumns);
            }

            GenerateSymbol(TSqlTokenType.RightParenthesis);
        }
    }
}