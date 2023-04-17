//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CoalesceExpression.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CoalesceExpression node)
        {
            GenerateKeyword(TSqlTokenType.Coalesce);

            GenerateSpace();
            GenerateParenthesisedCommaSeparatedList(node.Expressions);

			GenerateSpaceAndCollation(node.Collation);
		}
    }
}
