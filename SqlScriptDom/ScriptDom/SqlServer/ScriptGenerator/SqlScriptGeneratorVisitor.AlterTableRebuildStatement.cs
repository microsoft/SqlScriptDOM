//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterTableRebuildStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AlterTableRebuildStatement node)
        {
            GenerateAlterTableHead(node);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Rebuild);

            GenerateSpaceAndFragmentIfNotNull(node.Partition);

            if (node.IndexOptions.Count > 0)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.With);
                GenerateSpace();
                GenerateParenthesisedCommaSeparatedList(node.IndexOptions);
            }
        }
    }
}
