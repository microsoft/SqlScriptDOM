//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.IdentifierLoginOption.cs" company="Microsoft">
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
        private static Dictionary<PrincipalOptionKind, String> _loginOptionsNames = new Dictionary<PrincipalOptionKind, String>()
        {
            {PrincipalOptionKind.CheckExpiration, CodeGenerationSupporter.CheckExpiration},
            {PrincipalOptionKind.CheckPolicy, CodeGenerationSupporter.CheckPolicy},
            {PrincipalOptionKind.Credential, CodeGenerationSupporter.Credential},
            {PrincipalOptionKind.DefaultDatabase, CodeGenerationSupporter.DefaultDatabase},
            {PrincipalOptionKind.DefaultLanguage, CodeGenerationSupporter.DefaultLanguage},
            {PrincipalOptionKind.Name, CodeGenerationSupporter.Name},
            {PrincipalOptionKind.Password, CodeGenerationSupporter.Password},
            {PrincipalOptionKind.Sid, CodeGenerationSupporter.Sid},
            {PrincipalOptionKind.DefaultSchema, CodeGenerationSupporter.DefaultSchema},
            {PrincipalOptionKind.Login, CodeGenerationSupporter.Login},
            {PrincipalOptionKind.Type, CodeGenerationSupporter.Type},
        };

        public override void ExplicitVisit(PrincipalOption node)
        {
            System.Diagnostics.Debug.Assert(node.OptionKind == PrincipalOptionKind.NoCredential);

            GenerateIdentifier(CodeGenerationSupporter.No);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Credential);
        }

        public override void ExplicitVisit(IdentifierPrincipalOption node)
        {
            String optionName = GetValueForEnumKey(_loginOptionsNames, node.OptionKind);
            GenerateNameEqualsValue(optionName, node.Identifier);
        }
    }
}
