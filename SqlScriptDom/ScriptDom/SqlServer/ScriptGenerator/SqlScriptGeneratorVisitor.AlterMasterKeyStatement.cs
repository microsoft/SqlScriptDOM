//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterMasterKeyStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AlterMasterKeyStatement node)
        {
            GenerateKeyword(TSqlTokenType.Alter); 
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Master); 
            GenerateSpaceAndKeyword(TSqlTokenType.Key);
            
            GenerateSpace();

            switch (node.Option)
            {
                case AlterMasterKeyOption.Regenerate:
                    GenerateRegenerateOption(node.Password);
                    break;
                case AlterMasterKeyOption.ForceRegenerate:
                    GenerateIdentifier(CodeGenerationSupporter.Force);
                    GenerateSpace();
                    GenerateRegenerateOption(node.Password);
                    break;
                case AlterMasterKeyOption.AddEncryptionByPassword:
                    GenerateKeywordAndSpace(TSqlTokenType.Add);
                    GenerateEncryptionByPassword(node.Password);
                    break;
                case AlterMasterKeyOption.DropEncryptionByPassword:
                    GenerateKeywordAndSpace(TSqlTokenType.Drop);
                    GenerateEncryptionByPassword(node.Password);
                    break;
                case AlterMasterKeyOption.AddEncryptionByServiceMasterKey:
                    GenerateKeywordAndSpace(TSqlTokenType.Add); 
                    GenerateEncyptionByServiceMasterKey();
                    break;
                case AlterMasterKeyOption.DropEncryptionByServiceMasterKey:
                    GenerateKeywordAndSpace(TSqlTokenType.Drop); 
                    GenerateEncyptionByServiceMasterKey();
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false, "An unhandled option is encountered.");
                    break;
            }
        }

        private void GenerateRegenerateOption(Literal password)
        {
            GenerateIdentifier(CodeGenerationSupporter.Regenerate);
            GenerateSpaceAndKeyword(TSqlTokenType.With);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Encryption);
            GenerateSpaceAndKeyword(TSqlTokenType.By);
            GenerateSpace();
            GenerateNameEqualsValue(CodeGenerationSupporter.Password, password);
        }

        private void GenerateEncyptionByServiceMasterKey()
        {
            GenerateIdentifier(CodeGenerationSupporter.Encryption); 
            GenerateSpaceAndKeyword(TSqlTokenType.By); 
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Service); 
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Master); 
            GenerateSpaceAndKeyword(TSqlTokenType.Key); 
        }
    }
}
