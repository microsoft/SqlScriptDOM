//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CreateDefaultStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CreateDefaultStatement node)
        {
            GenerateKeyword(TSqlTokenType.Create);
            GenerateSpaceAndKeyword(TSqlTokenType.Default); 

            // name
            GenerateSpaceAndFragmentIfNotNull(node.Name);

            // expression
            if (node.Expression != null)
            {
                NewLineAndIndent();
                GenerateKeyword(TSqlTokenType.As);
                GenerateSpaceAndFragmentIfNotNull(node.Expression);
            }
        }
    }
}
