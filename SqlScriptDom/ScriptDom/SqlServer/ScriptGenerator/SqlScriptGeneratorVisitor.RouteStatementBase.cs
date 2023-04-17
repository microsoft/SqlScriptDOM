//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.RouteStatementBase.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        protected void GenerateRouteOptions(RouteStatement node)
        {
            // WITH
            NewLineAndIndent();
            GenerateKeyword(TSqlTokenType.With);
            GenerateSpace();
            GenerateCommaSeparatedList(node.RouteOptions);
        }
    }
}
