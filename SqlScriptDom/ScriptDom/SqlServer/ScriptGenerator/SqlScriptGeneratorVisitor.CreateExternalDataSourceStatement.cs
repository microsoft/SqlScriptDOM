//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CreateExternalDataSourceStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CreateExternalDataSourceStatement node)
        {
            GenerateKeyword(TSqlTokenType.Create);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.External);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Data);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Source);

            GenerateExternalDataSourceStatementBody(node);
        }

        protected static Dictionary<ExternalDataSourceType, string> _externalDataSourceTypeNames = new Dictionary<ExternalDataSourceType, string>()
        {
            {ExternalDataSourceType.HADOOP, CodeGenerationSupporter.Hadoop},
            {ExternalDataSourceType.RDBMS, CodeGenerationSupporter.Rdbms},
            {ExternalDataSourceType.SHARD_MAP_MANAGER, CodeGenerationSupporter.ShardMapManager},
            {ExternalDataSourceType.BLOB_STORAGE, CodeGenerationSupporter.BlobStorage}
        };

        protected static Dictionary<ExternalDataSourcePushdownOption, string> _externalDataSourcePushdownOption = new Dictionary<ExternalDataSourcePushdownOption, string>()
        {
            {ExternalDataSourcePushdownOption.ON, CodeGenerationSupporter.On},
            {ExternalDataSourcePushdownOption.OFF, CodeGenerationSupporter.Off}
        };

        protected void GenerateExternalDataSourceStatementBody(ExternalDataSourceStatement node)
        {
            // external data source name
            GenerateSpaceAndFragmentIfNotNull(node.Name);
            
            NewLineAndIndent();
            GenerateKeywordAndSpace(TSqlTokenType.With);
            GenerateKeyword(TSqlTokenType.LeftParenthesis);
            
            // external data source type
            if (node.DataSourceType != ExternalDataSourceType.EXTERNAL_GENERICS)
            {
                string externalDataSourceTypeName = GetValueForEnumKey(_externalDataSourceTypeNames, node.DataSourceType);
                if (!string.IsNullOrEmpty(externalDataSourceTypeName))
                {
                    NewLineAndIndent();
                    GenerateNameEqualsValue(CodeGenerationSupporter.Type, externalDataSourceTypeName);
                    GenerateSymbol(TSqlTokenType.Comma);
                }
            }

            // external data source location
            if (node.Location != null)
            {
                NewLineAndIndent();
                GenerateNameEqualsValue(CodeGenerationSupporter.Location, node.Location);
            }

            // external data source pushdown option, place in create script only when off or
            // if the external data source type is external generic
            // PUSHDOWN is supported when connecting to SQL Server, Oracle, Teradata, MongoDB, or ODBC 
            // at the external data source level.
            if (_options.SqlVersion >= SqlVersion.Sql150 &&
                node.PushdownOption.HasValue &&
                (node.PushdownOption == ExternalDataSourcePushdownOption.OFF ||
                node.DataSourceType == ExternalDataSourceType.EXTERNAL_GENERICS))
            {
                ExternalDataSourcePushdownOption pushdownOption = node.PushdownOption.Value;
                string externalDataSourcePushdownOption = GetValueForEnumKey(_externalDataSourcePushdownOption, pushdownOption);
                GenerateSymbol(TSqlTokenType.Comma);
                NewLineAndIndent();
                GenerateNameEqualsValue(CodeGenerationSupporter.PushdownOption, externalDataSourcePushdownOption);
            }

            // external data source optional parameters
            GenerateExternalDataSourceOptions(node);

            NewLineAndIndent();
            GenerateKeyword(TSqlTokenType.RightParenthesis);
        }

        private void GenerateExternalDataSourceOptions(ExternalDataSourceStatement node)
        {
            if (node.ExternalDataSourceOptions.Count > 0)
            {
                GenerateSymbol(TSqlTokenType.Comma);
                NewLineAndIndent();
                GenerateCommaSeparatedList(node.ExternalDataSourceOptions, true, true);
            }
        }
    }
}
