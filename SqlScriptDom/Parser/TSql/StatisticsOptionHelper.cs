//------------------------------------------------------------------------------
// <copyright file="StatisticsOptionHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    
    internal class StatisticsOptionHelper : OptionsHelper<StatisticsOptionKind>
    {
        private StatisticsOptionHelper()
        {
            AddOptionMapping(StatisticsOptionKind.FullScan, CodeGenerationSupporter.FullScan);
            AddOptionMapping(StatisticsOptionKind.NoRecompute, CodeGenerationSupporter.NoRecompute);
            AddOptionMapping(StatisticsOptionKind.Resample, CodeGenerationSupporter.Resample);
            AddOptionMapping(StatisticsOptionKind.Columns, CodeGenerationSupporter.Columns);
        }

        protected override TSqlParseErrorException GetMatchingException(antlr.IToken token)
        {
            return new TSqlParseErrorException(TSql80ParserBaseInternal.CreateParseError(
                "SQL46020", token, TSqlParserResource.SQL46020Message, token.getText()));
        }

        internal static readonly StatisticsOptionHelper Instance = new StatisticsOptionHelper();
    }
}
