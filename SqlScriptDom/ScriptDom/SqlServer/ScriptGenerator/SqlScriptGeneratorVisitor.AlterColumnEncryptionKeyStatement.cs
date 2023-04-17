//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterColumnEncryptionKeyStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    /// <summary>
    /// Generate code for ALTER COLUMN ENCRYPTION KEY STATEMENT
    /// </summary>
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AlterColumnEncryptionKeyStatement node)
        {
            //ALTER COLUMN ENCRYPTION KEY key_name 
            //[ ADD | DROP ] VALUE 
            //(
            //    COLUMN_MASTER_KEY = column_master_key_definition_name 
            //    [, ALGORITHM = 'algorithm_name' , ENCRYPTED_VALUE =  varbinary_literal ] 
            //) [;]

            GenerateKeyword(TSqlTokenType.Alter);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Column);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Encryption);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Key);

            GenerateSpaceAndFragmentIfNotNull(node.Name);

            NewLine();
            if (node.AlterType == ColumnEncryptionKeyAlterType.Add)
            {
                GenerateKeyword(TSqlTokenType.Add);
            }
            else if (node.AlterType == ColumnEncryptionKeyAlterType.Drop)
            {
                GenerateKeyword(TSqlTokenType.Drop);
            }
            else
            {
                throw new InvalidOperationException("ALTER COLUMN ENCRYPTION KEY statement can be used to either ADD or DROP a column encryption key value");
            }

            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Value);

            NewLine();

            GenerateCommaSeparatedList(node.ColumnEncryptionKeyValues, true);
        }
    }
}
