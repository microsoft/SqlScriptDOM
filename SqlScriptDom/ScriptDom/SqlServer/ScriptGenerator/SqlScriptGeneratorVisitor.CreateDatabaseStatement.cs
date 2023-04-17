//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CreateDatabaseStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Linq;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        private static HashSet<DatabaseOptionKind> azureOnlyOptions = new HashSet<DatabaseOptionKind>()
        {
            DatabaseOptionKind.MaxSize,
            DatabaseOptionKind.Edition,
            DatabaseOptionKind.ServiceObjective
        };

        private static Dictionary<AttachMode, TokenGenerator> _attachModeGenerators = new Dictionary<AttachMode, TokenGenerator>()
        {
            {AttachMode.Attach, new IdentifierGenerator(CodeGenerationSupporter.Attach)},
            {AttachMode.AttachForceRebuildLog, new IdentifierGenerator(CodeGenerationSupporter.AttachForceRebuildLog)},
            {AttachMode.AttachRebuildLog, new IdentifierGenerator(CodeGenerationSupporter.AttachRebuildLog)},
            {AttachMode.Load, new KeywordGenerator(TSqlTokenType.Load)},
        };

        public override void ExplicitVisit(CreateDatabaseStatement node)
        {
            GenerateKeyword(TSqlTokenType.Create);
            GenerateSpaceAndKeyword(TSqlTokenType.Database);

            GenerateSpaceAndFragmentIfNotNull(node.DatabaseName);

            GenerateSpaceAndFragmentIfNotNull(node.Containment);

            // filegroups
            if (node.FileGroups != null && node.FileGroups.Count > 0)
            {
                NewLineAndIndent();
                GenerateKeyword(TSqlTokenType.On);
                GenerateSpace();

                GenerateCommaSeparatedList(node.FileGroups);
            }

            // LOG ON files
            if (node.LogOn.Count > 0)
            {
                NewLineAndIndent();
                GenerateIdentifier(CodeGenerationSupporter.Log);
                GenerateSpaceAndKeyword(TSqlTokenType.On);
                GenerateSpace();
                GenerateCommaSeparatedList(node.LogOn);
            }

            // COLLATE
            GenerateSpaceAndCollation(node.Collation);

            // attach mode
            if (node.AttachMode != AttachMode.None)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.For);

                TokenGenerator generator = GetValueForEnumKey(_attachModeGenerators, node.AttachMode);
                if (generator != null)
                {
                    GenerateSpace();
                    GenerateToken(generator);
                }
            }

            // snapshot
            if (node.DatabaseSnapshot != null)
            {
                NewLineAndIndent();
                GenerateKeyword(TSqlTokenType.As);
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Snapshot);
                GenerateSpaceAndKeyword(TSqlTokenType.Of);

                GenerateSpaceAndFragmentIfNotNull(node.DatabaseSnapshot);
            }

            if (node.CopyOf != null)
            {
                NewLineAndIndent();
                GenerateKeyword(TSqlTokenType.As);
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Copy);
                GenerateSpaceAndKeyword(TSqlTokenType.Of);

                GenerateSpaceAndFragmentIfNotNull(node.CopyOf);
            }

            if (node.Options != null && node.Options.Count > 0)
            {
                IList<DatabaseOption> azureOnlyOptionsPresent = node.Options.Where(o => azureOnlyOptions.Contains(o.OptionKind)).ToList();
                IList<DatabaseOption> sharedOptionsPresent = node.Options.Where(o => !azureOnlyOptions.Contains(o.OptionKind)).ToList();

                if (azureOnlyOptionsPresent.Count > 0)
                {
                    NewLineAndIndent();
                    GenerateParenthesisedCommaSeparatedList(azureOnlyOptionsPresent, true);
                }

                if (sharedOptionsPresent.Count > 0)
                {
                    GenerateCommaSeparatedWithClause(sharedOptionsPresent, true, false);
                }
            }
        }
    }
}