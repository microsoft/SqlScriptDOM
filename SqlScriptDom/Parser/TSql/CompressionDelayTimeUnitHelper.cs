//------------------------------------------------------------------------------
// <copyright file="CompressionDelayTimeUnitHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
	internal class CompressionDelayTimeUnitHelper : OptionsHelper<CompressionDelayTimeUnit>
	{
		private CompressionDelayTimeUnitHelper()
		{
			AddOptionMapping(CompressionDelayTimeUnit.Unitless, string.Empty);
			AddOptionMapping(CompressionDelayTimeUnit.Minute, CodeGenerationSupporter.Minute);
			AddOptionMapping(CompressionDelayTimeUnit.Minutes, CodeGenerationSupporter.Minutes);
		}

		internal static readonly CompressionDelayTimeUnitHelper Instance = new CompressionDelayTimeUnitHelper();
	}
}