//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.ServerAuditSpecificationStatements.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AlterServerAuditSpecificationStatement node)
        {
            GenerateKeyword(TSqlTokenType.Alter);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Server);
            GenerateSpace();
            GenerateAuditSpecificationStatement(node);
        }

        public override void ExplicitVisit(CreateServerAuditSpecificationStatement node)
        {
            GenerateKeyword(TSqlTokenType.Create);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Server);
            GenerateSpace();
            GenerateAuditSpecificationStatement(node);
        }

        public override void ExplicitVisit(DropServerAuditSpecificationStatement node)
        {
            GenerateKeyword(TSqlTokenType.Drop);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Server);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Audit);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Specification);
            GenerateSpaceAndFragmentIfNotNull(node.Name);
        }
    }
}
