//------------------------------------------------------------------------------
// <copyright file="AutomaticTuningOptionsHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
	/// <summary>
	/// Helper Class for Automatic Tuning Options
	/// </summary>

	[Serializable]
	internal class AutomaticTuningOptionsHelper : OptionsHelper<AutomaticTuningOptionKind>
	{
		private AutomaticTuningOptionsHelper()
		{
			AddOptionMapping(AutomaticTuningOptionKind.Force_Last_Good_Plan, CodeGenerationSupporter.ForceLastGoodPlan);
			AddOptionMapping(AutomaticTuningOptionKind.Create_Index, CodeGenerationSupporter.CreateIndex);
			AddOptionMapping(AutomaticTuningOptionKind.Drop_Index, CodeGenerationSupporter.DropIndex);
			AddOptionMapping(AutomaticTuningOptionKind.Maintain_Index, CodeGenerationSupporter.MaintainIndex);
		}

		internal static readonly AutomaticTuningOptionsHelper Instance = new AutomaticTuningOptionsHelper();
	}
}