//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.WhileStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(WhileStatement node)
        {
            AlignmentPoint whileBody = new AlignmentPoint();

            GenerateKeyword(TSqlTokenType.While);
            GenerateSpaceAndFragmentIfNotNull(node.Predicate);

            NewLineAndIndent();
            MarkAndPushAlignmentPoint(whileBody);
            GenerateFragmentIfNotNull(node.Statement);
            GenerateSemiColonWhenNecessary(node.Statement);
            PopAlignmentPoint();
        }
    }
}
