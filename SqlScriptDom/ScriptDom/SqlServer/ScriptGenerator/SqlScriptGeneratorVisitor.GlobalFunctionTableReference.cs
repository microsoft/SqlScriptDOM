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
            // The global table-function name (e.g. STRING_SPLIT) is a built-in function name, not an
            // object name. Bracketing it ("[STRING_SPLIT]") makes the parser read it as a user-defined
            // function, so emit it without identifier formatting, matching FunctionCall's handling.
            GenerateWithoutIdentifierFormatting(() => GenerateFragmentIfNotNull(node.Name));

            GenerateSpace();
            GenerateParenthesisedCommaSeparatedList(node.Parameters, alwaysGenerateParenthses: true);

            GenerateSpaceAndAlias(node.Alias);
        }
    }
}
