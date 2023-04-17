//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterServerConfigurationSetSoftNumaStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Diagnostics;
namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AlterServerConfigurationSetSoftNumaStatement node)
        {
            GenerateSpaceSeparatedTokens(TSqlTokenType.Alter, CodeGenerationSupporter.Server,
                CodeGenerationSupporter.Configuration);

            GenerateSpace();

            GenerateSpaceSeparatedTokens(TSqlTokenType.Set, CodeGenerationSupporter.SoftNuma);

            GenerateSpace();

            GenerateCommaSeparatedList(node.Options);
        }

        public override void ExplicitVisit(AlterServerConfigurationSoftNumaOption node)
        {
            if (node.OptionKind == AlterServerConfigurationSoftNumaOptionKind.OnOff)
                GenerateFragmentIfNotNull(node.OptionValue);
            else
            {
                Debug.Assert(false, "Invalid AlterServerConfigurationSoftNumaOptionKind!");
            }
        }
    }
}
