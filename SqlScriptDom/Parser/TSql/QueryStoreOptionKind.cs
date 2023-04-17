//------------------------------------------------------------------------------
// <copyright file="QueryStoreOptionDetail.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

	/// <summary>
	/// The possible Query Store Options under 'ALTER DATABASE d1 SET QUERY_STORE = (...)'
	/// </summary>
	public enum QueryStoreOptionKind
	{
		Desired_State,
		Query_Capture_Mode,
		Size_Based_Cleanup_Mode,
		Flush_Interval_Seconds,
		Interval_Length_Minutes,
		Current_Storage_Size_MB,
		Max_Plans_Per_Query,
		Stale_Query_Threshold_Days
	}


#pragma warning restore 1591
}
