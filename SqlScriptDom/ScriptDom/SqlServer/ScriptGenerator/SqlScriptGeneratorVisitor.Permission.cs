//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.Permission.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(Permission node)
        {
            // Permission names (SELECT, ALTER, ANY, COLUMN, ...) are keyword Identifier fragments and
            // must not be bracketed or recased by the identifier options. The column list below is a
            // real object-name list and is still transformed.
            GenerateWithoutIdentifierFormatting(() => GenerateSpaceSeparatedList(node.Identifiers));

            if (node.Columns != null && node.Columns.Count > 0)
            {
                GenerateSpace();
                GenerateParenthesisedCommaSeparatedList(node.Columns);
            }
        }
    }
}
