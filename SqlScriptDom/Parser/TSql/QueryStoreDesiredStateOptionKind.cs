//------------------------------------------------------------------------------
// <copyright file="QueryStoreDesiredStateOptionKind.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

	/// <summary>
	/// The possible Query Store Desired State Options
	/// </summary>
	public enum QueryStoreDesiredStateOptionKind
	{
        Off = 0,
		ReadOnly = 1,
		ReadWrite = 2
	}


#pragma warning restore 1591
}