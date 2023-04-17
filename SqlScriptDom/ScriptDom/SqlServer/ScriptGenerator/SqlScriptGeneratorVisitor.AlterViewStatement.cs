//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterViewStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AlterViewStatement node)
        {
            GenerateKeyword(TSqlTokenType.Alter);
            GenerateSpace();

            if (node.IsMaterialized)
            {
                GenerateIdentifier(CodeGenerationSupporter.Materialized);
                GenerateSpace();
                GenerateKeyword(TSqlTokenType.View);
                GenerateSpaceAndFragmentIfNotNull(node.SchemaObjectName);
                GenerateSpace();

                if (node.IsDisable)
                {
                    GenerateIdentifier(CodeGenerationSupporter.Disable);
                }

                if (node.IsRebuild)
                {
                    GenerateIdentifier(CodeGenerationSupporter.Rebuild);
                }
            }
            else
            {
                GenerateViewStatementBody(node);
            }
        }
    }
}
