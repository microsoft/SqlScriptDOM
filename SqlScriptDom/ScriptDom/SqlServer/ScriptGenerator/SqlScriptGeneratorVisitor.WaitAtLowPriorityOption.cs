//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.WaitAtLowPriorityOption.cs" company="Microsoft">
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
		public override void ExplicitVisit(WaitAtLowPriorityOption node)
		{
			GenerateLowPriorityWaitOptions(node.Options);
		}
	}
}
