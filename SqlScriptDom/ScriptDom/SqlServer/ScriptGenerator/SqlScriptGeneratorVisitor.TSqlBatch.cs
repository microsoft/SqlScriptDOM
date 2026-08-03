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
            for (int index = 0; index < node.Statements.Count; index++)
            {
                TSqlStatement statement = node.Statements[index];
                GenerateStatementWithSemiColon(statement);

                if (index + 1 < node.Statements.Count)
                {
                    GenerateSeparatingSemiColonWhenNecessary(statement, node.Statements[index + 1]);
                }

                if (statement is TSqlStatementSnippet == false)
                {
                    NewLine();
                    NewLine();
                }
            }
        }
    }
}
