//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CreateColumnEncryptionKeyStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    /// <summary>
    /// Script generator for create column encryption key statement
    /// </summary>
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CreateColumnEncryptionKeyStatement node)
        {
            //CREATE COLUMN ENCRYPTION KEY key_name 
            //WITH VALUES
            //(
            //    COLUMN_MASTER_KEY = column_master_key_name, 
            //    ALGORITHM = 'algorithm_name', 
            //    ENCRYPTED_VALUE = varbinary_literal
            //) 
            //[, (
            //    COLUMN_MASTER_KEY = column_master_key_name, 
            //    ALGORITHM = 'algorithm_name', 
            //    ENCRYPTED_VALUE = varbinary_literal
            //) ] 
            //[;]

            GenerateKeyword(TSqlTokenType.Create);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Column);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Encryption);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Key);

            GenerateSpaceAndFragmentIfNotNull(node.Name);

            NewLine();
            GenerateKeyword(TSqlTokenType.With);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Values);

            NewLine();

            GenerateCommaSeparatedList(node.ColumnEncryptionKeyValues, true);
        }
    }
}
