//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AuditSpecificationStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        protected void GenerateAuditSpecificationStatement(AuditSpecificationStatement node)
        {
            GenerateIdentifier(CodeGenerationSupporter.Audit);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Specification);
            GenerateSpaceAndFragmentIfNotNull(node.SpecificationName);

            if (node.AuditName != null || node is CreateServerAuditSpecificationStatement || node is CreateDatabaseAuditSpecificationStatement)
            {
                //Always include FOR SERVER AUDIT in CREATE as it is a required part of the syntax.
                //
                NewLineAndIndent();
                GenerateKeyword(TSqlTokenType.For);
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Server);
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Audit);
                GenerateSpaceAndFragmentIfNotNull(node.AuditName);
            }

            if (node.Parts.Count > 0)
                NewLineAndIndent();

            GenerateList(node.Parts, delegate()
                {
                    GenerateSymbol(TSqlTokenType.Comma);
                    NewLineAndIndent();
                });

            if (node.AuditState != OptionState.NotSet)
            {
                NewLineAndIndent();
                GenerateKeyword(TSqlTokenType.With);
                GenerateSymbol(TSqlTokenType.LeftParenthesis);
                GenerateOptionStateWithEqualSign(CodeGenerationSupporter.State, node.AuditState);
                GenerateSymbol(TSqlTokenType.RightParenthesis);
            }
        }
    }
}