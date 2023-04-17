//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterAvailabilityGroupStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Diagnostics;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AlterAvailabilityGroupStatement node)
        {
            GenerateKeywordAndSpace(TSqlTokenType.Alter);
            GenerateIdentifier(CodeGenerationSupporter.Availability);
            GenerateSpaceAndKeyword(TSqlTokenType.Group);
            
            GenerateSpaceAndFragmentIfNotNull(node.Name);

            switch (node.AlterAvailabilityGroupStatementType)
            {
                case AlterAvailabilityGroupStatementType.Action:
                    GenerateSpaceAndFragmentIfNotNull(node.Action);
                    break;
                case AlterAvailabilityGroupStatementType.AddDatabase:
                    GenerateSpaceAndKeyword(TSqlTokenType.Add);
                    GenerateSpaceAndKeyword(TSqlTokenType.Database);
                    GenerateSpace();
                    GenerateCommaSeparatedList(node.Databases);
                    break;
                case AlterAvailabilityGroupStatementType.AddReplica:
                    GenerateSpaceAndKeyword(TSqlTokenType.Add);
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Replica);
                    GenerateSpaceAndKeyword(TSqlTokenType.On);
                    GenerateSpace();
                    GenerateCommaSeparatedList(node.Replicas);
                    break;
                case AlterAvailabilityGroupStatementType.ModifyReplica:
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Modify);
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Replica);
                    GenerateSpaceAndKeyword(TSqlTokenType.On);
                    GenerateSpace();
                    GenerateCommaSeparatedList(node.Replicas);
                    break;
                case AlterAvailabilityGroupStatementType.RemoveDatabase:
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Remove);
                    GenerateSpaceAndKeyword(TSqlTokenType.Database);
                    GenerateSpace();
                    GenerateCommaSeparatedList(node.Databases);
                    break;
                case AlterAvailabilityGroupStatementType.RemoveReplica:
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Remove);
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Replica);
                    GenerateSpaceAndKeyword(TSqlTokenType.On);
                    GenerateSpace();
                    GenerateCommaSeparatedList(node.Replicas);
                    break;
                case AlterAvailabilityGroupStatementType.Set:
                    GenerateSpaceAndKeyword(TSqlTokenType.Set);
                    GenerateSpace();
                    GenerateParenthesisedCommaSeparatedList(node.Options);
                    break;
                default:
                    Debug.Assert(false, "Unexepected option encountered");
                    break;
            }
        }

        public override void ExplicitVisit(AlterAvailabilityGroupAction node)
        {
            AlterAvailabilityGroupActionTypeHelper.Instance.GenerateSourceForOption(_writer, node.ActionType);
        }

        public override void ExplicitVisit(AlterAvailabilityGroupFailoverAction node)
        {
            GenerateIdentifier(CodeGenerationSupporter.Failover);
            if (node.Options != null && node.Options.Count > 0)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.With);
                GenerateSpace();
                GenerateParenthesisedCommaSeparatedList(node.Options);
            }
        }

        public override void ExplicitVisit(AlterAvailabilityGroupFailoverOption node)
        {
            GenerateNameEqualsValue(CodeGenerationSupporter.Target, node.Value);
        }
    }
}
