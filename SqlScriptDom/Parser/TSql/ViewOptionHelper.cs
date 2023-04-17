//------------------------------------------------------------------------------
// <copyright file="ViewOptionHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    
    internal class ViewOptionHelper : OptionsHelper<ViewOptionKind>
    {
        private ViewOptionHelper()
        {
            AddOptionMapping(ViewOptionKind.Encryption, CodeGenerationSupporter.Encryption);
            AddOptionMapping(ViewOptionKind.SchemaBinding, CodeGenerationSupporter.SchemaBinding);
            AddOptionMapping(ViewOptionKind.ViewMetadata, CodeGenerationSupporter.ViewMetadata);
            AddOptionMapping(ViewOptionKind.Distribution, CodeGenerationSupporter.Distribution);
        }

        protected override TSqlParseErrorException GetMatchingException(antlr.IToken token)
        {
            return new TSqlParseErrorException(TSql80ParserBaseInternal.CreateParseError(
                "SQL46025", token, TSqlParserResource.SQL46025Message, token.getText()));
        }

        internal static readonly ViewOptionHelper Instance = new ViewOptionHelper();
    }
}
