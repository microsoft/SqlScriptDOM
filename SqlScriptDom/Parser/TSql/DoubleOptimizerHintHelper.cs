//------------------------------------------------------------------------------
// <copyright file="DoubleOptimizerHintHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    internal class DoubleOptimizerHintHelper : OptionsHelper<OptimizerHintKind>
    {
        private DoubleOptimizerHintHelper()
        {
            AddOptionMapping(OptimizerHintKind.MaxGrantPercent, CodeGenerationSupporter.MaxGrantPercent, SqlVersionFlags.TSql130AndAbove);
            AddOptionMapping(OptimizerHintKind.MinGrantPercent, CodeGenerationSupporter.MinGrantPercent, SqlVersionFlags.TSql130AndAbove);
        }

        internal static readonly DoubleOptimizerHintHelper Instance = new DoubleOptimizerHintHelper();
    }
}

