//------------------------------------------------------------------------------
// <copyright file="DurabilityTableOptionHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{

	internal class DurabilityTableOptionHelper : OptionsHelper<DurabilityTableOptionKind>
	{
		private DurabilityTableOptionHelper()
		{
			AddOptionMapping(DurabilityTableOptionKind.SchemaOnly, CodeGenerationSupporter.SchemaOnly);
			AddOptionMapping(DurabilityTableOptionKind.SchemaAndData, CodeGenerationSupporter.SchemaAndData);
		}

		internal static readonly DurabilityTableOptionHelper Instance = new DurabilityTableOptionHelper();
	}
}
