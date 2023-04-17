//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.InPredicate.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(InPredicate node)
        {
            GenerateFragmentIfNotNull(node.Expression);

            if (node.NotDefined)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.Not);
            }

            GenerateSpaceAndKeyword(TSqlTokenType.In);

            if (node.Values.Count > 0)
            {
                GenerateSpace();
                GenerateParenthesisedCommaSeparatedList(node.Values);
            }

            GenerateSpaceAndFragmentIfNotNull(node.Subquery);
        }
    }
}
