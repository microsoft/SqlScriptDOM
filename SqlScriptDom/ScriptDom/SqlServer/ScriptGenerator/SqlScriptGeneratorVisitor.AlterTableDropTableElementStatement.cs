//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterTableDropTableElementStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AlterTableDropTableElementStatement node)
        {
            GenerateAlterTableHead(node);

            GenerateSpaceAndKeyword(TSqlTokenType.Drop);
            GenerateSpace();

            GenerateCommaSeparatedList(node.AlterTableDropTableElements);
        }
    }
}
