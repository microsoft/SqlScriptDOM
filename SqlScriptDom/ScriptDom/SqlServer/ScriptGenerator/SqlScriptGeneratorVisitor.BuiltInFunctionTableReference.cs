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
            // The built-in function name is a keyword-like function name, not an object name; do not
            // bracket or recase it (same rationale as GlobalFunctionTableReference / FunctionCall).
            GenerateWithoutIdentifierFormatting(() => GenerateFragmentIfNotNull(node.Name));

            GenerateSpace();
            GenerateParenthesisedCommaSeparatedList(node.Parameters, true);

            GenerateSpaceAndAlias(node.Alias);
        }
    }
}
