//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.ColumnEncryptionTypeParameter.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    /// <summary>
    /// Script generator for column encryption type parameter ENCRYPTION_TYPE = { DETERMINISTIC | RANDOMIZED }
    /// </summary>
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(ColumnEncryptionTypeParameter node)
        {
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.EncryptionType);
            GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
            switch(node.EncryptionType)
            {
                case ColumnEncryptionType.Deterministic:
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Deterministic);
                    break;
                case ColumnEncryptionType.Randomized:
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Randomized);
                    break;
                default:
                    throw new ArgumentException("Encryption type can be DETERMINISTIC or RANDOMIZED", "EncryptionType");
            }
        }
    }
}
