//------------------------------------------------------------------------------
// <copyright file="AutomaticTuningCreateIndexOptionHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
	/// <summary>
	/// Helper Class for Automatic Tuning Create Index Option State
	/// </summary>

	[Serializable]
	internal class AutomaticTuningCreateIndexOptionHelper : OptionsHelper<AutomaticTuningOptionState>
	{
		private AutomaticTuningCreateIndexOptionHelper()
		{
			AddOptionMapping(AutomaticTuningOptionState.Off, CodeGenerationSupporter.Off);
			AddOptionMapping(AutomaticTuningOptionState.On, CodeGenerationSupporter.On);
			AddOptionMapping(AutomaticTuningOptionState.Default, CodeGenerationSupporter.Default);
		}

		internal static readonly AutomaticTuningCreateIndexOptionHelper Instance = new AutomaticTuningCreateIndexOptionHelper();
	}
}