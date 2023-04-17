//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.ServerAuditStatementOptions.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(QueueDelayAuditOption node)
        {
            System.Diagnostics.Debug.Assert(node.OptionKind == AuditOptionKind.QueueDelay);
            GenerateNameEqualsValue(CodeGenerationSupporter.QueueDelay, node.Delay);
        }

        public override void ExplicitVisit(AuditGuidAuditOption node)
        {
            System.Diagnostics.Debug.Assert(node.OptionKind == AuditOptionKind.AuditGuid);
            GenerateNameEqualsValue(CodeGenerationSupporter.AuditGuid, node.Guid);
        }

        public override void ExplicitVisit(OnFailureAuditOption node)
        {
            System.Diagnostics.Debug.Assert(node.OptionKind == AuditOptionKind.OnFailure);
            GenerateTokenAndEqualSign(CodeGenerationSupporter.OnFailure);
            switch (node.OnFailureAction)
            {
                case AuditFailureActionType.Shutdown:
                    GenerateSpaceAndKeyword(TSqlTokenType.Shutdown);
                    break;
                case AuditFailureActionType.Continue:
                    GenerateSpaceAndKeyword(TSqlTokenType.Continue);
                    break;
                case AuditFailureActionType.FailOperation:
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.FailOperation);
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false, "Unexpected option encountered");
                    break;
            }
        }

        public override void ExplicitVisit(StateAuditOption node)
        {
            System.Diagnostics.Debug.Assert(node.OptionKind == AuditOptionKind.State);
            GenerateOptionStateWithEqualSign(CodeGenerationSupporter.State, node.Value);
        }
    }
}