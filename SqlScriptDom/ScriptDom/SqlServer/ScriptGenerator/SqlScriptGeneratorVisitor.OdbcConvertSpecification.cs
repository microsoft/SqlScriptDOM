//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.OdbcConvertSpecification.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(OdbcConvertSpecification node)
        {
            // The ODBC data-type name is a keyword Identifier fragment; do not bracket or recase it.
            GenerateWithoutIdentifierFormatting(() => GenerateFragmentIfNotNull(node.Identifier));
        }
    }
}
