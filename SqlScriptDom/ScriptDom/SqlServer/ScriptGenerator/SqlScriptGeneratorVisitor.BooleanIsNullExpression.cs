//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.BooleanIsNullExpression.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;
using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(BooleanIsNullExpression node)
        {
            GenerateFragmentIfNotNull(node.Expression);
            GenerateSpace();
            GenerateKeywordAndSpace(TSqlTokenType.Is);
            if (node.IsNot)
            {
                GenerateKeywordAndSpace(TSqlTokenType.Not);
            }
            GenerateKeyword(TSqlTokenType.Null);
        }
    }
}
