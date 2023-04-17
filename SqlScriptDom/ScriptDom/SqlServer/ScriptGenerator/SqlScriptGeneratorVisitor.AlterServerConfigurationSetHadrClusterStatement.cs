//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterServerConfigurationSetHadrClusterStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Diagnostics;
namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AlterServerConfigurationSetHadrClusterStatement node)
        {
            GenerateSpaceSeparatedTokens(TSqlTokenType.Alter, CodeGenerationSupporter.Server,
                CodeGenerationSupporter.Configuration);
            GenerateSpace();
            GenerateSpaceSeparatedTokens(TSqlTokenType.Set, CodeGenerationSupporter.Hadr, CodeGenerationSupporter.Cluster);

            GenerateSpace();
            GenerateCommaSeparatedList(node.Options);
        }

        public override void ExplicitVisit(AlterServerConfigurationHadrClusterOption node)
        {
            AlterServerConfigurationHadrClusterOptionHelper.Instance.GenerateSourceForOption(_writer, node.OptionKind);
            GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
            if (node.IsLocal)
            {
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Local);
            }
            else
            {
                GenerateSpaceAndFragmentIfNotNull(node.OptionValue);
            }
        }
    }
}
