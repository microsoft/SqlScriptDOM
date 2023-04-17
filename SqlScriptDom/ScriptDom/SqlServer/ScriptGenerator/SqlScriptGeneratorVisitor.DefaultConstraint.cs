//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.DefaultConstraint.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(DefaultConstraintDefinition node)
        {
            GenerateConstraintHead(node);

            GenerateKeywordAndSpace(TSqlTokenType.Default);

            GenerateFragmentIfNotNull(node.Expression);

            if (node.Column != null)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.For);
                GenerateSpaceAndFragmentIfNotNull(node.Column);
            }

            if (node.WithValues)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.With);
                GenerateSpaceAndKeyword(TSqlTokenType.Values); 
            }
        }
    }
}
