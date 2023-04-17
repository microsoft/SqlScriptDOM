//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CertificateCreateLoginSource.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CertificateCreateLoginSource node)
        {
            GenerateKeyword(TSqlTokenType.From);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Certificate);

            GenerateSpaceAndFragmentIfNotNull(node.Certificate);

            GenerateCredential(node.Credential);
        }
    }
}
