using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Collections.Generic;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
	partial class SqlScriptGeneratorVisitor
	{
		protected static Dictionary<QueryStoreCapturePolicyOptionKind, string> _captureModeOptionNames = new Dictionary<QueryStoreCapturePolicyOptionKind, string>()
		{
			{ QueryStoreCapturePolicyOptionKind.NONE, CodeGenerationSupporter.None},
			{ QueryStoreCapturePolicyOptionKind.AUTO, CodeGenerationSupporter.Auto},
			{ QueryStoreCapturePolicyOptionKind.ALL, CodeGenerationSupporter.All},            
		};

		public override void ExplicitVisit(QueryStoreCapturePolicyOption node)
		{
			System.Diagnostics.Debug.Assert(node.OptionKind == QueryStoreOptionKind.Query_Capture_Mode);

			string optionName = GetValueForEnumKey(_captureModeOptionNames, node.Value);
			GenerateNameEqualsValue(CodeGenerationSupporter.QueryCaptureMode, optionName);
		}
	}
}