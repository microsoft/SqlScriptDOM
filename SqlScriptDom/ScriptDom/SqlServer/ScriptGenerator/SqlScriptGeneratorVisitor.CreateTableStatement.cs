//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CreateTableStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CreateTableStatement node)
        {
            AlignmentPoint start = new AlignmentPoint();
            MarkAndPushAlignmentPoint(start);

            GenerateKeyword(TSqlTokenType.Create);
            GenerateSpaceAndKeyword(TSqlTokenType.Table);

            GenerateSpaceAndFragmentIfNotNull(node.SchemaObjectName);

            if (node.Definition != null)
            {
                List<TSqlFragment> columnsAndConstraintsAndIndexesAndPeriods = new List<TSqlFragment>();
                foreach (ColumnDefinition col in node.Definition.ColumnDefinitions)
                {
                    columnsAndConstraintsAndIndexesAndPeriods.Add(col);
                }
                foreach (ConstraintDefinition constraint in node.Definition.TableConstraints)
                {
                    columnsAndConstraintsAndIndexesAndPeriods.Add(constraint);
                }
                foreach (IndexDefinition index in node.Definition.Indexes)
                {
                    columnsAndConstraintsAndIndexesAndPeriods.Add(index);
                }

                ListGenerationOption option = ListGenerationOption.CreateOptionFromFormattingConfig(_options);

                if (node.Definition.SystemTimePeriod != null)
                {
                    columnsAndConstraintsAndIndexesAndPeriods.Add(node.Definition.SystemTimePeriod);
                }

                GenerateFragmentList(columnsAndConstraintsAndIndexesAndPeriods, option);
            }
            else if (node.CtasColumns.Count > 0)
            {
                // CTAS statements do not have column definitions, but rather support a column name list
                if (_options.MultilineViewColumnsList)
                {
                    ListGenerationOption option = ListGenerationOption.CreateOptionFromFormattingConfig(_options);
                    GenerateFragmentList(node.CtasColumns, option);
                }
                else
                {
                    GenerateSpace();
                    GenerateParenthesisedCommaSeparatedList(node.CtasColumns);
                }
            }
            if (node.AsEdge)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.As);
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.GraphEdge);
            }
            if (node.AsFileTable)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.As);
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.FileTable);
            }
            if (node.AsNode)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.As);
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.GraphNode);
            }

            PopAlignmentPoint();

            if (node.FederationScheme != null)
            {
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Federated);
                GenerateSpaceAndKeyword(TSqlTokenType.On);
                GenerateSpaceAndSymbol(TSqlTokenType.LeftParenthesis);
                GenerateFragmentIfNotNull(node.FederationScheme.DistributionName);
                GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
                GenerateSpaceAndFragmentIfNotNull(node.FederationScheme.ColumnName);
                GenerateSymbol(TSqlTokenType.RightParenthesis);
            }

            if (node.OnFileGroupOrPartitionScheme != null)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.On); 
                GenerateSpaceAndFragmentIfNotNull(node.OnFileGroupOrPartitionScheme);
            }

            if (node.TextImageOn != null)
            {
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.TextImageOn);
                GenerateSpaceAndFragmentIfNotNull(node.TextImageOn);
            }

            GenerateFileStreamOn(node);

            GenerateCommaSeparatedWithClause(node.Options, false, true);

            if(node.SelectStatement != null)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.As);
                GenerateNewLineOrSpace(true);
                GenerateFragmentIfNotNull(node.SelectStatement);
            }
        }
    }
}
