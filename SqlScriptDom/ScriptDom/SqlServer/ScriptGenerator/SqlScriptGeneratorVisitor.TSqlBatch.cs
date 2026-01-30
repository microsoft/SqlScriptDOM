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
            // Comments are now handled automatically by GenerateFragmentIfNotNull
            foreach (TSqlStatement statement in node.Statements)
            {
                GenerateFragmentIfNotNull(statement);

                GenerateSemiColonWhenNecessary(statement);

                if (statement is TSqlStatementSnippet == false)
                {
                    NewLine();
                    NewLine();
                }
            }
        }
    }
}
