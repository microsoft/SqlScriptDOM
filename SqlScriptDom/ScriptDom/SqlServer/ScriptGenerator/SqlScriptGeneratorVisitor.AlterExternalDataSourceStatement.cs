//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterExternalDataSourceStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AlterExternalDataSourceStatement node)
        {
            GenerateKeyword(TSqlTokenType.Alter);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.External);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Data);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Source);

            GenerateAlterExternalDataSourceStatementBody(node);
        }

        protected void GenerateAlterExternalDataSourceStatementBody(AlterExternalDataSourceStatement node)
        {
            // external data source name
            GenerateSpaceAndFragmentIfNotNull(node.Name);
            GenerateSpace();
            GenerateKeywordAndSpace(TSqlTokenType.Set);

            // external data source location
            if (node.Location != null)
            {
                GenerateNameEqualsValue(CodeGenerationSupporter.Location, node.Location);
            }

            // external data source pushdown option, place if altered
            if (_options.SqlVersion >= SqlVersion.Sql150 &&
                node.PushdownOption.HasValue &&
                node.PreviousPushDownOption != node.PushdownOption)
            {
                ExternalDataSourcePushdownOption pushdownOption = node.PushdownOption.Value;
                string externalDataSourcePushdownOption = GetValueForEnumKey(_externalDataSourcePushdownOption, pushdownOption);
                if (node.Location != null)
                {
                    GenerateSymbolAndSpace(TSqlTokenType.Comma);
                }
                GenerateNameEqualsValue(CodeGenerationSupporter.PushdownOption, externalDataSourcePushdownOption);
            }

            // external data source optional parameters
            GenerateAlterExternalDataSourceOptions(node);
        }

        private void GenerateAlterExternalDataSourceOptions(AlterExternalDataSourceStatement node)
        {
            if (node.ExternalDataSourceOptions.Count > 0)
            {
                if (node.Location != null ||
                    (_options.SqlVersion >= SqlVersion.Sql150 &&
                    node.PushdownOption.HasValue &&
                    node.PreviousPushDownOption != node.PushdownOption))
                {
                    GenerateSymbolAndSpace(TSqlTokenType.Comma);
                }

                GenerateCommaSeparatedList(node.ExternalDataSourceOptions);
            }
        }
    }
}
