//------------------------------------------------------------------------------
// <copyright file="AtomicBlockOptionKind.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
	/// <summary>
	/// Options specified in the ATOMIC block statement
	/// </summary>
	public enum AtomicBlockOptionKind
	{
		/// <summary>
		/// Isolation Level
		/// </summary>
		IsolationLevel = 0,

		/// <summary>
		/// Language
		/// </summary>
		Language = 1,
		
		/// <summary>
		/// Date First
		/// </summary>
		DateFirst = 2,

		/// <summary>
		/// Date Format
		/// </summary>
		DateFormat = 3,
		
		/// <summary>
		/// Date Format
		/// </summary>
		DelayedDurability = 4,
	}
}
