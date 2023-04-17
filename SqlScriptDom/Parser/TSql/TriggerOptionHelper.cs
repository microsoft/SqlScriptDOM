//------------------------------------------------------------------------------
// <copyright file="TriggerOptionHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    
    [Serializable]
    internal class TriggerOptionHelper : OptionsHelper<TriggerOptionKind>
    {
        private TriggerOptionHelper()
        {
            AddOptionMapping(TriggerOptionKind.Encryption, CodeGenerationSupporter.Encryption);
            AddOptionMapping(TriggerOptionKind.NativeCompile, CodeGenerationSupporter.NativeCompilation, SqlVersionFlags.TSql130AndAbove);
            AddOptionMapping(TriggerOptionKind.SchemaBinding, CodeGenerationSupporter.SchemaBinding, SqlVersionFlags.TSql130AndAbove);
        }

        protected override TSqlParseErrorException GetMatchingException(antlr.IToken token)
        {
            return new TSqlParseErrorException(TSql80ParserBaseInternal.CreateParseError("SQL46007", 
                token, TSqlParserResource.SQL46007Message, token.getText()));
        }

        internal static readonly TriggerOptionHelper Instance = new TriggerOptionHelper();
    }
}
