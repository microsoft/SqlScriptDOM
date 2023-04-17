//------------------------------------------------------------------------------
// <copyright file="PlanOptimizerHintHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    internal class PlanOptimizerHintHelper : OptionsHelper<OptimizerHintKind>
    {
        private PlanOptimizerHintHelper()
        {
            AddOptionMapping(OptimizerHintKind.RobustPlan, CodeGenerationSupporter.Robust, SqlVersionFlags.TSqlAll);
            AddOptionMapping(OptimizerHintKind.ShrinkDBPlan, CodeGenerationSupporter.ShrinkDb, SqlVersionFlags.TSqlAll);
            AddOptionMapping(OptimizerHintKind.AlterColumnPlan, CodeGenerationSupporter.AlterColumn, SqlVersionFlags.TSqlAll);
            AddOptionMapping(OptimizerHintKind.KeepPlan, CodeGenerationSupporter.Keep, SqlVersionFlags.TSqlAll);
            AddOptionMapping(OptimizerHintKind.KeepFixedPlan, CodeGenerationSupporter.KeepFixed, SqlVersionFlags.TSqlAll);
            AddOptionMapping(OptimizerHintKind.CheckConstraintsPlan, CodeGenerationSupporter.CheckConstraintsHint, SqlVersionFlags.TSql90AndAbove);
        }

        internal static readonly PlanOptimizerHintHelper Instance = new PlanOptimizerHintHelper();
    }
}
