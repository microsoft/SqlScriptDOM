//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.BulkInsertStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(BulkInsertStatement node)
        {
            GenerateKeyword(TSqlTokenType.Bulk);
            GenerateSpaceAndKeyword(TSqlTokenType.Insert);
            GenerateSpaceAndFragmentIfNotNull(node.To);
            GenerateSpaceAndKeyword(TSqlTokenType.From);
            GenerateSpaceAndFragmentIfNotNull(node.From);

            GenerateOption(node);
        }
    }
}
