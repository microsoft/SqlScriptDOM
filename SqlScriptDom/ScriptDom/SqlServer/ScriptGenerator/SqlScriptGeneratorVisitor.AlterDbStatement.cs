//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterDbStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        protected void GenerateAlterDbStatementHead(AlterDatabaseStatement node)
        {
            GenerateSpaceSeparatedTokens(TSqlTokenType.Alter, TSqlTokenType.Database);
            if (node.UseCurrent)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.Current);
            }
            else
            {
                GenerateSpaceAndFragmentIfNotNull(node.DatabaseName);
            }

        }
    }
}
