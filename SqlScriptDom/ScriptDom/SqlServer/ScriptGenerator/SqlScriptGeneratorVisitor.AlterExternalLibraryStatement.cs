//------------------------------------------------------------------------------
// <copyright company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        // Generates the statement
        // ALTER EXTERNAL LIBRARY [{node.Name}]
        //     AUTHORIZATION [{node.Owner}]
        //     SET (CONTENT = {content}, PLATFORM = {WINDOWS | LINUX})
        //     WITH (LANGUAGE = '{language}')
        //
        public override void ExplicitVisit(AlterExternalLibraryStatement node)
        {
            GenerateKeyword(TSqlTokenType.Alter);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.External);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Library);

            // name
            GenerateSpaceAndFragmentIfNotNull(node.Name);

            GenerateOwnerIfNotNull(node.Owner);

            NewLineAndIndent();
            GenerateKeyword(TSqlTokenType.Set);
            GenerateSpace();

            GenerateSymbol(TSqlTokenType.LeftParenthesis);

            var file = node.ExternalLibraryFiles[0];
            if (file.Path != null && this._options.AllowExternalLibraryPaths)
                GenerateNameEqualsValue(CodeGenerationSupporter.Content, file.Path);
            else
                GenerateNameEqualsValue(CodeGenerationSupporter.Content, file.Content);

            if (this._options.SqlVersion > SqlVersion.Sql140 && file.Platform != null)
            {
                GenerateSymbol(TSqlTokenType.Comma);
                GenerateSpace();
                GenerateNameEqualsValue(CodeGenerationSupporter.Platform, file.Platform);
            }

            GenerateSymbol(TSqlTokenType.RightParenthesis);

            NewLineAndIndent();
            GenerateKeyword(TSqlTokenType.With);
            GenerateSpace();

            GenerateSymbol(TSqlTokenType.LeftParenthesis);
            GenerateNameEqualsValue(CodeGenerationSupporter.Language, node.Language);
            GenerateSymbol(TSqlTokenType.RightParenthesis);
        }
    }
}
