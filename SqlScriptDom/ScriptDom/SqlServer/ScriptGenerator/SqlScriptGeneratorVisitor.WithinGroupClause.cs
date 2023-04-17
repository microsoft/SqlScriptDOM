//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.WithinGroup.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(WithinGroupClause node)
        {
            GenerateIdentifier(CodeGenerationSupporter.Within);
            GenerateSpaceAndKeyword(TSqlTokenType.Group);

            GenerateSpaceAndSymbol(TSqlTokenType.LeftParenthesis);

            if (node.HasGraphPath)
            {
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Graph);
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Path);
            }
            else
            {
                // the alignment point is not actually used in this situation
                // we still create since OrderByClause exptects one
                AlignmentPoint overClauseBody = new AlignmentPoint(ClauseBody);
                GenerateFragmentWithAlignmentPointIfNotNull(node.OrderByClause, overClauseBody);
            }

            GenerateSymbol(TSqlTokenType.RightParenthesis);
        }
    }
}
