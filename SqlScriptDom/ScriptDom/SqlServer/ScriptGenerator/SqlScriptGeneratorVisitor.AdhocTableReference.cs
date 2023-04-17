//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AdhocTableSource.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AdHocTableReference node)
        {
            GenerateFragmentIfNotNull(node.DataSource);
            GenerateSymbol(TSqlTokenType.Dot);
            // could be
            //      schemaObjectThreePartName
            //      stringLiteral
            GenerateFragmentIfNotNull(node.Object);

            GenerateSpaceAndAlias(node.Alias);
        }
    }
}
