//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterServerConfigurationSetDiagnosticsLogStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Diagnostics;
namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AlterServerConfigurationSetDiagnosticsLogStatement node)
        {
            GenerateSpaceSeparatedTokens(TSqlTokenType.Alter, CodeGenerationSupporter.Server,
                CodeGenerationSupporter.Configuration);
            GenerateSpace();
            GenerateSpaceSeparatedTokens(TSqlTokenType.Set, CodeGenerationSupporter.Diagnostics,
                CodeGenerationSupporter.Log);

            GenerateSpace();
            GenerateCommaSeparatedList(node.Options);
        }

        public override void ExplicitVisit(AlterServerConfigurationDiagnosticsLogOption node)
        {
            if (node.OptionKind == AlterServerConfigurationDiagnosticsLogOptionKind.OnOff)
                GenerateFragmentIfNotNull(node.OptionValue);
            else
            {
                AlterServerConfigurationDiagnosticsLogOptionHelper.Instance.GenerateSourceForOption(_writer, node.OptionKind);
                GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
                GenerateSpaceAndFragmentIfNotNull(node.OptionValue);
            }
        }

        public override void ExplicitVisit(AlterServerConfigurationDiagnosticsLogMaxSizeOption node)
        {
            if (node.OptionKind != AlterServerConfigurationDiagnosticsLogOptionKind.MaxSize)
                Debug.Assert(false, "Invalid AlterServerConfigurationDiagnosticsLogOptionKind!");

            AlterServerConfigurationDiagnosticsLogOptionHelper.Instance.GenerateSourceForOption(_writer, node.OptionKind);
            GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
            GenerateSpaceAndFragmentIfNotNull(node.OptionValue);

            if (node.SizeUnit != MemoryUnit.Unspecified)
            {
                GenerateSpace();
                MemoryUnitsHelper.Instance.GenerateSourceForOption(_writer, node.SizeUnit);
            }
        }
    }
}
