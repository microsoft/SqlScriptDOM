using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Collections.Generic;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
	partial class SqlScriptGeneratorVisitor
	{
		protected static Dictionary<QueryStoreSizeCleanupPolicyOptionKind, string> _sizeBasedCleanupOptionNames = new Dictionary<QueryStoreSizeCleanupPolicyOptionKind, string>()
		{
			{ QueryStoreSizeCleanupPolicyOptionKind.OFF, CodeGenerationSupporter.Off},
			{ QueryStoreSizeCleanupPolicyOptionKind.AUTO, CodeGenerationSupporter.Auto},
		};

		public override void ExplicitVisit(QueryStoreSizeCleanupPolicyOption node)
		{
			System.Diagnostics.Debug.Assert(node.OptionKind == QueryStoreOptionKind.Size_Based_Cleanup_Mode);

			string optionName = GetValueForEnumKey(_sizeBasedCleanupOptionNames, node.Value);
			GenerateNameEqualsValue(CodeGenerationSupporter.SizeBasedCleanupMode, optionName);
		}
	}
}