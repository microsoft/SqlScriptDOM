//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.InsertBulkColumnDefinition.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(InsertBulkColumnDefinition node)
        {
            GenerateFragmentIfNotNull(node.Column);

            switch (node.NullNotNull)
            {
                case NullNotNull.Null:
                    GenerateSpaceAndKeyword(TSqlTokenType.Null);
                    break;
                case NullNotNull.NotNull:
                    GenerateSpaceAndKeyword(TSqlTokenType.Not);
                    GenerateSpaceAndKeyword(TSqlTokenType.Null);
                    break;
                case NullNotNull.NotSpecified:
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false, "An unhandled option is encountered.");
                    break;
            }
        }
    }
}
