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
        public override void ExplicitVisit(BuiltInFunctionTableReference node)
        {
            GenerateSymbol(TSqlTokenType.DoubleColon);
            GenerateFragmentIfNotNull(node.Name);

            GenerateSpace();
            GenerateParenthesisedCommaSeparatedList(node.Parameters, true);

            GenerateSpaceAndAlias(node.Alias);
        }
    }
}
