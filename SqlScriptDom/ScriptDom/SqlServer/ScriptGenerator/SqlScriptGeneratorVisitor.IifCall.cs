//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.IIfCall.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
	partial class SqlScriptGeneratorVisitor
	{
		public override void ExplicitVisit(IIfCall node)
		{
			GenerateIdentifier(CodeGenerationSupporter.IIf);

			GenerateSpaceAndSymbol(TSqlTokenType.LeftParenthesis);
			GenerateFragmentIfNotNull(node.Predicate);
			GenerateSymbolAndSpace(TSqlTokenType.Comma);
			GenerateFragmentIfNotNull(node.ThenExpression);
			GenerateSymbolAndSpace(TSqlTokenType.Comma);
			GenerateFragmentIfNotNull(node.ElseExpression);
			GenerateSymbol(TSqlTokenType.RightParenthesis);

			GenerateSpaceAndCollation(node.Collation);
		}
	}
}
