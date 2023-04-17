//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CreateViewStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CreateViewStatement node)
        {
            GenerateKeyword(TSqlTokenType.Create);
            GenerateSpace();

            if (node.IsMaterialized)
            {
                GenerateIdentifier(CodeGenerationSupporter.Materialized);
                GenerateSpace();
            }

            GenerateViewStatementBody(node);
        }
    }
}
