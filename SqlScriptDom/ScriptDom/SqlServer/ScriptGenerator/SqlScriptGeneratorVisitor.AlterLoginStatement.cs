//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterLoginStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AlterLoginEnableDisableStatement node)
        {
            GenerateAlterLoginHeader(node);
            GenerateIdentifier(node.IsEnable ? CodeGenerationSupporter.Enable : CodeGenerationSupporter.Disable);
        }

        public override void ExplicitVisit(AlterLoginOptionsStatement node)
        {
            GenerateAlterLoginHeader(node);

            GenerateKeywordAndSpace(TSqlTokenType.With);
            // could be
            //      IdentifierPrincipalOption
            //      PasswordAlterPrincipalOption
            //      OnOffPrincipalOption
            //      SidCreatePrincipalOption
            GenerateFragmentList(node.Options);
        }

        public override void ExplicitVisit(AlterLoginAddDropCredentialStatement node)
        {
            GenerateAlterLoginHeader(node);
            GenerateKeywordAndSpace(node.IsAdd ? TSqlTokenType.Add : TSqlTokenType.Drop);
            GenerateIdentifier(CodeGenerationSupporter.Credential);
            GenerateSpaceAndFragmentIfNotNull(node.CredentialName);
        }

        private void GenerateAlterLoginHeader(AlterLoginStatement node)
        {
            GenerateKeyword(TSqlTokenType.Alter);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Login);
            GenerateSpaceAndFragmentIfNotNull(node.Name);
            GenerateSpace();
        }
    }
}
