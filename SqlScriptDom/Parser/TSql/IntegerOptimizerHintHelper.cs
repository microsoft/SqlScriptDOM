//------------------------------------------------------------------------------
// <copyright file="IntegerOptimizerHintHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    internal class IntegerOptimizerHintHelper : OptionsHelper<OptimizerHintKind>
    {
        private IntegerOptimizerHintHelper()
        {
            AddOptionMapping(OptimizerHintKind.Fast, CodeGenerationSupporter.Fast, SqlVersionFlags.TSqlAll);
            AddOptionMapping(OptimizerHintKind.MaxDop, CodeGenerationSupporter.MaxDop, SqlVersionFlags.TSqlAll);
            AddOptionMapping(OptimizerHintKind.UsePlan, CodeGenerationSupporter.UsePlan, SqlVersionFlags.TSqlAll);

            AddOptionMapping(OptimizerHintKind.MaxRecursion, CodeGenerationSupporter.MaxRecursion, SqlVersionFlags.TSql90AndAbove);

            AddOptionMapping(OptimizerHintKind.QueryTraceOn, CodeGenerationSupporter.QueryTraceOn, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(OptimizerHintKind.CardinalityTunerLimit, CodeGenerationSupporter.CardinalityTunerLimit, SqlVersionFlags.TSql100AndAbove);
        }

        internal static readonly IntegerOptimizerHintHelper Instance = new IntegerOptimizerHintHelper();
    }
}
