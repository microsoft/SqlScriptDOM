//------------------------------------------------------------------------------
// <copyright file="XmlCompressionOptionState.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{

	/// <summary>
	/// The possible Xml Compression Options under 'CREATE TABLE t1 XML_COMPRESSION = { ON | OFF }'
	/// </summary>
	public enum XmlCompressionOptionState
	{
		/// <summary>
		/// XML Compression is off
		/// </summary>
		Off,
		/// <summary>
		/// XML Compression is on
		/// </summary>
		On
	}
}
