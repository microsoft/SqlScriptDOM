//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.OverClause.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(OverClause node)
        {
            GenerateKeyword(TSqlTokenType.Over);

            bool windowNameExists = null != node.WindowName;
            bool windowSpecExist = (node.Partitions.Count > 0) || (node.OrderByClause != null) || (node.WindowFrameClause != null);

            if (windowNameExists && !windowSpecExist)
            {
                GenerateSpaceAndFragmentIfNotNull(node.WindowName);
            }
            else
            {
                GenerateSpaceAndSymbol(TSqlTokenType.LeftParenthesis);

                GenerateFragmentIfNotNull(node.WindowName);

                bool partitionByClauseExists = node.Partitions.Count > 0;
                if (partitionByClauseExists)
                {
                    if (windowNameExists)
                    {
                        GenerateSpace();
                    }

                    GenerateIdentifier(CodeGenerationSupporter.Partition);
                    GenerateSpaceAndKeyword(TSqlTokenType.By);

                    GenerateSpace();
                    GenerateCommaSeparatedList(node.Partitions);
                }

                if (node.OrderByClause != null)
                {
                    if (windowNameExists || partitionByClauseExists)
                    {
                        GenerateSpace();
                    }

                    // the alignment point is not actually used in this situation
                    // we still create since OrderByClause exptects one
                    AlignmentPoint overClauseBody = new AlignmentPoint(ClauseBody);
                    GenerateFragmentWithAlignmentPointIfNotNull(node.OrderByClause, overClauseBody);

                    GenerateSpaceAndFragmentIfNotNull(node.WindowFrameClause);
                }

                if (null == node.OrderByClause && windowNameExists)
                {
                    GenerateSpaceAndFragmentIfNotNull(node.WindowFrameClause);
                }

                GenerateSymbol(TSqlTokenType.RightParenthesis);
            }
        }
    }
}
