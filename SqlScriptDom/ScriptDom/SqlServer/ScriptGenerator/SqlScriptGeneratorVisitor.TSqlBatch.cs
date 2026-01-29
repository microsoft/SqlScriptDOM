//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.TSqlBatch.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(TSqlBatch node)
        {
            // Emit leading comments before the batch
            BeforeVisitFragment(node);

            foreach (TSqlStatement statement in node.Statements)
            {
                // Emit leading comments before each statement
                BeforeVisitFragment(statement);

                GenerateFragmentIfNotNull(statement);

                GenerateSemiColonWhenNecessary(statement);

                // Emit trailing comments after each statement
                AfterVisitFragment(statement);

                if (statement is TSqlStatementSnippet == false)
                {
                    NewLine();
                    NewLine();
                }
            }

            // Emit trailing comments after the batch
            AfterVisitFragment(node);
        }
    }
}
