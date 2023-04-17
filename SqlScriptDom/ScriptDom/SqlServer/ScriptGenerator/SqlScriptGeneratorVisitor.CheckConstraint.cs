//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CheckConstraint.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CheckConstraintDefinition node)
        {
            AlignmentPoint start = new AlignmentPoint();

            MarkAndPushAlignmentPoint(start);

            GenerateConstraintHead(node);

            GenerateKeyword(TSqlTokenType.Check); 

            if (node.NotForReplication)
            {
                GenerateSpace();
                GenerateNotForReplication();
            }

            GenerateSpace();

            GenerateParenthesisedFragmentIfNotNull(node.CheckCondition);

            PopAlignmentPoint();
        }
    }
}
