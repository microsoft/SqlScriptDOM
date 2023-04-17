//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.JsonForClause.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(JsonForClause node)
        {
            GenerateIdentifier(CodeGenerationSupporter.Json);

            GenerateSpace();
            GenerateCommaSeparatedList(node.Options);
        }
    }
}
