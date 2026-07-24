//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.OpenRowsetColumnDefinition.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(OpenRowsetColumnDefinition node)
        {
            GenerateFragmentIfNotNull(node.ColumnIdentifier);

            GenerateSpaceAndFragmentIfNotNull(node.DataType);

            // Collation names cannot be delimited or recased; GenerateSpaceAndCollation suppresses
            // identifier formatting for them.
            GenerateSpaceAndCollation(node.Collation);

            if (node.ColumnOrdinal != null)
            {
                GenerateSpaceAndFragmentIfNotNull(node.ColumnOrdinal);
            }
            else
            {
                GenerateSpaceAndFragmentIfNotNull(node.JsonPath);
            }
        }
    }
}
