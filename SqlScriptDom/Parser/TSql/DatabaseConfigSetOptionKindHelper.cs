//------------------------------------------------------------------------------
// <copyright file="DatabaseConfigSetOptionKindHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------



namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    internal class DatabaseConfigSetOptionKindHelper : OptionsHelper<DatabaseConfigSetOptionKind>
    {
        private DatabaseConfigSetOptionKindHelper()
        {
            // 130 Options
            AddOptionMapping(DatabaseConfigSetOptionKind.MaxDop, CodeGenerationSupporter.MaxDop, SqlVersionFlags.TSql130AndAbove);
            AddOptionMapping(DatabaseConfigSetOptionKind.LegacyCardinalityEstimate, CodeGenerationSupporter.LegacyCardinalityEstimation, SqlVersionFlags.TSql130AndAbove);
            AddOptionMapping(DatabaseConfigSetOptionKind.ParameterSniffing, CodeGenerationSupporter.ParameterSniffing, SqlVersionFlags.TSql130AndAbove);
            AddOptionMapping(DatabaseConfigSetOptionKind.QueryOptimizerHotFixes, CodeGenerationSupporter.QueryOptimizerHotFixes, SqlVersionFlags.TSql130AndAbove);
            AddOptionMapping(DatabaseConfigSetOptionKind.DWCompatibilityLevel, CodeGenerationSupporter.DWCompatibilityLevel, SqlVersionFlags.TSql130AndAbove);
        }

        internal static readonly DatabaseConfigSetOptionKindHelper Instance = new DatabaseConfigSetOptionKindHelper();
    }
}
