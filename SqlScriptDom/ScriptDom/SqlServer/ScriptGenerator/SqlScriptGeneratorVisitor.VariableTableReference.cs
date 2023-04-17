//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.VariableTableSource.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(VariableTableReference node)
        {
            GenerateFragmentIfNotNull(node.Variable);

            GenerateSpaceAndAlias(node.Alias);
        }

        public override void ExplicitVisit(VariableMethodCallTableReference node)
        {
            GenerateFragmentIfNotNull(node.Variable);

            GenerateSymbol(TSqlTokenType.Dot);
            GenerateFragmentIfNotNull(node.MethodName);
            GenerateParenthesisedCommaSeparatedList(node.Parameters, true);
            
            GenerateTableAndColumnAliases(node);
        }
    }
}
