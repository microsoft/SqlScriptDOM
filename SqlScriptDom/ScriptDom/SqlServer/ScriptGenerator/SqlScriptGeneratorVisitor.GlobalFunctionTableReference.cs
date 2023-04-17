//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.BuiltInFunctionTableSource.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(GlobalFunctionTableReference node)
        {
            GenerateFragmentIfNotNull(node.Name);

            GenerateSpace();
            GenerateParenthesisedCommaSeparatedList(node.Parameters, alwaysGenerateParenthses: true);

            GenerateSpaceAndAlias(node.Alias);
        }
    }
}
