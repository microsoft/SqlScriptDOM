//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.NullableConstraint.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(NullableConstraintDefinition node)
        {
            GenerateConstraintHead(node);

            if (!node.Nullable)
            {
                GenerateKeywordAndSpace(TSqlTokenType.Not); 
            }

            GenerateKeyword(TSqlTokenType.Null); 
        }
    }
}
