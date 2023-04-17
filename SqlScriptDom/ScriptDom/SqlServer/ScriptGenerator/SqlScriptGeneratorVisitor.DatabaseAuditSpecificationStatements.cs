//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.DatabaseAuditSpecificationStatements.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AlterDatabaseAuditSpecificationStatement node)
        {
            GenerateKeyword(TSqlTokenType.Alter);
            GenerateSpaceAndKeyword(TSqlTokenType.Database);
            GenerateSpace();
            GenerateAuditSpecificationStatement(node);
        }

        public override void ExplicitVisit(CreateDatabaseAuditSpecificationStatement node)
        {
            GenerateKeyword(TSqlTokenType.Create);
            GenerateSpaceAndKeyword(TSqlTokenType.Database);
            GenerateSpace();
            GenerateAuditSpecificationStatement(node);
        }

        public override void ExplicitVisit(DropDatabaseAuditSpecificationStatement node)
        {
            GenerateKeyword(TSqlTokenType.Drop);
            GenerateSpaceAndKeyword(TSqlTokenType.Database);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Audit);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Specification);
            GenerateSpaceAndFragmentIfNotNull(node.Name);
        }
    }
}
