//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.ExtractFromExpression.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(ExtractFromExpression node)
        {
            GenerateSpaceAndFragmentIfNotNull(node.ExtractedElement);
            GenerateSpaceAndKeyword(TSqlTokenType.From);
            GenerateSpaceAndFragmentIfNotNull(node.Expression);
        }
    }
}
