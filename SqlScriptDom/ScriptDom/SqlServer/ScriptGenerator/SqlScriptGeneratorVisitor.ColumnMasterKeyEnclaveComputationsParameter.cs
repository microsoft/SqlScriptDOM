// <copyright file="SqlScriptGeneratorVisitor.ColumnMasterKeyEnclaveComputationsParameter.cs" company="Microsoft">
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
        /// Script generator for ENCLAVE_COMPUTATIONS (SIGNATURE = signature)
        /// </summary>
        /// <param name="node"></param>
        public override void ExplicitVisit(ColumnMasterKeyEnclaveComputationsParameter node)
        {
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.EnclaveComputations);
            GenerateSpaceAndSymbol(TSqlTokenType.LeftParenthesis);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Signature);
            GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
            GenerateSpaceAndFragmentIfNotNull(node.Signature);
            GenerateSpaceAndSymbol(TSqlTokenType.RightParenthesis);
        }
    }
}
