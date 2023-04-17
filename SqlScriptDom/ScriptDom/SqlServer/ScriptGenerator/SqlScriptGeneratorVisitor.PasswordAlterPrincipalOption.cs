//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.PasswordAlterPrincipalOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(PasswordAlterPrincipalOption node)
        {
            System.Diagnostics.Debug.Assert(node.OptionKind == PrincipalOptionKind.Password);
            GenerateNameEqualsValue(CodeGenerationSupporter.Password, node.Password);

            if (node.OldPassword != null)
            {
                GenerateSpace();
                GenerateNameEqualsValue(CodeGenerationSupporter.OldPassword, node.OldPassword);
            }
            else
            {
                if (node.MustChange)
                {
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.MustChange); 
                }

                if (node.Hashed)
                {
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Hashed); 
                }

                if (node.Unlock)
                {
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Unlock); 
                }
            }
        }
    }
}
