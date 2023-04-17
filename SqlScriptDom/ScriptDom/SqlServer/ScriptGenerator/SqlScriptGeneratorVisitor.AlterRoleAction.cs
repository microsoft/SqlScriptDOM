//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterRoleAction.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(RenameAlterRoleAction node)
        {
            GenerateKeywordAndSpace(TSqlTokenType.With);
            GenerateNameEqualsValue(CodeGenerationSupporter.Name, node.NewName);
        }

        public override void ExplicitVisit(AddMemberAlterRoleAction node)
        {
            GenerateSpaceSeparatedTokens(
                TSqlTokenType.Add, 
                CodeGenerationSupporter.Member);

            GenerateSpaceAndFragmentIfNotNull(node.Member);
        }

        public override void ExplicitVisit(DropMemberAlterRoleAction node)
        {
            GenerateSpaceSeparatedTokens(
                TSqlTokenType.Drop,
                CodeGenerationSupporter.Member);

            GenerateSpaceAndFragmentIfNotNull(node.Member);
        }
    }
}
