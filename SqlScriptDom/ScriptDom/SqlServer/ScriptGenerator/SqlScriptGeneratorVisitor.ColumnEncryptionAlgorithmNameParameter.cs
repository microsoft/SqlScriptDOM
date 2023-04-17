//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.EncryptionAlgorithmParameter.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    /// <summary>
    /// Script generator for ALGORITHM = 'algorithm_name'
    /// </summary>
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(ColumnEncryptionAlgorithmNameParameter node)
        {
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Algorithm);
            GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
            GenerateSpaceAndFragmentIfNotNull(node.Algorithm);
        }
    }
}
