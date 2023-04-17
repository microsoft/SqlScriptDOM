//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.ParseCall.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
	partial class SqlScriptGeneratorVisitor
	{
		public override void ExplicitVisit(ParseCall node)
		{
			GenerateIdentifier(CodeGenerationSupporter.Parse);

			GenerateSpaceAndSymbol(TSqlTokenType.LeftParenthesis);
			GenerateFragmentIfNotNull(node.StringValue);
			GenerateSpaceAndKeyword(TSqlTokenType.As);
			GenerateSpaceAndFragmentIfNotNull(node.DataType);

			if (node.Culture != null)
			{
				GenerateSpace();
				GenerateIdentifier(CodeGenerationSupporter.Using);
				GenerateSpaceAndFragmentIfNotNull(node.Culture);
			}

			GenerateSymbol(TSqlTokenType.RightParenthesis);

			GenerateSpaceAndCollation(node.Collation);
		}
	}
}
