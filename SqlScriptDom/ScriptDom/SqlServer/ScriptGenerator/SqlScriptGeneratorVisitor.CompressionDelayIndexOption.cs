//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CompressionDelayIndexOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
	partial class SqlScriptGeneratorVisitor
	{
		public override void ExplicitVisit(CompressionDelayIndexOption node)
		{
			IndexOptionHelper.Instance.GenerateSourceForOption(_writer, node.OptionKind);
			GenerateSpaceAndKeyword(TSqlTokenType.EqualsSign);
			GenerateSpaceAndFragmentIfNotNull(node.Expression);
			if (node.TimeUnit != CompressionDelayTimeUnit.Unitless)
			{
				GenerateSpace();
				CompressionDelayTimeUnitHelper.Instance.GenerateSourceForOption(_writer, node.TimeUnit);
			}
		}
	}
}
