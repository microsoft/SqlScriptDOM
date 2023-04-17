//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.ColumnEncryptionKeyNameParameter.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    /// <summary>
    /// Script generator for column encryption key name parameter COLUMN_ENCRYPTION_KEY = key_name
    /// </summary>
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(ColumnEncryptionKeyNameParameter node)
        {
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.ColumnEncryptionKey);
            GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
            GenerateSpaceAndFragmentIfNotNull(node.Name);
        }
    }
}
