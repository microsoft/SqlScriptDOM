//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.KeyStoreProviderNameParameter.cs" company="Microsoft">
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
        /// Script generator for KEY_STORE_PROVIDER_NAME = 'key_store_provider_name'
        /// </summary>
        /// <param name="node"></param>
        public override void ExplicitVisit(ColumnMasterKeyStoreProviderNameParameter node)
        {
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.KeyStoreProviderName);
            GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
            GenerateSpaceAndFragmentIfNotNull(node.Name);
        }
    }
}
