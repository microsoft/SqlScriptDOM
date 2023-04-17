//------------------------------------------------------------------------------
// <copyright company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Globalization;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        // Generates the statement
        // CREATE EXTERNAL LANGUAGE [{node.Name}]
        //     AUTHORIZATION [{node.Owner}]
        //     FROM (CONTENT = {content}, FILE_NAME = {fileName}, PLATFORM = {windows|linux}, PARAMETERS = {parameters},  ENVIRONMENT_VARIABLES = {environmentVariables})
        //
        public override void ExplicitVisit(CreateExternalLanguageStatement node)
        {
            GenerateKeyword(TSqlTokenType.Create);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.External);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Language);

            // name
            GenerateSpaceAndFragmentIfNotNull(node.Name);

            GenerateOwnerIfNotNull(node.Owner);

            NewLineAndIndent();
            GenerateKeyword(TSqlTokenType.From);
            GenerateSpace();

            bool first = true;
            foreach (ExternalLanguageFileOption file in node.ExternalLanguageFiles)
            {
                if (!first)
                {
                    GenerateSymbol(TSqlTokenType.Comma);
                }

                GenerateSymbol(TSqlTokenType.LeftParenthesis);

                if (file.Path != null && this._options.AllowExternalLanguagePaths)
                {
                    GenerateNameEqualsValue(CodeGenerationSupporter.Content, file.Path);
                }
                else
                {
                    GenerateNameEqualsValue(CodeGenerationSupporter.Content, file.Content);
                }

                GenerateSymbol(TSqlTokenType.Comma);
                GenerateSpace();
                GenerateNameEqualsValue(CodeGenerationSupporter.File_Name, file.FileName);

                if (file.Platform != null)
                {
                    GenerateSymbol(TSqlTokenType.Comma);
                    GenerateSpace();
                    GenerateNameEqualsValue(CodeGenerationSupporter.Platform, file.Platform);
                }

                if (file.Parameters != null)
                {
                    GenerateSymbol(TSqlTokenType.Comma);
                    GenerateSpace();
                    GenerateNameEqualsValue(CodeGenerationSupporter.Parameters, file.Parameters);
                }

                if (file.EnvironmentVariables != null)
                {
                    GenerateSymbol(TSqlTokenType.Comma);
                    GenerateSpace();
                    GenerateNameEqualsValue(CodeGenerationSupporter.EnvironmentVariables, file.EnvironmentVariables);
                }

                GenerateSymbol(TSqlTokenType.RightParenthesis);

                first = false;
            }
        }
    }
}
