// <copyright file="SqlScriptGeneratorVisitor.FileTableDirectoryTableOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Diagnostics;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {

        /// <summary>
        /// Script out the Create Table statement with RemotDataArchive.
        /// </summary>
        /// <remarks>
        ///
        /// We are generating the following syntax:
        ///
        /// CREATE TABLE table_name
        /// WITH REMOTE_DATA_ARCHIVE = { ON | OFF | OFF_WITHOUT_DATA_RECOVERY}
        /// [ ( MIGRATION_STATE = { PAUSED | OUTBOUND | INBOUND } ) ]
        ///
        /// </remarks>
        public override void ExplicitVisit(RemoteDataArchiveTableOption node)
        {
            Debug.Assert(node.OptionKind == TableOptionKind.RemoteDataArchive, "TableOption does not match");

            GenerateIdentifier(CodeGenerationSupporter.RemoteDataArchive);
            GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
            GenerateSpace();
            RdaTableOptionHelper.Instance.GenerateSourceForOption(_writer, node.RdaTableOption);
            GenerateSpaceAndSymbol(TSqlTokenType.LeftParenthesis);
            GenerateIdentifier(CodeGenerationSupporter.MigrationState);
            GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
            GenerateSpace();
            MigrationStateHelper.Instance.GenerateSourceForOption(_writer, node.MigrationState);
            GenerateSymbol(TSqlTokenType.RightParenthesis);
        }

        /// <summary>
        /// Script out the Alter Table statement with RemotDataArchive.
        /// </summary>
        /// <remarks>
        ///
        /// We are generating the following syntax:
        ///
        /// ALTER TABLE table_name
        /// SET (REMOTE_DATA_ARCHIVE = {ON | OFF | OFF_WITHOUT_DATA_RECOVERY}
        /// [
        ///     (
        ///     MIGRATION_STATE = { PAUSED | OUTBOUND | INBOUND } 
        ///     [ FILTER_PREDICATE = { NULL | schema.function } ]
        ///     )
        /// ]
        /// )
        /// 
        ///
        /// </remarks>
        public override void ExplicitVisit(RemoteDataArchiveAlterTableOption node)
        {
            Debug.Assert(node.OptionKind == TableOptionKind.RemoteDataArchive, "TableOption does not match");

            GenerateIdentifier(CodeGenerationSupporter.RemoteDataArchive);
            GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
            GenerateSpace();
            RdaTableOptionHelper.Instance.GenerateSourceForOption(_writer, node.RdaTableOption);

            if (node.IsMigrationStateSpecified || node.IsFilterPredicateSpecified)
            {
                GenerateSpaceAndSymbol(TSqlTokenType.LeftParenthesis);

                if (node.IsMigrationStateSpecified)
                {
                    GenerateIdentifier(CodeGenerationSupporter.MigrationState);
                    GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
                    GenerateSpace();
                    MigrationStateHelper.Instance.GenerateSourceForOption(_writer, node.MigrationState);
                }

                if (node.IsMigrationStateSpecified && node.IsFilterPredicateSpecified)
                {
                    GenerateSymbolAndSpace(TSqlTokenType.Comma);
                }

                if (node.IsFilterPredicateSpecified && node.FilterPredicate != null)
                {
                    GenerateIdentifier(CodeGenerationSupporter.FilterPredicate);
                    GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
                    GenerateSpaceAndFragmentIfNotNull(node.FilterPredicate);
                }
                else if (node.IsFilterPredicateSpecified)
                {
                    GenerateIdentifier(CodeGenerationSupporter.FilterPredicate);
                    GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
                    GenerateSpaceAndSymbol(TSqlTokenType.Null);
                }

                GenerateSymbol(TSqlTokenType.RightParenthesis);
            }
        }
    }
}
