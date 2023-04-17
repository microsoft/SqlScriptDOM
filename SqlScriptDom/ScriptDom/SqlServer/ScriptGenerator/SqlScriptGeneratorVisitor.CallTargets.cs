//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CallTargets.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
	partial class SqlScriptGeneratorVisitor
	{
		public override void ExplicitVisit(ExpressionCallTarget node)
		{
			GenerateFragmentIfNotNull(node.Expression);
			GenerateSymbol(TSqlTokenType.Dot);
		}

		public override void ExplicitVisit(MultiPartIdentifierCallTarget node)
		{
			GenerateFragmentIfNotNull(node.MultiPartIdentifier);
			GenerateSymbol(TSqlTokenType.Dot);
		}

		public override void ExplicitVisit(UserDefinedTypeCallTarget node)
		{
			GenerateFragmentIfNotNull(node.SchemaObjectName);
			GenerateSymbol(TSqlTokenType.DoubleColon);
		}
	}
}
