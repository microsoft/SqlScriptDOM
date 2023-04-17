//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.InsertBulkStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(InsertBulkStatement node)
        {
            GenerateKeyword(TSqlTokenType.Insert);
            GenerateSpaceAndKeyword(TSqlTokenType.Bulk);
            GenerateSpaceAndFragmentIfNotNull(node.To);

            if (node.ColumnDefinitions.Count > 0)
            {
                GenerateSpace();
                GenerateParenthesisedCommaSeparatedList(node.ColumnDefinitions);
            }

            GenerateOption(node);
        }
    }
}
