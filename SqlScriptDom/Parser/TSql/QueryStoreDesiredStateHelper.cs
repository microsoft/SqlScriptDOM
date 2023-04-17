//------------------------------------------------------------------------------
// <copyright file="QueryStoreDesiredStateHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
	/// <summary>
	/// Helper Class for Query Store Desired State Option Kind
	/// </summary>

	[Serializable]
	internal class QueryStoreDesiredStateHelper : OptionsHelper<QueryStoreDesiredStateOptionKind>
	{
		private QueryStoreDesiredStateHelper()
		{
            AddOptionMapping(QueryStoreDesiredStateOptionKind.Off, CodeGenerationSupporter.Off);
			AddOptionMapping(QueryStoreDesiredStateOptionKind.ReadOnly, CodeGenerationSupporter.ReadOnly);
			AddOptionMapping(QueryStoreDesiredStateOptionKind.ReadWrite, CodeGenerationSupporter.ReadWrite);
		}

		internal static readonly QueryStoreDesiredStateHelper Instance = new QueryStoreDesiredStateHelper();
	}
}