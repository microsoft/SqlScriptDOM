//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterTableSetStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AlterTableSetStatement node)
        {
            GenerateAlterTableHead(node);
            GenerateSpaceAndKeyword(TSqlTokenType.Set);
            GenerateSpace();
            GenerateParenthesisedCommaSeparatedList(node.Options);
        }
    }
}