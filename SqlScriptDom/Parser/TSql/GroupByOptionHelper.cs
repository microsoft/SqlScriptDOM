//------------------------------------------------------------------------------
// <copyright file="GroupByOptionHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    internal class GroupByOptionHelper : OptionsHelper<GroupByOption>
    {
        private GroupByOptionHelper()
        {
            AddOptionMapping(GroupByOption.Cube, CodeGenerationSupporter.Cube);
            AddOptionMapping(GroupByOption.Rollup, CodeGenerationSupporter.Rollup);
        }

        protected override TSqlParseErrorException GetMatchingException(antlr.IToken token)
        {
            return new TSqlParseErrorException(TSql80ParserBaseInternal.CreateParseError(
                "SQL46023", token, TSqlParserResource.SQL46023Message, token.getText()));
        }

        internal static readonly GroupByOptionHelper Instance = new GroupByOptionHelper();
    }
}
