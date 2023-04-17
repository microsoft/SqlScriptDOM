//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.Subquery.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(ScalarSubquery node)
        {
            GenerateQueryExpressionInParentheses(node.QueryExpression);

			GenerateSpaceAndCollation(node.Collation);
        }
    }
}
