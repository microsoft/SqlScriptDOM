//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AutomaticTuningOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Collections.Generic;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
	partial class SqlScriptGeneratorVisitor
	{
		protected static Dictionary<AutomaticTuningOptionState, string> _optionNames = new Dictionary<AutomaticTuningOptionState, string>()
		{
			{ AutomaticTuningOptionState.Off, CodeGenerationSupporter.Off},
			{ AutomaticTuningOptionState.On, CodeGenerationSupporter.On},
			{ AutomaticTuningOptionState.Default, CodeGenerationSupporter.Default},
		};

		public override void ExplicitVisit(AutomaticTuningCreateIndexOption node)
		{
			System.Diagnostics.Debug.Assert(node.OptionKind == AutomaticTuningOptionKind.Create_Index);
			string optionName = GetValueForEnumKey(_optionNames, node.Value);
			GenerateNameEqualsValue(CodeGenerationSupporter.CreateIndex, optionName);
		}

		public override void ExplicitVisit(AutomaticTuningDropIndexOption node)
		{
			System.Diagnostics.Debug.Assert(node.OptionKind == AutomaticTuningOptionKind.Drop_Index);
			string optionName = GetValueForEnumKey(_optionNames, node.Value);
			GenerateNameEqualsValue(CodeGenerationSupporter.DropIndex, optionName);
		}

		public override void ExplicitVisit(AutomaticTuningForceLastGoodPlanOption node)
		{
			System.Diagnostics.Debug.Assert(node.OptionKind == AutomaticTuningOptionKind.Force_Last_Good_Plan);
			string optionName = GetValueForEnumKey(_optionNames, node.Value);
			GenerateNameEqualsValue(CodeGenerationSupporter.ForceLastGoodPlan, optionName);
		}

		public override void ExplicitVisit(AutomaticTuningMaintainIndexOption node)
		{
			System.Diagnostics.Debug.Assert(node.OptionKind == AutomaticTuningOptionKind.Maintain_Index);
			string optionName = GetValueForEnumKey(_optionNames, node.Value);
			GenerateNameEqualsValue(CodeGenerationSupporter.MaintainIndex, optionName);
		}
	}
}