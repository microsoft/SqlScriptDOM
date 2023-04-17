//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.DatabaseEncryptionKeyStatements.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AlterDatabaseEncryptionKeyStatement node)
        {
            GenerateKeywordAndSpace(TSqlTokenType.Alter);
            GenerateDatabaseEncryptionKeyHeader();

            if (node.Regenerate)
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Regenerate);

            GenerateSpace();
            GenerateDatabaseEncryptionKeyStatementBody(node);
        }

        public override void ExplicitVisit(CreateDatabaseEncryptionKeyStatement node)
        {
            GenerateKeywordAndSpace(TSqlTokenType.Create);
            GenerateDatabaseEncryptionKeyHeader();
            GenerateSpace();

            GenerateDatabaseEncryptionKeyStatementBody(node);
        }

        public override void ExplicitVisit(DropDatabaseEncryptionKeyStatement node)
        {
            GenerateKeywordAndSpace(TSqlTokenType.Drop);
            GenerateDatabaseEncryptionKeyHeader();
        }

        private void GenerateDatabaseEncryptionKeyHeader()
        {
            GenerateKeyword(TSqlTokenType.Database);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Encryption);
            GenerateSpaceAndKeyword(TSqlTokenType.Key);
        }

        protected void GenerateDatabaseEncryptionKeyStatementBody(DatabaseEncryptionKeyStatement node)
        {
            if (node.Algorithm != DatabaseEncryptionKeyAlgorithm.None)
            {
                GenerateKeyword(TSqlTokenType.With);
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Algorithm);
                GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
                GenerateSpace();
                DatabaseEncryptionKeyAlgorithmHelper.Instance.GenerateSourceForOption(_writer, node.Algorithm);
            }

            if (node.Encryptor != null)
            {
                NewLineAndIndent();
                GenerateIdentifier(CodeGenerationSupporter.Encryption);
                GenerateSpaceAndKeyword(TSqlTokenType.By);
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Server);
                GenerateSpace();
                GenerateFragmentIfNotNull(node.Encryptor);
            }
        }
    }
}
