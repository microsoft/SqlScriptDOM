//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.ColumnEncryptionKeyValue.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    /// <summary>
    /// Script generator for Column encryption key value parameters
    /// </summary>
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(ColumnEncryptionKeyValue node)
        {
            //(
            //COLUMN_MASTER_KEY = column_master_key_definition_name, 
            //ALGORITHM = 'algorithm_name', 
            //ENCRYPTED_VALUE = varbinary_literal
            //)

            GenerateSymbol(TSqlTokenType.LeftParenthesis);
            NewLineAndIndent();

            GenerateCommaSeparatedList(node.Parameters, true, true);
            NewLine();
            GenerateSymbol(TSqlTokenType.RightParenthesis);
        }
    }
}
