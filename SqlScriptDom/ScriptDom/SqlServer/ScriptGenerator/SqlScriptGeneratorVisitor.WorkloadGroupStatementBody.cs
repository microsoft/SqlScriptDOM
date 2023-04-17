//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.WorkloadGroupStatementBody.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;


namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        protected void GenerateWorkloadGroupStatementBody(WorkloadGroupStatement node)
        {
            AlignmentPoint ap = new AlignmentPoint();

            GenerateIdentifier(CodeGenerationSupporter.Workload);
            GenerateSpaceAndKeyword(TSqlTokenType.Group);

            // name
            GenerateSpaceAndFragmentIfNotNull(node.Name);

            if ((node.WorkloadGroupParameters != null) && (node.WorkloadGroupParameters.Count > 0))
            {
                NewLineAndIndent();
                MarkAndPushAlignmentPoint(ap);
                GenerateKeyword(TSqlTokenType.With);

                GenerateSpace();

                GenerateAlignedParenthesizedOptionsWithMultipleIndent(node.WorkloadGroupParameters, 2);
                               
                PopAlignmentPoint();
            }

            if (node.PoolName != null || node.ExternalPoolName != null)
            {
                NewLineAndIndent();
                MarkAndPushAlignmentPoint(ap);
                GenerateIdentifier(CodeGenerationSupporter.Using);

                if (node.PoolName != null)
                {
                    GenerateSpaceAndFragmentIfNotNull(node.PoolName);

                    if (node.ExternalPoolName != null)
                    {
                        GenerateKeyword(TSqlTokenType.Comma);
                    }
                }

                if (node.ExternalPoolName != null)
                {
                    GenerateSpaceAndKeyword(TSqlTokenType.External);
                    GenerateSpaceAndFragmentIfNotNull(node.ExternalPoolName);
                }

                PopAlignmentPoint();
            }
        }

        public override void ExplicitVisit(WorkloadGroupResourceParameter node)
        {
            WorkloadGroupResourceParameterHelper.Instance.GenerateSourceForOption(_writer, node.ParameterType);
            GenerateSpaceAndKeyword(TSqlTokenType.EqualsSign);
            GenerateSpaceAndFragmentIfNotNull(node.ParameterValue);
        }

        public override void ExplicitVisit(WorkloadGroupImportanceParameter node)
        {
            GenerateIdentifier(CodeGenerationSupporter.Importance);
            GenerateSpaceAndKeyword(TSqlTokenType.EqualsSign);
            GenerateSpace();
            ImportanceParameterHelper.Instance.GenerateSourceForOption(_writer, node.ParameterValue);
        }
    }
}
