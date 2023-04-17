//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.ColumnEncryptionDefinition.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    /// <summary>
    /// Script generator for column encryption paramters
    /// </summary>
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(ColumnEncryptionDefinition node)
        {
            //ENCRYPTED WITH 
            //( COLUMN_ENCRYPTION_KEY = key_name ,
            //ENCRYPTION_TYPE = { DETERMINISTIC | RANDOMIZED } , 
            //ALGORITHM = 'AEAD_AES_256_CBC_HMAC_SHA_256'
            //)

            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Encrypted);
            GenerateSpaceAndKeyword(TSqlTokenType.With);
            GenerateSpaceAndSymbol(TSqlTokenType.LeftParenthesis);
            NewLineAndIndent();
            
            GenerateCommaSeparatedList(node.Parameters, true, true);
            NewLineAndIndent();
            GenerateSymbol(TSqlTokenType.RightParenthesis);
        }
    }
}
