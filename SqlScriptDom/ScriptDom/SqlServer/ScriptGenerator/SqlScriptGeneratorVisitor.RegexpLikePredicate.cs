//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.RegexpLikePredicate.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    internal partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(RegexpLikePredicate node)
        {
            GenerateIdentifier(CodeGenerationSupporter.RegexpLike);

            GenerateSpace();
            GenerateSymbol(TSqlTokenType.LeftParenthesis);

            GenerateFragmentIfNotNull(node.Text);
            GenerateSymbol(TSqlTokenType.Comma);
            GenerateSpace();
            GenerateFragmentIfNotNull(node.Pattern);

            if (node.Flags != null)
            {
                GenerateSymbol(TSqlTokenType.Comma);
                GenerateSpace();
                GenerateFragmentIfNotNull(node.Flags);
            }

            GenerateSymbol(TSqlTokenType.RightParenthesis);
        }
    }
}
