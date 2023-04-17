using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
	partial class SqlScriptGeneratorVisitor
	{
		public override void ExplicitVisit(QueryStoreDatabaseOption node)
		{
			System.Diagnostics.Debug.Assert(node.OptionKind == DatabaseOptionKind.QueryStore);
			GenerateIdentifier(CodeGenerationSupporter.QueryStore);
			GenerateSpace();

			if (node.Clear == true)
			{
				GenerateIdentifier(CodeGenerationSupporter.Clear);
			}
			else if (node.ClearAll == true)
			{
				GenerateIdentifier(CodeGenerationSupporter.Clear);
				GenerateSpace();
				GenerateKeyword(TSqlTokenType.All);
			}
			else
			{
				switch (node.OptionState)
				{
					case OptionState.Off:
						GenerateSymbolAndSpace(TSqlTokenType.EqualsSign);
						GenerateKeyword(TSqlTokenType.Off);
						break;
					case OptionState.On:
						GenerateSymbolAndSpace(TSqlTokenType.EqualsSign);
						GenerateKeyword(TSqlTokenType.On);
						GenerateParenthesisedCommaSeparatedList(node.Options);
						break;
					default:
						GenerateParenthesisedCommaSeparatedList(node.Options);
						break;
				}
			}
		}

		public override void ExplicitVisit(QueryStoreDataFlushIntervalOption node)
		{
			System.Diagnostics.Debug.Assert(node.OptionKind == QueryStoreOptionKind.Flush_Interval_Seconds);
			GenerateNameEqualsValue(CodeGenerationSupporter.FlushIntervalSecondsAlt, node.FlushInterval);
		}

		public override void ExplicitVisit(QueryStoreIntervalLengthOption node)
		{
			System.Diagnostics.Debug.Assert(node.OptionKind == QueryStoreOptionKind.Interval_Length_Minutes);
			GenerateNameEqualsValue(CodeGenerationSupporter.IntervalLengthMinutes, node.StatsIntervalLength);
		}

		public override void ExplicitVisit(QueryStoreMaxStorageSizeOption node)
		{
			System.Diagnostics.Debug.Assert(node.OptionKind == QueryStoreOptionKind.Current_Storage_Size_MB);
			GenerateNameEqualsValue(CodeGenerationSupporter.MaxQdsSize, node.MaxQdsSize);
		}

		public override void ExplicitVisit(QueryStoreMaxPlansPerQueryOption node)
		{
			System.Diagnostics.Debug.Assert(node.OptionKind == QueryStoreOptionKind.Max_Plans_Per_Query);
			GenerateNameEqualsValue(CodeGenerationSupporter.MaxPlansPerQuery, node.MaxPlansPerQuery);
		}

		public override void ExplicitVisit(QueryStoreTimeCleanupPolicyOption node)
		{
			System.Diagnostics.Debug.Assert(node.OptionKind == QueryStoreOptionKind.Stale_Query_Threshold_Days);
			GenerateIdentifier(CodeGenerationSupporter.CleanupPolicy);
			GenerateSpace();
			GenerateSymbolAndSpace(TSqlTokenType.EqualsSign);
			GenerateSymbol(TSqlTokenType.LeftParenthesis);
			GenerateNameEqualsValue(CodeGenerationSupporter.StaleQueryThresholdDays, node.StaleQueryThreshold);
			GenerateSymbol(TSqlTokenType.RightParenthesis);
		}
	}
}