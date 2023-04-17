//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.ExternalTableColumnDefinition.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Diagnostics;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(ExternalTableColumnDefinition node)
        {
            GenerateFragmentIfNotNull(node.ColumnDefinition.ColumnIdentifier);

            // Special case (undocumented) for Serverless SQL pools - external table may contain computed columns which
            // represent partitioning-related columns for SPARK-backed databases.
            //
            // Users can't create such tables explicitly. But we can encounter such case when reverse engineering SPARK database.
            //
            if ((node.ColumnDefinition is ColumnDefinition) && (node.ColumnDefinition as ColumnDefinition).ComputedColumnExpression != null)
            {
                // Computed columns special case
                //
                GenerateSpace();
                GenerateKeyword(TSqlTokenType.As);
                GenerateSpace();
                GenerateFragmentIfNotNull((node.ColumnDefinition as ColumnDefinition).ComputedColumnExpression);
            }
            else
            {
                GenerateSpaceAndFragmentIfNotNull(node.ColumnDefinition.DataType);

                if (node.ColumnDefinition.Collation != null)
                {
                    GenerateSpaceAndCollation(node.ColumnDefinition.Collation);
                }

                if (node.NullableConstraint != null)
                {
                    GenerateSpaceAndFragmentIfNotNull(node.NullableConstraint);
                }
            }
        }
    }
}
