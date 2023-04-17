//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.WindowDefinition.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(WindowDefinition node)
        {
            GenerateFragmentIfNotNull(node.WindowName);
            GenerateSpaceAndKeyword(TSqlTokenType.As);

            GenerateSpaceAndSymbol(TSqlTokenType.LeftParenthesis);

            GenerateFragmentIfNotNull(node.RefWindowName);
            bool partitionByClauseExists = node.Partitions.Count > 0;
            if (partitionByClauseExists)
            {
                GenerateIdentifier(CodeGenerationSupporter.Partition);
                GenerateSpaceAndKeyword(TSqlTokenType.By); 

                GenerateSpace();
                GenerateCommaSeparatedList(node.Partitions);
            }

            if (node.OrderByClause != null)
            {
                if (partitionByClauseExists)
                {
                    GenerateSpace();
                }

                // the alignment point is not actually used in this situation
                // we still create since OrderByClause exptects one
                AlignmentPoint windowClauseBody = new AlignmentPoint(ClauseBody);
                GenerateFragmentWithAlignmentPointIfNotNull(node.OrderByClause, windowClauseBody);

                GenerateSpaceAndFragmentIfNotNull(node.WindowFrameClause);
            }

            GenerateSymbol(TSqlTokenType.RightParenthesis); 
        }
    }
}
