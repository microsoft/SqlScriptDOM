//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AsymmetricKeyCreateLoginSource.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AsymmetricKeyCreateLoginSource node)
        {
            GenerateKeyword(TSqlTokenType.From);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Asymmetric);
            GenerateSpaceAndKeyword(TSqlTokenType.Key);

            GenerateSpaceAndFragmentIfNotNull(node.Key);

            GenerateCredential(node.Credential);
        }
    }
}
