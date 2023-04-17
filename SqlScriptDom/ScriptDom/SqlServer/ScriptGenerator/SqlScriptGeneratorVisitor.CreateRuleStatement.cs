//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CreateRuleStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CreateRuleStatement node)
        {
            GenerateKeyword(TSqlTokenType.Create);
            GenerateSpaceAndKeyword(TSqlTokenType.Rule);

            // name
            GenerateSpaceAndFragmentIfNotNull(node.Name);

            // AS
            NewLineAndIndent();
            GenerateKeyword(TSqlTokenType.As);
            GenerateSpaceAndFragmentIfNotNull(node.Expression);
        }
    }
}
