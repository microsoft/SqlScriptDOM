//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.WindowClause.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(WindowClause node)
        {
            GenerateIdentifier(CodeGenerationSupporter.Window);
            GenerateSpace();
            GenerateCommaSeparatedList(node.WindowDefinition);
        }
    }
}
