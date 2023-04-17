//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterTableSwitchStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AlterTableSwitchStatement node)
        {
            GenerateAlterTableHead(node);

            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Switch); 

            GenerateForPartitionIfNotNull(node.SourcePartitionNumber);

            GenerateSpaceAndKeyword(TSqlTokenType.To);
            GenerateSpaceAndFragmentIfNotNull(node.TargetTable);

            GenerateForPartitionIfNotNull(node.TargetPartitionNumber);

            // With clause for options is introduced in V.12
            //
            if (_options.SqlVersion >= SqlVersion.Sql120 && node.Options.Count > 0)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.With);
                GenerateSpace();
                GenerateParenthesisedCommaSeparatedList(node.Options);
            }
        }

        private void GenerateForPartitionIfNotNull(ScalarExpression expression)
        {
            if (expression != null)
            {
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Partition);
                GenerateSpaceAndFragmentIfNotNull(expression);
            }
        }
    }
}
