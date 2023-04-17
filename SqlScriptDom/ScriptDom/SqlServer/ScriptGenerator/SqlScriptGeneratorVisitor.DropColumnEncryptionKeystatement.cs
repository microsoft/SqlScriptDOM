//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.DropColumnEncryptionKeyStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    /// <summary>
    /// Script generator for drop column encryption key statement
    /// </summary>
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(DropColumnEncryptionKeyStatement node)
        {
            //DROP COLUMN ENCRYPTION KEY key_name [;]
            //
            GenerateKeyword(TSqlTokenType.Drop);

            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Column);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Encryption);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Key);

            GenerateSpaceAndFragmentIfNotNull(node.Name);
        }
    }
}
