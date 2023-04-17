//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CreateLoginStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Collections.Generic;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CreateLoginStatement node)
        {
            GenerateKeyword(TSqlTokenType.Create);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Login);

            // name
            GenerateSpaceAndFragmentIfNotNull(node.Name);

            // soruce, could be
            //      AsymmetricKeyCreateLoginSource
            //      CertificateCreateLoginSource
            //      PasswordCreateLoginSource
            //      WindowsCreateLoginSource
            //      ExternalCreateLoginSource
            NewLineAndIndent();
            GenerateFragmentIfNotNull(node.Source);
        }
    }
}
