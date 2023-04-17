//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.ServerAuditStatements.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CreateServerAuditStatement node)
        {
            GenerateKeywordAndSpace(TSqlTokenType.Create);
            GenerateServerAuditName(node.AuditName);
            GenerateSpaceAndFragmentIfNotNull(node.AuditTarget);
            GenerateServerAuditOptions(node);
            if (node.PredicateExpression != null)
            {
                NewLineAndIndent();
                GenerateKeywordAndSpace(TSqlTokenType.Where);
                GenerateFragmentIfNotNull(node.PredicateExpression);
            }
        }

        public override void ExplicitVisit(AlterServerAuditStatement node)
        {
            GenerateKeywordAndSpace(TSqlTokenType.Alter);
            GenerateServerAuditName(node.AuditName);

            if (node.NewName != null)
            {
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Modify);
                GenerateSpace();
                GenerateNameEqualsValue(CodeGenerationSupporter.Name, node.NewName);
            }
            else if (node.RemoveWhere)
            {
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Remove);
                GenerateSpaceAndKeyword(TSqlTokenType.Where);
            }
            else
            {
                GenerateSpaceAndFragmentIfNotNull(node.AuditTarget);
                GenerateServerAuditOptions(node);
                if (node.PredicateExpression != null)
                {
                    NewLineAndIndent();
                    GenerateKeywordAndSpace(TSqlTokenType.Where);
                    GenerateFragmentIfNotNull(node.PredicateExpression);
                }
            }
        }

        public override void ExplicitVisit(DropServerAuditStatement node)
        {
            GenerateKeywordAndSpace(TSqlTokenType.Drop);
            GenerateServerAuditName(node.Name);
        }

        private void GenerateServerAuditName(Identifier name)
        {
            GenerateIdentifier(CodeGenerationSupporter.Server);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Audit);
            GenerateSpaceAndFragmentIfNotNull(name);
        }

        private void GenerateServerAuditOptions(ServerAuditStatement node)
        {
            if (node.Options.Count > 0)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.With);
                GenerateSpace();
                GenerateParenthesisedCommaSeparatedList(node.Options);
            }
        }
    }
}
