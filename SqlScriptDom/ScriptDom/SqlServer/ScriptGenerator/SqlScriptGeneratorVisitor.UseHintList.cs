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
        public override void ExplicitVisit(UseHintList node)
        {
            // Syntax: USE HINT('hint1', 'hint2')

            GenerateKeyword(TSqlTokenType.Use);
            GenerateSpace();
            GenerateIdentifier(CodeGenerationSupporter.Hint);

            GenerateParenthesisedCommaSeparatedList(node.Hints);
        }
    }
}
