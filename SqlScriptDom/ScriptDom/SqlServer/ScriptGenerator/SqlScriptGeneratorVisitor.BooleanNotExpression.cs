//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.BooleanNotExpression.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;
using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(BooleanNotExpression node)
        {
            GenerateToken(new KeywordGenerator(TSqlTokenType.Not));
            GenerateSpace();
            GenerateFragmentIfNotNull(node.Expression);
        }
    }
}
