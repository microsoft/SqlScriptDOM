//------------------------------------------------------------------------------
// <copyright file="QueryStoreCapturePolicyHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
	/// <summary>
	/// Helper Class for Query Store Capture Policy Option Kind
	/// </summary>

	[Serializable]
	internal class QueryStoreCapturePolicyHelper : OptionsHelper<QueryStoreCapturePolicyOptionKind>
	{
		private QueryStoreCapturePolicyHelper()
		{
			AddOptionMapping(QueryStoreCapturePolicyOptionKind.NONE, CodeGenerationSupporter.None);
			AddOptionMapping(QueryStoreCapturePolicyOptionKind.AUTO, CodeGenerationSupporter.Auto);
			AddOptionMapping(QueryStoreCapturePolicyOptionKind.ALL, CodeGenerationSupporter.All);
		}

		internal static readonly QueryStoreCapturePolicyHelper Instance = new QueryStoreCapturePolicyHelper();
	}
}