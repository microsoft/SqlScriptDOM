//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.SystemVersioningTableOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(SystemVersioningTableOption node)
        {
            GenerateIdentifier(CodeGenerationSupporter.SystemVersioning);
            GenerateSpaceAndKeyword(TSqlTokenType.EqualsSign);

            if (node.OptionState == OptionState.On)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.On);

                if (node.ConsistencyCheckEnabled != OptionState.NotSet || node.HistoryTable != null || node.RetentionPeriod != null)
                {
                    bool needComma = false;
                    GenerateSpaceAndKeyword(TSqlTokenType.LeftParenthesis);
                    
                    if (node.HistoryTable != null)
                    {
                        GenerateIdentifier(CodeGenerationSupporter.HistoryTable);
                        GenerateKeyword(TSqlTokenType.EqualsSign);
                        GenerateFragmentIfNotNull(node.HistoryTable);

                        needComma = true;
                    }

                    if (node.ConsistencyCheckEnabled != OptionState.NotSet)
                    {
                        if (needComma)
                        {
                            GenerateKeyword(TSqlTokenType.Comma);
                        }

                        GenerateSpaceAndIdentifier(CodeGenerationSupporter.DataConsistencyCheck);
                        GenerateKeyword(TSqlTokenType.EqualsSign);
                        
                        if (node.ConsistencyCheckEnabled == OptionState.On)
                        {
                            GenerateKeyword(TSqlTokenType.On);
                        }
                        else
                        {
                            GenerateKeyword(TSqlTokenType.Off);
                        }

                        needComma = true;
                    }

                    if (node.RetentionPeriod != null)
                    {
                        if (needComma)
                        {
                            GenerateKeyword(TSqlTokenType.Comma);
                        }

                        GenerateSpaceAndIdentifier(CodeGenerationSupporter.HistoryRetentionPeriod);
                        GenerateKeyword(TSqlTokenType.EqualsSign);

                        if (node.RetentionPeriod.IsInfinity)
                        {
                            GenerateIdentifier(CodeGenerationSupporter.Infinite);
                        }
                        else
                        {
                            GenerateFragmentIfNotNull(node.RetentionPeriod.Duration);
                            GenerateSpace();
                            TemporalRetentionPeriodUnitHelper.Instance.GenerateSourceForOption(_writer, node.RetentionPeriod.Units);
                        }
                    }

                    GenerateKeyword(TSqlTokenType.RightParenthesis);
                }
            }
            else
            {
                GenerateSpaceAndKeyword(TSqlTokenType.Off);
            }
        }
    }
}
