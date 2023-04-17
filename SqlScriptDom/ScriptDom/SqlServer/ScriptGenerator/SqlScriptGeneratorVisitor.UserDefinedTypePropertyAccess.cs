//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.UdtPropertyExpression.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
		public override void ExplicitVisit(UserDefinedTypePropertyAccess node)
        {
            GenerateFragmentIfNotNull(node.CallTarget);
            GenerateFragmentIfNotNull(node.PropertyName);
			GenerateSpaceAndCollation(node.Collation);
		}
    }
}
