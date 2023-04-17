//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AutomaticTuningDatabaseOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
	partial class SqlScriptGeneratorVisitor
	{
		public override void ExplicitVisit(AutomaticTuningDatabaseOption node)
		{
			System.Diagnostics.Debug.Assert(node.OptionKind == DatabaseOptionKind.AutomaticTuning);
			GenerateIdentifier(CodeGenerationSupporter.AutomaticTuning);
			GenerateSpace();

			switch (node.AutomaticTuningState)
			{
				case AutomaticTuningState.Inherit:
					GenerateSymbolAndSpace(TSqlTokenType.EqualsSign);
					GenerateIdentifier(CodeGenerationSupporter.Inherit);
					break;
				case AutomaticTuningState.Auto:
					GenerateSymbolAndSpace(TSqlTokenType.EqualsSign);
					GenerateIdentifier(CodeGenerationSupporter.Auto);
					break;
				case AutomaticTuningState.Custom:
					GenerateSymbolAndSpace(TSqlTokenType.EqualsSign);
					GenerateIdentifier(CodeGenerationSupporter.Custom);
					break;
				default:
					GenerateParenthesisedCommaSeparatedList(node.Options);
					break;
			}
		}
	}
}