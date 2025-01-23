//------------------------------------------------------------------------------
// <copyright file="QueryStoreOptionsHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
	/// <summary>
	/// Helper Class for Query Store Options
	/// </summary>

	[Serializable]
	internal class QueryStoreOptionsHelper : OptionsHelper<QueryStoreOptionKind>
	{
		private QueryStoreOptionsHelper()
		{
			AddOptionMapping(QueryStoreOptionKind.Desired_State, CodeGenerationSupporter.DesiredState);
			AddOptionMapping(QueryStoreOptionKind.Query_Capture_Mode, CodeGenerationSupporter.QueryCaptureMode);
			AddOptionMapping(QueryStoreOptionKind.Size_Based_Cleanup_Mode, CodeGenerationSupporter.SizeBasedCleanupMode);
			AddOptionMapping(QueryStoreOptionKind.Flush_Interval_Seconds, CodeGenerationSupporter.FlushIntervalSecondsAlt);
			AddOptionMapping(QueryStoreOptionKind.Interval_Length_Minutes, CodeGenerationSupporter.IntervalLengthMinutes);
			AddOptionMapping(QueryStoreOptionKind.Current_Storage_Size_MB, CodeGenerationSupporter.MaxQdsSize);
			AddOptionMapping(QueryStoreOptionKind.Max_Plans_Per_Query, CodeGenerationSupporter.MaxPlansPerQuery);
			AddOptionMapping(QueryStoreOptionKind.Stale_Query_Threshold_Days, CodeGenerationSupporter.CleanupPolicy);
			AddOptionMapping(QueryStoreOptionKind.Wait_Stats_Capture_Mode, CodeGenerationSupporter.WaitStatsCaptureMode, SqlVersionFlags.TSql140AndAbove);
		}

		internal static readonly QueryStoreOptionsHelper Instance = new QueryStoreOptionsHelper();
	}
}