//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.StatisticsOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;
using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(StatisticsOption node)
        {
            switch (node.OptionKind)
            {
                case StatisticsOptionKind.FullScan:
                    GenerateIdentifier(CodeGenerationSupporter.FullScan);
                    break;
                case StatisticsOptionKind.NoRecompute:
                    GenerateIdentifier(CodeGenerationSupporter.NoRecompute);
                    break;
                case StatisticsOptionKind.Resample:
                    GenerateIdentifier(CodeGenerationSupporter.Resample);
                    break;
                case StatisticsOptionKind.All:
                    GenerateKeyword(TSqlTokenType.All);
                    break;
                case StatisticsOptionKind.Columns:
                    GenerateIdentifier(CodeGenerationSupporter.Columns);
                    break;
                case StatisticsOptionKind.Index:
                    GenerateKeyword(TSqlTokenType.Index);
                    break;
                case StatisticsOptionKind.Rows:
                    GenerateIdentifier(CodeGenerationSupporter.Rows);
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false, "invalid option encountered");
                    break;
            }
        }

        public override void ExplicitVisit(LiteralStatisticsOption node)
        {
            switch (node.OptionKind)
            {
                case StatisticsOptionKind.SamplePercent:
                    GenerateIdentifier(CodeGenerationSupporter.Sample);
                    GenerateSpaceAndFragmentIfNotNull(node.Literal);
                    GenerateSpaceAndKeyword(TSqlTokenType.Percent);
                    break;
                case StatisticsOptionKind.SampleRows:
                    GenerateIdentifier(CodeGenerationSupporter.Sample);
                    GenerateSpaceAndFragmentIfNotNull(node.Literal);
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Rows);
                    break;
                case StatisticsOptionKind.StatsStream:
                    GenerateNameEqualsValue(CodeGenerationSupporter.StatsStream, node.Literal);
                    break;
                case StatisticsOptionKind.RowCount:
                    GenerateNameEqualsValue(TSqlTokenType.RowCount, node.Literal);
                    break;
                case StatisticsOptionKind.PageCount:
                    GenerateNameEqualsValue(CodeGenerationSupporter.PageCount, node.Literal);
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false, "invalid option encountered");
                    break;
            }
        }

        public override void ExplicitVisit(OnOffStatisticsOption node)
        {
            String optionName = String.Empty;
            switch (node.OptionKind)
            {
                case StatisticsOptionKind.Incremental:
                    optionName = CodeGenerationSupporter.Incremental;
                    break;
                case StatisticsOptionKind.AutoDrop:
                    optionName = CodeGenerationSupporter.AutoDrop;
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false, "invalid option encountered");
                    break;
            }
            GenerateOptionStateWithEqualSign(optionName, node.OptionState);
        }

        public override void ExplicitVisit(ResampleStatisticsOption node)
        {
            String optionName = String.Empty;
            switch (node.OptionKind)
            {
                case StatisticsOptionKind.Resample:
                    optionName = CodeGenerationSupporter.Resample;
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false, "invalid option encountered");
                    break;
            }
            GenerateIdentifier(optionName);
            if (node.Partitions.Count > 0)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.On);
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Partitions);
                GenerateSpace();
                GenerateParenthesisedCommaSeparatedList(node.Partitions);
            }
        }
    }
}