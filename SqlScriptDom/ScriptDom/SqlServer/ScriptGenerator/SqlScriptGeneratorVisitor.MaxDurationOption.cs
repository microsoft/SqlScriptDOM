//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.MaxDurationOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
	partial class SqlScriptGeneratorVisitor
	{
		public override void ExplicitVisit(MaxDurationOption node)
		{
			GenerateTokenAndEqualSign(CodeGenerationSupporter.MaxDuration);
			GenerateSpaceAndFragmentIfNotNull(node.MaxDuration);
			if (node.Unit.HasValue)
			{
				GenerateSpace();
				LowPriorityLockWaitMaxDurationTimeUnitHelper.Instance.GenerateSourceForOption(_writer, node.Unit.Value);
			}
		}
	}
}
