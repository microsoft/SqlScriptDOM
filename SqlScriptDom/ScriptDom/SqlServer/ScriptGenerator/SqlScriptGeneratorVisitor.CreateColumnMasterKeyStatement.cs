//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CreateColumnMasterKey.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    /// <summary>
    /// Create column master key statement
    /// </summary>
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CreateColumnMasterKeyStatement node)
        {
            //CREATE COLUMN MASTER KEY key_name 
            //WITH (
            //    KEY_STORE_PROVIDER_NAME = 'key_store_provider_name',
            //    KEY_PATH = 'key_path'
            //    [,ENCLAVE_COMPUTATIONS (SIGNATURE = signature)]
            //) 
            //[;]

            GenerateKeyword(TSqlTokenType.Create);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Column);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Master);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Key);

            GenerateSpaceAndFragmentIfNotNull(node.Name);

            NewLine();
            GenerateKeyword(TSqlTokenType.With);
            GenerateSpaceAndSymbol(TSqlTokenType.LeftParenthesis);
            NewLineAndIndent();

            GenerateCommaSeparatedList(node.Parameters, true, true);

            NewLine();
            GenerateSymbol(TSqlTokenType.RightParenthesis);
        }
    }
}