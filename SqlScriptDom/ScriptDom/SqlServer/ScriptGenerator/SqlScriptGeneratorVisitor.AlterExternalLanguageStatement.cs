//------------------------------------------------------------------------------
// <copyright company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        // Generates the statement
        // ALTER EXTERNAL LANGUAGE [{node.Name}]
        //     AUTHORIZATION [{node.Owner}]
        //     {SET|ADD} (CONTENT = {content}, FILE_NAME = {fileName}, PLATFORM = {windows|linux}, PARAMETERS = {parameters},  ENVIRONMENT_VARIABLES = {environmentVariables})
        // OR
        // ALTER EXTERNAL LANGUAGE [{node.Name}]
        //     AUTHORIZATION [{node.Owner}]
        //     REMOVE PLATFORM {windows|linux}
        // 
        //
        public override void ExplicitVisit(AlterExternalLanguageStatement node)
        {
            GenerateKeyword(TSqlTokenType.Alter);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.External);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Language);

            // name
            GenerateSpaceAndFragmentIfNotNull(node.Name);

            GenerateOwnerIfNotNull(node.Owner);
            NewLineAndIndent();

            // SET is the default Alter operation
            if (node.Operation == null)
            {
                node.Operation = new Identifier();
                node.Operation.Value = "SET";
            }

            if (node.Operation.Value.Equals("Remove", StringComparison.OrdinalIgnoreCase))
            {
                GenerateIdentifierWithoutCasing(CodeGenerationSupporter.Remove);
                GenerateSpace();
                GenerateIdentifierWithoutCasing(CodeGenerationSupporter.Platform);

                GenerateSpaceAndFragmentIfNotNull(node.Platform);
            }
            else
            {
                if (node.Operation.Value.Equals("Set", StringComparison.OrdinalIgnoreCase))
                {
                    GenerateKeyword(TSqlTokenType.Set);
                }
                else if (node.Operation.Value.Equals("Add", StringComparison.OrdinalIgnoreCase))
                {
                    GenerateKeyword(TSqlTokenType.Add);
                }

                GenerateSpace();

                bool first = true;
                foreach (ExternalLanguageFileOption file in node.ExternalLanguageFiles)
                {
                    if (!first)
                    {
                        GenerateSymbol(TSqlTokenType.Comma);
                    }

                    GenerateExternalLanguageFileFragment(file);

                    first = false;
                }
            }
        }

        private void GenerateExternalLanguageFileFragment(ExternalLanguageFileOption file)
        {

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
        }
    }
}
