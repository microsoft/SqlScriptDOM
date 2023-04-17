//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.WorkloadClassifierStatementBody.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;


namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        protected void GenerateWorkloadClassifierStatementBody(WorkloadClassifierStatement node)
        {
            AlignmentPoint ap = new AlignmentPoint();


            GenerateIdentifier(CodeGenerationSupporter.Workload);
            GenerateSpace();
            GenerateIdentifier(CodeGenerationSupporter.Classifier);

            // name
            GenerateSpaceAndFragmentIfNotNull(node.ClassifierName);

            if (node.Options.Count > 0) 
            {
                NewLineAndIndent();
                MarkAndPushAlignmentPoint(ap);

                GenerateKeyword(TSqlTokenType.With);
                GenerateSpace();

                GenerateAlignedParenthesizedOptionsWithMultipleIndent(node.Options, 2);
                PopAlignmentPoint();

            }
        }

        public override void ExplicitVisit(ClassifierWorkloadGroupOption node)
        {
            if (node.WorkloadGroupName != null)
            {
                GenerateNameEqualsValue(CodeGenerationSupporter.WorkloadGroup, node.WorkloadGroupName);
            }
        }

        public override void ExplicitVisit(ClassifierMemberNameOption node)
        {
            if (node.MemberName != null)
            {
                GenerateNameEqualsValue(CodeGenerationSupporter.MemberName, node.MemberName);
            }
        }

        public override void ExplicitVisit(ClassifierWlmLabelOption node)
        {
            if (node.WlmLabel != null)
            {
                GenerateNameEqualsValue(CodeGenerationSupporter.WlmLabel, node.WlmLabel);
            }
        }

        public override void ExplicitVisit(ClassifierWlmContextOption node)
        {
            if (node.WlmContext != null)
            {
                GenerateNameEqualsValue(CodeGenerationSupporter.WlmContext, node.WlmContext);
            }
        }

        public override void ExplicitVisit(ClassifierStartTimeOption node)
        {
            if (node.Time != null)
            {
                GenerateNameEqualsValue(CodeGenerationSupporter.StartTime, node.Time);
            }
        }

        public override void ExplicitVisit(ClassifierEndTimeOption node)
        {
            if (node.Time != null)
            {
                GenerateNameEqualsValue(CodeGenerationSupporter.EndTime, node.Time);
            }
        }

        public override void ExplicitVisit(ClassifierImportanceOption node)
        {
            GenerateIdentifier(CodeGenerationSupporter.Importance);
            GenerateSpaceAndKeyword(TSqlTokenType.EqualsSign);
            GenerateSpace();
            ImportanceParameterHelper.Instance.GenerateSourceForOption(_writer, node.Importance);
        }
    }
}
