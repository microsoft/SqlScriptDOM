//------------------------------------------------------------------------------
// <copyright file="TemporalRetentionPeriodUnitHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    internal class TemporalRetentionPeriodUnitHelper : OptionsHelper<TemporalRetentionPeriodUnit>
    {
        private TemporalRetentionPeriodUnitHelper()
        {
            AddOptionMapping(TemporalRetentionPeriodUnit.Day, CodeGenerationSupporter.Day);
            AddOptionMapping(TemporalRetentionPeriodUnit.Days, CodeGenerationSupporter.Days);

            AddOptionMapping(TemporalRetentionPeriodUnit.Week, CodeGenerationSupporter.Week);
            AddOptionMapping(TemporalRetentionPeriodUnit.Weeks, CodeGenerationSupporter.Weeks);

            AddOptionMapping(TemporalRetentionPeriodUnit.Month, CodeGenerationSupporter.Month);
            AddOptionMapping(TemporalRetentionPeriodUnit.Months, CodeGenerationSupporter.Months);

            AddOptionMapping(TemporalRetentionPeriodUnit.Year, CodeGenerationSupporter.Year);
            AddOptionMapping(TemporalRetentionPeriodUnit.Years, CodeGenerationSupporter.Years);
        }

        internal static readonly TemporalRetentionPeriodUnitHelper Instance = new TemporalRetentionPeriodUnitHelper();
    }
}