//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.EncryptedValueParameter.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    /// <summary>
    /// Script generator for ENCRYPTED_VALUE = varbinary_literal 
    /// </summary>
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(EncryptedValueParameter node)
        {
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.EncryptedValue);
            GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
            GenerateSpaceAndFragmentIfNotNull(node.Value);
        }
    }
}
