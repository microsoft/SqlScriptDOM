//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CreateExternalResourcePoolStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CreateExternalStreamStatement node)
        {
            GenerateKeyword(TSqlTokenType.Create);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.External);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Stream);
            GenerateExternalStreamStatementBody(node);
        }

        protected void GenerateExternalStreamStatementBody(ExternalStreamStatement node)
        {
            // external stream name
            GenerateSpaceAndFragmentIfNotNull(node.Name);

            NewLineAndIndent();
            GenerateKeywordAndSpace(TSqlTokenType.With);
            GenerateKeyword(TSqlTokenType.LeftParenthesis);

            // external stream location
            if (node.Location != null)
            {
                NewLineAndIndent();
                GenerateNameEqualsValue(CodeGenerationSupporter.Location, node.Location);
            }

            // external stream optional parameters
            GenerateExternalStreamOptions(node);

            // external stream Input options
            if (node.InputOptions != null)
            {
                GenerateSymbol(TSqlTokenType.Comma);
                NewLineAndIndent();
                GenerateNameEqualsValue(CodeGenerationSupporter.InputOptions, node.InputOptions);
            }

            // external stream Input options
            if (node.OutputOptions != null)
            {
                GenerateSymbol(TSqlTokenType.Comma);
                NewLineAndIndent();
                GenerateNameEqualsValue(CodeGenerationSupporter.OutputOptions, node.OutputOptions);
            }

            NewLineAndIndent();
            GenerateKeyword(TSqlTokenType.RightParenthesis);
        }

        private void GenerateExternalStreamOptions(ExternalStreamStatement node)
        {
            if (node.ExternalStreamOptions.Count > 0)
            {
                GenerateSymbol(TSqlTokenType.Comma);
                NewLineAndIndent();
                GenerateCommaSeparatedList(node.ExternalStreamOptions, true, true);
            }
        }
    }
}
