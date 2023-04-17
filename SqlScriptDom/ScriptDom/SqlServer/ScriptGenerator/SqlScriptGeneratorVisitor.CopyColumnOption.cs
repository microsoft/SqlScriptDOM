//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CopyColumnOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CopyColumnOption node)
        {
            GenerateSpaceAndFragmentIfNotNull(node.ColumnName);
            GenerateSpaceAndKeyword(TSqlTokenType.Default);
            GenerateSpaceAndFragmentIfNotNull(node.DefaultValue);
            GenerateSpaceAndFragmentIfNotNull(node.FieldNumber);
        }
    }
}
