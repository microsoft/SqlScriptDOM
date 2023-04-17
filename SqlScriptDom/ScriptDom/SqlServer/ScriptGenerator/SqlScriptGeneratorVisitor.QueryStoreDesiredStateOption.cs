using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
	partial class SqlScriptGeneratorVisitor
	{
		public override void ExplicitVisit(QueryStoreDesiredStateOption node)
		{
			System.Diagnostics.Debug.Assert(node.OptionKind == QueryStoreOptionKind.Desired_State);

			if (node.OperationModeSpecified == true)
			{
				GenerateIdentifier(CodeGenerationSupporter.OperationMode);
			}
			else
			{
				GenerateIdentifier(CodeGenerationSupporter.DesiredState);
			}
			GenerateSpace();
			GenerateSymbolAndSpace(TSqlTokenType.EqualsSign);

			if (node.Value == QueryStoreDesiredStateOptionKind.ReadWrite)
			{
				GenerateIdentifier(CodeGenerationSupporter.ReadWrite);
			}
			else if (node.Value == QueryStoreDesiredStateOptionKind.ReadOnly)
			{
				GenerateIdentifier(CodeGenerationSupporter.ReadOnly);
			}
		}

	}
}