//------------------------------------------------------------------------------
// <copyright file="AutomaticTuningForceLastGoodPlanOptionsHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
	/// <summary>
	/// Helper Class for Automatic Tuning Force Last Good Plan Option State
	/// </summary>

	[Serializable]
	internal class AutomaticTuningForceLastGoodPlanOptionHelper : OptionsHelper<AutomaticTuningOptionState>
	{
		private AutomaticTuningForceLastGoodPlanOptionHelper()
		{
			AddOptionMapping(AutomaticTuningOptionState.Off, CodeGenerationSupporter.Off);
			AddOptionMapping(AutomaticTuningOptionState.On, CodeGenerationSupporter.On);
			AddOptionMapping(AutomaticTuningOptionState.Default, CodeGenerationSupporter.Default);
		}

		internal static readonly AutomaticTuningForceLastGoodPlanOptionHelper Instance = new AutomaticTuningForceLastGoodPlanOptionHelper();
	}
}