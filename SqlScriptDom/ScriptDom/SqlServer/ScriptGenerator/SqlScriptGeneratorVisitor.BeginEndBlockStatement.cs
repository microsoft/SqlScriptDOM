//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.BeginEndBlockStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(BeginEndBlockStatement node)
        {
            AlignmentPoint body = new AlignmentPoint();

            GenerateKeyword(TSqlTokenType.Begin);

            NewLineAndIndent();
            MarkAndPushAlignmentPoint(body);
            GenerateFragmentIfNotNull(node.StatementList);
            PopAlignmentPoint();

            NewLine();
            GenerateKeyword(TSqlTokenType.End); 
        }
    }
}
