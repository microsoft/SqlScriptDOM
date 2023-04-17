//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterResourceGovernorStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CreateSpatialIndexStatement node)
        {
            AlignmentPoint ap = new AlignmentPoint();

            GenerateKeywordAndSpace(TSqlTokenType.Create);
            GenerateIdentifier(CodeGenerationSupporter.Spatial);
            GenerateSpaceAndKeyword(TSqlTokenType.Index);

            GenerateSpaceAndFragmentIfNotNull(node.Name);

            NewLineAndIndent();
            MarkAndPushAlignmentPoint(ap);
            GenerateKeyword(TSqlTokenType.On);
            GenerateSpaceAndFragmentIfNotNull(node.Object);

            GenerateSpaceAndKeyword(TSqlTokenType.LeftParenthesis);
            GenerateFragmentIfNotNull(node.SpatialColumnName);
            GenerateKeyword(TSqlTokenType.RightParenthesis);
            PopAlignmentPoint();

            if (node.SpatialIndexingScheme != SpatialIndexingSchemeType.None)
            {
                NewLineAndIndent();
                MarkAndPushAlignmentPoint(ap);
                GenerateIdentifier(CodeGenerationSupporter.Using);
                GenerateSpace();
                SpatialIndexingSchemeTypeHelper.Instance.GenerateSourceForOption(_writer, node.SpatialIndexingScheme);
                PopAlignmentPoint();
            }

            if ((node.SpatialIndexOptions != null) && (node.SpatialIndexOptions.Count > 0))
            {
                NewLineAndIndent();
                MarkAndPushAlignmentPoint(ap);
                GenerateKeyword(TSqlTokenType.With);
                GenerateSpace();
                GenerateAlignedParenthesizedOptionsWithMultipleIndent(node.SpatialIndexOptions, 2);
                PopAlignmentPoint();
                
            }

            if (node.OnFileGroup != null)
            {
                NewLineAndIndent();
                MarkAndPushAlignmentPoint(ap);
                GenerateKeyword(TSqlTokenType.On);
                GenerateSpaceAndFragmentIfNotNull(node.OnFileGroup);
                PopAlignmentPoint();
            }
        }

        public override void ExplicitVisit(SpatialIndexRegularOption node)
        {               
            GenerateFragmentIfNotNull(node.Option);
        }

        public override void ExplicitVisit(BoundingBoxSpatialIndexOption node)
        {
            GenerateIdentifier(CodeGenerationSupporter.BoundingBox);
            GenerateSpaceAndKeyword(TSqlTokenType.EqualsSign);
            GenerateSpace();
            if ((node.BoundingBoxParameters != null) && (node.BoundingBoxParameters.Count > 0))
                GenerateParenthesisedCommaSeparatedList(node.BoundingBoxParameters);                
        }

        public override void ExplicitVisit(BoundingBoxParameter node)
        {
            if (node.Parameter != BoundingBoxParameterType.None)
            {
                BoundingBoxParameterTypeHelper.Instance.GenerateSourceForOption(_writer, node.Parameter);
                GenerateSpaceAndKeyword(TSqlTokenType.EqualsSign);
                GenerateSpace();
            }
            GenerateFragmentIfNotNull(node.Value);
        }

        public override void ExplicitVisit(GridsSpatialIndexOption node)
        {
            GenerateIdentifier(CodeGenerationSupporter.Grids);
            GenerateSpaceAndKeyword(TSqlTokenType.EqualsSign);
            GenerateSpace();
            if ((node.GridParameters != null) && (node.GridParameters.Count > 0))
                GenerateParenthesisedCommaSeparatedList(node.GridParameters);                       
        }

        public override void ExplicitVisit(GridParameter node)
        {
            if (node.Parameter != GridParameterType.None)
            {
                GridParameterTypeHelper.Instance.GenerateSourceForOption(_writer, node.Parameter);
                GenerateSpaceAndKeyword(TSqlTokenType.EqualsSign);
                GenerateSpace();
            }
            ImportanceParameterHelper.Instance.GenerateSourceForOption(_writer, node.Value);
        }

        public override void ExplicitVisit(CellsPerObjectSpatialIndexOption node)
        {
            GenerateIdentifier(CodeGenerationSupporter.CellsPerObject);
            GenerateSpaceAndKeyword(TSqlTokenType.EqualsSign);
            GenerateSpaceAndFragmentIfNotNull(node.Value);
        }
    }
}
