//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterServiceMasterKeyStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AlterServiceMasterKeyStatement node)
        {
            GenerateKeyword(TSqlTokenType.Alter);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Service);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Master);
            GenerateSpaceAndKeyword(TSqlTokenType.Key);
            GenerateSpace();

            switch (node.Kind)
            {
                case AlterServiceMasterKeyOption.Regenerate:
                    GenerateIdentifier(CodeGenerationSupporter.Regenerate); 
                    break;
                case AlterServiceMasterKeyOption.ForceRegenerate:
                    GenerateIdentifier(CodeGenerationSupporter.Force);
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Regenerate); 
                    break;
                case AlterServiceMasterKeyOption.WithNewAccount:
                    GenerateWithClause(node, CodeGenerationSupporter.NewAccount, CodeGenerationSupporter.NewPassword);
                    break;
                case AlterServiceMasterKeyOption.WithOldAccount:
                    GenerateWithClause(node, CodeGenerationSupporter.OldAccount,CodeGenerationSupporter.OldPassword);
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false, "An unhandled option is encountered.");
                    break;
            }
        }

        private void GenerateWithClause(AlterServiceMasterKeyStatement node, String accountTitle, String passwordTitle)
        {
            GenerateKeywordAndSpace(TSqlTokenType.With);

            GenerateNameEqualsValue(accountTitle, node.Account);

            GenerateSymbolAndSpace(TSqlTokenType.Comma);

            GenerateNameEqualsValue(passwordTitle, node.Password);
        }
    }
}
