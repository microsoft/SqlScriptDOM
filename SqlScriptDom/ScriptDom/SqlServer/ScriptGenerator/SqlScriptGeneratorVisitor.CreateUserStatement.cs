//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CreateUserStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CreateUserStatement node)
        {
            GenerateKeyword(TSqlTokenType.Create);
            GenerateSpaceAndKeyword(TSqlTokenType.User);

            // name
            GenerateSpaceAndFragmentIfNotNull(node.Name);

            // login option
            GenerateSpaceAndFragmentIfNotNull(node.UserLoginOption);

            // WITH
            if (node.UserOptions != null && node.UserOptions.Count > 0)
            {
                NewLineAndIndent();
                GenerateKeyword(TSqlTokenType.With);
                GenerateSpace();
                GenerateCommaSeparatedList(node.UserOptions);
            }
        }
    }
}
