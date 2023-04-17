//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.SelectColumn.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(SelectScalarExpression node)
        {
            GenerateFragmentIfNotNull(node.Expression);

            if (node.ColumnName != null)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.As);
                GenerateSpaceAndFragmentIfNotNull(node.ColumnName);
            }
        }
    }
}
