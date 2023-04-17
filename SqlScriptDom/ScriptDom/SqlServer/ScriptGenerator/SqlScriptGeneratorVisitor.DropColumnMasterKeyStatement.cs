//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.DropColumnMasterKeyStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    /// <summary>
    /// Script generator for drop column master key statement
    /// </summary>
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(DropColumnMasterKeyStatement node)
        {
            // DROP COLUMN MASTER KEY key_name;
            //
            GenerateKeyword(TSqlTokenType.Drop);

            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Column);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Master);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Key);

            GenerateSpaceAndFragmentIfNotNull(node.Name);
        }
    }
}
