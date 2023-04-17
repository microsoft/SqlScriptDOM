//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.OpenJsonTableSource.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
	partial class SqlScriptGeneratorVisitor
	{
		public override void ExplicitVisit(OpenJsonTableReference node)
		{
			GenerateIdentifier(CodeGenerationSupporter.OpenJson);

			GenerateSpaceAndSymbol(TSqlTokenType.LeftParenthesis);

			GenerateFragmentIfNotNull(node.Variable);

			if (node.RowPattern != null)
			{
				GenerateSymbol(TSqlTokenType.Comma);
			}

			GenerateSpaceAndFragmentIfNotNull(node.RowPattern);

			GenerateSymbol(TSqlTokenType.RightParenthesis);

			if (node.SchemaDeclarationItems.Count > 0)
			{
				GenerateSpaceAndKeyword(TSqlTokenType.With);

				GenerateSpace();
				GenerateParenthesisedCommaSeparatedList(node.SchemaDeclarationItems);
			}

			GenerateSpaceAndAlias(node.Alias);
		}
	}
}
