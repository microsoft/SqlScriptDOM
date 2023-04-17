//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterServerConfigurationSetExternalAuthenticationStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Diagnostics;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AlterServerConfigurationSetExternalAuthenticationStatement node)
        {
            GenerateSpaceSeparatedTokens(TSqlTokenType.Alter, CodeGenerationSupporter.Server,
                CodeGenerationSupporter.Configuration);
            GenerateSpace();
            GenerateSpaceSeparatedTokens(TSqlTokenType.Set, CodeGenerationSupporter.External,
                CodeGenerationSupporter.Authentication);

            GenerateSpace();
            GenerateCommaSeparatedList(node.Options);
        }

        public override void ExplicitVisit(AlterServerConfigurationExternalAuthenticationContainerOption node)
        {
            GenerateFragmentIfNotNull(node.OptionValue);

            if (node.Suboptions.Count > 0)
            {
                if (node.Suboptions[0].OptionKind == AlterServerConfigurationExternalAuthenticationOptionKind.CredentialName
                    || node.Suboptions[0].OptionKind == AlterServerConfigurationExternalAuthenticationOptionKind.UseIdentity)
                {
                    GenerateSpace();
                    GenerateParenthesisedFragmentIfNotNull(node.Suboptions[0]);
                }
            }
        }

        public override void ExplicitVisit(AlterServerConfigurationExternalAuthenticationOption node)
        {
            AlterServerConfigurationExternalAuthenticationOptionHelper.Instance.GenerateSourceForOption(_writer, node.OptionKind);

            if (node.OptionKind == AlterServerConfigurationExternalAuthenticationOptionKind.CredentialName)
            {
                GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
                GenerateSpaceAndFragmentIfNotNull(node.OptionValue);
            }
        }
    }
}
