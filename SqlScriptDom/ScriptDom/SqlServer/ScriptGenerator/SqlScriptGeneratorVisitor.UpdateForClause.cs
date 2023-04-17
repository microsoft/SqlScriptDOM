//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.UpdateForClause.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(UpdateForClause node)
        {
            GenerateKeyword(TSqlTokenType.Update);

            if (node.Columns.Count > 0)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.Of);
                GenerateSpace();
            }

            GenerateCommaSeparatedList(node.Columns);
        }
    }
}
