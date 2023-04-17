//------------------------------------------------------------------------------
// <copyright file="AutomaticTuningDropIndexOptionHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
	/// <summary>
	/// Helper Class for Automatic Tuning Drop Index Option State
	/// </summary>

	[Serializable]
	internal class AutomaticTuningDropIndexOptionHelper : OptionsHelper<AutomaticTuningOptionState>
	{
		private AutomaticTuningDropIndexOptionHelper()
		{
			AddOptionMapping(AutomaticTuningOptionState.Off, CodeGenerationSupporter.Off);
			AddOptionMapping(AutomaticTuningOptionState.On, CodeGenerationSupporter.On);
			AddOptionMapping(AutomaticTuningOptionState.Default, CodeGenerationSupporter.Default);
		}

		internal static readonly AutomaticTuningDropIndexOptionHelper Instance = new AutomaticTuningDropIndexOptionHelper();
	}
}