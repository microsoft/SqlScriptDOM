//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.ThrowStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(ThrowStatement node)
        {
            GenerateIdentifier(CodeGenerationSupporter.Throw);
            GenerateSpaceAndFragmentIfNotNull(node.ErrorNumber);
            if (node.ErrorNumber != null && node.Message != null)
            {
                GenerateKeyword(TSqlTokenType.Comma);
            }
            GenerateSpaceAndFragmentIfNotNull(node.Message);
            if (node.Message != null && node.State != null)
            {
                GenerateKeyword(TSqlTokenType.Comma);
            }
            GenerateSpaceAndFragmentIfNotNull(node.State);
        }
    }
}
