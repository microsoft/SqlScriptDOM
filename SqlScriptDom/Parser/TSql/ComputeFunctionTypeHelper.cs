//------------------------------------------------------------------------------
// <copyright file="ComputeFunctionTypeHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    internal class ComputeFunctionTypeHelper : OptionsHelper<ComputeFunctionType>
    {
        private ComputeFunctionTypeHelper()
        {
            AddOptionMapping(ComputeFunctionType.Count, CodeGenerationSupporter.Count);
            AddOptionMapping(ComputeFunctionType.CountBig, CodeGenerationSupporter.CountBig);
            AddOptionMapping(ComputeFunctionType.Max, CodeGenerationSupporter.Max);
            AddOptionMapping(ComputeFunctionType.Min, CodeGenerationSupporter.Min);
            AddOptionMapping(ComputeFunctionType.Sum, CodeGenerationSupporter.Sum);
            AddOptionMapping(ComputeFunctionType.Avg, CodeGenerationSupporter.Avg);
            AddOptionMapping(ComputeFunctionType.Var, CodeGenerationSupporter.Var);
            AddOptionMapping(ComputeFunctionType.Varp, CodeGenerationSupporter.Varp);
            AddOptionMapping(ComputeFunctionType.Stdev, CodeGenerationSupporter.Stdev);
            AddOptionMapping(ComputeFunctionType.Stdevp, CodeGenerationSupporter.Stdevp);
            AddOptionMapping(ComputeFunctionType.ChecksumAgg, CodeGenerationSupporter.ChecksumAgg);
            AddOptionMapping(ComputeFunctionType.ModularSum, CodeGenerationSupporter.ModularSum);
        }

        protected override TSqlParseErrorException GetMatchingException(antlr.IToken token)
        {
            return new TSqlParseErrorException(TSql80ParserBaseInternal.CreateParseError(
                "SQL46024", token, TSqlParserResource.SQL46024Message, token.getText()));
        }

        internal static readonly ComputeFunctionTypeHelper Instance = new ComputeFunctionTypeHelper();
    }
}
