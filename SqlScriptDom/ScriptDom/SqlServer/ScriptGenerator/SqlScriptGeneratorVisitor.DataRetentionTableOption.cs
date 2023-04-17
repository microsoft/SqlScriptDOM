//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.DataRetentionTableOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Diagnostics;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(DataRetentionTableOption node)
        {
            // The following statement can be generated but we never generate SET (DATA_DELETION = ON (FILTER_COLUMN = column_name))
            // because on the server side changing just the filter column may change the retention period value. So we use a more
            // restricitve policy and generate both options (Filter column and Retention period).
            //
            // CREATE TABLE table_name
            // WITH ( DATA_DELETON = OFF )
            //
            // CREATE TABLE table_name
            // WITH ( DATA_DELETION = ON ( FILTER_COLUMN = column_name, RETENTION_PERIOD = infinty | period_value period_unit ) )
            //
            // ALTER TABLE table_name
            // SET (DATA_DELETION = ON (FILTER_COLUMN = column_name, RETENTION_PERIOD = 1 day))
            //
            // ALTER TABLE table_name
            // SET (DATA_DELETION = ON (FILTER_COLUMN = column_name))
            //
            // ALTER TABLE table_name
            // SET (DATA_DELETION = OFF)
            //
            Debug.Assert(node.OptionKind == TableOptionKind.DataRetention);
            GenerateOptionStateWithEqualSign(CodeGenerationSupporter.DataDeletion, node.OptionState);

            if (node.OptionState == OptionState.On)
            {
                GenerateSpace();
                GenerateSymbolAndSpace(TSqlTokenType.LeftParenthesis);
                Debug.Assert(node.FilterColumn != null, "Cannot generate a Data Retention statement without filter column");
                GenerateNameEqualsValue(CodeGenerationSupporter.FilterColumn, node.FilterColumn);
                Debug.Assert(node.RetentionPeriod != null, "Although retention period can be null we deliberately set a default in the parser to explicitly state what an empty retention period means");

                GenerateKeyword(TSqlTokenType.Comma);
                GenerateSpace();

                GenerateFragmentIfNotNull(node.RetentionPeriod);

                GenerateSpace();
                GenerateSymbolAndSpace(TSqlTokenType.RightParenthesis);
            }
        }

        // Generates the Retention period definition
        // RETENTION_PERIOD = infinty | duration unit
        //
        public override void ExplicitVisit(RetentionPeriodDefinition node)
        {
            GenerateIdentifier(CodeGenerationSupporter.RetentionPeriod);
            GenerateSpace();
            GenerateSymbolAndSpace(TSqlTokenType.EqualsSign);

            if (node.IsInfinity)
            {
                GenerateIdentifier(CodeGenerationSupporter.Infinite);
            }
            else
            {
                GenerateFragmentIfNotNull(node.Duration);
                GenerateSpace();
                TemporalRetentionPeriodUnitHelper.Instance.GenerateSourceForOption(_writer, node.Units);
            }
        }
    }
}
