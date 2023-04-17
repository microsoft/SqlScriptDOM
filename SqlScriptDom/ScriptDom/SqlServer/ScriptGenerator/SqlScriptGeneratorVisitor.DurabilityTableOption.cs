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
		public override void ExplicitVisit(DurabilityTableOption node)
		{
			System.Diagnostics.Debug.Assert(node.OptionKind == TableOptionKind.Durability, "TableOption does not match");
			GenerateTokenAndEqualSign(CodeGenerationSupporter.Durability);
			switch (node.DurabilityTableOptionKind)
			{
				case DurabilityTableOptionKind.SchemaAndData:
					GenerateSpaceAndIdentifier(CodeGenerationSupporter.SchemaAndData);
					break;
				case DurabilityTableOptionKind.SchemaOnly:
					GenerateSpaceAndIdentifier(CodeGenerationSupporter.SchemaOnly);
					break;
			}
		}
	}
}