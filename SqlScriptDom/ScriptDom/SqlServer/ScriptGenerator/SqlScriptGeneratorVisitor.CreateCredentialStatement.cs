//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CreateCredentialStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CreateCredentialStatement node)
        {
            GenerateKeywordAndSpace(TSqlTokenType.Create);

            GenerateCredentialStatementBody(node);

            // cryptographic provider name <!-- New for T-SQL 100 -->
            if (node.CryptographicProviderName != null)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.For);
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Cryptographic);
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Provider);
                GenerateSpaceAndFragmentIfNotNull(node.CryptographicProviderName);
            }
        }
    }
}
