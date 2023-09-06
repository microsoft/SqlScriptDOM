//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.ElasticPoolSpecification.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(ElasticPoolSpecification node)
        {
            DatabaseOptionKindHelper.Instance.GenerateSourceForOption(_writer, node.OptionKind);
            GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.ElasticPool);
            GenerateSpaceAndSymbol(TSqlTokenType.LeftParenthesis);
            GenerateIdentifier(CodeGenerationSupporter.Name);
            GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
            GenerateSpaceAndFragmentIfNotNull(node.ElasticPoolName);
            GenerateSymbol(TSqlTokenType.RightParenthesis);
        }
    }
}
