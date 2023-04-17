//------------------------------------------------------------------------------
// <copyright file="IndexTypeKind.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
	/// <summary>
	/// The possible index types.
	/// </summary>           
	public enum IndexTypeKind
	{
		/// <summary>
		/// Clustered index
		/// </summary>
		Clustered = 0,
		/// <summary>
		/// Non clustered index
		/// </summary>
		NonClustered = 1,
		/// <summary>
		/// NonClusteredHash index
		/// </summary>
		NonClusteredHash = 2,
		/// <summary>
		/// Clustered Column Store index
		/// </summary>
		ClusteredColumnStore = 3,
		/// <summary>
		/// Clustered Column Store index
		/// </summary>
		NonClusteredColumnStore = 4,
	}
}
