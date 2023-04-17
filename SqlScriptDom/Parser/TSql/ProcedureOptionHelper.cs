//------------------------------------------------------------------------------
// <copyright file="ProcedureOptionHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    
    [Serializable]
    internal class ProcedureOptionHelper : OptionsHelper<ProcedureOptionKind>
    {
        private ProcedureOptionHelper()
        {
            AddOptionMapping(ProcedureOptionKind.Encryption, CodeGenerationSupporter.Encryption);
            AddOptionMapping(ProcedureOptionKind.Recompile, CodeGenerationSupporter.Recompile);
            AddOptionMapping(ProcedureOptionKind.NativeCompilation, CodeGenerationSupporter.NativeCompilation, SqlVersionFlags.TSql120AndAbove);
            AddOptionMapping(ProcedureOptionKind.SchemaBinding, CodeGenerationSupporter.SchemaBinding, SqlVersionFlags.TSql120AndAbove);
        }

        protected override TSqlParseErrorException GetMatchingException(antlr.IToken token)
        {
            return new TSqlParseErrorException(TSql80ParserBaseInternal.CreateParseError("SQL46006", 
                token, TSqlParserResource.SQL46006Message, token.getText()));
        }

        internal static readonly ProcedureOptionHelper Instance = new ProcedureOptionHelper();
    }
}
