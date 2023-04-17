//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AvailabilityReplica.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Diagnostics;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AvailabilityReplica node)
        {
            GenerateFragmentIfNotNull(node.ServerName);
            if (node.Options != null && node.Options.Count > 0)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.With);
                GenerateSpace();
                GenerateParenthesisedCommaSeparatedList(node.Options);
            }
        }

        public override void ExplicitVisit(AvailabilityModeReplicaOption node)
        {
            Debug.Assert(node.OptionKind == AvailabilityReplicaOptionKind.AvailabilityMode);
            string availabilityMode = node.Value == AvailabilityModeOptionKind.AsynchronousCommit ? CodeGenerationSupporter.AsynchronousCommit : CodeGenerationSupporter.SynchronousCommit;
            GenerateNameEqualsValue(CodeGenerationSupporter.AvailabilityMode, availabilityMode);
        }

        public override void ExplicitVisit(FailoverModeReplicaOption node)
        {
            Debug.Assert(node.OptionKind == AvailabilityReplicaOptionKind.FailoverMode);
            string failoverMode = node.Value == FailoverModeOptionKind.Automatic ? CodeGenerationSupporter.Automatic : CodeGenerationSupporter.Manual;
            GenerateNameEqualsValue(CodeGenerationSupporter.FailoverMode, failoverMode);
        }

        public override void ExplicitVisit(LiteralReplicaOption node)
        {
            AvailabilityReplicaOptionsHelper.Instance.GenerateSourceForOption(_writer, node.OptionKind);
            GenerateSpaceAndKeyword(TSqlTokenType.EqualsSign);
            GenerateSpaceAndFragmentIfNotNull(node.Value);
        }

        public override void ExplicitVisit(SecondaryRoleReplicaOption node)
        {
            Debug.Assert(node.OptionKind == AvailabilityReplicaOptionKind.SecondaryRole);
            GenerateIdentifier(CodeGenerationSupporter.SecondaryRole);
            GenerateSpaceAndKeyword(TSqlTokenType.LeftParenthesis);
            GenerateIdentifier(CodeGenerationSupporter.AllowConnections);
            GenerateSpaceAndKeyword(TSqlTokenType.EqualsSign);
            GenerateSpace();
            AllowConnectionsOptionsHelper.Instance.GenerateSourceForOption(_writer, node.AllowConnections);
            GenerateKeyword(TSqlTokenType.RightParenthesis);
        }

        public override void ExplicitVisit(PrimaryRoleReplicaOption node)
        {
            Debug.Assert(node.OptionKind == AvailabilityReplicaOptionKind.PrimaryRole);
            GenerateIdentifier(CodeGenerationSupporter.PrimaryRole);
            GenerateSpaceAndKeyword(TSqlTokenType.LeftParenthesis);
            GenerateIdentifier(CodeGenerationSupporter.AllowConnections);
            GenerateSpaceAndKeyword(TSqlTokenType.EqualsSign);
            GenerateSpace();
            AllowConnectionsOptionsHelper.Instance.GenerateSourceForOption(_writer, node.AllowConnections);
            GenerateKeyword(TSqlTokenType.RightParenthesis);
        }
    }
}
