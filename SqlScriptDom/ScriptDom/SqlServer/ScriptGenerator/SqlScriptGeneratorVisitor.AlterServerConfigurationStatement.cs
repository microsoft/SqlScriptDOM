//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterServerConfigurationStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Diagnostics;
namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AlterServerConfigurationStatement node)
        {
            GenerateSpaceSeparatedTokens(TSqlTokenType.Alter,
                CodeGenerationSupporter.Server, CodeGenerationSupporter.Configuration);

            GenerateSpaceAndKeyword(TSqlTokenType.Set);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Process);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Affinity);

            switch (node.ProcessAffinity)
            {
                case ProcessAffinityType.CpuAuto:
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Cpu);
                    GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Auto);
                    break;
                case ProcessAffinityType.Cpu:
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Cpu);
                    GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
                    GenerateSpace();
                    GenerateCommaSeparatedList(node.ProcessAffinityRanges);
                    break;
                case ProcessAffinityType.NumaNode:
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.NumaNode);
                    GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
                    GenerateSpace();
                    GenerateCommaSeparatedList(node.ProcessAffinityRanges);
                    break;
                default:
                    Debug.Assert(false, "Unknown ProcessAffinityType!");
                    break;
            }
        }

        public override void ExplicitVisit(ProcessAffinityRange node)
        {
            GenerateFragmentIfNotNull(node.From);
            if (node.To != null)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.To);
                GenerateSpaceAndFragmentIfNotNull(node.To);
            }
        }
    }
}
