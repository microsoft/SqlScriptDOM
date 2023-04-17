//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.RestoreStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        private static Dictionary<RestoreStatementKind, TokenGenerator> _restoreStatementKindGenerators = new Dictionary<RestoreStatementKind, TokenGenerator>()
        {
            {RestoreStatementKind.None, new EmptyGenerator()},
            // exclude it: handled specially
            //{RestoreStatementKind.Database, new KeywordGenerator(TSqlTokenType.DATABASE)},
            {RestoreStatementKind.FileListOnly, new IdentifierGenerator(CodeGenerationSupporter.FileListOnly)},
            {RestoreStatementKind.HeaderOnly, new IdentifierGenerator(CodeGenerationSupporter.HeaderOnly)},
            {RestoreStatementKind.LabelOnly, new IdentifierGenerator(CodeGenerationSupporter.LabelOnly)},
            {RestoreStatementKind.RewindOnly, new IdentifierGenerator(CodeGenerationSupporter.RewindOnly)},
            // exclude it: handled specially
            //{RestoreStatementKind.TransactionLog, new IdentifierGenerator(CodeGenerationSupporter.Log) },
            {RestoreStatementKind.VerifyOnly, new IdentifierGenerator(CodeGenerationSupporter.VerifyOnly)},
        };
  
        public override void ExplicitVisit(RestoreStatement node)
        {
            GenerateKeyword(TSqlTokenType.Restore); 
            if (node.Kind == RestoreStatementKind.TransactionLog)
            {
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Log);
                GenerateSpaceAndFragmentIfNotNull(node.DatabaseName);
            }
            else if (node.Kind == RestoreStatementKind.Database)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.Database);
                GenerateSpaceAndFragmentIfNotNull(node.DatabaseName);
            }
            else
            {
                TokenGenerator generator = GetValueForEnumKey(_restoreStatementKindGenerators, node.Kind);
                if (generator != null)
                {
                    GenerateSpace();
                    GenerateToken(generator);
                }
            }

            if (node.Files.Count > 0)
            {
                GenerateSpace();
                GenerateCommaSeparatedList(node.Files);
            }

            if (node.Devices.Count > 0)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.From);
                GenerateSpace();
                GenerateCommaSeparatedList(node.Devices);
            }

            if (node.Options.Count > 0)
            {
                NewLineAndIndent();
                GenerateKeyword(TSqlTokenType.With);

                GenerateSpace();
                // could be
                //      MoveRestoreOption
                //      SimpleRestoreOption
                //      StopRestoreOption
                GenerateCommaSeparatedList(node.Options);
            }
        }
    }
}
