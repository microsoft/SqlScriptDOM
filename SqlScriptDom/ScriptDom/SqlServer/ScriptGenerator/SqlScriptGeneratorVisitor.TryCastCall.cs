//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.TryCastCall.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
	partial class SqlScriptGeneratorVisitor
	{
		public override void ExplicitVisit(TryCastCall node)
		{
			GenerateIdentifier(CodeGenerationSupporter.TryCast);

			GenerateSpaceAndSymbol(TSqlTokenType.LeftParenthesis);
			GenerateFragmentIfNotNull(node.Parameter);
			GenerateSpaceAndKeyword(TSqlTokenType.As);
			GenerateSpaceAndFragmentIfNotNull(node.DataType);
			GenerateSymbol(TSqlTokenType.RightParenthesis);

			GenerateSpaceAndCollation(node.Collation);
		}
	}
}
