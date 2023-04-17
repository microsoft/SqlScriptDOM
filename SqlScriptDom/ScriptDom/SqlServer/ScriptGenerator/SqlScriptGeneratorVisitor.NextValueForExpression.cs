//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.NextValueForExpression.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(NextValueForExpression node)
        {
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Next);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Value);
            GenerateSpaceAndKeyword(TSqlTokenType.For);
            GenerateSpaceAndFragmentIfNotNull(node.SequenceName);

            if (node.OverClause != null)
            {
                GenerateSpace();
                ExplicitVisit(node.OverClause);
            }
        }

    }
}
