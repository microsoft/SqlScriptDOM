//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterServerConfigurationSetFailoverClusterPropertyStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Diagnostics;
namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AlterServerConfigurationSetFailoverClusterPropertyStatement node)
        {
            GenerateSpaceSeparatedTokens(TSqlTokenType.Alter, CodeGenerationSupporter.Server,
                CodeGenerationSupporter.Configuration);
            GenerateSpace();
            GenerateSpaceSeparatedTokens(TSqlTokenType.Set, CodeGenerationSupporter.Failover,
                CodeGenerationSupporter.Cluster, CodeGenerationSupporter.Property);

            GenerateSpace();
            GenerateCommaSeparatedList(node.Options);
        }

        public override void ExplicitVisit(AlterServerConfigurationFailoverClusterPropertyOption node)
        {
            AlterServerConfigurationFailoverClusterPropertyOptionHelper.Instance.GenerateSourceForOption(_writer, node.OptionKind);
            GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
            GenerateSpaceAndFragmentIfNotNull(node.OptionValue);
        }
    }
}
