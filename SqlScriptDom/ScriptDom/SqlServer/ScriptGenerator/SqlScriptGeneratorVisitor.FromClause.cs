//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.FromClause.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Collections.Generic;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(FromClause node)
        {
            GenerateFromClause(node, null);
        }

        protected void GenerateFromClause(FromClause fromClause, AlignmentPoint clauseBody)
        {
            if (fromClause != null)
            {
                IList<TableReference> tableSources = fromClause.TableReferences;
                if (tableSources.Count > 0)
                {
                    GenerateSeparatorForFromClause();

                    AlignmentPoint start = new AlignmentPoint();
                    MarkAndPushAlignmentPoint(start);

                    GenerateKeyword(TSqlTokenType.From);

                    MarkClauseBodyAlignmentWhenNecessary(_options.NewLineBeforeFromClause, clauseBody);

                    GenerateNewLineOrSpace(_options.NewLineAfterFromClause);
                    if (_options.NewLineAfterFromClause)
                    {
                        Indent();
                    }
                    
                    AlignmentPoint fromItems = new AlignmentPoint();
                    MarkAndPushAlignmentPoint(fromItems);

                    GenerateCommaSeparatedList(tableSources);

                    PopAlignmentPoint();

                    PopAlignmentPoint();
                }
            }
        }
    }
}