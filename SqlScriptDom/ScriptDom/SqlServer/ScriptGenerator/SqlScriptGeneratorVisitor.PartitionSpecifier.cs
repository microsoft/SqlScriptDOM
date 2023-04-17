//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.PartitionSpecifier.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(PartitionSpecifier node)
        {
            GenerateIdentifier(CodeGenerationSupporter.Partition);
            GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
            GenerateSpace();
            if (node.All)
                GenerateKeyword(TSqlTokenType.All);
            else
                GenerateFragmentIfNotNull(node.Number);
        }
    }
}