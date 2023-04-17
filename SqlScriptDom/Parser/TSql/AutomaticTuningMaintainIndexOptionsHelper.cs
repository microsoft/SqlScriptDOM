//------------------------------------------------------------------------------
// <copyright file="AutomaticTuningMaintainIndexOptionHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
	/// <summary>
	/// Helper Class for Automatic Tuning Maintain Index Option State
	/// </summary>

	[Serializable]
	internal class AutomaticTuningMaintainIndexOptionHelper : OptionsHelper<AutomaticTuningOptionState>
	{
		private AutomaticTuningMaintainIndexOptionHelper()
		{
			AddOptionMapping(AutomaticTuningOptionState.Off, CodeGenerationSupporter.Off);
			AddOptionMapping(AutomaticTuningOptionState.On, CodeGenerationSupporter.On);
			AddOptionMapping(AutomaticTuningOptionState.Default, CodeGenerationSupporter.Default);
		}

		internal static readonly AutomaticTuningMaintainIndexOptionHelper Instance = new AutomaticTuningMaintainIndexOptionHelper();
	}
}