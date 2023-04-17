//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CreateExternalFileFormatStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CreateExternalFileFormatStatement node)
        {
            GenerateKeyword(TSqlTokenType.Create);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.External);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.File);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Format);

            GenerateExternalFileFormatStatementBody(node);
        }

        protected static Dictionary<ExternalFileFormatType, string> _externalFileFormatTypeNames = new Dictionary<ExternalFileFormatType, string>()
        {
            {ExternalFileFormatType.DelimitedText, CodeGenerationSupporter.DelimitedText},
            {ExternalFileFormatType.RcFile, CodeGenerationSupporter.RcFile},
            {ExternalFileFormatType.Orc, CodeGenerationSupporter.Orc},
            {ExternalFileFormatType.Parquet, CodeGenerationSupporter.Parquet},
            {ExternalFileFormatType.JSON, CodeGenerationSupporter.Json },
            {ExternalFileFormatType.Delta, CodeGenerationSupporter.Delta },
        };
        
        protected void GenerateExternalFileFormatStatementBody(ExternalFileFormatStatement node)
        {
            // external file format name
            GenerateSpaceAndFragmentIfNotNull(node.Name);

            NewLineAndIndent();
            GenerateKeywordAndSpace(TSqlTokenType.With);
            GenerateKeyword(TSqlTokenType.LeftParenthesis);

            // external file format type
            string externalFileFormatTypeName = GetValueForEnumKey(_externalFileFormatTypeNames, node.FormatType);
            if (!string.IsNullOrEmpty(externalFileFormatTypeName))
            {
                NewLineAndIndent();
                GenerateNameEqualsValue(CodeGenerationSupporter.FormatType, externalFileFormatTypeName);
            }
            
            // external file format options
            GenerateExternalFileFormatOptions(node);

            NewLineAndIndent();
            GenerateKeyword(TSqlTokenType.RightParenthesis);
        }

        private void GenerateExternalFileFormatOptions(ExternalFileFormatStatement node)
        {
            if (node.ExternalFileFormatOptions.Count > 0)
            {
                GenerateSymbol(TSqlTokenType.Comma);
                NewLineAndIndent();
                GenerateCommaSeparatedList(node.ExternalFileFormatOptions, true, true);
            }
        }
    }
}
