//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.StatementList.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;
using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(StatementList node)
        {
            if (node.Statements != null)
            {
                Boolean first = true;
                foreach (TSqlStatement statement in node.Statements)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        for (var i = 0; i < _options.NumNewlinesAfterStatement; i++)
                        {
                            NewLine();
                        }
                    }

                    GenerateFragmentIfNotNull(statement);
                    GenerateSemiColonWhenNecessary(statement);
                }
            }
        }
    }
}
