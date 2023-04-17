//------------------------------------------------------------------------------
// <copyright file="DurabilityTableOptionKind.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
	/// <summary>
	/// The table durability kinds
	/// </summary>
	public enum DurabilityTableOptionKind
	{
		/// <summary>
		/// Only schema is persisted
		/// </summary>
		SchemaOnly = 0,
		/// <summary>
		/// Both schema and data is persisted.
		/// </summary>
		SchemaAndData = 1,
	}
}