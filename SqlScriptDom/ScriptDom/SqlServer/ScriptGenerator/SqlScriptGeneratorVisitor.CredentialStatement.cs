//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CredentialStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        protected void GenerateCredentialStatementBody(CredentialStatement node)
        {
            if (node.IsDatabaseScoped)
            {
                GenerateIdentifier(CodeGenerationSupporter.Database);
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Scoped);
                GenerateSpace();
            }

            GenerateIdentifier(CodeGenerationSupporter.Credential);

            // name
            GenerateSpaceAndFragmentIfNotNull(node.Name);

            // identity
            if (node.Identity != null)
            {
                NewLineAndIndent();
                GenerateKeyword(TSqlTokenType.With);
                GenerateSpace();
                GenerateNameEqualsValue(TSqlTokenType.Identity, node.Identity);
            }

            // secret
            if (node.Secret != null)
            {
                GenerateSymbolAndSpace(TSqlTokenType.Comma);

                GenerateNameEqualsValue(CodeGenerationSupporter.Secret, node.Secret);
            }
        }
    }
}
