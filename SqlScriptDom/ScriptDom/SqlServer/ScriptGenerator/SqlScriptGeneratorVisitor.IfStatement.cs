//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.IfStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(IfStatement node)
        {
            AlignmentPoint branchStatements = new AlignmentPoint();

            GenerateKeyword(TSqlTokenType.If);
            GenerateSpaceAndFragmentIfNotNull(node.Predicate);

            NewLineAndIndent();
            MarkAndPushAlignmentPoint(branchStatements);
            GenerateStatementWithSemiColon(node.ThenStatement);
            PopAlignmentPoint();

            if (node.ElseStatement != null)
            {
                NewLine();
                GenerateKeyword(TSqlTokenType.Else);
                NewLineAndIndent();
                MarkAndPushAlignmentPoint(branchStatements);
                GenerateStatementWithSemiColon(node.ElseStatement);
                PopAlignmentPoint();
            }
        }
    }
}
