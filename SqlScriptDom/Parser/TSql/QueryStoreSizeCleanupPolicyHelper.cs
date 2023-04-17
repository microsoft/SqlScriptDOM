//------------------------------------------------------------------------------
// <copyright file="QueryStoreSizeCleanupPolicyHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
	/// <summary>
	/// Helper Class for Query Store Size Based Cleanup Policy Option Kind
	/// </summary>

	[Serializable]
	internal class QueryStoreSizeCleanupPolicyHelper : OptionsHelper<QueryStoreSizeCleanupPolicyOptionKind>
	{
		private QueryStoreSizeCleanupPolicyHelper()
		{
			AddOptionMapping(QueryStoreSizeCleanupPolicyOptionKind.AUTO, CodeGenerationSupporter.Auto);
			AddOptionMapping(QueryStoreSizeCleanupPolicyOptionKind.OFF, CodeGenerationSupporter.Off);
		}

		internal static readonly QueryStoreSizeCleanupPolicyHelper Instance = new QueryStoreSizeCleanupPolicyHelper();
	}
}