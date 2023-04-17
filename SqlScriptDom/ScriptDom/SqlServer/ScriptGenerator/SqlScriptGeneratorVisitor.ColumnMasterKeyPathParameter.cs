//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.KeyPathParameter.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        /// <summary>
        /// Script generator for KEY_PATH = 'key_path'
        /// </summary>
        /// <param name="node"></param>
        public override void ExplicitVisit(ColumnMasterKeyPathParameter node)
        {
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.KeyPath);
            GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
            GenerateSpaceAndFragmentIfNotNull(node.Path);
        }
    }
}
