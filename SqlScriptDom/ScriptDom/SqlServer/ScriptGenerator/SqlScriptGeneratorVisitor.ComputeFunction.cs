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
            // The aggregate name here is the same built-in as in a SELECT list, so it follows
            // BuiltInFunctionCasing rather than being emitted as a fixed-case literal.
            if (!ComputeFunctionTypeHelper.Instance.TryGetOptionIdentifier(node.ComputeFunctionType, out string name) ||
                !TryGenerateBuiltInFunctionName(name))
            {
                ComputeFunctionTypeHelper.Instance.GenerateSourceForOption(_writer, node.ComputeFunctionType);
            }

            GenerateParenthesisedFragmentIfNotNull(node.Expression);
        }
    }
}
