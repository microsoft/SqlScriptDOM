//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AuditTarget.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AuditTarget node)
        {
            GenerateKeywordAndSpace(TSqlTokenType.To);
            switch (node.TargetKind)
            {
                case AuditTargetKind.ApplicationLog:
                    GenerateIdentifier(CodeGenerationSupporter.ApplicationLog);
                    break;
                case AuditTargetKind.SecurityLog:
                    GenerateIdentifier(CodeGenerationSupporter.SecurityLog);
                    break;
                case AuditTargetKind.ExternalMonitor:
                    GenerateIdentifier(CodeGenerationSupporter.ExternalMonitor);
                    break;
                case AuditTargetKind.File:
                    GenerateKeywordAndSpace(TSqlTokenType.File);
                    GenerateParenthesisedCommaSeparatedList(node.TargetOptions);
                    break;
                case AuditTargetKind.Url:
                    GenerateIdentifier(CodeGenerationSupporter.Url);
                    GenerateIdentifier(CodeGenerationSupporter.SingleSpace);
                    GenerateParenthesisedCommaSeparatedList(node.TargetOptions);
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false, "An unhandled option is encountered.");
                    break;
            }
        }

        public override void ExplicitVisit(MaxSizeAuditTargetOption node)
        {
            System.Diagnostics.Debug.Assert(node.OptionKind == AuditTargetOptionKind.MaxSize, "unexpected option encountered");
            GenerateTokenAndEqualSign(CodeGenerationSupporter.MaxSize);
            if (node.IsUnlimited == true)
            {
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Unlimited);
            }
            else
            {
                GenerateSpaceAndFragmentIfNotNull(node.Size);
                GenerateSpace();
                MemoryUnitsHelper.Instance.GenerateSourceForOption(_writer, node.Unit);
            }
        }

        public override void ExplicitVisit(RetentionDaysAuditTargetOption node)
        {
            System.Diagnostics.Debug.Assert(node.OptionKind == AuditTargetOptionKind.RetentionDays, "unexpected option encountered");
            GenerateTokenAndEqualSign(CodeGenerationSupporter.RetentionDays);
            GenerateSpaceAndFragmentIfNotNull(node.Days);
        }

        public override void ExplicitVisit(MaxRolloverFilesAuditTargetOption node)
        {
            System.Diagnostics.Debug.Assert(node.OptionKind == AuditTargetOptionKind.MaxRolloverFiles, "unexpected option encountered");
            GenerateTokenAndEqualSign(CodeGenerationSupporter.MaxRolloverFiles);
            if (node.IsUnlimited == true)
            {
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Unlimited);
            }
            else
            {
                GenerateSpaceAndFragmentIfNotNull(node.Value);
            }
        }

        public override void ExplicitVisit(LiteralAuditTargetOption node)
        {
            switch (node.OptionKind)
            {
                case AuditTargetOptionKind.MaxFiles:
                    GenerateNameEqualsValue(CodeGenerationSupporter.MaxFiles, node.Value);
                    break;
                case AuditTargetOptionKind.FilePath:
                    GenerateNameEqualsValue(CodeGenerationSupporter.FilePath, node.Value);
                    break;
                case AuditTargetOptionKind.Path:
                    GenerateNameEqualsValue(CodeGenerationSupporter.Path, node.Value);
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false, "unexpected option encountered");
                    break;
            }
        }

        public override void ExplicitVisit(OnOffAuditTargetOption node)
        {
            System.Diagnostics.Debug.Assert(node.OptionKind==AuditTargetOptionKind.ReserveDiskSpace, "unexpected option encountered");
            GenerateOptionStateWithEqualSign(CodeGenerationSupporter.ReserveDiskSpace, node.Value);
        }

        public override void ExplicitVisit(OperatorAuditOption node)
        {
            System.Diagnostics.Debug.Assert(node.OptionKind == AuditOptionKind.OperatorAudit, "unexpected option encountered");
            GenerateOptionStateWithEqualSign(CodeGenerationSupporter.OperatorAudit, node.Value);
        }
    }
}
