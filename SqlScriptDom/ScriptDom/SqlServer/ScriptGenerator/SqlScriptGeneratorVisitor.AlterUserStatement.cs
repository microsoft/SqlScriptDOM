//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterUserStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AlterUserStatement node)
        {
            GenerateKeyword(TSqlTokenType.Alter);
            GenerateSpaceAndKeyword(TSqlTokenType.User);

            // name
            GenerateSpaceAndFragmentIfNotNull(node.Name);

            // WITH
            NewLineAndIndent();
            GenerateKeyword(TSqlTokenType.With);

            GenerateSpace();
            GenerateCommaSeparatedList(node.UserOptions);
        }
    }
}
