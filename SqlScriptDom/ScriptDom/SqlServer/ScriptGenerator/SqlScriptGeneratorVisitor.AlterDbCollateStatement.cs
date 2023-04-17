//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterDbCollateStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AlterDatabaseCollateStatement node)
        {
            GenerateAlterDbStatementHead(node);

            GenerateSpaceAndCollation(node.Collation);
        }
    }
}
