//------------------------------------------------------------------------------
// <copyright file="DbccOptionsHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------



namespace Microsoft.SqlServer.TransactSql.ScriptDom
{

    internal class DbccOptionsHelper : OptionsHelper<DbccOptionKind>
    {
        private DbccOptionsHelper()
        {
            AddOptionMapping(DbccOptionKind.AllErrorMessages, CodeGenerationSupporter.AllErrorMessages);
            AddOptionMapping(DbccOptionKind.CountRows,CodeGenerationSupporter.CountRows);
            AddOptionMapping(DbccOptionKind.NoInfoMessages,CodeGenerationSupporter.NoInfoMessages);
            AddOptionMapping(DbccOptionKind.TableResults,CodeGenerationSupporter.TableResults);
            AddOptionMapping(DbccOptionKind.TabLock,CodeGenerationSupporter.TabLock);
            AddOptionMapping(DbccOptionKind.StatHeader,CodeGenerationSupporter.StatHeader);
            AddOptionMapping(DbccOptionKind.DensityVector,CodeGenerationSupporter.DensityVector);
            AddOptionMapping(DbccOptionKind.HistogramSteps,CodeGenerationSupporter.HistogramSteps);
            AddOptionMapping(DbccOptionKind.EstimateOnly,CodeGenerationSupporter.EstimateOnly);
            AddOptionMapping(DbccOptionKind.Fast,CodeGenerationSupporter.Fast);
            AddOptionMapping(DbccOptionKind.AllLevels,CodeGenerationSupporter.AllLevels);
            AddOptionMapping(DbccOptionKind.AllIndexes,CodeGenerationSupporter.AllIndexes);
            AddOptionMapping(DbccOptionKind.PhysicalOnly,CodeGenerationSupporter.PhysicalOnly);
            AddOptionMapping(DbccOptionKind.AllConstraints,CodeGenerationSupporter.AllConstraints);
            AddOptionMapping(DbccOptionKind.StatsStream,CodeGenerationSupporter.StatsStream);
            AddOptionMapping(DbccOptionKind.Histogram,CodeGenerationSupporter.Histogram);
            AddOptionMapping(DbccOptionKind.DataPurity,CodeGenerationSupporter.DataPurity);
            AddOptionMapping(DbccOptionKind.MarkInUseForRemoval,CodeGenerationSupporter.MarkInUseForRemoval);
            AddOptionMapping(DbccOptionKind.ExtendedLogicalChecks, CodeGenerationSupporter.ExtendedLogicalChecks, SqlVersionFlags.TSql100AndAbove);
        }

        internal static readonly DbccOptionsHelper Instance = new DbccOptionsHelper();
    }
}
