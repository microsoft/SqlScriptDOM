//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.DistinctPredicate.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(DistinctPredicate node)
        {
            GenerateFragmentIfNotNull(node.FirstExpression);

            GenerateSpaceAndKeyword(TSqlTokenType.Is);

            if (node.IsNot)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.Not);
            }

            GenerateSpaceAndKeyword(TSqlTokenType.Distinct);

            GenerateSpaceAndKeyword(TSqlTokenType.From);

            GenerateSpaceAndFragmentIfNotNull(node.SecondExpression);
        }
    }
}
