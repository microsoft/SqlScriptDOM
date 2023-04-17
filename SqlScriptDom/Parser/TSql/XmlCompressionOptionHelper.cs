//------------------------------------------------------------------------------
// <copyright file="XmlCompressionOptionHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
	/// <summary>
	/// Helper Class for XmlCompression Index Option State
	/// </summary>

	[Serializable]
	internal class XmlCompressionOptionHelper : OptionsHelper<XmlCompressionOptionState>
	{
		private XmlCompressionOptionHelper()
		{
			AddOptionMapping(XmlCompressionOptionState.Off, CodeGenerationSupporter.Off);
			AddOptionMapping(XmlCompressionOptionState.On, CodeGenerationSupporter.On);
		}

		internal static readonly XmlCompressionOptionHelper Instance = new XmlCompressionOptionHelper();
	}
}