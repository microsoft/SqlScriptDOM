//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.ComputeFunction.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(ComputeFunction node)
        {
            ComputeFunctionTypeHelper.Instance.GenerateSourceForOption(_writer, node.ComputeFunctionType);
            GenerateParenthesisedFragmentIfNotNull(node.Expression);
        }
    }
}
