//------------------------------------------------------------------------------
// <copyright file="DatabaseOptionKindHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------



namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    internal class DatabaseOptionKindHelper : OptionsHelper<DatabaseOptionKind>
    {
        private DatabaseOptionKindHelper()
        {
            // 90 options
            AddOptionMapping(DatabaseOptionKind.CompatibilityLevel, CodeGenerationSupporter.CompatibilityLevel, SqlVersionFlags.TSql90AndAbove);
            
            //110 options
            AddOptionMapping(DatabaseOptionKind.DefaultFullTextLanguage, CodeGenerationSupporter.DefaultFullTextLanguage, SqlVersionFlags.TSql110AndAbove);
            AddOptionMapping(DatabaseOptionKind.DefaultLanguage, CodeGenerationSupporter.DefaultLanguage, SqlVersionFlags.TSql110AndAbove);
            AddOptionMapping(DatabaseOptionKind.TwoDigitYearCutoff, CodeGenerationSupporter.TwoDigitYearCutoff, SqlVersionFlags.TSql110AndAbove);
            AddOptionMapping(DatabaseOptionKind.Edition, CodeGenerationSupporter.Edition, SqlVersionFlags.TSql110AndAbove);
            
            // 120 options
            AddOptionMapping(DatabaseOptionKind.ServiceObjective, CodeGenerationSupporter.ServiceObjective, SqlVersionFlags.TSql120AndAbove);

            // 130 Options
            AddOptionMapping(DatabaseOptionKind.QueryStore, CodeGenerationSupporter.QueryStore, SqlVersionFlags.TSql130AndAbove);

            // 140 Options
            AddOptionMapping(DatabaseOptionKind.CatalogCollation, CodeGenerationSupporter.CatalogCollation, SqlVersionFlags.TSql140AndAbove);

            // 140 Options
            AddOptionMapping(DatabaseOptionKind.AutomaticTuning, CodeGenerationSupporter.AutomaticTuning, SqlVersionFlags.TSql140AndAbove);

            // 170 Options
            AddOptionMapping(DatabaseOptionKind.ManualCutover, CodeGenerationSupporter.ManualCutover, SqlVersionFlags.TSql170AndAbove);
            AddOptionMapping(DatabaseOptionKind.PerformCutover, CodeGenerationSupporter.PerformCutover, SqlVersionFlags.TSql170AndAbove);
        }

        internal static readonly DatabaseOptionKindHelper Instance = new DatabaseOptionKindHelper();

       

    }

}
