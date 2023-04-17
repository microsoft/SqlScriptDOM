//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.FileTableDirectoryTableOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
	partial class SqlScriptGeneratorVisitor
	{
		public override void ExplicitVisit(MemoryOptimizedTableOption node)
		{
			System.Diagnostics.Debug.Assert(node.OptionKind == TableOptionKind.MemoryOptimized, "TableOption does not match");
			GenerateOptionStateWithEqualSign(CodeGenerationSupporter.MemoryOptimized, node.OptionState);
		}
	}
}