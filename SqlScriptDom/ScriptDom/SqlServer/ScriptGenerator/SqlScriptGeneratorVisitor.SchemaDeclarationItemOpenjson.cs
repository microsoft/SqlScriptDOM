//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.SchemaDeclarationItem.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
	partial class SqlScriptGeneratorVisitor
	{
		public override void ExplicitVisit(SchemaDeclarationItemOpenjson node)
		{
			GenerateFragmentIfNotNull(node.ColumnDefinition);

			GenerateSpaceAndFragmentIfNotNull(node.Mapping);

			if (node.AsJson)
			{
				GenerateSpaceAndKeyword(TSqlTokenType.As);
				GenerateSpaceAndIdentifier(CodeGenerationSupporter.Json);
			}
		}
	}
}
