//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterTableStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        protected void GenerateAlterTableHead(AlterTableStatement node)
        {
            GenerateKeyword(TSqlTokenType.Alter);
            GenerateSpaceAndKeyword(TSqlTokenType.Table);

            // name
            GenerateSpaceAndFragmentIfNotNull(node.SchemaObjectName);
        }

        protected void GenerateConstraintEnforcement(ConstraintEnforcement enforcement)
        {
            switch (enforcement)
            {
                case ConstraintEnforcement.NoCheck:
                    GenerateKeyword(TSqlTokenType.NoCheck);
                    break;
                case ConstraintEnforcement.Check:
                    GenerateKeyword(TSqlTokenType.Check);
                    break;
                case ConstraintEnforcement.NotSpecified:
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false, "An unhandled option is encountered.");
                    break;
            }
        }
    }
}
