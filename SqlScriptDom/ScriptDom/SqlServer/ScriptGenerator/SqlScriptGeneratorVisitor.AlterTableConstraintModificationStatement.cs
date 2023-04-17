//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterTableConstraintModificationStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AlterTableConstraintModificationStatement node)
        {
            GenerateAlterTableHead(node);

            if (node.ExistingRowsCheckEnforcement != ConstraintEnforcement.NotSpecified)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.With);
                GenerateSpace();
                GenerateConstraintEnforcement(node.ExistingRowsCheckEnforcement);
            }

            GenerateSpace();
            GenerateConstraintEnforcement(node.ConstraintEnforcement);

            GenerateSpaceAndKeyword(TSqlTokenType.Constraint);
            GenerateSpace();

            if (node.All)
            {
                GenerateKeyword(TSqlTokenType.All);
            }
            else
            {
                GenerateCommaSeparatedList(node.ConstraintNames);
            }
        }
    }
}
