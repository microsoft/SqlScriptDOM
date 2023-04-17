//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.ColumnMasterKeyParameter.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    /// <summary>
    /// Script generator for column master key parameter COLUMN_MASTER_KEY = column_master_key_definition_name
    /// </summary>
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(ColumnMasterKeyNameParameter node)
        {
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.ColumnMasterKey);
            GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
            GenerateSpaceAndFragmentIfNotNull(node.Name);
        }
    }
}
