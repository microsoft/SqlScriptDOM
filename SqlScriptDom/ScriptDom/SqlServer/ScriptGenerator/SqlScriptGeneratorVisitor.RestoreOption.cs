//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.SimpleRestoreOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Collections.Generic;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {

        protected static Dictionary<RestoreOptionKind, TokenGenerator> _restoreOptionKindGenerators = new Dictionary<RestoreOptionKind, TokenGenerator>()
        {
            { RestoreOptionKind.BlockSize, new IdentifierGenerator(CodeGenerationSupporter.BlockSize) },
            { RestoreOptionKind.BufferCount, new IdentifierGenerator(CodeGenerationSupporter.BufferCount) },
            { RestoreOptionKind.Checksum, new IdentifierGenerator(CodeGenerationSupporter.Checksum) },
            { RestoreOptionKind.CommitDifferentialBase, new IdentifierGenerator(CodeGenerationSupporter.CommitDifferentialBase) },
            { RestoreOptionKind.ContinueAfterError, new IdentifierGenerator(CodeGenerationSupporter.ContinueAfterError) },
            { RestoreOptionKind.DboOnly, new IdentifierGenerator(CodeGenerationSupporter.DboOnly) },
            { RestoreOptionKind.EnableBroker, new IdentifierGenerator(CodeGenerationSupporter.EnableBroker) },
            { RestoreOptionKind.EnhancedIntegrity, new IdentifierGenerator(CodeGenerationSupporter.EnhancedIntegrity) },
            { RestoreOptionKind.ErrorBrokerConversations, new IdentifierGenerator(CodeGenerationSupporter.ErrorBrokerConversations) },
            { RestoreOptionKind.File, new KeywordGenerator(TSqlTokenType.File) },
            { RestoreOptionKind.KeepReplication, new IdentifierGenerator(CodeGenerationSupporter.KeepReplication) },
            { RestoreOptionKind.KeepTemporalRetention, new IdentifierGenerator(CodeGenerationSupporter.KeepTemporalRetention) },
            { RestoreOptionKind.LoadHistory, new IdentifierGenerator(CodeGenerationSupporter.LoadHistory) },
            { RestoreOptionKind.MaxTransferSize, new IdentifierGenerator(CodeGenerationSupporter.MaxTransferSize) },
            { RestoreOptionKind.MediaName, new IdentifierGenerator(CodeGenerationSupporter.MediaName) },
            { RestoreOptionKind.MediaPassword, new IdentifierGenerator(CodeGenerationSupporter.MediaPassword) },
            { RestoreOptionKind.NewBroker, new IdentifierGenerator(CodeGenerationSupporter.NewBroker) },
            { RestoreOptionKind.NoChecksum, new IdentifierGenerator(CodeGenerationSupporter.NoChecksum) },
            { RestoreOptionKind.NoLog, new IdentifierGenerator(CodeGenerationSupporter.NoLog) },
            { RestoreOptionKind.NoRecovery, new IdentifierGenerator(CodeGenerationSupporter.NoRecovery) },
            { RestoreOptionKind.NoRewind, new IdentifierGenerator(CodeGenerationSupporter.NoRewind) },
            { RestoreOptionKind.NoUnload, new IdentifierGenerator(CodeGenerationSupporter.NoUnload) },
            { RestoreOptionKind.Online, new IdentifierGenerator(CodeGenerationSupporter.Online) },
            { RestoreOptionKind.Partial, new IdentifierGenerator(CodeGenerationSupporter.Partial) },
            { RestoreOptionKind.Password, new IdentifierGenerator(CodeGenerationSupporter.Password) },
            { RestoreOptionKind.Recovery, new IdentifierGenerator(CodeGenerationSupporter.Recovery) },
            { RestoreOptionKind.Replace, new IdentifierGenerator(CodeGenerationSupporter.Replace) },
            { RestoreOptionKind.Restart, new IdentifierGenerator(CodeGenerationSupporter.Restart) },
            { RestoreOptionKind.RestrictedUser, new IdentifierGenerator(CodeGenerationSupporter.RestrictedUser) },
            { RestoreOptionKind.Rewind, new IdentifierGenerator(CodeGenerationSupporter.Rewind) },
            { RestoreOptionKind.Snapshot, new IdentifierGenerator(CodeGenerationSupporter.Snapshot) },
            { RestoreOptionKind.SnapshotImport, new IdentifierGenerator(CodeGenerationSupporter.SnapshotImport) },
            { RestoreOptionKind.SnapshotRestorePhase, new IdentifierGenerator(CodeGenerationSupporter.SnapshotRestorePhase) },
            { RestoreOptionKind.Standby, new IdentifierGenerator(CodeGenerationSupporter.Standby) },
            { RestoreOptionKind.Stats, new IdentifierGenerator(CodeGenerationSupporter.Stats) },
            { RestoreOptionKind.StopAt, new IdentifierGenerator(CodeGenerationSupporter.StopAt) },
            { RestoreOptionKind.StopOnError, new IdentifierGenerator(CodeGenerationSupporter.StopOnError) },
            { RestoreOptionKind.Unload, new IdentifierGenerator(CodeGenerationSupporter.Unload) },
            { RestoreOptionKind.Verbose, new IdentifierGenerator(CodeGenerationSupporter.Verbose) },
        };

        public override void ExplicitVisit(RestoreOption node)
        {
            TokenGenerator generator = GetValueForEnumKey(_restoreOptionKindGenerators, node.OptionKind);
            if (generator == null)
            {
                return;
            }
            GenerateToken(generator);
        }

        public override void ExplicitVisit(ScalarExpressionRestoreOption node)
        {
            TokenGenerator generator = GetValueForEnumKey(_restoreOptionKindGenerators, node.OptionKind);
            if (generator == null)
            {
                return;
            }
            if (node.Value != null)
            {
                GenerateNameEqualsValue(generator, node.Value);
            }
        }
    }
}
