//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterServerConfigurationSetBufferPoolExtensionStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Diagnostics;
namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AlterServerConfigurationSetBufferPoolExtensionStatement node)
        {
            GenerateSpaceSeparatedTokens(TSqlTokenType.Alter, CodeGenerationSupporter.Server,
                CodeGenerationSupporter.Configuration);
            GenerateSpace();
            GenerateSpaceSeparatedTokens(TSqlTokenType.Set, CodeGenerationSupporter.Buffer,
                CodeGenerationSupporter.Pool, CodeGenerationSupporter.Extension);

            GenerateSpace();
            GenerateCommaSeparatedList(node.Options);
        }

        public override void ExplicitVisit(AlterServerConfigurationBufferPoolExtensionContainerOption node)
        {
            GenerateFragmentIfNotNull(node.OptionValue);

            if (node.Suboptions.Count > 0)
            {
                GenerateSpace();
                GenerateParenthesisedCommaSeparatedList(node.Suboptions);
            }
        }

        public override void ExplicitVisit(AlterServerConfigurationBufferPoolExtensionOption node)
        {
            AlterServerConfigurationBufferPoolExtensionOptionHelper.Instance.GenerateSourceForOption(_writer, node.OptionKind);
            GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
            GenerateSpaceAndFragmentIfNotNull(node.OptionValue);
        }

        public override void ExplicitVisit(AlterServerConfigurationBufferPoolExtensionSizeOption node)
        {
            AlterServerConfigurationBufferPoolExtensionOptionHelper.Instance.GenerateSourceForOption(_writer, node.OptionKind);
            GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
            GenerateSpaceAndFragmentIfNotNull(node.OptionValue);
            GenerateSpace();
            MemoryUnitsHelper.Instance.GenerateSourceForOption(_writer, node.SizeUnit);
        }
    }
}