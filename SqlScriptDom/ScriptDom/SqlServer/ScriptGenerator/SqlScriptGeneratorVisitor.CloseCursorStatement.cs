//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CloseCursorStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CloseCursorStatement node)
        {
            GenerateKeyword(TSqlTokenType.Close);
            GenerateSpaceAndFragmentIfNotNull(node.Cursor);
        }
    }
}
