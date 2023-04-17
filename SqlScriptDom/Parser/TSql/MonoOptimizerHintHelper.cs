//------------------------------------------------------------------------------
// <copyright file="MonoOptimizerHintHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    internal class MonoOptimizerHintHelper : OptionsHelper<OptimizerHintKind>
    {
        private MonoOptimizerHintHelper()
        {
            AddOptionMapping(OptimizerHintKind.Recompile, CodeGenerationSupporter.Recompile, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(OptimizerHintKind.IgnoreNonClusteredColumnStoreIndex, CodeGenerationSupporter.IgnoreNonClusteredColumnStoreIndex, SqlVersionFlags.TSql110AndAbove);
            AddOptionMapping(OptimizerHintKind.NoPerformanceSpool, CodeGenerationSupporter.NoPerformanceSpool, SqlVersionFlags.TSql130AndAbove);
        }

        internal static readonly MonoOptimizerHintHelper Instance = new MonoOptimizerHintHelper();
    }
}
