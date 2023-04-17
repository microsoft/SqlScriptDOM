//------------------------------------------------------------------------------
// <copyright file="CompressionDelayTimeUnit.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
	/// <summary>
	/// Possible values for units in COMPRESSION_DELAY clause.
	/// </summary>
	public enum CompressionDelayTimeUnit
	{
		/// <summary>
		/// Compression delay without time unit, since time unit is optional.
		/// </summary>
		Unitless,

		/// <summary>
		/// Compression delay expressed as minute.
		/// </summary>
		Minute,

		/// <summary>
		/// Compression delay expressed in minutes.
		/// </summary>
		Minutes,
	}
}