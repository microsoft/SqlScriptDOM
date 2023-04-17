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
        // CREATE EXTERNAL LIBRARY [{node.Name}]
        //     AUTHORIZATION [{node.Owner}]
        //     SET (CONTENT = {content}, PLATFORM = {WINDOWS | LINUX})
        //     WITH (LANGUAGE = '{language}')
        //
        public override void ExplicitVisit(CreateExternalLibraryStatement node)
        {
            GenerateKeyword(TSqlTokenType.Create);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.External);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Library);

            // name
            GenerateSpaceAndFragmentIfNotNull(node.Name);

            GenerateOwnerIfNotNull(node.Owner);

            NewLineAndIndent();
            GenerateKeyword(TSqlTokenType.From);
            GenerateSpace();

            bool first = true;
            foreach (var file in node.ExternalLibraryFiles)
            {
                if (!first)
                {
                    GenerateSymbol(TSqlTokenType.Comma);
                }

                GenerateSymbol(TSqlTokenType.LeftParenthesis);

                if (file.Path != null && this._options.AllowExternalLibraryPaths)
                {
                    GenerateNameEqualsValue(CodeGenerationSupporter.Content, file.Path);
                }
                else
                {
                    GenerateNameEqualsValue(CodeGenerationSupporter.Content, file.Content);
                }

                if (this._options.SqlVersion > SqlVersion.Sql140 && file.Platform != null)
                {
                    GenerateSymbol(TSqlTokenType.Comma);
                    GenerateSpace();
                    GenerateNameEqualsValue(CodeGenerationSupporter.Platform, file.Platform);
                }

                GenerateSymbol(TSqlTokenType.RightParenthesis);

                first = false;
            }

            NewLineAndIndent();
            GenerateKeyword(TSqlTokenType.With);
            GenerateSpace();

            GenerateSymbol(TSqlTokenType.LeftParenthesis);
            GenerateNameEqualsValue(CodeGenerationSupporter.Language, node.Language);
            GenerateSymbol(TSqlTokenType.RightParenthesis);
        }
    }
}
